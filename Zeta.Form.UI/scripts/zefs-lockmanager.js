/**
 * Виджет менеджера блокировок
 */
!function($) {
    var zefsblockmanager = new root.security.Widget("zefsblockmanager", root.console.layout.position.layoutHeader, "left", { authonly: true, priority: 40 });
    var list = $('<div class="btn-group"/>');
    var menu = $('<ul class="dropdown-menu"/>');
    var checkbtn = $('<button class="btn btn-success btn-mini"/>').text("Утв.");
    var lockbtn = $('<button class="btn btn-warning btn-mini"/>').text("Заблок.");
    var unlockbtn = $('<button class="btn btn-danger btn-mini"/>').text("Разблок.");
    var progress = $('<img src="images/300.gif"/>').css("margin-left", 3).hide();
    checkbtn.click(function() {
        window.zefs.myform.checkform();
        progress.show();
    });
    lockbtn.click(function() {
        window.zefs.myform.lockform();
        progress.show();
    });
    unlockbtn.click(function() {
        window.zefs.myform.unlockform();
        progress.show();
    });
    var b = $('<button class="btn btn-small dropdown-toggle" data-toggle="dropdown" data-original-title="Управление блокировками"/>')
        .html('<i class="icon-lock"></i><span class="caret"/>');
    var controls = $('<li/>');
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
        if (lock.haslockrole) {
            controls.append(lockbtn);
            if (lock.canblock) {
                lockbtn.addClass("btn-warning");
                lockbtn.removeAttr("disabled");
            }
            else lockbtn.attr("disabled", "disabled");
            controls.append(unlockbtn);
            if (lock.canopen) {
                unlockbtn.addClass("btn-danger");
                unlockbtn.removeAttr("disabled");
            }
            else unlockbtn.attr("disabled", "disabled");
        }
        if (lock.hasholdlockrole) {
            controls.append(checkbtn);
            if (lock.cancheck) {
                checkbtn.addClass("btn-success");
                checkbtn.removeAttr("disabled");
            }
            else checkbtn.attr("disabled", "disabled");
        }
        if (!lock.haslockrole && !lock.hasholdlockrole) {
            controls.append($('<span style="margin: 0 7px;" class="label label-warning"/>').text("Вы не можете управлять блокировками"));
        }
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
    $(document).on('click.dropdown.data-api', '.zefsblockmanager li', function (e) {
        e.stopPropagation();
    });
    window.zefs.api.lock.set.onComplete(function() {
        progress.hide();
    });
    window.zefs.api.lock.history.onSuccess(function(e, result) {
        if(!$.isEmptyObject(result)) {
            window.zefs.lockhistory = result;
        }
        var body = $(h.find('tbody'));
        var hist = window.zefs.lockhistory;
        if (hist) {
            if (!$.isEmptyObject(hist)){
                body.empty();
                //.sort(function(a,b) { return a.Date < b.Date })
                $.each(hist, function(i,h) {
                    if (i == 10) {
                        body.append($('<tr colspan="3"/>')
                            .css("text-align", "center")
                            .text("Еще " + ($.map(hist, function(n, i) { return i; }).length - i + 1)));
                        return;
                    }
                    else if (i > 10) return;
                    var lockstate = $('<span/>');
                    if (h.State == "0ISOPEN") lockstate.text("Разблок.").addClass("state-open");
                    else if (h.State == "0ISBLOCK") lockstate.text("Заблок.").addClass("state-block");
                    else if (h.State == "0ISCHECKED") lockstate.text("Пров.").addClass("state-check");
                    var u = $('<span class="label label-inverse"/>').text(h.User);
                    body.append($('<tr/>').append(
                        $('<td/>').text(h.Date.format("dd.mm.yyyy HH:MM:ss")),
                        $('<td/>').html(lockstate),
                        $('<td/>').append(u)
//                      $('<td/>').append($('<span class="label label-inverse"/>').text(h.getUser())),
                    ));
                    u.zetauser();
                    lockstate = null;
                });
            }
        }
        body = hist = null;
    });
    b.tooltip({placement: 'bottom'});
    zefsblockmanager.body = $('<div/>').append(list);
    root.console.RegisterWidget(zefsblockmanager);
}(window.jQuery);