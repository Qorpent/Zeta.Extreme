﻿using Qorpent.BSharp.Builder;

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
            var sbe = new SetupBlockExtensions(Project);
            if (sbe.Active) {
                Project.CompilerExtensions.Add(sbe);
            }
            Tasks.Add(new CallZetaExtremeScriptTask());
            Tasks.Add(new BuildZetaBizIndexTask());
            Tasks.Add(new SetupTreeCodesTask());
            Tasks.Add(new UpdateRowCodes());
            Tasks.Add(new EvaluateFormPrimaryStatus());
            Tasks.Add(new BuildFormExcelTable());
        }
    }
}
