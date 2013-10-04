using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Qorpent.Bxl;
using Qorpent.Charts;
using Qorpent.Charts.FusionCharts;
using Qorpent.IO;
using Qorpent.IoC;
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
            var result = new Chart {
                Caption = "Котировки цинка на LME",
                SubCaption = "$ за тонну ZN"
            };

            var dataset1 = new ChartDataset();

            foreach (var q in GetDataCase2()) {         
                var set = new ChartSet();
                set.Set(FusionChartApi.Set_Label, Periods.Get(q.Time.Period).Name);
                set.Set(FusionChartApi.Set_Value, q.GetResult().NumericResult);
                dataset1.Add(set);
            }


            var dataset2 = new ChartDataset();

            foreach (var q in GetDataCase2Line2()) {
                var set = new ChartSet();
                set.Set(FusionChartApi.Set_Label, Periods.Get(q.Time.Period).Name);
                result.Categories.Add(new ChartCategory().SetLabelValue(Periods.Get(q.Time.Period).Name));
                set.Set(FusionChartApi.Set_Value, q.GetResult().NumericResult);
                dataset2.Add(set);
            }

            result.Datasets.Add(dataset1);
            result.Datasets.Add(dataset2);

            result.CaptionPadding = 10;
            result.Config = new ChartConfig {
                Type = FusionChartType.MSLine.ToString()
            };



            return result;

        }
        private IEnumerable<IQuery> GetDataCase2Line2() {
            var rowcode = "m203118";
            var colcode = "PLANGOD";
            var periods = new[] { 11, 12, 13, 14, 15, 16 };

            var s = new Session();
            return periods.Select(p => s.Register(new Query {
                Row = { Code = rowcode },
                Col = { Code = colcode },
                Time = { Year = 2013, Period = p }
            }));
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
