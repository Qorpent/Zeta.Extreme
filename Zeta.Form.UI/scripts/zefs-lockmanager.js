/**
 * Виджет менеджера блокировок
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var zefsblockmanager = new root.Widget("zefsblockmanager", root.console.layout.position.layoutHeader, "left", { authonly: true, priority: 99 });
    var list = $('<div class="btn-group"/>');
    var list2 = $('<div class="btn-group"/>');
    var menu = $('<ul class="dropdown-menu"/>');
    var checkbtn = $('<button class="btn btn-success btn-mini"/>').text("Утв.");
    var lockbtn = $('<button class="btn btn-warning btn-mini"/>').text("Заблок.");
    var unlockbtn = $('<button class="btn btn-danger btn-mini"/>').text("Разблок.");
    var checkbtn2 = $('<button class="btn btn-small" />').text("Утв.");
    var lockbtn2 = $('<button class="btn btn-small" />').text("Заблок.");
    var unlockbtn2 = $('<button class="btn btn-small" />').text("Разблок.");
    var progress = $('<img src="images/300.gif"/>').css("margin-left", 3).hide();
    var b = $('<button class="btn btn-small dropdown-toggle" data-original-title="Управление блокировками"/>')
        .html('<i class="icon-lock"></i><span class="caret"/>');
    var controls = $('<li/>');
    list2.append(lockbtn2);
    list2.append(unlockbtn2);
    list2.append(checkbtn2);
    $(window.zefs).on(window.zefs.handlers.on_getlockload, function() {
        var lock =  window.zefs.myform.lock;
        b.get(0).className = "btn btn-small dropdown-toggle";
        if (lock.state == "0ISOPEN"){
            b.addClass("btn-danger");
        }
        else if (lock.state == "0ISBLOCK") {
            b.addClass("btn-warning");
        }
        else if (lock.state == "0ISCHECKED") {
            b.addClass("btn-success");
        }
        $(b.find("i")).addClass("icon-white");
        controls.empty();
        lockbtn.get(0).className = unlockbtn.get(0).className = checkbtn.get(0).className = "btn btn-mini";
        lockbtn2.get(0).className = unlockbtn2.get(0).className = checkbtn2.get(0).className = "btn btn-small";
        if (lock.haslockrole) {
            controls.append(lockbtn);
            if (lock.canblock) {
                lockbtn.addClass("btn-warning");
                lockbtn2.addClass("btn-warning");
            }
            controls.append(unlockbtn);
            if (lock.canopen) {
                unlockbtn.addClass("btn-danger");
                unlockbtn2.addClass("btn-danger");
            }
        }
        if (lock.hasholdlockrole) {
            controls.append(checkbtn);
            if (lock.cancheck) {
                checkbtn.addClass("btn-success");
                checkbtn2.addClass("btn-success");
            }
        }
        if (!lock.haslockrole && !lock.hasholdlockrole) {
            controls.append($('<span style="margin: 0 7px;" class="label label-warning"/>').text("Вы не можете управлять блокировками"));
        }
        lockbtn.click(function() { window.zefs.myform.lockform(); progress.show(); });
        lockbtn2.click(function() { window.zefs.myform.lockform(); progress.show(); });
        unlockbtn.click(function() { window.zefs.myform.unlockform(); progress.show(); });
        unlockbtn2.click(function() { window.zefs.myform.unlockform(); progress.show(); });
        checkbtn.click(function() { window.zefs.myform.checkform(); progress.show(); });
        checkbtn2.click(function() { window.zefs.myform.checkform(); progress.show(); });
        progress.hide();
        controls.append(progress);
        menu.prepend(controls);
    });
    var h = $('<table class="table table-striped"/>');
    menu.append($('<li/>').append(h));
    h.append(
        $('<thead/>').append($('<tr/>').append($('<th colspan="3"/>').text("История блокировок"))),
        $('<tbody/>').append($('<tr/>').append($('<td colspan="3"/>').text("Форма не блокировалась")))
    );
    h.css({"margin":"0 5px", "width": 210});
    list.append(b,menu);
    b.dropdownHover({delay: 100});
    $(document).on('click.dropdown.data-api', '.zefsblockmanager li', function (e) {
        e.stopPropagation();
    });
    window.zefs.api.lock.set.onComplete(function() {
        progress.hide();
    });
    window.zefs.api.lock.setstateold.onComplete(function() {
        progress.hide();
    });
    window.zefs.api.lock.set.onError(function(e, result) {
        progress.hide();

    });
    window.zefs.api.lock.setstateold.onError(function(e, result) {
        progress.hide();
        
    });
    window.zefs.api.lock.history.onSuccess(function(e, result) {
        if(!$.isEmptyObject(result)) {
            window.zefs.lockhistory = result;
        }
        var hist = zefs.lockhistory;
        if (hist) {
            if (!$.isEmptyObject(hist)){
                //.sort(function(a,b) { return a.Date < b.Date })
                zeta.zetauser.getDetails($.unique($.map(hist, function(h) { return h.User })).join(","), function(users) {
                    var body = $(h.find('tbody'));
                    body.empty();
                    $.each(zefs.lockhistory, function(i, h) {
                        if (i == 10) {
                            body.append($('<tr colspan="3"/>')
                                .css("text-align", "center")
                                .text("Еще " + ($.map(zefs.lockhistory, function(n, i) { return i; }).length - i + 1)));
                            return;
                        }
                        else if (i > 10) return;
                        var lockstate = $('<span/>');
                        if (h.State == "0ISOPEN") lockstate.text("Разблок.").addClass("state-open");
                        else if (h.State == "0ISBLOCK") lockstate.text("Заблок.").addClass("state-block");
                        else if (h.State == "0ISCHECKED") lockstate.text("Пров.").addClass("state-check");
                        var u = $('<span class="label label-inverse"/>').text(users[h.User.toLowerCase()].ShortName);
                        var tr = $('<tr/>').append(
                            $('<td/>').text(h.Date.format("dd.mm.yyyy HH:MM:ss")),
                            $('<td/>').html(lockstate),
                            $('<td/>').append(u)
                        );
                        u.click(function() { zeta.zetauser.renderDetails(users[h.User.toLowerCase()]) });
                        var c = $('<td/>');
                        if (!!h.Comment) {
                            c.append('<i class="icon icon-comment"/>');
                            c.click(function() {
                                $(window.zeta).trigger(window.zeta.handlers.on_modal,
                                    { title: "Комментарий", text: h.Comment, width: 450 });
                            });
                        }
                        tr.append(c);
                        body.append(tr);
                        lockstate = null;
                    });
                });
            }
        }
    });
    b.tooltip({placement: 'bottom'});
    zefsblockmanager.body = $('<div/>').append(list, list2);
    root.console.RegisterWidget(zefsblockmanager);
}(window.jQuery);