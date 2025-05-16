<%@ Assembly Name="BART.SP.OCR.CP.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9abfeb7dc254e359" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewUserControl.ascx.cs" Inherits="BART.SP.OCR.CP.Web.View.ViewUserControl" %>
<%@ Register Assembly="Telerik.Web.UI, Version=2016.2.504.45, Culture=neutral, PublicKeyToken=121fae78165ba3d4" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<SharePoint:ScriptLink Name="clienttemplates.js" runat="server" LoadAfterUI="true" Localizable="false" />
<SharePoint:ScriptLink Name="clientforms.js" runat="server" LoadAfterUI="true" Localizable="false" />
<SharePoint:ScriptLink Name="clientpeoplepicker.js" runat="server" LoadAfterUI="true" Localizable="false" />
<SharePoint:ScriptLink Name="autofill.js" runat="server" LoadAfterUI="true" Localizable="false" />
<SharePoint:ScriptLink Name="sp.js" runat="server" LoadAfterUI="true" Localizable="false" />
<SharePoint:ScriptLink Name="sp.runtime.js" runat="server" LoadAfterUI="true" Localizable="false" />
<SharePoint:ScriptLink Name="sp.core.js" runat="server" LoadAfterUI="true" Localizable="false" />

<script type="text/javascript" src="/_layouts/15/BART.SP.OCR.CP.Web/js/bootstrap-datepicker.min.js?refId=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>"></script>
<script type="text/javascript" src="/SiteAssets/AppCP/Core.js?refId=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>"></script>
<script type="text/javascript" src="/SiteAssets/AppCP/UI.js?versionview=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>"></script>
<script type="text/javascript" src="/SiteAssets/AppCP/Validation.js?versionview=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>"></script>
<script type="text/javascript" src="/SiteAssets/AppCP/LoadNEvents.js?versionview=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>"></script>

<link rel="stylesheet" type="text/css" href="/_layouts/15/BART.SP.OCR.CP.Web/css/OriginalCSS.css?refId=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>" />
<link rel="stylesheet" type="text/css" href="/_layouts/15/BART.SP.OCR.CP.Web/css/bootstrap-datepicker3.css?refId=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>" />
<link rel="stylesheet" type="text/css" href="/SiteAssets/AppCP/Core.css?refId=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>" />

<span>
    <%=BART.SP.OCR.CP.Common.ProjectUtilities.BuildTopMenu(Request.Url.ToString().ToLower())%>
</span>

<%-- Start of Step 1 Edit Script --%>
<script>
    $(document).ready(function () {
        try {
            LoadReadOnlyView();
            createNewSaveAction();
            $('#div_OCRCCUAnalysis_TopSpan_InitialHelpText').html('OCR CCU Analysis Prepared By');
            $('#div_OCRCCUAnalysis_TopSpan').attr('title', 'OCR CCU Analysis Prepared By');

            $('#div_OCRLCUAnalysis_TopSpan_InitialHelpText').html('OCR LCU Analysis Prepared By');
            $('#div_OCRLCUAnalysis_TopSpan').attr('title', 'OCR LCU Analysis Prepared By');

            $('#div_OCRAnalyst_TopSpan_InitialHelpText').html('OCR Analyst');
            $('#div_OCRAnalyst_TopSpan').attr('title', 'OCR Analyst');
            
            $('#div_OCRAnalyst_TopSpan').css("background-color", "transparent");
            //$('#stakeHoldersDiv_TopSpan_InitialHelpText').html('Enter Stakeholders’s name');
            //$('#stakeHoldersDiv_TopSpan').attr('title', '');

            $('#RGroupManageroginDiv_TopSpan_InitialHelpText').html('Enter Group/Lead Manager’s name');
            $('#RGroupManageroginDiv_TopSpan .sp-peoplepicker-editorInput').attr('');
            $('#RGroupManageroginDiv_TopSpan').attr('title', '');
            $('.sp-peoplepicker-editorInput').attr('title', '');
              
        }
        catch (e) { }
    });
    
    function pageLoad()
    {

        InitPicker();
        Breaktext();
        DisableUnchecked();
        LoadCheckLCU();
        activaTabApproval();
        //displayCalendar();
        hideLoadingDiv();
        createVotingBtnActions();
        loadRatingFunction();
        tabClick();
        loaddocs();
        LoadProjectFunction();
        //linknextClickEdit();
        //linkbackClickNovalidate();
        //SelectProgram();
        if ($('.hdfCurrentTab').val())
            activaTab($('.hdfCurrentTab').val());
        LoadFloatFormat();
        LoadCalculationFormat();
        //showRequiredScheduleFields();
        $('[data-toggle="tooltip"]').tooltip();
        //LoadDelCtr();

    }

    function displayCalendar()
    {
        var date_input = $('div.input-group.date'); //our date input has the name "date"
        var container = $('.bootstrap-iso form').length > 0 ? $('.bootstrap-iso form').parent() : "body";
        var options = {
            format: 'mm/dd/yyyy',
            container: container,
            todayHighlight: true,
            autoclose: true,
        };
        date_input.datepicker(options);

        var date_input2 = $('.datepickertxt'); //our date input has the name "date"
        var container2 = $('.bootstrap-iso form').length > 0 ? $('.bootstrap-iso form').parent() : "body";
        var options2 = {
            format: 'mm/dd/yyyy',
            container: container2,
            todayHighlight: true,
            autoclose: true,
        };
        date_input2.datepicker(options2);

    }
    function loaddocs()
    {
        var cStatus = $('#' + '<%=this.hdfInitialStatusValue.ClientID%>').val(); 
        if (cStatus == '<%=BART.SP.OCR.CP.Common.ProjectSettings.ProjectStatusApproved%>' || cStatus == '<%=BART.SP.OCR.CP.Common.ProjectSettings.ProjectStatusCompleted%>') {$('.sswpuploadedDocuments li a.removeUploadedFile').hide();}
        else { LoadRemoveUploadedFile('<%= this.hdfDeletedDocMarkIDs.ClientID %>');}
    }
    function activaTab(tab) {
        try {

            $('#myTabs a[href="' + tab + '"]').tab('show');
            $('.btngroupactions a[linkvalue="' + tab + '"]').show();
            //balanceNextBackViewEdit(tab);

        } catch (e) { }
        
    }
    
    function tabClick()
    {
        $('#myTabs li a[role=tab]').click(function () {
            var hrefval = $(this).attr('href');
            $('.hdfCurrentTab').val(hrefval);
            //balanceNextBackViewEdit(hrefval);
        });
    }


    function InitPicker() {
        $('.divUserInputField').each(function (index) {
            InitaUserPickerSingle($(this).attr('id'));
        });
        $('.divUserInpuMultiple').each(function (index) {
            InitaUserPicker($(this).attr('id'));
        });
        $('.divUserNameAutoComplete').each(function (index) {
            InitaUserPickerSingle($(this).attr('id'));
        });
        $('.divUserNameAutoComplete').each(function (index) {
            InitaUserPickerSingleNameOnly($(this).attr('id'));
        });
        var pLink = '<%=BART.SP.OCR.CP.Common.ProjectUtilities.PrintReportURL(this.hdf_MainObjID.Value.Trim())%>';
        $('.printsswplink').attr('href', pLink);
        
        //cancelEnterinSingleText();
        allLoadJSCreateNew();
        <%--  BindTexttoPicker('div_SponsorProjectManager', '<%= this.hdf_SponsorProjectManagerLogin.ClientID %>');
        BindTexttoPicker('div_OCRAnalyst', '<%= this.hdf_OCRAnalystLogin.ClientID %>');
        BindTexttoPicker('div_DepartmentChief', '<%= this.hdf_DepartmentChiefLogin.ClientID %>');
        BindTexttoPicker('div_DepartmentAGM', '<%= this.hdf_DepartmentAGMLogin.ClientID %>');
        BindTexttoPicker('div_GroupManager', '<%= this.hdf_GroupManagerLogin.ClientID %>');
        BindTexttoPicker('div_OCRCCUAnalysis', '<%= this.hdf_OCRCCUAnalysisLogin.ClientID %>');
        BindTexttoPicker('div_OCRLCUAnalysis', '<%= this.hdf_OCRLCUAnalysisLogin.ClientID %>');--%>
         try{
            BindTexttoPicker('div_OCRAnalyst', '<%= this.hdf_OCRAnalystLogin.ClientID %>');
        }catch(e){}

    } 
    function updateApproversValue()
    {
        <%-- getUserInfo('div_SponsorProjectManager', '<%= this.hdf_SponsorProjectManagerLogin.ClientID %>');
        getUserInfo('div_OCRAnalyst', '<%= this.hdf_OCRAnalystLogin.ClientID %>');
        getUserInfo('div_DepartmentChief', '<%= this.hdf_DepartmentChiefLogin.ClientID %>');
        getUserInfo('div_DepartmentAGM', '<%= this.hdf_DepartmentAGMLogin.ClientID %>');
        getUserInfo('div_GroupManager', '<%= this.hdf_GroupManagerLogin.ClientID %>');
        getUserInfo('div_OCRCCUAnalysis', '<%= this.hdf_OCRCCUAnalysisLogin.ClientID %>');
        getUserInfo('div_OCRLCUAnalysis', '<%= this.hdf_OCRLCUAnalysisLogin.ClientID %>');--%>
        try{
            getUserInfo('div_OCRAnalyst', '<%= this.hdf_OCRAnalystLogin.ClientID %>');
        }catch(e){}
    }

    //function CopyReport()
    //{
    //    $('.commitAction').hide();
    //    $('.txtComfirmationMss').html('Copy this report to a new report ?');
    //    $('.commitAction.btnCommitCopy').show();
    //}
    //function RouteRR() {
    //    var rval = ValidateForm();
    //    if (rval == 0) {
    //        updateApproversValue();
    //        SaveInfo('2');
    //    }

    //}
    //function NextRRPage() {
    //    var rval = ValidateForm();
    //    if (rval == 0) {
    //        updateApproversValue();
    //        SaveInfo('0');
    //    }
    //    else {
    //        $('#myConfirmationModal').modal(); return false;
    //    }
    //}
   
    
