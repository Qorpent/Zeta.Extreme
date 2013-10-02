using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.Developer.MetaStorage;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
    /// Экспорт сформировать файл периодов
    /// </summary>
    [Action(DeveloperConstants.ExportThemastructureCommand, Arm = "dev", Help = "Сформировать экспортный файл структуры тем", Role = "DEVELOPER")]
    public class ExportThemaStructureAction : ExportActionBase<ThemaStructureExporter>
    {
        [Bind(Help="Развернуть только до второго уровня (до блоков)")]
        bool BlockOnly { get; set; }
        [Bind]
        string SubsystemAliases { get; set; }
        /// <summary>
        /// Параметр исключения части рутов
        /// </summary>
        [Bind]
        string ExcludeRoots { get; set; }

        /// <summary>
        /// Опция отключения фильтра по статусам темы
        /// </summary>
        [Bind]
        bool DisableStatusFilter { get; set; }
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override ThemaStructureExporter InitializeExporter()
        {
            var result = base.InitializeExporter();
            result.BlocksOnly = BlockOnly;
            result.SubsystemAliases = SubsystemAliases;
            result.ExcludeRoots = ExcludeRoots;
            result.DisableStatusFilter = DisableStatusFilter;
            return result;
        }
    }
}