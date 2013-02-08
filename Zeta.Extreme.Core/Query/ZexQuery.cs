#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : ZexQuery.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using System.Text;

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
		/// Конструктор запроса по умолчанию
		/// </summary>
		public ZexQuery() {
			Time = new TimeHandler();
			Row = new RowHandler();
			Col = new ColumnHandler();
			Obj =new ZoneHandler();
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
		public ZoneHandler Obj { get; set; }

		/// <summary>
		/// 	Выходная валюта
		/// </summary>
		public string Valuta { get; set; }

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
		/// 	Модификатор кэш-строки (префикс)
		/// </summary>
		public string CustomHashPrefix;

		private IList<ZexQuery> _children;

		/// <summary>
		/// Простая копия условия на время
		/// </summary>
		/// <param name="deep">Если да, то делает копии вложенных измерений</param>
		/// <returns></returns>
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
		/// Дочерние запросы
		/// </summary>
		public IList<ZexQuery> Children {
			get { return _children ?? (_children = new List<ZexQuery>()); }

		}

		/// <summary>
		/// Родительский запрос
		/// </summary>
		public ZexQuery Parent { get; set; }

		/// <summary>
		/// Обратная ссылка на сессию
		/// </summary>
		public ZexSession Session { get; set; }

		/// <summary>
		/// Стандартная процедура нормализации
		/// </summary>
		public void Normalize() {
			Time.Normalize(Session);
		}
	}
}