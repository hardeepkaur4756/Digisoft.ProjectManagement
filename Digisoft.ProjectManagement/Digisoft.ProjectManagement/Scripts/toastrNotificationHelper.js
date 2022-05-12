var _options = {
    "closeButton": true,
    "debug": false,
    "progressBar": true,
    "preventDuplicates": false,
    "positionClass": "toast-top-right",
    "onclick": null,
    "showDuration": "400",
    "hideDuration": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}

var notificationHelper = new Object();

notificationHelper.ClearAllNotifications = function () {
    toastr.clear();
};
notificationHelper.ShowSuccess = function (message) {
    toastr.options = _options;
    toastr.options.timeOut = 5000;
    toastr.options.extendedTimeOut = 1000;
    toastr.success(message, 'Success');
};

notificationHelper.ShowInfo = function (message) {
    toastr.options = _options;
    toastr.options.timeOut = 8000;
    toastr.options.extendedTimeOut = 1000;
    toastr.info(message, 'Information');
};
notificationHelper.ShowError = function (message) {
    toastr.options = _options;
    toastr.options.timeOut = 20000;
    toastr.options.extendedTimeOut = null;
    toastr.error(message, 'Error');
}