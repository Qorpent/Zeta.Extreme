﻿#region LICENSE
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
// PROJECT ORIGIN: Zeta.Extreme.Form/ThemaConfiguration.cs
#endregion
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.SqlSupport;

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
		/// Дополнительная строка соединения для БД (чтобы облегчить тестирование интеграции с БД)
		/// </summary>
		public string ConnectionString { get; set; }

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
		/// 	Обратная ссылка на конфигуратор
		/// </summary>
		public ThemaConfigurationProvider ConfigurationProvider { get; set; }

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
		public int Index { get; set; }

		/// <summary>
		/// 	Происхождение темы
		/// </summary>
		public string Evidence {
			get { return SrcXml.Attr("evidence"); }
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
			var prop = GetType().FindValueMember(name);
			if (null != prop) {
				var propinfo = prop.Member as PropertyInfo;
				return new TypedParameter
					{
						Name = name,
						Type = propinfo.PropertyType,
						Value = propinfo.GetValue(this, null).ToStr(),
					};
			}
			foreach (var import in Imports) {
				var value = import.ResolveParameter(name);
				if (!(typeof (Missing) == value.Type)) {
					return value;
				}
			}

			return new TypedParameter {Value = def.ToStr()};
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
			if (ClassName.IsNotEmpty()) {
				var clsname = ClassName;
				if (ConfigurationProvider.Options.ClassRedirectMap.ContainsKey(clsname)) {
					clsname = ConfigurationProvider.Options.ClassRedirectMap[clsname];
				}
				type = Type.GetType(clsname, true);
			}
			var result = type.Create<Thema>();
			result.Configuration = this;
			result.Code = Code;
			result.Name = Name;
			result.Role = Role;
			result.Idx = Index;
			//if (ConfigurationProvider.Options.LoadIerarchy) {
				result.IsGroup = IsGroup;
				result.Group = Group;
			//}
			result.Layout = Layout;
			result.Visible = Visible;
			result.IsTemplate = IsTemplate;
			//if (ConfigurationProvider.Options.LoadIerarchy) {
				result.Parent = Parent;
			//}

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

			ApplyBizProcessParameters(result);
			ApplyHoldResponsibility(result);

			return result;
		}
		static  int _0CHID = 0;
		private void ApplyHoldResponsibility(Thema thema) {
			try {

				var reader = new NativeZetaReader { ConnectionString = ConnectionString };
				if (0 == _0CHID) {
					_0CHID = reader.ReadObjects("Code = '0CH'").First().Id;
				}
				var login = reader.GetThemaResponsiveLogin(thema.Code, _0CHID);
				if (!string.IsNullOrWhiteSpace(login)) {
					thema.Parameters["hold.responsibility"] = login.ToLower();
				}

			}
			catch (SqlException e)
			{
				thema.Parameters["bizprocess.load.error"] = e;
			}
			catch (NullReferenceException e)
			{
				thema.Parameters["bizprocess.load.error"] = e;
			}
		}

		private static string[] bizprocessTypes = new[] {"report", "primary", "extension", "mix"};
		private void ApplyBizProcessParameters(IThema thema) {
			try {
				var bizprocess =
					new NativeZetaReader {ConnectionString = ConnectionString}.ReadBizProcesses("Code = '" + thema.Code + "'")
					                                                          .FirstOrDefault();
				if (null != bizprocess) {
					thema.Parameters["bizprocess.object"] = bizprocess;
					thema.Parameters["bizprocess.inprocess"] = bizprocess.InProcess;
					thema.Parameters["bizprocess.process"] = bizprocess.Process;
					thema.Parameters["bizporcess.isreport"] = TagHelper.Value(bizprocess.Tag, "report").ToBool();
					thema.Parameters["bizporcess.isprimary"] = TagHelper.Value(bizprocess.Tag, "primary").ToBool();
					thema.Parameters["bizporcess.isextension"] = TagHelper.Value(bizprocess.Tag, "primary").ToBool();
					thema.Parameters["bizporcess.ismix"] = TagHelper.Value(bizprocess.Tag, "primary").ToBool();

					thema.Parameters["bizprocess.type"] =
						bizprocessTypes.FirstOrDefault(
							_ => (bool) thema.Parameters["bizporcess.is" + _]
							) ?? "mix";
				}
			}
			catch (SqlException e) {
				thema.Parameters["bizprocess.load.error"] = e;
			}
			//in no conigured connection case
			catch (NullReferenceException e) {
				thema.Parameters["bizprocess.load.error"] = e;
			}
		}


		/// <summary>
		/// 	Простая перегрузка
		/// </summary>
		/// <returns> </returns>
		public override string ToString() {
			return Code + " " + Name;
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