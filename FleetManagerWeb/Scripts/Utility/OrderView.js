var blAdd, blEdit, blDelete;
jQuery(document).ready(function () {
    blAdd = jQuery('#hfAdd').val();
    blEdit = jQuery('#hfEdit').val();
    blDelete = jQuery('#hfDelete').val();

    $(window).bind('resize', function () {
        SetStyle();
    });

    LoadOrderGrid();

    jQuery('#btnDeleteOrder').on('click', function () {
        DeleteItemOrder();
    });
});

function LoadOrderGrid() {
    jQuery('#txtOrderSearch').on('keyup', function (e) {
        var postData = jQuery('#tblOrder').jqGrid("getGridParam", "postData");
        postData.search = jQuery('#txtOrderSearch').val().trim();
        jQuery('#tblOrder').jqGrid("setGridParam", { search: true });
        jQuery('#tblOrder').trigger("reloadGrid", [{ page: 1, current: true }]);
    });

    jQuery('#tblOrder').jqGrid({
        url: '/Order/BindOrderGrid/',
        postData: { search: jQuery('#txtOrderSearch').val().trim() },
        datatype: 'json',
        mtype: 'GET',
        colNames: [
            'Name', 'Category', 'Edit', 'Delete'],
        colModel: [
            { name: 'Name', index: 'Name', align: 'left', fixed: true },
            { name: 'OrderCategory', index: 'OrderCategory', align: 'left', fixed: true },
            { name: 'editoperation', index: 'editoperation', align: 'center', width: 40, sortable: false, formatter: EditFormat },
            { name: 'deleteoperation', index: 'deleteoperation', align: 'center', width: 40, sortable: false, formatter: DeleteFormat }
        ],
        pager: jQuery('#dvOrderFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'Name',
        sortorder: 'asc',
        viewrecords: true,
        multiselect: true,
        caption: 'List of Order',
        height: '100%',
        width: '100%',
        ondblClickRow: function (rowid) {
            if (blEdit.toLowerCase() != "false") {
                window.location.href = '../Order/Order?' + rowid;
            }
        },
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblOrder').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblOrder').prev()[0].innerHTML = '';
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
        jQuery('#tblOrder').jqGrid('hideCol', ['editoperation']);
    }

    if (blDelete.toLowerCase() == "false") {
        jQuery('#tblOrder').jqGrid('hideCol', ['deleteoperation']);
    }
    SetStyle();
}

function DeleteItemOrder(objId) {
    if (objId == undefined || objId == '') {
        var selRowIds = jQuery('#tblOrder').jqGrid('getGridParam', 'selarrrow');
        if (selRowIds.length == 0) {
            jAlert(kcs_Message.NoRecordToDelete('Order'));
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
    jConfirm(kcs_Message.DeleteConfirm('Order'), function (r) {
        if (r) {
            jQuery.post("/Order/DeleteOrder/", { strOrderId: objId },
            function (data) {
                if (data.toString() != "") {
                    jAlert(data);
                    $('#tblOrder').trigger('reloadGrid');
                }
            });
        }
    });
}

function SetStyle() {
    $('#tblOrder').setGridWidth($('#dvOrder').width());
}


function EditFormat(cellvalue, options, rowObject) {
    return "<a href='../Order/Order?" + options.rowId + "'><label class='IconEdit' title='Edit' alt='' /></a>";
}

function DeleteFormat(cellvalue, options, rowObject) {
    return "<a href='javascript:void(0);' onclick='DeleteItemOrder(\"" + options.rowId + "\")'><label class='IconDelete' title='Delete' alt='' /></a>";
}