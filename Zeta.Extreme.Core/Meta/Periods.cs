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
using System.Collections.Generic;
using System.Linq;
using Comdiv.Application;
using Comdiv.Common;
using Comdiv.Extensions;
using Comdiv.Zeta.Model;
using Zeta.Extreme.Poco;

namespace Zeta.Extreme.Meta{
    /// <summary>
    /// Обслуживание логики периодов
    /// </summary>
    public static class Periods{
	    private static IList<IPeriod> _cached;

		/// <summary>
		/// Вычисление формульного периода
		/// </summary>
		/// <param name="sourceyear"></param>
		/// <param name="sourceperiod"></param>
		/// <param name="formulaperiod"></param>
		/// <returns></returns>
		public static PeriodDefinition Eval(int sourceyear, int sourceperiod, int formulaperiod)
		{
			return Get(formulaperiod).Evaluate(sourceyear,DateExtensions.Begin,sourceperiod);
		}

        private static IEnumerable<IPeriod> cached{
            get { return _cached ?? (_cached = new NativeZetaReader().ReadPeriods().OfType<IPeriod>().ToList()); }
        }
	/// <summary>
	/// Перезагрузка кэша
	/// </summary>
        public static void Reload(){
            _cached = null;
        }

		/// <summary>
		/// Получение периода по ИД
		/// </summary>
		/// <param name="classicId"></param>
		/// <returns></returns>
	    public static IPeriod Get(int classicId){
            var result = cached.FirstOrDefault(x => x.ClassicId == classicId);
            if (null == result){
                result = new period();
                result.ClassicId = classicId;
                result.StartDate = DateExtensions.Begin;
                result.EndDate = DateExtensions.End;
				if(classicId<0) {
					result.IsFormula = true;
				}
            }
            return result;
        }

	    private static readonly IDictionary<string, IDictionary<int, string>> cache =
		    new Dictionary<string, IDictionary<int, string>>();

	    /// <summary>
	    /// Вычисление формулы
	    /// </summary>
	    /// <param name="period"></param>
	    /// <param name="otherperiodId"></param>
	    /// <returns></returns>
	    public static int Evaluate(this IPeriod period, int otherperiodId){
		    return period.Evaluate(DateTime.Today.Year, DateExtensions.Begin, otherperiodId).Periods[0];
	    }

	    /// <summary>
	    /// Вычисление смешенного периода
	    /// </summary>
	    /// <param name="period"></param>
	    /// <param name="otherperiodId"></param>
	    /// <returns></returns>
	    public static PeriodDefinition EvaluateDef(this IPeriod period, int otherperiodId)
	    {
		    return period.Evaluate(DateTime.Today.Year, DateExtensions.Begin, otherperiodId);
	    }

	    /// <summary>
	    /// Вычисление полной формулы
	    /// </summary>
	    /// <param name="period"></param>
	    /// <param name="year"></param>
	    /// <param name="date"></param>
	    /// <param name="otherperiodId"></param>
	    /// <returns></returns>
	    public static PeriodDefinition Evaluate(this IPeriod period, int year, DateTime date, int otherperiodId){
		    var result = new PeriodDefinition(year, period.ClassicId);
		    if (period.IsFormula){
			    result.Periods = new[]{otherperiodId};
			    EvaluateFormula(result, period.Formula);
		    }
		    return result;
	    }

	    private static void EvaluateFormula(PeriodDefinition definition, string formula){
		    var dict = cache.get(formula, () =>{
			                                       var res = new Dictionary<int, string>();
			                                       var rules = formula.split();
			                                       foreach (var r in rules){
				                                       var parts = r.split(false, true, '=');
				                                       res[parts[0].toInt()] = parts[1];
			                                       }
			                                       return res;
		    });
		    var rule = dict.get(definition.Periods[0],defobj: "");
		    if (rule.noContent()){
			    rule = dict.get(0,defobj: "");
		    }
		    if (rule.hasContent()) {
			    var pt = rule.split(false, true, '!');
			    if(pt.Count>1) {
				    var titlesoruce = pt[1].toInt();
				    definition.PeriodName = Get(titlesoruce).Name;
			    }
			    var py = pt[0].split(false, true, ':');
			    var ps = py[0].split(false,true,'+').Select(x=>ConvertExtensions.toInt(x)).ToArray();
			    var y = 0;
			    if (py.Count > 1){
				    y = py[1].toInt();
			    }
			    definition.Year = definition.Year + y;
			    definition.Periods = ps;
		    }
	    }
    }
}
