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

    }
}