/**
 * Виджет ленты сообщений
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var zefs = window.zefs || {};
    var chat = new root.Widget("zefschat attacher", root.console.layout.position.layoutHeader, "left", { authonly: true, priority: 97, ready: function() {
        chat.body.find('.btn-group').floatmenu();
    } });
    var b = $('<button class="btn btn-small dropdown-toggle" data-toggle="dropdown" data-original-title="Лента сообщений"/>')
        .html('<i class="icon-comment"></i>');
    var menu = $('<div class="dropdown-menu"/>').css({"padding": 5, "width": 450});
    var progress = $('<img src="images/300.gif" class="pull-right"/>').hide();
    var refresh = $('<button class="btn btn-mini pull-right"/>').text("Обновить");
    refresh.click(function() {
        zefs.myform.chatlist();
        refresh.hide(); progress.show();
    });
    var chatform = $('<form method="post"/>');
    var chatinput = $('<input type="text" name="text" placeholder="Текст сообщения..." class="input-small"/>');
    var chatadd = $('<input type="submit" class="btn btn-mini btn-primary pull-right"/>').val("Отправить сообщение");
    chatform.append(
        $("<table/>").append(
            $('<colgroup/>').append(
                $('<col/>').css("width",""),
                $('<col/>').css("width",140),
                $('<col/>').css("width",70)
            ),
            $('<tbody/>').append($('<tr/>').append(
                $('<td/>').append(chatinput),$('<td/>').append(chatadd),$('<td/>').append(refresh))
            )
        )
    );
    chatform.submit(function(e) {
        e.preventDefault();
        if (chatinput.val() != "") {
            zefs.myform.chatadd(chatinput.val());
            refresh.hide(); progress.show();
            chatinput.val("");
        }
    });
    var chatlist = $('<table class="table table-striped"/>');
    chatlist.append(
        $('<colgroup/>').append(
            $('<col/>').css("width",70),
            $('<col/>').css("width",230),
            $('<col/>').css("width",150)
        ),
        $('<thead/>').append($('<tr/>').append($('<th colspan="3"/>').append("Лента сообщений формы", progress))),
        $('<tbody/>').append($('<tr/>').append($('<td colspan="3"/>').text("Пока сообщений нет")))
    );
    $(zefs).on(zefs.handlers.on_chatlistload, function(e, cl) {
        refresh.show(); progress.hide();
        var body = $(chatlist.find('tbody'));
        if (cl != null && !$.isEmptyObject(cl)) {
            body.empty();
            b.addClass("btn-success");
            b.find("i").addClass("icon-white");
            $.each(cl, function(i,message) {
                var tr = $('<tr/>');
                var u = $('<span class="label label-inverse"/>');
                body.append(tr.append(
                    $('<td/>').text(message.Date.format("dd.mm.yyyy ")),
                    $('<td class="filename"/>').text(message.Text),
                    $('<td class="username"/>').append(u.text(message.User))
                ));
                u.zetauser();
                tr = u = null;
            });
        } else {
            body.html($('<tr/>').append($('<td colspan="10"/>').text("Пока сообщений нет")));
            b.removeClass("btn-success");
            b.find("i").removeClass("icon-white");
        }
        body = cl = null;
    });
    menu.append(chatform, chatlist);
    chat.body = $('<div/>').append($('<div class="btn-group"/>').append(b,menu));
    root.console.RegisterWidget(chat);
}(window.jQuery);