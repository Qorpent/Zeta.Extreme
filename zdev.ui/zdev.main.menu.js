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
					}
					
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
					}

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
		    "div",
		    {
		        title: "Экспорт дерева",
		        href : './zdev/exporttree.form.qweb?render=string'
		    },
			"div",
			{
				title : "Очистить кэш",
				onclick : { 
					command : "zdev.dropcache" , 
					onsuccess : { modal : { title : 'Внимание' , text : 'Кэш сброшен' ,fade: true, width: 200 } } 
				}
			}
		]
	}
});