$(document).ready(function () {
    $('.cancel').click(function () {
        if ($('#hdniFrame').val().toLowerCase() == 'false') {
            window.location.href = 'CompanyView';
        }
        else { window.parent.$("#divDialog").dialog("close"); }
    });

    DisplayMessage('CompanyView');

    $('#btnSubmit').click(function () {
        if ($('#strShortName').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Short Name'), 'strShortName');
            return false;
        }
        if ($('#strFullName').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Full Name'), 'strFullName');
            return false;
        }
        
        if ($('#strAddress1').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Address1'), 'strAddress1');
            return false;
        }
        if ($('#intVat').val() == '') {
            jAlert(kcs_Message.InputRequired('Vat'), 'intVat');
            return false;
        }
        if ($('#strEmail').val() == '') {
            jAlert(kcs_Message.InputRequired('Email'), 'strEmail');
            return false;
        }
        if ($('#strPerson').val() == '') {
            jAlert(kcs_Message.InputRequired('Person'), 'strPerson');
            return false;
        }
        if ($('#strContact').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Contact'), 'strContact');
            return false;
        }
        if ($("#strAdminUserIds").val() == null)
        {
            jAlert(kcs_Message.InputRequired('AdminUserIds'), 'strAdminUserIds');
            return false;
        }
        
        if ($("#strAdminUserIds").val() != null) {
            var data = $("#strAdminUserIds").val().toString();
            $("#hdnstrAdminUserIds").val(data);
        }
    });

});