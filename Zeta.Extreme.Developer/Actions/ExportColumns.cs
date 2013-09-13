using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.Developer.MetaStorage;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
    /// Экспорт сфорировать файл периодов
    /// </summary>
    [Action("zdev.exportcolumns", Arm = "dev", Help = "Сформировать эксортный файл колонок", Role = "DEVELOPER")]
    public class ExportColumns : ActionBase
    {

        /// <summary>
        /// Имя класса
        /// </summary>
        [Bind(Default = "columns")]
        public string ClassName { get; set; }
        /// <summary>
        /// Пространство имен
        /// </summary>
        [Bind(Default = "import")]
        public string Namespace { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override object MainProcess() {
            return new ColumnExporter().GenerateBSharp(Namespace, ClassName);
        }
    }
}