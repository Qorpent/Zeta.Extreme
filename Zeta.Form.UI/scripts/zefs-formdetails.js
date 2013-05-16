window.formdetails = window.formdetails || {};
var root = window.formdetails || $.extend(window.formdetails, {
    formcode : ""
});

(function() {
    root.formcode = location.hash.substr(1);
    if (root.formcode == "") return;
    $.ajax({
        url : "zefs/formdetails.json.qweb",
        type : "POST",
        dataType : "json",
        data : { form : root.formcode }
    }).success(function(result) {
        root.dependings = result;
        Render();
    });

    var Render = function() {
        var d = root.dependings;
        var b = $('body');
        b.append($('<h1/>').text(d.name));
        $.each(d.requiredFor, function(i1, r1) {
            var c1 = $('<div/>');
            c1.append($('<div class="level1"/>').text(r1.name));
            if (!$.isEmptyObject(r1.requiredFor)) c1.addClass("haschildrens hidden");
            $.each(r1.requiredFor, function(i2, r2) {
                var c2 = $('<div/>');
                c2.append($('<div class="level2"/>').text(r2.name));
                if (!$.isEmptyObject(r2.requiredFor)) c2.addClass("haschildrens hidden");
                $.each(r2.requiredFor, function(i3, r3) {
                    var c3 = $('<div/>');
                    c3.append($('<div class="level3"/>').text(r3.name));
                    if (!$.isEmptyObject(r3.requiredFor)) c3.addClass("haschildrens hidden");
                    $.each(r3.requiredFor, function(i4, r4) {
                        var c4 = $('<div/>');
                        c4.append($('<div class="level4"/>').text(r4.name));
                        if (!$.isEmptyObject(r4.requiredFor)) c4.addClass("haschildrens hidden");
                        $.each(r4.requiredFor, function(i, r5) {
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
            b.append(c1);
        });
        $(".haschildrens").each(function(i, e) {
            var t = $(e);
            $(t.children().get(0)).prepend($('<span/>'));
        });
        $("span").click(function(e) {
            var t = $(e.target).parent();
            if (t.parent().hasClass("hidden")) {
                t.parent().removeClass("hidden");
                t.nextAll().show();
            } else {
                t.parent().addClass("hidden");
                t.nextAll().hide();
            }
        });
    };
})();