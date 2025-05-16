var msgSaveOnly = "Save changes to the system ?";
var msgSaveandReturnHome = "Save changes and return to SSWP home page?";
var msgCancelChanges = "Cancel changes and return SSWP home page?";
var msgUpload = "Upload the file to the system?";
var msgArchive = "Archive SSWP?";
var msgRoute = "Route SSWP to selected approver?";
var msgPrint = "Save changes and print the Work Plan?";
var peoplePickerToolTip = "Enter names or email addresses....";
var mssProjectNoFormat = "Project Number must to be at least 7 characters long. Use seven zeros if unknown.";

function InitaUserPicker(divcontrol) {
    initializePeoplePicker(divcontrol);
    InitDefaultValueForPicker(divcontrol);
}
function InitaUserPickerSingle(divcontrol) {
    initializePeoplePickerSingle(divcontrol);
    InitDefaultValueForPicker(divcontrol);
}

function InitDefaultValueForPicker(divcontrol) {
    $('#' + divcontrol + '_TopSpan').addClass('TopSpanClass');
    $('#' + divcontrol + '_TopSpan_InitialHelpText').text(peoplePickerToolTip);
    $('#' + divcontrol + '_TopSpan').prop('title', peoplePickerToolTip);
    $('#' + divcontrol + '_TopSpan_EditorInput').prop('title', peoplePickerToolTip);
}

// --- Delete Dialog box
function DisplayModalMessageBoxDelete(lblmessage) {

    $('#msbDiv').attr("class", "ShowModalMessage");
    $('#parentMsbDiv').attr("class", "overlay-bg");
    $('#lblMessageDeleteFile').html(lblmessage);
    return false;
}
function RemoveModalMessageBoxDelete() {
    $('#msbDiv').attr("class", "ShowModalMessage");
    $('#parentMsbDiv').attr("class", "HideDiv");
    $('#btnActionHere').attr('href', '#');
    return false;
}
function RemoveModalMessageBoxDeleteContinue() {
    $('#msbDiv').attr("class", "ShowModalMessage");
    $('#parentMsbDiv').attr("class", "HideDiv");
}
function confirmDeleteFile() {
    $(".removeFileInGrid").click(function () {
        var hrefVal = $(this).attr('href');
        $('#btnActionHere').attr('href', hrefVal);
        DisplayModalMessageBoxDelete("Are you sure you want to delete this file?");
        //
        $("#btnActionHere").click(function () {
            RemoveModalMessageBoxDeleteContinue();
        });
        //
        return false;
    });

}
function KeppParentDivOverLay() {

}
function confirmDeleteItemWithMessage(delMsg) {
    $(".deleteWPAction").click(function () {
        var hrefVal = $(this).attr('href');
        $('#btnActionHere').attr('href', hrefVal);
        DisplayModalMessageBoxDelete(delMsg);
        $("#btnActionHere").click(function () {
            KeppParentDivOverLay();
        });
        return false;
    });
}



// --------------- END Delete Dialog box


function PrintNTransfer(eddCode, type) {
    var url = "WorkPlanPrintView.aspx?pndid=" + eddCode;
    if (type == '1')
        url = "WorkPlanPrintView.aspx?ViewMode=executive&pndid=" + eddCode;
    window.open(url, '_blank');
    //setTimeout(function () {
    //    window.location.href = "EditEDD.aspx?EDDID=" + eddCode;
    //}, 1);

}

function PrintNOnly(eddCode, type) {
    var url = "WorkPlanPrintView.aspx?pndid=" + eddCode;
    if (type == '1')
        url = "WorkPlanPrintView.aspx?ViewMode=executive&pndid=" + eddCode;
    window.open(url, '_blank');
}

function encodeVal() {

}

function initializePeoplePickerCommon(peoplePickerElementId, allowMultiple) {
    var schema = {};
    schema['PrincipalAccountType'] = 'User,DL,SecGroup,SPGroup';
    schema['SearchPrincipalSource'] = 15;
    schema['ResolvePrincipalSource'] = 15;
    schema['AllowMultipleValues'] = allowMultiple;
    schema['MaximumEntitySuggestions'] = 50;
    schema['Width'] = '100%';
    //schema['Height'] = '55px';
    SPClientPeoplePicker_InitStandaloneControlWrapper(peoplePickerElementId, null, schema);
}

