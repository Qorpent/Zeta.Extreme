window._ = window._ || {};
(function() {
    var WikiEdit = function() {};

    $.extend(WikiEdit.prototype, {
        editor: null,
        list: null,
        wikilist: null,
        wikisource: null,
        preview: null,
        previewhtml: null,
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
                    this.previewhtml.html(qwiki.toHTML(this.text.val()));
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
                    this.previewhtml.html(qwiki.toHTML($(e.target).val()));
                }, this)
            }];
            var editor = _.render.compile("zdev_wikiedit", this.wikisource, events);
            this.preview = editor.find('.wiki-preview');
            this.source = editor.find('.wiki-source');
            this.text = editor.find("#wikiEditText");
            this.text.attr("rows", this.wikisource.Text.split(/\r*\n/).length);
            this.code = editor.find("#wikiEditCode");
            this.title = editor.find("#wikiEditTitle");
            this.previewhtml = editor.find("#wikiEditPreview");
            this.editor = editor;
            editor.fluidlayout();
            _.layout.update("body", editor);
            if (!!this.wikisource) {
                this.previewhtml.html(qwiki.toHTML(this.wikisource.Text));
            }
            _.layout.body().on("mouseup", "*", $.proxy(function(e) {
                e.stopPropagation();
                this.text.css('width', '100%');
            }, this));
            $(window).resize($.proxy(function() {
                var h = $(window).height() - _.layout.header().height();
                this.previewhtml.height(h - 100);
                editor.find("#wikiEditSource").height(h - 100);
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
            }, {
                event: "change",
                selector: ".wiki-attach-file",
                handler: $.proxy(function(e) {
                    this.attachname.val(e.target.files[0].name);
                    this.attachcode.focus();
                }, this)
            }, {
                event: "click",
                selector: ".wiki-attach-select",
                handler: $.proxy(function() {
                    this.attachfile.trigger("click");
                }, this)
            }, {
                event: "click",
                selector: ".wiki-attach-submit",
                handler: $.proxy(function(e) {
                    e.preventDefault();
                    if (this.attachcode.val() != "") {
                        var fd = new FormData();
                        fd.append("code", this.attachcode.val());
                        fd.append("title", this.attachname.val());
                        fd.append("datafile", this.attachfile.get(0).files[0]);
                        this.attach(fd);
                    } else {
                        this.attachcode.focus();
                    }
                }, this)
            }];
            
            var list = _.render.compile("zdev_wikilist", this.wikilist, events);
            this.list = list;
            this.attachfile = list.find(".wiki-attach-file");
            this.attachname = list.find(".wiki-attach-name");
            this.attachcode = list.find(".wiki-attach-code");
            this.attachselect = list.find(".wiki-attach-select");
            this.attachsubmit = list.find(".wiki-attach-submit");
            _.layout.update("left", list);
        },

        attach: function(formdata) {
            var wikiattach = _.api.wiki.savefile.safeClone();
            wikiattach.onSuccess(function(e, result) {
                $('<p/>').text("Файл прикреплен").miamodal({
                    resizable: false,
                    closebutton: false,
                    autoclose: 2000,
                    width: 300
                });
            });
            wikiattach.execute(formdata);
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