var siteroot = document.location.pathname.match("^/([\\w\\d_\-]+)?/")[0];
var root = window.zeta = window.zeta || {};
root.handlers = $.extend(root.handlers, {
    // Zeta handlers:
    on_zetaready : "zetaready",
    on_zetafailed : "zetafailed",
    // Login handlers:
    on_loginsuccess : "loginsuccess",
    on_loginfaild : "loginfaild",
    on_logout : "logount",
    on_impersonate : "impersonate",
    on_deimpersonate : "deimpersonate",
    on_getuserinfo : "getuserinfo"
});

root.security = root.security || $.extend(root.security, {
    user : null,
    auth : null
});

!function($) {
    var options = window.zeta.options;

    var Console = function() {

    };

    Console.prototype = {
        widgets : [],
        RegisterWidget : function(e) {
            this.widgets.push(e);
        },
        // Setup layout
        layout : {
            position : {
                layoutHeader : "header",
//              layoutBodyLeft : "body-left",
                layoutBodyMain : "body-main"
            },
            header : $('<div id="consoleHeader"/>').append(
                $('<div class="navbar"/>').append(
                    $('<div class="navbar-inner"/>')
                )
            ),
            body : $('<div id="consoleBody" class="console-body"/>'),
            /*.append($('<div/>').append($('<section/>'))
                $('<div class="span3 console-sidebar" />').append($('<ul class="nav nav-list"/>')),
                 $('<div class="span9" />').append($('<section/>'))
            ),*/
            footer : $('<div id="consoleFooter"/>'),
            add : function(e) {
                $(e.body).addClass(e.name)
                if (e.float != "none") $(e.body).addClass("pull-" + e.float)
                if (e.pos == this.position.layoutHeader) $('#consoleHeader > .navbar > .navbar-inner').append(e.body);
                //  if (e.pos == this.position.layoutBodyLeft) $('#consoleBody > .row > .span3 > .nav-list').append(e.body);
                if (e.pos == this.position.layoutBodyMain) $('#consoleBody').append(e.body);
            }
        },

        Setup : function() {
            $('body').append(this.layout.header,this.layout.body,this.layout.footer);
            $.each(this.widgets.sort(function(a,b) { return b.options.priority - a.options.priority }),
                $.proxy(function(i, e) {
                    if ((root.security.user != null && root.security.user.getLogonName() != "" ) || !e.options.authonly) {
                        if (e.options.adminonly && root.security.user != null) {
                            if (!root.security.user.getIsAdmin()) return;
                        }
                        if (!this.widgets[i].installed) {
                            this.layout.add(e);
                            if (e.options.ready != null) e.options.ready();
                            this.widgets[i].installed = true;
                        }
                    }
                }, this)
            );
        },

        Uninstallwidgets: function() {
            $.each(this.widgets, function() {
                if (this.installed && this.options.authonly) {
                    this.body.remove();
                    this.installed = false;
                }
            }, this);
        }
    }

    Console.prototype.whoami = function() {
        $.ajax({
            url: siteroot+root.options.whoami_command,
            context: this,
            dataType: 'json'
        }).success($.proxy(function(d) {
            root.security.user = root.options.asUserAuth(d);
            $(root).trigger(root.handlers.on_getuserinfo);
            window.zeta.console.Setup();
        }, this));
    };

    Console.prototype.authorize = function(l,p) {
        $.ajax({
            url: siteroot+root.options.login_command,
            type: "POST",
            context: this,
            data: {
                _l_o_g_i_n_: l,
                _p_a_s_s_: p
            },
            dataType: 'json'
        }).success($.proxy(function(d) {
            var auth = root.options.asAuth(d);
            root.security.auth = root.options.asAuth(d);
            $(root).trigger(auth.getIsLogin() ? root.handlers.on_loginsuccess : root.handlers.on_loginfaild);
        }, this));
    };

    Console.prototype.unauthorize = function() {
        $.ajax({
            url: siteroot+root.options.logout_command,
            context: this,
            dataType: 'json'
        }).complete($.proxy(function(d) {
            $(root).trigger(root.handlers.on_logout);
        }, this));
    };

    Console.prototype.impersonate = function(l) {
        $.ajax({
            url: siteroot+root.options.impersonate_command,
            type: "POST",
            context: this,
            data: { Target : l},
            dataType: 'json'
        }).success($.proxy(function(d) {
            $(root).trigger(l != null ? root.handlers.on_impersonate : root.handlers.on_deimpersonate);
        }, this));
    };

    root.security.Widget = function(n, p, f, o) {
        this.name = n != null ? n : "widget";
        this.pos = p != null ? p : "none";
        this.float = f != null ? f : "none";
        this.options = $.extend({
            priority: 0,
            authonly: true,
            adminonly: false,
            // Функция которая вызывается после того как виджет добавлен
            ready: null
        }, o);
        this.body = null;
        this.installed = false;
    };

    root.console = new Console();
}(window.jQuery);

/*!function($) {
    $.getScript("scripts/zefs-debug.js");
    $.getScript("scripts/zefs-auth.js");
    $.getScript("scripts/zefs-authinfo.js");
    $.getScript("scripts/zefs-formsave.js");
    $.getScript("scripts/zefs-feedback.js");
    $.getScript("scripts/zefs-info.js");
    $.getScript("scripts/zefs-formheader.js");
    $.getScript("scripts/zefs-table.js");
    $.getScript("scripts/zefs-attacher.js");
    $.getScript("scripts/zefs-periods.js");
    $.getScript("scripts/zefs-objs.js");
    $.getScript("scripts/zefs-colmanager.js");
    $.getScript("scripts/zefs-lockmanager.js");
    $.getScript("scripts/zefs-alerter.js");
}(window.jQuery);*/

document.write('<script src="scripts/zefs-debug.js" type="text/javascript"></script>');
document.write('<script src="scripts/zefs-restart.js" type="text/javascript"></script>');
document.write('<script src="scripts/zefs-auth.js" type="text/javascript"></script>');
document.write('<script src="scripts/zefs-authinfo.js" type="text/javascript"></script>');
document.write('<script src="scripts/zefs-formsave.js" type="text/javascript"></script>');
document.write('<script src="scripts/zefs-feedback.js" type="text/javascript"></script>');
document.write('<script src="scripts/zefs-info.js" type="text/javascript"></script>');
document.write('<script src="scripts/zefs-formheader.js" type="text/javascript"></script>');
document.write('<script src="scripts/zefs-extrapannel.js" type="text/javascript"></script>');
document.write('<script src="scripts/zefs-table.js" type="text/javascript"></script>');
document.write('<script src="scripts/zefs-attacher.js" type="text/javascript"></script>');
document.write('<script src="scripts/zefs-periods.js" type="text/javascript"></script>');
document.write('<script src="scripts/zefs-objs.js" type="text/javascript"></script>');
document.write('<script src="scripts/zefs-colmanager.js" type="text/javascript"></script>');
document.write('<script src="scripts/zefs-lockmanager.js" type="text/javascript"></script>');
document.write('<script src="scripts/zefs-alerter.js" type="text/javascript"></script>');
document.write('<script src="scripts/zeta-modal.js" type="text/javascript"></script>');
document.write('<script src="scripts/zefs-report.js" type="text/javascript"></script>');

!function($) {
    $(window).load(function() { window.zeta.console.whoami() });
}(window.jQuery);