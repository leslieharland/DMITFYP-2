!function ($) {

    "use strict"; // jshint ;_;

    var Rowlink = function (element, options) {
        options = $.extend({}, $.fn.rowlink.defaults, options)
        var tr = element.nodeName == 'tr' ? $(element) : $(element).find('tr:has(td)')

        tr.each(function () {
            var link = $(this).find(options.target).first()
            if (!link.length) return

            var href = link.attr('href')

            $(this).find('td').not('.nolink').click(function () {
                window.location = href;
            })

            $(this).addClass('rowlink')
            link.replaceWith(link.html())
        })
    }


    /* ROWLINK PLUGIN DEFINITION
     * =========================== */

    $.fn.rowlink = function (options) {
        return this.each(function () {
            var $this = $(this)
            , data = $this.data('rowlink')
            if (!data) $this.data('rowlink', (data = new Rowlink(this, options)))
        })
    }

    $.fn.rowlink.defaults = {
        target: "a"
    }

    $.fn.rowlink.Constructor = Rowlink


    /* ROWLINK DATA-API
     * ================== */

    $(function () {
        $('[data-provides="rowlink"]').each(function () {
            $(this).rowlink($(this).data())
        })
    })

}(window.jQuery)