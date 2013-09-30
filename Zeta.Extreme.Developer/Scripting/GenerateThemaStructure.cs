using Qorpent.Utils.Extensions;
using Zeta.Extreme.Developer.Analyzers;

namespace Zeta.Extreme.Developer.Scripting {
    /// <summary>
    /// 
    /// </summary>
    public class GenerateThemaStructure : ScriptCommandBase
    {
        bool BlockOnly { get; set; }
        string SubsystemAliases { get; set; }
        string ExcludeRoots { get; set; }
        bool DisableStatusFilter { get; set; }

        /// <summary>
        /// Просто вызывает стандартный экспорт периодов
        /// </summary>
        /// <returns></returns>
        protected override string GetCommandName()
        {
            return DeveloperConstants.ExportThemastructureCommand;
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
            result["blockonly"] = BlockOnly;
            result["subsystemaliases"] = SubsystemAliases;
            result["excluderoots"] = ExcludeRoots;
            result["disablestatusfilter"] = DisableStatusFilter;
            return result;
        }

        /// <summary>
        /// Инициализатор
        /// </summary>
        /// <param name="def"></param>
        public override void Initialize(System.Xml.Linq.XElement def)
        {
            base.Initialize(def);
            BlockOnly = def.Attr("blockonly").ToBool();
            SubsystemAliases = def.Attr("subsystemaliases");
            ExcludeRoots = def.Attr("excluderoots");
            DisableStatusFilter = def.Attr("disablestatusfilter").ToBool();
        }
    }
}