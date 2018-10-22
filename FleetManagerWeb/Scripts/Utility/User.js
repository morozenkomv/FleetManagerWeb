$(document).ready(function () {
    $('.cancel').click(function () {
        if ($('#hdniFrame').val().toLowerCase() == 'false') {
            window.location.href = 'UserView';
        }
        else { window.parent.$("#divDialog").dialog("close"); }
    });

    $("#strEmployeeCode").mask("EMP/99999");

    //if (parseInt($('#lgBranchId').val()) > 0) {
    //    $('#strBranchName').val($("#lgBranchId option:selected").text());
    //    if (parseInt($('#lgVehicleDistributeId').val()) > 0) {
    //        //$('#dvBranchDropdown').hide();
    //        //$('#dvBranchTextBox').show();
    //    }
    //}

    DisplayMessage('UserView');

    $('#btnSubmit').click(function () {
        if ($('#strFirstName').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('First Name'), 'strFirstName');
            return false;
        }
        if ($('#strSurName').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Surname'), 'strSurName');
            return false;
        }
        if ($('#strMobileNo').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Mobile No'), 'strMobileNo');
            return false;
        }
        if ($('#strEmailID').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Email Id'), 'strEmailID');
            return false;
        }
        if ($('#strUserName').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('User Name'), 'strUserName');
            return false;
        }
        if ($('#strPassword').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Password'), 'strPassword');
            return false;
        }
        if ($('#strConfirmPassword').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Confirm Password'), 'strPassword');
            return false;
        }

        if ($('#lgBranchId').val() == '') {
            jAlert(kcs_Message.SelectRequired('Branch'), 'lgBranchId');
            return false;
        }
        //if ($('#strEmployeeCode').val().trim() == '') {
        //    jAlert(kcs_Message.InputRequired('Employee Code'), 'strEmployeeCode');
        //    return false;
        //}

        if ($('#strPassword').val().trim() != $("#strConfirmPassword").val().trim()) {
            jAlert(kcs_Message.PasswordNotMatch);
            return false;
        }

        $("#blTripStartDate").val($("#ckbTripStartDate").prop("checked"));
        $("#blTripEndDate").val($("#ckbTripEndDate").prop("checked"));
        $("#blLocationStart").val($("#ckbLocationStart").prop("checked"));
        $("#blLocationEnd").val($("#ckbLocationEnd").prop("checked"));
        $("#blTripReasonName").val($("#ckbTripReasonName").prop("checked"));
        $("#blReasonRemarks").val($("#ckbReasonRemarks").prop("checked"));
        $("#blKmStart").val($("#ckbKmStart").prop("checked"));
        $("#blKmEnd").val($("#ckbKmEnd").prop("checked"));
        $("#blKmDriven").val($("#ckbKmDriven").prop("checked"));
        $("#blFuelStart").val($("#ckbFuelStart").prop("checked"));
        $("#blFuelEnd").val($("#ckbFuelEnd").prop("checked"));
        $("#blUsername").val($("#ckbUsername").prop("checked"));
        $("#blEntryDatetime").val($("#ckbEntryDatetime").prop("checked"));
        $("#blEntryMethod").val($("#ckbEntryMethod").prop("checked"));
        $("#blEditable").val($("#ckbEditable").prop("checked"));
        $("#blActive").val($("#ckbActive").prop("checked"));
        $("#blCompanyName").val($("#ckbCompanyName").prop("checked"));
        $("#blEditColumn").val($("#ckbEditColumn").prop("checked"));
        $("#blDeleteColumn").val($("#ckbDeleteColumn").prop("checked"));
    });

    $(document).on('click', '#divblIsActive', function () {
        if ($("#blIsActive").prop("disabled")) {
            jAlert('Set the checkbox should not be possible until the user is assigned to the role');
        }
    });

    $("#lgRoleId").change(function () {
        ManageAssignAdmin();
    });
});

function ManageAssignAdmin() {

    if ($("#lgRoleId").val() == "") {
        $("#blIsActive").prop("checked", false);
        $("#blIsActive").prop("disabled", "disabled");
        $("#blIsActive").parent().removeClass("checked");
        $("#divAdminUser").hide();
        $("#lgAdminUserId").empty();
        $(".chzn-select").trigger("chosen:updated");
    }
    else {
        $("#blIsActive").prop("disabled", false);

        $("#blIsActive").prop("checked", true);
        $("#blIsActive").parent().addClass("checked");

        if ($("#lgRoleId").val() == AdminRoleID || $("#lgRoleId").val() == SYSAdminRoleID) {
            $("#divAdminUser").hide();
            $("#lgAdminUserId").empty();
            $(".chzn-select").trigger("chosen:updated");
        }
        else {
            $("#divAdminUser").show();
            //bind Admin User By Role
            var objParam = new Object();
            objParam.RoleId = $("#lgRoleId").val();
            $.ajax({
                type: "POST",
                url: "BindAdminUserList",
                contentType: "application/json",
                data: JSON.stringify(objParam),
                async: false,
                success: function (res) {
                    $("#lgAdminUserId").empty();
                    if (res.length > 0) {
                        $.each(res, function (key, Data) {
                            $("#lgAdminUserId").append($("<option></option>").val(Data.Value).html(Data.Text));
                        });
                        $(".chzn-select").trigger("chosen:updated");
                    }
                    else {
                        $("#divAdminUser").hide();
                        $("#lgAdminUserId").empty();
                        $(".chzn-select").trigger("chosen:updated");
                    }

                },
                error: function (res) {
                    responseData = null;
                }
            });
        }
    }
}

