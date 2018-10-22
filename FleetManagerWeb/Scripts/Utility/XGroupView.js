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

    LoadGroupGrid();

    jQuery('#btnDeleteGroup').on('click', function () {
        DeleteItemGroup();
    });
});

function LoadGroupGrid() {
    $grid = $("#tblGroup");

    jQuery('#txtGroupSearch').on('keyup', function (e) {
        var postData = $grid.jqGrid("getGridParam", "postData");
        postData.search = jQuery('#txtGroupSearch').val().trim();
        $grid.jqGrid("setGridParam", { search: true });
        $grid.trigger("reloadGrid", [{ page: 1, current: true }]);
    });

    $grid.jqGrid({
        url: '/Group/BindGroupGrid/',
        postData: { search: jQuery('#txtGroupSearch').val().trim(), ParentGroupId: 0 },
        datatype: 'json',
        mtype: 'GET',
        colNames: ["GroupId", "Group Name", "Company Name", "No of Users", "Edit", "Delete"],
        colModel: [
            { name: 'GroupId', index: 'GroupId', hidden: true },
            { name: 'GroupName', index: 'GroupName', align: 'left', fixed: true },
            { name: 'CompanyName', index: 'CompanyName', align: 'left', fixed: true },
            { name: 'UserCount', index: 'UserCount', align: 'center', fixed: true, formatter: UserMasterFormat },

            { name: 'editoperation', index: 'editoperation', align: 'center', width: 40, sortable: false, formatter: EditFormat },
            { name: 'deleteoperation', index: 'deleteoperation', align: 'center', width: 40, sortable: false, formatter: DeleteFormat },
        ],
        pager: jQuery('#dvGroupFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'GroupName',
        sortorder: 'asc',
        viewrecords: true,
        multiselect: true,
        caption: 'List of Group',
        height: '100%',
        width: '100%',
        ondblClickRow: function (rowid, iRow, iCol, event) {
            var colname = $(event.target).attr('aria-describedby') == undefined ? "" : $(event.target).attr('aria-describedby').substr("tblGroup".length + 1);
            if (colname == "GroupName" || colname == "CompanyName") {
                var rowData = $(this).getRowData(rowid);
                if (blEdit.toLowerCase() != "false") {
                    window.location.href = '../Group/Group?' + rowData.GroupId;
                }
            }
        },
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblGroup').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblGroup').prev()[0].innerHTML = '';
            }
            jQuery('input:checkbox.cbox').uniform();
            SetStyle();
        },
        onSelectAll: function (aRowids, status) {
            jQuery.uniform.update(jQuery('input:checkbox.cbox'));
        },
        //beforeSelectRow: function (rowid, e) {
        //    var $myGrid = $(this),
        //        i = $.jgrid.getCellIndex($(e.target).closest('td')[0]),
        //        cm = $myGrid.jqGrid('getGridParam', 'colModel');
        //    return (cm[i].name === 'cb');
        //},
        subGrid: true,
        subGridRowExpanded: showChildGrid,
    });

    $grid.jqGrid("navGrid", "#pager", { add: false, edit: false, del: false });
    if (blEdit.toLowerCase() == "false") {
        $grid.jqGrid('hideCol', ['editoperation']);
    }

    if (blDelete.toLowerCase() == "false") {
        $grid.jqGrid('hideCol', ['deleteoperation']);
    }

    SetStyle();
    jQuery('#btnExportPdf').on('click', function () {
        kcs_Common.ExportCsvPDF(false, 'Group');
    });

    jQuery('#btnExportCsv').on('click', function () {
        kcs_Common.ExportCsvPDF(true, 'Group');
    });
}

function showChildGrid(parentRowID, parentRowKey) {
    var childGridID = parentRowID + "_table";
    var childGridPagerID = parentRowID + "_pager";
    $('#' + parentRowID).append('<table id=' + childGridID + '></table><div id=' + childGridPagerID + '></div>');

    $('#' + childGridID).jqGrid({
        url: '/Group/BindSubGroupGrid/',
        postData: { search: jQuery('#txtGroupSearch').val().trim(), ParentGroupId: parentRowKey },
        mtype: "GET",
        datatype: "json",
        page: 1,
        colNames: ["Group Name", "Company Name", "User Assigned", "Edit", "Delete"],
        colModel: [
            { name: 'GroupName', index: 'GroupName', align: 'left', fixed: true },
            { name: 'CompanyName', index: 'CompanyName', align: 'left', fixed: true },
            { name: 'UserCount', index: 'UserCount', align: 'center', fixed: true, formatter: UserChildFormat },
            { name: 'editoperation', index: 'editoperation', align: 'center', sortable: false, formatter: SubGridEditFormat },
            { name: 'deleteoperation', index: 'deleteoperation', align: 'center', sortable: false, formatter: SubGridDeleteFormat }
        ],

        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'GroupName',
        sortorder: 'asc',
        viewrecords: true,
        caption: 'List of Group',
        height: '100%',
        width: '100%',
        pager: "#" + childGridPagerID,
        ondblClickRow: function (rowid, iRow, iCol, event) {
            var tableName = '#' + childGridID;
            var colname = $(event.target).attr('aria-describedby') == undefined ? "" : $(event.target).attr('aria-describedby').substr(tableName.length);
            if (colname == "GroupName" || colname == "CompanyName") {
                if (blEdit.toLowerCase() != "false") {
                    window.location.href = '../Group/Group?' + rowid;
                }
            }
        },
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#' + childGridID).prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#' + childGridID).prev()[0].innerHTML = '';
            }
            jQuery('input:checkbox.cbox').uniform();
            $('#' + childGridID).setGridWidth($('#dvGroup').width());
        },
        onSelectAll: function (aRowids, status) {
            jQuery.uniform.update(jQuery('input:checkbox.cbox'));
        },
    });
}

