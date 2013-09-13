namespace Zeta.Extreme.Developer.Scripting {
    /// <summary>
    /// 
    /// </summary>
    public class GenerateColumns : ScriptCommandBase
    {
        /// <summary>
        /// Просто вызывает стандартный экспорт периодов
        /// </summary>
        /// <returns></returns>
        protected override string GetCommandName()
        {
            return ScriptConstants.EXPORT_COLUMNS_COMMAND;
        }

    }
}