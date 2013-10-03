using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.Developer.Scripting {
    /// <summary>
    /// 
    /// </summary>
    public class GenerateObjects : ScriptCommandBase
    {
        /// <summary>
        /// Просто вызывает стандартный экспорт периодов
        /// </summary>
        /// <returns></returns>
        protected override string GetCommandName()
        {
            return DeveloperConstants.ExportObjectsCommand;
        }

        /// <summary>
        /// Сформировать параметры запроса
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override System.Collections.Generic.IDictionary<string, object> SetupParameters(string commandName, Qorpent.Config.IConfig context)
        {
            var result = base.SetupParameters(commandName, context);
            result["useoutorganizations"] = UseOutOrganization;
            result["onlyownonroot"] = OnlyOwnOnRoot;
            return result;
        }
        /// <summary>
        /// Признак использования внешних организаций
        /// </summary>
        public bool UseOutOrganization { get; set; }
        /// <summary>
        /// Признак использования внешних организаций
        /// </summary>
        public bool OnlyOwnOnRoot { get; set; }

        /// <summary>
        /// Инициализатор
        /// </summary>
        /// <param name="def"></param>
        public override void Initialize(System.Xml.Linq.XElement def)
        {
            base.Initialize(def);
            UseOutOrganization = def.Attr("useoutorganizations").ToBool();
            OnlyOwnOnRoot = def.Attr("onlyownonroot").ToBool();
        }

    }
}