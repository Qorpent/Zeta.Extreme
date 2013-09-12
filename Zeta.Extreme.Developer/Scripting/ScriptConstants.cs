namespace Zeta.Extreme.Developer.Scripting {
    /// <summary>
    /// Константы скриптов
    /// </summary>
    public static class ScriptConstants {
        /// <summary>
        /// Команда экспорта колонок
        /// </summary>
        public const string EXPORT_COLUMNS_COMMAND = "zdev/exportcolumns";

        /// <summary>
        /// Комманда экспорта форм
        /// </summary>
        public const string GENERATE_FORM_COMMAND = "zdev/exporttree";

        /// <summary>
        /// Команда зависимостей формы
        /// </summary>
        public const string FORM_DEPENDENCY_COMMAND = "zdev/exportdependencydot";
        /// <summary>
        /// Команда экспорта периодов
        /// </summary>
        public const string EXPORT_PERIODS_COMMAND = "zdev/exportperiods";
        /// <summary>
        /// Команда переноса данных
        /// </summary>
        public const string TRANSFER_DATA_COMMAND = "zdev/transferdata";
    }
}