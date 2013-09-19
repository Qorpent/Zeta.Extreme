using System.Diagnostics;
using System.Linq;
using Qorpent.BSharp;
using Qorpent.BSharp.Builder;
using Qorpent.Integration.BSharp.Builder.Tasks;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.CompilerExtensions {
    /// <summary>
    /// Выполняет указанные скрипты Zeta.Script перед началом общей компиляции
    /// </summary>
    public class CallZetaExtremeScriptTask : BSharpBuilderTaskBase {
        /// <summary>
        /// Создает стандартную задачу выполнения скрипта Zeta.Extreme
        /// </summary>
        public CallZetaExtremeScriptTask() {
            Index = TaskConstants.LoadAllSourcesTaskIndex - TaskConstants.INDEX_STEP;
            Phase = BSharpBuilderPhase.PreProcess;
        }
        /// <summary>
        /// Выполняет указанные в проекте Zeta.Script с использованием штатного ZDS (использует имено EXE)
        /// </summary>
        /// <param name="context"></param>
        public override void Execute(IBSharpContext context) {
            var scripts = Project.SrcClass.Compiled.Elements("Zeta.Script").ToArray();
            foreach (var script in scripts) {
                Project.Log.Info("start script "+script.GetCode());
                var process = Process.Start("zds.exe", script.GetCode());
                process.WaitForExit();
                Project.Log.Info("script " + script.GetCode()+" finished");
            }
        }
    }
}