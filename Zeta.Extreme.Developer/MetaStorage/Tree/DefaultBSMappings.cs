namespace Zeta.Extreme.Developer.MetaStorage.Tree {
	/// <summary>
	/// Мапинги по перегонки старых форм в новый язык по умолчания AP-103
	/// </summary>
    public static class DefaultBsMappings
    {
        /// <summary>
        /// Стандартный мапинг меток для строк по AP-107
        /// </summary>
        public static TagMap[] DefaultRowMarks = new[] {
			
			new TagMap("0SA") {Element="sum"},
            new TagMap("0CAPTION") {Element="title"},
            new TagMap("CONTROLPOINT") {Element="controlpoint"},
            
            new TagMap("0NOSUM", AttributeType.Bool,"nosum"),
			new TagMap("CALCSUM", AttributeType.Bool,"calcsum"),
            new TagMap("0SHOWCODE_IN_INPUT", AttributeType.Bool,"showcode"),
            new TagMap("0NOINPUT", AttributeType.Bool,"noinput"),
            new TagMap("0NOOUTPUT", AttributeType.Bool,"nooutput"),
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

            new TagMap("form") {Ignore = true},
            new TagMap("ver") {Ignore = true},
            new TagMap("regional") {Ignore = true},
			new TagMap("active",AttributeType.Bool),
            new TagMap("_line") {Ignore = true},
            new TagMap("_file") {Ignore = true},
			new TagMap("vgrp"),
            new TagMap("m140import") {Ignore = true},
            new TagMap("numberformat"),
            new TagMap("typeraw"),
            new TagMap("viewforgroup"),
            new TagMap("UDPOK",AttributeType.Bool),
            new TagMap("pr_typecol"),
            new TagMap("nodosum",AttributeType.Bool),
            new TagMap("extreme") {Ignore = true},
            new TagMap("period"),
            new TagMap("isplan",AttributeType.Bool),
            new TagMap("specialformview"),
            new TagMap("casbill"),
            new TagMap("olap_tmc"){Group="olap"},
            new TagMap("OBLINK"),
            new TagMap("pr_dtype") {Ignore = true},
            new TagMap("olap_ssosnprod"){Group="olap"},
            new TagMap("olap_lehr"){Group="olap"},
            new TagMap("usedetails",AttributeType.Bool),
            new TagMap("sumobj",AttributeType.Bool),
            new TagMap("olap_bal"){Group="olap"},
            new TagMap("olap_calcruda"){Group="olap"},
            new TagMap("casform"),
            new TagMap("protocol"),
            new TagMap("olap_finrez"){Group="olap"},
            new TagMap("pr_slide") {Ignore = true},
            new TagMap("pr_measure") {Ignore = true},
            new TagMap("pr_colors") {Ignore = true},
            new TagMap("pr_block") {Ignore = true},
            new TagMap("KMULT"),
            new TagMap("isform") {Ignore = true},
            new TagMap("tagforrow",AttributeType.Bool),
            new TagMap("activecol"),
            new TagMap("obsolete"),
            new TagMap("role"),
            new TagMap("pr_stobj") {Ignore = true},
            new TagMap("OPS"),
            new TagMap("OFS"),
            new TagMap("sumrec",AttributeType.Bool),
            new TagMap("olap_fin_anal"){Group="olap"},
            new TagMap("sap-use",AttributeType.Bool),
            new TagMap("sap-region"),
            new TagMap("sap-form"),
            new TagMap("pr_ylimits") {Ignore = true},
            new TagMap("pr_numberscalevalue") {Ignore = true},
            new TagMap("slideformat") {Ignore = true},
            new TagMap("grpprefix"),
            new TagMap("slotview"),
            new TagMap("linkconto"),
            new TagMap("olap_inv"){Group="olap"},
            new TagMap("olap_fp"){Group="olap"},
            new TagMap("pr_hide") {Ignore = true},
            new TagMap("uselines") {Ignore = true},
            new TagMap("colcategory") {Ignore = true},
            new TagMap("pres") {Ignore = true},
            new TagMap("pr_legendposition") {Ignore = true},
            new TagMap("pr_yminvalue") {Ignore = true},
            new TagMap("pr_ymaxvalue") {Ignore = true},
            new TagMap("OPK"),
            new TagMap("OFK"),
            new TagMap("OLAP_COLP"),
            new TagMap("OLAP_COLF"),
            new TagMap("fromyear"),


        };
    }
}







