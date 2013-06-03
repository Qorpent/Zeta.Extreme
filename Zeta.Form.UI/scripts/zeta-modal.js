/**
 * Виджет окна с сообщениями
 */
!function($) {
    window.zeta.handlers = $.extend(window.zeta.handlers, {
        on_modal : "modal"
    });
    var zetamodal = new root.Widget("zetamodal", root.console.layout.position.layoutBodyMain, null);
    var ShowModal = function(p) {
        p = $.extend({
            title: "",
            content: null,
            customButton: null,
            closebutton: true,
            text: "",
            type: "",
            fade: false,
            width: null,
            height: null,
            backdrop: false,
            name : ""
        },p);
        $.extend({
            class : "btn-primary",
            text : "OK",
            click : function() {}
        }, p.customButton);
        var backdrop = $('<div/>').css({
            position: "fixed",
            "z-index" : 10000,
            backgroundColor : "white",
            opacity : 0.7,
            width: "100%",
            height: "100%",
            top: 0
        });
        var modal = $('<div class="modal" role="dialog" />').css({"z-index": 10001, marginLeft: (p.width || 560)/-2});
        if (p.name != "") {
            modal.addClass(p.name);
        }
        if (!!p.width) modal.css("width", p.width);
        var modalheader = $("<div/>", {"class":"modal-header"});
        if (p.closebutton) {
            modalheader.append($('<button type="button" class="close" data-dismiss="modal" aria-hidden="true" />').html("&times;"));
        }
        modalheader.append($('<h3/>').text(p.title));
        var modalbody = $('<div class="modal-body scrollable" />').append(p.content || p.text);
        modalbody.css("max-height", $(window).height() - 220);
        if (!!p.height) modalbody.css("height", p.height);
        var modalfooter = $('<div class="modal-footer"/>');
        if (p.customButton != null) {
            modalfooter.append($('<a href="#" class="btn" />').addClass(p.customButton.class)
                .click(function(e) {
                    e.preventDefault();
                    p.customButton.click();
                })
                .html(p.customButton.text));
            if (p.customButton.class == "btn-primary") {
                modalfooter.find('.closebtn').removeClass("btn-primary");
            }
        }
        if (p.closebutton) {
            modalfooter.append(
                $('<a href="#" class="closebtn btn btn-primary" data-dismiss="modal" />')
                    .text("Закрыть"));
        }
        if (p.title != "") {
            modal.append(modalheader);
        }
        modal.append(modalbody, modalfooter);
        if (p.backdrop) {
            $('body').append(backdrop);
        }
        $('body').append(modal);
        modalheader = modalbody = modalfooter = null;
        $(modal).modal({backdrop: false});
        // Убиваем окно после его закрытия
        $(modal).on('hidden', function() {
            $(this).remove();
            backdrop.remove()
        });
        $(modal).draggable({ handle: ".modal-header", containment: "window"});
    };
    $(window.zeta).on(window.zeta.handlers.on_modal, function(e,params) {
        $(document).trigger('click.dropdown.data-api');
        ShowModal(params);
    });
    zetamodal.body = $('<div/>');
    root.console.RegisterWidget(zetamodal);
}(window.jQuery);