function DeleteItemGroup(objId) {
    if (objId == undefined || objId == '') {
        var gridData = jQuery('#tblGroup').jqGrid('getGridParam', 'selarrrow');

        if (gridData.length == 0) {
            jAlert(kcs_Message.NoRecordToDelete('Group'));
            return false;
        }
        for (var i = 0; i < gridData.length; i++) {
            var Data = $("#tblGroup").getRowData(gridData[i]);
            if (i == 0) {
                objId = Data.GroupId;
            }
            else {
                objId += ',' + Data.GroupId;
            }
        }

    }
    jConfirm(kcs_Message.DeleteConfirm('Group'), function (r) {
        if (r) {
            jQuery.post("/Group/DeleteGroup/", { strGroupId: objId },
            function (data) {
                if (data.toString() != "") {
                    jAlert(data);
                    $('#tblGroup').trigger('reloadGrid');
                }
            });
        }
    });
}

function SetStyle() {
    $('#tblGroup').setGridWidth($('#dvGroup').width());
}

function EditFormat(cellvalue, options, rowObject) {
    return "<a href='../Group/Group?" + rowObject.GroupId + "'><label class='IconEdit' title='Edit' alt='' /></a>";
}

function DeleteFormat(cellvalue, options, rowObject) {
    return "<a href='javascript:void(0);' onclick='DeleteItemGroup(\"" + rowObject.GroupId + "\")'><label class='IconDelete' title='Delete' alt='' /></a>";
}

function SubGridEditFormat(cellvalue, options, rowObject) {
    return "<a href='../Group/Group?" + options.rowId + "'><label class='IconEdit' title='Edit' alt='' /></a>";
}

function SubGridDeleteFormat(cellvalue, options, rowObject) {
    return "<a href='javascript:void(0);' onclick='DeleteItemGroup(\"" + options.rowId + "\")'><label class='IconDelete' title='Delete' alt='' /></a>";
}


function UserMasterFormat(cellvalue, options, rowObject) {
    var link = "";
    if (rowObject.UserCount > 0) {
        link = "<a href='javascript:void(0);' title='Click here to view users assigned to Group' onclick='GetUserData(\"" + rowObject.GroupId + "\",\"" + rowObject.GroupName + "\")'><i class='fa fa-info'></i> <span title='Number of usres assigned to group.'>" + rowObject.UserCount + "</span> </a>";
    }
    return link;
}

function UserChildFormat(cellvalue, options, rowObject) {
    var link = "";
    if (rowObject.UserCount > 0) {
        link = "<a href='javascript:void(0);' title='Click here to view users assigned to Group' onclick='GetUserData(\"" + options.rowId + "\",\"" + rowObject.GroupName + "\")'><i class='fa fa-info'></i> <span title='Number of usres assigned to group.'>" + rowObject.UserCount + "</span> </a>";
    }
    return link;
}

function GetUserData(objId, GroupName) {
    
    $("#spnGroupName").text(GroupName);
    LoadUserDetailGrid(objId);
}
function LoadUserDetailGrid(objId) {

    $('#tblUserGroup').jqGrid('GridUnload');
    jQuery('#tblUserGroup').jqGrid({
        url: '/Group/GetUsersByGroup/',
        postData: { strGroupId: objId },
        datatype: 'json',
        mtype: 'GET',
        colNames: [
            'EmployeeCode', 'FirstName', 'UserName', 'MobileNo', 'EmailID', 'Address'],
        colModel: [
            { name: 'EmployeeCode', index: 'EmployeeCode', align: 'left', fixed: true },
            { name: 'FirstName', index: 'FirstName', align: 'left', fixed: true },
            { name: 'UserName', index: 'UserName', align: 'left', fixed: true },
            { name: 'MobileNo', index: 'MobileNo', align: 'left', fixed: true },
            { name: 'EmailID', index: 'EmailID', align: 'left', fixed: true },
            { name: 'Address', index: 'Address', align: 'left', fixed: true },
        ],
        pager: jQuery('#dvUserGroupFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'UserName',
        sortorder: 'asc',
        viewrecords: true,
        caption: 'List of User',
        height: '100%',
        width: '100%',
        loadComplete: function (data) {
            $('#tblUserGroup').setGridWidth($('#dvUserGroup').width());
        },
    });

    setTimeout(function () {
        $('#tblUserGroup').setGridWidth($('#dvUserGroup').width());
    }, 500);

    $("#UserGroupModal").modal('show');
}