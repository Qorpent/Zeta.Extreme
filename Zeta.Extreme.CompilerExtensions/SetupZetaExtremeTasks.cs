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
            var task = new BuildZetaBizIndexTask();
            task.SetProject(Project);
            Tasks.Add(task);
        }
    }
}