function LoadGroupPermission() {
    jQuery('#tblGroupPermission').jqGrid({
        url: '/User/BindUserTrackerPermissionGrid/',
        postData: { lgUserId: jQuery('#lgId').val() },
        datatype: 'json',
        mtype: 'GET',
        colNames: [
            'Id', 'TripStartDate', 'TripEndDate', 'LocationStart', 'LocationEnd', 'TripReason', 'ReasonRemarks', 'KmStart', 'KmEnd', 'KmDriven', 'FuelStart', 'FuelEnd', 'Username', 'EntryDatetime', 'EntryMethod', 'Editable', 'Active', 'CompanyName', 'Edit', 'Delete'], colModel: [
            { name: 'Id', index: 'Id', align: 'left', key: true, hidden: true },
            { name: 'TripStartDate', index: 'TripStartDate', align: 'center', sortable: false, formatter: checkboxFormatter },
            { name: 'TripEndDate', index: 'TripEndDate', align: 'center', sortable: false, formatter: checkboxFormatter },
            { name: 'LocationStart', index: 'LocationStart', align: 'center', sortable: false, formatter: checkboxFormatter },
            { name: 'LocationEnd', index: 'LocationEnd', align: 'center', sortable: false, formatter: checkboxFormatter },
            { name: 'TripReasonName', index: 'TripReasonName', align: 'center', sortable: false, formatter: checkboxFormatter },
            { name: 'ReasonRemarks', index: 'ReasonRemarks', align: 'center', sortable: false, formatter: checkboxFormatter },
            { name: 'KmStart', index: 'KmStart', align: 'center', sortable: false, formatter: checkboxFormatter },
            { name: 'KmEnd', index: 'KmEnd', align: 'center', sortable: false, formatter: checkboxFormatter },
            { name: 'KmDriven', index: 'KmDriven', align: 'center', sortable: false, formatter: checkboxFormatter },
            { name: 'FuelStart', index: 'FuelStart', align: 'center', sortable: false, formatter: checkboxFormatter },
            { name: 'FuelEnd', index: 'FuelEnd', align: 'center', sortable: false, formatter: checkboxFormatter },
            { name: 'Username', index: 'Username', align: 'center', sortable: false, formatter: checkboxFormatter },
            { name: 'EntryDatetime', index: 'EntryDatetime', align: 'center', sortable: false, formatter: checkboxFormatter },
            { name: 'EntryMethod', index: 'EntryMethod', align: 'center', sortable: false, formatter: checkboxFormatter },
            { name: 'Editable', index: 'Editable', align: 'center', sortable: false, formatter: checkboxFormatter },
            { name: 'Active', index: 'Active', align: 'center', sortable: false, formatter: checkboxFormatter },
            { name: 'CompanyName', index: 'CompanyName', align: 'center', sortable: false, formatter: checkboxFormatter },
            { name: 'EditColumn', index: 'EditColumn', align: 'center', sortable: false, formatter: checkboxFormatter },
            { name: 'DeleteColumn', index: 'DeleteColumn', align: 'center', sortable: false, formatter: checkboxFormatter }
            ],
        rowNum: 1000,
        viewrecords: true,
        caption: 'Group Tracker Permission',
        height: '100%',
        width: '100%',
        loadComplete: function (data) {
            jQuery('input:checkbox, input:radio').uniform();
            SetStyle();
        }
    });
}

function checkboxFormatter(cellvalue, options, rowObject) {
    cellvalue = cellvalue + "";
    cellvalue = cellvalue.toLowerCase();
    var bchk = cellvalue.search(/(false|0|no|off|n)/i) < 0 ? " checked=\"checked\"" : "";
    return "<input type='checkbox' " + bchk + " value='" + cellvalue + "' offval='no' id=ckb" + options.colModel.name + " name=cls" + options.colModel.name + " class='ckbGroupTracker' />"
}

function SetStyle() {
    $('#tblGroupPermission').setGridWidth($('#dvRolePermissionFooter').width());
}