</script>
<%-- End of Step 1 Edit Script --%>


<div class="container displayview">
    <div class="row sswpsecondnav sub-nav" style="display: none !important">
        <div class="col-md-6 sswpbreadcrumb sub-nav__link ">
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="index.aspx">Home</a></li>
                <li class="breadcrumb-item active">Creating New MDD</li>
                <li class="breadcrumb-item">Step 1</li>
            </ol>
        </div>
        <div class="col-md-6 sub-nav__buttons">
            <a id="print" data-toggle="tooltip" style="display: none;" class="badge printer" href="#" title="Print SSWP"><i class="fa fa-print fa-2" aria-hidden="true"></i></a>
            <span data-toggle="tooltip" title="Cancel changes and return to home page."><a href="#" class="badge printer lbtCancel btnAction" onclick="CancelChanges();"><i class="fa fa-times fa-2" aria-hidden="true"></i></a></span>
        </div>
    </div>

    <%--
<ul class="nav nav-wizard row">
  <li class="completed">
      <a href="#" data-toggle="tab">1- Project Summary</a>
  </li>
  <li class="completed"><a>2- Funding/Budget</a></li>
  <li class="completed"><a>3- Schedule/Activities/Look-Ahead</a></li>
  <li class="completed"><a>4- Change Status/Project Issues</a></li>
  <li class="completed"><a>5- Photo</a></li>
