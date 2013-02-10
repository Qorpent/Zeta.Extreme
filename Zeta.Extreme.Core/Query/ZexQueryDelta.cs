#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : SubQueryDescriptor.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Описывает потенциальный подзапрос для оптимизации расчета сумм и простых формул
	/// </summary>
	public sealed class ZexQueryDelta {
		
		/// <summary>
		/// 	Применяет смещение к целевому запросу
		/// </summary>
		/// <param name="target"> </param>
		/// <returns> </returns>
		public ZexQuery Apply(ZexQuery target) {
			lock(this) {
				if (NoChanges(target)) {
					return target;
				}
				var result = target.Copy();
				MoveColumn(result);
				MoveRow(result);
				MoveObj(result);
				MoveTime(result);
				result.InvalidateCacheKey();
				return result;
			}
		}

		private void MoveTime(ZexQuery result) {
			if ((Year != 0 && Year != result.Time.Year) || (Period != 0 && Period != result.Time.Period)) {
				result.Time = result.Time.Copy();
				if (0 != Year && Year < 1900) {
					result.Time.Year += Year;
				}else if(Year!=result.Time.Year) {
					result.Time.Year = Year;
				}
				if (0 != Period) {
					if (0 < Period) {
						result.Time.Period = Period;
					}
					else {
						var eval = Periods.Eval(result.Time.Year, result.Time.Period, Period);
						result.Time.Year = eval.Year;
						result.Time.Period = eval.Periods[0];
					}
				}
			}
		}

		private void MoveColumn(ZexQuery result) {
			if (null != Column) {
				if (Column != result.Col.Native) {
					result.Col = new ColumnHandler {Native = Column};
				}
			}
			else if (!string.IsNullOrWhiteSpace(ColumCode)) {
				if (ColumCode != result.Col.Code) {
					result.Col = result.Col.Copy();
					if (null != result.Col.Native) {
						result.Col.Native = null;
					}
					result.Col.Code = ColumCode;
				}
			}
		}

		private void MoveRow(ZexQuery result) {
			if (null != Row) {
				if (Row != result.Row.Native) {
					result.Row = new RowHandler {Native = Row};
				}
			}
			else if (!string.IsNullOrWhiteSpace(RowCode)) {
				if (RowCode != result.Row.Code) {
					result.Row = result.Row.Copy();
					if (null != result.Row.Native) {
						result.Row.Native = null;
					}
					result.Row.Code = RowCode;
				}
			}
		}

		private void MoveObj(ZexQuery result) {
			if (null != Obj) {
				if (Obj != result.Obj.Native) {
					result.Obj = new ObjHandler {Native = Obj};
				}
			}
			else if (0 != ObjId) {
				if (ObjId != result.Obj.Id) {
					result.Obj = result.Obj.Copy();
					if (null != result.Obj.Native) {
						result.Obj.Native = null;
					}
					result.Obj.Id = ObjId;
				}
			}
		}


		private bool NoChanges(ZexQuery target) {
			if (0 > Period) {
				return false; //формульный период
			}
			if (0 != Year && Math.Abs(Year) < 1900) {
				return false; //смещение года
			}
			if (null != Column && Column != target.Col.Native) {
				return false;
			}
			if (null != Row && Row != target.Row.Native) {
				return false;
			}
			if (null != Obj && Obj != target.Obj.Native) {
				return false;
			}
			if (!string.IsNullOrWhiteSpace(ColumCode) && ColumCode != target.Col.Code) {
				return false;
			}
			if (!string.IsNullOrWhiteSpace(RowCode) && RowCode != target.Row.Code) {
				return false;
			}
			if (0 != ObjId && ObjId != target.Obj.Id) {
				return false;
			}
			if (0 != Period && Period != target.Time.Period) {
				return false;
			}
			if (0 != Year && Year != target.Time.Year) {
				return false;
			}
			return true;
		}

		/// <summary>
		/// 	Смещение по колонке (код) или пустое значение для отсутствия смещения
		/// </summary>
		public string ColumCode;

		/// <summary>
		/// 	Смещение по колонке (прямое) или null
		/// </summary>
		public IZetaColumn Column;

		/// <summary>
		/// 	Множитель при расчете значений
		/// </summary>
		public decimal Multiplicator =1;

		/// <summary>
		/// 	Прямое смещение по объекту
		/// </summary>
		public IZetaMainObject Obj;

		/// <summary>
		/// 	Смещение по объекту
		/// </summary>
		public int ObjId;

		/// <summary>
		/// 	Прямое или дельтированное указание периода
		/// </summary>
		public int Period;

		/// <summary>
		/// 	Прямое укзание смещения строки или отсутствие
		/// </summary>
		public IZetaRow Row;

		/// <summary>
		/// 	Смещение по строке или пустое значение для отсутствия смещения
		/// </summary>
		public string RowCode;

		/// <summary>
		/// 	Прямое или дельтированое указание года
		/// </summary>
		public int Year;
	}
}