$(document).ready(function () {
    $('#strUserId').multiselect({
        includeSelectAllOption: true,
        nonSelectedText: 'All User',
        enableFiltering: true
    });

    if ($("#hdnstrUserIds").val() != "") {
        var values = $("#hdnstrUserIds").val();
        $.each(values.split(","), function (i, e) {
            $("#strUserId option[value='" + e + "']").prop("selected", true);
        });
        $("#strUserId").multiselect("rebuild");
    }

    $('.cancel').click(function () {
        if ($('#hdniFrame').val().toLowerCase() == 'false') {
            window.location.href = 'OrderView';
        }
        else { window.parent.$("#divDialog").dialog("close"); }
    });

    DisplayMessage('OrderView');

    $('#btnSubmit').click(function () {
        if ($('#strName').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Name'), 'strName');
            return false;
        }
        if ($('#lgOrderCategoryId').val() == '' || $('#lgOrderCategoryId').val() == 0) {
            jAlert(kcs_Message.SelectRequired('Order Category'), 'lgOrderCategoryId');
            return false;
        }

        if ($("#strUserId").val() != null) {
            var data = $("#strUserId").val().toString();
            $("#hdnstrUserIds").val(data);
        }
    });
});

function SetStyle() {
    $('#tblGroupPermission').setGridWidth($('#dvRolePermissionFooter').width());
}