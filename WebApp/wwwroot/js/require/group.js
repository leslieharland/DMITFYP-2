function refreshGroupTable(oldGroupNumber, direction) {
    var viewModel = {
        groupNumber: (direction == "forward" ? (parseInt(oldGroupNumber) + 1) : direction == "backward" ? (parseInt(oldGroupNumber) - 1) : parseInt(oldGroupNumber)),
        supervisorToAssign: null,
        projectToAssign: null,
        allSupervisors: null,
        allProjects: null,
        groupRoles: null,
        allStudents: null,
        groupDetails: null,
    }

    $.ajax({
        url: "/Group/ViewGroups",
        data: viewModel,
        dataType: "json",
        type: "GET",
        error: function(xhr, status, error) {
            $("#groupTable tbody").html("");
            $("#groupTable tbody").append("<tr><td colspan='7'>Sorry, no students are shown as a HTTP " + xhr.status + " " + xhr.statusText + " error has occured. Please contact your system administrator for assistance.</td></tr>");
        },
        success: function(result, status, xhr) {
            if (result.error != undefined) {
                $("#groupTable tbody").html("");
                $("#groupTable tbody").append("<tr><td colspan='7'>Sorry, no students are shown as the following error occured; " + result.error + " Please contact your system administrator for assistance.</td></tr>");
                return;
            }

            $("#previousSupervisorToAssignHiddenField").val(result.groupDetails.lecturerId);
            $("#previousProjectToAssignHiddenField").val(result.groupDetails.projectId);
            $("#supervisorToAssign").val(result.groupDetails.lecturerId);
            $("#projectToAssign").val(result.groupDetails.projectId);

            $("#groupTable tbody").html("");
            var iterationNumber = 0;
            if (result.groupDetails.groupMembers != null) {
                if (result.groupDetails.groupMembers.length == 0) {
                    var row = $("<tr>");
                    row.append("<td colspan='5'>No student(s) in Group " + result.groupDetails.groupNumber + " yet.</td>");
                    row.append("<td rowspan='5' style='vertical-align:middle;text-align:center'><button type='button' class='btn btn-success btn-block addGroupMembersButton'>Add</button></td>");
                    row.append("<td rowspan='5' style='vertical-align:middle;text-align:center'>" +
                        "<button type='button' class='btn btn-success btn-block deleteGroupButton'>Delete</button>" +
                        "<input type='hidden' id='groupIdHiddenField' value='" + result.groupDetails.groupId + "'/>" +
                        "</td>");
                    $("#groupTable tbody").append(row);
                }

                for (var a = 0; a < result.groupDetails.groupMembers.length; ++a) {
                    ++iterationNumber;
                    var row = $("<tr>");
                    if (iterationNumber == 1) {
                        row.append("<td rowspan='" + result.groupDetails.groupMembers.length + "' style='vertical-align:middle;text-align:center'><strong>" + result.groupDetails.groupNumber + "</strong></td>");
                    }
                    row.append("<td>" + result.groupDetails.groupMembers[a].admin_number + "</td>");
                    row.append("<td>" + result.groupDetails.groupMembers[a].full_name + "</td>");
                    row.append("<td>" +
                        "<select class='form-control groupRoleDropDownList' id='" + result.groupDetails.groupMembers[a].student_id + "studentIdGroupRole' name='" + result.groupDetails.groupMembers[a].student_id + "studentIdGroupRole'" + (result.groupDetails.groupMembers.length != 5 ? "disabled" : "") + ">" +
                        "<option value='1'" + (result.groupDetails.groupMembers[a].group_role == "1" ? "selected" : "") + ">Leader</option>" +
                        "<option value='2'" + (result.groupDetails.groupMembers[a].group_role == "2" ? "selected" : "") + ">Assistant Leader</option>" +
                        "<option value='3'" + (result.groupDetails.groupMembers[a].group_role == "3" ? "selected" : "") + ">Member</option>" +
                        "</select>" +
                        "<input type='hidden' id='previous" + result.groupDetails.groupMembers[a].student_id + "studentIdGroupRoleHiddenField' value='" + result.groupDetails.groupMembers[a].group_role + "'/>" +
                        "</td>");
                    row.append("<td><button type='button' class='btn btn-success deleteGroupMemberButton' id='" + result.groupDetails.groupMembers[a].student_id + "studentIdDeleteGroupMember'>Delete</button></td>");
                    if (iterationNumber == 1) {
                        row.append("<td rowspan='" + result.groupDetails.groupMembers.length + "' style='vertical-align:middle;text-align:center'><button type='button' class='btn btn-success btn-block addGroupMembersButton'" + (result.groupDetails.groupMembers.length == 5 ? "disabled" : "") + ">Add</button></td>");
                        row.append("<td rowspan='" + result.groupDetails.groupMembers.length + "' style='vertical-align:middle;text-align:center'>" +
                            "<button type='button' class='btn btn-success btn-block deleteGroupButton'>Delete</button>" +
                            "<input type='hidden' id='groupIdHiddenField' value='" + result.groupDetails.groupId + "'/>" +
                            "</td>");
                    }
                    $("#groupTable tbody").append(row);
                }
            }

            if (result.numberOfGroups == 0) {
                $("#assignSupervisorAndProjectForGroupForm").addClass("hidden");
                $("#groupTable").addClass("hidden");
                $("#leftArrowViewGroup").addClass("hidden");
                $("#rightArrowViewGroup").addClass("hidden");
                $("#groupNumberIndicationSpan").addClass("hidden");
                $("#noGroupsCreatedYetH4").removeClass("hidden");
                return;
            }

            if (result.numberOfGroups == 1) {
                $("#assignSupervisorAndProjectForGroupForm").removeClass("hidden");
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
    $(".groupRoleDropDownList").change(function() {
        var currentDropDownList = $(this);
        var currentDropDownListIndex = $(".groupRoleDropDownList").index($(this))
        if ($(this).find(":selected").text() == "Leader") {
            $.each($(".groupRoleDropDownList"), function(index, value) {
                if (index != currentDropDownListIndex) {
                    if ($(value).find(":selected").text() == "Leader") {
                        $(value).val("3");
                        $("#studentIdGroupRoleOneHiddenField").val(currentDropDownList.attr("id").split("studentIdGroupRole")[0]);
                        $("#studentIdGroupRoleTwoHiddenField").val($(value).attr("id").split("studentIdGroupRole")[0]);
                        return false;
                    } else {
                        $("#studentIdGroupRoleOneHiddenField").val(currentDropDownList.attr("id").split("studentIdGroupRole")[0]);
                    }
                }
            });
        } else if ($(this).find(":selected").text() == "Assistant Leader") {
            $.each($(".groupRoleDropDownList"), function(index, value) {
                if (index != currentDropDownListIndex) {
                    if ($(value).find(":selected").text() == "Assistant Leader") {
                        $(value).val("3");
                        $("#studentIdGroupRoleOneHiddenField").val(currentDropDownList.attr("id").split("studentIdGroupRole")[0]);
                        $("#studentIdGroupRoleTwoHiddenField").val($(value).attr("id").split("studentIdGroupRole")[0]);
                        return false;
                    } else {
                        $("#studentIdGroupRoleOneHiddenField").val(currentDropDownList.attr("id").split("studentIdGroupRole")[0]);
                    }
                }
            });
        } else {
            $("#studentIdGroupRoleOneHiddenField").val(currentDropDownList.attr("id").split("studentIdGroupRole")[0]);
        }

        $("#confirmChangeGroupRolesModal").modal();
    });

    $(".deleteGroupButton").click(function() {
        $("#confirmDeleteGroupModal").modal();
    });

    $(".deleteGroupMemberButton").click(function() {
        $("#studentIdDeleteGroupMemberHiddenField").val((($(this).attr("id")).split("studentIdDeleteGroupMember"))[0]);
        $("#confirmDeleteGroupMemberModal").modal();
    });

    $(".addGroupMembersButton").click(function() {
        $("#chooseGroupMembersToAddModal").modal();
    });
}

function cancelAssignProject() {
    $("#projectToAssign").val($("#previousProjectToAssignHiddenField").val());
}

function cancelAssignSupervisor() {
    $("#supervisorToAssign").val($("#previousSupervisorToAssignHiddenField").val());
}

function cancelAddGroupMembers() {
    $.each($(".studentCheckBox"), function(index, value) {
        $(value).attr("checked", false);
    });
}

function cancelChangeGroupRoles() {
    $.each($(".groupRoleDropDownList"), function(index, value) {
        $(value).val($("#previous" + $(value).attr("id").split("studentIdGroupRole")[0] + "studentIdGroupRoleHiddenField").val());
    });

    $("#studentIdGroupRoleOneHiddenField").val("");
    $("#studentIdGroupRoleTwoHiddenField").val("");
}

function AJAXChangeGroupRoles() {
    var alertClass = getCurrentAlertClass();
    var inStudentIds = [];
    var inGroupRoles = [];
    inStudentIds.push($("#studentIdGroupRoleOneHiddenField").val());
    inGroupRoles.push($("#" + $("#studentIdGroupRoleOneHiddenField").val() + "studentIdGroupRole").val());
    if ($("#studentIdGroupRoleTwoHiddenField").val() != "") {
        inStudentIds.push($("#studentIdGroupRoleTwoHiddenField").val());
        inGroupRoles.push($("#" + $("#studentIdGroupRoleTwoHiddenField").val() + "studentIdGroupRole").val());
    }

    $.ajax({
        url: "/Group/ChangeGroupRoles",
        data: {
            studentIds: inStudentIds,
            groupRoles: inGroupRoles
        },
        traditional: true,
        dataType: "json",
        type: "GET",
        error: function(xhr, status, error) {
            configureAlertPaneMessageError(xhr, alertClass);

            $("#globalAlertPane").removeClass("hidden");
            $.each($(".groupRoleDropDownList"), function(index, value) {
                $(value).val($("#previous" + $(value).attr("id").split("studentIdGroupRole")[0] + "studentIdGroupRoleHiddenField").val());
            });
            $("#studentIdGroupRoleOneHiddenField").val("");
            $("#studentIdGroupRoleTwoHiddenField").val("");
        },
        success: function(result, status, xhr) {
            configureAlertPaneMessageSuccess(result, alertClass, "group member", "updated");

            $("#globalAlertPane").removeClass("hidden");
            refreshGroupTable($("#groupNumberIndicationSpan").text().split(" ")[0], "none");
            $("#studentIdGroupRoleOneHiddenField").val("");
            $("#studentIdGroupRoleTwoHiddenField").val("");
        }
    });
}

function AJAXDeleteGroup() {
    var alertClass = getCurrentAlertClass();
    $.ajax({
        url: "/Group/DeleteGroup",
        data: {
            groupId: $("#groupIdHiddenField").val()
        },
        dataType: "json",
        type: "GET",
        error: function(xhr, status, error) {
            configureAlertPaneMessageError(xhr, alertClass);

            $("#globalAlertPane").removeClass("hidden");
        },
        success: function(result, status, xhr) {
            configureAlertPaneMessageSuccess(result, alertClass, "group", "deleted");

            $("#globalAlertPane").removeClass("hidden");
            refreshGroupTable(1, "none");
        }
    });
}

function AJAXDeleteGroupMember() {
    var alertClass = getCurrentAlertClass();
    $.ajax({
        url: "/Group/DeleteGroupMember",
        data: {
            studentId: $("#studentIdDeleteGroupMemberHiddenField").val()
        },
        dataType: "json",
        type: "GET",
        error: function(xhr, status, error) {
            configureAlertPaneMessageError(xhr, alertClass);

            $("#globalAlertPane").removeClass("hidden");
        },
        success: function(result, status, xhr) {
            configureAlertPaneMessageSuccess(result, alertClass, "group member", "deleted");

            $("#globalAlertPane").removeClass("hidden");
            refreshGroupTable($("#groupNumberIndicationSpan").text().split(" ")[0], "none");
        }
    });
}

function AJAXAddGroupMembers(groupMembersToAdd) {
    var alertClass = getCurrentAlertClass();
    var temporaryStoreRowsAffected;
    if (groupMembersToAdd.length == 0) return;
    var inStudentIds = [];
    $.each(groupMembersToAdd, function(index, value) {
        inStudentIds.push($(value).attr("id").split("studentIdGroup")[0]);
    });
    $.ajax({
        url: "/Group/AddGroupMembers",
        traditional: true,
        data: {
            studentIds: inStudentIds,
            groupId: $("#groupIdHiddenField").val()
        },
        dataType: "json",
        type: "GET",
        error: function(xhr, status, error) {
            configureAlertPaneMessageError(xhr, alertClass);

            $("#globalAlertPane").removeClass("hidden");
        },
        success: function(result, status, xhr) {
            configureAlertPaneMessageSuccess(result, alertClass, "group member", "added");
            $("#globalAlertPane").removeClass("hidden");

            if (result.invalidStudents != undefined && result.invalidityReasons != undefined) {
                var url;
                url = "/Group/DownloadInvalidStudentsFile?";
                for (var a = 0; a < result.invalidStudents.length; ++a) {
                    url += "invalidStudentIds=" + result.invalidStudents[a] + "&";
                }

                for (var b = 0; b < result.invalidityReasons.length; ++b) {
                    url += "reasonsForInvalidity=" + result.invalidityReasons[b] + "&";
                }

                url = url.substring(0, url.length - 1);
                window.location.href = url;
            }
            refreshGroupTable($("#groupNumberIndicationSpan").text().split(" ")[0], "none");
        }
    });
}

function AJAXAssignProject() {
    var alertClass = getCurrentAlertClass();
    $.ajax({
        url: "/Group/AssignProject",
        data: {
            projectId: $("#projectToAssign").val().length != 0 ? $("#projectToAssign").val() : null,
            groupId: $("#groupIdHiddenField").val()
        },
        dataType: "json",
        type: "GET",
        error: function(xhr, status, error) {
            configureAlertPaneMessageError(xhr, alertClass);
            $("#globalAlertPane").removeClass("hidden");

            $("#projectToAssign").val($("#previousProjectToAssignHiddenField").val());
        },
        success: function(result, status, xhr) {
            configureAlertPaneMessageSuccess(result, alertClass, "group", "updated");
            $("#globalAlertPane").removeClass("hidden");

            if (result.rowsAffected > 0 && result.error == undefined)
                $("#previousProjectToAssignHiddenField").val($("#projectToAssign").val());
            else
                $("#projectToAssign").val($("#previousProjectToAssignHiddenField").val());
        }
    });
}

function AJAXAssignSupervisor() {
    var alertClass = getCurrentAlertClass();
    $.ajax({
        url: "/Group/AssignSupervisor",
        data: {
            lecturerId: $("#supervisorToAssign").val().length != 0 ? $("#supervisorToAssign").val() : null,
            groupId: $("#groupIdHiddenField").val()
        },
        dataType: "json",
        type: "GET",
        error: function(xhr, status, error) {
            configureAlertPaneMessageError(xhr, alertClass);
            $("#globalAlertPane").removeClass("hidden");

            $("#supervisorToAssign").val($("#previousSupervisorToAssignHiddenField").val());
        },
        success: function(result, status, xhr) {
            configureAlertPaneMessageSuccess(result, alertClass, "group", "updated");
            $("#globalAlertPane").removeClass("hidden");

            if (result.rowsAffected > 0 && result.error == undefined)
                $("#previousSupervisorToAssignHiddenField").val($("#supervisorToAssign").val());
            else
                $("#supervisorToAssign").val($("#previousSupervisorToAssignHiddenField").val());
        }
    });
}

$("document").ready(function() {
    $("#downloadProjectSelectionSpreadsheetButton").on("click", function() {
        $(this).button("loading");
        window.location.href = "/Group/DownloadProjectSelectionSpreadsheet"
        var downloadProgressTimer = window.setInterval(function() {
            if (getCookie("completedDownloadToken") == "downloaded") {
                $("#downloadProjectSelectionSpreadsheetButton").button("reset");
                setCookie("completedDownloadToken", "initial");
                clearInterval(downloadProgressTimer);
            }
        }, 1);
    });

    wireViewGroupsEventHandlers();

    $("#leftArrowViewGroup").click(function() {
        var c = $("#groupNumberIndicationSpan").text().split(" ")[0];
        refreshGroupTable(c, "backward");
    });

    $("#rightArrowViewGroup").click(function() {
        var c = $("#groupNumberIndicationSpan").text().split(" ")[0];
        refreshGroupTable(c, "forward");
    });

    $("#projectToAssign").change(function() {
        $("#confirmAssignSupervisorOrProjectButton").attr("onclick", "AJAXAssignProject()");
        $("#cancelAssignSupervisorOrProjectButton").attr("onclick", "cancelAssignProject()");
        $("#confirmAssignSupervisorOrProjectModal").modal();
    })
    $("#supervisorToAssign").change(function() {
        $("#confirmAssignSupervisorOrProjectButton").attr("onclick", "AJAXAssignSupervisor()");
        $("#cancelAssignSupervisorOrProjectButton").attr("onclick", "cancelAssignSupervisor()");
        $("#confirmAssignSupervisorOrProjectModal").modal();
    });

    $("#addGroupButton").click(function() {
        alertClass = getCurrentAlertClass();
        $.ajax({
            url: "/Group/AddGroup",
            type: "GET",
            dataType: "json",
            error: function(xhr, status, error) {
                configureAlertPaneMessageError(xhr, alertClass);
                $("#globalAlertPane").removeClass("hidden");
            },
            success: function(result, status, xhr) {
                configureAlertPaneMessageSuccess(result, alertClass, "group", "added");
                $("#globalAlertPane").removeClass("hidden");

                refreshGroupTable(1, "none");
            }
        });
    });
});