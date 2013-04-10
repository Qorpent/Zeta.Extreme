/**
 * Виджет ленты сообщений
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var zefs = window.zefs || {};
    var chat = new root.Widget("zefschat", root.console.layout.position.layoutHeader, "left", { authonly: true, priority: 97, ready: function() {
        chat.body.find('.btn-group').floatmenu();
    } });
    var b = $('<button class="btn btn-small dropdown-toggle" data-toggle="dropdown" data-original-title="Лента сообщений"/>')
        .html('<i class="icon-comment"></i>');
    var menu = $('<div class="dropdown-menu"/>').css({"padding": 5, "width": 400});
    var progress = $('<img src="images/300.gif"/>').hide();
    var refresh = $('<button class="btn btn-mini"/>').append($('<i class="icon-repeat"/>'));
    refresh.click(function() {
        zefs.myform.chatlist();
        refresh.hide(); progress.show();
    });
    var chatform = $('<form method="post"/>');
    var chatinput = $('<textarea type="text" name="text" placeholder="Текст сообщения..." class="input-small"/>').css("height", 32);
    var chatadd = $('<button type="submit" class="btn btn-mini btn-primary"/>').append($('<i class="icon-white icon-pencil"/>'));
    chatform.append($('<div class="chat-input"/>').append(chatinput, chatadd, refresh));
    chatform.submit(function(e) {
        e.preventDefault();
        if (chatinput.val() != "") {
            zefs.myform.chatadd(chatinput.val());
            refresh.hide(); progress.show();
            chatinput.val("");
        }
    });
    var chatlist = $('<div class="chat-list"/>');
    chatlist.append(
        $('<div class="chat-list-header"/>').append("Лента сообщений формы", progress),
        $('<div class="chat-list-body"/>').text("Пока сообщений нет")
    );
    $(zefs).on(zefs.handlers.on_chatlistload, function(e, cl) {
        refresh.show(); progress.hide();
        var body = $(chatlist.find('.chat-list-body'));
        if (cl != null && !$.isEmptyObject(cl)) {
            body.empty();
            b.addClass("btn-success");
            b.find("i").addClass("icon-white");
            $.each(cl, function(i,message) {
                var tr = $('<div class="chat-list-row"/>');
                var u = $('<span class="label label-inverse"/>');
                body.append(tr.append(
                    $('<div class="chat-list-cell username"/>').append(u.text(message.User)),
                    $('<div class="chat-list-cell date"/>').text(message.Date.format("dd.mm.yyyy HH:MM")),
                    $('<div class="chat-list-cell message"/>').text(message.Text)
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
    b.tooltip({placement: 'bottom'});
    $(document).on('click.dropdown.data-api', '.zefschat>div', function (e) {
        // e.preventDefault();
        e.stopPropagation();
    });
    chat.body = $('<div/>').append($('<div class="btn-group"/>').append(b,menu));
    root.console.RegisterWidget(chat);
}(window.jQuery);