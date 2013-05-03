(function()    {
var root = window.zeta = window.zeta || {};
var api = root.api = root.api || {};
var Command = window.qweb.Command;

$.extend(api,(function(){
    return {
        security : {
            login : new Command({domain:"_sys", name:"login"}),
            logout : new Command({domain: "_sys", name: "logout"}),
            impersonate : new Command({domain: "_sys", name: "impersonate"}),
            impersonateall : function(params) {
                $.ajax({ url: "_sys/impersonate.qweb", data: params });
                var apps = ["zefs","zefs1","zefs2","zefs3","zefs4"];
                for (var i in apps) {
                    $.ajax({ url: "/" + apps[i] + "/_sys/impersonate.qweb", data: params });
                }
            },
            whoami : $.extend(new Command({domain: "_sys", name: "whoami"}), {
                wrap : function(obj) {
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
                        getIsImpDataMaster : function(){return this.impdatamaster || null}
                    });
                    return obj;
                }
            })
        },

        metadata : {
            userinfo : $.extend(new Command({domain: "zeta", name: "getuserinfo"}), {
                wrap : function(obj) {
                    var getShortName = function() {
                        var n = obj.Name.trim().split(" ");
                        if (obj.Name != "NOT REGISTERED IN DB" && n.length == 3) {
                            return n[0] + " " + n[1].substring(0,1) + ". " + n[2].substring(0,1) + ".";
                        } else {
                            return obj.Name;
                        }
                    }
                    $.extend(obj, {
                        ShortName : getShortName()
                    });
                    return obj;
                }
            })
        }
    }
})());
})();
