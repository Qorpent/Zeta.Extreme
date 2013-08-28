namespace Zeta.Extreme.Developer.MetaStorage.Tree {
	/// <summary>
	/// Мапинги по перегонки старых форм в новый язык по умолчания AP-103
	/// </summary>
	public static class DefaultBsMappings {
		/// <summary>
		/// Стандартный мапинг меток для строк по AP-107
		/// </summary>
		public static TagMap[] DefaultRowMarks = new[] {
			
			new TagMap("0SA") {Element="sum"},
            new TagMap("0CAPTION") {Element="title"},
            new TagMap("CONTROLPOINT") {Element="controlpoint "},
            
            new TagMap("0NOSUM", AttributeType.Bool,"nosum"),
			new TagMap("CALCSUM", AttributeType.Bool,"calcsum"),
            new TagMap("0SHOWCODE_IN_INPUT", AttributeType.Bool,"showcode"),
            new TagMap("0NOINPUT", AttributeType.Bool,"noinput"),
            new TagMap("0NOOUTPUT", AttributeType.Bool,"nooutput "),
            new TagMap("0MINUS", AttributeType.Bool,"minus"),


			new TagMap("0EXT"){Error=true},
            new TagMap("0OTHERS"){Error=true},
            new TagMap("0AA"){Error=true},

            new TagMap("0PARTITION") {Ignore = true},
            new TagMap("0OPTIMIZESUM") {Ignore = true},
            new TagMap("0INPUTROOT") {Ignore = true},
            new TagMap("FORMGROUP") {Ignore = true},
            new TagMap("OLAP") {Ignore = true},
            new TagMap("0NOTCACHE") {Ignore = true},
            new TagMap("ANALYTIC") {Ignore = true},
            new TagMap("0L1DETAILED") {Ignore = true},
            new TagMap("0SPEC") {Ignore = true},
            new TagMap("0NEEDSUBPART") {Ignore = true},
            new TagMap("NALOGGROUPS") {Ignore = true},
            new TagMap("0SPDEL") {Ignore = true},
            new TagMap("NALOGROOT") {Ignore = true},


		};

		/// <summary>
		/// Стандартный мапинг тегов для строк  по AP-108
		/// </summary>
		public static TagMap[] DefaultRowTags = new[] {
			new TagMap("active",AttributeType.Bool),
			new TagMap("vgrp"),
			new TagMap("olap_tmc"){Group="olap"},
		};

	}
}