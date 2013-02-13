// // Copyright 2007-2010 Comdiv (F. Sadykov) - http://code.google.com/u/fagim.sadykov/
// // Supported by Media Technology LTD 
// //  
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //  
// //      http://www.apache.org/licenses/LICENSE-2.0
// //  
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// // 
// // MODIFICATIONS HAVE BEEN MADE TO THIS FILE
using System;
using System.Linq;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Inversion;
using Comdiv.Model;
using Comdiv.Reporting;
using Comdiv.Security;
using Comdiv.Zeta.Data;
using Comdiv.Zeta.Model;
using Comdiv.Zeta.Report;
using Comdiv.Zeta.Web.InputTemplates;

namespace Comdiv.Zeta.Web.Themas{
    public class EcoThemaHelper{
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
        public IInputTemplate main;
        public IInputTemplate plan;
        public IInputTemplate planc;

    	public string mainperiod {
    		get { return getperiod(mainform, orgreport); }
    	}
		public string planperiod
		{
			get { return getperiod(planform, orgplanreport); }
		}
		public string plancperiod
		{
			get { return getperiod(plancform, orgplancreport); }
		}

    	private string getperiod(IInputTemplate form, IReportDefinition report) {
    		var period = 0;
			if(null!=form) {
				period = form.Period;
			}else {
				if(null!=report) {
					var binded = ((ZetaReportDefinition) report).GetBindedForm();
					if(null!=binded) {
						var t = (EcoThema) this.thema;
						binded =binded.PrepareForPeriod(t.Year, t.Period, DateExtensions.Begin, t.Object);
						period = binded.Period;
					}
				}
			}
			if (0 == period) return "";
    		return Periods.GetName(period) + ":";
    	}

    	public EcoThemaHelper(IThema thema){
            this.thema = thema;
            statemanager = ioc.get<IStateManager>();
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
            _invtarget = thema.Parameters.get("invalidtargetobject", false);
        }

        public IZetaMainObject Object { get; set; }

        public bool isdetail{
            get { return thema.Parameters.get("isdetail", false); }
        }

        public string DetailClasses{
            get { return thema.Parameters.get("detailclasses", ""); }
        }


        public HtmlListDefinition listdefinition{
            get{
                if (!isdetail){
                    return null;
                }
                return myapp.ioc.get<IThemaFactoryProvider>().Get().Cache.get(thema.Code + "_listdefinition_" + Object.Code,
                                               () =>{
                                                   var result = new HtmlListDefinition{Id = thema.Code + "_detail"};
                                                   var classes = DetailClasses.hasContent()
                                                                     ? DetailClasses.split().Select(
                                                                         x =>
                                                                         myapp.storage.Get<IDetailObjectClass>().
                                                                             Load(x))
                                                                           .Where(x => x != null).ToArray()
                                                                     : myapp.storage.Get<IDetailObjectClass>().All().
                                                                           ToArray();
                                                   foreach (var cls  in classes){
                                                       var details =
                                                           myapp.storage.Get<IZetaDetailObject>().Query(
                                                               "Type.Class = ? and Org = ?", cls, Object).ToArray();

                                                       if (details.Length != 0){
                                                           //var _cls = result.Add(cls.Code, cls.Name);

                                                           foreach (var type in cls.Types){
                                                               var typedetails =
                                                                   details.Where(
                                                                       x => ModelExtensions.Code(x.Type) == type.Code).
                                                                       ToArray();
                                                               if (typedetails.Length > 0){
                                                                   var _type = result.Add(type.Code,
                                                                                          type.Name);

                                                                   foreach (var detail in typedetails){
                                                                       _type.Add(detail.Id.ToString(), detail.Name);
                                                                   }
                                                               }
                                                           }
                                                       }
                                                   }

                                                   return result;
                                               });
            }
        }


        public bool useplanform{
            get { return plan != null; }
        }

    	public IInputTemplate plancform {
			get { return planc; }
    	}

        public bool useplancform{
            get { return planc != null; }
        }

        public bool mainformvisible{
            get { return thema.Parameters.get("f_visibleA", true); }
        }

        public bool planlockvisible{
            get { return thema.Parameters.get("fl_visibleB", true); }
        }


        public bool plansvodvisible{
            get { return thema.Parameters.get("ra_visibleB", true); }
        }

        public bool planclockvisible{
            get { return thema.Parameters.get("fl_visibleC", true); }
        }


        public bool plancsvodvisible{
            get { return thema.Parameters.get("ra_visibleC", true); }
        }


        public bool mainsvodvisible{
            get { return thema.Parameters.get("ra_visibleA", true); }
        }

