using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comdiv.Application;
using Comdiv.Persistence;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;
using Qorpent.Applications;
using Qorpent.IoC;
using Zeta.Extreme.Form.Themas;

namespace Zeta.Extreme.FrontEnd
{
	/// <summary>
	/// Выполняет стартовую настройку сервера форм
	/// </summary>
	[ContainerComponent(Lifestyle.Transient,ServiceType = typeof(IApplicationStartup),Name = "extreme.form.start")]
	public class FormServer:IApplicationStartup
	{
		/// <summary>
		/// Инстанция по умолчанию
		/// </summary>
		public static FormServer Default { get; protected set; }
		/// <summary>
		/// Конструктор по умолчанию
		/// </summary>
		public FormServer() {
			if(null==Default) {
				Default = this;
				Sessions = new List<FormSession>();
			}else {
				_doNotRun = true;
			}
		}

		/// <summary>
		/// Список сессий
		/// </summary>
		public List<FormSession> Sessions { get; private set; } 

		/// <summary>
		/// Проверяет общее состояние загрузки
		/// </summary>
		public bool IsOk {
			get {
				return HibernateLoad.IsCompleted && MetaCacheLoad.IsCompleted && CompileFormulas.IsCompleted &&
				       LoadThemas.IsCompleted;
			}
		}

		private readonly bool _doNotRun;

		/// <summary>
		/// Корневая папка тем
		/// </summary>
		public string ThemaRootDirectory { get; set; }
		/// <summary>
		/// Обобщающая задача всей загрузки в целом
		/// </summary>
		public TaskWrapper ReadyToServeForms { get; set; }

		private Task GetCompileFormulasTask() {
			return new Task(()=>
				{
					FormulaStorage.Default.AutoBatchCompile = false;
					var _sumh = new StrongSumProvider();
					var formulas = RowCache.byid.Values.Where(_ => _.IsFormula && !_sumh.IsSum(_)).ToArray();

					foreach (var f in formulas)
					{
						var req = new FormulaRequest { Key = "row:" + f.Code, Formula = f.Formula, Language = f.FormulaEvaluator };
						FormulaStorage.Default.Register(req);

					}

					var colformulas = (
										  from c in myapp.storage.AsQueryable<col>()
										  where c.IsFormula && c.FormulaEvaluator == "boo" && null != c.Formula && "" != c.Formula
										  select new { c = c.Code, f = c.Formula ,tag=c.Tag}
									  ).ToArray();


					foreach (var c in colformulas)
					{
						var req = new FormulaRequest { Key = "col:" + c.c, Formula = c.f, Language = "boo", Tags = c.tag};
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
				    var _f = ((ExtremeFormProvider)FormProvider).Factory; //force reload
			});
		}

		/// <summary>
		/// Коллекция форм приложения
		/// </summary>
		public IExtremeFormProvider FormProvider { get; private set; }

		/// <summary>
		/// Процесс компиляции формул
		/// </summary>
		public TaskWrapper CompileFormulas { get; private  set; }
		/// <summary>
		/// Процесс загрузки тем
		/// </summary>
		public TaskWrapper LoadThemas { get; private set; }

		private Task GetMetaCacheLoadTask() {
			return new Task(()=>
				{
					var codes =
						myapp.storage.AsQueryable<row>().Where(_ => _.Tag.Contains("/extreme:1/")).Select(_ => _.Code).ToArray();
					RowCache.start(codes); 
					ColumnCache.start();
				});
		}

		/// <summary>
		/// Имя строки соединения с БД
		/// </summary>
		public string ConnectionName { get; set; }
		/// <summary>
		/// Процесс загрузки метаданных
		/// </summary>
		public TaskWrapper MetaCacheLoad { get; private set; }

		private Task GetHibernateTask() {
			return new Task(() =>
				{
					var connectionString = Application.Current.DatabaseConnections.GetConnectionString(ConnectionName);
					myapp.ioc.setupHibernate(new NamedConnection(ConnectionName, connectionString), new ZetaClassicModel());
				});
		}
		/// <summary>
		/// Процесс загрузки гиберейта
		/// </summary>
		public TaskWrapper HibernateLoad { get; private set; }

		private int _idx = -100;

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
			if(_doNotRun)return; //singleton imitation
			
			this.HibernateLoad = new TaskWrapper(GetHibernateTask());
			this.LoadThemas = new TaskWrapper(GetLoadThemasTask());
			this.MetaCacheLoad = new TaskWrapper(GetMetaCacheLoadTask(), HibernateLoad);
			this.CompileFormulas = new TaskWrapper(GetCompileFormulasTask(), MetaCacheLoad);
			this.ReadyToServeForms = new TaskWrapper(Task.FromResult(true), HibernateLoad, LoadThemas, MetaCacheLoad,
													 CompileFormulas);
			HibernateLoad.Run();
			MetaCacheLoad.Run();
			CompileFormulas.Run();
			LoadThemas.Run();
			ReadyToServeForms.Run();
		}
		

	}
}
