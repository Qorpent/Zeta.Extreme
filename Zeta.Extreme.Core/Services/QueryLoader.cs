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
// PROJECT ORIGIN: Zeta.Extreme.Core/QueryLoader.cs
#endregion
using System;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Extensions;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

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
		public QueryLoader(ISession session) {
			_session = session;
			_sumh = new StrongSumProvider();
		}

		/// <summary>
		/// 	выполняет препроцессинг
		/// </summary>
		/// <param name="query"> </param>
		/// <returns> </returns>
		public virtual IQuery Process(IQuery query) {
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

		private void PrepareFormulas(IQuery internalquery) {
			PrepareFormulaForRows(internalquery);
			PrepareFormulaForColumn(internalquery);
			PrepareFormulaForObject(internalquery);
		}

		IFormulaStorage Formulas {
			get {
				if (_session is ISessionWithExtendedServices) {
					return (_session as ISessionWithExtendedServices).FormulaStorage ?? FormulaStorage.Default;
				}
				return FormulaStorage.Default;
			}
		}

		private void PrepareFormulaForObject(IQuery internalquery) {
			if (internalquery.Obj.IsFormula && !_sumh.IsSum(internalquery.Obj)) {
				var key = "obj:" + internalquery.Row.Code;
				if (null == internalquery.Obj.Native) {
					key = "dynobj:" + internalquery.Obj.Formula;
				}
				if (!Formulas.Exists(key)) {
					Formulas.Register(new FormulaRequest
						{
							Key = key,
							Formula = internalquery.Obj.Formula,
							Language = internalquery.Obj.FormulaType,
							Tags = internalquery.Obj.Tag
						});
				}
			}
		}

		private void PrepareFormulaForColumn(IQuery internalquery) {
			if (internalquery.Col.IsFormula && !_sumh.IsSum(internalquery.Col)) {
				var key = "col:" + internalquery.Col.Code;
				if (null == internalquery.Col.Native) {
					key = "dyncol:" + internalquery.Col.Formula;
				}
				if (!Formulas.Exists(key)) {
					Formulas.Register(new FormulaRequest
						{
							Key = key,
							Formula = internalquery.Col.Formula,
							Language = internalquery.Col.FormulaType,
							Tags = internalquery.Col.Tag,
							Marks = internalquery.Col.Native == null ? "" : internalquery.Col.Native.MarkCache
						});
				}
			}
		}

		private void PrepareFormulaForRows(IQuery internalquery) {
			if (internalquery.Row.IsFormula && !_sumh.IsSum(internalquery.Row)) {
				var key = "row:" + internalquery.Row.Code;
				if (null == internalquery.Row.Native) {
					key = "dynrow:" + internalquery.Row.Formula;
				}
				if (!Formulas.Exists(key)) {
					Formulas.Register(new FormulaRequest
						{
							Key = key,
							Formula = internalquery.Row.Formula,
							Language = internalquery.Row.FormulaType,
							Tags = internalquery.Row.Tag,
							Marks = internalquery.Row.Native == null ? "" : internalquery.Row.Native.MarkCache
						});
				}
			}
		}

		private static bool CheckoutObsolete(IQuery internalquery) {
			var obsolete = TagHelper.Value(internalquery.Row.Tag, "obsolete").ToInt();
			if (obsolete != 0) {
				if (obsolete <= internalquery.Time.Year) {
					return true;
				}
			} // устаревшие строки не могут быть целью
			return false;
		}

		private static bool ChekoutCaption(IQuery internalquery) {
			if (internalquery.Row.Native != null && internalquery.Row.Native.IsMarkSeted("0CAPTION")) {
				return true;
			}
			return false;
		}


		private readonly StrongSumProvider _sumh;

		/// <summary>
		/// 	обратная ссылка на сессию
		/// </summary>
		protected ISession _session;
	}
}