/**
 * Виджет менеджера блокировок
 */
!function($) {
    var zefsblockmanager = new root.security.Widget("zefsblockmanager", root.console.layout.position.layoutHeader, "left", { authonly: true, priority: 40 });
    var list = $('<div class="btn-group"/>');
    var checkbtn = $('<button class="btn btn-success btn-mini"/>').text("Утв.");
    var lockbtn = $('<button class="btn btn-warning btn-mini"/>').text("Заблок.").click(/*function() { window.zefs.myform.lockform } */);
    var unlockbtn = $('<button class="btn btn-danger btn-mini"/>').text("Разблок.");
    var b = $('<button class="btn btn-small dropdown-toggle" data-toggle="dropdown" data-original-title="Управление блокировками"/>')
        .html('<i class="icon-lock"></i><span class="caret"/>');
    var menu = $('<ul class="dropdown-menu"/>');
    menu.append($('<li/>').append(lockbtn,checkbtn,unlockbtn));
    var h = $('<table class="table table-striped"/>');
    $(window.zefs).on(window.zefs.handlers.on_getlockload, function() {
        var lock =  window.zefs.myform.lock;
        if (lock != null) {
            if (lock.getState() == "0ISOPEN"){
                b.addClass("btn-danger");
                unlockbtn.removeClass("btn-danger").attr("disabled","disabled");
                checkbtn.removeClass("btn-success").attr("disabled","disabled");
            }
            else if (lock.getState() == "0ISBLOCK") {
                b.addClass("btn-warning");
                lockbtn.removeClass("btn-warning").attr("disabled","disabled");
            }
            else if (lock.getState() == "0ISCHECKED") {
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
    $(window.zefs).on(window.zefs.handlers.on_getlockhistoryload, function() {
        var body = $(h.find('tbody'));
        var hist = window.zefs.myform.lockhistory;
        if (hist) {
            if (hist.length > 0){
                body.empty();
                $.each(hist.sort(function(a,b) { return a.getDate() < b.getDate() }), function(i,h) {
                    var lockstate = $('<span class="label"/>').text(h.getState());
                    if (h.getStateCode() == "0ISOPEN") lockstate.addClass("label-important");
                    else if (h.getStateCode() == "0ISBLOCK") lockstate.addClass("label-warning");
                    else if (h.getStateCode() == "0ISCHECKED") lockstate.addClass("label-success");
                    body.append($('<tr/>').append(
                        $('<td/>').text(h.getDate().format("dd.mm.yyyy HH:MM:ss")),
                        $('<td/>').text(h.getUser()),
                        $('<td/>').html(lockstate)
                    ));
                    lockstate = null;
                });
            }
        }
        body = hist = null;
    })
    b.tooltip({placement: 'bottom'});
    zefsblockmanager.body = $('<div/>').append(list);
    root.console.RegisterWidget(zefsblockmanager);
}(window.jQuery);