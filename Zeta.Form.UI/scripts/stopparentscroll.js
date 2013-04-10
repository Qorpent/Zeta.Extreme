(function($) {
    $.fn.stopParentScroll = function() {
        if (this.get(0).scrollHeight <= this.innerHeight()) return;
        this.bind('mousewheel DOMMouseScroll',function(e) {
            var delta = e.originalEvent.wheelDelta || -e.originalEvent.detail;
            if (delta > 0 && $(this).scrollTop() <= 0) return false;
            if (delta < 0 && $(this).scrollTop() >= this.scrollHeight - $(this).innerHeight()) return false;
            return true;
        });
    }
})(jQuery);