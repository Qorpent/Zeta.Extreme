namespace Zeta.Extreme.Developer {
	/// <summary>
	/// Общие константы для разработки
	/// </summary>
	public static class DeveloperConstants {
		/// <summary>
		/// XPath исключения системных атрибутов
		/// </summary>
		public const string XpathSysAttributesFilter =
			"local-name()!='_line' and local-name()!='_file' and local-name()!='code' and local-name()!='__code'  and local-name()!='id'  and local-name()!='__id' and local-name()!='name'  and local-name()!='__name'";

	    /// <summary>
	    /// Команда экспорта дивизионов
	    /// </summary>
	    public const string ExportObjdivsCommand = "zdev.exportobjdivs";

	    /// <summary>
	    /// Команда экспорта колонок
	    /// </summary>
	    public const string ExportColumnsCommand = "zdev.exportcolumns";

	    /// <summary>
	    /// Комманда экспорта форм
	    /// </summary>
	    public const string GenerateFormCommand = "zdev.exporttree";

	    /// <summary>
	    /// Команда зависимостей формы
	    /// </summary>
	    public const string FormDependencyCommand = "zdev.exportdependencydot";

	    /// <summary>
	    /// Команда экспорта периодов
	    /// </summary>
	    public const string ExportPeriodsCommand = "zdev.exportperiods";

	    /// <summary>
	    /// Команда экспорта географии
	    /// </summary>
	    public const string ExportGeoCommand = "zdev.exportgeo";

	    /// <summary>
	    /// Команда экспорта бизпроцессов
	    /// </summary>
	    public const string ExportBizprocessesCommand = "zdev.exportbizprocesses";

	    /// <summary>
	    /// Команда экспорта типов лобъектов
	    /// </summary>
	    public const string ExportObjtypesCommand = "zdev.exportobjtypes";

	    /// <summary>
	    /// Команда экспорта структуры тем
	    /// </summary>
	    public const string ExportThemastructureCommand = "zdev.exportthemastructure";

	    /// <summary>
	    /// Команда переноса данных
	    /// </summary>
	    public const string TransferDataCommand = "zdev.transferdata";
	}
}