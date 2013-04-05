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
// PROJECT ORIGIN: Zeta.Extreme.FrontEnd/FormServer.cs
#endregion
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Qorpent;
using Qorpent.Applications;
using Qorpent.Events;
using Qorpent.IoC;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Form.SaveSupport;
using Zeta.Extreme.Form.Themas;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.Querying;
using Zeta.Extreme.Model.SqlSupport;

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

		private MD5 md5 = MD5.Create();
		/// <summary>
		/// Возвращает ETag для кэша, не привязанного к пользователю
		/// </summary>
		/// <returns></returns>
		public string GetCommonETag() {
			return Convert.ToBase64String(md5.ComputeHash(GetETagBase(null)));
		}

		private byte[] GetETagBase(object context) {
			return Encoding.ASCII.GetBytes(LastRefreshTime.ToString()+context.ToStr());
		}

		/// <summary>
		/// Возвращает стандартное время для LastModified
		/// </summary>
		/// <returns></returns>
		public DateTime GetCommonLastModified() {
			return LastRefreshTime;
		}

		/// <summary>
		/// Возвращает ETag, связанный с пользователем
		/// </summary>
		/// <returns></returns>
		public string GetUserETag(IPrincipal user = null) {
			user = user ?? Application.Principal.CurrentUser;
			return Convert.ToBase64String(md5.ComputeHash(GetETagBase(user.Identity.Name)));
			
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
		public int Index {
			get { return _index; }
			set { _index = value; }
		}

		/// <summary>
		/// 	Executes some startup logic against given application
		/// </summary>
		/// <param name="application"> </param>
		public void Execute(IApplication application) {
			if (_doNotRun) {
				return; //singleton imitation
			}
			var container = Application.Container;
			Debug.Assert(null!=container.Get<IExtremeFactory>());
			LoadThemas = new TaskWrapper(GetLoadThemasTask());
			MetaCacheLoad = new TaskWrapper(GetMetaCacheLoadTask());
			CompileFormulas = new TaskWrapper(GetCompileFormulasTask(), MetaCacheLoad);
			ReadyToServeForms = new TaskWrapper(GetCompleteAllTask(), LoadThemas, MetaCacheLoad,
			                                    CompileFormulas);

			MetaCacheLoad.Run();
			CompileFormulas.Run();
			LoadThemas.Run();
			ReadyToServeForms.Run();
		}

		private Task<bool> GetCompleteAllTask() {
			return new Task<bool>(() =>
				{
					LastRefreshTime = DateTime.Now;
					return true;
				});

		}
		/// <summary>
		/// Время  последней глобальной очистки
		/// </summary>
		protected DateTime LastRefreshTime { get; set; }

		/// <summary>
		/// 	Возвращает инстанцию класса для сохранения данных
		/// </summary>
		/// <returns> </returns>
		public IFormSessionDataSaver GetSaver() {
			return ResolveService<IFormSessionDataSaver>() ?? new DefaultSessionDataSaver();
		}


		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		public object GetServerStateInfo() {
			var formulas = ExtremeFactory.GetFormulaStorage();
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
					lastrefresh = LastRefreshTime,
					reloadcount = ReloadCount,
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
								formulas.LastCompileError == null ? "" : formulas.LastCompileError.ToString(),
							formulacount = formulas.Count,
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
			lock (ReloadState) {
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
			lock (ReloadState) {
					((IResetable) (Application.Files).GetResolver()).Reset(null);
					((IResetable) Application.Roles).Reset(null);
					Sessions.Clear();

					LoadThemas = new TaskWrapper(GetLoadThemasTask());
					MetaCacheLoad = new TaskWrapper(GetMetaCacheLoadTask());
					CompileFormulas = new TaskWrapper(GetCompileFormulasTask(), MetaCacheLoad);
					ReadyToServeForms = new TaskWrapper(GetCompleteAllTask(),
					                                    LoadThemas,
					                                    MetaCacheLoad,
					                                    CompileFormulas);
					MetaCacheLoad.Run();
					CompileFormulas.Run();
					LoadThemas.Run();
					ReadyToServeForms.Run();
				
			
			}
		}
		/// <summary>
		/// Объект синхронизации с перезагрузкой
		/// </summary>
		public readonly object ReloadState = new object();

		/// <summary>
		/// Проверяет глобальный маркер очистки Zeta и производит перезапуск системы ( в синхронном режиме)
		/// </summary>
		public void CheckGlobalReload() {
			lock (ReloadState) {
				if (null != ReadyToServeForms && !ReadyToServeForms.IsCompleted) {
					ReadyToServeForms.Wait();
				}
				var lastrefresh = new NativeZetaReader().GetLastGlobalRefreshTime();
				if (lastrefresh > LastRefreshTime) {
					Sessions.Clear(); //иначе будет устаревшая структура
					Reload();
					ReadyToServeForms.Wait();
					LastRefreshTime = lastrefresh;
					ReloadCount ++;
				}
			}
		}

		/// <summary>
		/// Счетчик числа перезагрузок
		/// </summary>
		public int ReloadCount { get; set; }

		private Task GetCompileFormulasTask() {
			return new Task(() =>
				{
					var formulas = ExtremeFactory.GetFormulaStorage();
					var _sw = Stopwatch.StartNew();
					formulas.AutoBatchCompile = false;
					formulas.Clear();
					var tmp = Application.Files.Resolve("~/.tmp/formula_dll", false);
					formulas.LoadDefaultFormulas(tmp);
					formulas.AutoBatchCompile = true;
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
		private int _index = -100;
	}
}