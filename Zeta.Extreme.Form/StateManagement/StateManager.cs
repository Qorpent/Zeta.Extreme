﻿#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : StateManager.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Comdiv.Application;
using Comdiv.Common;
using Comdiv.Extensions;
using Comdiv.Inversion;
using Comdiv.Persistence;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;
using Zeta.Extreme.Form.InputTemplates;
using Zeta.Extreme.Form.Themas;

namespace Zeta.Extreme.Form.StateManagement {
	/// <summary>
	/// 	Стандартный менеджер статусов
	/// </summary>
	public class StateManager : IStateManager {
		/// <summary>
		/// </summary>
		public StateManager() : this(true) {}

		/// <summary>
		/// </summary>
		/// <param name="withreload"> </param>
		public StateManager(bool withreload) {
			myapp.OnReload += Reload;
		}

		/// <summary>
		/// 	Поставщик фабики тем
		/// </summary>
		public IThemaFactoryProvider FactoryProvider { get; set; }

		/// <summary>
		/// 	Обратная ссылка на контейнер
		/// </summary>
		public IInversionContainer Container {
			get {
				if (_container.invalid()) {
					lock (this) {
						if (_container.invalid()) {
							Container = ioc.Container;
						}
					}
				}
				return _container;
			}
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
			using (var c = myapp.ioc.getConnection()) {
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
			using (var c = myapp.ioc.getConnection()) {
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
				return dependences.Where(x => x.attr("source") == source.Code)
					.Select(x =>
						{
							var result = FactoryProvider.Get().GetForm(x.attr("target"));
							if (null == result) {
								throw new Exception("Отсутствует целевая форма " + x.attr("target"));
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
		/// 	Установить статус периода
		/// </summary>
		/// <param name="year"> </param>
		/// <param name="period"> </param>
		/// <param name="state"> </param>
		public void SetPeriodState(int year, int period, int state) {
			new PeriodStateManager().UpdateState(new PeriodStateRecord
				{Year = year, Period = period, State = state == 0 ? false : true});
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
				return dependences.Where(x => x.attr("target") == target.Code)
					.Where(x => x.attr("group", "").noContent() || null == obj || obj.GroupCache.Contains(x.attr("group")))
					.Select(x =>
						{
							var result = FactoryProvider.Get().GetForm(x.attr("source"), true);
							if (null == result) {
								throw new Exception("Не могу найти исходную форму для проверки блокировок - " +
								                    x.attr("source"));
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
				var maine = safers.FirstOrDefault(x => x.attr("safer") == safer.Code);
				if (null == maine) {
					return null;
				}
				var maincode = maine.attr("main");
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
				var safere = safers.FirstOrDefault(x => x.attr("main") == main.Code);
				if (null == safere) {
					return null;
				}
				var safercode = safere.attr("safer");
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
		public bool CanSet(InputTemplate template, IZetaMainObject obj, IZetaDetailObject detail, string state) {
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
		public bool CanSet(InputTemplate template, IZetaMainObject obj, IZetaDetailObject detail, string state,
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
		public bool CanSet(InputTemplate template, IZetaMainObject obj, IZetaDetailObject detail, string state,
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
						cause = "Период " + Periods.GetName(template.Period) + " " + template.Year + "г. закрыт для правки";
						return false;
					}
				}
				if (state == "0ISBLOCK") {
					var checkperiod = perioddependency.get(template.Period, defobj: 0);
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

				if (state == "0ISBLOCK" && template.NeedFiles.hasContent()) {
					var validperiod = true;
					if (template.NeedFilesPeriods.hasContent()) {
						var needperiods = template.NeedFilesPeriods.split().Select(x => x.toInt()).ToList();
						if (!needperiods.Contains(template.Period)) {
							validperiod = false;
						}
					}
					if (validperiod) {
						var req = new InputTemplateRequest();
						req.Template = template;
						req.TemplateCode = template.Code;
						req.Year = template.Year;
						req.Period = template.Period;
						req.Object = obj;
						req.ObjectId = obj.Id;
						req.Detail = detail;
						req.DetailId = detail.Id();
						req.Date = DateExtensions.Begin;

						/*
						 * var pkg = req.GetDefaultPkg();
						var files =
							myapp.ioc.get<IDbfsRepository>().Search(new Dictionary<string, string>
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
					if (masters.yes() && state == "0ISBLOCK") {
						foreach (var master in masters) {
							if (master.GetState(obj, detail) == "0ISOPEN") {
								cause = "нельзя блокировать, пока открыта '" + master.Name + "'";
								return false;
							}
						}
					}


					/* правило 4 - дочка не может быть проверена, пока мастер не проверен */
					if (masters.yes() && state == "0ISCHECKED") {
						foreach (var master in masters) {
							if (master.GetState(obj, detail) != "0ISCHECKED") {
								cause = "нельзя выставить статус проверено, пока не проверена '" + master.Name + "'";
								return false;
							}
						}
					}
				}


				/* правило 5 - нельзя открывать форму если утверждена финальная */
				if (fs.yes() && state == "0ISOPEN") {
					var periodmapper = Container.get<ILockPeriodMapper>();
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
		public void Process(InputTemplate template, IZetaMainObject obj, IZetaDetailObject detail, string state, int parent) {
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
			using (var c = myapp.ioc.getConnection()) {
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
				return dependences.Where(x => x.attr("target") == target.Code)
					.Select(x =>
						{
							var result = FactoryProvider.Get().GetForm(x.attr("source"), true);
							if (null == result) {
								throw new Exception("Не могу найти исходную форму для проверки блокировок - " +
								                    x.attr("source"));
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
				return primaries.Where(x => x.attr("result") == target.Code)
					.Select(x =>
						{
							var result = FactoryProvider.Get().GetForm(x.attr("source"), true);
							if (null == result) {
								throw new Exception("Не могу найти исходную форму для проверки блокировок - " +
								                    x.attr("source"));
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
				return extensions.Where(x => x.attr("source") == target.Code)
					.Select(x =>
						{
							var result = FactoryProvider.Get().GetForm(x.attr("result"), true);
							if (null == result) {
								throw new Exception("Не могу найти исходную форму для проверки блокировок - " +
								                    x.attr("source"));
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
			var tsy = t.Thema.Parameters.get("beginActualYear", 1900);
			var tey = t.Thema.Parameters.get("endActualYear", 3000);
			return y >= tsy && y <= tey;
		}


		private void Reload(object sender, EventWithDataArgs<int> args) {
			lock (this) {
				initialized = false;
			}
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

				return finals.Where(x => x.attr("source") == source.Code)
					.Select(x =>
						{
							var form = FactoryProvider.Get().GetForm(x.attr("result"));
							if (form == null) {
								throw new Exception(string.Format("no {0}->{1}", source.Code, x.attr("result")));
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
			                   (x.ResultState.noContent() || x.ResultState == state)
			                   && (x.CurrentState.noContent() || x.CurrentState == target.GetState(obj, null))
			                   && (x.Current.noContent() || x.Current + ".in" == target.Code)
				).ToArray();
		}

		private void checkinit() {
			lock (this) {
				if (!initialized) {
					if (null == FactoryProvider) {
						FactoryProvider = myapp.ioc.get<IThemaFactoryProvider>();
					}
					var factory = FactoryProvider.Get();
					var x = XElement.Parse(factory.SrcXml);
					safers = x.XPathSelectElements("//processes/safer").ToList();
					dependences = x.XPathSelectElements("//processes/dependency").ToList();
					extensions = x.XPathSelectElements("//processes/formextension").ToList();
					primaries = x.XPathSelectElements("//processes/primaryform").ToList();
					finals = x.XPathSelectElements("//processes/finalform").ToList();
					foreach (var item in x.XPathSelectElements("//processes/perioddependency")) {
						perioddependency[item.attr("on", 0)] = item.attr("need", 0);
					}
					rules = x.XPathSelectElements("//processes/rule").Select(e =>
						{
							var result = new StateRule();
							e.applyTo(result);
							return result;
						}).ToList();

					initialized = true;
				}
			}
		}

		private static bool checkByControlPoints(InputTemplate template, IZetaMainObject obj, IZetaDetailObject detail,
		                                         RowDescriptor root, out string cp) {
			cp = "";
			var th = template.Thema as EcoThema;
			if (null == th) {
				return true;
			}
			var firstyear = th.GetParameter("firstyear", 0);
			if (firstyear > template.Year) {
				return true;
			}
			var result = true;
			cp = "";
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
			if (myapp.roles.IsInRole(myapp.usr, "SYS_NOCONTROLPOINTS", false)) {
				if (!result) {
					cp = "cpavoid";
				}
				return true;
			}
			return result;
		}

		private readonly IDictionary<int, int> perioddependency = new Dictionary<int, int>();
		private IInversionContainer _container;
		private List<XElement> dependences;
		private List<XElement> extensions;
		private List<XElement> finals;
		private bool initialized;
		private List<XElement> primaries;
		private List<StateRule> rules;
		private List<XElement> safers;
	}
}