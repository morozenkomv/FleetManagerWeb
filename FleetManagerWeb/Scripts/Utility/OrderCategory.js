$(document).ready(function () {
    $('.cancel').click(function () {
        if ($('#hdniFrame').val().toLowerCase() == 'false') {
            window.location.href = 'OrderCategoryView';
        }
        else {
            window.parent.$("#divDialog").dialog("close");
        }
    });

    DisplayMessage('OrderCategoryView');

    $('#btnSubmit').click(function () {
        if ($('#strName').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Name'), 'strName');
            return false;
        }
    });
   
});