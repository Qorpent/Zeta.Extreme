window._ = window._ || {};
(function() {
    var WikiEdit = function() {};

    $.extend(WikiEdit.prototype, {
        editor: null,
        list: null,
        wikilist: null,
        wikisource: null,
        preview: null,
        source: null,
        text: null,
        code: null,
        title: null,
        editorinit: function(wikisource) {
            this.wikisource = wikisource;
            var events = [{
                event: "click",
                selector: "#wikiPreviewBtn",
                handler: $.proxy(function(e) { 
                    this.preview.html(qwiki.toHTML(this.text.val()));
                }, this)
            }, {
                event: "click",
                selector: "#wikiSaveBtn",
                handler: $.proxy(function(e) { 
                    e.stopPropagation();
                    this.save();
                }, this)
            }, {
                event: "keyup",
                selector: "#wikiEditText.autopreview",
                handler: $.proxy(function(e) {
                    this.preview.html(qwiki.toHTML($(e.target).val()));
                }, this)
            }];
            var editor = _.render.compile("zdev_wikiedit", this.wikisource, events);
            this.preview = editor.find('#wikiEditPreview');
            this.source = editor.find('.wiki-source');
            this.text = editor.find("#wikiEditText");
            this.code = editor.find("#wikiEditCode");
            this.title = editor.find("#wikiEditTitle");
            if (this.editor) {
                this.editor.append(editor);
            } else {
                this.editor = editor;
            }
            editor.fluidlayout();
            _.layout.update("body", editor);
            if (!!this.wikisource) {
                this.preview.html(qwiki.toHTML(this.wikisource.Text));
            }
            _.layout.body().on("mouseup", "*", $.proxy(function(e) {
                e.stopPropagation();
                this.text.css('width', '100%');
            }, this));
            $(window).resize($.proxy(function() {
                var h = $(window).height() - _.layout.header().height();
                this.preview.height(h - 50);
                this.source.height(h - 50);
            }, this));
            $(window).trigger("resize");
        },

        listinit: function(wikilist) {
            this.wikilist = wikilist;
            var events = [{
                event: "click",
                selector: ".wiki-article-link",
                handler: function(e) { 
                    e.preventDefault();
                    e.stopPropagation(); 
                    var id = $(e.target).attr("wikicode");
                    if (e.target.tagName == "I") {
                        id = $(e.target).parent().attr("wikicode");
                    }
                    _.zdev.wikiedit.open(id);
                }
            }, {
                event: "click",
                selector: ".wiki-create",
                handler: function(e) { 
                    _.zdev.wikiedit.open();
                }
            }, {
                event: "click",
                selector: ".wiki-delete",
                handler: function(e) {
                    e.stopPropagation();
                    var code = $(e.target).parent().attr("wikicode") || "";
                    if (code != "") {
                        $('<p/>').text("Статья wiki с кодом " + code + " будет удалена").miamodal({
                            title: "Удаление статьи",
                            width: 400,
                            customButton: {
                                class : "btn-danger",
                                text: "Удалить",
                                click : function() {
                                    _.api.wiki.delete.safeClone()
                                        .onSuccess(function() { _.api.wiki.find.execute() })
                                        .execute({code: code});         
                                }
                            }
                        });
                    }
                }
            }];
            
            var list = _.render.compile("zdev_wikilist", this.wikilist, events);
            this.list = list;
            _.layout.update("left", list);
        },

        save: function() {
            var wikisave = _.api.wiki.save.safeClone();
            var message = "";
            message = this.code.val() == "" 
                ? "Ошибка при сохранении статьи. Не указан код." 
                : "Статья сохранена успешно";
            wikisave.onSuccess(function() {
                $('<p/>').text(message).miamodal({
                    resizable: false,
                    closebutton: false,
                    autoclose: 2000,
                    width: 300
                });
            });
            if (this.code.val() != "") {
                wikisave.execute({
                    code: this.code.val(),
                    title: this.title.val(),
                    text: this.text.val()
                });
            }
        },

        open: function(id) {
            if (!!id) {
                var wikiget = _.api.wiki.get.safeClone();
                wikiget.onSuccess($.proxy(function(i, result) {
                    this.editorinit(result.articles.length > 0 ? result.articles[0] : null);
                }, this));
                wikiget.execute({code: id});   
            } else {
                this.editorinit(null);
            }
        },

        getlist: function() {
            var wikifind = _.api.wiki.find.safeClone()
            wikifind.onSuccess($.proxy(function(e, result) {
                this.listinit(result);
            }, this));
            wikifind.execute({search: "/"});
        }
    });

    _.zdev = _.zdev || {};
    $.extend(_.zdev, {
        wikiedit: new WikiEdit()
    });
    
    var wikiedit_widget = _.widget.register({
        name : "wikiedit",
        position : "header:left",
        icon : "icon-edit",
        title: "Редактор вики",
        type: "button",
        onclick: function() {
			_.zdev.wikiedit.getlist();
        }
    });
})();