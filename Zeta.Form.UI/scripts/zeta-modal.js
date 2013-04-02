/**
 * Виджет окна с сообщениями
 */
!function($) {
    window.zeta.handlers = $.extend(window.zeta.handlers, {
        on_modal : "modal"
    });
    var zetamodal = new root.security.Widget("zetamodal", root.console.layout.position.layoutBodyMain, null);
    var ShowModal = function(p) {
        p = $.extend({
            title: "",
            content: null,
            ok: null,
            text: "",
            type: "",
            fade: false,
            width: 0,
            height: 0
        },p);
        var modal = $('<div class="modal" role="dialog" />');
        if (p.width != 0) modal.css("width", p.width);
        var modalheader = $("<div/>", {"class":"modal-header"}).append(
            $('<button type="button" class="close" data-dismiss="modal" aria-hidden="true" />').html("&times;"),
            $('<h3/>').text(p.title));
        var modalbody = $('<div class="modal-body" />').append(p.content || p.text);
        if (p.height != 0) modalbody.css("height", p.height);
        var modalfooter = $('<div class="modal-footer"/>').append(
            $('<a href="#" class="closebtn btn btn-primary" data-dismiss="modal" />')
                .text("Закрыть"));
        if (p.ok != null) {
            modalfooter.append($('<a href="#" class="btn btn-primary" />')
                .click(function() {
                    p.ok();
                    $(modal).modal('hide');
                })
                .html("OK"));
            modalfooter.find('.closebtn').removeClass("btn-primary");
        }
        modal.append(modalheader, modalbody, modalfooter);
        $('body').append(modal);
        modalheader = modalbody = modalfooter = null;
        $(modal).modal({backdrop: false});

        // Убиваем окно после его закрытия
        $(modal).on('hidden', function() { $(this).remove() });
        $(modal).draggable({ handle: ".modal-header", containment: "window"});
    };
    $(window.zeta).on(window.zeta.handlers.on_modal, function(e,params) {
        $(document).trigger('click.dropdown.data-api');
        ShowModal(params);
    });
    zetamodal.body = $('<div/>');
    root.console.RegisterWidget(zetamodal);
}(window.jQuery);