#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : StandardRowData.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Globalization;
using Qorpent;

namespace Zeta.Extreme.Poco.Inerfaces {
	public class StandardRowData {
		public StandardRowData() {
			DateValue = QorpentConst.Date.Begin;
			Multiplicator = 1;
		}

		public long IntValue { get; set; }
		public decimal DecimalValue { get; set; }
		public bool BoolValue { get; set; }
		public string StringValue { get; set; }
		public DateTime DateValue { get; set; }
		public decimal Multiplicator { get; set; }

		public object GetValue(IOlapColumn column) {
			if (null == column) {
				return null;
			}
			if (column.DataType == ValueDataType.Int) {
				return IntValue;
			}
			if (column.DataType == ValueDataType.Decimal) {
				return DecimalValue;
			}
			if (column.DataType == ValueDataType.Bool) {
				return BoolValue;
			}
			if (column.DataType == ValueDataType.Date) {
				return DateValue;
			}
			return StringValue;
		}

		public void SetValue(IOlapColumn column, object value) {
			if (null == column) {
				throw new OperationCanceledException("cannot setup value to cell before column is applyed");
			}
			StringValue = null;
			if (null != value) {
				StringValue = value.ToString();
			}
			DateValue = QorpentConst.Date.Begin;
			if (column.DataType == ValueDataType.Int) {
				IntValue = 0;
				if (null != value) {
					IntValue = Convert.ToInt64(value);
					DecimalValue = IntValue;
					BoolValue = 0 != IntValue;
				}
			}
			if (column.DataType == ValueDataType.Decimal) {
				DecimalValue = 0;
				if (null != value) {
					var strval = value.ToString().Replace(" ", "").Replace(",", ".");

					var val = value is decimal
						          ? (decimal) value
						          : Convert.ToDecimal(strval, CultureInfo.InvariantCulture);
					DecimalValue = val;
					try {
						IntValue = (int) Math.Round(DecimalValue);
					}
					catch (OverflowException) {
						IntValue = -1;
					}
					BoolValue = 0 != IntValue;
				}
			}
			if (column.DataType == ValueDataType.Int) {
				DecimalValue = 0;
				if (null != value) {
					var strval = value.ToString().Replace(" ", "").Replace(".", ",");

					var val = value is decimal
						          ? (decimal) value
						          : Convert.ToDecimal(strval, CultureInfo.InvariantCulture);
					DecimalValue = Math.Round(val);
					IntValue = (int) DecimalValue;
					BoolValue = 0 != IntValue;
				}
			}
			if (column.DataType == ValueDataType.Bool) {
				BoolValue = false;
				if (null != value) {
					BoolValue = Convert.ToBoolean(value);
					IntValue = BoolValue ? 1 : 0;
					DecimalValue = BoolValue ? 1 : 0;
				}
			}
			if (column.DataType == ValueDataType.Bool) {
				if (null != value) {
					DateValue = (value is DateTime) ? (DateTime) value : DateTime.Parse(value.ToString());
					if (DateValue < QorpentConst.Date.Begin) {
						DateValue = QorpentConst.Date.Begin;
					}
					IntValue = DateValue == QorpentConst.Date.Begin ? 0 : 1;
					DecimalValue = DateValue == QorpentConst.Date.Begin ? 0 : 1;
					BoolValue = DateValue > QorpentConst.Date.Begin;
				}
			}
		}
	}
}