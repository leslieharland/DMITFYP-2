require([
         'bforms-initUI',
], function () {
    var AccountAll = function (options) {
        this.options = $.extend(true, {}, options);
    };

    AccountAll.prototype.init = function () {
        this.$demoForm = $(".js-demoForm");

        //apply BForms plugins
        this.$demoForm.bsInitUI(this.options.styleInputs);
    };

    $(document).ready(function () {
        var ctrl = new AccountAll(requireConfig.pageOptions);
        ctrl.init();
    });
});