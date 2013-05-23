/**
 * Виджет информации о форме
 */
!function($) {
    var root = window.zefs = window.zefs || {};
    var zefsforminfo = new zeta.Widget("zefsforminfo", zeta.console.layout.position.layoutPageHeader, "right", { authonly: true, priority: 80 });
    var content = $('<div/>'),
        formdetails = $('<div class="zefsformdetails"/>'),
        formdependence  = $('<div class="zefsformdependence"/>'),
        formdocumentation = $('<div class="zefsformdocumentation"/>').append(
            $('<h5 class="btn-link non-printable"/>').css("margin-bottom", 0)
                .html('Документация<span class="caret"/>')
        ),
        docprint = $('<button class="btn btn-mini non-printable">Печать</button>');
    formdocumentation.children().append(docprint);
    docprint.click(function() {
        $('#zefsFormDoc').printelement();
    });
    var t = $('<span class="label-tree"/>');
    content.append(formdetails, formdependence, formdocumentation);

    var GetFormType = function(type) {
        switch (type) {
            case "mix" : return "Комбинированная";
            case "extension" : return "Расширение";
            case "analytic" : return "Аналитическая";
            case "primary" : return "Первичная";
            default : return "";
        }
    };

    var RenderDetails = function() {
        var d = zefs.myform.details;
        var t = $('<table class="table-bordered table"/>').append(
            $('<tbody/>').append(
                $('<tr/>').append($('<td/>').text("Код темы"), $('<td/>').text(d.code)),
                $('<tr/>').append($('<td/>').text("Тип"), $('<td/>').text(GetFormType(d.type)))
            )
        );
        var detailstitle = $('<h5 class="btn-link"/>').html('Техническая информация<span class="caret"/>');
        formdetails.append(detailstitle);
        formdetails.append(t.hide());
        var dependancetitle = $('<h5 class="btn-link"/>').html('Зависимые формы<span class="caret"/>');
        formdependence.append(dependancetitle);
        $.each(d.requiredFor, function(i1, r1) {
            if (r1.code == "controlpointall") return;
            var c1 = $('<div/>');
            c1.append($('<div class="level1"/>').text(r1.name));
            if (!$.isEmptyObject(r1.requiredFor)) c1.addClass("haschildrens collapsed");
            $.each(r1.requiredFor, function(i2, r2) {
                if (r2.code == "controlpointall") return;
                var c2 = $('<div/>');
                c2.append($('<div class="level2"/>').text(r2.name));
                if (!$.isEmptyObject(r2.requiredFor)) c2.addClass("haschildrens collapsed");
                $.each(r2.requiredFor, function(i3, r3) {
                    if (r3.code == "controlpointall") return;
                    var c3 = $('<div/>');
                    c3.append($('<div class="level3"/>').text(r3.name));
                    if (!$.isEmptyObject(r3.requiredFor)) c3.addClass("haschildrens collapsed");
                    $.each(r3.requiredFor, function(i4, r4) {
                        if (r4.code == "controlpointall") return;
                        var c4 = $('<div/>');
                        c4.append($('<div class="level4"/>').text(r4.name));
                        if (!$.isEmptyObject(r4.requiredFor)) c4.addClass("haschildrens collapsed");
                        $.each(r4.requiredFor, function(i, r5) {
                            if (r5.code == "controlpointall") return;
                            var c5 = $('<div/>');
                            c5.append($('<div class="level5"/>').text(r5.name));
//                            if (!$.isEmptyObject(r5.requiredFor)) c5.addClass("haschildrens");
//                            $.each(r4.recuiredFor, function(i, r5) {
//
//                            });
                            c4.append(c5.hide());
                        });
                        c3.append(c4.hide());
                    });
                    c2.append(c3.hide());
                });
                c1.append(c2.hide());
            });
            formdependence.append(c1);
        });
        $.each(formdependence.find(".haschildrens"), function(i, e) {
            var t = $(e);
            if (t.children().length < 2) return;
            var c = $('<span/>');
            c.click(function() {
                if (t.hasClass("collapsed")) {
                    t.removeClass("collapsed");
                    $(t.children().get(0)).nextAll().show();
                } else {
                    t.addClass("collapsed");
                    $(t.children().get(0)).nextAll().hide();
                }
            });
            $(t.children().get(0)).prepend(c);
        });
    };

    var RenderDocumentation = function() {
        var codes = $.map(zefs.myform.documentation, function(w) { return w.Code }).join(',');
        if (codes.length > 0) {
            $.ajax({
                url : "wiki/get.json.qweb",
                type : "POST",
                dataType : "json",
                data : {
                    code : codes,
                    usage : "formhelp"
                }
            }).success(function(result) {
                var hint = "";
                if (!$.isEmptyObject(result)) {
                    hint = "(нажмите по ссылке для получения справки по отдельной строке)";
                }
                var doc = $('<div id="zefsFormDoc"/>');
                doc.append($('<h2 style="display: none;"/>').text("Справка по форме: " + zefs.myform.currentSession.FormInfo.Name));
                doc.append($('<p class="hint non-printable"/>').text(hint));
                $.each(result, function(i, w) {
                    var title = $('<h4 class="btn-link"/>').text(w.Title);
                    doc.append(title, $('<p/>').html(wiky.process(w.Text)).hide());
                });
                formdocumentation.append(doc);
            });
        } else {
            formdocumentation.append($('<p class="hint non-printable"/>').text("Для данной формы пока нет документации"));
        }
    };

    $(root).on(root.handlers.on_detailsload, function() {
        RenderDetails();
    });

    $(root).on(root.handlers.on_documentationload, function() {
        RenderDocumentation();
    });

    t.click(function() {
        $(zeta).trigger(window.zeta.handlers.on_modal, {
            title: "Справка по форме",
            content: content,
            width: 700, height: 350
        });
        formdocumentation.delegate("h5", "click", function(e) {
            $('#zefsFormDoc').toggle();
        });
        formdetails.delegate("h5", "click", function(e) {
            $(e.target).nextAll().toggle();
        });
        formdependence.delegate("h5", "click", function(e) {
            $(e.target).nextAll().toggle();
        });
    });

    zefsforminfo.body = $('<div/>').append(t);
    zeta.console.RegisterWidget(zefsforminfo);
}(window.jQuery);