function initializePeoplePicker(peoplePickerElementId) {
    initializePeoplePickerCommon(peoplePickerElementId, true);
}

function initializePeoplePickerSingle(peoplePickerElementId) {
    initializePeoplePickerCommon(peoplePickerElementId, false);
}
function UpdateUserPicker2TxtInGrid(upicker) {
    var parentEle = document.getElementById(upicker).parentNode;
    getUserInfo(upicker, parentEle.getElementsByClassName('txtApproverHidden')[0].id);
}
function UpdateTxtInGrid2UserPicker(txtInGrind) {
    var parentEle = document.getElementById(txtInGrind).parentNode;
    var value = document.getElementById(txtInGrind).value;
    if (value != '') {
        SPClientPeoplePicker.SPClientPeoplePickerDict[parentEle.getElementsByClassName('UserApproverDiv')[0].id + '_TopSpan'].AddUserKeys(value);
    }
}
function BindTexttoPicker(pickerControlId, txtControlId) {
    var value = document.getElementById(txtControlId).value;
    if (value != '') {
        SPClientPeoplePicker.SPClientPeoplePickerDict[pickerControlId + '_TopSpan'].AddUserKeys(value);
    }
}

//PeoplePickerDiv_TopSpan
function getUserInfo(PeoplePickerDiv, keyControlId) {

    var peoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[PeoplePickerDiv + '_TopSpan'];
    /*var users = peoplePicker.GetAllUserInfo();
    var userInfo = '';
    for (var i = 0; i < users.length; i++) {
        var user = users[i];
        for (var userProperty in user) {
            userInfo += userProperty + ':  ' + user[userProperty] + '<br>';
        }
    }*/
    var keys = peoplePicker.GetAllUserKeys();
    document.getElementById(keyControlId).value = keys;
}
function GetValueofPicker(PeoplePickerDiv) {
    var peoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[PeoplePickerDiv + '_TopSpan'];
    return peoplePicker.GetAllUserKeys();
}
function resizeFreeTextBox(freeTxtId) {
    $('.' + freeTxtId + '_OuterTable').css('width', '100%');
    $('.' + freeTxtId + '_DesignBox').css('width', '100%');
}

function ValidateNumberWithLength(txtValueId, lgth) {
    x = document.getElementById(txtValueId).value;

    // If x is Not a Number or less than one or greater than 10
    if (isNaN(x) || x.length != lgth) {
        message = "Project Number must to be numberic and " + lgth + " digits long";
    }
}


function showMessageInPage(ParentmssPanel, mssPanel, lblMessage, message) {
    document.getElementById(ParentmssPanel).className = "overlay-bg";
    document.getElementById(mssPanel).className = "ShowModalMessage";
    document.getElementById(lblMessage).innerHTML = message;
    $('.btnDialogboxGroup :button').hide();
    $('.btnDialogboxGroup :button').hide();
    return false;
}


// Get parameters from the query string.
// For production purposes you may want to use a library to handle the query string.
function getQueryStringParameter(paramToRetrieve) {
    var params = document.URL.split("?")[1].split("&");
    for (var i = 0; i < params.length; i = i + 1) {
        var singleParam = params[i].split("=");
        if (singleParam[0] == paramToRetrieve) return singleParam[1];
    }
}

function ShowMessage(mss) {
    document.getElementById('sMessage').innerHTML = String.format('* {0}', mss);
    $('#myModal').modal('show');
    document.getElementById('okConfirmBtn').style.display = 'none';
    document.getElementById('cancelConfirmBtn').style.display = 'none';

}
function ConfirmMss(mss) {
    document.getElementById('sMessage').innerHTML = String.format('* {0}', mss);
    $('#myModal').modal('show');
    document.getElementById('okConfirmBtn').style.display = '';
    document.getElementById('cancelConfirmBtn').style.display = 'none';
}
function ConfirmCancelUpload() {
    var mssCancel = "This action will close the form, you might loose unsaved changes in the form. Click OK if you wish to continue.";
    document.getElementById('sMessage').innerHTML = String.format('* {0}', mssCancel);
    $('#myModal').modal('show');
    document.getElementById('okConfirmBtn').style.display = 'none';
    document.getElementById('cancelConfirmBtn').style.display = '';
}
function CloseMessage(mss) {
    $('#sMessage').innerHTML = '';
    $('#myModal').modal('hide');
}
function ConfirmUpload() {
    CloseMessage();
}

