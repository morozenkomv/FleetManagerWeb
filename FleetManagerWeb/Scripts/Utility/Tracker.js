$(document).ready(function () {
    $('.cancel').click(function () {
        if ($('#hdniFrame').val().toLowerCase() == 'false') {
            window.location.href = 'TrackerView';
        }
        else {
            window.parent.$("#divDialog").dialog("close");
        }
    });

    DisplayMessage('TrackerView');

    $('#inCarIdForRegistration').on('change', function () {
        $('#inCarId').val($('#inCarIdForRegistration').val());
        $('#inCodeId').val($('#inCarIdForRegistration').val());
    });

    $('#inCarId').on('change', function () {
        $('#inCarIdForRegistration').val($('#inCarId').val());
        $('#inCodeId').val($('#inCarId').val());
    });

    $('#inCodeId').on('change', function () {
        $('#inCarIdForRegistration').val($('#inCodeId').val());
        $('#inCarId').val($('#inCodeId').val());
    });

    $('#btnSubmit').click(function () {  
        if ($('#inCarIdForRegistration').val().trim() == '' || $('#inCarIdForRegistration').val().trim() == 0) {
            jAlert(kcs_Message.SelectRequired('Registration'), 'inCarIdForRegistration');
            return false;
        }
        if ($('#inCarId').val().trim() == '' || $('#inCarId').val().trim() == 0) {
            jAlert(kcs_Message.SelectRequired('Car'), 'inCarId');
            return false;
        }
        if ($('#strTripStart').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Trip Start Date'), 'strTripStart');
            return false;
        }
        if ($('#strTripEnd').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Trip End Date'), 'strTripEnd');
            return false;
        }
        if (!CompareDate($('#strTripStart').val().trim(), $('#strTripEnd').val().trim())) {
            jAlert('Trip End Date End should be greater than Trip Start Date', 'strTripEnd');
            return false;
        }
        if ($('#strLocationStart').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Location Start'), 'strLocationStart');
            return false;
        }
        if ($('#strLocationEnd').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Location End'), 'strLocationEnd');
            return false;
        }
        if ($('#lgReasonId').val().trim() == '' || $('#lgReasonId').val().trim() == 0) {
            jAlert(kcs_Message.SelectRequired('Trip Reason'), 'lgReasonId');
            return false;
        }
        if ($('#inKmStart').val().trim() == '' || $('#inKmStart').val().trim() == 0) {
            jAlert(kcs_Message.InputRequired('Km Start'), 'inKmStart');
            return false;
        }
        if ($('#inKmEnd').val().trim() == '' || $('#inKmEnd').val().trim() == 0) {
            jAlert(kcs_Message.InputRequired('Km End'), 'inKmEnd');
            return false;
        }
        if ($('#inKmDriven').val().trim() != '' && parseInt($('#inKmDriven').val().trim()) <= 0) {
            jAlert('Km End should be greater than Km Start', 'inKmEnd');
            return false;
        }
        if ((parseInt($('#inFuelEnd').val().trim()) - parseInt($('#inFuelStart').val().trim())) <= 0) {
            jAlert('Fuel End should be greater than Fuel Start', 'inKmEnd');
            return false;
        }
        if ($('#strEntryMethod').val().trim() == '') {
            jAlert(kcs_Message.InputRequired('Entry Method'), 'strEntryMethod');
            return false;
        }
        if ($('#lgCompanyId').val().trim() == '' || $('#lgCompanyId').val().trim() == 0) {
            jAlert(kcs_Message.SelectRequired('Company'), 'lgCompanyId');
            return false;
        }
    });

    $(".chzn-select").chosen();

    $("#lgCompanyId").change(function () {
        BindGroupByCompany();
    });

    $("#inFuelStart-slider").slider({
        range: "min",
        value: $("#inFuelStart").val(),
        step: 1,
        min: 0,
        max: 8,
        slide: function (event, ui) {
            $("#inFuelStart").val(ui.value);
            $("#inFuelStart-slider-value").html(ui.value);
        }
    });
    $("#inFuelEnd-slider").slider({
        range: "min",
        value: $("#inFuelEnd").val(),
        step: 1,
        min: 0,
        max: 8,
        slide: function (event, ui) {
            $("#inFuelEnd").val(ui.value);
            $("#inFuelEnd-slider-value").html(ui.value);
        }
    });

    $("#inKmStart").change(function () {
        SetKmDriven();
    });

    $("#inKmEnd").change(function () {
        SetKmDriven();
    });
});

