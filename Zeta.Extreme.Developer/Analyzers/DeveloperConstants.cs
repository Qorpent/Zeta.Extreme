namespace Zeta.Extreme.Developer.Analyzers {
	/// <summary>
	/// Общие константы для разработки
	/// </summary>
	public static class DeveloperConstants {
		/// <summary>
		/// XPath исключения системных атрибутов
		/// </summary>
		public const string XpathSysAttributesFilter =
			"local-name()!='_line' and local-name()!='_file' and local-name()!='code' and local-name()!='__code'  and local-name()!='id'  and local-name()!='__id' and local-name()!='name'  and local-name()!='__name'";
	}
}