function closeWindow() {
    window.close();
}


function displayLayover(url) {
    var options = SP.UI.$create_DialogOptions();
    options.url = url;
    options.showClose = false,
    options.dialogReturnValueCallback = scallback;
    SP.UI.ModalDialog.showModalDialog(options);
    return false;
}

function scallback(dialogResult, returnValue) {
    if (dialogResult == SP.UI.DialogResult.OK) {
        $('.refreshbtncss').click();
        //SP.UI.ModalDialog.RefreshPage(SP.UI.DialogResult.OK, null);
    }
}
function LoadFloatFormat() {
    $(".floatInput").on("blur", null, function () {
        var input = $(this);
        var value = input.val().replace(/,/g, '');
        var num = parseFloat(value).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
        if (num != 'NaN')
            input.val(num);
    });
}
function LoadInputFloatFormat() {
    $(".floatInput").each(function () {
        var lbl = $(this);
        var value = lbl.val().replace(/,/g, '');
        var num = parseFloat(value).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
        if (num != 'NaN')
            lbl.val(num);
    });
}
function LoadLableFloatFormat() {
    $(".floatInput").each(function () {
        var lbl = $(this);
        var value = lbl.html().replace(/,/g, '');
        var num = parseFloat(value).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
        if (num != 'NaN')
            lbl.html('$' + num);
    });
}

function PagingFithteen() {
    try {

        var PageNumber = 15;
        $('.pagination').find("a").hide();
        var totalPage = Number($('.pagination').find("li").length - 2);
        if (totalPage > 1)
            $('.pagination__previous').show();
        var currentPage = Number($('.pageActive').text());
        $('.pagination').find("a").each(function () {
            var start = Math.floor(currentPage / PageNumber) * PageNumber;
            var end = Number(start) + Number(PageNumber);
            var val = Number($(this).text());

            if (currentPage >= PageNumber) {
                if (val == 1) {
                    $(this).show();
                    var parentEle = $(this).parent();
                    var chtml = $(this).parent().html();
                    parentEle = $(this).parent().html(chtml + '<span>...</span>');
                }
            }
            //
            if (val >= (start - 1) && val <= end) {
                $(this).show();
            }
            if (end <= totalPage && totalPage > PageNumber) {
                if (val == totalPage) {
                    $(this).show();
                    var parentEle = $(this).parent();
                    var chtml = $(this).parent().html();
                    parentEle = $(this).parent().html('<span>...</span>' + chtml);
                }
            }

        });
    }
    catch (ex) {

    }
}
function LoadHelpLink() {
    $('.helpcsslink').click(function () {
        window.open("help.aspx", "_blank");

    });

}




function confirmRemoveFile() {
    $('.bottom-nav__link').find('.removeButton').click(function () {

    });

}

function getAllApprovalDepts(hdfUpdateId) {
    var allSelected = '';
    $(".optionWithRealDeptValue input:checked").each(function () {
        var deptCode = $(this).attr('id');
        allSelected += deptCode + ';'
    });
    allSelected = ";" + allSelected;
    $('#' + hdfUpdateId).val(allSelected);
}

function loadAllApprovalDepts(hdfUpdateId) {
    var allSelected = $('#' + hdfUpdateId).val();
    $(".optionWithRealDeptValue input[type=checkbox]").each(function () {
        var deptCode = ';' + $(this).attr('id') + ';';
        if (allSelected.indexOf(deptCode) > -1) {
            $(this).prop("checked");
        }
    });
}

function createNewSaveAction() {
    $('.btnSaveAction').attr('data-toggle', 'modal');
    $('.btnSaveAction').attr('data-target', '#myConfirmationModal');
    $('.btnAction').attr('data-toggle', 'modal');
    $('.btnAction').attr('data-target', '#myConfirmationModal');
}