        public bool planformvisible{
            get { return thema.Parameters.get("f_visibleB", true); }
        }

        public bool plancformvisible{
            get { return thema.Parameters.get("f_visibleC", true); }
        }

        public bool mainreportvisible{
            get { return thema.Parameters.get("rb_visibleA", true); }
        }

        public bool planreportvisible{
            get { return thema.Parameters.get("rb_visibleB", true); }
        }


        public bool plancreportvisible{
            get { return thema.Parameters.get("rb_visibleC", true); }
        }

        public bool mainlockvisible{
            get { return thema.Parameters.get("fl_visibleA", true); }
        }

        public IInputTemplate planform{
            get { return plan; }
        }

        public bool usemainform{
            get { return main != null; }
        }

        public IInputTemplate mainform{
            get { return main; }
        }

        public bool useorgreport{
            get { return _dreport != null; }
        }

        public IReportDefinition orgreport{
            get { return _dreport; }
        }

        public bool useorgplanreport{
            get { return _dpreport != null; }
        }

        public IReportDefinition orgplanreport{
            get { return _dpreport; }
        }

        public bool usesvodreport{
            get { return _sreport != null; }
        }

        public bool usesvodplanreport{
            get { return _spreport != null; }
        }


        /// <summary>
        ///   //////////////
        /// </summary>
        public bool useorgplancreport{
            get { return _cpreport != null; }
        }

        public IReportDefinition orgplancreport{
            get { return _cpreport; }
        }


        public bool usesvodplancreport{
            get { return _creport != null; }
        }


        public bool useplancreport{
            get { return _cpreport != null; }
        }

        public IReportDefinition svodplancreport{
            get { return _creport; }
        }

        ///////////////////////////////


        public IReportDefinition svodreport{
            get { return _sreport; }
        }

        public bool useplanreport{
            get { return _dpreport != null; }
        }

        public IReportDefinition svodplanreport{
            get { return _spreport; }
        }

        public bool isvisible{
            get{
                if (myapp.roles.IsAdmin()){
                    return true;
                }
                if (invalidtarget){
                    return false;
                }

                return null != (main ?? plan ?? (object) _dreport ?? _dpreport ?? svodreport ?? svodplanreport);
            }
        }

        public bool ismainopen{
            get{
                if (!UseMainForm()){
					if(UseDefaultReport()) {
						return getreportstate(orgreport);
					}
                	return false;
                }
                return main.IsOpen;
            }
        }

    	private bool getreportstate(IReportDefinition report, string  statetocheck="0ISOPEN") {
			if (null == report) return false;
    		return
    			((ZetaReportDefinition) report).GetState(((Thema) thema).Object, ((Thema) thema).Year, ((Thema) thema).Period) ==
				statetocheck;
    	}

    	public bool isplanopen{
            get{
                if (!useplanform){
                    if(useplanreport) {
						return getreportstate(orgplanreport);
					}
                	return false;
                }
                return plan.IsOpen;
            }
        }

        public bool isplancopen{
            get{
                if (!useplancform){
					if (useplancreport)
					{
						return getreportstate(orgplancreport);
					}
                    return false;
                }
                return planc.IsOpen;
            }
        }

        public bool ismainchecked{
            get{
                if (!UseMainForm()){
					if (UseDefaultReport())
					{
						return getreportstate(orgreport,"0ISCHECKED");
					}
                    return false;
                }
                return main.IsChecked;
            }
        }

        public bool isplanchecked{
            get{
                if (!useplanform){
					if (useplanreport)
					{
						return getreportstate(orgplanreport,"0ISCHECKED");
					}
                    return false;
                }
                return plan.IsChecked;
            }
        }

        public bool isplancchecked{
            get{
                if (!useplancform){
					if (useplancreport)
					{
						return getreportstate(orgplancreport, "0ISCHECKED");
					}
                    return false;
                }
                return planc.IsChecked;
            }
        }


        public string openmaincommand{
            get{
                if (null == main){
                    return "comdiv.modal.alert('Вам не доступно управление формой ввода!');";
                }
                return string.Format("Zeta.form.open('{0}','{1}','{2}');", main.Code, thema.Code, main.Period);
            }
        }

        public string openmaintitle{
            get{
                if (null == main){
                    return "Форма ввода (недоступно)";
                }
                if (ismainopen){
                    return "Заполнить форму ввода";
                }
                return "Просмотреть форму ввода";
            }
        }

        public string lockmaincommand{
            get { return getLockCommand(usemainunderwrite, ismainopen, main); }
        }

        public string lockmaintitle{
            get{
                if (!usemainunderwrite){
                    return "Управление блокировкой (недоступно)";
                }
                if (ismainopen){
                    return "Блокировать форму ввода";
                }
                return "Снять блокировку";
            }
        }


