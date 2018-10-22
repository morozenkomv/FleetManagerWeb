
$(document).ready(function () {
    $('.cancel').click(function () {
        if ($('#hdniFrame').val().toLowerCase() == 'false') {
            window.location.href = 'GroupView';
        }
        else { window.parent.$("#divDialog").dialog("close"); }
    });

    DisplayMessage('GroupView');

    $('#btnSubmit').click(function () {
        if ($('#strGroupName').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Group Name'), 'strGroupName');
            return false;
        }

        if ($('#lgParentGroupId ').val() == 0 || $('#lgParentGroupId').val() == '') {
            if ($('#lgCompanyId').val() == 0 || $('#lgCompanyId').val() == '') {
                jAlert(kcs_Message.InputRequired('Company'), 'lgCompanyId');
                return false;
            }
        }

        if ($("#strUserId").val() == null) {
            jAlert(kcs_Message.InputRequired('User'), 'strUserId');
            return false;
        }

        if ($("#strUserId").val() != null) {
            var data = $("#strUserId").val().toString();
            $("#hdnstrUserIds").val(data);
        }
    });
});

function BindAllUsersByCompany() {

    if ($("#lgParentGroupId").val() == "" && $("#lgCompanyId").val() == "") {
        $("#divAdminUser").hide();
        $("#strUserId").multiselect("rebuild");
    }
    else {
        $("#divAdminUser").show();
        var objParam = new Object();
        objParam.CompanyId = $("#lgCompanyId").val();
        objParam.ParentGroupId = $("#lgParentGroupId").val();

        $.ajax({
            type: "POST",
            url: "BindAllUsersByCompany",
            contentType: "application/json",
            data: JSON.stringify(objParam),
            async: false,
            success: function (res) {
                $("#strUserId").empty();
                if (res.length > 0) {
                    for (var i = 0; i < res.length; i++) {
                        $("#strUserId").append($("<option></option>").val(res[i].Value).html(res[i].Text));
                    }
                    $("#strUserId").multiselect("rebuild");
                }
                else {
                    $("#divAdminUser").hide();
                    $("#strUserId").empty();
                    $("#strUserId").multiselect("rebuild");
                }


                if ($("#hdnstrUserIds").val() != "") {
                    var values = $("#hdnstrUserIds").val();
                    $.each(values.split(","), function (i, e) {
                        $("#strUserId option[value='" + e + "']").prop("selected", true);
                    });
                    $("#strUserId").multiselect("rebuild");
                }
            },
            error: function (res) {
                responseData = null;
            }
        });
    }
}

