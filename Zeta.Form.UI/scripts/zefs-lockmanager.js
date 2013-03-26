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
    $(window.zefs).on(window.zefs.handlers.on_getcanlockload, function() {
//        if (window.zefs.myform.canlock.canblock) {
          menu.prepend($('<li/>').append(lockbtn,checkbtn,unlockbtn, progress));
//        }
    });
    var h = $('<table class="table table-striped"/>');
    $(window.zefs).on(window.zefs.handlers.on_getlockload, function() {
        progress.hide();
        var lock =  window.zefs.myform.lock;
        if (lock != null) {
            b.get(0).className = "btn btn-small dropdown-toggle";
            lockbtn.get(0).className = "btn-warning btn btn-mini";
            unlockbtn.get(0).className = "btn-danger btn btn-mini";
            checkbtn.get(0).className = "btn-success btn btn-mini";
            lockbtn.removeAttr("disabled","disabled");
            unlockbtn.removeAttr("disabled","disabled");
            checkbtn.removeAttr("disabled","disabled");
            if (lock.state == "0ISOPEN"){
                b.addClass("btn-danger");
                unlockbtn.removeClass("btn-danger").attr("disabled","disabled");
                checkbtn.removeClass("btn-success").attr("disabled","disabled");
            }
            else if (lock.state == "0ISBLOCK") {
                b.addClass("btn-warning");
                lockbtn.removeClass("btn-warning").attr("disabled","disabled");
            }
            else if (lock.state == "0ISCHECKED") {
                b.addClass("btn-success");
                lockbtn.removeClass("btn-warning").attr("disabled","disabled");
                checkbtn.removeClass("btn-success").attr("disabled","disabled");
            }
            $(b.find("i")).addClass("icon-white");
        }
    });
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
                    var lockstate = $('<span/>').text(h.ReadableState);
                    if (h.State == "0ISOPEN") lockstate.addClass("state-open");
                    else if (h.State == "0ISBLOCK") lockstate.addClass("state-block");
                    else if (h.State == "0ISCHECKED") lockstate.addClass("state-check");
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