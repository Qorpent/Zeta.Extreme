(function(){
    var qwiki = window.qwiki = window.qwiki || {};
    $.extend(qwiki, {
        getMENU : function(code) {
            var wiki = {};
            zefs.api.wiki.getmenu.execute({code: code}, {onsuccess: function(result) { wiki = result }});
            var toc = qwiki.toTOC(wiki[0].Text);

            var items = $(toc.items);
            if (!items || items.length == 0) return;

            return qwiki.renderMENU(items);
        },

        renderMENU : function(items) {
            var result = $('<ul class="dropdown-menu"/>');
            $.each(items, function(i, item) {
                if (item.raw.search("admin") != -1 && !zeta.user.getIsAdmin()) {
                    return;
                }
                var li = $('<li/>');
                if (item.raw == "----") {
                    li.addClass("divider");
                } else {
                    var link = item.title || item.raw;
                    if (link[0] == "<") {
                        link = $('<a/>').append($(link));
                    } else {
                        link = $('<a/>').text(link);
                    }
                    li.html(link);
                    if (!!item.addr) {
                        if (item.addr.length == 0) return;
                        var action = function() {};
                        if (item.addr[0] == "/") {
                            action = function() {
                                zefs.api.wiki.getsync.execute({code: item.addr});
                            }
                        }
                        if (/javascript:.+/.test(item.addr)) {
                            action = function() {
                                eval(item.addr.substring(11));
                            }
                        }
                        link.click(function(e) {
                            e.preventDefault();
                            action();
                        });
                    }
                }
                if (item.items.length > 0) {
                    var submenu = qwiki.renderMENU(item.items);
                    li.addClass("dropdown-submenu");
                    li.append(submenu);
                }
                result.append(li);
            });
            return result;
        }
    });
})();