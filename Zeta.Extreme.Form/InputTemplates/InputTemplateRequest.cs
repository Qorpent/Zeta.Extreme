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
// PROJECT ORIGIN: Zeta.Extreme.Form/InputTemplateRequest.cs
#endregion
using System;
using System.Collections.Generic;
using Qorpent.Applications;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Form.InputTemplates {
	/// <summary>
	/// 	Запрос на форму
	/// </summary>
	public class InputTemplateRequest {
		/// <summary>
		/// </summary>
		public InputTemplateRequest() {
			//storage = myapp.storage.Get<IZetaCell>();
		}

		/// <summary>
		/// 	Проверяет, не является ли форма read-only
		/// </summary>
		public bool IsReadOnly {
			get {
				if (Application.Current.Roles.IsInRole("NOBLOCK", false)) {
					return false;
				}
				return Template.GetState(Object, null) != "0ISOPEN" || !Template.IsPeriodOpen();
			}
		}

		/// <summary>
		/// 	Код формы
		/// </summary>
		public string TemplateCode {
			get { return templateCode; }
			set {
				if (templateCode != value) {
					templateCode = value;
					Template = null;
				}
			}
		}
/*
		/// <summary>
		/// 	ID детали
		/// </summary>
		public object DetailId {
			get { return detailId; }
			set {
				if (detailId != value) {
					detailId = value;
					Detail = null;
				}
			}
		}
*/
		/// <summary>
		/// 	ID объекта
		/// </summary>
		public object ObjectId {
			get { return objectId; }
			set {
				if (objectId != value) {
					objectId = value;
					Object = null;
				}
			}
		}

		/// <summary>
		/// 	Год
		/// </summary>
		public int Year { get; set; }

		/// <summary>
		/// 	Период
		/// </summary>
		public int Period { get; set; }

		/// <summary>
		/// 	Прямая дата
		/// </summary>
		public DateTime Date { get; set; }
		/*
		/// <summary>
		/// 	Деталь
		/// </summary>
		public IZetaDetailObject Detail {
			get {
				if (null == detail && null != detailId) {
					detail = storage.Load<IZetaDetailObject>(DetailId);
				}
				return detail;
			}
			set { detail = value; }
		}
		 * */

		/// <summary>
		/// 	Объект
		/// </summary>
		public IZetaMainObject Object {
			get {
				if (null == @object && null != ObjectId) {
					@object = Template.FixedObject ?? MetaCache.Default.Get<IZetaMainObject>(ObjectId); //storage.Load<IZetaMainObject>(ObjectId);
					@objectId = @object.Id;
				}
				return @object;
			}
			set { @object = value; }
		}

		/// <summary>
		/// 	Шаблон
		/// </summary>
		public IInputTemplate Template {
			get {
				if (null == template && TemplateCode.IsNotEmpty()) {
					ReloadTemplate();
				}
				return template;
			}
			set { template = value; }
		}

		/// <summary>
		/// 	Параметры
		/// </summary>
		public IDictionary<string, string> Parameters {
			get { return parameters; }
		}


		/// <summary>
		/// 	Создает копию
		/// </summary>
		/// <returns> </returns>
		public InputTemplateRequest Copy() {
			var result = (InputTemplateRequest) MemberwiseClone();
			return result;
		}

		/// <summary>
		/// 	Возвращает строку, которая представляет текущий объект.
		/// </summary>
		/// <returns> Строка, представляющая текущий объект. </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString() {
			return string.Format("FORM REQUEST T:{0},O:{1},D:{2}, Y:{3},P:{4},D:{5}", TemplateCode, ObjectId, 0,
			                     Year, Period, Date.ToString("ddMMyyyyHHmmss"));
		}

		/// <summary>
		/// 	Перегрузить шаблон
		/// </summary>
		public void ReloadTemplate() {
			ReloadTemplate(true);
		}

		// static object sync = new object();
		/// <summary>
		/// </summary>
		/// <param name="keepParameters"> </param>
		/// <exception cref="NotSupportedException"></exception>
		public void ReloadTemplate(bool keepParameters) {
			throw new NotSupportedException("not ported in extreme");
			//lock (typeof (InputTemplateRequest)){
			//	var oldParameters = template != null ? template.Parameters : null;
			//	if(@object == null && 0!= @objectId.toInt()) {
			//		@object = myapp.storage.Get<IZetaMainObject>().Load(@objectId);
			//	}
			//	template = new InputTemplateRepository().GetTemplate(TemplateCode).PrepareForPeriod(Year, Period, Date,
			//																						@object);
			//		//myapp.storage.Get<IInputTemplate>().Load(TemplateCode).PrepareForPeriod(Year, Period, Date);
			//	foreach (var row in template.Rows){
			//		row.Target = RowCache.get(row.Code);
			//	}

			//	foreach (var parameter in Parameters){
			//		template.Parameters[parameter.Key] = parameter.Value;
			//	}

			//	if (keepParameters && null != oldParameters){
			//		foreach (var parameter in oldParameters){
			//			template.Parameters[parameter.Key] = parameter.Value;
			//		}
			//	}
			//}
		}


		/// <summary>
		/// 	Установить объект
		/// </summary>
		/// <param name="object"> </param>
		public void SetObject(IZetaMainObject @object) {
			ObjectId = @object.Code;
			Object = @object;
		}

		/// <summary>
		/// 	Установить шаблон
		/// </summary>
		/// <param name="template"> </param>
		public void SetTemplate(IInputTemplate template) {
			TemplateCode = template.Code;
			Template = template;
		}

		private readonly IDictionary<string, string> parameters = new Dictionary<string, string>();
		//private readonly StorageWrapper<IZetaCell> storage;
		/*
		private IZetaDetailObject detail;
		private object detailId;
		 */
		private IZetaMainObject @object;
		private object objectId;
		private IInputTemplate template;
		private string templateCode;
	}
}