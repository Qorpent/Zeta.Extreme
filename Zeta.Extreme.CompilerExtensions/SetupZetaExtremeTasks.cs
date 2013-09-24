using Qorpent.BSharp.Builder;

namespace Zeta.Extreme.CompilerExtensions
{
    /// <summary>
    /// Проста проверка работы расширений
    /// </summary>
    public class SetupZetaExtremeTasks : BSharpBuilderExtensionBase
    {
        /// <summary>
        /// Перекрыть при изменении в составе задач
        /// </summary>
        protected override void PrepareTasks() {

            Tasks.Add(new CallZetaExtremeScriptTask());
            Tasks.Add(new BuildZetaBizIndexTask());
            Tasks.Add(new SetupTreeCodesTask());
            Tasks.Add(new UpdateRowCodes());
            Tasks.Add(new EvaluateFormPrimaryStatus());
        }
    }
}
