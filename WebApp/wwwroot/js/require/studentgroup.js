function allFieldsValid(fieldsValidityArray) {
    var valid = true;
    for (var a = 0; a < fieldsValidityArray.length; ++a) {
        if (fieldsValidityArray[a] == false) {
            valid = false;
        }
    }
    return valid;
}

function unbindAllEventsOfProjectChoiceFormFields() {
    $.each($(".projectChoiceDropDownList"), function (index, value) {
        $(value).unbind();
    });
}

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
        url: "/Group/StudentViewGroups",
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
                    if (iterationNumber == 1) {
                        row.append("<td rowspan='" + result.groupDetails.groupMembers.length + "' style='vertical-align:middle;text-align:center'>" +
                            "<button type='button' class='btn btn-success btn-block' id='sendGroupJoiningRequestButton'" + (result.groupDetails.groupMembers.length == 5 ? "disabled" : "") + ">Send</button>" +
                            "<input type='hidden' id='groupIdHiddenField' value='" + result.groupDetails.groupId + "' />" +
                            "</td>");
                    }
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
                wireViewGroupsEventHandlers();
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
            wireViewGroupsEventHandlers();
        }
    });
}

function wireViewGroupsEventHandlers() {
    $("#sendGroupJoiningRequestButton").click(function () {
        $.ajax({
            url: "/GroupJoiningRequest/AddGroupJoiningRequest",
            data: {
                groupId: $("#groupIdHiddenField").val()
            },
            dataType: "json",
            type: "GET",
            error: function (xhr, status, error) {
                alertify.notify("Sorry, HTTP " + xhr.status + " " + xhr.statusText + " error has occured, please contact your system administrator for assistance.", "error", function () { });
            },
            success: function (result, status, xhr) {
                if (result.rowsAffected > 0 && result.error == undefined) {
                    alertify.notify("All " + result.rowsAffected + " group joining request(s) were successfully sent.", "success", function () { });
                    groupJoiningRequestButton.parent().fadeOut("slow");
                    updatePendingGroupJoiningRequestBadge();
                } else {
                    alertify.notify("Only " + result.rowsAffected + " group joining request(s) were sent because; " + result.error + " Please contact your system administrator for assistance, if neccesary.", "error", function () { });
                }
            }
        });
    });
}