</ul>--%>

    <asp:Panel runat="server" ID="pnlErrorMsg" Visible="false" CssClass="alert alert-danger alert-dismissable pnlErrorMsgCss">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">×</a>
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlSuccessMsg" Visible="false" CssClass="alert alert-success alert-dismissable pnlErrorMsgCss">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">×</a>
        <asp:Label ID="lblComplete" runat="server">Your input was saved successfully.</asp:Label>
    </asp:Panel>

    <div data-example-id="togglable-tabs" class="tabingparentDiv EditReport">

        <asp:Label ID="lblErrorMessage" ForeColor="Red" runat="server" Text=""></asp:Label>
        <a name="top"></a>

        <ul class="nav nav-tabs" id="myTabs" role="tablist" style="margin-left: -12px;">
            <li role="presentation" class="active"><a href="#step1" id="step1-tab" role="tab" data-toggle="tab" aria-controls="step1tab" aria-expanded="false">1. General Info</a></li>
            <li role="presentation"><a href="#step2" id="step2-tab" role="tab" data-toggle="tab" aria-controls="step2tab" aria-expanded="false">2. Contracts</a></li>
            <li role="presentation"><a href="#step3" id="step3-tab" role="tab" data-toggle="tab" aria-controls="step3tab" aria-expanded="false">3. Approvers</a></li>
            <li role="presentation"><a href="#step4" id="step4-tab" role="tab" data-toggle="tab" aria-controls="step4tab" aria-expanded="false"><i class="fa fa-comments" aria-hidden="true"></i>View Comments</a></li>
            <li role="presentation"><a href="#step5" id="step5-tab" role="tab" data-toggle="tab" aria-controls="step5tab" aria-expanded="false"><i class="fa fa-history" aria-hidden="true"></i>Histories</a></li>
            <li style="float: right">
                <div style="margin-top: -3px;">
                    <a id="Edittop" data-toggle="tooltip" onserverclick="Editbottom_ServerClick" class="badge printer" runat="server" href="EditMDD.aspx" title="Click here to edit this CP"><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>
                    <a id="printTop" data-toggle="tooltip" class="badge printer printsswplink" href="#" target="_blank" title="Print Contracting Plan"><i class="fa fa-print" aria-hidden="true"></i></a>
                    <span data-toggle="tooltip" title="Cancel changes and return to home page."><a href="#" class="badge printer lbtCancel btnAction" onclick="CancelChanges();"><i class="fa fa-times fa-2" aria-hidden="true"></i></a></span>

                </div>
            </li>


            <li class="votingLi" style="float: right; margin-right: 15px;">
                <asp:Panel runat="server" ID="pndVoting" CssClass="divVotingApproval">
                    <span data-toggle="tooltip" title="Concur" style="display: inline-block; margin-right: 5px;"><a id="lbtVoteConcur" class="btn btn-success active approvebtn btnApprovalDecision" href="#"><i class="fa fa-check-square-o" aria-hidden="true"></i>Concur</a></span>
                    <span data-toggle="tooltip" title="Write a Comment" style="display: inline-block; margin-right: 5px;"><a id="lbtWriteComment" class="btn btn-info active cmmtbtn btnApprovalDecision" href="#"><i class="fa fa-comment" aria-hidden="true"></i>Comment</a></span>
                    <span data-toggle="tooltip" title="If you reject, initiator will be forced to re-route from beginning." style="display: inline-block; margin-right: 5px;"><a id="lbtVoteAbstain" class="btn btn-warning active rejectbtn btnApprovalDecision" href="#"><i class="fa fa-ban" aria-hidden="true"></i>Reject</a></span>
                </asp:Panel>

                <asp:Panel runat="server" ID="pnlAsign" CssClass="divAssignOCR">
                    <a id="lbtAssignOCRA" class="btn btn-info active assignocrbtn btnApprovalDecision" href="#" title="Click here to assign OCR Analyst"><i class="fa fa-user-circle" aria-hidden="true"></i>Assign OCR Analyst >></a>
                </asp:Panel>

            </li>
        </ul>
        <div class="tab-content" id="ssswpTabContent">


            <%-- Step 1 --%>

            <div class="tab-pane fade in active" id="step1">
                <div class="row top-container">
                    <div class="top-inner-container">
                        <asp:Panel ID="pnlStep1" CssClass="col-md-12" runat="server">


                            <div class="form-group sswpFields" style="margin-bottom: 0px;">

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="col-md-4">
                                            <label class="lblforField">
                                                Service Type(s)
                                            </label>
                                            <span class="markRequired">*</span>
                                            <asp:CheckBoxList ID="cbx_ServiceType" label="Service Type(s)" CssClass="cbx_ConsultantBType checkboxlist checklistrequired" RepeatColumns="3" runat="server">
                                                <asp:ListItem Value="Construction">Construction</asp:ListItem>
                                                <asp:ListItem Value="Design Build">Design Build</asp:ListItem>
                                                <asp:ListItem Value="IFB">IFB</asp:ListItem>
                                                <asp:ListItem Value="NASPO">NASPO</asp:ListItem>
                                                <asp:ListItem Value="Procurement">Procurement</asp:ListItem>
                                                <asp:ListItem Value="Service Agreement">Service Agreement</asp:ListItem>
                                            </asp:CheckBoxList>

                                        </div>
                                        <div class="col-md-4">
                                            <label class="lblforField">
                                                Program Name</label><span class="markRequired">*</span>
                                            <asp:Label CssClass="form-control" data-toggle="tooltip" ID="lbl_ProgramDes" placeholder="" runat="server"></asp:Label>
                                        </div>

                                        <div class="col-md-4">
                                            <label class="lblforField">
                                                Project Name</label><span class="markRequired">*</span>

                                            <asp:Label CssClass="form-control" data-toggle="tooltip" ID="lbl_ProjectName" placeholder="" runat="server"></asp:Label>
                                        </div>

                                    </div>
                                </div>

                                <div class="row">

                                    <div class="col-md-12">

                                        <div class="col-md-4">
                                            <label class="lblforField">
                                                Project ID</label>
                                            <asp:Label CssClass="txtreadonly form-control txt_ProjectID" data-toggle="tooltip" ID="lbl_ProjectID" placeholder="(Auto-Populated)" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <label class="lblforField">
                                                Originating Business Unit
                                            </label>
                                            <asp:Label CssClass="form-control" label="Originating Business Unit" data-toggle="tooltip" ID="lbl_BusinessUnit" placeholder="Originating Business Unit" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <label class="lblforField">
                                                Department/Sponsor
                                            </label>
                                            <span class="markRequired">*</span>
                                            <asp:Label CssClass="form-control" data-toggle="tooltip" ID="lbl_SponsorDepartment" runat="server"></asp:Label>
                                        </div>

                                    </div>

                                </div>


                                <div class="row">

                                    <div class="col-md-12">

                                        <div class="col-md-4">

                                            <label>
                                                Project Manager
                                            </label>
                                            <span class="markRequired">*</span>
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-user-o" aria-hidden="true"></i></span>
                                                <asp:Label CssClass="form-control" data-toggle="tooltip" ID="lbl_SponsorProjectManagerName" runat="server"></asp:Label>
                                                <%--<div id="div_SponsorProjectManager" data-toggle="tooltip" label="Sponsor Project Manager" class="form-control divUserInputField userfieldrequired"></div>--%>
                                            </div>
                                        </div>




                                        <div class="col-md-4">

                                            <label>
                                                Project Group Manager
                                            </label>
                                            <span class="markRequired">*</span>
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-user-o" aria-hidden="true"></i></span>
                                                <asp:Label CssClass="form-control" data-toggle="tooltip" ID="lbl_GroupManagerName" runat="server"></asp:Label>

                                            </div>
                                        </div>

                                        <div class="col-md-4">

                                            <label>
                                                Department Chief /Director
                                            </label>
                                            <span class="markRequired">*</span>
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-user-o" aria-hidden="true"></i></span>
                                                <asp:Label CssClass="form-control" data-toggle="tooltip" ID="lbl_DepartmentChiefName" runat="server"></asp:Label>
                                            </div>
                                        </div>


                                    </div>


                                </div>


                                <div class="row">



                                    <div class="col-md-12">



                                        <div class="col-md-4" style="z-index: 1">

                                            <label>
                                                Executive Office Sponsor
                                            </label>
                                            <span class="markRequired">*</span>
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-user-o" aria-hidden="true"></i></span>
                                                <asp:Label CssClass="form-control" data-toggle="tooltip" ID="lbl_DepartmentAGMName" runat="server"></asp:Label>

                                            </div>
                                        </div>

                                        <div class="col-md-4" style="z-index: 1">

                                            <label>
                                                OCR Analyst (Assigned by OCR Manager Group)
                                            </label>
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-user-o" aria-hidden="true"></i></span>
                                                <asp:Label CssClass="form-control" data-toggle="tooltip" ID="lbl_OCRAnalystName" runat="server"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-md-4" style="padding-right: 7px; z-index: 1;">
                                            <label class="lblforField">
                                                Kickoff Meeting Date
                                            </label>
                                            <asp:Label ID="txt_KickoffMeetingDate" data-toggle="tooltip" CssClass="form-control datepickertxt" runat="server"></asp:Label>
                                        </div>

                                    </div>

                                </div>




                                <div class="row">

                                    <div class="col-md-12">

                                        <div class="col-md-4">
                                            <label class="lblforField">Contracting Plan Status</label>
                                            <asp:Label ID="lbl_Status" runat="server" CssClass="sswpStatusValue" Text="Draft"></asp:Label>
                                        </div>

                                        <div class="col-md-4">
                                            <label class="lblforField">Date Submitted</label>
                                            <asp:Label class="sswpStatusValue" ID="lbl_DateSubmitted" runat="server" Text="N/A"></asp:Label>
                                        </div>

                                        <div class="col-md-4">
                                            <label class="lblforField">Initiated By</label>
                                            <asp:Label class="sswpStatusValue" ID="lbl_RequesterName" runat="server" Text=""></asp:Label>
                                        </div>

                                    </div>

                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="col-md-12">
                                            <label><b>Project Justification</b> (Outlines the need for the project, problem(s) addressed and risks being mitigated)</label>
                                            <span class="markRequired">*</span>
                                            <asp:Label runat="server" Enabled="false" TextMode="MultiLine" CssClass="form-control txtMultipleLines cssRequired txt_ProjectJustification" ID="lbl_ProjectJustification"></asp:Label>
                                        </div>
                                        <div class="col-md-12">
                                            <label><b>Project Scope Of Work</b></label>
                                            <span class="markRequired">*</span>
                                            <asp:Label runat="server" Enabled="false" TextMode="MultiLine" CssClass="form-control txtMultipleLines cssRequired txt_ProjectScopeOfWork" ID="lbl_ProjectScopeOfWork"></asp:Label>
                                        </div>
                                    </div>
                                </div>




                                <div class="row">

                                    <div class="col-md-12">
                                        <h5 style="margin-left: 2px;" class="formTextSmallSectionTitle">Unbundling Evaluation</h5>
                                        <p style="font-style: initial; font-weight: bold; font-style: italic;">
                                            The Project filled out the information (in italics below) required by the General Manager’s memo dated November 2, 2012.
                                BART Staff shall consider all of the following prior to determination of issuance of a solicitation:
                                        </p>
                                        <div class="col-md-12">
                                            <h6><u><b>By Dollar Amount</b></u></h6>
                                            <div class="checklistone" label="Can the contract be separated into two or more contacts based on the dollar value?">
                                                Can the contract be separated into two or more contacts based on the dollar value?
                                        <asp:CheckBox ID="cb_UnbundlingByDollarAmount_Yes" CssClass="cb_UnbundlingByDollarAmount_Yes" runat="server" Text="Yes" />
                                                &nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="cb_UnbundlingByDollarAmount_No" CssClass="cb_UnbundlingByDollarAmount_No" runat="server" Text="No" />
                                            </div>
                                            <asp:Label runat="server" Enabled="false" placeholder="Analysis" TextMode="MultiLine" CssClass="form-control txtMultipleLines txt_ByDollarAmountAnalysis" ID="lbl_ByDollarAmountAnalysis"></asp:Label>

                                        </div>
                                        <div class="col-md-12">
                                            <h6><u><b>By scope of work</b></u></h6>
                                            <ul>
                                                <li class="checklistone" label="Can the contract be separated into multiple scopes of work?">Can the contract be separated into multiple scopes of work?
                                        <asp:CheckBox ID="cb_UnbundlingByMultipleScopesOfWork_Yes" CssClass="cb_UnbundlingByMultipleScopesOfWork_Yes" runat="server" Text="Yes" />
                                                    &nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="cb_UnbundlingByMultipleScopesOfWork_No" CssClass="cb_UnbundlingByMultipleScopesOfWork_No" runat="server" Text="No" />
                                                </li>
                                                <li class="checklistone" label="Are there specific technical requirements in the scope of work where the contract can be separated?">Are there specific technical requirements in the scope of work where the contract can be separated?
                                        <asp:CheckBox ID="cb_UnbundlingByContractSeparated_Yes" CssClass="cb_UnbundlingByContractSeparated_Yes" runat="server" Text="Yes" />
                                                    &nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="cb_UnbundlingByContractSeparated_No" CssClass="cb_UnbundlingByContractSeparated_No" runat="server" Text="No" />
                                                </li>
                                                <li class="checklistone" label="Can the project/contract be separated into one or more smaller projects/contracts?">Can the project/contract be separated into one or more smaller projects/contracts?
                                        <asp:CheckBox ID="cb_UnbundlingBySmallerProjects_Yes" CssClass="cb_UnbundlingBySmallerProjects_Yes" runat="server" Text="Yes" />
                                                    &nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="cb_UnbundlingBySmallerProjects_No" CssClass="cb_UnbundlingBySmallerProjects_No" runat="server" Text="No" />
                                                </li>
                                            </ul>
                                            <asp:Label runat="server" Enabled="false" placeholder="Analysis" TextMode="MultiLine" CssClass="form-control txtMultipleLines txt_ByScopeOfWorkAnalysis" ID="lbl_ByScopeOfWorkAnalysis"></asp:Label>


                                            <h6><u><b>By schedule</b></u></h6>
                                            <ul>
                                                <li class="checklistone" label="Can the project be separated into smaller phases?">Can the project be separated into smaller phases?
                                            <asp:CheckBox ID="cb_UnbundlingBySchedule_Yes" CssClass="cb_UnbundlingBySchedule_Yes" runat="server" Text="Yes" />
                                                    &nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="cb_UnbundlingBySchedule_No" CssClass="cb_UnbundlingBySchedule_No" runat="server" Text="No" />
                                                </li>
                                            </ul>
                                            <asp:Label runat="server" label="Analysis" placeholder="Analysis" TextMode="MultiLine" CssClass="form-control txtMultipleLines txt_ByScheduleAnalysis" ID="lbl_ByScheduleAnalysis"></asp:Label>



                                            <h6><u><b>By geographical location</b></u></h6>
                                            <ul>
                                                <li class="checklistone" label="Are there any opportunities to separate the contract into geographic areas?">Are there any opportunities to separate the contract into geographic areas?
                                            <asp:CheckBox ID="cb_UnbundlingByLocation_Yes" CssClass="cb_UnbundlingByLocation_Yes" runat="server" Text="Yes" />
                                                    &nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="cb_UnbundlingByLocation_No" CssClass="cb_UnbundlingByLocation_No" runat="server" Text="No" />
                                                </li>
                                            </ul>
                                            <asp:Label runat="server" Enabled="false" placeholder="Analysis" TextMode="MultiLine" CssClass="form-control txtMultipleLines txt_ByLocationAnalysis" ID="lbl_ByLocationAnalysis"></asp:Label>




                                            <h6><u><b>By BART SEIU maintenance forces</b></u></h6>
                                            <ul>
                                                <li class="checklistone" label="Can the BART SEIU Maintenance Force be used to accomplish unbundling or perform one of the unbundled segments?">Can the BART SEIU Maintenance Force be used to accomplish unbundling or perform one of the unbundled segments?
                                            <asp:CheckBox ID="cb_UnbundlingByBARTSEIU_Yes" CssClass="cb_UnbundlingByBARTSEIU_Yes" runat="server" Text="Yes" />
                                                    &nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="cb_UnbundlingByBARTSEIU_No" CssClass="cb_UnbundlingByBARTSEIU_No" runat="server" Text="No" />
                                                </li>
                                            </ul>
                                            <asp:Label runat="server" Enabled="false" TextMode="MultiLine" CssClass="form-control txtMultipleLines txt_ByBARTSEIUAnalysis" ID="lbl_ByBARTSEIUAnalysis"></asp:Label>

                                        </div>



                                        <div class="col-md-12 attachmentSectionDev" style="padding-left: 15px;">
                                            <h6 style="margin-left: 2px; font-weight: bold; color: black;">Contracting Plan Related Attachment(s) <span style="font-size: smaller; font-style: italic; font-weight: 500; color: black;">(Please attach any related documents other than worlkplan / flowchart here)</span> </h6>
                                            <asp:Label ID="lblUploadedDocs" runat="server" Text=""></asp:Label>
                                        </div>


                                        <asp:Panel ID="pnl_OCRGeneralInfo" CssClass="row" runat="server">

                                            <div class="col-md-12">
                                                <hr style="margin-left: 15px;" />

                                                <div class="col-md-6">
                                                    <h6><b class="lblforField ocrlbl">OCR CCU Analysis (For OCR Only)</b></h6>
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-user-o" aria-hidden="true"></i></span>
                                                        <asp:Label CssClass="form-control" data-toggle="tooltip" ID="lbl_OCRCCUAnalysisName" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <p>&nbsp;</p>
                                                    <%-- <asp:CheckBox ID="cb_OCROver10M_Yes" Enabled="false" ForeColor="#0066cc" Text="Construction contract over $5M ?" runat="server" CssClass="cb_OCROver10M_Yes" />--%>
                                                    <asp:CheckBox ID="cb_OCROver10M_Yes" ForeColor="#0066cc" Text="Select if any Public Works Contracts or any Contract impacted by prevailing wage are Included" Enabled="false" runat="server" CssClass="cb_OCROver10M_Yes" />
                                                </div>

                                                <div class="col-md-12" style="margin-top: 5px;">
                                                    <asp:Label runat="server" Enabled="false" TextMode="MultiLine" CssClass="form-control txtMultipleLines txt_OCRCCUAnalysisSummary OCRAnalysis" ID="lbl_OCRCCUAnalysisSummary"></asp:Label>
                                                </div>

                                            </div>

                                            <div class="col-md-12 div_OCRLCUAnalysis" style="margin-top: 10px;">

                                                <div class="col-md-6">
                                                    <h6><b class="lblforField ocrlbl">OCR LCU Analysis (For OCR Only)</b></h6>
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-user-o" aria-hidden="true"></i></span>
                                                        <asp:Label CssClass="form-control" data-toggle="tooltip" ID="lbl_OCRLCUAnalysisName" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                </div>

                                                <div class="col-md-12" style="margin-top: 5px;">
                                                    <asp:Label runat="server" Enabled="false" TextMode="MultiLine" CssClass="form-control txtMultipleLines txt_OCRLCUAnalysisSummary OCRAnalysis" ID="lbl_OCRLCUAnalysisSummary"></asp:Label>
                                                </div>


                                            </div>


                                        </asp:Panel>

                                    </div>

                                </div>


                            </div>



                        </asp:Panel>

                    </div>
                </div>
            </div>

            <%-- End Step 1 --%>


            <%-- Step 2 --%>
            <div class="tab-pane fade" id="step2">
                <div class="row top-container">
                    <div class="top-inner-container">

                        <asp:Panel ID="pnlStep2" CssClass="col-md-12" runat="server">





                            <div class="row">
                                <div class="col-md-12">

                                    <div style="margin-top: 10px;">
                                        <span class="lbllarge">Number of Contracts: </span>
                                        <asp:Label CssClass="lbllarge lblNoofContract" ID="lblNoofContract" runat="server" Text="N/A"></asp:Label>
                                    </div>

                                    <asp:Repeater ID="rpt_Contracts" runat="server" OnItemDataBound="rpt_Contracts_ItemDataBound">
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                        <ItemTemplate>


                                            <div class="col-md-12 contractform" style="margin-bottom: 10px;">

                                                <div class="row">

                                                    <div class="row gridheader">
                                                        <h5 style="margin-left: 15px;" class="formTextSmallSectionTitleContract">CONTRACT <%# Container.ItemIndex + 1 %></h5>

                                                    </div>

                                                    <div class="col-md-4">

                                                        <label class="lblforField">
                                                            Funding Source
                                                        </label>
                                                        <span class="markRequired">*</span>
                                                        <asp:Label CssClass="form-control" Text='<%# Eval("FundingSource")%>' runat="server"></asp:Label>

                                                    </div>
                                                    <div class="col-md-4">
                                                        <label class="lblforField">
                                                            Target Completion Date
                                                        </label>
                                                        <asp:Label ID="txt_TargetCompletionDate" data-toggle="tooltip" Text='<%# BART.SP.OCR.CP.Common.ProjectUtilities.DisplayDateTimeMMDDYYYY(Eval("TargetCompletionDate"))%>' CssClass="form-control datepickertxt" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <label class="lblforField">
                                                            Duration
                                                        </label>
                                                        <asp:Label CssClass="form-control" Text='<%# Eval("Duration")%>' data-toggle="tooltip" ID="lbl_Duration" placeholder="Duration" runat="server"></asp:Label>
                                                    </div>

                                                </div>

                                                <div class="row" style="margin-top: 10px; margin-bottom: 5px;">

                                                    <div class="col-md-4">
                                                        <label class="lblforField">
                                                            Contract No
                                                        </label>
                                                        <span class="markRequired">*</span>
                                                        <asp:Label CssClass="form-control cssRequired" Text='<%# Eval("ContractNo")%>' data-toggle="tooltip" ID="lbl_ContractNo" placeholder="Contract No" runat="server"></asp:Label>
                                                    </div>

                                                    <div class="col-md-4">
                                                        <label class="lblforField">
                                                            Dollar Amount
                                                        </label><span class="markRequired">*</span>
                                                        <asp:Label CssClass="form-control" label="Dollar Amount" data-toggle="tooltip" Text='<%# Eval("DollarAmount")%>' ID="lbl_DollarAmount" placeholder="$" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <label class="lblforField">
                                                            Contract Status
                                                        </label>

                                                        <asp:Label CssClass="form-control" data-toggle="tooltip" Text='<%# Eval("Status")%>' ID="Label1" runat="server"></asp:Label>
                                                    </div>

                                                </div>
                                                <div class="row" style="margin-top: 10px; margin-bottom: 5px;">

                                                    <div class="col-md-8">
                                                        <label class="lblforField">
                                                            Description
                                                        </label>
                                                        <span class="markRequired">*</span>
                                                        <asp:Label runat="server" Enabled="false" Text='<%# Eval("Description")%>' label="Contract Description" placeholder="Contract Description" TextMode="MultiLine" CssClass="form-control txtMultipleLines txt_Description" ID="txt_Description"></asp:Label>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <label class="lblforField ocrlbl">OCR analysis for this contract (For OCR Only)</label>
                                                        <asp:Label runat="server" Text='<%# Eval("OCRAnalysis")%>' label="OCR analysis for this contract" TextMode="MultiLine" CssClass="form-control txtMultipleLines txt_OCRAnalysis OCRAnalysis" Enabled="false" ID="txt_OCRAnalysis"></asp:Label>
                                                    </div>
                                                </div>

                                                <asp:Label ID="lblContractAttachmentUploaded" runat="server" Text='<%# Eval("DisplayFiles")%>'></asp:Label>
                                                <asp:TextBox ID="txt_OrderInTable" data-toggle="tooltip" Text='<%# Eval("OrderInTable")%>' CssClass="form-control ctrlHidden" runat="server"></asp:TextBox>
                                                <asp:TextBox ID="txt_ItemID" data-toggle="tooltip" Text='<%# Eval("ItemID")%>' CssClass="form-control ctrlHidden" runat="server"></asp:TextBox>
                                                <asp:TextBox ID="hiddenStatus" data-toggle="tooltip" Text='<%# Eval("Status")%>' CssClass="form-control ctrlHidden" runat="server"></asp:TextBox>
                                                <asp:TextBox ID="hiddenFundingSource" data-toggle="tooltip" Text='<%# Eval("FundingSource")%>' CssClass="form-control ctrlHidden" runat="server"></asp:TextBox>
                                                <asp:TextBox ID="txtVisible" Visible='<%# Eval("Visible")%>' CssClass="ctrlHidden" runat="server"></asp:TextBox>
                                            </div>

                                        </ItemTemplate>
                                    </asp:Repeater>

                                </div>

                            </div>

                            <asp:HiddenField ID="hdfNoOfContracts" Value="0" runat="server" />
                        </asp:Panel>
                    </div>
                </div>
            </div>

            <%-- END Step 2 --%>







            <%-- Step 3 --%>
            <div class="tab-pane fade" id="step3">
                <div class="row top-container">
                    <div class="top-inner-container">









                        <asp:Panel ID="pnlStep3" CssClass="col-md-12 pnlStep3" runat="server">


                            <h5 style="margin-left: 2px;" class="formTextSmallSectionTitle">APPROVAL TASKS</h5>

                            <div class="row">



                                <ul class="nav nav-wizard row" id="approvalTabs" role="tablist">
                                    <li role="presentation" class="active"><a href="#listep1" id="listep1-tab" role="tab" data-toggle="tab" aria-controls="listep1tab" aria-expanded="false">Staff Approval</a></li>
                                    <li role="presentation"><a href="#listep2" id="listep2-tab" role="tab" data-toggle="tab" aria-controls="listep2tab" aria-expanded="false">Management Approval</a></li>
                                    <li role="presentation"><a href="#listep3" id="listep3-tab" role="tab" data-toggle="tab" aria-controls="listep3tab" aria-expanded="false">Executive Approval</a></li>
                                </ul>


                                <div class="tab-content">

                                    <div class="tab-pane fade in active" id="listep1">

                                        <div class="col-md-12 div_task">

                                            <div class="taskList gridview" style="border-bottom: 1px dashed lightgray;">

                                                <asp:Repeater ID="rpt_Tasks_Staff" runat="server">



                                                    <HeaderTemplate>
                                                        <div class="row gridheader">

                                                            <div class="col-md-4 task_title" style="width: 22%">
                                                                Task
                                                            </div>
                                                            <div class="col-md-2 task_status" style="width: 14%">
                                                                Status
                                                            </div>
                                                            <div class="col-md-2 task_date" style="width: 16%">
                                                                Mofified
                                                            </div>
                                                            <div class="col-md-4 task_assigned text-left" style="width: 24%">
                                                                Assigned To
                                                            </div>
                                                            <div class="col-md-4 task_assigned text-left" style="width: 24%">
                                                                Completed By
                                                            </div>
                                                        </div>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>

                                                        <div class="row gridbody">
                                                            <div class="col-md-4 item" style="width: 22%">
                                                                <asp:Label ID="grid_lbl_Title" runat="server" Text='<%# Eval("Title")%>'></asp:Label>
                                                            </div>
                                                            <div class="col-md-2 item text-center" style="width: 14%">
                                                                <asp:Label ID="grid_lbl_Status" runat="server" Text='<%# Eval("TaskStatus")%>'></asp:Label>
                                                            </div>
                                                            <div class="col-md-2 item text-center" style="width: 16%">
                                                                <asp:Label ID="grid_lbl_ApprovedDate" runat="server" Text='<%# Eval("ApprovedDate")%>'></asp:Label>
                                                            </div>
                                                            <div class="col-md-4 item" style="width: 24%">
                                                                <asp:Label ID="grid_lbl_AssignedToName" runat="server" Text='<%# Eval("AssignedToName")%>'></asp:Label>
                                                                <asp:TextBox ID="grid_txtOrderIntbl" data-toggle="tooltip" Text='<%# Eval("ApprovalOrder")%>' CssClass="form-control requiredInline ctrlHidden" runat="server"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-4 item" style="width: 24%">
                                                                <asp:Label ID="grid_lblApprovedBy" runat="server" Text='<%# Eval("ApprovedByName")%>'></asp:Label>
                                                            </div>

                                                        </div>
                                                    </ItemTemplate>




                                                </asp:Repeater>

                                            </div>


                                        </div>



                                    </div>

                                    <div class="tab-pane fade in" id="listep2">



                                        <div class="col-md-12 div_task">

                                            <div class="taskList gridview" style="border-bottom: 1px dashed lightgray;">














                                                <asp:Repeater ID="rpt_Tasks_Management" runat="server">






                                                    <HeaderTemplate>
                                                        <div class="row gridheader">

                                                            <div class="col-md-4 task_title" style="width: 22%">
                                                                Task
                                                            </div>
                                                            <div class="col-md-2 task_status" style="width: 14%">
                                                                Status
                                                            </div>
                                                            <div class="col-md-2 task_date" style="width: 16%">
                                                                Mofified
                                                            </div>
                                                            <div class="col-md-4 task_assigned text-left" style="width: 24%">
                                                                Assigned To
                                                            </div>
                                                            <div class="col-md-4 task_assigned text-left" style="width: 24%">
                                                                Completed By
                                                            </div>
                                                        </div>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>

                                                        <div class="row gridbody">
                                                            <div class="col-md-4 item" style="width: 22%">
                                                                <asp:Label ID="grid_lbl_Title" runat="server" Text='<%# Eval("Title")%>'></asp:Label>
                                                            </div>
                                                            <div class="col-md-2 item text-center" style="width: 14%">
                                                                <asp:Label ID="grid_lbl_Status" runat="server" Text='<%# Eval("TaskStatus")%>'></asp:Label>
                                                            </div>
                                                            <div class="col-md-2 item text-center" style="width: 16%">
                                                                <asp:Label ID="grid_lbl_ApprovedDate" runat="server" Text='<%# Eval("ApprovedDate")%>'></asp:Label>
                                                            </div>
                                                            <div class="col-md-4 item" style="width: 24%">
                                                                <asp:Label ID="grid_lbl_AssignedToName" runat="server" Text='<%# Eval("AssignedToName")%>'></asp:Label>
                                                                <asp:TextBox ID="grid_txtOrderIntbl" data-toggle="tooltip" Text='<%# Eval("ApprovalOrder")%>' CssClass="form-control requiredInline ctrlHidden" runat="server"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-4 item" style="width: 24%">
                                                                <asp:Label ID="grid_lblApprovedBy" runat="server" Text='<%# Eval("ApprovedByName")%>'></asp:Label>
                                                            </div>

                                                        </div>
                                                    </ItemTemplate>





                                                </asp:Repeater>

                                            </div>


                                        </div>


                                    </div>


                                    <div class="tab-pane fade in" id="listep3">



                                        <div class="col-md-12 div_task">

                                            <div class="taskList gridview" style="border-bottom: 1px dashed lightgray;">

                                                <asp:Repeater ID="rpt_Tasks_Executive" runat="server">




                                                    <HeaderTemplate>
                                                        <div class="row gridheader">

                                                            <div class="col-md-4 task_title" style="width: 22%">
                                                                Task
                                                            </div>
                                                            <div class="col-md-2 task_status" style="width: 14%">
                                                                Status
                                                            </div>
                                                            <div class="col-md-2 task_date" style="width: 16%">
                                                                Mofified
                                                            </div>
                                                            <div class="col-md-4 task_assigned text-left" style="width: 24%">
                                                                Assigned To
                                                            </div>
                                                            <div class="col-md-4 task_assigned text-left" style="width: 24%">
                                                                Completed By
                                                            </div>
                                                        </div>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>

                                                        <div class="row gridbody">
                                                            <div class="col-md-4 item" style="width: 22%">
                                                                <asp:Label ID="grid_lbl_Title" runat="server" Text='<%# Eval("Title")%>'></asp:Label>
                                                            </div>
                                                            <div class="col-md-2 item text-center" style="width: 14%">
                                                                <asp:Label ID="grid_lbl_Status" runat="server" Text='<%# Eval("TaskStatus")%>'></asp:Label>
                                                            </div>
                                                            <div class="col-md-2 item text-center" style="width: 16%">
                                                                <asp:Label ID="grid_lbl_ApprovedDate" runat="server" Text='<%# Eval("ApprovedDate")%>'></asp:Label>
                                                            </div>
                                                            <div class="col-md-4 item" style="width: 24%">
                                                                <asp:Label ID="grid_lbl_AssignedToName" runat="server" Text='<%# Eval("AssignedToName")%>'></asp:Label>
                                                                <asp:TextBox ID="grid_txtOrderIntbl" data-toggle="tooltip" Text='<%# Eval("ApprovalOrder")%>' CssClass="form-control requiredInline ctrlHidden" runat="server"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-4 item" style="width: 24%">
                                                                <asp:Label ID="grid_lblApprovedBy" runat="server" Text='<%# Eval("ApprovedByName")%>'></asp:Label>
                                                            </div>

                                                        </div>
                                                    </ItemTemplate>




                                                </asp:Repeater>

                                            </div>


                                        </div>


                                    </div>




                                </div>

                                <div class="col-md-12" style="text-align: right; padding-right: 30px;">
                                </div>

                            </div>



                        </asp:Panel>


                    </div>
                </div>
            </div>


            <%-- END Step 3 --%>


            <div class="tab-pane fade" id="step4">

                <div class="row divcontainerComments ">
                    <asp:Label ID="lblComments" runat="server" Text="(There is no comment)"></asp:Label>
                </div>

            </div>


            <%-- Step 5 --%>
            <div class="tab-pane fade" id="step5">
                <div class="row divcontainerHistory">
                    <h4 style="margin-top: 0px; margin-bottom: 2px; text-transform: uppercase; border-bottom: 1px dashed lightblue; color: #c35939;">Previous Version(s)</h4>
                    <asp:Label ID="lblPreviousVer" runat="server" Text="(There is no previous version)"></asp:Label>


                    <h4 style="text-transform: uppercase; margin-top: 0px; text-transform: uppercase; border-bottom: 1px dashed lightblue; color: #c35939; margin-bottom: 2px; margin-top: 15px;">Activity Logs</h4>
                    <asp:Label ID="lblHistory" runat="server" Text="(There is no activity log)"></asp:Label>
                </div>
            </div>
            <%-- END Step 5 --%>
        </div>


    </div>

    <asp:Panel runat="server" ID="pnlErrorMsgBottom" Visible="false" CssClass="alert alert-danger alert-dismissable pnlErrorMsgCss errorpnlbottom">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">×</a>
        <asp:Label ID="lblErroBottom" runat="server"></asp:Label>
    </asp:Panel>

    <div class="row sswpsecondnavbottom">
        <div class="col-md-6 sub-nav__buttons" style="text-align: left">
            <%--<a id="printbottom" data-toggle="tooltip" class="badge printer" style="display:none;" href="#" title="Print SSWP"><i class="fa fa-print fa-2" aria-hidden="true"></i></a>--%>
            <%--<span data-toggle="tooltip" title="Save Changes & Preview SSWP"><asp:LinkButton ID="previewbottom" runat="server" OnClientClick="return SavePreviewSSWP();"  CssClass="badge floppy btnAction btnSavePreviewAction "><i class="fa fa-eye fa-2"></i></asp:LinkButton></span>--%>
            <%--<span data-toggle="tooltip" title="Save Changes and route to approvers"><asp:LinkButton ID="lbtRouteBottom" OnClientClick="return RouteSSWP();" runat="server" CssClass="badge floppy btnSaveAction"><i class="fa fa-random fa-rotate-270" style="color:#337ab7"></i></asp:LinkButton></span>--%>
            <span data-toggle="tooltip" title="Cancel changes and return to home page."><a href="#" class="badge printer lbtCancel btnAction" onclick="CancelChanges();"><i class="fa fa-times fa-2" aria-hidden="true"></i></a></span>
            <a id="printbottom" data-toggle="tooltip" class="badge printer printsswplink" href="#" target="_blank" title="Print Contracting Plan"><i class="fa fa-print" aria-hidden="true"></i></a>
            <a id="Editbottom" data-toggle="tooltip" onserverclick="Editbottom_ServerClick" class="badge printer" runat="server" href="EditMDD.aspx" title="Click here to edit this CP"><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>
            <%--          <span data-toggle="tooltip" title="Save Changes"><asp:LinkButton ID="lbtSaveBottom" runat="server" OnClientClick="return SaveObject('Draft');" CssClass="badge floppy btnSaveAction"><i class="fa fa-floppy-o"></i></asp:LinkButton></span>--%>
            <%--<span data-toggle="tooltip" title="Copy this report(This feature allows PM to copy all data in this report to create a new report quickly.)"><asp:LinkButton ID="lbtCopy" OnClientClick="return CopyReport();" runat="server" CssClass="badge floppy btnAction btnCopy"><i style="color:#009688" class="fa fa-files-o"></i></asp:LinkButton></span>--%>
            <%--          <a data-toggle="tooltip" title="Route Contracting Plan for Approval" runat="server" id="btnSubmitForApproval" onclick="return SavenRoute();" class="btn btn-primary btnAction btnMoveStep submit4Approval">Submit for Approval</a>--%>
            <%--          <a data-toggle="tooltip" title="Save changes and re-route for department's manager approval" runat="server" id="btnReRouteForApproval" onclick="return SavenRe_Route();" class="btn btn-primary btnAction btnMoveStep submit4Approval">Re-Route</a>--%>
        </div>
        <div class="col-md-6" style="text-align: right">

            <asp:Panel ID="pnlButton" Visible="false" CssClass="btngroupactions" runat="server">
                <div style="padding-top: 10px;">
                    <a href="#top" class="btnnext hidelow" linkval="step1-tab" id="backbtn" role="tab" data-toggle="tab" aria-expanded="false"><i class="fa fa-arrow-left" aria-hidden="true"></i>Back </a>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                <a href="#top" class="btnnext" linkval="step2-tab" id="nextbtn" role="tab" data-toggle="tab" aria-expanded="false">Next <i class="fa fa-arrow-right" aria-hidden="true"></i></a>

                </div>


                <%-- <a data-toggle="tooltip" title="Save RR Report as Draft" style="margin-right:15px" runat="server" id="btnSaveAsDraft" onclick="return SaveInfo(1);" class="btn btn-primary btnAction btnMoveStep saveasdraft">Save</a>--%>
            </asp:Panel>

        </div>

    </div>

