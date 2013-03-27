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
// PROJECT ORIGIN: Zeta.Extreme.Form/EcoThemaHelper.cs
#endregion
using System;
using System.Linq;
using Qorpent.Applications;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.Reports;
using Zeta.Extreme.BizProcess.StateManagement;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	Вспомогательный класс для темы
	/// </summary>
	public class EcoThemaHelper {
		/// <summary>
		/// 	Создает всомогательный класс хелпера Eco для форм
		/// </summary>
		/// <param name="thema"> </param>
		public EcoThemaHelper(IThema thema) {
			this.thema = thema;
			statemanager = Application.Current.Container.Get<IStateManager>();
			main = GetMainForm();
			plan = thema.GetForm(thema.Code + "-plan") ?? thema.GetForm("B");
			planc = thema.GetForm(thema.Code + "-plan-c") ?? thema.GetForm("C");
			_dreport = thema.GetReport(thema.Code + "-p") ?? thema.GetReport("Ab");
			_dpreport = thema.GetReport(thema.Code + "-plan-p") ?? thema.GetReport("Bb");
			_sreport = thema.GetReport(thema.Code) ?? thema.GetReport("Aa");
			_spreport = thema.GetReport(thema.Code + "-plan") ?? thema.GetReport("Ba");
			_creport = thema.GetReport(thema.Code + "-plan-c") ?? thema.GetReport("Ca");
			_cpreport = thema.GetReport(thema.Code + "-plan-c-p") ?? thema.GetReport("Cb");
			_help = thema.GetDocument(thema.Code);
			_invtarget = thema.Parameters.SafeGet("invalidtargetobject").ToBool();
		}

		/// <summary>
		/// 	Период главной формы
		/// </summary>
		public string mainperiod {
			get { return getperiod(mainform, orgreport); }
		}

		/// <summary>
		/// 	Период плановой формы
		/// </summary>
		public string planperiod {
			get { return getperiod(planform, orgplanreport); }
		}

		/// <summary>
		/// 	Период формы корректива
		/// </summary>
		public string plancperiod {
			get { return getperiod(plancform, orgplancreport); }
		}

		/// <summary>
		/// 	Целевой объект
		/// </summary>
		public IZetaMainObject Object { get; set; }

		/// <summary>
		/// 	Признак того, что тема для деталей
		/// </summary>
		public bool isdetail {
			get { return thema.Parameters.SafeGet("isdetail", false); }
		}

		/// <summary>
		/// 	Класс деталей
		/// </summary>
		public string DetailClasses {
			get { return thema.Parameters.SafeGet("detailclasses", ""); }
		}


		/// <summary>
		/// 	Призанк использования плановой формы
		/// </summary>
		public bool useplanform {
			get { return plan != null; }
		}

		/// <summary>
		/// 	Форма коррективов
		/// </summary>
		public IInputTemplate plancform {
			get { return planc; }
		}

		/// <summary>
		/// 	Признак использования коррективной формы
		/// </summary>
		public bool useplancform {
			get { return planc != null; }
		}

		/// <summary>
		/// 	Признак видимости главной формы
		/// </summary>
		public bool mainformvisible {
			get { return thema.Parameters.SafeGet("f_visibleA", true); }
		}

		/// <summary>
		/// 	Признак видимости блокиратора плановой формы
		/// </summary>
		public bool planlockvisible {
			get { return thema.Parameters.SafeGet("fl_visibleB", true); }
		}

		/// <summary>
		/// 	Признак видимости сводного планового отчета
		/// </summary>
		public bool plansvodvisible {
			get { return thema.Parameters.SafeGet("ra_visibleB", true); }
		}

		/// <summary>
		/// 	Признак видимости блокиратора коррективов
		/// </summary>
		public bool planclockvisible {
			get { return thema.Parameters.SafeGet("fl_visibleC", true); }
		}

		/// <summary>
		/// 	Признак видимости сводного коррективного отчета
		/// </summary>
		public bool plancsvodvisible {
			get { return thema.Parameters.SafeGet("ra_visibleC", true); }
		}

		/// <summary>
		/// 	Признак видимости главного сводного отчета
		/// </summary>
		public bool mainsvodvisible {
			get { return thema.Parameters.SafeGet("ra_visibleA", true); }
		}

		/// <summary>
		/// 	Признак видимости плановой формы
		/// </summary>
		public bool planformvisible {
			get { return thema.Parameters.SafeGet("f_visibleB", true); }
		}

		/// <summary>
		/// 	Признак видимости коррективной формы
		/// </summary>
		public bool plancformvisible {
			get { return thema.Parameters.SafeGet("f_visibleC", true); }
		}

		/// <summary>
		/// 	Признак видимости главного отчета предприятия
		/// </summary>
		public bool mainreportvisible {
			get { return thema.Parameters.SafeGet("rb_visibleA", true); }
		}

		/// <summary>
		/// 	Признак видимости планового отчета предприятия
		/// </summary>
		public bool planreportvisible {
			get { return thema.Parameters.SafeGet("rb_visibleB", true); }
		}


		/// <summary>
		/// 	Признак видимости коррективного отчета предприятия
		/// </summary>
		public bool plancreportvisible {
			get { return thema.Parameters.SafeGet("rb_visibleC", true); }
		}

		/// <summary>
		/// 	Признак видимости блокиратора главной формы
		/// </summary>
		public bool mainlockvisible {
			get { return thema.Parameters.SafeGet("fl_visibleA", true); }
		}

		/// <summary>
		/// 	Плановая форма
		/// </summary>
		public IInputTemplate planform {
			get { return plan; }
		}

		/// <summary>
		/// 	Признак использования главной формы
		/// </summary>
		public bool usemainform {
			get { return main != null; }
		}

		/// <summary>
		/// 	Главная форма
		/// </summary>
		public IInputTemplate mainform {
			get { return main; }
		}

		/// <summary>
		/// 	Признак использования отчета предприятия
		/// </summary>
		public bool useorgreport {
			get { return _dreport != null; }
		}

		/// <summary>
		/// 	Ссылка на отчет предприятия
		/// </summary>
		public IReportDefinition orgreport {
			get { return _dreport; }
		}

		///<summary>
		///	Признак видимости планового отчета предприятия
		///</summary>
		public bool useorgplanreport {
			get { return _dpreport != null; }
		}

		/// <summary>
		/// 	Плановый отчет предприятия
		/// </summary>
		public IReportDefinition orgplanreport {
			get { return _dpreport; }
		}

		/// <summary>
		/// 	Признак использования основного сводного отчета
		/// </summary>
		public bool usesvodreport {
			get { return _sreport != null; }
		}

		/// <summary>
		/// 	Признак использования сводного планового отчета
		/// </summary>
		public bool usesvodplanreport {
			get { return _spreport != null; }
		}


		/// <summary>
		/// 	Признак использования коррективного отчета предприятия
		/// </summary>
		public bool useorgplancreport {
			get { return _cpreport != null; }
		}

		/// <summary>
		/// 	Коррективный отчт предприятия
		/// </summary>
		public IReportDefinition orgplancreport {
			get { return _cpreport; }
		}


		/// <summary>
		/// 	Признак использования сводного коррективного отчета
		/// </summary>
		public bool usesvodplancreport {
			get { return _creport != null; }
		}


		/// <summary>
		/// 	Признак использования коррективного отчета предприятия
		/// </summary>
		public bool useplancreport {
			get { return _cpreport != null; }
		}

		/// <summary>
		/// 	Сводный отчет по коррективам
		/// </summary>
		public IReportDefinition svodplancreport {
			get { return _creport; }
		}

		///////////////////////////////


		/// <summary>
		/// 	Главный сводный отчет
		/// </summary>
		public IReportDefinition svodreport {
			get { return _sreport; }
		}

		/// <summary>
		/// 	Признак использования планового отчета
		/// </summary>
		public bool useplanreport {
			get { return _dpreport != null; }
		}

		/// <summary>
		/// 	Плановый сводный отчет
		/// </summary>
		public IReportDefinition svodplanreport {
			get { return _spreport; }
		}

		/// <summary>
		/// 	Видимость темы
		/// </summary>
		public bool isvisible {
			get {
				if (Application.Current.Roles.IsAdmin())
				{
					return true;
				}
				if (invalidtarget) {
					return false;
				}

				return null != (main ?? plan ?? (object) _dreport ?? _dpreport ?? svodreport ?? svodplanreport);
			}
		}

		/// <summary>
		/// 	Главная форма открыта
		/// </summary>
		public bool ismainopen {
			get {
				if (!UseMainForm()) {
					if (UseDefaultReport()) {
						return getreportstate(orgreport);
					}
					return false;
				}
				return main.IsOpen;
			}
		}

		/// <summary>
		/// 	признак открытости плана
		/// </summary>
		public bool isplanopen {
			get {
				if (!useplanform) {
					if (useplanreport) {
						return getreportstate(orgplanreport);
					}
					return false;
				}
				return plan.IsOpen;
			}
		}

		/// <summary>
		/// 	Признак открытости корректива
		/// </summary>
		public bool isplancopen {
			get {
				if (!useplancform) {
					if (useplancreport) {
						return getreportstate(orgplancreport);
					}
					return false;
				}
				return planc.IsOpen;
			}
		}

		/// <summary>
		/// 	Признак утверждения главной формы
		/// </summary>
		public bool ismainchecked {
			get {
				if (!UseMainForm()) {
					if (UseDefaultReport()) {
						return getreportstate(orgreport, "0ISCHECKED");
					}
					return false;
				}
				return main.IsChecked;
			}
		}

		/// <summary>
		/// 	Признак утверждения плана
		/// </summary>
		public bool isplanchecked {
			get {
				if (!useplanform) {
					if (useplanreport) {
						return getreportstate(orgplanreport, "0ISCHECKED");
					}
					return false;
				}
				return plan.IsChecked;
			}
		}

		/// <summary>
		/// 	Признак утверждения корректива
		/// </summary>
		public bool isplancchecked {
			get {
				if (!useplancform) {
					if (useplancreport) {
						return getreportstate(orgplancreport, "0ISCHECKED");
					}
					return false;
				}
				return planc.IsChecked;
			}
		}


		/// <summary>
		/// 	Команда откр
		/// </summary>
		public string openmaincommand {
			get {
				if (null == main) {
					return "comdiv.modal.alert('Вам не доступно управление формой ввода!');";
				}
				return string.Format("Zeta.form.open('{0}','{1}','{2}');", main.Code, thema.Code, main.Period);
			}
		}

		/// <summary>
		/// 	Заголовок команды открытия главной формы
		/// </summary>
		public string openmaintitle {
			get {
				if (null == main) {
					return "Форма ввода (недоступно)";
				}
				if (ismainopen) {
					return "Заполнить форму ввода";
				}
				return "Просмотреть форму ввода";
			}
		}

		/// <summary>
		/// 	Команда блокировки главной формы
		/// </summary>
		public string lockmaincommand {
			get { return getLockCommand(usemainunderwrite, ismainopen, main); }
		}

		/// <summary>
		/// 	Заголовок команды блокировки главной формы
		/// </summary>
		public string lockmaintitle {
			get {
				if (!usemainunderwrite) {
					return "Управление блокировкой (недоступно)";
				}
				if (ismainopen) {
					return "Блокировать форму ввода";
				}
				return "Снять блокировку";
			}
		}


		/// <summary>
		/// 	Команда открытия плана
		/// </summary>
		public string openplancommand {
			get {
				if (null == plan) {
					return "comdiv.modal.alert('Вам не доступно управление формой ввода плана!');";
				}
				return string.Format("Zeta.form.open('{0}','{1}','{2}');", plan.Code, thema.Code, plan.Period);
			}
		}

		/// <summary>
		/// 	Заголовок команды открытия плана
		/// </summary>
		public string openplantitle {
			get {
				if (null == plan) {
					return "Форма ввода плана (недоступно)";
				}
				if (isplanopen) {
					return "Заполнить форму ввода плана";
				}
				return "Просмотреть форму ввода плана";
			}
		}

		/// <summary>
		/// 	Команда закрытия корректива
		/// </summary>
		public string lockplancommand {
			get { return getLockCommand(useplanunderwrite, isplanopen, plan); }
		}

		/// <summary>
		/// 	Заголовок команды блокирования плана
		/// </summary>
		public string lockplantitle {
			get {
				if (!useplanunderwrite) {
					return "Управление блокировкой плановой формы (недоступно)";
				}
				if (isplanopen) {
					return "Блокировать форму ввода плана";
				}
				return "Снять блокировку (форма плана)";
			}
		}

		/// <summary>
		/// 	Команда открытия корректива
		/// </summary>
		public string openplanccommand {
			get {
				if (null == planc) {
					return "comdiv.modal.alert('Вам не доступно управление формой ввода корректив плана!');";
				}
				return string.Format("Zeta.form.open('{0}','{1}','{2}');", planc.Code, thema.Code, planc.Period);
			}
		}

		/// <summary>
		/// 	Заголовок команды открытия корректива
		/// </summary>
		public string openplanctitle {
			get {
				if (null == planc) {
					return "Форма ввода корректив (недоступно)";
				}
				if (isplancopen) {
					return "Заполнить форму ввода корректив";
				}
				return "Просмотреть форму ввода корректив";
			}
		}

		/// <summary>
		/// 	Команда закрытия корректива
		/// </summary>
		public string lockplanccommand {
			get { return getLockCommand(useplancunderwrite, isplancopen, planc); }
		}

		/// <summary>
		/// 	Заголовок команды закртыия корректива
		/// </summary>
		public string lockplanctitle {
			get {
				if (!useplancunderwrite) {
					return "Управление блокировкой формы корректив (недоступно)";
				}
				if (isplancopen) {
					return "Блокировать форму ввода корректив";
				}
				return "Снять блокировку (форма корректива)";
			}
		}


		///////////////////////////////////

		/// <summary>
		/// 	Команда открытия простого отчета
		/// </summary>
		public string orgreportcommand {
			get {
				if (!UseDefaultReport()) {
					return "comdiv.modal.alert('Вам не доступeн вызов стандартного отчета!');";
				}

				return "Zeta.report.open('" + _dreport.Code + "','" + thema.Code + "');";
			}
		}

		/// <summary>
		/// 	Заголовок команды открытия простого отчета
		/// </summary>
		public string orgreporttitle {
			get {
				if (!UseDefaultReport()) {
					return "Базовый отчет (недоступно)";
				}

				return "Сформировать базовый отчет";
			}
		}

		/// <summary>
		/// 	Команда открытия планового отчета
		/// </summary>
		public string orgplanreportcommand {
			get {
				if (!useorgplanreport) {
					return "comdiv.modal.alert('Вам не доступeн вызов стандартного планового отчета!');";
				}

				return "Zeta.report.open('" + _dpreport.Code + "','" + thema.Code + "');";
			}
		}

		/// <summary>
		/// 	Заголовок команды открытия планового отчета
		/// </summary>
		public string orgplanreporttitle {
			get {
				if (!useorgplanreport) {
					return "Базовый плановый отчет (недоступно)";
				}

				return "Сформировать базовый плановый отчет";
			}
		}

		/// <summary>
		/// 	Команда открытия отчета по коррективам
		/// </summary>
		public string orgplancreportcommand {
			get {
				if (!useorgplancreport) {
					return "comdiv.modal.alert('Вам не доступeн вызов стандартного отчета корректив!');";
				}

				return "Zeta.report.open('" + _cpreport.Code + "','" + thema.Code + "');";
			}
		}

		/// <summary>
		/// 	Заголовок открытия отчета по коррективам
		/// </summary>
		public string orgplancreporttitle {
			get {
				if (!useorgplancreport) {
					return "Отчет корректив (недоступно)";
				}

				return "Сформировать отчет корректив";
			}
		}


		/////

		/// <summary>
		/// 	Команда получения справки
		/// </summary>
		public string helpcommand {
			get { return "zeta.workbench.loadthemadetails('" + thema.Code + "');"; }
		}

		/// <summary>
		/// 	Заголовок справки
		/// </summary>
		public string helptitle {
			get {
				if (!usehelp) {
					return "Дополнительная информация (недоступно)";
				}

				return "Дополнительная информация";
			}
		}

		/// <summary>
		/// 	Признак использования справки
		/// </summary>
		public bool usehelp {
			get { return _help != null; }
		}

		/// <summary>
		/// 	Команда открытия свода
		/// </summary>
		public string svodreportcommand {
			get {
				if (!UseSvodReport()) {
					return "comdiv.modal.alert('Вам не доступeн вызов сводного отчета!');";
				}

				return "Zeta.report.prepare('" + _sreport.Code + "','" + thema.Code + "');";
			}
		}

		/// <summary>
		/// 	Заголовок команды открытия свода
		/// </summary>
		public string svodreporttitle {
			get {
				if (!UseSvodReport()) {
					return "Сводный отчет (недоступно)";
				}

				return "Сформировать сводный отчет";
			}
		}


		///<summary>
		///	Команда открытия сводного корректива
		///</summary>
		public string svodplancreportcommand {
			get {
				if (!usesvodplancreport) {
					return "comdiv.modal.alert('Вам не доступeн вызов сводного планового отчета!');";
				}

				return "Zeta.report.prepare('" + _creport.Code + "','" + thema.Code + "');";
			}
		}

		/// <summary>
		/// 	Заголовок команды открытия сводного корректива
		/// </summary>
		public string svodplancreporttitle {
			get {
				if (!usesvodplancreport) {
					return "Сводный плановый отчет (недоступно)";
				}

				return "Сформировать сводный плановый отчет";
			}
		}


		///////

		/// <summary>
		/// 	Использовать главный блокиратор
		/// </summary>
		public bool usemainunderwrite {
			get {
				if (!UseMainForm()) {
					return false;
				}
				if (main.UnderwriteCode.IsEmpty()) {
					return false;
				}
				if (main.UnderwriteRole.IsEmpty()) {
					return Application.Current.Roles.IsAdmin();
				}

				if (ismainopen) {
					var state = main.GetScheduleState();
					if (!state.Date.IsDateNull()) {
						var from = new DateTime(state.Date.Year, state.Date.Month, 1);
						if (DateTime.Today < (from.AddDays(-10))) {
							return false;
						}
					}
				}

				return getUnderwriteByRoles(main);
			}
		}


		/// <summary>
		/// 	Команда свода по плану
		/// </summary>
		public string svodplanreportcommand {
			get {
				if (!usesvodplanreport) {
					return "comdiv.modal.alert('Вам не доступeн вызов сводного планового отчета!');";
				}

				return "Zeta.report.prepare('" + _spreport.Code + "','" + thema.Code + "');";
			}
		}

		/// <summary>
		/// 	Заголовок свода по плану
		/// </summary>
		public string svodplanreporttitle {
			get {
				if (!usesvodplanreport) {
					return "Сводный плановый отчет (недоступно)";
				}

				return "Сформировать сводный плановый отчет";
			}
		}

		/// <summary>
		/// 	Использовать блокиратор плана
		/// </summary>
		public bool useplanunderwrite {
			get {
				if (!useplanform) {
					return false;
				}
				if (plan.UnderwriteCode.IsEmpty()) {
					return false;
				}
				if (plan.UnderwriteRole.IsEmpty()) {
					return Application.Current.Roles.IsAdmin();
				}


				if (isplanopen) {
					var state = plan.GetScheduleState();
					if (state.Date.Year < 2500) {
						var from = new DateTime(state.Date.Year, state.Date.Month, 1);
						if (DateTime.Today < from) {
							return false;
						}
					}
				}

				var form = plan;
				return getUnderwriteByRoles(form);
			}
		}

		/// <summary>
		/// 	Использовать блокиратор корректива
		/// </summary>
		public bool useplancunderwrite {
			get {
				if (!useplancform) {
					return false;
				}
				if (planc.UnderwriteCode.IsEmpty()) {
					return false;
				}
				if (planc.UnderwriteRole.IsEmpty()) {
					return Application.Current.Roles.IsAdmin();
				}


				if (isplancopen) {
					var state = planc.GetScheduleState();
					if (state.Date.Year < 2500) {
						var from = new DateTime(state.Date.Year, state.Date.Month, 1);
						if (DateTime.Today < from) {
							return false;
						}
					}
				}

				return getUnderwriteByRoles(planc);
			}
		}

		/// <summary>
		/// 	Неверная цель (объект)
		/// </summary>
		public bool invalidtarget {
			get { return _invtarget; }
		}

		/// <summary>
		/// 	Получает строку с периодом (???)
		/// </summary>
		/// <param name="form"> </param>
		/// <param name="report"> </param>
		/// <returns> </returns>
		private string getperiod(IInputTemplate form, IReportDefinition report) {
			var period = 0;
			if (null != form) {
				period = form.Period;
			}
			else {
				//NOTE: Extreme пока не поддерживает отчетов
				/*
				if(null!=report) {
					var binded = ((ZetaReportDefinition) report).GetBindedForm();
					if(null!=binded) {
						var t = (EcoThema) this.thema;
						binded =binded.PrepareForPeriod(t.Year, t.Period, DateExtensions.Begin, t.Object);
						period = binded.Period;
					}
				}
				 */
			}
			if (0 == period) {
				return "";
			}
			return Periods.Get(period).Name + ":";
		}

		private bool getreportstate(IReportDefinition report, string statetocheck = "0ISOPEN") {
			return false;
			//NOTE: на данный момент отчеты в Extreme не работают
			/*
			if (null == report) return false;
    		return
    			((ZetaReportDefinition) report).GetState(((Thema) thema).Object, ((Thema) thema).Year, ((Thema) thema).Period) ==
				statetocheck;
			 */
		}

		/// <summary>
		/// 	Использовать главную форму
		/// </summary>
		/// <returns> </returns>
		public bool UseMainForm() {
			return main != null;
		}

		/// <summary>
		/// 	Использовать базовый отчет
		/// </summary>
		/// <returns> </returns>
		public bool UseDefaultReport() {
			return _dreport != null;
		}

		/// <summary>
		/// 	Использовать сводный отчет
		/// </summary>
		/// <returns> </returns>
		public bool UseSvodReport() {
			return _sreport != null;
		}

		private IInputTemplate GetMainForm() {
			return thema.GetForm(thema.Code) ?? thema.GetForm("A");
		}

		private string getLockCommand(bool useunderwrite, bool isopen, IInputTemplate target) {
			if (!useunderwrite) {
				return "comdiv.modal.alert('Вам не доступно управлений блокировкой формы ввода!');";
			}
			if (isopen) {
				//if ((message = target.CanSetState(Object, null, "0ISBLOCK")).noContent()){
				return string.Format("Zeta.form.lock('{0}',true,'{1}','{2}',{3},'{4}');", target.Code, target.Period
				                     , target.Name, target.Year, Periods.Get(target.Period).Name);
				//}
				//else{
				//   return "comdiv.modal.alert('На данный момент форму нельзя блокировать (" +
				//         message.Replace("'", "&apos;") + ")!');";
				// }
			}
			//if ((message = target.CanSetState(Object, null, "0ISOPEN")).noContent()){
			var deps = statemanager.GetDependentTemplates(target);
			if (deps.Count() == 0) {
				return string.Format("Zeta.form.lock('{0}',false,'{1}');", target.Code, target.Period);
			}
			else {
				return
					string.Format(
						"if (confirm('Данное действие также откроет:\" {2} \", вы уверены?'))Zeta.form.lock('{0}',false,'{1}');",
						target.Code
						, target.Period, deps.Select(x => x.Name.Replace("'", "\\'")).ConcatString(", "));
			}
			//}
			//else{
			//   return "comdiv.modal.alert('На данный момент форму нельзя открыть (" + message.Replace("'", "&apos;") +
			//         ")!');";
			//}
		}

		private bool getUnderwriteByRoles(IInputTemplate form) {
			if (Application.Current.Roles.IsAdmin()) {
				return true;
			}
			if (Application.Current.Roles.IsInRole("HOLDUNDERWRITER"))
			{
				return true;
			}
			if (!Application.Current.Roles.IsInRole(form.UnderwriteRole))
			{
				return false;
			}
			var s = form.GetState(Object, null);
			if (s == "0ISOPEN") {
				return true;
			}
			if (s == "0ISBLOCK" && Application.Current.Roles.IsInRole("DIVUNDERWRITER"))
			{
				return true;
			}
			return false;
		}

		private readonly IReportDefinition _cpreport;
		private readonly IReportDefinition _creport;
		private readonly IReportDefinition _dpreport;
		private readonly IReportDefinition _dreport;
		private readonly IDocument _help;
		private readonly bool _invtarget;
		private readonly IReportDefinition _spreport;
		private readonly IReportDefinition _sreport;
		private readonly IStateManager statemanager;
		private readonly IThema thema;

		/// <summary>
		/// 	Основная форма
		/// </summary>
		public IInputTemplate main;

		/// <summary>
		/// 	Плановая форма
		/// </summary>
		public IInputTemplate plan;

		/// <summary>
		/// 	Форма корректива
		/// </summary>
		public IInputTemplate planc;
	}
}