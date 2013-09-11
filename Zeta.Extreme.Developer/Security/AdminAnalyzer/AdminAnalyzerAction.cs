using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qorpent.Mvc;

namespace Zeta.Extreme.Developer.Security.AdminAnalyzer
{
    /// <summary>
    /// Действие по анализу настройки администраторов на предприятиях
    /// </summary>
    [Action("zdev_security.admins",Role = "SECURITY_MANAGER")]
    public class AdminAnalyzerAction:ActionBase
    {
        /// <summary/>
        protected override object MainProcess() {
            return new AdminAnalyzerHelper(Context)
                .Collect()
                .OrderBy(_=>_.Index)
                .ToArray();
        }
    }
}
