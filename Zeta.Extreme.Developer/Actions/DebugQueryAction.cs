using Qorpent.Mvc;
using Qorpent.Mvc.Binding;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
    /// 
    /// </summary>
    [Action("zdev.debugquery",Arm="dev",Role="DEVELOPER")]
    public class DebugQueryAction:ActionBase {

        [Bind(Name = "epz")]
        private bool ExcludePrimaryZeroes { get; set; }
        [Bind(Name = "enpz")]
        private bool ExcludeNonPrimaryZeroes { get; set; }
        [Bind(Name = "rcode",Default = "m260530")]
        private string RowCode { get; set; }
        [Bind(Name = "ccode",Default = "Á1")]
        private string ColCode { get; set; }
        [Bind(Name = "year",Default = 2013)]
        private int Year { get; set; }
        [Bind(Name = "period",Default = 1)]
        private int Period { get; set; }
        [Bind(Name = "obj", Default = 352)]
        private int ObjId { get; set; }
        [Bind(Name = "nocompact")]
        private bool PreserveCompact { get; set; }
        [Bind(Name = "cur")]
        private string Currency { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override object MainProcess() {
            var q = new Query(RowCode, ColCode, ObjId, Year, Period);
            if (!string.IsNullOrWhiteSpace(Currency)) {
                q.Currency = Currency;
            }
            var debugger = new Debugger.QueryGraphBuilder(q) {
                ExcludePrimaryZeroes = ExcludePrimaryZeroes, 
                ExcludeNonPrimaryZeroes = ExcludeNonPrimaryZeroes,
                PreserveCompact = PreserveCompact
            };
            return debugger;
        }
    }
}