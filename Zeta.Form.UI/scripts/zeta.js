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

(function($) {
    root.user = root.user || {};
    root.auth = root.auth || {};

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
                layoutTools : "tools",
//              layoutBodyLeft : "body-left",
                layoutBodyMain : "body-main",
                layoutPageHeader : "pageheader"
            },
            header : $('<div id="consoleHeader"/>').append(
                $('<div class="navbar"/>').append(
                    $('<div class="navbar-inner"/>')
                )
            ),
            tools : $('<div id="consoleTools" class="console-tools"/>'),
            pageheader : $('<div id="consolePageHeader" class="console-pageheader"/>'),
            body : $('<div id="consoleBody" class="console-body"/>'),
            footer : $('<div id="consoleFooter"/>'),
            add : function(w) {
                $(w.body).addClass(w.name);
                if (w.float != "none") $(w.body).addClass("pull-" + w.float);
                if (!!zeta.widgethelp) {
                    if (!!zeta.widgethelp[w.name]) {
                        $(w.body).addClass("widget");
                        $(w.body).append($('<span class="wikirowhelp"/>').click(function(e) { e.stopPropagation(); zefs.api.wiki.getsync.execute({code: zeta.widgethelp[w.name]}); }));
                    }
                }
                if (w.pos == this.position.layoutHeader) $('#consoleHeader > .navbar > .navbar-inner').append(w.body);
                if (w.pos == this.position.layoutBodyMain) $('#consoleBody').append(w.body);
                if (w.pos == this.position.layoutTools) $('#consoleTools').append(w.body);
                if (w.pos == this.position.layoutPageHeader) $('#consolePageHeader').append(w.body);
            }
        },

        Setup : function() {
            $('body').append(this.layout.header,this.layout.tools,this.layout.pageheader, this.layout.body,this.layout.footer);
            if (zeta.user.logonname != "") {
                root.api.wiki.getsync.execute({code: "/form/widgets"}, {
                    onsuccess: function(result) {
                        if (!result[0]) return;
                        if (!result[0].Text || result[0].Text == "") return;
                        zeta.widgethelp = {};
                        $.each(result[0].Text.split(/,\s+/), function(i,k) {
                            zeta.widgethelp[k.split(':')[0]] = k.split(':')[1];
                        })
                    }
                });
            }
            $.each(this.widgets.sort(function(a,b) { return b.options.priority - a.options.priority }),
                $.proxy(function(i, e) {
                    if ((root.user != null && root.user.getLogonName() != "" ) || !e.options.authonly) {
                        if (e.options.adminonly && root.user != null) {
                            if (!root.user.getIsAdmin()) return;
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
    };

    Console.prototype.whoami = function() {
        root.api.security.whoami.execute();
    };

    Console.prototype.authorize = function(l,p) {
        root.api.security.login.execute({ _l_o_g_i_n_: l, _p_a_s_s_: p });
    };

    Console.prototype.unauthorize = function() {
        root.api.security.logout.execute();
    };

    Console.prototype.impersonate = function(params) {
        root.api.security.impersonateall(params);
    };

    root.Widget = function(n, p, f, o) {
        this.name = n != null ? n : "widget";
        this.pos = p != null ? p : "none";
        this.float = f != null ? f : "none";
        this.options = $.extend({
            priority: 0,
            authonly: true,
            adminonly: false,
            help: null,
            // Функция которая вызывается после того как виджет добавлен
            ready: null
        }, o);
        this.body = null;
        this.installed = false;
    };

    root.api.security.whoami.onSuccess(function(e, result){
        root.user = result;
        $(window.zeta).trigger(root.handlers.on_getuserinfo);
        window.zeta.console.Setup();
    });

    root.api.security.impersonate.onSuccess(function(e, result){
        $(window.zeta).trigger(result != null ? root.handlers.on_impersonate : root.handlers.on_deimpersonate);
    });

    root.api.security.login.onSuccess(function(e, result){
        location.reload();
        root.auth = result;
        $(root).trigger(result.authenticated ? root.handlers.on_loginsuccess : root.handlers.on_loginfaild);
    });

    root.api.security.logout.onComplete(function(){
        $(root).trigger(root.handlers.on_logout);
        location.reload();
    });

    root.console = new Console();
})(jQuery);

(function($) {
    $(window).load(function() { window.zeta.console.whoami() });
})(jQuery);

(function ($) {
    $.fn.printelement = function () {
        return this.each(function () {
            var container = $(this);
            var hidden_IFrame = $('<iframe></iframe>').attr({
                width: '1px',
                height: '1px',
                display: 'none'
            }).appendTo(container);
            var myIframe = hidden_IFrame.get(0);
            var script_tag = myIframe.contentWindow.document.createElement("script");
            var style_tag = myIframe.contentWindow.document.createElement("style");
            style_tag.innerText = "@media print{.non-printable{display: none !important;}.printable{display: inherit !important;}}";
            script_tag.type = "text/javascript";
            var script = myIframe.contentWindow.document.createTextNode('function Print(){ window.print(); }');
            script_tag.appendChild(script);
            myIframe.contentWindow.document.body.innerHTML = container.html().replace(/display:\s?none;/g, '');
            myIframe.contentWindow.document.body.appendChild(script_tag);
            myIframe.contentWindow.document.body.appendChild(style_tag);
            myIframe.contentWindow.Print();
            hidden_IFrame.remove();
        });
    };
})(jQuery);