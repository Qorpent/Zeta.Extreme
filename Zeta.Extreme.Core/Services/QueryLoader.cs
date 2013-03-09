#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : QueryLoader.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Препроцессор до загрузки по умолчанию
	/// </summary>
	public class QueryLoader : IPreloadProcessor {
		/// <summary>
		/// 	Конструктор по умолчнию в привязке к сессии
		/// </summary>
		/// <param name="session"> </param>
		/// <exception cref="NotImplementedException"></exception>
		public QueryLoader(Session session) {
			_session = session;
			_sumh = new StrongSumProvider();
		}

		/// <summary>
		/// 	выполняет препроцессинг
		/// </summary>
		/// <param name="query"> </param>
		/// <returns> </returns>
		public virtual Query Process(Query query) {
			var internalquery = query.Copy(true);
			// внутри сессии работаем только с копиями
			// ибо иначе отконтроллировать изменения препроцессора по сути невозможно
			//сначала вызываем стандартную процедуру нормализации запроса
			internalquery.Normalize(_session);
			if (ChekoutCaption(internalquery)) {
				return null;
			}
			if (CheckoutObsolete(internalquery)) {
				return null;
			}
			PrepareFormulas(internalquery);
			return internalquery;
		}

		private void PrepareFormulas(Query internalquery) {
			if (internalquery.Row.IsFormula && !_sumh.IsSum(internalquery.Row)) {
				var key = "row:" + internalquery.Row.Code;
				if (null == internalquery.Row.Native) {
					key = "dynrow:" + internalquery.Row.Formula;
				}
				if (!FormulaStorage.Default.Exists(key)) {
					FormulaStorage.Default.Register(new FormulaRequest
						{
							Key = key,
							Formula = internalquery.Row.Formula,
							Language = internalquery.Row.FormulaType,
							Tags = internalquery.Row.Tag,
							Marks = internalquery.Row.Native == null ? "" : internalquery.Row.Native.MarkCache
						});
				}
			}

			if (internalquery.Col.IsFormula && !_sumh.IsSum(internalquery.Col)) {
				var key = "col:" + internalquery.Col.Code;
				if (null == internalquery.Col.Native) {
					key = "dyncol:" + internalquery.Col.Formula;
				}
				if (!FormulaStorage.Default.Exists(key)) {
					FormulaStorage.Default.Register(new FormulaRequest
						{
							Key = key,
							Formula = internalquery.Col.Formula,
							Language = internalquery.Col.FormulaType,
							Tags = internalquery.Col.Tag,
							Marks = internalquery.Col.Native == null ? "" : internalquery.Col.Native.MarkCache
						});
				}
			}

			if (internalquery.Obj.IsFormula && !_sumh.IsSum(internalquery.Obj)) {
				var key = "obj:" + internalquery.Row.Code;
				if (null == internalquery.Obj.Native) {
					key = "dynobj:" + internalquery.Obj.Formula;
				}
				if (!FormulaStorage.Default.Exists(key)) {
					FormulaStorage.Default.Register(new FormulaRequest
						{
							Key = key,
							Formula = internalquery.Obj.Formula,
							Language = internalquery.Obj.FormulaType,
							Tags = internalquery.Obj.Tag
						});
				}
			}
		}

		private static bool CheckoutObsolete(Query internalquery) {
			var obsolete = TagHelper.Value(internalquery.Row.Tag, "obsolete").ToInt();
			if (obsolete != 0) {
				if (obsolete <= internalquery.Time.Year) {
					return true;
				}
			} // устаревшие строки не могут быть целью
			return false;
		}

		private static bool ChekoutCaption(Query internalquery) {
			if (internalquery.Row.Native != null && internalquery.Row.Native.IsMarkSeted("0CAPTION")) {
				return true;
			}
			return false;
		}


		private readonly StrongSumProvider _sumh;

		/// <summary>
		/// 	обратная ссылка на сессию
		/// </summary>
		protected Session _session;
	}
}