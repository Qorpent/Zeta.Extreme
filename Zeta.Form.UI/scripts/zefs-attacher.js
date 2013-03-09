/**
 * Виджет менеджера прикрепленных файлов
 */
!function($) {
    var attacher = new root.security.Widget("attacher", root.console.layout.position.layoutHeader, "left", { authonly: true, priority: 30 });
    var b = $('<button class="btn btn-small dropdown-toggle" data-toggle="dropdown"/>')
        .html('<i class="icon-file"></i><span class="caret"></span>');
    b.tooltip({title: "Прикрепленные файлы", placement: 'bottom'});
    var attachlist = $('<table class="table table-striped"/>');
    attachlist.append(
        $('<colgroup/>').html('<col style="width: 30px;"><col style="width: 80px;"><col style="width: 70px;"><col style="width: 200px;"><col style="width: 20px;">'),
        $('<thead/>').append($('<tr/>').append($('<th colspan="5"/>').text("Прикрепленные файлы"))),
        $('<tbody/>').append($('<tr/>').append($('<td colspan="5"/>').text("Пока прикрепленных фалов нет")))
    );
    var filelist = $('<ul class="dropdown-menu"/>').css("padding", 5);
    var ConfigureAttachList = function() {
        var f = window.zefs.myform.attachment;
        var body = $(attachlist.find('tbody'));
        if (f != null && f.length > 0) {
            body.empty();
            b.addClass("btn-info");
            b.find("i").addClass("icon-white");
            $.each(f, function(i,file) {
                var tr = $('<tr/>');
                body.append(tr.append(
                    $('<td class="type"/>').addClass(file.getExtension().substring(1)),
                    $('<td/>').text(file.getDate().format("dd.mm.yyyy")),
                    $('<td/>').append($('<span class="label label-inverse"/>').text(file.getUser())),
                    $('<td class="filename"/>').html('<a href="' + window.zefs.myform.downloadfile(file.getUid()) + '" target="_blank">' + file.getName() + '</a>')
                ));
                if (window.zeta.security.user != null) {
                    if (window.zeta.security.user.getIsAdmin()) {
                        tr.append($('<td class="delete"/>').html($('<span class="icon icon-remove"/>').click(function(e) {
                                $(window.zeta).trigger(window.zeta.handlers.on_modal, {
                                    title: "Удаление файла",
                                    content: $('<p/>').html("Файл <strong>" + file.getName() + "</strong> будет удален. Продолжить?"),
                                    ok: function() { window.zefs.myform.deletefile(file.getUid()) }
                                });
                            }
                        )));
                    }
                }
                tr = null;
            });
        } else {
            body.html($('<tr/>').append($('<td colspan="10"/>').text("Пока прикрепленных фалов нет")));
            b.removeClass("btn-info");
            b.find("i").removeClass("icon-white");
        }
        body = f = null;
    };

    var progress = $('<div class="progress progress-striped active"/>').append($('<div class="bar" style="width:1%;"/>'));
    var uploadform = $('<form method="post"/>').submit(function(e) {
        e.preventDefault();
        window.zefs.myform.attachfile($(e.target));
    });
    var uploadbtn = $('<button type="submit" class="btn btn-mini btn-primary"/>').text("Прикрепить");
    var selectbtn = $('<button type="button" class="btn btn-mini"/>').text("Выбрать файл");
    // Поле с файлом
    var file = $('<input type="file" name="datafile"/>').hide();
    var type = $('<select name="type" class="input-mini"/>').append(
        $('<option/>').text("default"),
        $('<option/>').text("balans"),
        $('<option/>').text("prib")
    );
    var filename = $('<input type="text" name="filename" placeholder="Изменить имя..." class="input-small"/>');
    var uid = $('<input type="hidden" name="uid"/>');
    file.change(function() { filename.attr("placeholder", this.files[0].name) });
    selectbtn.click(function() { file.trigger("click") });
    uploadform.append(
        selectbtn,
        file, filename, type, uid,
        uploadbtn
    );
    filelist.append(progress.hide(), uploadform, $('<div/>').append(attachlist));
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