var paginationPageSize = 5;
function AJAXDeleteStudent() {
    var alertClass = getCurrentAlertClass();
    var studentId = $('#studentIdHiddenField').val();

    $.ajax({
        url: "/Student/DeleteStudent",
        data: {
            studentId: studentId
        },
        dataType: "json",
        type: "GET",
        error: function (xhr, status, error) {
            configureAlertPaneMessageError(xhr, alertClass);
            $("#globalAlertPane").removeClass("hidden");
        },
        success: function (result, status, xhr) {
            configureAlertPaneMessageSuccess(result, alertClass, "student", "deleted");
            // After performing all necessary "configurations" on the alert pane, show it.
            $("#globalAlertPane").removeClass("hidden");
            $('#example').find('tr#row_' + studentId).remove();
            //$("#partialFullName").val("");
            //$("option[value='@WebApp.Models.Student.AdminNumberDatabaseColumnName']").attr("selected", "selected");
            //$("option[value='@WebApp.Models.Constants.AscendingOrderSql']").attr("selected", "selected");
            //refreshStudentTable(1, 1);
        }
    });

}
