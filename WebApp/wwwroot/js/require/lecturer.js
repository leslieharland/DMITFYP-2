function AJAXDeleteLecturer() {
    var alertClass = getCurrentAlertClass();
    var lecturerId = $("#lecturerIdHiddenField").val();

    $.ajax({
        url: "/Lecturer/DeleteLecturer",
        data: {
            lecturerId: lecturerId,
        },
        dataType: "json",
        type: "GET",
        error: function (xhr, status, error) {
            configureAlertPaneMessageError(xhr, alertClass);
            $("#globalAlertPane").removeClass("hidden");
        },
        success: function (result, status, xhr) {
            configureAlertPaneMessageSuccess(result, alertClass, "lecturer", "deleted");

            $("#globalAlertPane").removeClass("hidden");
            $("#lecturerTable").find("tr#row_" + lecturerId).remove();
        }
    });
}