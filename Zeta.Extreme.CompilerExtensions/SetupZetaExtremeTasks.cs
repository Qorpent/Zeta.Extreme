using Qorpent.BSharp.Builder;

namespace Zeta.Extreme.CompilerExtensions
{
    /// <summary>
    /// Проста проверка работы расширений
    /// </summary>
    public class SetupZetaExtremeTasks : BSharpBuilderExtensionBase
    {
        protected override void PrepareTasks()
        {
            Tasks.Add(new BuildThemaIndexTask());
            Tasks.Add(new PushThemasIntoBlocksTask());
        }
    }


}
