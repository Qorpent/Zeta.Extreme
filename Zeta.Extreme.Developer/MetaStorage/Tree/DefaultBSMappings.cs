﻿namespace Zeta.Extreme.Developer.MetaStorage.Tree {
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
            new TagMap("pr_caption") {Ignore = true},
            new TagMap("olap_os"){Group="olap"},
            new TagMap("tagforcol",AttributeType.Bool),
            new TagMap("prefix") {Ignore = true},
            new TagMap("linkrole"),
            new TagMap("forperiods"),
            new TagMap("ts_tree") {Ignore = true},
            new TagMap("olap_fin"){Group="olap"},
            new TagMap("matrix",AttributeType.Bool),
            new TagMap("linkisformula",AttributeType.Bool),
            new TagMap("usecombine") {Ignore = true},
            new TagMap("ts_m140") {Ignore = true},
            new TagMap("r_filter") {Ignore = true},
            new TagMap("metal"),
            new TagMap("valuta"),
            new TagMap("group") {Ignore = true},
            new TagMap("pr_sort") {Ignore = true},
            new TagMap("r_filter") {Ignore = true},
            new TagMap("pr_rowheight") {Ignore = true},
            new TagMap("nds_avans"){Group="nds"},
            new TagMap("extslot"),
            new TagMap("export",AttributeType.Bool),
            new TagMap("CALC_OP"),
            new TagMap("pr_tablestyle") {Ignore = true},
            new TagMap("olap_nal"){Group="olap"},
            new TagMap("oknull",AttributeType.Bool),
            new TagMap("nds_nob"){Group="nds"},
            new TagMap("linkconto1"),
            new TagMap("fr_filter") {Ignore = true},
            new TagMap("assert",AttributeType.Bool),
            new TagMap("SWK"),
            new TagMap("olap_uslchop",AttributeType.Bool),
            new TagMap("view") {Ignore = true},
            new TagMap("pr_decimals") {Ignore = true},
            new TagMap("prot") {Ignore = true},
            new TagMap("pr_linethikness") {Ignore = true},
            new TagMap("pr_filter") {Ignore = true},
            new TagMap("tagforobj",AttributeType.Bool),
            new TagMap("osn_prod",AttributeType.Bool),
            new TagMap("fstr",AttributeType.Bool),
            new TagMap("useosv",AttributeType.Bool),
            new TagMap("formfilter",AttributeType.Bool),
            new TagMap("casplitdiv",AttributeType.Bool),
            new TagMap("olap__nds19"){Group="olap"},
            new TagMap("biztran"),
            new TagMap("afdep"),
            new TagMap("type"),
            new TagMap("sourcelink"),
            new TagMap("ts_showroot") {Ignore = true},
            new TagMap("prg_showvalues") {Ignore = true},
            new TagMap("showvalues",AttributeType.Bool),
            new TagMap("rows"),
            new TagMap("prtr"),
            new TagMap("prb",AttributeType.Bool),
            new TagMap("olap_tep"){Group="olap"},
            new TagMap("objset") {Ignore = true},
            new TagMap("masterrow",AttributeType.Bool),
            new TagMap("castruc"),
            new TagMap("carole"),
            new TagMap("CALC_TP"),
            new TagMap("CALC_RP"),
            new TagMap("usedoughnut2d") {Ignore = true},
            new TagMap("usedoughnut") {Ignore = true},
            new TagMap("usearea") {Ignore = true},
            new TagMap("ts_rootrow") {Ignore = true},
            new TagMap("total",AttributeType.Bool),
            new TagMap("tagfordetail",AttributeType.Bool),
            new TagMap("roleprefix"),
            new TagMap("protocolview") {Ignore = true},
            new TagMap("prg_showsum") {Ignore = true},
            new TagMap("ppvalues") {Ignore = true},
            new TagMap("primary",AttributeType.Bool),
            new TagMap("KOLDIV1000",AttributeType.Bool),
            new TagMap("dependent",AttributeType.Bool),
            new TagMap("checklimit",AttributeType.Bool),
            new TagMap("pptarget") {Ignore = true},
            new TagMap("ppsource") {Ignore = true},
            new TagMap("pplevel") {Ignore = true},
            new TagMap("ppformat") {Ignore = true},
            new TagMap("olap_sir"){Group="olap"},
            new TagMap("deplist"),
            new TagMap("bt"),
            new TagMap("aliased"),
            new TagMap("noblocker",AttributeType.Bool),
            

        };
    }
}






