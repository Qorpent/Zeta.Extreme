#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : Column.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Comdiv.Extensions;

namespace Zeta.Forms {
	/// <summary>
	/// 
	/// </summary>
	public class Column : FormElement {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="form"></param>
		public Column(Form form) {
			Form = form;
			Cells = new List<Cell>();
			Form.Columns.Add(this);
			AggregateSources = new List<ColumnAggregatePosition>();
			AggregateType = "sum";
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsPrimary { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public bool IsAggregate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string AggregateType { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public IList<ColumnAggregatePosition> AggregateSources { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string ColumnCode { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int Year { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int Period { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Form Form { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public IList<Cell> Cells { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string MatrixId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string MatrixFormula { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string ForRows { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsNotSupported() {
			return !IsSupported();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsSupported() {
			return IsPrimary || IsAggregate;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="row"></param>
		/// <returns></returns>
		public bool IsMatch(Row row) {
			if (row.SelfColumnsOnly && ForRows.noContent()) {
				return false;
			}
			if (!row.SelfColumnsOnly && ForRows.noContent()) {
				return true;
			}
			return row.Code.like(ForRows);
		}
	}
}