﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Qorpent.Applications;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.Developer.Analyzers.UsageStat
{
    /// <summary>
    /// 
    /// </summary>
    public class PeriodUsageAnalyzer
    {
        NativeZetaReader reader = new NativeZetaReader();
        private Period[] periods;
        /// <summary>
        /// 
        /// </summary>
        public PeriodUsageAnalyzer(){}
        private PeriodUsageAnalyzer(Period[] periods1) {
            periods = periods1;
        }

        /// <summary>
        /// Анализирует использование всех имеющихся периодов
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UsageStatistics> AnalyzeAll() {
            periods = reader.ReadPeriods().ToArray();
            return periods.AsParallel().WithDegreeOfParallelism(10).Select(_=>new PeriodUsageAnalyzer(periods). Analyze(_));
        }
        /// <summary>
        /// Анализирует отдельный период
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public UsageStatistics Analyze(Period period) {
            var result = new UsageStatistics {Target = period, Code = period.BizId.ToString(), Name = period.Name};
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
                AnalyzeInPeriodFormulaUsages(period, result);
            }
            AnalyzeRowFormulaUsages(period,result);
            AnalyzeColFormulaUsages(period,result);
            AnalyzeThemaUsages(period,result);
            if (period.IsFormula) {
                if (0 == result.RowFormula + result.ColFormula + result.Themas) {
                    result.IsProblem = true;
                    result.Problem += "формула не используется; ";
                }
                else if (10 >= result.RowFormula + result.ColFormula + result.Themas)
                {
                    result.IsProblem = true;
                    result.Problem += "формула практически не используется; ";
                }
            }

          
            return result;
        }

        private void AnalyzeThemaUsages(Period period, UsageStatistics result) {
            var cnt = 0;
            var analyzer = Application.Current.Container.Get<IAnalyzer>();
            var index = analyzer.Index;
            var code = period.BizId.ToString();
            foreach (var src  in index.GetAllSources()) {
                var xml = src.XmlContent;
                var cols = xml.Descendants("col").ToArray();
                foreach (var col in cols) {
                    var p = col.Attr("period");
                    if (p == code) cnt++;
                    var formula = col.Attr("formula");
                    if (!string.IsNullOrWhiteSpace(formula)) {
                        if (Regex.IsMatch(formula, "\\.P" + period.BizId + "\\D"))
                        {
                            cnt++;
                        }
                    }
                }
            }
            result.Themas = cnt;
        }

        private void AnalyzeColFormulaUsages(Period period, UsageStatistics result) {
            var cnt = 0;
            foreach (var col in ColumnCache.Byid.Values.Where(_=>_.IsFormula)) {
                if (Regex.IsMatch(col.Formula, "\\.P" + period.BizId + "\\D")) {
                    cnt++;
                }
            }
            result.ColFormula = cnt;
        }

        private void AnalyzeRowFormulaUsages(Period period, UsageStatistics result) {
            var cnt = 0;
            foreach (var col in RowCache.Byid.Values.Where(_ => _.IsFormula))
            {
                if (Regex.IsMatch(col.Formula, "\\.P" + period.BizId + "\\D"))
                {
                    cnt++;
                }
            }
            result.ColFormula = cnt;
        }

        private void AnalyzeInPeriodFormulaUsages(Period period, UsageStatistics result) {
            var cnt = 0;
            foreach (var source in periods.Where(_ => _.IsFormula))
            {
                var elements = source.Formula.SmartSplit(false, true, '=', ',', ';', ':');
                if (elements.Contains(period.BizId.ToString()))
                {
                    cnt++;
                }
            }
            result.PeriodFormula = cnt;
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
            using (var c = reader.GetConnection())
            {
                c.Open();
                var cmd = c.CreateCommand();
                cmd.CommandText = "select count(id) from cell where period = " + period.BizId;
                result.NotZeroPrimary = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}
