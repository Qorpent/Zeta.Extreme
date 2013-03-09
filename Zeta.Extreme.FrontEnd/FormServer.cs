#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : FormServer.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Qorpent;
using Qorpent.Applications;
using Qorpent.Events;
using Qorpent.IoC;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Form.SaveSupport;
using Zeta.Extreme.Form.Themas;
using Zeta.Extreme.Poco.Inerfaces;
using Zeta.Extreme.Poco.NativeSqlBind;

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
			get { return MetaCacheLoad.IsCompleted && CompileFormulas.IsCompleted && LoadThemas.IsCompleted; }
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
			ReadyToServeForms = new TaskWrapper(Task.FromResult(true), LoadThemas, MetaCacheLoad,
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
			if (0 != Sessions.Count) {
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
							compiletime = _formulaRegisterTime
						},
					themas = new
						{
							time = LoadThemas.ExecuteTime,
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
		/// <param name="initsavemode"> пред-открытие для сохранения </param>
		/// <returns> </returns>
		public FormSession Start(IInputTemplate template, IZetaMainObject obj, int year, int period, bool initsavemode = false) {
			lock (this) {
				var usr = Application.Principal.CurrentUser.Identity.Name;
				var existed =
					Sessions.FirstOrDefault(
						_ =>
						_.Usr == usr && _.Year == year && _.Period == period && _.Template.Code == template.Code && _.Object.Id == obj.Id);
				if (null == existed) {
					var session = new FormSession(template, year, period, obj);
					session.FormServer = this;
					session.InitSaveMode = initsavemode;
					Sessions.Add(session);
					session.Start();
					return session;
				}
				else {
					existed.Activations++;
					if (!initsavemode) {
						if (!existed.IsStarted) {
							existed.Start();
						}
						else {
							if (existed.IsFinished) {
								existed.Error = null;
								existed.RestartData();
							}
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
			((IResetable) (Application.Files).GetResolver()).Reset(null);
			((IResetable) Application.Roles).Reset(null);
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
					var _sw = Stopwatch.StartNew();
					FormulaStorage.Default.AutoBatchCompile = false;
					FormulaStorage.Default.Clear();
					var tmp = Application.Files.Resolve("~/.tmp/formula_dll", false);
					FormulaStorage.LoadDefaultFormulas(tmp);


					FormulaStorage.Default.AutoBatchCompile = true;
					_sw.Stop();
					_formulaRegisterTime = _sw.Elapsed;
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
					ColumnCache.Start();
					ObjCache.Start();
				});
		}


		private readonly bool _doNotRun;
		private TimeSpan _formulaRegisterTime;
		private int _idx = -100;
	}
}