</div>

<%-- All hidden fields --%>

<asp:HiddenField ID="hdf_UserCreatedLogin" runat="server" />
<asp:HiddenField ID="hdf_PendingAtLogin" runat="server" />
<asp:HiddenField ID="hdf_UserModifiedLogin" runat="server" />
<asp:HiddenField ID="hdf_SponsorProjectManagerLogin" runat="server" />
<asp:HiddenField ID="hdf_OCRAnalystLogin" runat="server" />

<asp:HiddenField ID="hdf_DepartmentChiefLogin" runat="server" />
<asp:HiddenField ID="hdf_DepartmentAGMLogin" runat="server" />
<asp:HiddenField ID="hdf_GroupManagerLogin" runat="server" />
<asp:HiddenField ID="hdf_RequesterLogin" runat="server" />
<asp:HiddenField ID="hdf_OCRCCUAnalysisLogin" runat="server" />
<asp:HiddenField ID="hdf_OCRLCUAnalysisLogin" runat="server" />
<asp:HiddenField ID="hdf_MainObjID" runat="server" />
<asp:HiddenField ID="hdfDeletedDocMarkIDs" runat="server" />
<asp:HiddenField ID="hdfInitialStatusValue" runat="server" />
<asp:HiddenField ID="hdfCurrentTaskID" runat="server" />
<asp:HiddenField ID="hdfCurrentLogin" runat="server" />
<asp:TextBox ID="txt_CurrentStep" CssClass="ctrlHidden txt_CurrentStep" runat="server"></asp:TextBox>

