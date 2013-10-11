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
                    href: './zdev_security/admins.html.qweb?__xslt=zdev-security-admins',
                },

                {
                    title: 'Анализ проблем настройки префиксов (по умолчанию)',
                    href: './zdev_security/prefixproblems.html.qweb?__xslt=zdev-security-prefixproblems',
                },
				{
                 title: "Анализ проблем настройки префиксов (настраиваемый)",
                 onclick: {
                     modal: { title: "Анализ проблем настройки префиксов", template: 'zdev_prefix_analyze', width: 450 },
                 },
             },

		
		]
	}
});