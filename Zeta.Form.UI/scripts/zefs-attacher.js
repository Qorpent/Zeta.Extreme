/**
 * Виджет менеджера прикрепленных файлов
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var attacher = new root.Widget("attacher", root.console.layout.position.layoutHeader, "left", { authonly: true, priority: 98, ready: function() {
        attacher.body.find('.btn-group').floatmenu();
    } });
    var b = $('<button class="btn btn-small dropdown-toggle" data-toggle="dropdown" data-original-title="Прикрепленные файлы"/>')
        .html('<i class="icon-file"></i>');
    var attachlist = $('<table class="table table-striped"/>');
    attachlist.append(
        $('<colgroup/>').append(
            $('<col/>').css("width",22),
            $('<col/>').css("width",70),
            $('<col/>').css("width",""),
            $('<col/>').css("width","25%"),
            $('<col/>').css("width",22)
        ),
        $('<thead/>').append($('<tr/>').append($('<th colspan="5"/>').text("Прикрепленные файлы"))),
        $('<tbody/>').append($('<tr/>').append($('<td colspan="5"/>').text("Пока прикрепленных фалов нет")))
    );
    var filelist = $('<div class="dropdown-menu"/>').css({"padding": 5, "width": 450});
    var ConfigureAttachList = function() {
        var f = window.zefs.myform.attachment;
        var body = $(attachlist.find('tbody'));
        if (f != null && !$.isEmptyObject(f)) {
            body.empty();
            b.addClass("btn-info");
            b.find("i").addClass("icon-white");
            $.each(f, function(i,file) {
                var tr = $('<tr/>');
                var u = $('<span class="label label-inverse"/>');
                body.append(tr.append(
                    $('<td class="type"/>').addClass(file.Extension.substring(1)),
                    $('<td/>').text(file.Date.format("dd.mm.yyyy")),
                    $('<td class="filename"/>').html('<a href="' + window.zefs.api.file.download.getUrl(file.Uid) + '" target="_blank">' + file.Name + '</a>'),
                    $('<td class="username"/>').append(u.text(file.User))
                ));
                if (window.zeta.user != null) {
                    if (window.zeta.user.getIsAdmin()) {
                        tr.append($('<td class="delete"/>').html($('<span class="icon icon-remove"/>').click(
                            function(e) {
                                $(window.zeta).trigger(window.zeta.handlers.on_modal, {
                                    title: "Удаление файла",
                                    content: $('<p/>').html("Файл <strong>" + file.Name + "</strong> будет удален. Продолжить?"),
                                    customButton: {
                                        text: "Удалить",
                                        click: function() { window.zefs.myform.deletefile(file.Uid) }
                                    }
                                });
                            }
                        )));
                    }
                }
                u.zetauser();
                tr = u = null;
            });
        } else {
            body.html($('<tr/>').append($('<td colspan="10"/>').text("Пока прикрепленных фалов нет")));
            b.removeClass("btn-info");
            b.find("i").removeClass("icon-white");
        }
        body = f = null;
    };

    var progress = $('<div class="progress progress-striped active"/>').append($('<div class="bar" style="width:1%;"/>'));
    var uploadform = $('<form method="post"/>');
    var uploadbtn = $('<button type="submit" class="btn btn-mini btn-primary"/>').text("Прикрепить");
    var selectbtn = $('<button type="button" class="btn btn-mini"/>').text("Выбрать файл");
    // Поле с файлом
    var file = $('<input type="file" name="datafile"/>').hide();
    var type = $('<select name="type" class="input-mini"/>').append(
        $('<option/>').text("default"),
        $('<option/>').text("balans"),
        $('<option/>').text("stfr"),
        $('<option/>').text("prib"),
        $('<option/>').text("freeact")
    );
    var filename = $('<input type="text" name="filename" placeholder="Изменить имя..." class="input-small"/>');
    var uid = $('<input type="hidden" name="uid"/>');
    file.change(function() {
        filename.attr("placeholder", this.files[0].name);
    });
    selectbtn.click(function() { file.trigger("click") });
    uploadform.append(
        $('<table/>').append(
            $('<colgroup/>').append(
                $('<col/>').css("width",92),
                $('<col/>').css("width", ""),
                $('<col/>').css("width",85),
                $('<col/>').css("width",80)
            ),
            $('<tr/>').append(
                $('<td/>').append(selectbtn),
                $('<td/>').append(file, filename),
                $('<td/>').append(type),
                $('<td/>').append(uid,uploadbtn)
            )
        )
    );
    uploadform.submit(function(e) {
        e.preventDefault();
        if (file.get(0).files.length == 0) return;
        window.zefs.myform.attachfile($(e.target));
    });
    var floating = $('<div class="floatmode"/>').click(function() {
        $(this).toggleClass("active");
        filelist.toggleClass("floating");
        b.toggleClass("active");
        if (filelist.hasClass("ui-draggable")) {
            filelist.draggable('destroy');
            filelist.css({"top": "", "left": ""});
            $(document).trigger('click.dropdown.data-api');
        } else {
            filelist.draggable();
        }
    });
    b.tooltip({placement: 'bottom'});
    filelist.append(floating, progress.hide(), uploadform, attachlist);
    $(document).on('click.dropdown.data-api', '.attacher div', function (e) {
        // e.preventDefault();
        e.stopPropagation();
    });
    $(window.zefs).on(window.zefs.handlers.on_fileloadstart, function() {
        uploadform.hide(); progress.show();
    });
    $(window.zefs).on(window.zefs.handlers.on_fileloadfinish, function() {
        progress.hide(); uploadform.show();
        progress.find('.bar').css("width", 0);
    });
    $(window.zefs).on(window.zefs.handlers.on_fileloadprocess, function(e, p) {
        progress.find('.bar').css("width", (p.loaded / p.total * 100) + "%");
    });
    $(window.zefs).on(window.zefs.handlers.on_fileloaderror, function(e, error) {
        progress.hide(); uploadform.show();
        progress.find('.bar').css("width", 0);
        $(window.zeta).trigger(window.zeta.handlers.on_modal, {
            title: "Ошибка загрузки файла",
            text: "Во время загрузки файла произошла ошибка: " + error.message
        });
    });

    $(window.zefs).on(window.zefs.handlers.on_attachmentload, function() {
        ConfigureAttachList();
    });
    $(window.zefs).on(window.zeta.handlers.on_getuserinfo, function() {
        ConfigureAttachList();
    });
    attacher.body = $('<div/>').append($('<div class="btn-group"/>').append(b,filelist));
    root.console.RegisterWidget(attacher);
}(window.jQuery);