using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.Developer.Analyzers.UsageStat
{
    /// <summary>
    /// 
    /// </summary>
    public class PeriodUsageAnalyzer
    {
        NativeZetaReader reader = new NativeZetaReader();
        /// <summary>
        /// Анализирует использование всех имеющихся периодов
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UsageStatistics> AnalyzeAll() {
            var periods = reader.ReadPeriods().ToArray();
            return periods.AsParallel().WithDegreeOfParallelism(10).Select(_=>new PeriodUsageAnalyzer(). Analyze(_));
        }
        /// <summary>
        /// Анализирует отдельный период
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public UsageStatistics Analyze(Period period) {
            var result = new UsageStatistics();
            if (!period.IsFormula) {
                result.IsPrimary = true;
                AnalyzePrimaryUsage(period,result);
                AnalyzeNoZeroPrimaryUsage(period,result);
            }
            return result;
        }

        private void AnalyzeNoZeroPrimaryUsage(Period period, UsageStatistics result) {
            using (var c = reader.GetConnection()) {
                c.Open();
                var cmd = c.CreateCommand();
                cmd.CommandText = "select count(id) from cell where period = " + period.BizId + " and decimalvalue !=0";
                result.NotZeroPrimary = Convert.ToInt32(cmd.ExecuteScalar());
            }

        }

        private void AnalyzePrimaryUsage(Period period, UsageStatistics result) {
            
        }
    }
}
