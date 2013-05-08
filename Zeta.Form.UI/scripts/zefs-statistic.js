(function($) {
    var zefs = window.zefs = window.zefs || {};
    var zeta = window.zeta = window.zeta || {};
    zefs.statistic = zefs.statistic || {
        head : {
            host : "",
            user : "",
            type : "",
            session : null,
            startTime : null,
            endTime : null
        },
        body: {}
    };
    var statistic = zefs.statistic;
    statistic.head.host = location.pathname.match(/\/(\w+)\//)[1];
    $(zeta).on(zeta.handlers.on_getuserinfo, function() {
        statistic.head.user = zeta.user.logonname;
    });
    zefs.api.session.start.onSuccess(function(e, result) {
        statistic.head.session = result.Uid;
    });
    $.extend(statistic.head, zefs.api.getParameters());
    $(window).load(function() {
        var now = new Date();
        statistic.head.startTime = now.toJSON();
    });
    $(zefs).on(zefs.handlers.on_dataload, function() {
        var now = new Date();
        statistic.head.endTime = now.toJSON();
    });
})(jQuery)