<asp:TextBox runat="server" ID="hdfCurrentTab" CssClass="hdfCurrentTab ctrlHidden"></asp:TextBox>

<asp:TextBox runat="server" ID="txt_hdfdeletedFiles" CssClass="form-control ctrlHidden txthdfdeletedFiles"></asp:TextBox>
<asp:TextBox ID="txtCommitAction" CssClass="ctrlHidden txtCommitAction" runat="server"></asp:TextBox>
<%-- END hidden fields --%>










<div class="modal fade in" id="myApprovalForm" role="dialog" aria-hidden="false">
    <div class="modal-dialog boxConfirm">
        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" style="display: none">X</button>
                <h4 class="modal-title" id="titleApprovalForm">APPROVAL FORM</h4>
                <span class="approval_act_type ctrlHidden">0</span>
            </div>
            <div class="modal-body">
                <asp:Panel ID="pndApprovalProxy" runat="server" CssClass="row approvalDivApprover firstLevelApprovalrow">
                    <div class="col-md-12">
                        <div style="float: left; padding-top: 8px;">
                            <span class="approvalFormLable lablechooseproxy">I am submitting<span class="markRequired">*</span></span>
                        </div>
                        <div class="col-md-5">
                            <asp:DropDownList ID="ddlSubmitPermission" CssClass="form-control ApproveAs approvalformrequireInput" runat="server">
                            </asp:DropDownList>
                        </div>

                    </div>
                </asp:Panel>

                <asp:Panel CssClass="row pnlApprovalOCRAnalyst" ID="pnlApprovalOCRAnalyst" Visible="false" runat="server">
                    <div class="col-md-12">

                        <label class="approvalFormLable">
                            Enter OCR Analyst for this Contracting Plan here<span class="markRequired">*</span>
                        </label>
                        <hr style="margin-top: 1px; margin-bottom: 10px;" />
                    </div>
                    <div class="col-md-7">
                        <div class="input-group">
                            <span class="input-group-addon"><i class="fa fa-user-o" aria-hidden="true"></i></span>
                            <div id="div_OCRAnalyst" data-toggle="tooltip" label="OCR Analyst" class="form-control divUserInputField userfieldrequired"></div>
                        </div>

                    </div>



                </asp:Panel>

                <asp:Panel CssClass="row approvalDivDecisionOptions" ID="pnlApprovalDecision" runat="server">
                    <div class="col-md-12 firstLevelApprovalrow">
                        <span class="approvalFormLable">Your Decision</span><span class="markRequired">*</span>
                        <div class="col-md-12 secondLevelApprovalrow radiolistApprovalDecision">
                            <asp:RadioButton GroupName="approvalcheck" ID="RadioConcur" CssClass="radioDecision rDecisionConcur" Text="Concur" runat="server" /><br />
                            <asp:RadioButton GroupName="approvalcheck" ID="RadioWriteAComment" CssClass="radioDecision rDecisionWriteAComment" Text="Comment" runat="server" /><br />
                            <asp:RadioButton GroupName="approvalcheck" ID="RadioReject" CssClass="radioDecision rDecisionReject" runat="server" Text="Reject" />
                        </div>
                    </div>
                </asp:Panel>


                <asp:Panel ID="pnlApprovalComments" runat="server">

                    <div class="row">
                        <div class="col-md-12 firstLevelApprovalrow">
                            <span class="approvalFormLable">Your Comment </span><span class="optionalfieldcss optionalcomment">(Optional)</span> <span class="markRequired requiredcomment">* (Please write a comment)</span>
                            <div class="col-md-12 secondLevelApprovalrow">
                                <asp:TextBox runat="server" ID="txtApprovalComments" TextMode="MultiLine" CssClass="form-control txtApprovalComments">

                                </asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12 firstLevelApprovalrow" style="margin-bottom: 5px;">
                            <div class="col-md-12 secondLevelApprovalrow" style="padding-top: 0px; border: 0px;">
                                <telerik:RadAsyncUpload RenderMode="Lightweight" AllowedFileExtensions="jpg,jpeg,png,gif,doc,docx,txt,xls,xlsx,ppt,pptx,pdf,mpp,mpt,xlsb,cvs,xer,prx" runat="server" ID="CtrlAttachment" MultipleFileSelection="Automatic" TemporaryFolder="D:\BartApps\tempfiles" Skin="Material" Localization-Select="ATTACH FILES" />
                            </div>
                        </div>
                    </div>

                </asp:Panel>

            </div>
            <div class="modal-footer">
                <button type="button" id="btnSubmitApproval" onclick="return validateApprovalFormOnView();" class="btn btn-primary commitAction btnCommitApprovalVote" title="Submit my decision">
                    Submit
                </button>
                <asp:Button ID="btnActualClickActionApproval" OnClientClick="updateApproversValue();" CssClass="btnActualClickActionApprovalCSS" OnClick="btnActualClickActionApproval_Click" runat="server" Text="HiddenApprove" />
                <button type="button" class="btn btn-primary btnDismissDlg btnNobox" data-dismiss="modal" title="Cancel and close dialog box">Cancel</button>
            </div>
        </div>
    </div>
