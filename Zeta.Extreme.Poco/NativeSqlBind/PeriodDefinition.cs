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

#region

using System;
using System.Linq;
using Comdiv.Extensions;

#endregion

namespace Zeta.Extreme.Poco.NativeSqlBind{
	

	/// <summary>
	/// Определение периода (с учетом года и дат)
	/// </summary>
	public class PeriodDefinition {
		/// <summary>
		/// 
		/// </summary>
        public PeriodDefinition(){
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="year"></param>
		/// <param name="periods"></param>
        public PeriodDefinition(int year, params int[] periods){
            Year = year;
            Periods = periods.Where(p => p != -1).ToArray();
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="startDate"></param>
        public PeriodDefinition(DateTime startDate){
            StartDate = startDate;
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
        public PeriodDefinition(DateTime startDate, DateTime endDate){
            StartDate = startDate;
            EndDate = endDate;
        }
		/// <summary>
		/// Периоды
		/// </summary>
        public int[] Periods { get; set; }
		/// <summary>
		/// Год
		/// </summary>
        public int Year { get; set; }
		/// <summary>
		/// Начальная дата
		/// </summary>
        public DateTime StartDate { get; set; }
		/// <summary>
		/// Конечная дата
		/// </summary>
        public DateTime EndDate { get; set; }
		/// <summary>
		/// Имя периода
		/// </summary>
        public string PeriodName { get; set; }
		/// <summary>
		/// Признак определенности периода
		/// </summary>
		/// <returns></returns>
        public bool IsPeriodDefined(){
            if (null == Periods){
                return false;
            }
            if (0 == Periods.Length){
                return false;
            }
            if (1 == Periods.Length && Periods[0] == 0){
                return false;
            }
            return true;
        }
		/// <summary>
		/// Признак определенности года
		/// </summary>
		/// <returns></returns>
        public bool IsYearDefined(){
            return 0 != Year;
        }
		/// <summary>
		/// Признак определенности начальной даты
		/// </summary>
		/// <returns></returns>
        public bool IsStartDateDefined(){
            return StartDate > DateExtensions.Begin;
        }
		/// <summary>
		/// Признак определенности конечной даты
		/// </summary>
		/// <returns></returns>
        public bool IsEndDateDefined(){
            return EndDate > DateExtensions.Begin;
        }
		/// <summary>
		/// Признак единичного периода
		/// </summary>
		/// <returns></returns>
        public bool IsOnePeriod(){
            return 1 == Periods.Length;
        }


		/// <summary>
		/// Возвращает строку, которая представляет текущий объект.
		/// </summary>
		/// <returns>
		/// Строка, представляющая текущий объект.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString(){
   
            return string.Format("{0} {1} {2}-{3}", NativeSqlBind.Periods.Get(Periods[0]).Name, Year,
                                 StartDate.ToString("dd.MM.yyyy"), EndDate.ToString("dd.MM.yyyy"));
   
        }
    }
}