var blAdd, blEdit, blDelete;
jQuery(document).ready(function () {
    blAdd = jQuery('#hfAdd').val();
    blEdit = jQuery('#hfEdit').val();
    blDelete = jQuery('#hfDelete').val();

    $(window).bind('resize', function () {
        SetStyle();
    });

    LoadGrid();

    jQuery('#btnDelete').on('click', function () {
        DeleteItem();
    });
});

function LoadGrid() {
    jQuery('#txtSearch').on('keyup', function (e) {
        var postData = jQuery('#tblOrderCategory').jqGrid("getGridParam", "postData");
        postData.search = jQuery('#txtSearch').val().trim();
        jQuery('#tblOrderCategory').jqGrid("setGridParam", { search: true });
        jQuery('#tblOrderCategory').trigger("reloadGrid", [{ page: 1, current: true }]);
    });

    jQuery('#tblOrderCategory').jqGrid({
        url: '/OrderCategory/BindOrderCategoryGrid/',
        postData: { search: jQuery('#txtSearch').val().trim() },
        datatype: 'json',
        mtype: 'GET',
        colNames: [
            'Name', 'Is Active', 'Edit', 'Delete'],
        colModel: [
            { name: 'Name', index: 'Name', align: 'left', fixed: true },
            { name: 'IsActive', index: 'IsActive', align: 'left', fixed: true },
            { name: 'editoperation', index: 'editoperation', align: 'center', width: 40, sortable: false, formatter: EditFormat },
            { name: 'deleteoperation', index: 'deleteoperation', align: 'center', width: 40, sortable: false, formatter: DeleteFormat }
        ],
        pager: jQuery('#dvOrderCategoryFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'Name',
        sortorder: 'asc',
        viewrecords: true,
        multiselect: true,
        caption: 'List of Order Category',
        height: '100%',
        width: '100%',
        ondblClickRow: function (rowid) {
            if (blEdit.toLowerCase() != "false") {
                window.location.href = '../OrderCategory/OrderCategory?' + rowid;
            }
        },
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblOrderCategory').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblOrderCategory').prev()[0].innerHTML = '';
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
        jQuery('#tblOrderCategory').jqGrid('hideCol', ['editoperation']);
    }

    if (blDelete.toLowerCase() == "false") {
        jQuery('#tblOrderCategory').jqGrid('hideCol', ['deleteoperation']);
    }
    SetStyle();
}

function DeleteItem(objId) {
    if (objId == undefined || objId == '') {
        var selRowIds = jQuery('#tblOrderCategory').jqGrid('getGridParam', 'selarrrow');
        if (selRowIds.length == 0) {
            jAlert(kcs_Message.NoRecordToDelete('Order Category'));
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
    jConfirm(kcs_Message.DeleteConfirm('Order Category'), function (r) {
        if (r) {
            jQuery.post("/OrderCategory/DeleteOrderCategory/", { strOrderCategoryId: objId },
            function (data) {
                if (data.toString() != "") {
                    jAlert(data);
                    $('#tblOrderCategory').trigger('reloadGrid');
                }
            });
        }
    });
}

function SetStyle() {
    $('#tblOrderCategory').setGridWidth($('#dvOrderCategory').width());
}


function EditFormat(cellvalue, options, rowObject) {
    return "<a href='../OrderCategory/OrderCategory?" + options.rowId + "'><label class='IconEdit' title='Edit' alt='' /></a>";
}

function DeleteFormat(cellvalue, options, rowObject) {
    return "<a href='javascript:void(0);' onclick='DeleteItem(\"" + options.rowId + "\")'><label class='IconDelete' title='Delete' alt='' /></a>";
}