</div>


<%-- Start Dialog Box --%>
<div class="modal fade in" id="myConfirmationModal" role="dialog" aria-hidden="false">
    <div class="modal-dialog boxConfirm">
        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" style="display: none">X</button>
                <h4 class="modal-title" id="titleMyConfirm">Important Message</h4>
            </div>
            <div class="modal-body">
                <p class="txtComfirmationMss"></p>
            </div>
            <div class="modal-footer">
                <button type="button" runat="server" id="btnRoute" onclick="showLoadingDiv();" data-dismiss="modal" onserverclick="btnRoute_ServerClick" class="btn btn-primary commitAction btnCommitRoute" title="Save Changes and route to approvers">
                    Yes
                </button>
                <a class="btn btn-primary commitAction cancelCommit" id="btnCancelReport" runat="server" onserverclick="btnCancelReport_ServerClick" title="Cancel changes and return home">Yes</a>
                <button type="button" class="btn btn-primary btnDismissDlg btnNobox" data-dismiss="modal" title="Cancel and close dialog box">No</button>
            </div>
        </div>
    </div>
</div>

<%-- End Dialog Box --%>



<div class="modalbackdropDiv">
    <div>
        <i class="fa fa-circle-o-notch fa-spin" style="float: left; color: black; color: #fff; font-size: 100px;"></i>
    </div>
</div>
