#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.Form/ThemaConfigurationProvider.cs
#endregion
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Qorpent.Applications;
using Qorpent.IO;
using Qorpent.IoC;
using Zeta.Extreme.BizProcess.Themas;

using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	Провайдер конфигураций
	/// </summary>
	public class ThemaConfigurationProvider : IThemaConfigurationProvider {
		/// <summary>
		/// 	Создает стандартный конфигуратор
		/// </summary>
		public ThemaConfigurationProvider(ThemaLoaderOptions options = null) {
			Options = options ?? new ThemaLoaderOptions();
		}


		/// <summary>
		/// 	Обратная ссылка на контейнер
		/// </summary>
		public IContainer Container {
			get { return _container ?? (_container = Application.Current.Container); }
			set { _container = value; }
		}

		/// <summary>
		/// 	Резольвер файлов
		/// </summary>
		public IFileService PathResolver {
			get {
				lock (this) {
					if (null == _pathResolver) {
						_pathResolver = Application.Current.Files;
					}
					return _pathResolver;
				}
			}
			set { _pathResolver = value; }
		}

		/// <summary>
		/// 	Прямой XML
		/// </summary>
		public string DirectXml { get; set; }

		/// <summary>
		/// 	Загружаемые фильтры компиляции
		/// </summary>
		public string LoadCompileFilters { get; set; }

		/// <summary>
		/// 	Опции зашгрузки темы
		/// </summary>
		public ThemaLoaderOptions Options { get; set; }


		/// <summary>
		/// 	Установить значение параметра для конкретной темы
		/// </summary>
		/// <param name="themacode"> </param>
		/// <param name="parameter"> </param>
		/// <param name="value"> </param>
		public void Set(string themacode, string parameter, object value) {
			lock (this) {
				value = value ?? "";
				var type = ReflectionExtensions.ResolveWellKnownName(value.GetType());
				var overfile = PathResolver.Resolve("data/override.xml");
				if (null == overfile) {
					overfile = PathResolver.Resolve("~/usr/data/override.xml", false);
					PathResolver.Write(overfile, "<root/>");
				}
				var content = XElement.Parse(PathResolver.Read<string>(overfile));
				var themaelement = content.XPathSelectElement("./thema[@id='" + themacode + "']");
				if (themaelement == null) {
					themaelement = new XElement("thema", new XAttribute("id", themacode));
					content.Add(themaelement);
				}
				var param = themaelement.XPathSelectElement("./param[@id='" + parameter + "']");
				if (param == null) {
					param = new XElement("param", new XAttribute("id", parameter));
					themaelement.Add(param);
				}
				param.SetAttributeValue("type", type);
				param.SetValue(value);
				content.Save(overfile);
			}
		}


		/// <summary>
		/// 	Получить конфигурацию
		/// </summary>
		/// <returns> </returns>
		public IThemaFactoryConfiguration Get() {
			_cfgVersion = DateTime.Now;
			var compiledxml = LoadCompiled().ToArray();


			IDictionary<string, ThemaConfiguration> configurations2 = new Dictionary<string, ThemaConfiguration>();
			prepareEmpty(configurations2, compiledxml);
			readParameters(configurations2, compiledxml);
			readAll(configurations2, compiledxml);
			applyRenames(configurations2);
			var result = new ThemaFactoryConfiguration();
			foreach (var themaConfiguration in configurations2.Values) {
				result.Configurations.Add(themaConfiguration);
			}
			result.SrcXml = compiledxml;

			result.Version = _cfgVersion;

			return result;
		}


		/// <summary>
		/// 	Загружает откомпилированные темы
		/// </summary>
		/// <returns> </returns>
		public IEnumerable<XElement> LoadCompiled() {
			var filters = LoadCompileFilters.SmartSplit();
			_cfgVersion = new DateTime();
			string[] files = null;
			if (!Options.RootDirectory.Contains("~") && Path.IsPathRooted(Options.RootDirectory)) {
				files = Directory.GetFiles(Options.RootDirectory, "*.xml");
			}
			else {
				files =
					Application.Current.Files.ResolveAll(new FileSearchQuery
						{
							ExistedOnly = true,
							ProbeFiles = new[] {"*.xml"},
							ProbePaths = new[] {Options.RootDirectory},
							All = true,
							PathType = FileSearchResultType.FullPath
						}).ToArray();
			}
			files = files.OrderBy(x => Path.GetFileNameWithoutExtension(x)).ToArray();
			foreach (var f in files) {
				if (File.GetLastWriteTime(f) > _cfgVersion) {
					_cfgVersion = File.GetLastWriteTime(f);
				}
				var txt = File.ReadAllText(f);

				if (!string.IsNullOrWhiteSpace(Options.FilterParameters)) {
					if (Options.LoadLibraries && txt.Substring(0, 30).Contains("lib\"")) {
						yield return XElement.Load(f);
						continue;
					}

					foreach (var flag in Options.FilterParameters.Split(',')) {
						if (!txt.Contains("<param id=\"" + flag + "\"")) {
							continue;
						}
						var x = XElement.Load(f);
						var e = x.Elements("param").FirstOrDefault(_ => XmlExtensions.Attr(_, "id") == flag);
						if (null == e) {
							continue;
						}
						var val = e.Value;
						if (string.IsNullOrWhiteSpace(val)) {
							val = XmlExtensions.Attr(e, "value");
						}
						if (ConvertExtensions.ToBool(val)) {
							yield return x;
							break;
						}
					}

					continue;
				}
				if (filters.Count == 0) {
					yield return XElement.Load(f);
				}
				else {
					if (null != filters.FirstOrDefault(_ => f.Like(_))) {
						yield return XElement.Load(f);
					}
				}
			}
		}


		private void readParameters(IDictionary<string, ThemaConfiguration> configurations2, XElement[] compiledxml) {
			foreach (var cfg in configurations2.Values) {
				var parameters = cfg.SrcXml.Elements("param");
				var idx = 0;
				foreach (var p in parameters) {
					var id = p.Attr("id");
					var val = p.Attr("value");
					if (val.IsEmpty()) {
						val = p.Value;
					}
					idx += 10;
					cfg.Parameters[id] = new TypedParameter {Type = typeof (string), Name = id, Value = val, Idx = idx};
				}
			}
		}


		private void applyRenames(IDictionary<string, ThemaConfiguration> configurations) {
			foreach (var configuration in configurations.Values) {
				foreach (var rreport in configuration.SrcXml.XPathSelectElements("./renamereport")) {
					var code = configuration.Code + rreport.Attr("code") + ".out";
					var newv = configuration.Code + rreport.Attr("name") + ".out";
					var r = configuration.Outputs.SafeGet(code);
					if (null != r) {
						r.Code = newv;
						configuration.Outputs.Remove(code);
						configuration.Outputs[newv] = r;
					}
				}
				foreach (var rform in configuration.SrcXml.XPathSelectElements("./renameform")) {
					var code = configuration.Code + rform.Attr("code") + ".in";
					var newv = configuration.Code + rform.Attr("name") + ".in";
					var r = configuration.Inputs.SafeGet(code);
					if (null != r) {
						r.Code = newv;
						configuration.Inputs.Remove(code);
						configuration.Inputs[newv] = r;
					}
				}
			}
		}


		private void readAll(IDictionary<string, ThemaConfiguration> configurations, XElement[] descriptor) {
			foreach (var configuration in configurations.Values) {
				configuration.Name = configuration.SrcXml.Attr("name", configuration.Code);
				applyPseudoProperties(configuration);
				if (Options.ElementTypes.HasFlag(ElementType.Command)) {
					readCommands(configuration);
				}
				if (Options.ElementTypes.HasFlag(ElementType.Document)) {
					readDocuments(configuration);
				}
				if (Options.ElementTypes.HasFlag(ElementType.Form)) {
					readInputs(configuration);
				}
				if (Options.ElementTypes.HasFlag(ElementType.Report)) {
					readOutputs(configuration);
				}
			}
		}

		private void applyPseudoProperties(ThemaConfiguration configuration) {
			foreach (var parameter in configuration.Parameters) {
				if (parameter.Key.StartsWith(".")) {
					var name = parameter.Key.Substring(1);
					configuration.SetValue(name, parameter.Value.GetValue());
				}
			}
		}

		private void readCommands(ThemaConfiguration configuration) {
			foreach (var element in configuration.SrcXml.Elements("command")) {
				var cmd = new CommandConfiguration();
				cmd.Thema = configuration;
				cmd.Code = configuration.Code + element.Attr("id", "") + ".cmd";
				cmd.Name = element.Attr("name", cmd.Code);
				cmd.Url = element.Attr("url", "");
				cmd.Role = element.Attr("role", configuration.DefaultElementRole);
				element.Apply(cmd, "id", "name", "code", "role", "url");
				if (!cmd.Active) {
					continue;
				}
				configuration.Commands[cmd.Code] = cmd;
			}
		}

		private void readDocuments(ThemaConfiguration configuration) {
			foreach (var element in configuration.SrcXml.Elements("doc")) {
				var doc = new DocumentConfiguration();
				doc.Thema = configuration;
				doc.Code = configuration.Code + element.IdCodeOrValue() + ".doc";
				doc.Name = element.Attr("name", doc.Code);
				doc.Url = element.Attr("url", "");
				doc.Role = element.Attr("role", configuration.DefaultElementRole);
				doc.Type = element.Attr("type", "link");
				element.Apply(doc, "id", "name", "code", "role", "type");
				if (!doc.Active) {
					continue;
				}
				configuration.Documents[doc.Code] = doc;
			}
		}

		private void readInputs(ThemaConfiguration configuration) {
			var groupcodes = new[] {"A", "B", "C"};
			var inputs = configuration.SrcXml.Elements("in").Union(configuration.SrcXml.Elements("form")).ToArray();
			foreach (var element in inputs) {
				var id = element.IdCodeOrValue();
				if (groupcodes.Any(_=>_==id)) {
					if (configuration.Parameters.ContainsKey("active" + id)) {
						var active = configuration.ResolveParameter("active" + id).Value.ToBool();
						if (!active) {
							element.Remove();
							continue;
						}
					}
				}
				var input = new InputConfiguration();
				foreach (var parameter in configuration.Parameters) {
					if (parameter.Key.StartsWith("input.")) {
						var name = parameter.Key.Substring(6);
						input.SetValue(name, parameter.Value.GetValue());
					}
				}

				input.Thema = configuration;
				input.Code = configuration.Code + element.IdCodeOrValue() + ".in";
				input.Name = element.Attr("name", input.Name ?? input.Code);
				input.Role = element.Attr("role", input.Role ?? configuration.DefaultElementRole);
				input.Template = element.Attr("template", input.Template ?? input.Code);
				input.SqlOptimization = element.Attr("sqloptimization", "");
				if (!input.Template.EndsWith(".in")) {
					input.Template += ".in";
				}

				element.Apply(input, "id", "name", "code", "role", "template");
				input.IgnorePeriodState = element.Attr("ignoreperiodstate").ToBool();
				input.IsObjectDependent = element.Attr("isobjectdependent").ToBool();


				input.FixedObj = element.ElementOrAttr("fixedobj", "");

				input.Biztran = element.Attr("biztran", "");
				input.NeedPreloadScript = element.ElementOrAttr("needpreloadscript", false);
				input.UseQuickUpdate = element.ElementOrAttr("usequickupdate", false);
				input.DocumentRoot = element.Attr("docroot", "");
				if (input.DocumentRoot.IsEmpty()) {
					input.DocumentRoot = element.Elements("docroot").LastOrDefault().Attr("code", "");
				}


				if (!input.Active) {
					continue;
				}
				
				input.Sources = element.Elements("uselib").Select(x => x.Attr("id")).Where(x => x.IsNotEmpty()).ToArray();
				input.ColumnDefinitions = element.XPathSelectElements("./col").ToArray();
				input.RowDefinitions = element.XPathSelectElements("./row").ToArray();
				var root = element.XPathSelectElement("./root");
				if (root != null) {
					input.RootCode = root.Attr("code", "");
				}


				var parameters = element.XPathSelectElements("./param");
				foreach (var parameter in parameters) {
					var param = readParameter(parameter);
					input.Parameters.Add(param);
				}

				var docs = element.XPathSelectElements("./doc");
				foreach (var x in docs) {
					input.Documents[x.Attr("code")] = x.Attr("name");
				}


				configuration.Inputs[input.Code] = input;
			}
		}

		private void readOutputs(ThemaConfiguration configuration) {
			var outs = configuration.SrcXml.Elements("out").ToArray();
			foreach (var element in outs) {
				var id = element.Attr("code", "");
				if (id.IsIn("Aa", "Ab", "Ba", "Bb", "Ca", "Cb")) {
					if (configuration.Parameters.ContainsKey("active" + id.Substring(0, 1))) {
						var active = configuration.ResolveParameter("active" + id.Substring(0, 1)).Value.ToBool();
						if (!active) {
							element.Remove();
							continue;
						}
					}
				}


				var output = new OutputConfiguration();

				foreach (var parameter in configuration.Parameters) {
					if (parameter.Key.StartsWith("out.")) {
						var name = parameter.Key.Substring(4);
						output.SetValue(name, parameter.Value.GetValue());
					}
				}

				output.Thema = configuration;

				output.Code = configuration.Code + element.IdCodeOrValue() + ".out";
				output.Name = element.Attr("name", output.Code);
				output.Role = element.Attr("role", output.Role ?? configuration.DefaultElementRole);
				output.Type = element.Attr("type", "call");
				output.Template = element.Attr("template", output.Template ?? output.Code);
				output.Sources = element.Elements("uselib").Select(x => x.IdCodeOrValue()).Where(x => x.IsNotEmpty()).ToArray();

				if (!output.Template.EndsWith(".out")) {
					output.Template += ".out";
				}
				var exfile = PathResolver.ResolveAll("data", output.Template + ".xml", true).FirstOrDefault();
				if (null == exfile) {
					output.Template = "empty.out";
				}

				element.Apply(output, "id", "name", "code", "role", "type", "template");


				if (!output.Active) {
					continue;
				}
				if (output.Template == "empty.out") {
					output.TemplateXml = XElement.Parse("<report/>");
					foreach (var e in element.Elements()) {
						output.TemplateXml.Add(e);
					}
				}
				configuration.Outputs[output.Code] = output;
			}
		}

		

		private TypedParameter readParameter(XElement parameter) {
			var p = new TypedParameter
				{
					Type = ReflectionExtensions.ResolveTypeByWellKnownName(parameter.Attr("type", "str")),
					Value = parameter.Attr("value", null) ?? parameter.Value,
					Name = parameter.IdCodeOrValue(),
					Mode = parameter.Attr("mode", "static")
				};
			return p;
		}

		private void prepareEmpty(IDictionary<string, ThemaConfiguration> configurations, XElement[] themas) {
			foreach (var thema in themas) {
				var desc = new ThemaConfiguration
					{
						Code = thema.IdCodeOrValue("default"),
						Name = thema.Attr("name", thema.Attr("code", "default")),
						Active = thema.Attr("active").IsEmpty() || thema.Attr("active").ToBool(),
						AutoImport = thema.Attr("autoimport").ToBool(),
						Layout = thema.Attr("layout", "default"),
						Abstract = thema.Attr("abst").ToBool(),
						Idx = thema.Attr("idx").ToInt(),
						SrcXml = thema,
					};
				thema.Apply(desc, "id", "name", "code", "active", "autoimport", "layout", "abst", "idx");
				if (desc.Abstract) {
					desc.Active = false;
				}
				desc.ConfigurationProvider = this;
				configurations[desc.Code] = desc;
			}
		}

		private DateTime _cfgVersion;
		private IContainer _container;

		private IFileService _pathResolver;
	}
}