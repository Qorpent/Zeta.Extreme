using Qorpent.Charts;
using Qorpent.Charts.FusionCharts;
using Qorpent.Mvc;

namespace Zeta.Extreme.Developer.Actions.Charts {
    /// <summary>
    /// 
    /// </summary>
    [Action("zdev.chart", Role = "DEFAULT")]
    public class ChartAction : ChartActionBase {
        /// <summary>
        ///     
        /// </summary>
        /// <param name="year"></param>
        /// <param name="period"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected override object GenerateChart(int year, int period, int obj) {
            return new Chart()
                .SetXAxisName("Периоды")
                .SetYAxisName("Котировки")
                .SetCaption("Котировки цинка на LME")
                .SetSubCaption("$ за тонну ZN")
                .SetCaptionPadding(10)
                .SetBgColor("FFFFFF,FFFFFF")
                .SetDivIntervalHints("1600,1700,1800,1900,2000,2100,2200,2300")
                .SetAlpha(0)
                .SetShowAlternateHGridColor(false)
                .SetDivLineAlpha(0)
                .SetConfig(
                    new ChartConfig().SetChartType(FusionChartType.MSLine))
                .Add(
                    ChartFrontEndBuilder.BuildDataset(year, "m203103", "PLANGOD", new[] { 11, 12, 13, 14, 15, 16, 1 })
                        .SetSeriesName("На конец периода")
                        .SetAnchorSides(4)
                        .SetAnchorRadius(5)
                        .SetColor("424242"))
                .Add(
                    ChartFrontEndBuilder.BuildDataset(year, "m203118", "PLANGOD", new[] { 11, 12, 13, 14, 15, 16, 1 })
                        .SetSeriesName("На конец периода")
                        .SetAnchorSides(3)
                        .SetAnchorRadius(5)
                        .SetColor("424242"))
                .Add(
                    new ChartTrendLine()
                        .SetStartValue(1790.0)
                        .SetColor("FF0000")
                        .SetDashed(true)
                        .SetDisplayValue("ТПФП"))
                .AddElements(ChartFrontEndBuilder.BuildCategories(new[] {11, 12, 13, 14, 15, 16, 1}));

        }
    }
}
