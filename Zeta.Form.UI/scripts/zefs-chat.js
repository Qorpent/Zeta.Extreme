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
        menu = $('<div class="dropdown-menu"/>').css({"padding": 5, "width": 650}),
        progress = $('<img src="images/300.gif"/>').hide(),
        refresh = $('<button class="btn btn-mini refresh-btn"/>').append($('<i class="icon-repeat"/>')),
        chatform = $('<form method="post"/>'),
        chatinput = $('<textarea type="text" name="text" placeholder="Текст сообщения..." class="input-small"/>').css("height", 32),
        lentaadd = $('<button class="btn btn-mini" data-original-title="Будет видимо в текущей форме"/>').text("В ленту"),
        formkuratoradd = $('<button class="btn btn-mini" data-original-title="Отправляется куратору формы"/>').text("Куратору формы"),
        objkuratoradd = $('<button class="btn btn-mini" data-original-title="Отправляется куратору предприятия"/>').text("Куратору предприятия"),
        supportadd = $('<button class="btn btn-mini" data-original-title="Отправляется поддержке"/>').text("В поддержку"),
        locksadd = $('<button class="btn btn-mini" data-original-title="В канал блокировок"/>').text("По блокировкам"),
        adminadd = $('<button class="btn btn-mini" data-original-title="Админам"/>').text("Админам"),
        addhelp = $('<button class="btn btn-warning btn-mini help-btn"/>').html('<i class="icon-white icon-asterisk"></i>'),
        chatlist = $('<div class="chat-list scrollable"/>');
    b.click(function() {
        if ($(this).hasClass("hasunread")) {
            zefs.myform.chatread();
            if (!!window.chatbtnanimation) {
                clearInterval(window.chatbtnanimation);
                b.css("opacity", "1");
            }
            $(this).removeClass("hasunread");
            var html = $(this).html();
            $(this).empty();
            $(this).html(html);
            html = null;
        }
    });
    chatinput.keydown(function(e) {
        if (e.keyCode == 13 && e.ctrlKey) chatform.trigger("submit");
    });
    lentaadd.tooltip({placement: 'bottom'});
    formkuratoradd.tooltip({placement: 'bottom'});
    objkuratoradd.tooltip({placement: 'bottom'});
    supportadd.tooltip({placement: 'bottom'});
    var chatbuttons = $('<div class="chat-buttons"/>')
        .append(lentaadd, objkuratoradd, formkuratoradd, supportadd, locksadd, refresh, progress, addhelp);
    chatform.append($('<div class="chat-input"/>').append(chatinput), chatbuttons);
    $(zeta).on(zeta.handlers.on_getuserinfo, function() {
        if (zeta.user.getIsAdmin()) {
            chatbuttons.append(adminadd);
        }
    });
    var chatadd = function(type) {
        if (chatinput.val() != "") {
            zefs.myform.chatadd(chatinput.val(), type);
            refresh.hide(); progress.show();
            chatinput.val("");
        }
    };
    lentaadd.click(function() { chatadd("default") });
    formkuratoradd.click(function() { chatadd("formcurrator") });
    objkuratoradd.click(function() { chatadd("objcurrator") });
    supportadd.click(function() { chatadd("support") });
    locksadd.click(function() { chatadd("locks") });
    adminadd.click(function() { chatadd("admin") });
    refresh.click(function() {
        zefs.myform.chatlist();
        refresh.hide();
        progress.show();
    });
    chatlist.append(
        $('<div class="userchat chat-list-header"/>').click(function() {
            $(chatlist.find('.userchat.chat-list-header')).toggleClass("collapsed");
        }).append("Лента сообщений текущей формы"),
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
        window.setTimeout(function(){
            zefs.myform.chatupdateds();
        },60000);
        var strong = $(b.find('strong'));
        if (strong.length != 0) {
            strong.empty();
        } else {
            strong = $('<strong/>');
            b.append(strong);
        }
        if (parseInt(count) > 0) {
            b.addClass("hasunread");
            strong.text(count);
            zefs.myform.chatlist();
            if (!window.chatbtnanimation) {
                window.chatbtnanimation = setInterval(function() { chatbtnanimate() }, 300);
            }
        } else {
            b.removeClass("hasunread");
            strong.empty();
        }
    });

    var chatbtnanimate = function() {
        var opacity = b.css("opacity") || "1";
        opacity == "1" ? opacity = "0.2" : opacity = "1";
        b.animate({ opacity: opacity }, { duration: 150});
    };
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
                var ch = $('<span class="label" style="margin-left: 4px;"/>').text(message.ReadableType);
                if (message.Type != "admin") {
                    ch.addClass("label-info");
                } else {
                    ch.addClass("label-important");
                }
                body.append(tr.append(
                    $('<div class="userchat chat-list-cell username"/>').append(u.text(message.User), ch),
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
            chatlist.append(header.append("Все ваши сообщения"));
        }
        var filters = $(chatlist.find('.adminchat.chat-list-filter'));
        if (filters.length == 0) {
            filters = $('<div class="adminchat chat-list-filter"/>');
            chatlist.append(filters);
        }
        chatlist.append(body);
        body.empty();
        filters.empty();
        if (cl != null && !$.isEmptyObject(cl)) {
            b.addClass("btn-success");
            b.find("i").addClass("icon-white");
            filters.append($('<button class="btn-link active"/>').text("Все").click(function(e) {
                filters.children().removeClass("active");
                $(e.target).addClass("active");
                $(".adminchat.chat-list-row").show();
            }));
            $.each(cl, function(i,message) {
                var tr = $('<div class="adminchat chat-list-row"/>').addClass(message.Type);
                var u = $('<span class="label label-inverse"/>');
                var arch = $('<i class="icon icon-ok pull-right"/>');
                var f = $(filters.find('button.' + message.Type));
                if (f.length == 0) {
                    filters.append($('<button class="btn-link"/>').text(message.ReadableType).click(function(e) {
                        filters.children().removeClass("active");
                        $(e.target).addClass("active");
                        $(".adminchat.chat-list-row").hide();
                        $(".adminchat.chat-list-row." + message.Type).show();
                    }).addClass(message.Type));
                }
                arch.click(function() {
                    zefs.myform.chatarchive(message.Id);
                });
                var form = $.map(zefs.forms, function(i) { if (i.Code == message.FormCode.replace(/[A|B]\.in/,'')) return i });
                var obj = $.map(zefs.objects, function(o) { if (o.id == message.ObjId) return o });
                var formname = $('<span class="adminchat chat-list-cell formname"/>')
                    .text($(form).get(0).Name + " " + $(obj).get(0).name + " за " + zefs.getperiodbyid(message.Period) + ", " + message.Year + " год");
                formname.hover(function() { $(this).toggleClass("hovering") });
                body.append(tr.append(
                    arch,
                    $('<div class="adminchat chat-list-cell username"/>').append(
                        u.text(message.User),
                        $('<div class="adminchat chat-list-cell date"/>').text(message.Date.format("dd.mm.yyyy HH:MM")),
                        formname
                    ),
                    $('<div class="adminchat chat-list-cell message"/>').text(message.Text)
                ));
                formname.click(function(e) {
                    var m = $(this).parents('.chat-list-row').data();
                    e.preventDefault();
                    zefs.myform.openform({form: m.FormCode, period: m.Period, obj: m.ObjId, year: m.Year}, true);
                });
                // помечаем прочитанным
                if (!!message.Userdata) {
                    if (message.Userdata.archive) tr.addClass("archived");
                }
                if (message.isnew) {
                    tr.addClass("notreaded");
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