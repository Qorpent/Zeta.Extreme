/**
 * Виджет сообщений
 */
!function($) {
    var root = window.zeta = window.zeta || {};
    var zefslogo = new root.Widget("zefslogo", root.console.layout.position.layoutHeader, null, { authonly: false });

    var main = $('<div class="logo-main"/>');
    main.click(function() {
        window.open("http://ru.wikipedia.org/wiki/%D0%94%D0%B5%D0%BD%D1%8C_%D0%9F%D0%BE%D0%B1%D0%B5%D0%B4%D1%8B", "_blank");
    });
    zefslogo.body = $($('<div/>').append(
        main/*,
        $('<div class="logo-second"/>')*/
    ));
    root.console.RegisterWidget(zefslogo);
}(window.jQuery);