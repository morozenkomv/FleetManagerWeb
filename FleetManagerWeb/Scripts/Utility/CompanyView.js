var blAdd, blEdit, blDelete, hdnUserRoleID;
jQuery(document).ready(function () {
    blAdd = jQuery('#hfAdd').val();
    blEdit = jQuery('#hfEdit').val();
    blDelete = jQuery('#hfDelete').val();
    hdnUserRoleID = jQuery('#hdnUserRoleID').val();
    hdnSYSAdminRoleID = jQuery('#hdnSYSAdminRoleID').val();
    hdnAdminRoleID = jQuery('#hdnAdminRoleID').val();
    $(window).bind('resize', function () {
        SetStyle();
    });

    LoadCompanyGrid();

    jQuery('#btnDeleteCompany').on('click', function () {
        DeleteItemCompany();
    });
});

function LoadCompanyGrid() {
    jQuery('#txtCompanySearch').on('keyup', function (e) {
        var postData = jQuery('#tblCompany').jqGrid("getGridParam", "postData");
        postData.search = jQuery('#txtCompanySearch').val().trim();
        jQuery('#tblCompany').jqGrid("setGridParam", { search: true });
        jQuery('#tblCompany').trigger("reloadGrid", [{ page: 1, current: true }]);
    });

    jQuery('#tblCompany').jqGrid({
        url: '/Company/BindCompanyGrid/',
        postData: { search: jQuery('#txtCompanySearch').val().trim() },
        datatype: 'json',
        mtype: 'GET',
        colNames: [
            'Short Name', 'Full Name', 'Address1', 'Address2', 'Address3', 'Vat', 'Email', 'Person', 'Contact', 'Phone', 'Edit', 'Delete', 'Action'],
        colModel: [
            { name: 'ShortName', index: 'ShortName', align: 'left', fixed: true },
            { name: 'FullName', index: 'FullName', align: 'left', fixed: true },
            { name: 'Address1', index: 'Address1', align: 'left', fixed: true },
            { name: 'Address2', index: 'Address2', align: 'left', fixed: true },
            { name: 'Address3', index: 'Address3', align: 'left', fixed: true },
            { name: 'Vat', index: 'Vat', align: 'left', fixed: true },
            { name: 'Email', index: 'Email', align: 'left', fixed: true },
            { name: 'Person', index: 'Person', align: 'left', fixed: true },
            { name: 'Contact', index: 'Contact', align: 'left', fixed: true },
            { name: 'Phone', index: 'Phone', align: 'left', fixed: true },
            { name: 'editoperation', index: 'editoperation', align: 'center', width: 40, sortable: false, formatter: EditFormat },
            { name: 'deleteoperation', index: 'deleteoperation', align: 'center', width: 40, sortable: false, formatter: DeleteFormat },
            { name: 'AddUser', index: 'AddUser', align: 'center', width: 40, sortable: false, formatter: AddUserFormat }
        ],
        pager: jQuery('#dvCompanyFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'ShortName',
        sortorder: 'asc',
        viewrecords: true,
        multiselect: true,
        caption: 'List of Company',
        height: '100%',
        width: '100%',
        ondblClickRow: function (rowid) {
            if (blEdit.toLowerCase() != "false") {
                window.location.href = '../Company/Company?' + rowid;
            }
        },
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblCompany').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblCompany').prev()[0].innerHTML = '';
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

    if (blEdit.toLowerCase() == "false") {
        jQuery('#tblCompany').jqGrid('hideCol', ['editoperation']);
    }

    if (blDelete.toLowerCase() == "false") {
        jQuery('#tblCompany').jqGrid('hideCol', ['deleteoperation']);
    }

    // only sysadmin add update delete company, 
    if (hdnUserRoleID == hdnSYSAdminRoleID) {  // 14 for System Admin
        jQuery('#tblCompany').jqGrid('hideCol', ['AddUser']);
    }
    else {
        if (hdnUserRoleID != hdnAdminRoleID) { // 22 for Admin
            jQuery('#tblCompany').jqGrid('hideCol', ['AddUser']);
        }
        jQuery('#tblCompany').jqGrid('hideCol', ['editoperation']);
        jQuery('#tblCompany').jqGrid('hideCol', ['deleteoperation']);
    }

    SetStyle();
    jQuery('#btnExportPdf').on('click', function () {
        kcs_Common.ExportCsvPDF(false, 'Company');
    });
    jQuery('#btnExportCsv').on('click', function () {
        kcs_Common.ExportCsvPDF(true, 'Company');
    });
}

function DeleteItemCompany(objId) {
    if (objId == undefined || objId == '') {
        var selRowIds = jQuery('#tblCompany').jqGrid('getGridParam', 'selarrrow');
        if (selRowIds.length == 0) {
            jAlert(kcs_Message.NoRecordToDelete('Company'));
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
    }
    jConfirm(kcs_Message.DeleteConfirm('Company'), function (r) {
        if (r) {
            jQuery.post("/Company/DeleteCompany/", { strCompanyId: objId },
            function (data) {
                if (data.toString() != "") {
                    jAlert(data);
                    $('#tblCompany').trigger('reloadGrid');
                }
            });
        }
    });
}

function AddUserItemCompany(objId) {

    if (objId == undefined || objId == '') {
        var selRowIds = jQuery('#tblCompany').jqGrid('getGridParam', 'selarrrow');
        if (selRowIds.length == 0) {
            jAlert(kcs_Message.NoRecordToDelete('Company'));
            return false;
        }
    }

    jQuery.post("/User/User/", { lgCompanyID: objId },
    function (data) {
        if (data.toString() != "") {
            jAlert(data);
            $('#tblCompany').trigger('reloadGrid');
        }
    });

}

function SetStyle() {
    $('#tblCompany').setGridWidth($('#dvCompany').width());
}

function EditFormat(cellvalue, options, rowObject) {
    return "<a href='../Company/Company?" + options.rowId + "'><label class='IconEdit' title='Edit' alt='' /></a>";
}

function DeleteFormat(cellvalue, options, rowObject) {
    return "<a href='javascript:void(0);' onclick='DeleteItemCompany(\"" + options.rowId + "\")'><label class='IconDelete' title='Delete' alt='' /></a>";
}

function AddUserFormat(cellvalue, options, rowObject) {
    return "<a href='../User/User?CompanyId=" + options.rowId + "' class='btn btn-primary' style='color: #ffffff;'>Add User</a>";
    //<a class="btn btn-primary" href="/Company/Company">Add New Company</a>
    //<label class='IconAdd' title='Add User' alt='' />
}