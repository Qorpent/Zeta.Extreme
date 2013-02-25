#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : FormServer.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Comdiv.Application;
using Comdiv.Persistence;
using Comdiv.Zeta.Model;
using Qorpent;
using Qorpent.Applications;
using Qorpent.Events;
using Qorpent.IO;
using Qorpent.IoC;
using Qorpent.Security;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Form.InputTemplates;
using Zeta.Extreme.Form.SaveSupport;
using Zeta.Extreme.Form.Themas;
using Zeta.Extreme.Meta;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// 	Выполняет стартовую настройку сервера форм
	/// </summary>
	[ContainerComponent(Lifestyle.Transient, ServiceType = typeof (IApplicationStartup), Name = "extreme.form.start")]
	public class FormServer : ServiceBase, IApplicationStartup {
		/// <summary>
		/// 	Конструктор по умолчанию
		/// </summary>
		public FormServer() {
			if (null == Default) {
				Default = this;
				Sessions = new List<FormSession>();
			}
			else {
				_doNotRun = true;
			}
		}


		/// <summary>
		/// 	Инстанция по умолчанию
		/// </summary>
		public static FormServer Default { get; protected set; }

		/// <summary>
		/// 	Список сессий
		/// </summary>
		public List<FormSession> Sessions { get; private set; }

		/// <summary>
		/// 	Проверяет общее состояние загрузки
		/// </summary>
		public bool IsOk {
			get {
				return MetaCacheLoad.IsCompleted && CompileFormulas.IsCompleted && LoadThemas.IsCompleted;
			}
		}


		/// <summary>
		/// 	Корневая папка тем
		/// </summary>
		public string ThemaRootDirectory { get; set; }

		/// <summary>
		/// 	Обобщающая задача всей загрузки в целом
		/// </summary>
		public TaskWrapper ReadyToServeForms { get; set; }

		/// <summary>
		/// 	Коллекция форм приложения
		/// </summary>
		public IExtremeFormProvider FormProvider { get; private set; }

		/// <summary>
		/// 	Процесс компиляции формул
		/// </summary>
		public TaskWrapper CompileFormulas { get; private set; }

		/// <summary>
		/// 	Процесс загрузки тем
		/// </summary>
		public TaskWrapper LoadThemas { get; private set; }

		/// <summary>
		/// 	Имя строки соединения с БД
		/// </summary>
		public string ConnectionName { get; set; }

		/// <summary>
		/// 	Процесс загрузки метаданных
		/// </summary>
		public TaskWrapper MetaCacheLoad { get; private set; }



		/// <summary>
		/// 	An index of object
		/// </summary>
		public int Idx {
			get { return _idx; }
			set { _idx = value; }
		}

		/// <summary>
		/// 	Executes some startup logic against given application
		/// </summary>
		/// <param name="application"> </param>
		public void Execute(IApplication application) {
			if (_doNotRun) {
				return; //singleton imitation
			}

			LoadThemas = new TaskWrapper(GetLoadThemasTask());
			MetaCacheLoad = new TaskWrapper(GetMetaCacheLoadTask());
			CompileFormulas = new TaskWrapper(GetCompileFormulasTask(), MetaCacheLoad);
			ReadyToServeForms = new TaskWrapper(Task.FromResult(true),  LoadThemas, MetaCacheLoad,
			                                    CompileFormulas);

			MetaCacheLoad.Run();
			CompileFormulas.Run();
			LoadThemas.Run();
			ReadyToServeForms.Run();
		}

		/// <summary>
		/// 	Возвращает инстанцию класса для сохранения данных
		/// </summary>
		/// <returns> </returns>
		public IFormSessionDataSaver GetSaver() {
			return ResolveService<IFormSessionDataSaver>() ?? new DefaultSessionDataSaver();
		}

		/// <summary>
		/// 	Возвращает список форм
		/// </summary>
		/// <returns> </returns>
		public object GetFormList() {
			LoadThemas.Wait();
			return ((ExtremeFormProvider) FormProvider).Factory
				.GetAll().Where(_ => !_.Code.Contains("lib")).SelectMany(_ => _.GetAllForms())
				.Select(_ => new {code = _.Code, name = _.Name}).ToArray();
		}

		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		public object GetServerStateInfo() {
			object sessions = null;
			if(0!=Sessions.Count) {
				sessions = new
					{
						count = Sessions.Count,
						users = Sessions.Select(_ => _.Usr).Distinct().Count(),
						activations = Sessions.Select(_ => _.Activations).Sum(),
						uniqueforms =
							Sessions.Select(_ => new {y = _.Year, p = _.Period, o = _.Object.Id, f = _.Template.Code}).Distinct().Count(),
						totaldatatime = Sessions.Select(_ => _.OverallDataTime).Aggregate((a, x) => a + x),
						avgdatatime =
							TimeSpan.FromMilliseconds(Sessions.Select(_ => _.OverallDataTime).Aggregate((a, x) => a + x).TotalMilliseconds/
							                          Sessions.Select(_ => _.DataCollectionRequests).Sum()),
					};
			}
			return new
				{
					time = ReadyToServeForms.ExecuteTime,
					meta = new
						{
							status = MetaCacheLoad.Status,
							error = MetaCacheLoad.Error.ToStr(),
							rows = RowCache.Byid.Count,
							time = MetaCacheLoad.ExecuteTime,
						},
					formulas = new
						{
							status = CompileFormulas.Status,
							taskerror = CompileFormulas.Error.ToStr(),
							time = CompileFormulas.ExecuteTime,
							compileerror =
								FormulaStorage.Default.LastCompileError == null ? "" : FormulaStorage.Default.LastCompileError.ToString(),
							formulacount = FormulaStorage.Default.Count,
						},
					themas = new
						{
							time = CompileFormulas.ExecuteTime,
							status = Default.LoadThemas.Status,
							error = Default.LoadThemas.Error.ToStr(),
						},
					sessions
				};
		}

		/// <summary>
		/// 	Инициирует новую или возвращает имеющуюся сессию
		/// </summary>
		/// <param name="template"> </param>
		/// <param name="obj"> </param>
		/// <param name="year"> </param>
		/// <param name="period"> </param>
		/// <returns> </returns>
		public FormSession Start(IInputTemplate template, IZetaMainObject obj, int year, int period) {
			lock (this) {
				var usr = Application.Principal.CurrentUser.Identity.Name;
				var existed =
					Sessions.FirstOrDefault(
						_ =>
						_.Usr == usr && _.Year == year && _.Period == period && _.Template.Code == template.Code && _.Object.Id == obj.Id);
				if (null == existed) {
					var session = new FormSession(template, year, period, obj);
					session.FormServer = this;
					Sessions.Add(session);
					session.Start();
					return session;
				}
				else {
					existed.Activations++;

					if (!existed.IsStarted) {
						existed.Start();
					}
					else {
						if (existed.IsFinished) {
							existed.Error = null;
							existed.RestartData();
						}
					}
					return existed;
				}
			}
		}

		/// <summary>
		/// 	Перезагрузка системы
		/// </summary>
		public void Reload() {
			((IResetable) ( Application.Files).GetResolver()).Reset(null);
			((IResetable)Application.Roles).Reset(null);
			myapp.files.Reload();
			Sessions.Clear();

			LoadThemas = new TaskWrapper(GetLoadThemasTask());
			MetaCacheLoad = new TaskWrapper(GetMetaCacheLoadTask());
			CompileFormulas = new TaskWrapper(GetCompileFormulasTask(), MetaCacheLoad);
			ReadyToServeForms = new TaskWrapper(Task.FromResult(true),
			                                    LoadThemas,
			                                    MetaCacheLoad,
			                                    CompileFormulas);
			MetaCacheLoad.Run();
			CompileFormulas.Run();
			LoadThemas.Run();
			ReadyToServeForms.Run();
		}

		private Task GetCompileFormulasTask() {
			return new Task(() =>
				{
					FormulaStorage.Default.AutoBatchCompile = false;
					FormulaStorage.Default.Clear();
					var tmp = Application.Files.Resolve("~/.tmp/formula_dll", false);
					FormulaStorage.Default.BuildCache(tmp);
				


					var _sumh = new StrongSumProvider();
					var oldrowformulas = RowCache.Byid.Values.Where(
						_ => _.IsFormula && !_sumh.IsSum(_)
						     && _.ResolveTag("extreme") == "1"
							 && _.Version < DateTime.Today
						).ToArray();

					var newrowformulas = RowCache.Byid.Values.Where(
						_ => _.IsFormula && !_sumh.IsSum(_)
							 && _.ResolveTag("extreme") == "1"
							 && _.Version >= DateTime.Today
						).ToArray();

					

					var oldcolformulas = (
						                  from c in ColumnCache.Byid.Values//myapp.storage.AsQueryable<col>()
						                  where c.IsFormula
						                        && c.FormulaEvaluator == "boo" && !string.IsNullOrEmpty(c.Formula)
												&& c.Version < DateTime.Today
						                  select new {c = c.Code, f = c.Formula, tag = c.Tag, version=c.Version}
					                  ).ToArray();

					var newcolformulas = (
						                     from c in ColumnCache.Byid.Values //myapp.storage.AsQueryable<col>()
										  where c.IsFormula
												&& c.FormulaEvaluator == "boo" && !string.IsNullOrEmpty(c.Formula)
												&& c.Version >= DateTime.Today
										  select new { c = c.Code, f = c.Formula, tag = c.Tag, version=c.Version }
									  ).ToArray();

					foreach (var f in oldrowformulas)
					{
						var req = new FormulaRequest { Key = "row:" + f.Code, Formula = f.Formula, Language = f.FormulaEvaluator,Version = f.Version.ToString(CultureInfo.InvariantCulture)};
						FormulaStorage.Default.Register(req);
					}
					
					foreach (var c in oldcolformulas)
					{
						var req = new FormulaRequest { Key = "col:" + c.c, Formula = c.f, Language = "boo", Tags = c.tag, Version = c.version.ToString(CultureInfo.InvariantCulture) };
						FormulaStorage.Default.Register(req);
					}
					FormulaStorage.Default.CompileAll(tmp);


					foreach (var f in newrowformulas)
					{
						var req = new FormulaRequest { Key = "row:" + f.Code, Formula = f.Formula, Language = f.FormulaEvaluator ,Version = f.Version.ToString(CultureInfo.InvariantCulture) };
						FormulaStorage.Default.Register(req);
					}

					foreach (var c in newcolformulas)
					{
						var req = new FormulaRequest { Key = "col:" + c.c, Formula = c.f, Language = "boo", Tags = c.tag, Version = c.version.ToString(CultureInfo.InvariantCulture) };
						FormulaStorage.Default.Register(req);
					}
					FormulaStorage.Default.CompileAll(tmp);

					

					
					FormulaStorage.Default.AutoBatchCompile = true;
				});
		}


		private Task GetLoadThemasTask() {
			return new Task(() =>
				{
					//Debugger.Break();
					FormProvider = new ExtremeFormProvider(ThemaRootDirectory);
					var _f = ((ExtremeFormProvider) FormProvider).Factory; //force reload
					Application.Container.Register(new BasicComponentDefinition
						{
							Implementation = _f,
							ServiceType = typeof (IThemaFactory),
							Lifestyle = Lifestyle.Singleton,
							Name = "form.server.themas",
						});
				});
		}

		private Task GetMetaCacheLoadTask() {
			return new Task(() =>
				{
					Periods.Get(12);
					RowCache.start();
					ColumnCache.start();
					ObjCache.Start();
					
				});
		}

		
		private readonly bool _doNotRun;
		private int _idx = -100;
	}
}