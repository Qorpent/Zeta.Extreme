using Qorpent.Mvc.Binding;
using Qorpent.Mvc;

namespace Zeta.Extreme.Developer.Actions
{
    /// <summary>
    /// Возвращает строку Hello World
    /// </summary>
    [Action("zdev.hwout", Help = "Возвращает строку Hello World")]
    public class HwOutAction : ActionBase
    {
        [Bind(Required = true)]
        private string usr = "";
      /// <summary>
        /// MainProcess
        /// </summary>
        /// <returns></returns>
        protected override object MainProcess()
        {
            return "Hello " + usr;
        }
    }
}