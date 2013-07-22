using Qorpent.Mvc.Binding;
using Qorpent.Mvc;

namespace Zeta.Extreme.Developer.Actions
{
    /// <summary>
    /// Возвращает строку Hello World
    /// </summary>
    [Action("zdev.hwout", Help = "Проверка роли у пользователя22")]
    public class HwOutAction : ActionBase
    {
        [Bind(Required = true)]
        private string usr = "";
        //[Bind(Required = true)]
        //private string role = "";
        /// <summary>
        /// Тарама папаамамапама
        /// </summary>
        /// <returns></returns>
        protected override object MainProcess()
        {
            return "Hello " + usr;
        }
    }
}