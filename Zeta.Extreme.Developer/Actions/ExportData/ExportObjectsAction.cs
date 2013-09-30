using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.Developer.MetaStorage;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
    /// Экспорт сфорировать файл периодов
    /// </summary>
    [Action(DeveloperConstants.ExportObjectsCommand, Arm = "dev", Help = "Сформировать эксортный файл старших объектов", Role = "DEVELOPER")]
    public class ExportObjectsAction : ExportActionBase<ObjectExporter> {
        [Bind]
        private bool UseOutOrganization { get; set; }

        /// <summary>
        /// Признак использования внешних организаций
        /// </summary>
        [Bind]private bool OnlyOwnOnRoot { get; set; }

        /// <summary>
        /// Инициализирует экспортер
        /// </summary>
        /// <returns></returns>
        protected override ObjectExporter InitializeExporter()
        {
            var result= base.InitializeExporter();
            result.UseOutOrganization = UseOutOrganization;
            result.OnlyOwnOnRoot = OnlyOwnOnRoot;
            return result;
        }
    }
}