        public string openplancommand{
            get{
                if (null == plan){
                    return "comdiv.modal.alert('Вам не доступно управление формой ввода плана!');";
                }
                return string.Format("Zeta.form.open('{0}','{1}','{2}');", plan.Code, thema.Code, plan.Period);
            }
        }

        public string openplantitle{
            get{
                if (null == plan){
                    return "Форма ввода плана (недоступно)";
                }
                if (isplanopen){
                    return "Заполнить форму ввода плана";
                }
                return "Просмотреть форму ввода плана";
            }
        }

        public string lockplancommand{
            get { return getLockCommand(useplanunderwrite, isplanopen, plan); }
        }

        public string lockplantitle{
            get{
                if (!useplanunderwrite){
                    return "Управление блокировкой плановой формы (недоступно)";
                }
                if (isplanopen){
                    return "Блокировать форму ввода плана";
                }
                return "Снять блокировку (форма плана)";
            }
        }

        /// <summary>
        ///   ////////////////////////////
        /// </summary>
        public string openplanccommand{
            get{
                if (null == planc){
                    return "comdiv.modal.alert('Вам не доступно управление формой ввода корректив плана!');";
                }
                return string.Format("Zeta.form.open('{0}','{1}','{2}');", planc.Code, thema.Code, planc.Period);
            }
        }

        public string openplanctitle{
            get{
                if (null == planc){
                    return "Форма ввода корректив (недоступно)";
                }
                if (isplancopen){
                    return "Заполнить форму ввода корректив";
                }
                return "Просмотреть форму ввода корректив";
            }
        }

        public string lockplanccommand{
            get { return getLockCommand(useplancunderwrite, isplancopen, planc); }
        }

        public string lockplanctitle{
            get{
                if (!useplancunderwrite){
                    return "Управление блокировкой формы корректив (недоступно)";
                }
                if (isplancopen){
                    return "Блокировать форму ввода корректив";
                }
                return "Снять блокировку (форма корректива)";
            }
        }


        ///////////////////////////////////


        public string orgreportcommand{
            get{
                if (!UseDefaultReport()){
                    return "comdiv.modal.alert('Вам не доступeн вызов стандартного отчета!');";
                }

                return "Zeta.report.open('" + _dreport.Code + "','" + thema.Code + "');";
            }
        }


        public string orgreporttitle{
            get{
                if (!UseDefaultReport()){
                    return "Базовый отчет (недоступно)";
                }

                return "Сформировать базовый отчет";
            }
        }


        public string orgplanreportcommand{
            get{
                if (!useorgplanreport){
                    return "comdiv.modal.alert('Вам не доступeн вызов стандартного планового отчета!');";
                }

                return "Zeta.report.open('" + _dpreport.Code + "','" + thema.Code + "');";
            }
        }


        public string orgplanreporttitle{
            get{
                if (!useorgplanreport){
                    return "Базовый плановый отчет (недоступно)";
                }

                return "Сформировать базовый плановый отчет";
            }
        }

        /// <summary>
        ///   /////
        /// </summary>
        public string orgplancreportcommand{
            get{
                if (!useorgplancreport){
                    return "comdiv.modal.alert('Вам не доступeн вызов стандартного отчета корректив!');";
                }

                return "Zeta.report.open('" + _cpreport.Code + "','" + thema.Code + "');";
            }
        }


        public string orgplancreporttitle{
            get{
                if (!useorgplancreport){
                    return "Отчет корректив (недоступно)";
                }

                return "Сформировать отчет корректив";
            }
        }


        /////


        public string helpcommand{
            get { return "zeta.workbench.loadthemadetails('" + thema.Code + "');"; }
        }

        public string helptitle{
            get{
                if (!usehelp){
                    return "Дополнительная информация (недоступно)";
                }

                return "Дополнительная информация";
            }
        }

        public bool usehelp{
            get { return _help != null; }
        }


        public string svodreportcommand{
            get{
                if (!UseSvodReport()){
                    return "comdiv.modal.alert('Вам не доступeн вызов сводного отчета!');";
                }

                return "Zeta.report.prepare('" + _sreport.Code + "','" + thema.Code + "');";
            }
        }

        public string svodreporttitle{
            get{
                if (!UseSvodReport()){
                    return "Сводный отчет (недоступно)";
                }

                return "Сформировать сводный отчет";
            }
        }


