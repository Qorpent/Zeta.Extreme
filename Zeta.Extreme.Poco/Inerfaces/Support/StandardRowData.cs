// // Copyright 2007-2010 Comdiv (F. Sadykov) - http://code.google.com/u/fagim.sadykov/
// // Supported by Media Technology LTD 
// //  
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //  
// //      http://www.apache.org/licenses/LICENSE-2.0
// //  
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// // 
// // MODIFICATIONS HAVE BEEN MADE TO THIS FILE
using System;
using System.Globalization;
using Comdiv.Extensions;

namespace Comdiv.Olap.Model{
    public class StandardRowData{
        public StandardRowData(){
            DateValue = DateExtensions.Begin;
            Multiplicator = 1;
        }

        public long IntValue { get; set; }
        public decimal DecimalValue { get; set; }
        public bool BoolValue { get; set; }
        public string StringValue { get; set; }
        public DateTime DateValue { get; set; }
        public decimal Multiplicator { get; set; }

        public object GetValue(IOlapColumn column){
            if (null == column){
                return null;
            }
            if (column.DataType == ValueDataType.Int){
                return IntValue;
            }
            if (column.DataType == ValueDataType.Decimal){
                return DecimalValue;
            }
            if (column.DataType == ValueDataType.Bool){
                return BoolValue;
            }
            if (column.DataType == ValueDataType.Date){
                return DateValue;
            }
            return StringValue;
        }

        public void SetValue(IOlapColumn column, object value){
            if (null == column){
                throw new OperationCanceledException("cannot setup value to cell before column is applyed");
            }
            StringValue = null;
            if (null != value){
                StringValue = value.ToString();
            }
            DateValue = DateExtensions.Begin;
            if (column.DataType == ValueDataType.Int){
                IntValue = 0;
                if (null != value){
                    IntValue = Convert.ToInt64(value);
                    DecimalValue = IntValue;
                    BoolValue = 0 != IntValue;
                }
            }
            if (column.DataType == ValueDataType.Decimal){
                DecimalValue = 0;
                if (null != value){
                    var strval = value.ToString().Replace(" ", "").Replace(",", ".");

                    var val = value is decimal
                                  ? (decimal) value
                                  : Convert.ToDecimal(strval, CultureInfo.InvariantCulture);
                    DecimalValue = val;
					try {
						IntValue = (int) Math.Round(DecimalValue);
					}catch(OverflowException) {
						IntValue = -1;
					}
	                BoolValue = 0 != IntValue;
                }
            }
            if (column.DataType == ValueDataType.Int){
                DecimalValue = 0;
                if (null != value){
                    var strval = value.ToString().Replace(" ", "").Replace(".", ",");

                    var val = value is decimal
                                  ? (decimal) value
                                  : Convert.ToDecimal(strval, CultureInfo.InvariantCulture);
                    DecimalValue = Math.Round(val);
                    IntValue = (int) DecimalValue;
                    BoolValue = 0 != IntValue;
                }
            }
            if (column.DataType == ValueDataType.Bool){
                BoolValue = false;
                if (null != value){
                    BoolValue = Convert.ToBoolean(value);
                    IntValue = BoolValue ? 1 : 0;
                    DecimalValue = BoolValue ? 1 : 0;
                }
            }
            if (column.DataType == ValueDataType.Bool){
                if (null != value){
                    DateValue = (value is DateTime) ? (DateTime) value : DateTime.Parse(value.ToString());
                    if (DateValue < DateExtensions.Begin){
                        DateValue = DateExtensions.Begin;
                    }
                    IntValue = DateValue == DateExtensions.Begin ? 0 : 1;
                    DecimalValue = DateValue == DateExtensions.Begin ? 0 : 1;
                    BoolValue = DateValue > DateExtensions.Begin;
                }
            }
        }
    }
}