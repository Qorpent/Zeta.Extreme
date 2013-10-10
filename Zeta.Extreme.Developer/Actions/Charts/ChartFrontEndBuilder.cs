using System.Collections.Generic;
using System.Linq;
using Qorpent.Charts;
using Qorpent.Charts.FusionCharts;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Developer.Actions.Charts {
    /// <summary>
    /// 
    /// </summary>
    public static class ChartFrontEndBuilder {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="rowCode"></param>
        /// <param name="colCode"></param>
        /// <param name="periods"></param>
        /// <returns></returns>
        private static IEnumerable<IQuery> GetData(int year, string rowCode, string colCode, IEnumerable<int> periods) {
            var session = new Session();
            return periods.Select(p => session.Register(new Query {
                Row = { Code = rowCode },
                Col = { Code = colCode },
                Time = { Year = year, Period = p }
            }));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="rowCode"></param>
        /// <param name="colCode"></param>
        /// <param name="periods"></param>
        /// <returns></returns>
        public static ChartDataset BuildDataset(int year, string rowCode, string colCode, IEnumerable<int> periods) {
            var dataset = new ChartDataset();

            foreach (var q in GetData(year, rowCode, colCode, periods)) {
                dataset.AddSet(Periods.Get(q.Time.Period).Name, q.GetResult().NumericResult);
            }

            return dataset;
        }
        /// <summary>
        ///     Собирает представление категорий по переданному перечислению периодов
        /// </summary>
        /// <param name="periods"></param>
        /// <returns></returns>
        public static IEnumerable<IChartCategory> BuildCategories(IEnumerable<int> periods) {
            return BuildCategories(periods.Select(_ => _.ToString()));
        }
        /// <summary>
        ///     Собирает представление категорий по переданному перечислению названий лэйблов
        /// </summary>
        /// <param name="labels">Перечисление лэйблов</param>
        /// <returns></returns>
        public static IEnumerable<IChartCategory> BuildCategories(IEnumerable<string> labels) {
            return labels.Select(_ => new ChartCategory().SetLabelValue(_));
        }
    }
}
