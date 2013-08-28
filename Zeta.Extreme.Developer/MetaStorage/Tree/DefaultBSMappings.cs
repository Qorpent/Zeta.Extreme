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
            





CALC_OP	, z210400, z2504000, z2604000, z4104000, z4202800	7	a=v
pr_tablestyle	, _dobpok, _factor, _macropok, _osnfinpok, _osn...	6	-
olap_nal	, 1	6	g=olap
oknull	, 1	6	a
nds_nob	, m224921, m2249263, m224927, m224929, m224942	6	g=nds
linkconto1	, BA010_931, BA010_932, BA045_01, BA060_01	6	a=v
fr_filter	, DIR_REPORTS_PRES	6	-
assert	, true	6	a
olap_uslchop	, 1	5	g=olap
view	, m140_filter, m140view, verstka_1	4	-
tagforobj	, 1	4	a
pr_decimals	, 0, 2	4	-
osn_prod		4	a
fstr	, 1	4	a
useosv	, 1	3	a
SWK	, m1301200, m1302100	3	a=v
prot	, pr	3	-
pr_linethikness	, 4	3	-
pr_filter	, m140	3	-
olap__nds19	, 1	3	g=olap
formfilter	, 1	3	a
casplitdiv	, 1	3	a
biztran	, SALE90, SIR003, SIR1091	3	a=v
afdep	, m111, m112, this	3	a=v
type	, ;CE_KSG;, ;CE_SCR;CE_MZR;	2	a=v
ts_showroot	, true	2	-
sourcelink	, DS=Pd, NDSLINK=NDS	2	a=v
showvalues	, 0	2	a
rows	, m111, m260170,m260180	2	a=v
prtr	, 2, 3	2	a=v
prg_showvalues	, 0, 1	2	-
prb	, 1	2	a
olap_tep		2	g=olap
objset	, 536,467	2	-
masterrow	, true	2	a
castruc	, CA_STRUCT	2	a=v
carole	, DK_PP, DK_PZ	2	a=v
CALC_TP	, z2505000, z2605000	2	a=v
CALC_RP	, z2506000, z2606000	2	a=v
usedoughnut2d	, 1	1	-
usedoughnut	, 1	1	-
usearea	, 1	1	-
ts_rootrow	, m203200	1	-
total	, 1	1	a
tagfordetail	, 1	1	a
roleprefix	, FIN	1	a=v
protocolview	, m140table	1	-
primary	, 0	1	a
prg_showsum	, 1	1	-
ppvalues	, dfs	1	-
pptarget	, nkls	1	-
ppsource	, sdlfj	1	-
pplevel	, 1	1	-
ppformat	, sdlj	1	-
olap_sir	, 1	1	g=olap
KOLDIV1000	, 1	1	a
deplist	, this	1	a=v
dependent	, 0	1	a
checklimit	, 1.0	1	a
bt	, mtr	1	a=v
aliased	, dscd	1	a=v


        };
    }
}







