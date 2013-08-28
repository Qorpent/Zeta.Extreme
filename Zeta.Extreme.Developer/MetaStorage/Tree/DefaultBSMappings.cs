namespace Zeta.Extreme.Developer.MetaStorage.Tree {
	/// <summary>
	/// Мапинги по перегонки старых форм в новый язык по умолчания AP-103
	/// </summary>
	public static class DefaultBsMappings {
		/// <summary>
		/// Стандартный мапинг меток для строк по AP-107
		/// </summary>
		public static TagMap[] DefaultRowMarks = new[] {
			new TagMap("0PARTITION") {Ignore = true},
			new TagMap("0SA") {Element="sum"},
			new TagMap("0NOSUM", AttributeType.Bool,"nosum"),
			new TagMap("0AA"){Error=true},
		};

		/// <summary>
		/// Стандартный мапинг тегов длястрок  по AP-108
		/// </summary>
		public static TagMap[] DefaultRowTags = new[] {
			new TagMap("active",AttributeType.Bool),
			new TagMap("vgrp"),
			new TagMap("olap_tmc"){Group="olap"},
		};

	}
}