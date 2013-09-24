using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Qorpent.Applications;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.Developer.Analyzers.UsageStat {
    /// <summary>
    /// Анализатор использования колонок
    /// </summary>
    public class ColumnUsageAnalyzer {
        NativeZetaReader reader = new NativeZetaReader();
        private Column[] columns;
        /// <summary>
        /// 
        /// </summary>
        public ColumnUsageAnalyzer(){}
        private ColumnUsageAnalyzer(Column[] columns1)
        {
            columns = columns1;
        }

        /// <summary>
        /// Анализирует использование всех имеющихся периодов
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UsageStatistics> AnalyzeAll() {
            columns = reader.ReadColumns().ToArray();
            return columns.AsParallel().WithDegreeOfParallelism(10).Select(_=>new ColumnUsageAnalyzer(columns). Analyze(_));
        }
        /// <summary>
        /// Анализирует отдельный период
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public UsageStatistics Analyze(Column period) {
            var result = new UsageStatistics {Target = period, Code = period.Code, Name = period.Name};
            if (!period.IsFormula) {
                result.IsPrimary = true;
                AnalyzePrimaryUsage(period,result);
                AnalyzeNoZeroPrimaryUsage(period,result);
                if (result.NotZeroPrimary == 0) {
                    result.IsProblem = true;
                    result.Problem += "нет реальных значений; ";
                }
                else if(result.NotZeroPrimary<=100) {
                    result.IsProblem = true;
                    result.Problem += "пренебрежимо мало значений; ";
                }
            }
            AnalyzeRowFormulaUsages(period,result);
            AnalyzeColFormulaUsages(period,result);
            AnalyzeThemaUsages(period,result);
            if (period.IsFormula) {
                if (0 == result.RowFormula + result.ColFormula + result.Themas) {
                    result.IsProblem = true;
                    result.Problem += "формула не используется; ";
                }
            }

          
            return result;
        }

        private void AnalyzeThemaUsages(Column column, UsageStatistics result) {
            var cnt = 0;
            var analyzer = Application.Current.Container.Get<IAnalyzer>();
            var index = analyzer.Index;
            var code = column.Code;
            foreach (var src  in index.GetAllSources()) {
                var xml = src.XmlContent;
                var cols = xml.Descendants("col").ToArray();
                foreach (var col in cols) {
                    var p = col.Attr("code");
                    if (p == code) cnt++;
                    var formula = col.Attr("formula");
                    if (!string.IsNullOrWhiteSpace(formula)) {
                        if (IsMatch(column, formula))
                        {
                            cnt++;
                        }
                    }
                }
            }
            result.Themas = cnt;
        }

        private static bool IsMatch(Column column, string formula) {
            return Regex.IsMatch(formula, "@" + column.Code + "\\W");
        }

        private void AnalyzeColFormulaUsages(Column column, UsageStatistics result) {
            var cnt = 0;
            foreach (var col in this.columns.Where(_=>_.IsFormula)) {
                if (IsMatch(column,col.Formula)) {
                    cnt++;
                }
            }
            result.ColFormula = cnt;
        }

        private void AnalyzeRowFormulaUsages(Column column, UsageStatistics result) {
            var cnt = 0;
            foreach (var row in RowCache.Byid.Values.Where(_ => _.IsFormula))
            {
                if (IsMatch(column,row.Formula))
                {
                    cnt++;
                }
            }
            result.ColFormula = cnt;
        }

      

        private void AnalyzeNoZeroPrimaryUsage(Column column, UsageStatistics result) {
            using (var c = reader.GetConnection()) {
                c.Open();
                var cmd = c.CreateCommand();
                cmd.CommandText = "select count(id) from cell where col = " + column.Id + " and decimalvalue !=0";
                result.NotZeroPrimary = Convert.ToInt32(cmd.ExecuteScalar());
            }

        }

        private void AnalyzePrimaryUsage(Column column, UsageStatistics result)
        {
            using (var c = reader.GetConnection())
            {
                c.Open();
                var cmd = c.CreateCommand();
                cmd.CommandText = "select count(id) from data where col = " + column.Id;
                result.NotZeroPrimary = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}