function SetKmDriven() {
    var kmStart = 0, kmEnd = 0;
    if ($("#inKmStart").val() != '') {
        kmStart = parseInt($("#inKmStart").val());
    }
    if ($("#inKmEnd").val() != '') {
        kmEnd = parseInt($("#inKmEnd").val());
    }

    $("#inKmDriven").val(kmEnd - kmStart);
}

function BindGroupByCompany()
{
    var objParam = new Object();
    objParam.CompanyId = $("#lgCompanyId").val();
    $.ajax({
        type: "POST",
        url: "BindGroupListByCompany",
        contentType: "application/json",
        data: JSON.stringify(objParam),
        async: false,
        success: function (res) {
            $("#lgGroupId").empty();
            if (res.length > 0) {
                $.each(res, function (key, Data) {
                    $("#lgGroupId").append($("<option></option>").val(Data.Value).html(Data.Text));
                });
                $(".chzn-select").trigger("chosen:updated");
            }
            else {
                $("#lgGroupId").empty();
                $(".chzn-select").trigger("chosen:updated");
            }

        },
        error: function (res) {
            responseData = null;
        }
    });
}

function LoadChangeLogTracker(trackerId) {
    jQuery('#tblChangeLogTracker').jqGrid({
        url: '/Tracker/BindChangeLogTracker/',
        datatype: 'json',
        postData: { trackerId: trackerId },
        mtype: 'GET',
        colNames: [
            'Id', 'Audit Comments', 'Trip Start', 'Trip End', 'Location Start', 'Location End', 'Trip Reason Id', 'Reason Remarks', 'Km Start', 'Km End', 'Km Driven', 'Fuel Start', 'Fuel End', 'Entry Method', 'Editable', 'Active', 'Company Id', 'User', 'Entry Date'],
        colModel: [
            { name: 'Id', index: 'Id', align: 'left', key: true, hidden: true },
            { name: 'AuditComments', index: 'AuditComments', align: 'left' },
            { name: 'TripStartDate', index: 'Trip_Start', align: 'left' },
            { name: 'TripEndDate', index: 'Trip_End', align: 'left' },
            { name: 'LocationStart', index: 'Location_Start', align: 'left' },
            { name: 'LocationEnd', index: 'Location_End', align: 'left' },
            { name: 'TripReasonId', index: 'Reason_Id', align: 'left' },
            { name: 'ReasonRemarks', index: 'Reason_Remarks', align: 'left' },
            { name: 'KmStart', index: 'Km_Start', align: 'left' },
            { name: 'KmEnd', index: 'Km_End', align: 'left' },
            { name: 'KmDriven', index: 'Km_Driven', align: 'left' },
            { name: 'FuelStart', index: 'Fuel_Start', align: 'left' },
            { name: 'FuelEnd', index: 'Fuel_End', align: 'left' },
            { name: 'EntryMethod', index: 'Entry_Method', align: 'left' },
            { name: 'Editable', index: 'Editable', align: 'left' },
            { name: 'Active', index: 'Active', align: 'left' },
            { name: 'CompanyId', index: 'CompanyId', align: 'left' },
            { name: 'Username', index: 'ModifiedBy', align: 'left' },
            { name: 'EntryDate', index: 'ModifiedOn', align: 'left' },
        ],
        pager: jQuery('#dvChangeLogTrackerFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'ModifiedOn',
        sortorder: 'desc',
        viewrecords: true,
        caption: '',
        height: '100%',
        width: '100%',
        multiselect: true,
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblChangeLogTracker').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblChangeLogTracker').prev()[0].innerHTML = '';
            }
            jQuery('input:checkbox.cbox').uniform();
            //SetStyle();
        },
        beforeSelectRow: function (rowid, e) {
            var $myGrid = $(this),
            i = $.jgrid.getCellIndex($(e.target).closest('td')[0]),
            cm = $myGrid.jqGrid('getGridParam', 'colModel');
            return (cm[i].name === 'cb');
        }
    });

    $('#logModal').modal('show');
}

function SetStyle() {
    $('#tblChangeLogTracker').setGridWidth($('#dvChangeLogTracker').width());
}