(function(){
var root = window.zeta = window.zeta || {};
var options = root.options = root.options || {};
$.extend(options,(function(){
	return {
		login_command : "_sys/login.json.qweb",
		logout_command : "_sys/logout.json.qweb",
		impersonate_command : "_sys/impersonate.json.qweb",
		whoami_command : "_sys/whoami.json.qweb",
			
		getParameters : function(){
			return null;
		},

        asAuth : function(obj) {
            $.extend(obj, {
                // признак того, что пользователь аутентифицирован
                getIsLogin : function() {return this.authenticated},
                // логин пользователя
                getLoginName : function() {return this.login},
                // ошибка при аутентификации (если она есть)
                getError : function() {return this.errortype},
                // сообщение ошибки
                getErrorMessage : function() {return this.errormessage}
            });
            return obj;
        },

		asUserAuth : function (obj) {
			$.extend(obj,{
				// реальное имя с которым входит пользователь
				getRealLogonName : function(){return this.logonname},
				// реальный признак того, что пользователь является администратором
				getRealIsAdmin : function(){return this.logonadmin},
				// реальный признак того, что пользователь является разработчиком
				getRealIsDeveloper : function(){return this.logondeveloper},
				// реальный признак того, что пользователь является администратором данных
				getRealIsDataMaster : function(){return this.logondatamaster},
				// имя с которым входит пользователь
				getLogonName : function(){return this.impersonation != null ? this.impersonation : this.logonname},
				// признак того, что пользователь является администратором
				getIsAdmin : function(){return this.impadmin != null ? this.impadmin : this.logonadmin},
				// признак того, что пользователь является разработчиком
				getIsDeveloper : function(){return this.impdeveloper != null ? this.impdeveloper : this.logondeveloper},
				// признак того, что пользователь является администратором данных
				getIsDataMaster : function(){return this.impdatamaster != null ? this.impdatamaster : this.logondatamaster},
                // возвращает логин пользователя за которого был осуществлен вход (если был)
                getImpersonation : function() {return this.impersonation || null},
                getIsImpAdmin : function(){return this.impadmin || null},
                getIsImpDeveloper : function(){return this.impdeveloper || null},
                getIsImpDataMaster : function(){return this.impdatamaster || null},
                data : {},
                prepare : function() {
                    this.data.Connection = this["header:Connection"];
                    this.data.Accept = this["header:Accept"];
                    this.data.AcceptCharset = this["header:Accept-Charset"];
                    this.data.AcceptEncoding = this["header:Accept-Encoding"];
                    this.data.AcceptLanguage = this["header:Accept-Language"];
                    this.data.Cookie = this["header:Cookie"];
                    this.data.Host = this["header:Host"];
                    this.data.UserAgent = this["header:User-Agent"];
                    this.data.Cookie = this["cookie:.ASSOIROOT"];
                }
			});
            obj.prepare();
			return obj;
		}
	}
	
})())
})();
