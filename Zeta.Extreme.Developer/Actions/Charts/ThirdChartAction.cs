using Qorpent.Charts;
using Qorpent.Charts.FusionCharts;
using Qorpent.Mvc;

namespace Zeta.Extreme.Developer.Actions.Charts {
    /// <summary>
    /// 
    /// </summary>
    [Action("zdev.thirdchart", Role = "DEFAULT")]
    public class ThirdChartAction : ChartActionBase {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="period"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected override object GenerateChart(int year, int period, int obj) {
            var result = new Chart {
                Caption = "Котировки золота на ЦБ РФ",
                SubCaption = "$ за грамм AU",
                CaptionPadding = 10,
                Config = new ChartConfig {
                    Type = FusionChartType.MSLine.ToString()
                }
            };

            var dataset1 = ChartFrontEndBuilder.BuildDataset(year, "m203122", "PLANGOD", new[] { 11, 12, 13, 14, 15, 16, 1 });
            var dataset2 = ChartFrontEndBuilder.BuildDataset(year, "m203127", "PLANGOD", new[] { 11, 12, 13, 14, 15, 16, 1 });

            result.Datasets.Add(dataset1);
            result.Datasets.Add(dataset2);

            foreach (var cat in ChartFrontEndBuilder.BuildCategories(new[] { 11, 12, 13, 14, 15, 16, 1 })) {
                result.Categories.Add(cat);
            }

            result.AsFusion().AddTrendLine(result, new ChartLine {
                StartValue = 1610.0,
                Color = "FF0000",
                Dashed = true,
                DisplayValue = "ТПФП"
            });

            return result;
        }
    }
}
