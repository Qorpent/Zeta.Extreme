using System.Collections.Generic;
using System.Linq;
using Qorpent.Charts;
using Qorpent.Charts.FusionCharts;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Developer.Actions.Charts {
    /// <summary>
    /// 
    /// </summary>
    [Action("zdev.chart", Role = "DEFAULT")]
    public class ChartAction : ActionBase {
        /// <summary>
        ///     Год
        /// </summary>
        [Bind(Required = true)] public int Year { get; set; }
        /// <summary>
        ///     Период
        /// </summary>
        [Bind(Required = true)] public int Period { get; set; }
        /// <summary>
        ///     Объект
        /// </summary>
        [Bind(Required = true)] public int Object { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override object MainProcess() {
            return GenerateChart(Year, Period, Object);
        }
        /// <summary>
        ///     
        /// </summary>
        /// <param name="year"></param>
        /// <param name="period"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private object GenerateChart(int year, int period, int obj) {
            var result = new Chart { Caption = "Monthly Revenue" };

            foreach (var q in GetDataCase2()) {
                var qr = q.GetResult();
          
                result.AddSet(Periods.Get(q.Time.Period).Name, qr.NumericResult);
            }

            result.Config = new ChartConfig {Type = FusionChartType.Line.ToString()};
            return result;

        }
        /// <summary>
        ///     Собирает данные для «Котировки цинка на LME»
        /// </summary>
        /// <returns></returns>
        private IEnumerable<IQuery> GetDataCase2() {
            var rowcode = "m203103";
            var colcode = "PLANGOD";
            var periods = new[] { 11, 12, 13, 14, 15, 16 };

            var s = new Session();
            return periods.Select(p => s.Register(new Query {
                Row = {Code = rowcode},
                Col = {Code = colcode},
                Time = {Year = 2013, Period = p}
            }));
            
        }
    }
}
