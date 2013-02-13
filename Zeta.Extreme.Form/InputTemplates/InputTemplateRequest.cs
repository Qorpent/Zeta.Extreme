#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : InputTemplateRequest.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Inversion;
using Comdiv.Persistence;
using Comdiv.Security;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;
using NHibernate;
using Zeta.Extreme.Form.SaveSupport;

namespace Zeta.Extreme.Form.InputTemplates {
	/// <summary>
	/// 	Запрос на форму
	/// </summary>
	public class InputTemplateRequest {
		/// <summary>
		/// </summary>
		public InputTemplateRequest() {
			storage = myapp.storage.Get<IZetaCell>();
		}

		/// <summary>
		/// 	Проверяет, не является ли форма read-only
		/// </summary>
		public bool IsReadOnly {
			get {
				if (myapp.roles.IsInRole("NOBLOCK", false)) {
					return false;
				}
				return Template.GetState(Object, Detail) != "0ISOPEN" || !Template.IsPeriodOpen();
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

		/// <summary>
		/// 	Объект
		/// </summary>
		public IZetaMainObject Object {
			get {
				if (null == @object && null != ObjectId) {
					@object = Template.FixedObject ?? storage.Load<IZetaMainObject>(ObjectId);
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
				if (null == template && TemplateCode.hasContent()) {
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
		/// 	Связанная длительная задача
		/// </summary>
		public LongTask Task { get; set; }

		/// <summary>
		/// 	Проверка активности формы
		/// </summary>
		/// <param name="full"> </param>
		/// <exception cref="Exception"></exception>
		public void CheckFormLive(bool full = false) {
			if (Task.HaveToTerminate || (full && (null != Task.Context && !Task.Context.Response.IsClientConnected))) {
				Task.Terminate();
				throw new Exception("Выполнение формы было отменено (noerrc)");
			}
		}

		/// <summary>
		/// 	Создает копию
		/// </summary>
		/// <returns> </returns>
		public InputTemplateRequest Copy() {
			var result = (InputTemplateRequest) MemberwiseClone();
			result.cachedPkg = null;
			return result;
		}

		/// <summary>
		/// 	Готовит пакет по умолчанию
		/// </summary>
		/// <returns> </returns>
		public IPkg GetDefaultPkg() {
			if (null == cachedPkg) {
				using (var s = new TemporaryTransactionSession()) {
					var type = storage.Load<IPkgType>("DEFAULTFORMPKG");
					if (null == type) {
						type = storage.New<IPkgType>();
						type.Code = "DEFAULTFORMPKG";
						type.Name = "Пакет для заполняемой формы в целом (вне сеанса)";
						storage.Save(type);
						s.CanBeCommited = true;
					}
					var code = ToString().toSystemName().Replace("_", "");
					var result = storage.Load<IPkg>(code);
					if (null == result) {
						result = storage.New<IPkg>();
						result.Code = code;
						result.Name = "Пакет формы по умолчанию для " + this;
						result.Usr = myapp.usrName;
						result.Type = type;
						result.CreateTime = DateTime.Now;
						result.State = PkgState.None;
						result.Date = DateTime.Now;
						if (Period != 0) {
							result.Date = Periods.Get(Period).EndDate.accomodateToYear(Year);
						}
						if (Date.Year > 1901) {
							result.Date = Date;
						}
						result.Object = Object;
						result.DetailObject = Detail;
						storage.Save(result);
						s.CanBeCommited = true;
					}
					cachedPkg = result;
				}
			}
			return cachedPkg;
		}

		/// <summary>
		/// 	Возвращает строку, которая представляет текущий объект.
		/// </summary>
		/// <returns> Строка, представляющая текущий объект. </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString() {
			return string.Format("FORM REQUEST T:{0},O:{1},D:{2}, Y:{3},P:{4},D:{5}", TemplateCode, ObjectId, DetailId,
			                     Year, Period, Date.ToString("ddMMyyyyHHmmss"));
		}

		/// <summary>
		/// 	Производит сохранение данных
		/// </summary>
		/// <param name="xml"> </param>
		/// <exception cref="Exception"></exception>
		public void Save(string xml) {
			ReloadTemplate();
			Task.PhaseFinished("prepare template");
			if (IsReadOnly) {
				if (!Template.IsPeriodOpen()) {
					throw new Exception("Период был заблокирован, ввод новых данных невозможен");
				}
				throw new Exception("Шаблон был заблокирован, ввод новых данных невозможен");
			}

			var reader = new CellSerializer();
			var saver = new CellSaver {TemplateRequest = this};
			if (Template.CustomSave.hasContent()) {
				var formsaver = myapp.ioc.get(Template.CustomSave) as ICustomFormSaver;
				formsaver.Save(this, xml);
			}
			else {
				using (DataSaveLock.Get()) {
					using (var handler = new TemporaryTransactionSession()) {
						handler.Session.FlushMode = FlushMode.Commit;

						var cells = reader.ReadXml(xml);
						Task.PhaseFinished("cells readed");
						saver.Save(cells);
						Task.PhaseFinished("save executed");
						handler.Commit();
					}
					Task.PhaseFinished("transaction commited");
				}

				using (var c = myapp.ioc.getConnection()) {
					c.ExecuteNonQuery(
						@"exec usm.after_save_trigger 
                            @form=@form,
                            @obj=@obj,
                            @year=@year,
                            @period=@period,
                            @usr=@usr",
						new Dictionary<string, object>
							{
								{"@form", TemplateCode},
								{"@obj", ObjectId},
								{"@year", Year},
								{"@period", Period},
								{"@usr", myapp.usrName.ToLower()}
							}, 900);
				}
				Task.PhaseFinished("after save trigger called");
			}
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
		private readonly StorageWrapper<IZetaCell> storage;
		private IPkg cachedPkg;
		private IZetaDetailObject detail;
		private object detailId;
		private IZetaMainObject @object;
		private object objectId;
		private IInputTemplate template;
		private string templateCode;
	}
}