$("document").ready(function () {

    wireViewGroupsEventHandlers();

    $("#createGroupButton").click(function () {
        alertify.dialog("confirm").setting("closable");
        alertify.dialog("confirm").set({
            "title": "Add Group",
            "message": "Attempting this action would automatically delete all your pending-response group-joining-request(s). Are you sure?",
            "onok": function () {
                alertClass = getCurrentAlertClass();

                $.ajax({
                    url: "/Group/StudentAddGroup",
                    type: "GET",
                    dataType: "json",
                    error: function (xhr, status, error) {
                        configureAlertPaneMessageError(xhr, alertClass);
                        $("#globalAlertPane").removeClass("hidden");
                    },
                    success: function (result, status, xhr) {
                        configureAlertPaneMessageSuccess(result, alertClass, "group", "added");
                        $("#globalAlertPane").removeClass("hidden");

                        refreshGroupTable(1, "none");
                        $("#createGroupButton").fadeOut("slow");
                        $("h3:contains('Add Group')").fadeOut("slow");
                    }
                })
            }
        }).show();
    });

    $("#leftArrowViewGroup").click(function () {
        var c = $("#groupNumberIndicationSpan").text().split(" ")[0];
        refreshGroupTable(c, "backward");
    });

    $("#rightArrowViewGroup").click(function () {
        var c = $("#groupNumberIndicationSpan").text().split(" ")[0];
        refreshGroupTable(c, "forward");
    });

    $("#submitProjectChoiceButton").click(function () {
        var fieldsValidity = [false, false, false, false];
        var temporaryStoreFormField;
        var projectChoiceDropDownLists = $(".projectChoiceDropDownList");
        var finalProjectChoices = [];
        var finalProjectChoiceDropDownLists;
        var errorMessage;
        var alertClass = getCurrentAlertClass();
        $.each($(".projectChoiceDropDownList"), function (index, value) {
            temporaryStoreFormField = $(value);

            if (temporaryStoreFormField.val() == "0") {
                fieldsValidity[index] = false;
                errorMessage = "This project choice cannot be left blank.";
            } else {
                for (var a = 0; a < projectChoiceDropDownLists.length; ++a) {
                    if (index == a) continue;
                    if ($(projectChoiceDropDownLists[a]).val() == temporaryStoreFormField.val()) {
                        fieldsValidity[index] = false;
                        errorMessage = "This project choice has been selected before.";
                        break;
                    } else {
                        fieldsValidity[index] = true;
                    }
                }
            }

            if (!fieldsValidity[index]) {
                temporaryStoreFormField.parent().addClass("has-error");
                temporaryStoreFormField.next().text(errorMessage);
                temporaryStoreFormField.next().removeClass("hidden");
            }

            temporaryStoreFormField.on("input", function () {
                if ($(this).val() == "0") {
                    fieldsValidity[index] = false;
                    errorMessage = "This project choice cannot be left blank.";
                } else {
                    for (var a = 0; a < projectChoiceDropDownLists.length; ++a) {
                        if (index == a) continue;
                        if ($(projectChoiceDropDownLists[a]).val() == $(this).val()) {
                            fieldsValidity[index] = false;
                            errorMessage = "This project choice has been selected before.";
                            break;
                        } else {
                            fieldsValidity[index] = true;
                        }
                    }
                }

                if (fieldsValidity[index]) {
                    $(this).parent().removeClass("has-error");
                    $(this).next().addClass("hidden");
                    if (allFieldsValid(fieldsValidity)) $("#submitProjectChoiceButton").removeAttr("disabled");
                } else {
                    $(this).parent().addClass("has-error");
                    $(this).next().text(errorMessage);
                    $(this).next().removeClass("hidden");
                    $("#submitProjectChoiceButton").attr("disabled", "disabled");
                }
            });
        });

        if (!allFieldsValid(fieldsValidity)) {
            $("#submitProjectChoiceButton").attr("disabled", "disabled");
            return;
        }

        finalProjectChoiceDropDownLists = $(".projectChoiceDropDownList");
        $.each(finalProjectChoiceDropDownLists, function (index, value) {
            finalProjectChoices.push($(value).val());
        });

        $.ajax({
            url: "/ProjectChoice/SubmitProjectChoices",
            type: "GET",
            data: {
                projectIds: finalProjectChoices
            },
            dataType: "json",
            traditional: true,
            error: function (xhr, status, error) {
                configureAlertPaneMessageError(xhr, alertClass);

                $("#globalAlertPane").removeClass("hidden");
                unbindAllEventsOfProjectChoiceFormFields();
                $.each($(".projectChoiceDropDownList"), function (index, value) {
                    $(value).val("0");
                });
            },
            success: function (result, status, xhr) {
                configureAlertPaneMessageSuccess(result, alertClass, "project choices", "submitted");
                unbindAllEventsOfProjectChoiceFormFields();
                $("#globalAlertPane").removeClass("hidden");
                $.ajax({
                    url: "/ProjectChoice/RetrieveSubmittedProjectChoices",
                    type: "GET",
                    error: function (xhr, status, error) {
                        alert("Sorry, HTTP " + xhr.status + " " + xhr.statusText + " error has occured, please contact your system administrator for assistance.");
                    }, success: function (result, status, xhr) {
                        var serializedDate = result.submittedProjectChoices[0].submitted_date;
                        var date = new Date(parseInt(serializedDate.substring(6, serializedDate.length - 2)));

                        $("#lastSubmittedDateParagraph").text("Last Submitted Date: " + (date.getFullYear() + "/" + ((date.getMonth() + 1).toString().length == 1 ? "0" : "") + (date.getMonth() + 1) + "/" + date.getDate() + " " + date.getHours() + ":" + date.getMinutes()));
                    }
                });
            }
        });
    });
    $("button[data-hide='alert']").on("click", function () {
        // Hide the alert pane, by simply/ merely adding back the class; hidden.
        $("#globalAlertPane").addClass("hidden");
    });
});