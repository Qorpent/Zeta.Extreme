/**
 * Виджет ленты сообщений
 */
!function($) {
    var root = window.zeta = window.zeta || {},
        zefs = window.zefs || {},
        chat = new root.Widget("zefschat", root.console.layout.position.layoutHeader, "left", { authonly: true, priority: 97, ready: function() {
            chat.body.find('.btn-group').floatmenu();
            zefs.api.chat.updatecount.execute();
        }});
    var b = $('<button class="btn btn-small dropdown-toggle" data-toggle="dropdown" data-original-title="Лента сообщений"/>').html('<i class="icon-comment"></i>'),
        menu = $('<div class="dropdown-menu"/>').css({"padding": 5, "width": 400}),
        progress = $('<img src="images/300.gif"/>').hide(),
        refresh = $('<button class="btn btn-mini"/>').append($('<i class="icon-repeat"/>')),
        chatform = $('<form method="post"/>'),
        chatinput = $('<textarea type="text" name="text" placeholder="Текст сообщения..." class="input-small"/>').css("height", 32),
        chatadd = $('<button class="btn btn-mini btn-primary"/>').append($('<i class="icon-white icon-pencil"/>')),
        chatlist = $('<div class="chat-list scrollable"/>');
    b.click(function() {
        if ($(this).hasClass("hasunread")) {
            zefs.api.chat.haveread.execute();
            $(this).removeClass("hasunread");
            $(this).empty();
            $(this).html('<i class="icon-comment"></i>');
        }
    });
    chatinput.keydown(function(e) {
        if (e.keyCode == 13 && e.ctrlKey) chatform.trigger("submit");
    });
    chatadd.click(function() {
        chatform.trigger("submit");
    });
    chatform.append($('<div class="chat-input"/>').append(chatinput, chatadd, refresh, progress));
    chatform.submit(function(e) {
        e.preventDefault();
        if (chatinput.val() != "") {
            zefs.myform.chatadd(chatinput.val());
            refresh.hide(); progress.show();
            chatinput.val("");
        }
    });
    refresh.click(function() {
        zefs.myform.chatlist();
        refresh.hide();
        progress.show();
    });
    chatlist.append(
        $('<div class="userchat chat-list-header"/>').click(function() {
            $(chatlist.find('.userchat.chat-list-header')).toggleClass("collapsed");
        }).append("Лента сообщений формы"),
        $('<div class="userchat chat-list-body"/>')
    );
    var showarchive = $('<i class="icon pull-right" data-original-title="Показать/скрыть проченные"/>');
    if ($.isEmptyObject(root.chatoptionsstorage.Get())) {
        zeta.chatoptionsstorage.AddOrUpdate({showarchived: false});
        showarchive.addClass("icon-eye-open");
    }
    else if (root.chatoptionsstorage.Get()["showarchived"]) {
        showarchive.addClass("icon-eye-close");
    } else {
        showarchive.addClass("icon-eye-open");
    }
    showarchive.tooltip({placement: 'bottom',container: chatlist});
    showarchive.click(function(e) {
        e.stopPropagation();
        if ($(this).hasClass("icon-eye-open")) {
            zeta.chatoptionsstorage.AddOrUpdate({showarchived: true});
        } else {
            zeta.chatoptionsstorage.AddOrUpdate({showarchived: false});
        }
        $(this).toggleClass("icon-eye-open icon-eye-close");
        refresh.trigger("click");
    });
    var UpdateButtonStatus = function() {
        var adminchatbody = $(chatlist.find('.adminchat.chat-list-body')),
            userchatbody = $(chatlist.find('.userchat.chat-list-body'));
        if (adminchatbody.children().length == 0 && userchatbody.children().length == 0) {
            b.removeClass("btn-success");
            b.find("i").removeClass("icon-white");
        }
        adminchatbody = userchatbody = null;
    }
    // Количество новых сообщений
    $(zefs).on(zefs.handlers.on_adminchatcountload, function(e, count) {
        if (parseInt(count) > 0) {
            b.addClass("hasunread");
            b.append($('<strong/>').text(count));
        }
    });
    // Наполняем ленту сообщений
    $(zefs).on(zefs.handlers.on_chatlistload, function(e, cl) {
        progress.hide(); refresh.show();
        var body = $(chatlist.find('.userchat.chat-list-body'));
        if (cl != null && !$.isEmptyObject(cl)) {
            body.empty();
            b.addClass("btn-success");
            b.find("i").addClass("icon-white");
            $.each(cl, function(i,message) {
                var tr = $('<div class="userchat chat-list-row"/>');
                var u = $('<span class="label label-inverse"/>');
                body.append(tr.append(
                    $('<div class="userchat chat-list-cell username"/>').append(u.text(message.User)),
                    $('<div class="userchat chat-list-cell date"/>').text(message.Date.format("dd.mm.yyyy HH:MM")),
                    $('<div class="userchat chat-list-cell message"/>').text(message.Text)
                ));
                u.zetauser();
                tr = u = null;
            });
        }
        UpdateButtonStatus();
        body = cl = null;
    });
    // Наполняем АДМИНСКУЮ ленту сообщений
    $(zefs).on(zefs.handlers.on_adminchatlistload, function(e, cl) {
        progress.hide(); refresh.show();
        var body = $(chatlist.find('.adminchat.chat-list-body'));
        var header = $(chatlist.find('.adminchat.chat-list-header'));
        if (body.length == 0) {
            body = $('<div class="adminchat chat-list-body"/>');
            header = $('<div class="adminchat chat-list-header"/>').click(function() {
                $(chatlist.find('.adminchat.chat-list-header')).toggleClass("collapsed");
            });
            header.append(showarchive);
            chatlist.append(header.append("Все ваши сообщения"), body);
        }
        body.empty();
        if (cl != null && !$.isEmptyObject(cl)) {
            b.addClass("btn-success");
            b.find("i").addClass("icon-white");
            $.each(cl, function(i,message) {
                var tr = $('<div class="adminchat chat-list-row"/>');
                var u = $('<span class="label label-inverse"/>');
                var arch = $('<i class="icon icon-ok pull-right"/>');
                arch.click(function() {
                    zefs.myform.chatarchive(message.Id);
                });
                body.append(tr.append(
                    arch,
                    $('<div class="adminchat chat-list-cell username"/>').append(u.text(message.User)),
                    $('<div class="adminchat chat-list-cell date"/>').text(message.Date.format("dd.mm.yyyy HH:MM")),
                    $('<div class="adminchat chat-list-cell message"/>').append($('<a/>').click(
                        function(e) {
                            var cl = $(this).parents('.chat-list-row').data();
                            e.preventDefault();
                            zefs.myform.openform({form: cl.FormCode, period: cl.Period, obj: cl.ObjId, year: cl.Year}, true);
                        }
                    ).text(message.Text))
                ));
                // помечаем прочитанным
                if (!!message.Userdata) {
                    if (message.Userdata.archive) tr.addClass("archived");
                }
                tr.data(message);
                u.zetauser();
                tr = u = null;
            });
        }
        UpdateButtonStatus();
        body = cl = null;
    });
    menu.append(chatform, chatlist);
    b.tooltip({placement: 'bottom'});
    $(document).on('click.dropdown.data-api', '.zefschat>div', function (e) {
        e.preventDefault();
        e.stopPropagation();
    });
    chat.body = $('<div/>').append($('<div class="btn-group"/>').append(b,menu));
    root.console.RegisterWidget(chat);
}(window.jQuery);