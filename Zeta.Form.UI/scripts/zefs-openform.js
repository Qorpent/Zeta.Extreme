/**
 * Виджет формы перехода на новую форму :)
 */
(function() {
    var m = $('<div class="btn-group pull-left"/>');
    var l = $('<ul class="dropdown-menu"/>');
    var b = $('<button class="btn btn-small dropdown-toggle" data-toggle="dropdown" data-original-title="Смена формы (Ctrl+R)"/>')
        .html('<i class="icon-search"></i>');
    m.append(b, l);
    b.tooltip({placement: 'bottom'});
    var input = $('<input class="input-normal" type="text" id="zefsnewform-query"/>').css("width", 300);
    var openInNewWindow = $('<input type="checkbox"/>');
    //openInNewWindow.attr('checked', settings.get("form_in_new_window"));
    openInNewWindow.change(function() {
        //settings.set("form_in_new_window", openInNewWindow.is(":checked"));
    });
    l.append(
        $('<div/>').append("Переход на другую форму", $('<label/>').append('в новом окне', openInNewWindow)),
        $('<li/>').append(input)
    );

    var search_data = [];

    var getParameters = function(val) {
        var result = {};
        $.each(val, function(i,v) {
            result[v.split('=')[0]] = v.split('=')[1];
        });
        return result;
    };

    input.select2({
        multiple: true,
        width: "element",
        data: search_data,
        submit: function(val) { form.open(getParameters(val), openInNewWindow.is(':checked')) },
        formatNoMatches: function() { return "Нет совпадений" },
        formatSearching: function() { return "Поиск" },
        formatSelectionCssClass : function(o) { return o.type },
        matcher: function(term, text, o) {
            term = term.toString();
            return o.id.toString().toLowerCase() == term.toLowerCase() ||
                o.id.toString().toLowerCase().indexOf(term.toLowerCase())>=0 ||
                o.text.toString().toLowerCase().indexOf(term.toLowerCase())>=0;
        },
        query: function(query) {
            var t = query.term, filtered = { results: [] }, process;
            if (t === "" || (t.length <= 2 && !parseInt(t))) {
                query.callback(filtered);
                return;
            }
            var selection = $(query.element.context).data().selection ||
                { form: null, obj: null, period: null, year: null};
            process = function(datum, collection) {
                datum = datum[0];
                if (datum.children) {
                    var group = [];
                    $(datum.children).each2(function(i, datum) { process(datum, group); });
                    if (!$.isEmptyObject(group)) collection.push({ text: datum.text, children: group});
                } else {
                    if (!!selection[datum.type]) return;
                    if (query.matcher(t, datum.text, datum)) {
                        collection.push(datum);
                    }
                }
            };
            $(search_data).each2(function(i, datum) { process(datum, filtered.results); });
            query.callback(filtered);
        }
    });

    zefs.api.metadata.getperiods.onSuccess(function(e, result) {
        var data = { text: "Периоды", children: []};
        $.each(result, function(i, g) {
            if (!g.periods) return;
            if (g.type == "Year") {
                $.each(g.periods, function(i, p) {
                    data.children.push({id: "year=" + p.id || "", text: p.id + " год" || "", type: "year"});
                });
            } else {
                $.each(g.periods, function(i, p) {
                    var item = {id: "period=" + p.id || "", text: p.name.trim() + " (" + g.readableType + ", " + p.id + ")" || "", type: "period"};
                    if (p.readableType) {
                        item.text += " (" + p.readableType + ")";
                    }
                    data.children.push(item);
                });
            }
        });
        search_data.push(data);
    });

    zefs.api.metadata.getforms.onSuccess(function(e, result) {
        var data = { text: "Формы", children: []};
        $.each(result, function(i, f) {
            if (!f.Code) return;
            data.children.push({id: "form=" + f.Code || "", text: f.Name + " (" + f.Code + ")" || "", type: "form"});
        });
        search_data.push(data);
    });

    zefs.api.metadata.getobjects.onSuccess(function(e, result) {
        var data = {text: "Предприятия", children: []};
        $.each(result.objs, function(i, o) {
            data.children.push({id: "obj=" + o.id || "", text: "(" + o.id + ") " + o.name || "", type: "obj"});
        });
        search_data.push(data);
    });

    input.on("change", function() {
        var val = input.select2("val");
        input.data("selection", getParameters(val));
    });


    var zefsnewform = new root.Widget("zefsnewform", root.console.layout.position.layoutHeader, "left", { authonly: false, priority: 0 });


/*    var zefsnewform = new widget.W({
        authonly: true,
        adminonly: true,
        name: "zefsnewform",
        append: "toheader",
        float: "left",
        routes: "form"
    });
    zefsnewform.el = m;
    widgets.push(zefsnewform);
*/

    zefsnewform.body = $('<div/>').append(m);
    root.console.RegisterWidget(zefsnewform);

    $(document).on('click.dropdown.data-api', '.zefsnewform', function (e) {
        e.stopPropagation();
    });
    $(document).keydown(function(e) {
        if (e.keyCode == 82 && e.ctrlKey) {
            e.preventDefault();
            e.stopPropagation();
            b.trigger("click");
            if (!m.hasClass("open")) {
                input.select2("close");
            } else {
                input.select2("open");
            }
        }
    });
})();