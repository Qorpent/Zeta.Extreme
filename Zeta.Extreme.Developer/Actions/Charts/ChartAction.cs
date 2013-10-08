﻿using Qorpent.Charts;
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
            var result = new Chart {
                Caption = "Котировки цинка на LME",
                SubCaption = "$ за тонну ZN",
                CaptionPadding = 10,
                Config = new ChartConfig {
                    Type = FusionChartType.MSLine.ToString()
                }
            };

            var dataset1 = ChartFrontEndBuilder.BuildDataset(year, "m203103", "PLANGOD", new[] { 11, 12, 13, 14, 15, 16 });
            var dataset2 = ChartFrontEndBuilder.BuildDataset(year, "m203118", "PLANGOD", new[] { 11, 12, 13, 14, 15, 16 });

            dataset1.Set(FusionChartApi.Dataset_SeriesName, "Одна фигня");
            dataset2.Set(FusionChartApi.Dataset_SeriesName, "Другая фигня");

            result.Datasets.Add(dataset1);
            result.Datasets.Add(dataset2);

            foreach (var cat in ChartFrontEndBuilder.BuildCategories(new[] { 11, 12, 13, 14, 15, 16 })) {
                result.Categories.Add(cat);
            }

            result.Set(FusionChartApi.Chart_LegendPosition, "RIGHT");

            result.AsFusion().AddTrendLine(result, new ChartLine {
                StartValue = 1750.0,
                Color = "FF0000",
                Dashed = true,
                DisplayValue = "ТПФП"
            });

            return result;
        }
    }
}
