using Qorpent.BSharp;
using Qorpent.BSharp.Builder;
using Qorpent.Integration.BSharp.Builder.Tasks;

namespace Zeta.Extreme.CompilerExtensions {
    /// <summary>
    /// Задача для
    /// </summary>
    public class BuildZetaBizIndexTask : BSharpBuilderTaskBase
    {
        /// <summary>
        /// Формирует задачу посткомиляции для построения ZETA INDEX
        /// </summary>
        public BuildZetaBizIndexTask() {
            Phase = BSharpBuilderPhase.Build;
            Index = TaskConstants.CompileBSharpTaskIndex + 10;
        }

        /// <summary>
        /// Трансформирует классы прототипа BIZINDEX в полноценные карты соотношения тем, блоков, подсистем
        /// </summary>
        /// <param name="context"></param>
        public override void Execute(IBSharpContext context) {
            Project.Log.Info("BuildZetaBizIndexTask called");
            var bizindexes = context.ResolveAll("bizindex", null);
            foreach (var bSharpClass in bizindexes) {
                Project.Log.Info(bSharpClass.FullName+" will be processed");   
            }
        }
    }
}