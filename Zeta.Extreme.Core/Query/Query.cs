#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : ZexQuery.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Инкапсуляция запроса к Zeta
	/// </summary>
	/// <remarks>
	/// 	В обновленной версии не используется избыточных
	/// 	интерфейсов IQuery, IQueryBuilder, наоборот ZexQuery
	/// 	создан с учетом оптимизации и минимальной мутации
	/// </remarks>
	public sealed class Query : CacheKeyGeneratorBase {
		/// <summary>
		/// 	Конструктор запроса по умолчанию
		/// </summary>
		public Query() {
			Time = new TimeHandler();
			Row = new RowHandler();
			Col = new ColumnHandler();
			Obj = new ObjHandler();
			Valuta = "NONE";
		}

		/// <summary>
		/// 	Условие на время
		/// </summary>
		public TimeHandler Time { get; set; }

		/// <summary>
		/// 	Условие на строку
		/// </summary>
		public RowHandler Row { get; set; }

		/// <summary>
		/// 	Условие на колонку
		/// </summary>
		public ColumnHandler Col { get; set; }

		/// <summary>
		/// 	Условие на объект
		/// </summary>
		public ObjHandler Obj { get; set; }

		/// <summary>
		/// 	Выходная валюта
		/// </summary>
		public string Valuta { get; set; }

		/// <summary>
		/// 	Дочерние запросы
		/// </summary>
		public IList<Query> FormulaDependency {
			get { return _formulaDependency ?? (_formulaDependency = new List<Query>()); }
		}

		/// <summary>
		/// 	Обратная ссылка на сессию
		/// </summary>
		public Session Session { get; set; }

		/// <summary>
		/// 	Проверяет "первичность запроса"
		/// </summary>
		public bool IsPrimary {
			get { return Obj.IsPrimary() && Col.IsPrimary() && Row.IsPrimary(); }
		
		}

		/// <summary>
		/// 	Рабочий процесс получения результата
		/// </summary>
		public Task<QueryResult> GetResultTask { get; set; }

		/// <summary>
		/// 	Синхронный результат
		/// </summary>
		public QueryResult Result { get; set; }

		/// <summary>
		/// 	Проверяет готовность запроса к выполнению
		/// </summary>
		public bool IsNotPrepared {
			get { return null == Result && null == GetResultTask; }
		}

		/// <summary>
		/// 	Автоматический код запроса, присваиваемый системой
		/// </summary>
		public long UID;

		/// <summary>
		/// 	Кэшированный запрос SQL
		/// </summary>
		public string SqlRequest;

		/// <summary>
		/// 	Back-reference to preparation tasks
		/// </summary>
		public Task PrepareTask { get; set; }

		/// <summary>
		/// Client processed mark
		/// </summary>
		public bool Processed { get; set; }

		/// <summary>
		/// Зависимости для суммовых запросов
		/// </summary>
		public IList<Tuple<decimal, Query>> SummaDependency {
			get { return _summaDependency ?? (_summaDependency = new List<Tuple<decimal, Query>>()); }
		
		}


		/// <summary>
		/// Тип вычисления запроса
		/// </summary>
		public QueryEvaluationType EvaluationType;

		/// <summary>
		/// Формула, которая присоединяется к запросу на фазе подготовки
		/// </summary>
		public IFormula AssignedFormula;

		/// <summary>
		/// Sign that primary was not set
		/// </summary>
		public bool HavePrimary;

		/// <summary>
		/// 	Позволяет синхронизировать запросы в подсессиях
		/// </summary>
		/// <param name="timeout"> </param>
		public void WaitPrepare(int timeout=-1) {	
			while(null==PrepareTask) {
				Thread.Sleep(30);
			}
			if (PrepareTask != null) {
				if (!PrepareTask.IsCompleted) {
					if(timeout>0) {
						PrepareTask.Wait(timeout);
					}else {
						PrepareTask.Wait();
					}
				}
			}
		}


		/// <summary>
		/// 	Функция непосредственного вычисления кэшевой строки
		/// </summary>
		/// <returns> </returns>
		protected override string EvalCacheKey() {
			var sb = new StringBuilder();

			if (null != CustomHashPrefix) {
				sb.Append('/');
				sb.Append(CustomHashPrefix);
			}
			sb.Append('/');
			sb.Append(null == Obj ? "NOOBJ" : Obj.GetCacheKey());
			sb.Append('/');
			sb.Append(null == Row ? "NOROW" : Row.GetCacheKey());
			sb.Append('/');
			sb.Append(null == Col ? "NOCOL" : Col.GetCacheKey());
			sb.Append('/');
			sb.Append(null == Time ? "NOTIME" : Time.GetCacheKey());
			sb.Append('/');
			sb.Append(string.IsNullOrWhiteSpace(Valuta) ? "NOVAL" : "VAL:" + Valuta);

			return sb.ToString();
		}

		/// <summary>
		/// 	Простая копия условия на время
		/// </summary>
		/// <param name="deep"> Если да, то делает копии вложенных измерений </param>
		/// <returns> </returns>
		public Query Copy(bool deep = false) {
			var result = (Query) MemberwiseClone();
			result.PrepareTask = null;
			result.GetResultTask = null;
			result.Result = null;
			if(null!=TraceList) {
				result.TraceList = new List<string>();
			}
			if (deep) {
				result.Col = result.Col.Copy();
				result.Row = result.Row.Copy();
				result.Time = result.Time.Copy();
				result.Obj = result.Obj.Copy();
			}

			return result;
		}


		/// <summary>
		/// 	Стандартная процедура нормализации
		/// </summary>
		public void Normalize(Session session = null) {
			var objt = Task.Run(() => Obj.Normalize(session ?? Session)); //объекты зачастую из БД догружаются
			Time.Normalize(session ?? Session);
			Col.Normalize(session ?? Session);
			var rowt = Task.Run(() => Row.Normalize(session ?? Session, Col.Native)); //тут формулы парсим простые как рефы			
			Task.WaitAll(objt, rowt);
			InvalidateCacheKey();
		}

		/// <summary>
		/// 	Сбрасывает кэш-строку
		/// </summary>
		public override void InvalidateCacheKey()
		{
			base.InvalidateCacheKey();
			Row.InvalidateCacheKey();
			Col.InvalidateCacheKey();
			Time.InvalidateCacheKey();
			Obj.InvalidateCacheKey();
		}

		/// <summary>
		/// 	Обеспечивает возврат результата запроса
		/// </summary>
		/// <param name="timeout"> </param>
		/// <returns> </returns>
		/// <exception cref="Exception"></exception>
		public QueryResult GetResult(int timeout =-1) {
			WaitResult(timeout);
			if (null != Result) {
				return Result;
			}
			if (null == GetResultTask) {
				throw new Exception("cannot retrieve result - no process or direct result attached");
			}

			if (GetResultTask.Status == TaskStatus.Faulted) {
				throw new Exception("cannot retrieve result - some problems int getresult task - faulted ", GetResultTask.Exception);
			}

			if (null != GetResultTask) {
				//некоторые задачи выставляют результат собственными средствами
				Result = GetResultTask.Result;
			}
			return Result;
		}

		/// <summary>
		/// 	Переводит строку (по нативу)
		/// </summary>
		/// <param name="zetaRow"> </param>
		/// <param name="selfcopy"> </param>
		/// <param name="rowcopy"> </param>
		public Query ToRow(IZetaRow zetaRow, bool selfcopy = false, bool rowcopy = false) {
			var q = this;
			if (selfcopy) {
				q = Copy();
			}
			if (rowcopy || selfcopy) {
				q.Row = q.Row.Copy();
			}
			q.Row.Native = zetaRow;
			q.InvalidateCacheKey();
			return q;
		}

		/// <summary>
		/// 	Синхронизатор результата
		/// </summary>
		/// <param name="timeout"> </param>
		public void WaitResult(int timeout) {
			WaitPrepare(timeout);
			while(null == Result && null == GetResultTask) {
				Thread.Sleep(5);
			}
			if (null != GetResultTask) {
				if(GetResultTask.Status==TaskStatus.Created)
					if(IsPrimary) {
						Session.RunSqlBatch();
					}else {
						try {
							GetResultTask.Start();
						}
						catch {}
					}
				if(timeout>0) {
					GetResultTask.Wait(timeout);
				}else {
					GetResultTask.Wait();
				}
			}
		}

		/// <summary>
		/// 	Модификатор кэш-строки (префикс)
		/// </summary>
		public string CustomHashPrefix;

		private IList<Query> _formulaDependency;

		/// <summary>
		/// Реестр трассы
		/// </summary>
		public List<string> TraceList;

		private IList<Tuple<decimal, Query>> _summaDependency;
	}
}