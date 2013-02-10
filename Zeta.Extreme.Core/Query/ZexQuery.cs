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
	public sealed class ZexQuery : CacheKeyGeneratorBase {
		/// <summary>
		/// 	Конструктор запроса по умолчанию
		/// </summary>
		public ZexQuery() {
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
		public IList<ZexQuery> Children {
			get { return _children ?? (_children = new List<ZexQuery>()); }
		}

		/// <summary>
		/// 	Родительский запрос
		/// </summary>
		public ZexQuery Parent { get; set; }

		/// <summary>
		/// 	Обратная ссылка на сессию
		/// </summary>
		public ZexSession Session { get; set; }

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
		/// Автоматический код запроса, присваиваемый системой
		/// </summary>
		public long UID { get; set; }

		/// <summary>
		/// Кэшированный запрос SQL
		/// </summary>
		public string SqlRequest { get; set; }

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
		public ZexQuery Copy(bool deep = false) {
			var result = (ZexQuery) MemberwiseClone();
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
		public void Normalize(ZexSession session = null) {
			var objt = Task.Run(() => Obj.Normalize(session ?? Session)); //объекты зачастую из БД догружаются
			Time.Normalize(session ?? Session);
			Col.Normalize(session ?? Session);
			var rowt = Task.Run(() => Row.Normalize(session ?? Session, Col.Native)); //тут формулы парсим простые как рефы			
			Task.WaitAll(objt, rowt);
		}

		/// <summary>
		/// 	Обеспечивает возврат результата запроса
		/// </summary>
		/// <returns> </returns>
		/// <exception cref="Exception"></exception>
		public QueryResult GetResult() {
			if (null != Result) {
				return Result;
			}
			if (null == GetResultTask) {
				throw new Exception("cannot retrieve result - no process or direct result attached");
			}
			if (GetResultTask.Status == TaskStatus.Created) {
				GetResultTask.Start();
			}
			if (GetResultTask.Status == TaskStatus.Faulted) {
				throw new Exception("cannot retrieve result - some problems int getresult task - faulted ", GetResultTask.Exception);
			}
			GetResultTask.Wait();
			if(null!=GetResultTask) { //некоторые задачи выставляют результат собственными средствами
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
		public ZexQuery ToRow(IZetaRow zetaRow, bool selfcopy = false, bool rowcopy = false) {
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
		/// 	Модификатор кэш-строки (префикс)
		/// </summary>
		public string CustomHashPrefix;

		private IList<ZexQuery> _children;

		/// <summary>
		/// Синхронизатор результата
		/// </summary>
		public void WaitResult() {

			if(null!=GetResultTask) {
				GetResultTask.Wait();
			}
		}
	}
}