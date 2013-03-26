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
	/// 	��������������� ����� ��� ����
	/// </summary>
	public class EcoThemaHelper {
		/// <summary>
		/// 	������� �������������� ����� ������� Eco ��� ����
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
		/// 	������ ������� �����
		/// </summary>
		public string mainperiod {
			get { return getperiod(mainform, orgreport); }
		}

		/// <summary>
		/// 	������ �������� �����
		/// </summary>
		public string planperiod {
			get { return getperiod(planform, orgplanreport); }
		}

		/// <summary>
		/// 	������ ����� ����������
		/// </summary>
		public string plancperiod {
			get { return getperiod(plancform, orgplancreport); }
		}

		/// <summary>
		/// 	������� ������
		/// </summary>
		public IZetaMainObject Object { get; set; }

		/// <summary>
		/// 	������� ����, ��� ���� ��� �������
		/// </summary>
		public bool isdetail {
			get { return thema.Parameters.SafeGet("isdetail", false); }
		}

		/// <summary>
		/// 	����� �������
		/// </summary>
		public string DetailClasses {
			get { return thema.Parameters.SafeGet("detailclasses", ""); }
		}


		/// <summary>
		/// 	������� ������������� �������� �����
		/// </summary>
		public bool useplanform {
			get { return plan != null; }
		}

		/// <summary>
		/// 	����� �����������
		/// </summary>
		public IInputTemplate plancform {
			get { return planc; }
		}

		/// <summary>
		/// 	������� ������������� ������������ �����
		/// </summary>
		public bool useplancform {
			get { return planc != null; }
		}

		/// <summary>
		/// 	������� ��������� ������� �����
		/// </summary>
		public bool mainformvisible {
			get { return thema.Parameters.SafeGet("f_visibleA", true); }
		}

		/// <summary>
		/// 	������� ��������� ����������� �������� �����
		/// </summary>
		public bool planlockvisible {
			get { return thema.Parameters.SafeGet("fl_visibleB", true); }
		}

		/// <summary>
		/// 	������� ��������� �������� ��������� ������
		/// </summary>
		public bool plansvodvisible {
			get { return thema.Parameters.SafeGet("ra_visibleB", true); }
		}

		/// <summary>
		/// 	������� ��������� ����������� �����������
		/// </summary>
		public bool planclockvisible {
			get { return thema.Parameters.SafeGet("fl_visibleC", true); }
		}

		/// <summary>
		/// 	������� ��������� �������� ������������� ������
		/// </summary>
		public bool plancsvodvisible {
			get { return thema.Parameters.SafeGet("ra_visibleC", true); }
		}

		/// <summary>
		/// 	������� ��������� �������� �������� ������
		/// </summary>
		public bool mainsvodvisible {
			get { return thema.Parameters.SafeGet("ra_visibleA", true); }
		}

		/// <summary>
		/// 	������� ��������� �������� �����
		/// </summary>
		public bool planformvisible {
			get { return thema.Parameters.SafeGet("f_visibleB", true); }
		}

		/// <summary>
		/// 	������� ��������� ������������ �����
		/// </summary>
		public bool plancformvisible {
			get { return thema.Parameters.SafeGet("f_visibleC", true); }
		}

		/// <summary>
		/// 	������� ��������� �������� ������ �����������
		/// </summary>
		public bool mainreportvisible {
			get { return thema.Parameters.SafeGet("rb_visibleA", true); }
		}

		/// <summary>
		/// 	������� ��������� ��������� ������ �����������
		/// </summary>
		public bool planreportvisible {
			get { return thema.Parameters.SafeGet("rb_visibleB", true); }
		}


		/// <summary>
		/// 	������� ��������� ������������� ������ �����������
		/// </summary>
		public bool plancreportvisible {
			get { return thema.Parameters.SafeGet("rb_visibleC", true); }
		}

		/// <summary>
		/// 	������� ��������� ����������� ������� �����
		/// </summary>
		public bool mainlockvisible {
			get { return thema.Parameters.SafeGet("fl_visibleA", true); }
		}

		/// <summary>
		/// 	�������� �����
		/// </summary>
		public IInputTemplate planform {
			get { return plan; }
		}

		/// <summary>
		/// 	������� ������������� ������� �����
		/// </summary>
		public bool usemainform {
			get { return main != null; }
		}

		/// <summary>
		/// 	������� �����
		/// </summary>
		public IInputTemplate mainform {
			get { return main; }
		}

		/// <summary>
		/// 	������� ������������� ������ �����������
		/// </summary>
		public bool useorgreport {
			get { return _dreport != null; }
		}

		/// <summary>
		/// 	������ �� ����� �����������
		/// </summary>
		public IReportDefinition orgreport {
			get { return _dreport; }
		}

		///<summary>
		///	������� ��������� ��������� ������ �����������
		///</summary>
		public bool useorgplanreport {
			get { return _dpreport != null; }
		}

		/// <summary>
		/// 	�������� ����� �����������
		/// </summary>
		public IReportDefinition orgplanreport {
			get { return _dpreport; }
		}

		/// <summary>
		/// 	������� ������������� ��������� �������� ������
		/// </summary>
		public bool usesvodreport {
			get { return _sreport != null; }
		}

		/// <summary>
		/// 	������� ������������� �������� ��������� ������
		/// </summary>
		public bool usesvodplanreport {
			get { return _spreport != null; }
		}


		/// <summary>
		/// 	������� ������������� ������������� ������ �����������
		/// </summary>
		public bool useorgplancreport {
			get { return _cpreport != null; }
		}

		/// <summary>
		/// 	������������ ���� �����������
		/// </summary>
		public IReportDefinition orgplancreport {
			get { return _cpreport; }
		}


		/// <summary>
		/// 	������� ������������� �������� ������������� ������
		/// </summary>
		public bool usesvodplancreport {
			get { return _creport != null; }
		}


		/// <summary>
		/// 	������� ������������� ������������� ������ �����������
		/// </summary>
		public bool useplancreport {
			get { return _cpreport != null; }
		}

		/// <summary>
		/// 	������� ����� �� �����������
		/// </summary>
		public IReportDefinition svodplancreport {
			get { return _creport; }
		}

		///////////////////////////////


		/// <summary>
		/// 	������� ������� �����
		/// </summary>
		public IReportDefinition svodreport {
			get { return _sreport; }
		}

		/// <summary>
		/// 	������� ������������� ��������� ������
		/// </summary>
		public bool useplanreport {
			get { return _dpreport != null; }
		}

		/// <summary>
		/// 	�������� ������� �����
		/// </summary>
		public IReportDefinition svodplanreport {
			get { return _spreport; }
		}

		/// <summary>
		/// 	��������� ����
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
		/// 	������� ����� �������
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
		/// 	������� ���������� �����
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
		/// 	������� ���������� ����������
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
		/// 	������� ����������� ������� �����
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
		/// 	������� ����������� �����
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
		/// 	������� ����������� ����������
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
		/// 	������� ����
		/// </summary>
		public string openmaincommand {
			get {
				if (null == main) {
					return "comdiv.modal.alert('��� �� �������� ���������� ������ �����!');";
				}
				return string.Format("Zeta.form.open('{0}','{1}','{2}');", main.Code, thema.Code, main.Period);
			}
		}

		/// <summary>
		/// 	��������� ������� �������� ������� �����
		/// </summary>
		public string openmaintitle {
			get {
				if (null == main) {
					return "����� ����� (����������)";
				}
				if (ismainopen) {
					return "��������� ����� �����";
				}
				return "����������� ����� �����";
			}
		}

		/// <summary>
		/// 	������� ���������� ������� �����
		/// </summary>
		public string lockmaincommand {
			get { return getLockCommand(usemainunderwrite, ismainopen, main); }
		}

		/// <summary>
		/// 	��������� ������� ���������� ������� �����
		/// </summary>
		public string lockmaintitle {
			get {
				if (!usemainunderwrite) {
					return "���������� ����������� (����������)";
				}
				if (ismainopen) {
					return "����������� ����� �����";
				}
				return "����� ����������";
			}
		}


		/// <summary>
		/// 	������� �������� �����
		/// </summary>
		public string openplancommand {
			get {
				if (null == plan) {
					return "comdiv.modal.alert('��� �� �������� ���������� ������ ����� �����!');";
				}
				return string.Format("Zeta.form.open('{0}','{1}','{2}');", plan.Code, thema.Code, plan.Period);
			}
		}

		/// <summary>
		/// 	��������� ������� �������� �����
		/// </summary>
		public string openplantitle {
			get {
				if (null == plan) {
					return "����� ����� ����� (����������)";
				}
				if (isplanopen) {
					return "��������� ����� ����� �����";
				}
				return "����������� ����� ����� �����";
			}
		}

		/// <summary>
		/// 	������� �������� ����������
		/// </summary>
		public string lockplancommand {
			get { return getLockCommand(useplanunderwrite, isplanopen, plan); }
		}

		/// <summary>
		/// 	��������� ������� ������������ �����
		/// </summary>
		public string lockplantitle {
			get {
				if (!useplanunderwrite) {
					return "���������� ����������� �������� ����� (����������)";
				}
				if (isplanopen) {
					return "����������� ����� ����� �����";
				}
				return "����� ���������� (����� �����)";
			}
		}

		/// <summary>
		/// 	������� �������� ����������
		/// </summary>
		public string openplanccommand {
			get {
				if (null == planc) {
					return "comdiv.modal.alert('��� �� �������� ���������� ������ ����� ��������� �����!');";
				}
				return string.Format("Zeta.form.open('{0}','{1}','{2}');", planc.Code, thema.Code, planc.Period);
			}
		}

		/// <summary>
		/// 	��������� ������� �������� ����������
		/// </summary>
		public string openplanctitle {
			get {
				if (null == planc) {
					return "����� ����� ��������� (����������)";
				}
				if (isplancopen) {
					return "��������� ����� ����� ���������";
				}
				return "����������� ����� ����� ���������";
			}
		}

		/// <summary>
		/// 	������� �������� ����������
		/// </summary>
		public string lockplanccommand {
			get { return getLockCommand(useplancunderwrite, isplancopen, planc); }
		}

		/// <summary>
		/// 	��������� ������� �������� ����������
		/// </summary>
		public string lockplanctitle {
			get {
				if (!useplancunderwrite) {
					return "���������� ����������� ����� ��������� (����������)";
				}
				if (isplancopen) {
					return "����������� ����� ����� ���������";
				}
				return "����� ���������� (����� ����������)";
			}
		}


		///////////////////////////////////

		/// <summary>
		/// 	������� �������� �������� ������
		/// </summary>
		public string orgreportcommand {
			get {
				if (!UseDefaultReport()) {
					return "comdiv.modal.alert('��� �� ������e� ����� ������������ ������!');";
				}

				return "Zeta.report.open('" + _dreport.Code + "','" + thema.Code + "');";
			}
		}

		/// <summary>
		/// 	��������� ������� �������� �������� ������
		/// </summary>
		public string orgreporttitle {
			get {
				if (!UseDefaultReport()) {
					return "������� ����� (����������)";
				}

				return "������������ ������� �����";
			}
		}

		/// <summary>
		/// 	������� �������� ��������� ������
		/// </summary>
		public string orgplanreportcommand {
			get {
				if (!useorgplanreport) {
					return "comdiv.modal.alert('��� �� ������e� ����� ������������ ��������� ������!');";
				}

				return "Zeta.report.open('" + _dpreport.Code + "','" + thema.Code + "');";
			}
		}

		/// <summary>
		/// 	��������� ������� �������� ��������� ������
		/// </summary>
		public string orgplanreporttitle {
			get {
				if (!useorgplanreport) {
					return "������� �������� ����� (����������)";
				}

				return "������������ ������� �������� �����";
			}
		}

		/// <summary>
		/// 	������� �������� ������ �� �����������
		/// </summary>
		public string orgplancreportcommand {
			get {
				if (!useorgplancreport) {
					return "comdiv.modal.alert('��� �� ������e� ����� ������������ ������ ���������!');";
				}

				return "Zeta.report.open('" + _cpreport.Code + "','" + thema.Code + "');";
			}
		}

		/// <summary>
		/// 	��������� �������� ������ �� �����������
		/// </summary>
		public string orgplancreporttitle {
			get {
				if (!useorgplancreport) {
					return "����� ��������� (����������)";
				}

				return "������������ ����� ���������";
			}
		}


		/////

		/// <summary>
		/// 	������� ��������� �������
		/// </summary>
		public string helpcommand {
			get { return "zeta.workbench.loadthemadetails('" + thema.Code + "');"; }
		}

		/// <summary>
		/// 	��������� �������
		/// </summary>
		public string helptitle {
			get {
				if (!usehelp) {
					return "�������������� ���������� (����������)";
				}

				return "�������������� ����������";
			}
		}

		/// <summary>
		/// 	������� ������������� �������
		/// </summary>
		public bool usehelp {
			get { return _help != null; }
		}

		/// <summary>
		/// 	������� �������� �����
		/// </summary>
		public string svodreportcommand {
			get {
				if (!UseSvodReport()) {
					return "comdiv.modal.alert('��� �� ������e� ����� �������� ������!');";
				}

				return "Zeta.report.prepare('" + _sreport.Code + "','" + thema.Code + "');";
			}
		}

		/// <summary>
		/// 	��������� ������� �������� �����
		/// </summary>
		public string svodreporttitle {
			get {
				if (!UseSvodReport()) {
					return "������� ����� (����������)";
				}

				return "������������ ������� �����";
			}
		}


		///<summary>
		///	������� �������� �������� ����������
		///</summary>
		public string svodplancreportcommand {
			get {
				if (!usesvodplancreport) {
					return "comdiv.modal.alert('��� �� ������e� ����� �������� ��������� ������!');";
				}

				return "Zeta.report.prepare('" + _creport.Code + "','" + thema.Code + "');";
			}
		}

		/// <summary>
		/// 	��������� ������� �������� �������� ����������
		/// </summary>
		public string svodplancreporttitle {
			get {
				if (!usesvodplancreport) {
					return "������� �������� ����� (����������)";
				}

				return "������������ ������� �������� �����";
			}
		}


		///////

		/// <summary>
		/// 	������������ ������� ����������
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
		/// 	������� ����� �� �����
		/// </summary>
		public string svodplanreportcommand {
			get {
				if (!usesvodplanreport) {
					return "comdiv.modal.alert('��� �� ������e� ����� �������� ��������� ������!');";
				}

				return "Zeta.report.prepare('" + _spreport.Code + "','" + thema.Code + "');";
			}
		}

		/// <summary>
		/// 	��������� ����� �� �����
		/// </summary>
		public string svodplanreporttitle {
			get {
				if (!usesvodplanreport) {
					return "������� �������� ����� (����������)";
				}

				return "������������ ������� �������� �����";
			}
		}

		/// <summary>
		/// 	������������ ���������� �����
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
		/// 	������������ ���������� ����������
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
		/// 	�������� ���� (������)
		/// </summary>
		public bool invalidtarget {
			get { return _invtarget; }
		}

		/// <summary>
		/// 	�������� ������ � �������� (???)
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
				//NOTE: Extreme ���� �� ������������ �������
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
			//NOTE: �� ������ ������ ������ � Extreme �� ��������
			/*
			if (null == report) return false;
    		return
    			((ZetaReportDefinition) report).GetState(((Thema) thema).Object, ((Thema) thema).Year, ((Thema) thema).Period) ==
				statetocheck;
			 */
		}

		/// <summary>
		/// 	������������ ������� �����
		/// </summary>
		/// <returns> </returns>
		public bool UseMainForm() {
			return main != null;
		}

		/// <summary>
		/// 	������������ ������� �����
		/// </summary>
		/// <returns> </returns>
		public bool UseDefaultReport() {
			return _dreport != null;
		}

		/// <summary>
		/// 	������������ ������� �����
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
				return "comdiv.modal.alert('��� �� �������� ���������� ����������� ����� �����!');";
			}
			if (isopen) {
				//if ((message = target.CanSetState(Object, null, "0ISBLOCK")).noContent()){
				return string.Format("Zeta.form.lock('{0}',true,'{1}','{2}',{3},'{4}');", target.Code, target.Period
				                     , target.Name, target.Year, Periods.Get(target.Period).Name);
				//}
				//else{
				//   return "comdiv.modal.alert('�� ������ ������ ����� ������ ����������� (" +
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
						"if (confirm('������ �������� ����� �������:\" {2} \", �� �������?'))Zeta.form.lock('{0}',false,'{1}');",
						target.Code
						, target.Period, deps.Select(x => x.Name.Replace("'", "\\'")).ConcatString(", "));
			}
			//}
			//else{
			//   return "comdiv.modal.alert('�� ������ ������ ����� ������ ������� (" + message.Replace("'", "&apos;") +
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
		/// 	�������� �����
		/// </summary>
		public IInputTemplate main;

		/// <summary>
		/// 	�������� �����
		/// </summary>
		public IInputTemplate plan;

		/// <summary>
		/// 	����� ����������
		/// </summary>
		public IInputTemplate planc;
	}
}