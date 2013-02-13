#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : ThemaConfiguration.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;
using Comdiv.Extensions;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	Основной класс конфигурации темы
	/// </summary>
	public class ThemaConfiguration : IThemaConfiguration {
		/// <summary>
		/// 	Создает типовую конфигурацию темы
		/// </summary>
		public ThemaConfiguration() {
			Active = true;
			Visible = true;
			Parameters = new Dictionary<string, TypedParameter>();
			Imports = new List<IThemaConfiguration>();
		}

		/// <summary>
		/// 	Признак абстрактности
		/// </summary>
		public bool Abstract { get; set; }

		/// <summary>
		/// 	Код родительской темы
		/// </summary>
		public string Parent { get; set; }

		/// <summary>
		/// 	Признак групповой темы
		/// </summary>
		public bool IsGroup { get; set; }

		/// <summary>
		/// 	Код группы
		/// </summary>
		public string Group { get; set; }

		/// <summary>
		/// 	Класс для загрузки темы
		/// </summary>
		public string ClassName { get; set; }

		/// <summary>
		/// 	Признак, что используется шаблоны (старые)
		/// </summary>
		public bool IsTemplate { get; set; }

		/// <summary>
		/// 	Признак автоматического импорта
		/// </summary>
		public bool AutoImport { get; set; }

		/// <summary>
		/// 	Признак того, что импорт произведен
		/// </summary>
		public bool ImportsProcessed { get; set; }

		/// <summary>
		/// 	Признак видимости
		/// </summary>
		public bool Visible { get; set; }

		/// <summary>
		/// 	Вид рамки на стартовой странице
		/// </summary>
		public string Layout { get; set; }

		/// <summary>
		/// 	Шоткат к коду, но с Id-именем
		/// </summary>
		public string Id {
			get { return Code; }
		}

		/// <summary>
		/// 	Параметры темы
		/// </summary>
		public IDictionary<string, TypedParameter> Parameters { get; set; }

		/// <summary>
		/// 	Исходный XML
		/// </summary>
		public XElement SrcXml { get; set; }

		/// <summary>
		/// 	Роль доступа
		/// </summary>
		public string Role { get; set; }

		/// <summary>
		/// 	Код темы
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// 	Название темы
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 	Признак активности темы
		/// </summary>
		public bool Active { get; set; }

		/// <summary>
		/// 	Порядковый номер темы
		/// </summary>
		public int Idx { get; set; }

		/// <summary>
		/// 	Происхождение темы
		/// </summary>
		public string Evidence {
			get { return SrcXml.attr("evidence"); }
		}

		/// <summary>
		/// 	Метод определения значения парамтера с учетом импорта
		/// </summary>
		/// <param name="name"> </param>
		/// <param name="def"> </param>
		/// <returns> </returns>
		public TypedParameter ResolveParameter(string name, object def = null) {
			if (Parameters.ContainsKey(name)) {
				return Parameters[name];
			}
			var prop = GetType().resolveProperty(name);
			if (null != prop) {
				return new TypedParameter
					{
						Name = name,
						Type = prop.PropertyType,
						Value = prop.GetValue(this, null).toStr(),
					};
			}
			foreach (var import in Imports) {
				var value = import.ResolveParameter(name);
				if (!typeof (Missing).Equals(value.Type)) {
					return value;
				}
			}

			return new TypedParameter {Value = def.toStr()};
		}

		/// <summary>
		/// 	Импортированные конфигурации
		/// </summary>
		public IList<IThemaConfiguration> Imports { get; set; }

		/// <summary>
		/// 	Роль элемента по умолчанию
		/// </summary>
		public string DefaultElementRole { get; set; }


		/// <summary>
		/// 	Метод конструирования темы
		/// </summary>
		/// <returns> </returns>
		public IThema Configure() {
			var type = typeof (Thema);
			if (ClassName.hasContent()) {
				type = Type.GetType(ClassName, true);
			}
			var result = type.create<Thema>();
			result.Configuration = this;
			result.Code = Code;
			result.Name = Name;
			result.Role = Role;
			result.Idx = Idx;
			result.IsGroup = IsGroup;
			result.Group = Group;
			result.Layout = Layout;
			result.Visible = Visible;
			result.IsTemplate = IsTemplate;
			result.Parent = Parent;


			try {
				foreach (var output in Outputs.Values) {
					if (!output.IsError) {
						var def = output.Configure();
						def.Thema = result;
						result.Reports[output.Code] = def;
					}
				}

				foreach (var input in Inputs.Values) {
					if (!input.IsError) {
						var def = input.Configure();
						def.Thema = result;
						result.Forms[input.Code] = def;
					}
				}

				foreach (var command in Commands.Values) {
					if (!command.IsError) {
						result.Commands[command.Code] = command.Configure();
					}
				}

				foreach (var doc in Documents.Values) {
					if (!doc.IsError) {
						result.Documents[doc.Code] = doc.Configure();
					}
				}

				foreach (var parameter in Parameters.Values) {
					//skips temporary and pseudo-property parameters
					if (!parameter.Name.Contains(".")) {
						result.Parameters[parameter.Name] = parameter.GetValue();
					}
				}
			}
			catch (Exception ex) {
				result.Error = ex;
			}
			return result;
		}


		/// <summary>
		/// 	Простая перегрузка
		/// </summary>
		/// <returns> </returns>
		public override string ToString() {
			return Code + " " + Name;
		}

		/// <summary>
		/// 	Производит обработку импортируемых конфигураций
		/// </summary>
		public void ProcessImports() {
			if (ImportsProcessed) {
				return;
			}
			var firstelement = SrcXml.Elements().FirstOrDefault();
			foreach (ThemaConfiguration import in Imports) {
				import.ProcessImports();

				foreach (var element in import.SrcXml.Elements()) {
					XElement subelement = null;
					//if(element.Name.LocalName=="param" && !element.Attribute("id").Value.Contains(".")){
					//    subelement = new XElement(element);
					//    subelement.Attribute("id").Value = import.Code + "." + subelement.Attribute("id").Value;
					//}
					if (firstelement != null) {
						firstelement.AddBeforeSelf(element);
						if (subelement != null) {
							firstelement.AddBeforeSelf(subelement);
						}
					}
					else {
						SrcXml.Add(new XElement(element));
						if (subelement != null) {
							SrcXml.Add(new XElement(subelement));
						}
					}
				}
			}
			embedEarlyParametersIntoXml(this, SrcXml);
			ImportsProcessed = true;
		}

		private void embedEarlyParametersIntoXml(ThemaConfiguration configuration, XElement x) {
			foreach (XAttribute attr in ((IEnumerable) x.XPathEvaluate(".//@*"))) {
				attr.Value = embedEarlyParameters(configuration, attr.Value);
				//escapes some custom constructions, that must use ${...} constructions further
				//attr.Value = attr.Value.Replace("#{", "${");
			}
			foreach (XText e in ((IEnumerable) x.XPathEvaluate(".//text()"))) {
				e.Value = embedEarlyParameters(configuration, e.Value);
				//escapes some custom constructions, that must use ${...} constructions further
				//e.Value = e.Value.Replace("#{", "${");
			}
		}

		private string embedEarlyParameters(ThemaConfiguration configuration, string val) {
			if (val.like(@"ZZ([\.\w]+)ZZ")) {
				val = val.replace(@"ZZ([\.\w]+)ZZ", m =>
					{
						var par =
							configuration.SrcXml.XPathSelectElements("./param[@id='" +
							                                         m.Groups[1].Value +
							                                         "']").
								LastOrDefault();
						if (null == par) {
							return m.Value;
						}
						var v = par.attr("value");
						if (v.noContent()) {
							v = par.Value;
						}
						return v;
					});
			}
			return val;
		}

		///<summary>
		///	Конфигурации команд
		///</summary>
		public readonly IDictionary<string, CommandConfiguration> Commands =
			new Dictionary<string, CommandConfiguration>();

		///<summary>
		///	Конфигурации документов
		///</summary>
		public readonly IDictionary<string, DocumentConfiguration> Documents =
			new Dictionary<string, DocumentConfiguration>();

		/// <summary>
		/// 	Конфигурации форм
		/// </summary>
		public readonly IDictionary<string, InputConfiguration> Inputs = new Dictionary<string, InputConfiguration>();

		/// <summary>
		/// 	Конфигурации отчетов
		/// </summary>
		public readonly IDictionary<string, OutputConfiguration> Outputs = new Dictionary<string, OutputConfiguration>();
	}
}