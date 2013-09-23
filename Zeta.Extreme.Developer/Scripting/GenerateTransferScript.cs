using System.Collections.Generic;
using Qorpent.Applications;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.Developer.Scripting {
    /// <summary>
    /// 
    /// </summary>
    public class GenerateTransferScript : ScriptCommandBase
    {
        private string @from;
        private string to;
        private string col;
        private string year;
        private string period;
        private string obj;
        private string usr;

        /// <summary>
        /// Просто вызывает стандартный экспорт периодов
        /// </summary>
        /// <returns></returns>
        protected override string GetCommandName()
        {
            return ScriptConstants.TRANSFER_DATA_COMMAND;
        }

        /// <summary>
        /// Инициализатор
        /// </summary>
        /// <param name="def"></param>
        public override void Initialize(System.Xml.Linq.XElement def)
        {
            base.Initialize(def);
            from = def.Attr("from");
            to = def.Attr("to");
            col = def.Attr("col");
            year = def.Attr("year");
            period = def.Attr("period");
            obj = def.Attr("obj");
            usr = "transfer@"+from+"@"+ Application.Current.Principal.CurrentUser.Identity.Name;
        }

        /// <summary>
        /// Сформировать параметры запроса
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override  IDictionary<string, object> SetupParameters(string commandName, Qorpent.Config.IConfig context) {
            return new { @from, to, col, year, period, obj, usr }.ToDict();
        }

        /// <summary>
        /// Получить имя  файла по умолчанию
        /// </summary>
        /// <returns></returns>
        protected override string GetDefaultFileName() {
            return string.Format("{0}_{1}_{2}_{3}_{4}_{5}.sql", from, to, col, year, period, obj).Replace(",", ".");
        }
    }
}