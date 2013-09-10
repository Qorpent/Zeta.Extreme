/**
 * Виджет формы запроса документации wiki
 */
_.widget.register({
	name : "zdev_security_menu",
	title :"Анализ безопасности",
	type : "menu",
	position : "header",
	icon : "icon-lock",
	menu : {
		items : [
			
                {
                    title: 'Анализ настройки администраторов предприятий',
                    href: './zdev_security/admins.xml.qweb',
                },


		
		]
	}
});