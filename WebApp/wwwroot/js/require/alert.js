$("document").ready(function () {
    $("button[data-hide='alert']").on("click", function () {
        // Hide the alert pane, by simply/ merely adding back the class; hidden.
        $("#globalAlertPane").addClass("hidden");
    });
});


function configureAlertPaneMessageSuccess(data, alertClass, subject, action) {
    if (data.rowsAffected > 0 && data.error == undefined) {
        if (alertClass != alertClasses.success) {
            // Change the class of the alert pane to alert-success if it is not alert-success.
            $("#globalAlertPane").removeClass(alertClass);
            $("#globalAlertPane").addClass(alertClasses.success);
        }
        // Change the message of the alert pane.
        $($("#globalAlertPane").children()[1]).html("All <b><u>" + data.rowsAffected + "</u></b> " + subject + "(s) were successfully " + action + ".");
    } else {
        if (alertClass != alertClasses.warning) {
            // Change the class of the alert pane to alert-warning if it is not alert-warning.
            $("#globalAlertPane").removeClass(alertClass);
            $("#globalAlertPane").addClass(alertClasses.warning);
        }
        // Change the message of the alert pane.
        $($("#globalAlertPane").children()[1]).html("Only <b><u>" + data.rowsAffected + "</u></b> " + subject + "(s) were " + action + " because; " + data.error + " Please contact your system administrator for assistance, if neccesary.");
    }
}

function configureAlertPaneMessageError(xhrObject, alertClass) {
    // Change the class of the alert pane to alert-danger if it is not alert-danger.
    if (alertClass != alertClasses.danger) {
        $("#globalAlertPane").removeClass(alertClass);
        $("#globalAlertPane").addClass(alertClasses.danger);
    }

    // Change the message of the alert pane.
    $($("#globalAlertPane").children()[1]).text("Sorry, HTTP " + xhrObject.status + " " + xhrObject.statusText + " error has occured, please contact your system administrator for assistance.");
}

var alertClasses = {
    success: "alert-success",
    info: "alert-info",
    warning: "alert-warning",
    danger: "alert-danger"
}

function getCurrentAlertClass() {
    var currentAlertClass;
    var classList = $("#globalAlertPane").attr("class").split(/\s+/);
    for (var a = 0; a < classList.length; ++a) {
        if (classList[a] == alertClasses.success || classList[a] == alertClasses.info || classList[a] == alertClasses.warning || classList[a] == alertClasses.danger) {
            currentAlertClass = classList[a];

            // A break statement is sure, as not to carry on with the for-loop in futility.
            break;
        }
    }
    return currentAlertClass;
}