        /// <summary>
        ///   ////////////////////////
        /// </summary>
        public string svodplancreportcommand{
            get{
                if (!usesvodplancreport){
                    return "comdiv.modal.alert('Вам не доступeн вызов сводного планового отчета!');";
                }

                return "Zeta.report.prepare('" + _creport.Code + "','" + thema.Code + "');";
            }
        }

        public string svodplancreporttitle{
            get{
                if (!usesvodplancreport){
                    return "Сводный плановый отчет (недоступно)";
                }

                return "Сформировать сводный плановый отчет";
            }
        }


        ///////


        public bool usemainunderwrite{
            get{
                if (!UseMainForm()){
                    return false;
                }
                if (main.UnderwriteCode.noContent()){
                    return false;
                }
                if (main.UnderwriteRole.noContent()){
                    return myapp.roles.IsAdmin();
                }

                if (ismainopen){
                    var state = main.GetScheduleState();
                    if (!state.Date.isNull()){
                        var from = new DateTime(state.Date.Year, state.Date.Month, 1);
                        if (DateTime.Today < (from.AddDays(-10))){
                            return false;
                        }
                    }
                }

                return getUnderwriteByRoles(main);
            }
        }


        public string svodplanreportcommand{
            get{
                if (!usesvodplanreport){
                    return "comdiv.modal.alert('Вам не доступeн вызов сводного планового отчета!');";
                }

                return "Zeta.report.prepare('" + _spreport.Code + "','" + thema.Code + "');";
            }
        }

        public string svodplanreporttitle{
            get{
                if (!usesvodplanreport){
                    return "Сводный плановый отчет (недоступно)";
                }

                return "Сформировать сводный плановый отчет";
            }
        }

        public bool useplanunderwrite{
            get{
                if (!useplanform){
                    return false;
                }
                if (plan.UnderwriteCode.noContent()){
                    return false;
                }
                if (plan.UnderwriteRole.noContent()){
                    return myapp.roles.IsAdmin();
                }


                if (isplanopen){
                    var state = plan.GetScheduleState();
                    if (state.Date.Year < 2500){
                        var from = new DateTime(state.Date.Year, state.Date.Month, 1);
                        if (DateTime.Today < from){
                            return false;
                        }
                    }
                }

                var form = plan;
                return getUnderwriteByRoles(form);
            }
        }


        public bool useplancunderwrite{
            get{
                if (!useplancform){
                    return false;
                }
                if (planc.UnderwriteCode.noContent()){
                    return false;
                }
                if (planc.UnderwriteRole.noContent()){
                    return myapp.roles.IsAdmin();
                }


                if (isplancopen){
                    var state = planc.GetScheduleState();
                    if (state.Date.Year < 2500){
                        var from = new DateTime(state.Date.Year, state.Date.Month, 1);
                        if (DateTime.Today < from){
                            return false;
                        }
                    }
                }

                return getUnderwriteByRoles(planc);
            }
        }

        public bool invalidtarget{
            get { return _invtarget; }
        }

        public bool UseMainForm(){
            return main != null;
        }

        public bool UseDefaultReport(){
            return _dreport != null;
        }

        public bool UseSvodReport(){
            return _sreport != null;
        }

        private IInputTemplate GetMainForm(){
            return thema.GetForm(thema.Code) ?? thema.GetForm("A");
        }

        private string getLockCommand(bool useunderwrite, bool isopen, IInputTemplate target){
            if (!useunderwrite){
                return "comdiv.modal.alert('Вам не доступно управлений блокировкой формы ввода!');";
            }
            var message = "";
            if (isopen){
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
                if (deps.Count() == 0){
                    return string.Format("Zeta.form.lock('{0}',false,'{1}');", target.Code, target.Period);
                }
                else{
                    return
                        string.Format(
                            "if (confirm('Данное действие также откроет:\" {2} \", вы уверены?'))Zeta.form.lock('{0}',false,'{1}');",
                            target.Code
                            , target.Period, deps.Select(x => x.Name.Replace("'","\\'")).concat(", "));
                }
            //}
            //else{
             //   return "comdiv.modal.alert('На данный момент форму нельзя открыть (" + message.Replace("'", "&apos;") +
              //         ")!');";
            //}
        }

        private bool getUnderwriteByRoles(IInputTemplate form){
            if (myapp.roles.IsAdmin()){
                return true;
            }
            if (myapp.roles.IsInRole("HOLDUNDERWRITER")){
                return true;
            }
            if (!myapp.roles.IsInRole(form.UnderwriteRole)){
                return false;
            }
            var s = form.GetState(Object, null);
            if (s == "0ISOPEN"){
                return true;
            }
            if (s == "0ISBLOCK" && myapp.roles.IsInRole("DIVUNDERWRITER")){
                return true;
            }
            return false;
        }
    }
}