function refreshGroupTable(oldGroupNumber, direction) {
    var viewModel = {
        groupNumber: (direction == "forward" ? (parseInt(oldGroupNumber) + 1) : direction == "backward" ? (parseInt(oldGroupNumber) - 1) : parseInt(oldGroupNumber)),
        supervisorToAssign: null,
        projectToAssign: null,
        allSupervisors: null,
        allProjects: null,
        groupDetails: null,
    }
    $.ajax({
        url: "/Group/LecturerViewGroups",
        data: viewModel,
        dataType: "json",
        type: "GET",
        error: function (xhr, status, error) {
            $("#groupTable tbody").html("");
            $("#groupTable tbody").append("<tr><td colspan='7'>Sorry, no students are shown as a HTTP " + xhr.status + " " + xhr.statusText + " error has occured. Please contact your system administrator for assistance.</td></tr>");
        },
        success: function (result, status, xhr) {
            if (result.error != undefined) {
                $("#groupTable tbody").html("");
                $("#groupTable tbody").append("<tr><td colspan='7'>Sorry, no students are shown as the following error occured; " + result.error + " Please contact your system administrator for assistance.</td></tr>");
                return;
            }

            $("#supervisorToAssign").val(result.groupDetails.lecturerId);
            $("#projectToAssign").val(result.groupDetails.projectId);

            $("#groupTable tbody").html("");
            var iterationNumber = 0;
            if (result.groupDetails.groupMembers != null) {
                for (var a = 0; a < result.groupDetails.groupMembers.length; ++a) {
                    ++iterationNumber;
                    var row = $("<tr>");
                    if (iterationNumber == 1) {
                        row.append("<td rowspan='" + result.groupDetails.groupMembers.length + "' style='vertical-align:middle;text-align:center'><strong>" + result.groupDetails.groupNumber + "</strong></td>");
                    }
                    row.append("<td>" + result.groupDetails.groupMembers[a].admin_number + "</td>");
                    row.append("<td>" + result.groupDetails.groupMembers[a].full_name + "</td>");
                    row.append("<td>" + (result.groupDetails.groupMembers[a].group_role == "1" ? "Leader" : (result.groupDetails.groupMembers[a].group_role == "2" ? "Assistant Leader" : "Member")) + "</td>");

                    $("#groupTable tbody").append(row);
                }
            }

            if (result.numberOfGroups == 0) {
                $("#groupTable").addClass("hidden");
                $("#leftArrowViewGroup").addClass("hidden");
                $("#rightArrowViewGroup").addClass("hidden");
                $("#groupNumberIndicationSpan").addClass("hidden");
                $("#noGroupsCreatedYetH4").removeClass("hidden");
                return;
            }

            if (result.numberOfGroups == 1) {
                $("#groupTable").removeClass("hidden");
                $("#leftArrowViewGroup").removeClass("hidden");
                $("#rightArrowViewGroup").removeClass("hidden");
                $("#groupNumberIndicationSpan").removeClass("hidden");
                $("#noGroupsCreatedYetH4").addClass("hidden");

                $("#leftArrowViewGroup").attr("disabled", "disabled");
                $("#rightArrowViewGroup").attr("disabled", "disabled");
                $("#groupNumberIndicationSpan").html("<strong>" + oldGroupNumber + "</strong> of <strong>" + result.numberOfGroups + " </strong>");
                return;
            }

            if (direction == "forward" || direction == "backward") {
                if (direction == "forward") {
                    if (oldGroupNumber == "1") {
                        $("#leftArrowViewGroup").removeAttr("disabled");
                    }
                } else {
                    if (oldGroupNumber == "2") {
                        $("#leftArrowViewGroup").attr("disabled", "disabled");
                    }
                }

                if (direction == "forward") {
                    if ((parseInt(oldGroupNumber) + 1) == result.numberOfGroups) {
                        $("#rightArrowViewGroup").attr("disabled", "disabled");
                    }
                } else {
                    if ((parseInt(oldGroupNumber) - 1) != result.numberOfGroups) {
                        $("#rightArrowViewGroup").removeAttr("disabled");
                    }
                }
            } else {
                if (oldGroupNumber == "1") {
                    $("#leftArrowViewGroup").attr("disabled", "disabled");
                    $("#rightArrowViewGroup").removeAttr("disabled");
                }
            }
            $("#groupNumberIndicationSpan").html("<strong>" + (direction == "forward" ? (parseInt(oldGroupNumber) + 1) : direction == "backward" ? (parseInt(oldGroupNumber) - 1) : parseInt(oldGroupNumber)) + "</strong> of <strong>" + result.numberOfGroups + " </strong>");
        }
    });
}

$("document").ready(function () {

    $("#leftArrowViewGroup").click(function () {
        var c = $("#groupNumberIndicationSpan").text().split(" ")[0];
        refreshGroupTable(c, "backward");
    });

    $("#rightArrowViewGroup").click(function () {
        var c = $("#groupNumberIndicationSpan").text().split(" ")[0];
        refreshGroupTable(c, "forward");
    });

    $("button[data-hide='alert']").on("click", function () {
        // Hide the alert pane, by simply/ merely adding back the class; hidden.
        $("#globalAlertPane").addClass("hidden");
    });
});