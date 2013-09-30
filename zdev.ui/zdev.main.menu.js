/**
 * Виджет формы запроса документации wiki
 */
_.widget.register({
	name : "zdev_main_menu",
	title :"Коммады для разработчика Zeta",
	type : "menu",
	position : "header",
	icon : "icon-retweet",
	menu : {
		items : [
			{
				title:'Параметры отчетов',
				icon : 'icon-wrench',
				items: [
				    {
				        title: 'Список параметров (полный в XML)',
				        href: './zdev/params.xml.qweb',
				    },
				    {
				        title: 'Список параметров (полный в HTML)',
				        href: './zdev/params.html.qweb?__xslt=zdev-parameters-report',
				    },
					{
						title : 'Атрибуты параметров (полный в XML)',
						href : './zdev/paramattributes.xml.qweb',
					},
					{
						title : 'Атрибуты параметров (полный в HTML)',
						href : './zdev/paramattributes.html.qweb?__xslt=zdev-paramattributes-report',
					}
				]
			},
			{
				title : 'Колсеты',
				icon : 'icon-list-alt',
				items : [
					{
						title : 'Атрибуты колсетов (полный в XML)',
						href : './zdev/colattributes.xml.qweb',
					},
					{
						title : 'Атрибуты колсетов (полный в HTML)',
						href : './zdev/colattributes.html.qweb?__xslt=zdev-colattributes-report',
					},
				    "div",
				     {
				         title: "Экспорт периодов",
				         href: './zdev/exportperiods.string.qweb',
				     },
		              {
		                  title: "Периоды, проблемные в использовании",
		                  href: './zdev/analyzeperiodsusage.xml.qweb?ProblemsOnly=true',
		              },
		             {
		                 title: "Экспорт колонок",
		                 href: './zdev/exportcolumns.string.qweb',
		             },
		            {
		                title: "Колонки, проблемные в использовании",
		                href: './zdev/analyzecolumnsusage.xml.qweb?ProblemsOnly=true',
		            },
					
				]
			},
		    {
		        title: 'Отчеты',
		        icon: 'icon-list-alt',
		        items: [
					{
					    title: 'Атрибуты отчетов (полный в XML)',
					    href: './zdev/reportattributes.xml.qweb',
					},
					{
					    title: 'Атрибуты отчетов (полный в HTML)',
					    href: './zdev/reportattributes.html.qweb?__xslt=zdev-reportattributes-report',
					}

		        ]
		    },
		    {
		        title: 'Формы',
		        icon: 'icon-list-alt',
		        items: [
					{
					    title: 'Атрибуты форм (полный в XML)',
					    href: './zdev/formattributes.xml.qweb',
					},
					{
					    title: 'Атрибуты форм (полный в HTML)',
					    href: './zdev/formattributes.html.qweb?__xslt=zdev-formattributes-report',
					},
		          "div",
		            {
		                title: "Экспорт дерева",
		                onclick: {
		                    modal: { title: "Экспорт дерева", template:'zdev_export_tree_form' , width:400 },
		                },
		        
		            },
		            {
		                title: "График зависимостей формы",
		                onclick: {
		                    modal: { title: "График зависимостей формы", template: 'zdev_export_fdep_dot', width: 400 },
		                },
		            },
		    {
		        title: "График зависимостей строки",
		        onclick: {
		            modal: { title: "График зависимостей строки", template: 'zdev_export_dep_dot', width: 400 },
		        },
		    },

		        ]
		    },
		    {
		        title: 'Темы',
		        icon: 'icon-list-alt',
		        items: [
					{
					    title: 'Атрибуты тем (полный в XML)',
					    href: './zdev/themaattributes.xml.qweb',
					},
					{
					    title: 'Атрибуты тема (полный в HTML)',
					    href: './zdev/themaattributes.html.qweb?__xslt=zdev-themaattributes-report',
					},
		             {
		                 title: "Экспорт бизпроцессов",
		                 href: './zdev/exportbizprocesses.string.qweb',
		             },
		            
                     {
                         title: "Экспорт структуры тем",
                         href: './zdev/exportthemastructure.string.qweb',
                     },

		        ]
		    },
			{
		        title: 'Substitutions',
		        icon: 'icon-list-alt',
		        items: [
					{
					    title: 'Subst атрибутов (полный в XML)',
					    href: './zdev/substattributes.xml.qweb?__xslt=zdev-substattributes-report',
					},
					{
					    title: 'Subst атрибутов (полный в HTML)',
					    href: './zdev/substattributes.html.qweb?__xslt=zdev-substattributes-report',
					}
				]
		    },
		    {
		        title: 'Элементы кода',
		        items: [
					{
					    title: 'Типы элементов (полный в XML)',
					    href: './zdev/getelementsmap.xml.qweb',
					},
					{
					    title: 'Типы элементов (полный в HTML)',
					    href: './zdev/getelementsmap.html.qweb?__xslt=zdev-getelementsmap-report',
					}

		        ]
		    },
		{
		    title: 'Объекты',
		    items: [
		        {
		            title: "Экспорт  объектов",
		            href: './zdev/exportobjects.string.qweb',
		        },
		         {
		             title: "Экспорт типов объектов",
		             href: './zdev/exportobjtypes.string.qweb',
		         },
		     {
		         title: "Экспорт дивизионов",
		         href: './zdev/exportobjdivs.string.qweb',
		     },
		        {
		            title: "Экспорт географии",
		            href: './zdev/exportgeo.string.qweb',
		        },
		    ]
		},
		  "div",
		   
		   
		    
		    
		    {
		        title: "Отладка запроса",
		        onclick: {
		            modal: { title: "Отладка запроса", template: 'zdev_debug_query', width: 400 },
		        },
		    },
		    {
		        title: "Создать скрипт переноса формулы в первичную строку",
		        type: 'primary',
		        onclick: {
		            modal: {
		                title: "Скрипт переноса формулы в первичную строку",
		                template: 'zdev_transfer_data',
		                width: 600,
		            },
		        },
		    },
            "div",
		      {
		          title: 'Метрики',
		          items: [
                      {
                          title: 'Все, в формате XML',
                          href: './zdev/getmetrics.xml.qwebb',
                      },
                      {
                          title: 'Все в формате HTML',
                          href: './zdev/getmetrics.html.qweb?__xslt=zdev-metrics-report',
                      }

		          ]
		      },

			"div",
			{
				title : "Очистить кэш",
				onclick : { 
					command : "zdev.dropcache" , 
					onsuccess: {
					    modal: { title: 'Внимание', text: 'Кэш сброшен', fade: true, width: 200 }
					}
				}
			}
		]
	}
});