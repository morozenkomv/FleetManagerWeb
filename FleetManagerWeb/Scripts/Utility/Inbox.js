var blDelete;
jQuery(document).ready(function () {
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
        var postData = jQuery('#tblMail').jqGrid("getGridParam", "postData");
        postData.search = jQuery('#txtSearch').val().trim();
        jQuery('#tblMail').jqGrid("setGridParam", { search: true });
        jQuery('#tblMail').trigger("reloadGrid", [{ page: 1, current: true }]);
    });

    jQuery('#tblMail').jqGrid({
        url: '/Mail/BindInbox/',
        postData: { search: jQuery('#txtSearch').val().trim() },
        datatype: 'json',
        mtype: 'GET',
        colNames: [
            'Message', 'Date', 'Delete'],
        colModel: [
            { name: 'Mail', index: 'Mail', align: 'left', fixed: true },
            { name: 'Date', index: 'Date', align: 'left', fixed: true },
            { name: 'deleteoperation', index: 'deleteoperation', align: 'center', width: 40, sortable: false, formatter: DeleteFormat }
        ],
        pager: jQuery('#dvMailFooter'),
        rowNum: kcs_Common.GridPageSize,
        rowList: kcs_Common.GridPageArray,
        sortname: 'Date',
        sortorder: 'desc',
        viewrecords: true,
        multiselect: true,
        caption: 'Inbox',
        height: '100%',
        width: '100%',
        loadComplete: function (data) {
            if (data.records == 0) {
                $('#tblMail').prev()[0].innerHTML = kcs_Message.GridNoDataFound;
            }
            else {
                $('#tblMail').prev()[0].innerHTML = '';
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

    if (blDelete.toLowerCase() == "false") {
        jQuery('#tblUser').jqGrid('hideCol', ['deleteoperation']);
    }
    SetStyle();
}

function DeleteItem(objId) {
    if (objId == undefined || objId == '') {
        var selRowIds = jQuery('#tblMail').jqGrid('getGridParam', 'selarrrow');
        if (selRowIds.length == 0) {
            jAlert(kcs_Message.NoRecordToDelete('Mail'));
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
    jConfirm(kcs_Message.DeleteConfirm('Mail'), function (r) {
        if (r) {
            jQuery.post("/Mail/DeleteMail/", { strMailId: objId },
            function (data) {
                if (data.toString() != "") {
                    jAlert(data);
                    $('#tblMail').trigger('reloadGrid');
                }
            });
        }
    });
}

function SetStyle() {
    $('#tblMail').setGridWidth($('#dvUser').width());
}

function DeleteFormat(cellvalue, options, rowObject) {
    return "<a href='javascript:void(0);' onclick='DeleteItem(\"" + options.rowId + "\")'><label class='IconDelete' title='Delete' alt='' /></a>";
}