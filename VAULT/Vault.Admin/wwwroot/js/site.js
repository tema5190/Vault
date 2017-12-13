// Write your JavaScript code.

$(document).ready(function () {

    var isRegistrationFinishedInput = $("#isRegistrationFinishedInput");
    var authModelTypeSelectDiv = $("#authModelTypeSelectDiv");

    if (!isRegistrationFinishedInput.is(":checked")) {
        authModelTypeSelectDiv.hide()
    } else {
        authModelTypeSelectDiv.show();
    };


    isRegistrationFinishedInput.change(function () {
        if ($(this).is(":checked")) {
            authModelTypeSelectDiv.fadeIn("fast");
        }
        else {
            authModelTypeSelectDiv.fadeOut("slow");
        }
    });

})

authModelTypeSelectDiv