(function($) {
    var zefs = window.zefs || {};
    var api = zefs.api || {};
    var zeta = window.zeta || {};
    if ($.isEmptyObject(zefs)) return;
    if ($.isEmptyObject(api)) return;

    $.extend(zefs.handlers, {
        on_wikifileloadstart : "wikifileloadstart",
        on_wikifileloadfinish : "wikifileloadfinish",
        on_wikifileloadprocess : "wikifileloadprocess",
        on_wikifileloaderror : "wikifileloaderror"
    });

    var GetRowWiki = function(rowcode) {
        if ($("#wiki_dialog___row_" + rowcode + "_default").length > 0) return;
        var wikicode = '/row/' + rowcode + '/default';
        api.wiki.getsync.execute({code: wikicode});
        var row = $.map(zefs.myform.currentSession.structure.rows, function(r) { if (r.code == rowcode && !!r.comment) return r; });
        if (row.length > 0) {
            var commentarticle = $('<div class="commentarticle"/>').attr("id", "commentarticle_" + rowcode);
            commentarticle.insertAfter($("#wikiarticle__row_" + rowcode + "_default"));
            commentarticle.append(
                $('<div class="wikititle"/>').text("Комментарий к строке"),
                $('<div class="wikitext"/>').html(row["0"].comment.replace(/\s*\r\n/g,'</br>'))
            );
        }
        var colset = $.map($('th.primary'), function(th) { return $(th).attr("title") }).join(";");
        api.metadata.getformuladependency.execute({code: rowcode, colset: colset, obj: zefs.myform.currentSession.ObjInfo.Id});
    };

    var SaveWiki = function(code, text, params) {
        // с параметрами уточнить как слать
        if (zeta.user.getIsDocWriter()) {
            api.wiki.save.execute({code: code, text: text});
        }
    };

    var WikiAttachFile = function(form) {
        var fd = new FormData();
        fd.append("code", form.find('input[name="code"]').val());
        fd.append("title", form.find('input[name="title"]').val());
        fd.append("datafile", form.find('input[name="datafile"]').get(0).files[0]);
//      $(root).trigger(root.handlers.on_fileloadstart);
        api.wiki.savefile.execute(fd);
        $(zefs).trigger(zefs.handlers.on_wikifileloadstart);
    };

    api.wiki.getsync.onSuccess(function(e, result) {
        if (!$.isEmptyObject(result)) {
            var content = $('<div/>');
            var id = "";
            $.each(result, function(i, w) {
                id += w.Code;
                var wikiarticle = $('<div class="wikiarticle"/>');
                wikiarticle.attr("id", "wikiarticle_" + w.Code.replace(/\//g, "_"));
                var wikititle = $('<div class="wikititle"/>').text(w.Title || w.Code);
                wikititle.attr("title", w.Code);
                wikititle.data("history", w.Title || w.Code);
                var wikitext = $('<div class="wikitext"/>');
                if (w.Text != "") {
                    wikitext.html(qwiki.toHTML(w.Text));
                    wikitext.data("history", qwiki.toHTML(w.Text));
                }
                var wikiinfo = $('<div class="wikiinfo"/>').text("Последняя правка: ");
                if (!!w.Date) {
                    wikiinfo.text(wikiinfo.text() + w.Date.format("dd.mm.yyyy HH:MM:ss"));
                }
                if (!!w.Editor) {
                    var user = $('<span class="label label-info"/>').text(w.Editor);
                    wikiinfo.append(user);
                    user.zetauser();
                }
                if (zeta.user.getIsDocWriter()) {
                    var wikiedit = $('<textarea class="wikiedit"/>').val(w.Text);
                    var wikititleedit = $('<input type="text" class="wikititleedit"/>').val(w.Title || w.Code);
                    wikiedit.hide(); wikititleedit.hide();
                    var wikicontrols = $('<div class="wikicontrols"/>');
                    var wikieditbtn = $('<button class="btn btn-mini"/>').text("Править");
                    var wikisavebtn = $('<button class="btn btn-mini btn-success"/>').html('<i class="icon-white icon-ok"/>').hide();
                    var wikicancelbtn = $('<button class="btn btn-mini btn-danger"/>').html('<i class="icon-white icon-remove"/>').hide();
                    var wikiprintbtn = $('<button class="btn btn-mini"/>').html('<i class="icon-print"/>');
                    var progress = $('<div class="progress progress-striped active"/>').append($('<div class="bar" style="width:1%;"/>'));
                    var uploadform = $('<form method="post"/>').css("display", "inline-block");
                    var uploadbtn = $('<button type="submit" class="btn btn-mini btn-primary"/>').text("Прикрепить");
                    var selectbtn = $('<button type="button" class="btn btn-mini"/>').text("Выбрать файл").css("margin-right", 3);
                    // Поле с файлом
                    var file = $('<input type="file" name="datafile"/>').hide();
                    var code = $('<input name="code" type="text" placeholder="Код" class="input-small"/>').css({"padding": "0px 6px", "margin-right" : 3});
                    var filename = $('<input type="text" name="title" placeholder="Название" class="input-normal"/>').css("padding", "0px 6px");
                    var uid = $('<input type="hidden" name="uid"/>');
                    var message = $('<div/>').css({
                        position: "absolute",
                        fontSize: "8pt",
                        color: "grey",
                        right: 0
                    })
                    file.change(function() {
                        uploadbtn.text("Прикрепить " + this.files[0].name);
                    });
                    selectbtn.click(function() { file.trigger("click") });
                    uploadform.append(selectbtn,file,code,filename,uploadbtn, message);
                    uploadform.submit(function(e) {
                        e.preventDefault();
                        if (file.get(0).files.length == 0) return;
                        WikiAttachFile($(e.target));
                    });
                    wikicontrols.append(uploadform, progress.hide(), wikiprintbtn, wikieditbtn, wikicancelbtn, wikisavebtn);
                    wikiedit.keyup(function() {
                        wikitext.html(qwiki.toHTML(wikiedit.val()));
                    });
                    wikititleedit.keyup(function() {
                        wikititle.text(wikititleedit.val());
                    });
                    wikieditbtn.click(function() {
                        wikieditbtn.hide(); wikiedit.show(); wikititleedit.show(); wikisavebtn.show(); wikicancelbtn.show();
                    });
                    wikiprintbtn.click(function() {
                        wikitext.printelement();
                    });
                    wikicancelbtn.click(function() {
                        if (!!wikitext.data("history")) {
                            wikitext.text(qwiki.toHTML(wikitext.data("history")));
                        }
                        wikititle.text(wikititle.data("history"));
                        wikiedit.hide(); wikititleedit.hide(); wikisavebtn.hide(); wikicancelbtn.hide(); wikieditbtn.show();
                    });
                    wikisavebtn.click(function() {
                        wikiedit.hide(); wikititleedit.hide(); wikisavebtn.hide(); wikicancelbtn.hide();
                        var save = $.ajax({
                            url: "wiki/save.json.qweb",
                            type: "POST",
                            data: { code: w.Code, text: wikiedit.val(), title: wikititleedit.val() }
                        });
                        save.success(function(r) {
                            wikititle.text(r.Title || r.Code);
                            var v = eval(r.LastWriteTime.substring(2));
                            wikiinfo.text("Сохранено: " + v.format("dd.mm.yyyy HH:MM:ss"));
                            if (!!r.Editor) {
                                var user = $('<span class="label label-info"/>').text(r.Editor);
                                wikiinfo.append(user);
                                user.zetauser();
                            }
                        });
                        wikieditbtn.show();
                    });
                    wikiarticle.append(wikititleedit, wikiedit, wikicontrols);
                    $(zefs).on(zefs.handlers.on_wikifileloadstart, function() {
                        uploadform.hide(); progress.show();
                        message.text("");
                    });
                    $(zefs).on(zefs.handlers.on_wikifileloadfinish, function() {
                        progress.hide(); uploadform.show();
                        message.text("Файл с кодом " + code.val() + " был прикреплен");
                        code.val("");
                        filename.val("");
                        uploadbtn.text("Прикрепить");
                        progress.find('.bar').css("width", 0);
                    });
                    $(zefs).on(zefs.handlers.on_wikifileloadprocess, function(e, p) {
                        progress.find('.bar').css("width", (p.loaded / p.total * 100) + "%");
                    });
                    $(zefs).on(zefs.handlers.on_wikifileloaderror, function(e, error) {
                        progress.hide(); uploadform.show();
                        progress.find('.bar').css("width", 0);
                        $(window.zeta).trigger(window.zeta.handlers.on_modal, {
                            title: "Ошибка загрузки файла",
                            text: "Во время загрузки файла произошла ошибка: " + error.message
                        });
                    });
                }
                wikiarticle.append(wikititle, wikitext, wikiinfo);
                content.append(wikiarticle);
            });
            if ($("#wiki_dialog__" + id.replace(/\//g, "_")).length == 0) {
                $(zeta).trigger(zeta.handlers.on_modal, {
                    title: "База знаний", width: 900,
                    content: $('<div/>').append(content),
                    id: "wiki_dialog__" + id.replace(/\//g, "_"),
                    customButton: {
                        class : "btn-warning",
                        text : "Справка",
                        click : function() { api.wiki.getsync.execute({code: "/wiki/wikimarkup/default"}); }
                    }
                });
            }
        }
    });

    api.wiki.exists.onSuccess(function(e, result) {
        zefs.myform.documentation = result;
        $.each(result, function(i, c) {
            var code = c.Code.match(/row\/([^\/]+)/);
            var ifexist = $.map(zefs.myform.currentSession.structure.rows, function(r, i) { if (r.code == code[1] && !r.hasHelp) return i; });
            if (ifexist.length > 0) {
                zefs.myform.currentSession.structure.rows[ifexist[0]].hasHelp = true;
            }
        });
        $(zefs).trigger(zefs.handlers.on_documentationload);
        var withhelp = $.map(zefs.myform.currentSession.structure.rows, function(r) { if (r.hasHelp) return r; });
        $.each(withhelp, function(i, row) {
            var helpcode = '/row/' + row.code + '/default';
            var wikibtn = $('#wiki_' + helpcode.replace(/\//g, '_'));
            wikibtn.removeClass("notexist");
            wikibtn.click(function() {
                clearTimeout(window.wikitimer);
            });
            wikibtn.mouseenter(function() {
                window.wikitimer = setTimeout(function() {
                    var content = "";
                    if (!!row.comment && row.comment != "") {
                        content = '<p class="popover-comment">' + row.comment.replace(/\s*\r\n/g,'</br>') + '</p>';
                    }
                    var req = $.ajax({url : "wiki/get.json.qweb", data: {code: wikibtn.attr("code")}});
                    req.success(function(result) {
                        if (!!result[0]) {
                            if (!!result[0].Text) {
                                content = qwiki.toHTML(result[0].Text) + content;
                            }
                        }
                        wikibtn.popover({selector: 'body', placement: "right", trigger: "hover", html: true, content: content });
                        wikibtn.popover("show");
                        content = null;
                    });
                }, 500);
            });
            wikibtn.mouseleave(function() {
                clearTimeout(window.wikitimer);
                if (!!wikibtn.data('popover')) {
                    wikibtn.popover('destroy');
                    wikibtn.removeAttr("data-original-title");
                    wikibtn.removeAttr("title");
                }
            });
        });
        if (!zeta.user.getIsDocWriter() && !zeta.user.getIsAdmin()) {
            $('.wikirowhelp.notexist').remove();
        }
        $('.name>.wikirowhelp').show();
    });

    api.metadata.getformuladependency.onSuccess(function(e, result) {
        if (typeof result == "string") return;
        var code = result.code || "";
        var article = $('<div class="detailsarticle"/>').attr("id", "detailsarticle_" + code);
        article.insertAfter($("#wikiarticle__row_" + code + "_default"));
        var title = $('<div class="wikititle"/>').text("Зависимости формулы");
        article.append(title);
        var getReadableType = function(type) {
            switch (type) {
                case "formula" : return "Формула";
                case "ref" : return "Ссылка";
                case "exref" : return "Вн.ссылка";
                default : return type;
            }
        }
        var tbody = $('<tbody/>');
        var header = $('<tr/>').append(
                $('<th/>').text("Код."),
                $('<th/>').text("Вн.код"),
                $('<th/>').text("Форма"),
                $('<th/>').text("Наименование"),
                $('<th/>').text("Зав-ть")
            );
        var table = $('<table class="table table-bordered"/>').append(
            $('<thead/>').append(header), tbody
        );
        $.each($('th.primary'), function(i, th) {
            header.append($('<th/>').text($(th).text()));
        });
        var primary_cols = $('th.primary');
        $.each(result.dependency, function(i1, d1) {
            var tr = $('<tr/>').append(
                $('<td/>').text(d1.outercode),
                $('<td/>').text(d1.code),
                $('<td/>').append(d1.form),
                $('<td/>').text(d1.name),
                $('<td/>').text(getReadableType(d1.type))
            );
            tbody.append(tr);
            if (!!d1.values) {
                $.each(d1.values, function(i, v) {
                    tr.append($('<td/>').text(v))
                });
            }
        });
        article.append(table);
        if (!!result.forms) {
            var formstitle = $('<div class="wikititle"/>').text("Формы, содержащие исходные данные");
            article.append(formstitle);
            $.each(result.forms, function(i, f) {
                if (zefs.myform.currentSession.FormInfo.Code.search(f.code) != -1) return;
                var a = $('<button class="btn-link"/>').text(f.name);
                if (f.allow) {
                    a.click(function() { OpenForm({form: f.code}, true) });
                }
                article.append(a);
            });
            if (formstitle.next().length == 0) formstitle.remove();
        }
    });

    api.wiki.savefile.onSuccess(function() {
        $(zefs).trigger(zefs.handlers.on_wikifileloadfinish);
    });

    api.wiki.savefile.onError(function(e, error) {
        $(zefs).trigger(zefs.handlers.on_wikifileloaderror, JSON.parse(error.responseText));
    });

    api.wiki.savefile.onProgress(function(e, result) {
        $(zefs).trigger(zefs.handlers.on_wikifileloadprocess, result);
    });

    $.extend(zefs.myform, {
        wikiget: GetRowWiki,
        wikisave: SaveWiki
    });
})(jQuery)