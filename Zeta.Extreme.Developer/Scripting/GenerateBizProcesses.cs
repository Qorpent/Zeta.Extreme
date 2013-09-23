using System.Collections.Generic;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.Developer.Scripting {
    /// <summary>
    /// 
    /// </summary>
    public class GenerateBizProcesses : ScriptCommandBase
    {
        /// <summary>
        /// Просто вызывает стандартный экспорт периодов
        /// </summary>
        /// <returns></returns>
        protected override string GetCommandName()
        {
            return ScriptConstants.EXPORT_BIZPROCESSES_COMMAND;
        }
        /// <summary>
        /// Пробрасывает параметр primary-only
        /// </summary>
        /// <param name="def"></param>
        public override void Initialize(System.Xml.Linq.XElement def)
        {
            base.Initialize(def);
            this.PrimaryOnly = def.HasSignificantAttribute("primary-only");
        }
        /// <summary>
        /// Опция генерации только первичных процессов
        /// </summary>
        public bool PrimaryOnly { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override  IDictionary<string, object> SetupParameters(string commandName, Qorpent.Config.IConfig context)
        {
            var result = base.SetupParameters(commandName, context);
            result["primaryonly"] = PrimaryOnly;
            return result;
        }

    }
}