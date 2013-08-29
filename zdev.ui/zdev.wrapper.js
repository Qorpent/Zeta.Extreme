window._ = window._ || {};
(function(wrapper) {
    $.extend(wrapper, {
        wiki_findWrap: function(obj) {
            var result = {
                pages: []
            }
            $.each(obj, function(i, o) {
                result.pages.push(o);
            });
            result.pages.reverse();
            return result;
        }
    });
})(_.wrapper = _.wrapper || {});