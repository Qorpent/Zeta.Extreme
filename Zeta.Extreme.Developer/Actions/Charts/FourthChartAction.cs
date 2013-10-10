using Qorpent.Charts;
using Qorpent.Charts.FusionCharts;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;

namespace Zeta.Extreme.Developer.Actions.Charts {
    /// <summary>
    /// 
    /// </summary>
    [Action("zdev.fourthchart", Role = "DEFAULT")]
    public class FourthChartAction : ChartActionBase {
        /// <summary>
        /// 
        /// </summary>
        [Bind]
        public string CustomType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="period"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected override object GenerateChart(int year, int period, int obj) {
            var chart = new Chart();

            chart.SetConfig(new ChartConfig { Type = CustomType ?? FusionChartType.MSLine.ToString() })
                 .SetAttribute(FusionChartApi.Chart_Caption, "Котировки серебра на ЦБ РФ")
                 .SetAttribute(FusionChartApi.Chart_SubCaption, "$ за грамм AG")
                 .SetAttribute(FusionChartApi.Chart_CaptionPadding, 10);


            chart.AddElement(
                ChartFrontEndBuilder.BuildDataset(year, "m203123", "PLANGOD", new[] { 11, 12, 13, 14, 15, 16, 1 })
            ).AddElement(
                ChartFrontEndBuilder.BuildDataset(year, "m203128", "PLANGOD", new[] { 11, 12, 13, 14, 15, 16, 1 })
            ).AddElement(new ChartLine {
                StartValue = 20.0,
                Color = "FF0000",
                Dashed = true,
                DisplayValue = "ТПФП"
            }).AddElements(
                ChartFrontEndBuilder.BuildCategories(new[] { 11, 12, 13, 14, 15, 16, 1 })
            );


            return chart;
        }
    }
}
