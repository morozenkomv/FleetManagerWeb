jQuery(document).ready(function () {
    $(window).bind('resize', function () {
        SetStyle();
    });

    $('#btnSend').click(function () {
        SendMessage();
    });

    LoadUserGrid();
});

function LoadUserGrid() {
    jQuery('#txtUserSearch').on('keyup', function (e) {
        var postData = jQuery('#tblUser').jqGrid("getGridParam", "postData");
        postData.search = jQuery('#txtUserSearch').val().trim();
        jQuery('#tblUser').jqGrid("setGridParam", { search: true });
        jQuery('#tblUser').trigger("reloadGrid", [{ page: 1, current: true }]);
    });

    jQuery('#tblUser').jqGrid({
        url: '/Mail/BindUserGrid/',
        postData: { search: jQuery('#txtUserSearch').val().trim() },
        datatype: 'json',
        mtype: 'GET',
        colNames: [
            'Employee Code', 'First Name', 'Surname', 'Mobile No', 'Email Id', 'User Name', 'Address', 'Role Name', 'Branch Name', 'Company', 'Is Active'],
        colModel: [
            { name: 'EmployeeCode', index: 'EmployeeCode', align: 'left', fixed: true },
            { name: 'FirstName', index: 'FirstName', align: 'left', fixed: true },
            { name: 'SurName', index: 'SurName', align: 'left', fixed: true },
            { name: 'MobileNo', index: 'MobileNo', align: 'left', fixed: true },
            { name: 'EmailID', index: 'EmailID', align: 'left', fixed: true },
            { name: 'UserName', index: 'UserName', align: 'left', fixed: true },
            { name: 'Address', index: 'Address', align: 'left', fixed: true },
            { name: 'RoleName', index: 'RoleName', align: 'left', fixed: true },
            { name: 'BranchName', index: 'BranchName', align: 'left', fixed: true },
            { name: 'CompanyName', index: 'CompanyName', align: 'left', fixed: true, formatter: CompanyFormat },
            { name: 'IsActive', index: 'IsActive', align: 'left', fixed: true },
        ],
        pager: jQuery('#dvUserFooter'),
        rowNum: 10,
        rowList: [10, 20, 50, 100, 200, 500],
        sortname: 'FirstName',
        sortorder: 'asc',
        viewrecords: true,
        multiselect: true,
        caption: 'List of User',
        height: '100%',
        width: '100%',
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblUser').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblUser').prev()[0].innerHTML = '';
            }
            jQuery('input:checkbox.cbox').uniform();
            SetStyle();
        },
        onSelectAll: function (aRowids, status) {
            jQuery.uniform.update(jQuery('input:checkbox.cbox'));
        },
        beforeSelectRow: function (rowid, e) {
            var $myGrid = $(this),
                i = $.jgrid.getCellIndex($(e.target).closest('td')[0]),
                cm = $myGrid.jqGrid('getGridParam', 'colModel');
            return (cm[i].name === 'cb');
        }
    });
    SetStyle();
}

function CompanyFormat(cellvalue, options, rowObject) {

    if (rowObject.RoleId == AdminRoleID) {
        var link = "";
        if (rowObject.CompanyCount > 0) {
            link = "<a href='javascript:void(0);' title='Click here to view companies assigned to admin' onclick='GetCompanyData(\"" + options.rowId + "\",\"" + rowObject.UserName + "\")'><i class='fa fa-info'></i> <span title='Number of companies assigned to admin.'>" + rowObject.CompanyCount + "</span> </a>";
        }
        return link;
    }
    else {
        return "<span>" + rowObject.CompanyName + "</span>";
    }
}

function SetStyle() {
    $('#tblUser').setGridWidth($('#dvUser').width());
}

function SendMessage() {
    var selRowIds = jQuery('#tblUser').jqGrid('getGridParam', 'selarrrow');
    var objId;
    if (selRowIds.length == 0) {
        jAlert('Please Select At Least One User');
        return false;
    }
    if ($('#txtMessage').val().trim() == '') {
        jAlert(kcs_Message.InputRequired('Message'), 'txtMessage');
        return false;
    }

    for (var i = 0; i < selRowIds.length; i++) {
        if (i == 0) {
            objId = selRowIds[i];
        }
        else {
            objId += ',' + selRowIds[i];
        }
    }
    jQuery.post("/Mail/SendMesaage/", { strUserId: objId, message: $('#txtMessage').val()},
        function (data) {
            if (data.toString() != "") {
                jAlert(data);
                $('#tblUser').trigger('reloadGrid');
            }
        });

}