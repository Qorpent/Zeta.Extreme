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
				title:'Работа с параметрами',
				icon : 'icon-wrench',
				items : [
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
				title : 'Работа с колсетами',
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
			}
		]
	}
});