jQuery(document).ready(function () {
    $('#btnGenerateReport').on('click', function () {
        if ($('#txtTripStartDateSearch').val().trim() == '') {
            jAlert(kcs_Message.SelectRequired('Trip Start'), 'txtTripStartDateSearch');
            return false;
        }
        if ($('#txtTripEndDateSearch').val().trim() == '') {
            jAlert(kcs_Message.SelectRequired('Trip End'), 'txtTripEndDateSearch');
            return false;
        }
        if ($('#ddlCar').val().trim() == '' || $('#ddlCar').val().trim() == 0) {
            jAlert(kcs_Message.SelectRequired('Car'), 'ddlCar');
            return false;
        }

        $("#btnPrint").hide();
        $("#dvReport").hide ();
        var objParam = new Object();
        objParam.tripStartDate = $("#txtTripStartDateSearch").val();
        objParam.tripEndDate = $("#txtTripEndDateSearch").val();
        objParam.carId = $("#ddlCar").val();

        $.ajax({
            type: "POST",
            url: "/Tracker/GenerateTrackerFormattedReport",
            contentType: "application/json",
            data: JSON.stringify(objParam),
            async: false,
            success: function (Data) {
                if(Data != null && Data.Data.length > 0){
                    $("#ltrCarDescription").html(Data.Data[0].Desc);
                    $("#ltrKmDriven").html(Data.KMDriven);
                    $("#ltrDateRange").html(formatDate($("#txtTripStartDateSearch").val()) + ' to ' + formatDate($("#txtTripEndDateSearch").val()));
                    var row = $("#tblData tr:last-child").removeAttr("style").removeClass().clone(true);
                    $("#tblData tr").not($("#tblData tr:first-child")).remove();
                    $.each(Data.Data, function () {
                        var d = this;
                        $("td", row).eq(0).html(d.Trip_Start);
                        $("td", row).eq(1).html(d.Trip_End);
                        $("td", row).eq(2).html(d.Location_Start);
                        $("td", row).eq(3).html(d.Location_End);
                        $("td", row).eq(4).html(d.TripReasonName);
                        $("td", row).eq(5).html(d.Reason_Remarks);
                        $("td", row).eq(6).html(d.Km_Start);
                        $("td", row).eq(7).html(d.Km_End);
                        $("td", row).eq(8).html(d.Km_Driven);

                        if (!d.IsValid)
                            $(row).addClass('invalid');

                        $("#tblData").append(row);
                        row = $("#tblData tr:last-child").clone(true);
                    });

                    if (Data.IsValid) {
                        $("#btnPrint").show();
                    } else {
                        jAlert('You trips is not valid');
                    }                       

                    $("#dvReport").show();
                }
            },
            error: function (Data) {
                responseData = null;
            }
        });
    });
});

function formatDate(date) {
    var parts = date.split("/");
    var d = new Date(parts[2], parts[1]-1, parts[0]);
    var month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [year, month, day].join('-');
}