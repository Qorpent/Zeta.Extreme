#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : FormServer.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comdiv.Application;
using Comdiv.Persistence;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;
using Qorpent;
using Qorpent.Applications;
using Qorpent.Events;
using Qorpent.IO;
using Qorpent.IoC;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Form.InputTemplates;
using Zeta.Extreme.Form.Themas;
using Zeta.Extreme.FrontEnd.Session;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// 	Выполняет стартовую настройку сервера форм
	/// </summary>
	[ContainerComponent(Lifestyle.Transient, ServiceType = typeof (IApplicationStartup), Name = "extreme.form.start")]
	public class FormServer : ServiceBase,IApplicationStartup
	{
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
		/// Возвращает список форм
		/// </summary>
		/// <returns></returns>
		public  object GetFormList()
		{
			LoadThemas.Wait();
			return ((ExtremeFormProvider)FormProvider).Factory
				.GetAll().Where(_ => !_.Code.Contains("lib")).SelectMany(_ => _.GetAllForms())
				.Select(_ => new { code = _.Code, name = _.Name }).ToArray();
		}
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		public object GetServerStateInfo() {
			
			return new
				{
					hibernate = new
						{
							status = HibernateLoad.Status,
							error = HibernateLoad.Error.ToStr()
						},
					meta = new
						{
							status =MetaCacheLoad.Status,
							error = MetaCacheLoad.Error.ToStr(),
							rows = RowCache.byid.Count,
						},
					formulas = new
						{
							status = CompileFormulas.Status,
							taskerror =CompileFormulas.Error.ToStr(),
							compileerror =
								FormulaStorage.Default.LastCompileError == null ? "" : FormulaStorage.Default.LastCompileError.ToString(),
							formulacount = FormulaStorage.Default.Count,
						},
					themas = new
						{
							status =Default.LoadThemas.Status,
							error = Default.LoadThemas.Error.ToStr(),
						},
				};
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
				return HibernateLoad.IsCompleted && MetaCacheLoad.IsCompleted && CompileFormulas.IsCompleted &&
				       LoadThemas.IsCompleted;
			}
		}


		/// <summary>
		/// Инициирует новую или возвращает имеющуюся сессию
		/// </summary>
		/// <param name="template"></param>
		/// <param name="obj"></param>
		/// <param name="year"></param>
		/// <param name="period"></param>
		/// <returns></returns>
		public FormSession Start (IInputTemplate template, IZetaMainObject obj,  int year, int period) {
			lock(this) {
				var usr = Application.Principal.CurrentUser.Identity.Name;
				var existed =
					Sessions.FirstOrDefault(
						_ =>
						_.Usr == usr && _.Year == year && _.Period == period && _.Template.Code == template.Code && _.Object.Id == obj.Id);
				if(null==existed) {
					var session = new FormSession(template, year, period, obj);
				
					Sessions.Add(session);
					session.Start();
					return session;
				}else {
					

					existed.Activations++;
					
					if (!existed.IsStarted)
					{
						existed.Start();
					}
					else
					{
						if (existed.IsFinished)
						{
							existed.Error = null;
							existed.StartCollectData();
						}
					}
					return existed;
				}

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
		/// 	Процесс загрузки гиберейта
		/// </summary>
		public TaskWrapper HibernateLoad { get; private set; }

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

			HibernateLoad = new TaskWrapper(GetHibernateTask());
			LoadThemas = new TaskWrapper(GetLoadThemasTask());
			MetaCacheLoad = new TaskWrapper(GetMetaCacheLoadTask(), HibernateLoad);
			CompileFormulas = new TaskWrapper(GetCompileFormulasTask(), MetaCacheLoad);
			ReadyToServeForms = new TaskWrapper(Task.FromResult(true), HibernateLoad, LoadThemas, MetaCacheLoad,
			                                    CompileFormulas);
			HibernateLoad.Run();
			MetaCacheLoad.Run();
			CompileFormulas.Run();
			LoadThemas.Run();
			ReadyToServeForms.Run();
		}

		/// <summary>
		/// 	Перезагрузка системы
		/// </summary>
		public void Reload() {
			((IResetable)((FileService) Application.Files).GetResolver()).Reset(null);
			LoadThemas = new TaskWrapper(GetLoadThemasTask());
			MetaCacheLoad = new TaskWrapper(GetMetaCacheLoadTask(), HibernateLoad);
			CompileFormulas = new TaskWrapper(GetCompileFormulasTask(), MetaCacheLoad);
			ReadyToServeForms = new TaskWrapper(Task.FromResult(true),
			                                    HibernateLoad,
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
					var _sumh = new StrongSumProvider();
					var formulas = RowCache.byid.Values.Where(
						_ => _.IsFormula && !_sumh.IsSum(_) 
						&& _.ResolveTag("extreme")=="1"
						).ToArray();

					foreach (var f in formulas) {
						var req = new FormulaRequest {Key = "row:" + f.Code, Formula = f.Formula, Language = f.FormulaEvaluator};
						FormulaStorage.Default.Register(req);
					}

					var colformulas = (
						                  from c in myapp.storage.AsQueryable<col>()
						                  where c.IsFormula 
										  && c.FormulaEvaluator == "boo" && null != c.Formula && "" != c.Formula
						                  select new {c = c.Code, f = c.Formula, tag = c.Tag}
					                  ).ToArray();


					foreach (var c in colformulas) {
						var req = new FormulaRequest {Key = "col:" + c.c, Formula = c.f, Language = "boo", Tags = c.tag};
						FormulaStorage.Default.Register(req);
					}
					FormulaStorage.Default.CompileAll();
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
							ServiceType = typeof(IThemaFactory),
							Lifestyle = Lifestyle.Singleton,
							Name = "form.server.themas",
						});
				});
		}

		private Task GetMetaCacheLoadTask() {
			return new Task(() =>
				{
					//var codes =
						//myapp.storage.AsQueryable<row>().Where(_ => _.Tag.Contains("/extreme:1/")).Select(_ => _.Code).ToArray();
					RowCache.start();
					ColumnCache.start();
				});
		}

		private Task GetHibernateTask() {
			return new Task(() =>
				{
					var connectionString = Application.DatabaseConnections.GetConnectionString(ConnectionName);
					myapp.ioc.setupHibernate(new NamedConnection(ConnectionName, connectionString), new ZetaClassicModel());
				});
		}

		private readonly bool _doNotRun;
		private int _idx = -100;
	}
}