using Qorpent.Mvc;
using Qorpent.Mvc.Binding;

namespace Zeta.Extreme.Developer.Security.PrefixProblemAnalyzer
{
    /// <summary>
    /// Действие по анализу настройки администраторов на предприятиях
    /// </summary>
    [Action("zdev_security.prefixproblems", Role = "SECURITY_MANAGER")]
    public class PrefixRoleAnalyzerAction:ActionBase
    {
        /// <summary>
        /// Параметры
        /// </summary>
        [Bind] PrefixRecordParameters Parameters { get; set; }
        /// <summary/>
        protected override object MainProcess() {
            return PrefixRecordIndex.Build(Parameters);
        }
    }
}
