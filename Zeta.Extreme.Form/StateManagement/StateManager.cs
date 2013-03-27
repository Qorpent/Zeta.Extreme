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
// PROJECT ORIGIN: Zeta.Extreme.Form/StateManager.cs
#endregion
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Qorpent.Applications;
using Qorpent.IoC;
using Zeta.Extreme.BizProcess.StateManagement;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Form.InputTemplates;
using Zeta.Extreme.Form.Themas;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.Form.StateManagement {
	/// <summary>
	/// 	Стандартный менеджер статусов
	/// </summary>
	public class StateManager : IStateManager {
		private static StateManager _default;


		/// <summary>
		/// 	Инстанция по умолчанию
		/// </summary>
		public static IStateManager Default {
			get { return _default ?? (_default = new StateManager()); }
		}

		/// <summary>
		/// 	Поставщик фабики тем
		/// </summary>
		public IThemaFactoryProvider FactoryProvider { get; set; }

		/// <summary>
		/// 	Обратная ссылка на контейнер
		/// </summary>
		public IContainer Container {
			get { return _container ?? (_container = Application.Current.Container); }
			set { _container = value; }
		}


		/// <summary>
		/// 	Выполнить установку статуса
		/// </summary>
		/// <param name="objid"> </param>
		/// <param name="year"> </param>
		/// <param name="period"> </param>
		/// <param name="template"> </param>
		/// <param name="templatecode"> </param>
		/// <param name="usr"> </param>
		/// <param name="state"> </param>
		/// <param name="comment"> </param>
		/// <param name="parent"> </param>
		/// <returns> </returns>
		public int DoSet(int objid, int year, int period, string template, string templatecode, string usr, string state,
		                 string comment,
		                 int parent) {
			using (var c = GetConnection()) {
				c.WellOpen();
				var result = c.ExecuteScalar<int>(
					@"exec usm.set_state 
                        @obj=@obj,
                        @year=@year,
                        @period=@period,
                        @template=@template,
                        @templatecode=@templatecode,
                        @state=@state,
                        @comment=@comment,
                        @usr=@usr,
                        @parent=@parent",
					new Dictionary<string, object>
						{
							{"@obj", objid},
							{"@year", year},
							{"@period", period},
							{"@template", template},
							{"@templatecode", templatecode},
							{"@state", state},
							{"@comment", comment},
							{"@parent", parent},
							{"@usr", usr},
						});
				return result;
			}
		}

		/// <summary>
		/// 	Выполнить установку статуса
		/// </summary>
		/// <param name="objid"> </param>
		/// <param name="year"> </param>
		/// <param name="period"> </param>
		/// <param name="template"> </param>
		/// <returns> </returns>
		public string DoGet(int objid, int year, int period, string template) {
			using (var c = GetConnection()) {
				c.WellOpen();
				var result = c.ExecuteScalar<string>(
					@"exec usm.get_state 
                        @obj=@obj,
                        @year=@year,
                        @period=@period,
                        @template=@template
                       ",
					new Dictionary<string, object>
						{
							{"@obj", objid},
							{"@year", year},
							{"@period", period},
							{"@template", template},
						});
				return result;
			}
		}

		/// <summary>
		/// 	Найти зависимые формы
		/// </summary>
		/// <param name="source"> </param>
		/// <returns> </returns>
		public IInputTemplate[] GetDependentTemplates(IInputTemplate source) {
			lock (this) {
				checkinit();
				return dependences.Where(x => x.Attr("source") == source.Code)
					.Select(x =>
						{
							var result = FactoryProvider.Get().GetForm(x.Attr("target"));
							if (null == result) {
								throw new Exception("Отсутствует целевая форма " + x.Attr("target"));
							}
							return result.Clone();
						})
					.Where(x => isActualThema(source, x))
					.Select(x => update(x, source)).ToArray();
			}
		}

		/// <summary>
		/// 	Получить статус периода
		/// </summary>
		/// <param name="year"> </param>
		/// <param name="period"> </param>
		/// <returns> </returns>
		public int GetPeriodState(int year, int period) {
			return new PeriodStateManager().Get(year, period).State ? 1 : 0;
		}

		/// <summary>
		/// 	Найти формы от которых зависит текущая
		/// </summary>
		/// <param name="target"> </param>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		public IInputTemplate[] GetSourceTemplates(IInputTemplate target, IZetaMainObject obj) {
			lock (this) {
				checkinit();
				return dependences.Where(x => x.Attr("target") == target.Code)
					.Where(x => x.Attr("group", "").IsEmpty() || null == obj || obj.GroupCache.Contains(x.Attr("group")))
					.Select(x =>
						{
							var result = FactoryProvider.Get().GetForm(x.Attr("source"), true);
							if (null == result) {
								throw new Exception("Не могу найти исходную форму для проверки блокировок - " +
								                    x.Attr("source"));
							}
							return result.Clone();
						}
					)
					.Where(x => isActualThema(target, x))
					.Select(x => update(x, target)).ToArray();
			}
		}


		/// <summary>
		/// 	Определить главную форму
		/// </summary>
		/// <param name="safer"> </param>
		/// <returns> </returns>
		public IInputTemplate GetMainTemplate(IInputTemplate safer) {
			lock (this) {
				checkinit();
				var maine = safers.FirstOrDefault(x => x.Attr("safer") == safer.Code);
				if (null == maine) {
					return null;
				}
				var maincode = maine.Attr("main");
				return update(FactoryProvider.Get().GetForm(maincode).Clone(), safer);
			}
		}

		/// <summary>
		/// 	Определеить форму-сейфер
		/// </summary>
		/// <param name="main"> </param>
		/// <returns> </returns>
		public IInputTemplate GetSaferTemplate(IInputTemplate main) {
			lock (this) {
				checkinit();
				var safere = safers.FirstOrDefault(x => x.Attr("main") == main.Code);
				if (null == safere) {
					return null;
				}
				var safercode = safere.Attr("safer");
				return update(FactoryProvider.Get().GetForm(safercode).Clone(), main);
			}
		}

		/// <summary>
		/// 	Признак возможности установить статус
		/// </summary>
		/// <param name="template"> </param>
		/// <param name="obj"> </param>
		/// <param name="detail"> </param>
		/// <param name="state"> </param>
		/// <returns> </returns>
		public bool CanSet(IInputTemplate template, IZetaMainObject obj, IZetaDetailObject detail, string state) {
			string cause;
			return CanSet(template, obj, detail, state, out cause);
		}

		/// <summary>
		/// 	Определяет возможность установить статус
		/// </summary>
		/// <param name="template"> </param>
		/// <param name="obj"> </param>
		/// <param name="detail"> </param>
		/// <param name="state"> </param>
		/// <param name="cause"> </param>
		/// <returns> </returns>
		public bool CanSet(IInputTemplate template, IZetaMainObject obj, IZetaDetailObject detail, string state,
		                   out string cause) {
			return CanSet(template, obj, detail, state, out cause, 0);
		}

		/// <summary>
		/// 	Еще один вариант проверки возможности установки статуса
		/// </summary>
		/// <param name="template"> </param>
		/// <param name="obj"> </param>
		/// <param name="detail"> </param>
		/// <param name="state"> </param>
		/// <param name="cause"> </param>
		/// <param name="parent"> </param>
		/// <returns> </returns>
		public bool CanSet(IInputTemplate template, IZetaMainObject obj, IZetaDetailObject detail, string state,
		                   out string cause, int parent) {
			lock (this) {
				checkinit();
				cause = "";
				var cpavoid = false;
				if (!template.IsActualOnYear) {
					return state == "0ISBLOCK";
				}
				if (parent == 0) {
					var denyrules = GetMathchedRules(template, state, obj).Where(x => x.Type == "deny");
					if (null != denyrules.FirstOrDefault()) {
						cause = "Специальное правило запрещает подобное действие";
						return false;
					}
				}
				if (!template.IgnorePeriodState) {
					var periodstate = GetPeriodState(template.Year, template.Period);
					if (periodstate == 0 && state == "0ISOPEN") {
						cause = "Период " + Periods.Get(template.Period).Name + " " + template.Year + "г. закрыт для правки";
						return false;
					}
				}
				if (state == "0ISBLOCK") {
					var checkperiod = perioddependency.SafeGet(template.Period);
					if (checkperiod != 0) {
						var t2 = template.Clone();
						t2.Period = checkperiod;
						var s = t2.GetState(obj, detail);
						if (s == "0ISOPEN") {
							cause = "зависимость от статусов других периодов данной формы";
							return false;
						}
					}

					if ((template.Rows.Count) == 1 && (template.Rows[0].Code != "STUB")) {
						var root = template.Rows[0];
						string cp;
						var controlpointResult = checkByControlPoints(template, obj, detail, root, out cp);
						if (!controlpointResult) {
							cause = "контрольные точки не сходятся " + cp;
							return false;
						}
						if (cp == "cpavoid") {
							cpavoid = true;
						}
					}
				}

				if (state == "0ISBLOCK" && template.NeedFiles.IsNotEmpty()) {
					var validperiod = true;
					if (template.NeedFilesPeriods.IsNotEmpty()) {
						var needperiods = template.NeedFilesPeriods.SmartSplit().Select(x => x.ToInt()).ToList();
						if (!needperiods.Contains(template.Period)) {
							validperiod = false;
						}
					}
					if (validperiod) {
						new InputTemplateRequest
							{
								Template = template,
								TemplateCode = template.Code,
								Year = template.Year,
								Period = template.Period,
								Object = obj,
								ObjectId = obj.Id,
							/*	Detail = detail,
								DetailId = detail.Id,*/
								Date = Qorpent.QorpentConst.Date.Begin
							};

						/*
						 * var pkg = req.GetDefaultPkg();
						var files =
							Application.Current.Container.Get<IDbfsRepository>().Search(new Dictionary<string, string>
																	{{"pkg", pkg.Id.ToString()}});
						 * 
                        var tags = template.NeedFiles.split();
                        var proceed = true;
                        var badtag = "";
                        foreach (var x in tags){
                            var file = files.FirstOrDefault(f => TagHelper.Value(f.Tag, "doctype") == x);
                            if (null == file){
                                proceed = false;
                                badtag = x;
                                break;
                            }
                        }
                        if (!proceed){
                            cause = "не хватает файла с меткой " + badtag;
                            return false;
                        }*/
						//файлы пока не портируем
					}
				}


				var masters = GetSourceTemplates(template, obj);
				var fs = GetFinalTemplates(template);
				var safer = GetSaferTemplate(template);
				var main = GetMainTemplate(template);

				/* правило 1 - сейфер не может быть открыт если главка не проверена */
				if (main != null && state == "0ISOPEN" && main.GetState(obj, detail) != "0ISCHECKED") {
					cause = "защищенная форма не может быть открыта при непроверенной основной";
					return false;
				}
				/* правило 2 - главка не может быть открыта, если сейыер не проверен */
				if (safer != null && state == "0ISOPEN" && safer.GetState(obj, detail) != "0ISCHECKED") {
					cause = "главная форма не может быть открыта если не проверена защищенная версия";
					return false;
				}

				var backcheckrules = GetMathchedRules(template, state, obj).Where(x => x.Type == "backcheck");
				if (null == backcheckrules.FirstOrDefault()) {
					/* правило 3 - дочка не может быть блокирована, если родитель открыт */
					if (masters.ToBool() && state == "0ISBLOCK") {
						foreach (var master in masters) {
							if (master.GetState(obj, detail) == "0ISOPEN") {
								cause = "нельзя блокировать, пока открыта '" + master.Name + "'";
								return false;
							}
						}
					}


					/* правило 4 - дочка не может быть проверена, пока мастер не проверен */
					if (masters.ToBool() && state == "0ISCHECKED") {
						foreach (var master in masters) {
							if (master.GetState(obj, detail) != "0ISCHECKED") {
								cause = "нельзя выставить статус проверено, пока не проверена '" + master.Name + "'";
								return false;
							}
						}
					}
				}


				/* правило 5 - нельзя открывать форму если утверждена финальная */
				if (fs.ToBool() && state == "0ISOPEN") {
					var periodmapper = Container.Get<ILockPeriodMapper>();
					var toperiods = new int[] {};
					if (null != periodmapper) {
						var op = LockOperation.None;
						if (state == "0ISBLOCK") {
							op = LockOperation.Block;
						}
						else if (state == "0ISOPEN") {
							op = LockOperation.Open;
						}
						toperiods = periodmapper.GetLockingPeriods(op, template.Period);
					}

					foreach (var f in fs) {
						if (f.GetState(obj, detail) == "0ISCHECKED") {
							cause = "нельзя открыть форму если утверждена финальная форма '" + f.Name + "'";
							return false;
						}
					}

					foreach (var toperiod in toperiods) {
						foreach (var f in fs) {
							var f_ = f.Clone();
							f_.Period = toperiod;
							if (f_.GetState(obj, detail) == "0ISCHECKED") {
								cause = "нельзя открыть форму если утверждена финальная форма '" + f_.Name + "' (" + toperiod + ")";
								return false;
							}
						}
					}
				}
				if (cpavoid) {
					cause = "cpavoid";
				}
				return true;
			}
		}


		/// <summary>
		/// 	ВЫполнить установку статуса
		/// </summary>
		/// <param name="template"> </param>
		/// <param name="obj"> </param>
		/// <param name="detail"> </param>
		/// <param name="state"> </param>
		/// <param name="parent"> </param>
		public void Process(IInputTemplate template, IZetaMainObject obj, IZetaDetailObject detail, string state, int parent) {
			/* на выполнение процесса у нас одно единственное правило - "если открывать, то с зависимыми*/
			lock (this) {
				checkinit();
				if (state == "0ISOPEN") {
					var deps = GetDependentTemplates(template);
					foreach (var dep in deps) {
						dep.SetState(obj, detail, state, false, parent);
					}
				}


				var copyrules = GetMathchedRules(template, state, obj).Where(x => x.Type == "copy");
				foreach (var rule in copyrules) {
					var target = FactoryProvider.Get().GetForm(rule.Target).Clone();
					target.Year = template.Year;
					target.Period = template.Period;
					target.SetState(obj, null, state, true, parent); //копирование статуса производится в обход проверки
				}
				if (state == "0ISCHECKED") {
					var backcheckrules = GetMathchedRules(template, state, obj).Where(x => x.Type == "backcheck");
					if (null != backcheckrules.FirstOrDefault()) {
						var sources = GetSourceTemplates(template, obj);
						foreach (var source in sources) {
							source.Year = template.Year;
							source.Period = template.Period;
							source.SetState(obj, null, state, true, parent);
						}
					}
				}
			}
		}

		private static IDbConnection GetConnection() {
			return Application.Current.DatabaseConnections.GetConnection("Default") ;
		}

		/// <summary>
		/// 	Получает кэш статусов
		/// </summary>
		/// <param name="obj"> </param>
		/// <param name="year"> </param>
		/// <param name="period"> </param>
		/// <returns> </returns>
		/// <exception cref="Exception"></exception>
		public IDictionary<string, object> GetStateCache(IZetaMainObject obj, int year, int period) {
			if (null == obj) {
				return new Dictionary<string, object>();
			}
			using (var c = GetConnection()) {
				c.WellOpen();
				if (null == c) {
					throw new Exception("connection is null");
				}
				var d = c.ExecuteDictionaryReader("exec usm.get_state_table @obj=@obj, @year=@year, @period = @period",
				                                  new Dictionary<string, object>
					                                  {{"@obj", obj.Id}, {"@year", year}, {"@period", period}}
					);
				return d;
			}
		}

		/// <summary>
		/// 	Простой вариант расчета форм - источников
		/// </summary>
		/// <param name="target"> </param>
		/// <returns> </returns>
		/// <exception cref="Exception"></exception>
		public IInputTemplate[] GetSourceTemplatesSimple(IInputTemplate target) {
			lock (this) {
				checkinit();
				return dependences.Where(x => x.Attr("target") == target.Code)
					.Select(x =>
						{
							var result = FactoryProvider.Get().GetForm(x.Attr("source"), true);
							if (null == result) {
								throw new Exception("Не могу найти исходную форму для проверки блокировок - " +
								                    x.Attr("source"));
							}
							return result.Clone();
						}
					)
					.Where(x => isActualThema(target, x))
					.ToArray();
			}
		}

		/// <summary>
		/// 	Получает набор входных первичных форм
		/// </summary>
		/// <param name="target"> </param>
		/// <returns> </returns>
		/// <exception cref="Exception"></exception>
		public IInputTemplate[] GetPrimaryTemplatesSimple(IInputTemplate target) {
			lock (this) {
				checkinit();
				return primaries.Where(x => x.Attr("result") == target.Code)
					.Select(x =>
						{
							var result = FactoryProvider.Get().GetForm(x.Attr("source"), true);
							if (null == result) {
								throw new Exception("Не могу найти исходную форму для проверки блокировок - " +
								                    x.Attr("source"));
							}
							return result.Clone();
						}
					)
					.Where(x => isActualThema(target, x))
					.ToArray();
			}
		}

		/// <summary>
		/// 	Получает формы - расширения
		/// </summary>
		/// <param name="target"> </param>
		/// <returns> </returns>
		/// <exception cref="Exception"></exception>
		public IInputTemplate[] GetExtensionTemplatesSimple(IInputTemplate target) {
			lock (this) {
				checkinit();
				return extensions.Where(x => x.Attr("source") == target.Code)
					.Select(x =>
						{
							var result = FactoryProvider.Get().GetForm(x.Attr("result"), true);
							if (null == result) {
								throw new Exception("Не могу найти исходную форму для проверки блокировок - " +
								                    x.Attr("source"));
							}
							return result.Clone();
						}
					)
					.Where(x => isActualThema(target, x))
					.ToArray();
			}
		}

		private bool isActualThema(IInputTemplate q, IInputTemplate t) {
			if (null == t.Thema) {
				return true;
			}
			var y = q.Year;
			if (y <= 1900) {
				y = DateTime.Today.Year;
			}
			var tsy = t.Thema.Parameters.SafeGet("beginActualYear", 1900);
			var tey = t.Thema.Parameters.SafeGet("endActualYear", 3000);
			return y >= tsy && y <= tey;
		}


		/// <summary>
		/// 	Получить формы - финализаторы
		/// </summary>
		/// <param name="source"> </param>
		/// <returns> </returns>
		/// <exception cref="Exception"></exception>
		public IInputTemplate[] GetFinalTemplates(IInputTemplate source) {
			lock (this) {
				checkinit();

				return finals.Where(x => x.Attr("source") == source.Code)
					.Select(x =>
						{
							var form = FactoryProvider.Get().GetForm(x.Attr("result"));
							if (form == null) {
								throw new Exception(string.Format("no {0}->{1}", source.Code, x.Attr("result")));
							}
							return form.Clone();
						})
					.Select(x => update(x, source)).ToArray();
			}
		}

		/// <summary>
		/// 	Обновляет целевлй шаблон из исходного
		/// </summary>
		/// <param name="target"> </param>
		/// <param name="src"> </param>
		/// <returns> </returns>
		private IInputTemplate update(IInputTemplate target, IInputTemplate src) {
			target.Year = src.Year;
			target.Period = src.Period;
			target.DirectDate = src.DirectDate;
			return target;
		}

// вот с какой радости это здесь

		/// <summary>
		/// 	Получить набор правил
		/// </summary>
		/// <param name="target"> </param>
		/// <param name="state"> </param>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		private StateRule[] GetMathchedRules(IInputTemplate target, string state, IZetaMainObject obj) {
			return rules.Where(x =>
			                   (x.ResultState.IsEmpty() || x.ResultState == state)
			                   && (x.CurrentState.IsEmpty() || x.CurrentState == target.GetState(obj, null))
			                   && (x.Current.IsEmpty() || x.Current + ".in" == target.Code)
				).ToArray();
		}

		private void checkinit() {
			lock (this) {
				if (!initialized) {
					//if (null == FactoryProvider) {
					//	FactoryProvider = Qorpent.Applications.Application.Current.Container.Get<IThemaFactoryProvider>(); // Application.Current.Container.Get<IThemaFactoryProvider>();
					//}
					var factory = Application.Current.Container.Get<IThemaFactory>("form.server.themas");
					var x = XElement.Parse(factory.SrcXml);
					safers = x.XPathSelectElements("//processes/safer").ToList();
					dependences = x.XPathSelectElements("//processes/dependency").ToList();
					extensions = x.XPathSelectElements("//processes/formextension").ToList();
					primaries = x.XPathSelectElements("//processes/primaryform").ToList();
					finals = x.XPathSelectElements("//processes/finalform").ToList();
					foreach (var item in x.XPathSelectElements("//processes/perioddependency")) {
						perioddependency[item.Attr("on").ToInt()] = item.Attr("need").ToInt();
					}
					rules = x.XPathSelectElements("//processes/rule").Select(e =>
						{
							var result = new StateRule();
							e.Apply(result);
							return result;
						}).ToList();

					initialized = true;
				}
			}
		}

		private static bool checkByControlPoints(IInputTemplate template, IZetaMainObject obj, IZetaDetailObject detail,
		                                         RowDescriptor root, out string cp) {
			cp = "";
#pragma warning disable 612,618
			var th = template.Thema as EcoThema;
#pragma warning restore 612,618
			if (null == th) {
				return true;
			}
			var firstyear = th.GetParameter("firstyear", 0);
			if (firstyear > template.Year) {
				return true;
			}
			var result = true;
			cp = "";

			var _controlpoint_session = template.AttachedSession as IFormSessionControlPointSource;
			if (null != _controlpoint_session) {
				var controlpoints = _controlpoint_session.ControlPoints;
				if (0 != controlpoints.Length) {
					foreach (var bp in controlpoints.Where(_ => _.Value != 0)) {
						cp += "Контрольная точка: " + bp.Row.Name + ", " + bp.Col.Title + "; ";
						result = false;
					}
				}
			}
			/*
			else {
				if ((root.Code != "STUB") && root.Target != null) {
					foreach (var check in RowCache.GetControlPoints(root.Target)) {
						if (!result) {
							break;
						}
						foreach (var col in template.GetAllColumns()) {
							if (col.GetIsVisible(obj) && col.ControlPoint) {
								cp = string.Format("({0},{1},{2},{3},{4}", check.Code, col.Code, obj.Id, col.Year,
								                   col.Period);
								var zone = new Zone(obj);
								var rd = new RowDescriptor(check);

								var val =
									new Comdiv.Zeta.Data.Minimal.Query(zone, rd, col, template.Thema).eval().toDecimal();
								if (val != 0) {
									result = false;
									break;
								}
							}
						}
					}
				}
			}*/
			if (Application.Current.Roles.IsInRole(Application.Current.Principal.CurrentUser, "SYS_NOCONTROLPOINTS", false)) {
				if (!result) {
					cp = "cpavoid";
				}
				return true;
			}
			return result;
		}

		private readonly IDictionary<int, int> perioddependency = new Dictionary<int, int>();
		private IContainer _container;
		private List<XElement> dependences;
		private List<XElement> extensions;
		private List<XElement> finals;
		private bool initialized;
		private List<XElement> primaries;
		private List<StateRule> rules;
		private List<XElement> safers;
	}
}