
<%@ Assembly Name="BART.SP.OCR.CP.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9abfeb7dc254e359" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateNewUserControl.ascx.cs" Inherits="BART.SP.OCR.CP.Web.CreateNew.CreateNewUserControl" %>
<%@ Register Assembly="Telerik.Web.UI, Version=2016.2.504.45, Culture=neutral, PublicKeyToken=121fae78165ba3d4" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<SharePoint:ScriptLink name="clienttemplates.js" runat="server" LoadAfterUI="true" Localizable="false" />
<SharePoint:ScriptLink name="clientforms.js" runat="server" LoadAfterUI="true" Localizable="false" />
<SharePoint:ScriptLink name="clientpeoplepicker.js" runat="server" LoadAfterUI="true" Localizable="false" />
<SharePoint:ScriptLink name="autofill.js" runat="server" LoadAfterUI="true" Localizable="false" />
<SharePoint:ScriptLink name="sp.js" runat="server" LoadAfterUI="true" Localizable="false" />
<SharePoint:ScriptLink name="sp.runtime.js" runat="server" LoadAfterUI="true" Localizable="false" />
<SharePoint:ScriptLink name="sp.core.js" runat="server" LoadAfterUI="true" Localizable="false" />


<script type="text/javascript" src="/_layouts/15/BART.SP.OCR.CP.Web/js/bootstrap-datepicker.min.js?refId=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>"></script>
<script type="text/javascript" src="/SiteAssets/AppCP/Core.js?refId=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>"></script>
<script type="text/javascript" src="/SiteAssets/AppCP/UI.js?versionview=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>"></script>
<script type="text/javascript" src="/SiteAssets/AppCP/Validation.js?versionview=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>"></script>
<script type="text/javascript" src="/SiteAssets/AppCP/LoadNEvents.js?versionview=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>"></script>

<link  rel="stylesheet" type="text/css" href="/_layouts/15/BART.SP.OCR.CP.Web/css/OriginalCSS.css?refId=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>" />
<link  rel="stylesheet" type="text/css" href="/_layouts/15/BART.SP.OCR.CP.Web/css/bootstrap-datepicker3.css?refId=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>" />
<link  rel="stylesheet" type="text/css" href="/SiteAssets/AppCP/Core.css?refId=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>" />

<span>
    <%=BART.SP.OCR.CP.Common.ProjectUtilities.BuildTopMenu(Request.Url.ToString().ToLower())%>
</span>

<style>
    .ms-dlgTitleBtns
    {
        margin-right:-5px !important;

    }
    .ms-dlgContent
    {
        border-radius:8px;
    }
</style>

<%-- Start of Step 1 Edit Script --%>
<script>
    $(document).ready(function () {
        try {
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
        displayCalendar();
        hideLoadingDiv();
        tabClick();
        LoadProjectFunction();
        FirstLoadProjectProgram();
        linknextClick();
        linkbackClick();
        if ($('.hdfCurrentTab').val())
            activaTab($('.hdfCurrentTab').val());
        LoadFloatFormat();
        LoadCalculationFormat();
        //showRequiredScheduleFields();
        $('[data-toggle="tooltip"]').tooltip();
        LoadDelCtr();
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
        
    function activaTab(tab) {
        try {

            $('#myTabs a[href="' + tab + '"]').tab('show');
            $('.btngroupactions a[linkvalue="' + tab + '"]').show();
            balanceNextBack(tab);

        } catch (e) { }
        
    };

    function tabClick()
    {
        $('#myTabs li a[role=tab]').click(function () {
            var hrefval = $(this).attr('href');
            $('.hdfCurrentTab').val(hrefval);
            balanceNextBack(hrefval);
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
        //$('.divUserNameAutoComplete').each(function (index) {
        //    InitaUserPickerSingleNameOnly($(this).attr('id'));
        //});
        var pLink = '<%=BART.SP.OCR.CP.Common.ProjectUtilities.PrintReportURL(this.hdf_MainObjID.Value.Trim())%>';
        $('.printsswplink').attr('href', pLink);
        
        //cancelEnterinSingleText();
        allLoadJSCreateNew();
        BindTexttoPicker('div_SponsorProjectManager', '<%= this.hdf_SponsorProjectManagerLogin.ClientID %>');
        BindTexttoPicker('div_OCRAnalyst', '<%= this.hdf_OCRAnalystLogin.ClientID %>');
        BindTexttoPicker('div_DepartmentChief', '<%= this.hdf_DepartmentChiefLogin.ClientID %>');
        BindTexttoPicker('div_DepartmentAGM', '<%= this.hdf_DepartmentAGMLogin.ClientID %>');
        BindTexttoPicker('div_GroupManager', '<%= this.hdf_GroupManagerLogin.ClientID %>');
        BindTexttoPicker('div_OCRCCUAnalysis', '<%= this.hdf_OCRCCUAnalysisLogin.ClientID %>');
        BindTexttoPicker('div_OCRLCUAnalysis', '<%= this.hdf_OCRLCUAnalysisLogin.ClientID %>');

    } 
    function updateApproversValue()
    {
        getUserInfo('div_SponsorProjectManager', '<%= this.hdf_SponsorProjectManagerLogin.ClientID %>');
        getUserInfo('div_OCRAnalyst', '<%= this.hdf_OCRAnalystLogin.ClientID %>');
        getUserInfo('div_DepartmentChief', '<%= this.hdf_DepartmentChiefLogin.ClientID %>');
        getUserInfo('div_DepartmentAGM', '<%= this.hdf_DepartmentAGMLogin.ClientID %>');
        getUserInfo('div_GroupManager', '<%= this.hdf_GroupManagerLogin.ClientID %>');
        getUserInfo('div_OCRCCUAnalysis', '<%= this.hdf_OCRCCUAnalysisLogin.ClientID %>');
        getUserInfo('div_OCRLCUAnalysis', '<%= this.hdf_OCRLCUAnalysisLogin.ClientID %>');


        $('.txtComfirmationMss').html(msgSaveandReturnHome);
        return false;
    }

    function CopyReport()
    {
        $('.commitAction').hide();
        $('.txtComfirmationMss').html('Copy this report to a new report ?');
        $('.commitAction.btnCommitCopy').show();
    }
    function RouteRR() {
        var rval = ValidateForm();
        if (rval == 0) {
            updateApproversValue();
            SaveInfo('2');
        }

    }
    function NextRRPage() {
        var rval = ValidateForm();
        if (rval == 0) {
            updateApproversValue();
            SaveInfo('0');
        }
        else {
            $('#myConfirmationModal').modal(); return false;
        }

    }
    
</script>
<%-- End of Step 1 Edit Script --%>


<div class="container">
<div class="row sswpsecondnav sub-nav" style="display:none !important">
        <div class="col-md-6 sswpbreadcrumb sub-nav__link ">
        <ol class="breadcrumb">
          <li class="breadcrumb-item"><a href="mycp.aspx">Home</a></li>
          <li class="breadcrumb-item active">Creating New CP</li>
          <li class="breadcrumb-item">Step 1</li>
        </ol>
        </div>
            <div class="col-md-6 sub-nav__buttons">
               <a id="print" data-toggle="tooltip" style="display:none;" class="badge printer" href="#" title="Print Contracting Plan"><i class="fa fa-print fa-2" aria-hidden="true"></i></a>
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

<div data-example-id="togglable-tabs" class="tabingparentDiv EditReport"> 

<asp:Label ID="lblErrorMessage" ForeColor="Red" runat="server" Text=""></asp:Label>
<a name="top"></a>

<ul class="nav nav-tabs" id="myTabs" role="tablist" style="margin-left:-12px;">
    <li role="presentation" class="active"><a href="#step1" id="step1-tab" role="tab" data-toggle="tab" aria-controls="step1tab" aria-expanded="false">1. General Info</a></li>
    <li role="presentation"><a href="#step2" id="step2-tab" role="tab" data-toggle="tab" aria-controls="step2tab" aria-expanded="false">2. Contracts</a></li>
    <li role="presentation"><a href="#step3" id="step3-tab" role="tab" data-toggle="tab" aria-controls="step3tab" aria-expanded="false">3. Approvers</a></li>
    <%--<li role="presentation"><a href="#step4" id="step4-tab" role="tab" data-toggle="tab" aria-controls="step4tab" aria-expanded="false">4. Approvers</a></li>--%>
    <li style="float:right">
      <div style="margin-top:-3px;">
        <span data-toggle="tooltip" title="Cancel changes and return to home page."><a href="#" class="badge printer lbtCancel btnAction" onclick="CancelChanges();" ><i class="fa fa-times fa-2"  aria-hidden="true"></i></a></span>
<%--        <a id="printTop" data-toggle="tooltip" class="badge printer printsswplink" href="#" target="_blank" title="Print Contracting Plan"><i class="fa fa-print" aria-hidden="true"></i></a>--%>
        <span data-toggle="tooltip" title="Save Changes"><asp:LinkButton ID="lbtSaveTop" runat="server" OnClientClick="return SaveObject('Draft');" CssClass="badge floppy btnSaveAction"><i class="fa fa-floppy-o"></i></asp:LinkButton></span>
    </div>
    </li>

</ul>
<div class="tab-content" id="ssswpTabContent">


<%-- Step 1 --%>

    <div class="tab-pane fade in active" id="step1">
    <div class="row top-container">
        <div class="top-inner-container">
        <asp:Panel ID="pnlStep1" CssClass="col-md-12" runat="server">


                <div class="form-group sswpFields" style="margin-bottom:0px;">

            <div class="row">
                    <div class="col-md-12">
                        <div class="col-md-4">
                            <label class="lblforField">
                                Service Type(s)
                            </label><span class="markRequired">*</span>
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
                                Program Name</label><span class="markRequired">*</span> <span  class="lblTop lblSubRight" data-toggle="tooltip" title="If your Project doesn't belong to any Program then please choose N/A. Otherwise please create a new Project with new Program if your Program is not in this list.">Can't find your program?</span>
                                <asp:DropDownList label="Project Name" runat="server" AppendDataBoundItems="true" ID="ddl_ProgramName" CssClass="form-control ddl_ProgramName cssRequired">
                                    <asp:ListItem Value="">------- Select a Program ------- </asp:ListItem>
                                </asp:DropDownList>
                        </div>

                        <div class="col-md-4">
                            <label class="lblforField">
                                Project Name</label><span class="markRequired">*</span> <span class="lblQuestion lblTop" data-toggle="tooltip" title="If you can not find your program or project then click Create New Project to add your project into the list." style="float:right;"><i class="fa fa-question-circle"></i></span> <asp:LinkButton ID="lblNewProject" OnClientClick="return openSPPopUp('NewProject.aspx?isDlg=1','Create New Project');" ToolTip="If you can not find your program or project then click here to add your project into the list." CssClass="lblTop linkSubRight" runat="server" data-toggle="tooltip">Create New Project</asp:LinkButton>
                            <asp:DropDownList label="Project Name" runat="server" AppendDataBoundItems="true" ID="ddlProjectList" CssClass="form-control cssRequired ddl_ProjectID">
                                <asp:ListItem Value="">---------- Select a Project ----------</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                    </div>
                </div>

                <div class="row">

                    <div class="col-md-12">

                    <div class="col-md-4">
                            <label class="lblforField">
                                Project ID</label>
                            <asp:TextBox CssClass="txtreadonly txtnoinput form-control txt_ProjectID" data-toggle="tooltip" ID="txt_ProjectID" placeholder="(Auto-Populated)" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-4">
                        <label class="lblforField">
                            Originating Business Unit
                        </label>
                        <asp:TextBox CssClass="form-control" label="Originating Business Unit" data-toggle="tooltip" ID="txt_BusinessUnit" placeholder="Originating Business Unit" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-4">
                        <label class="lblforField">
                            Department/Sponsor
                        </label><span class="markRequired">*</span>
                            <asp:DropDownList runat="server" label="Department/Sponsor" AppendDataBoundItems="true" ID="ddl_sDepartmentCode" CssClass="form-control cssRequired sDepartmentCode">
                                <asp:ListItem Value="">----------- Select Department/Sponsor --------</asp:ListItem>
                            </asp:DropDownList>
                    </div> 

                </div>

                </div>


                <div class="row">

                    <div class="col-md-12">

                        <div class="col-md-4">
                           
                            <label>
                                Project Manager
                            </label><span class="markRequired">*</span>
                            <div class="input-group">
                                <span class="input-group-addon"><i class="fa fa-user-o" aria-hidden="true"></i></span>
                                <div id="div_SponsorProjectManager" data-toggle="tooltip" label="Project Manager" title="Project Manager" class="form-control divUserInputField userfieldrequired"></div>
                            </div>
                        </div>




                        <div class="col-md-4">
                           
                            <label>
                                Project Group Manager
                            </label><span class="markRequired">*</span>
                            <div class="input-group">
                                <span class="input-group-addon"><i class="fa fa-user-o" aria-hidden="true"></i></span>
                                <div id="div_GroupManager" data-toggle="tooltip" title="Group Manager or equivalent" label="Project Group Manager" class="form-control divUserInputField userfieldrequired"></div>
                            </div>
                        </div>

                        <div class="col-md-4">
                           
                            <label>
                                Department Chief /Director
                            </label><span class="markRequired">*</span>
                            <div class="input-group">
                                <span class="input-group-addon"><i class="fa fa-user-o" aria-hidden="true"></i></span>
                                <div id="div_DepartmentChief" data-toggle="tooltip" label="Department Chief /Director" title="Department Chief /Director or equivalent" class="form-control divUserInputField userfieldrequired"></div>
                            </div>
                        </div>


                </div>


                </div>

            
                <div class="row">

               

                    <div class="col-md-12">

                

                        <div class="col-md-4" style="z-index:1">
                           
                            <label>
                                Executive Office Sponsor
                            </label><span class="markRequired">*</span>
                            <div class="input-group">
                                <span class="input-group-addon"><i class="fa fa-user-o" aria-hidden="true"></i></span>
                                <div id="div_DepartmentAGM" data-toggle="tooltip" label="Executive Office Sponsor" class="form-control divUserInputField userfieldrequired"></div>
                            </div>
                        </div>
                    
                        <div class="col-md-4" style="z-index:1">
                           
                            <label>
                                OCR Analyst (To be assigned by OCR Manager Group)
                            </label>
                            <div class="input-group">
                                <span class="input-group-addon"><i class="fa fa-user-o" aria-hidden="true"></i></span>
                                <div id="div_OCRAnalyst" data-toggle="tooltip" label="OCR Analyst" class="form-control divUserInputField txtreadonly txtnoinput"></div>
                            </div>
                        </div>

                        <div class="col-md-4" style="padding-right:7px;">
                        <label class="lblforField">
                            Kickoff Meeting Date
                        </label><span class="markRequired">*</span>
                        <asp:TextBox ID="txt_KickoffMeetingDate" data-toggle="tooltip" label="Kickoff Meeting Date" ToolTip="The project team must schedule a kickoff meeting with OCR and Procurement" placeholder="MM/DD/YYYY" CssClass="form-control datepickertxt cssRequired" runat="server"></asp:TextBox>
                    </div>

                    </div>

                    </div>




                     <div class="row">

                        <div class="col-md-12">

                            <div class="col-md-4">
                                <label class="lblforField">Contracting Plan Status</label>
                                <asp:Label ID="lbl_Status" runat="server" CssClass="sswpStatusValue lblStatus" Text="Draft"></asp:Label>
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
                                <asp:TextBox runat="server" label="Project Justification" TextMode="MultiLine" CssClass="form-control txtMultipleLines cssRequired txt_ProjectJustification" ID="txt_ProjectJustification" ></asp:TextBox>
                            </div>
                                <div class="col-md-12">
                                <label><b>Project Scope Of Work</b></label>
                                <span class="markRequired">*</span>
                                <asp:TextBox runat="server" label="Project Scope Of Work" TextMode="MultiLine" CssClass="form-control txtMultipleLines cssRequired txt_ProjectScopeOfWork" ID="txt_ProjectScopeOfWork" ></asp:TextBox>
                            </div>
                        </div>
                    </div>




                    <div class="row">

                        <div class="col-md-12">
                        <h5 style="margin-left: 2px;" class="formTextSmallSectionTitle">Unbundling Evaluation</h5>    
                            <p style="font-style:initial; font-weight:bold; font-style:italic;">The Project filled out the information (in italics below) required by the General Manager’s memo dated November 2, 2012.
                                BART Staff shall consider all of the following prior to determination of issuance of a solicitation:</p>
                            <div class="col-md-12">
                                <h6><u><b>By Dollar Amount</b></u></h6>
                                <div class="checklistone" label="Can the contract be separated into two or more contacts based on the dollar value?">
                                        Can the contract be separated into two or more contacts based on the dollar value?
                                        <asp:CheckBox ID="cb_UnbundlingByDollarAmount_Yes" CssClass="cb_UnbundlingByDollarAmount_Yes" runat="server" Text="Yes" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="cb_UnbundlingByDollarAmount_No" CssClass="cb_UnbundlingByDollarAmount_No" runat="server" Text="No" />
                                </div>
                                <asp:TextBox runat="server" label="Analysis" placeholder="Analysis" TextMode="MultiLine"  CssClass="form-control txtMultipleLines txt_ByDollarAmountAnalysis" ID="txt_ByDollarAmountAnalysis" ></asp:TextBox>

                            </div>                       
                            <div class="col-md-12">
                                <h6><u><b>By scope of work</b></u></h6>
                                <ul>
                                    <li class="checklistone"  label="Can the contract be separated into multiple scopes of work?">
                                        Can the contract be separated into multiple scopes of work?
                                        <asp:CheckBox ID="cb_UnbundlingByMultipleScopesOfWork_Yes" CssClass="cb_UnbundlingByMultipleScopesOfWork_Yes" runat="server" Text="Yes" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="cb_UnbundlingByMultipleScopesOfWork_No" CssClass="cb_UnbundlingByMultipleScopesOfWork_No" runat="server" Text="No" />
                                    </li>
                                    <li class="checklistone"  label="Are there specific technical requirements in the scope of work where the contract can be separated?">
                                        Are there specific technical requirements in the scope of work where the contract can be separated?
                                        <asp:CheckBox ID="cb_UnbundlingByContractSeparated_Yes" CssClass="cb_UnbundlingByContractSeparated_Yes" runat="server" Text="Yes" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="cb_UnbundlingByContractSeparated_No" CssClass="cb_UnbundlingByContractSeparated_No" runat="server" Text="No" />
                                    </li>
                                    <li class="checklistone"  label="Can the project/contract be separated into one or more smaller projects/contracts?">
                                        Can the project/contract be separated into one or more smaller projects/contracts?
                                        <asp:CheckBox ID="cb_UnbundlingBySmallerProjects_Yes" CssClass="cb_UnbundlingBySmallerProjects_Yes" runat="server" Text="Yes" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="cb_UnbundlingBySmallerProjects_No" CssClass="cb_UnbundlingBySmallerProjects_No" runat="server" Text="No" />
                                    </li>
                                </ul>
                                <asp:TextBox runat="server" label="Analysis" placeholder="Analysis" TextMode="MultiLine"  CssClass="form-control txtMultipleLines txt_ByScopeOfWorkAnalysis" ID="txt_ByScopeOfWorkAnalysis" ></asp:TextBox>


                                <h6><u><b>By schedule</b></u></h6>
                                <ul>
                                    <li class="checklistone"  label="Can the project be separated into smaller phases?">
                                            Can the project be separated into smaller phases?
                                            <asp:CheckBox ID="cb_UnbundlingBySchedule_Yes" CssClass="cb_UnbundlingBySchedule_Yes" runat="server" Text="Yes" />
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="cb_UnbundlingBySchedule_No" CssClass="cb_UnbundlingBySchedule_No" runat="server" Text="No" />
                                    </li>
                                </ul>
                                <asp:TextBox runat="server" label="Analysis" placeholder="Analysis" TextMode="MultiLine"  CssClass="form-control txtMultipleLines txt_ByScheduleAnalysis" ID="txt_ByScheduleAnalysis" ></asp:TextBox>



                                <h6><u><b>By geographical location</b></u></h6>
                                <ul>
                                    <li class="checklistone"  label="Are there any opportunities to separate the contract into geographic areas?">
                                            Are there any opportunities to separate the contract into geographic areas?
                                            <asp:CheckBox ID="cb_UnbundlingByLocation_Yes" CssClass="cb_UnbundlingByLocation_Yes" runat="server" Text="Yes" />
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="cb_UnbundlingByLocation_No" CssClass="cb_UnbundlingByLocation_No" runat="server" Text="No" />
                                    </li>
                                </ul>
                                <asp:TextBox runat="server" label="Analysis" placeholder="Analysis" TextMode="MultiLine"  CssClass="form-control txtMultipleLines txt_ByLocationAnalysis" ID="txt_ByLocationAnalysis" ></asp:TextBox>




                                <h6><u><b>By BART SEIU maintenance forces</b></u></h6>
                                <ul>
                                    <li class="checklistone"  label="Can the BART SEIU Maintenance Force be used to accomplish unbundling or perform one of the unbundled segments?">
                                            Can the BART SEIU Maintenance Force be used to accomplish unbundling or perform one of the unbundled segments?
                                            <asp:CheckBox ID="cb_UnbundlingByBARTSEIU_Yes" CssClass="cb_UnbundlingByBARTSEIU_Yes" runat="server" Text="Yes" />
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="cb_UnbundlingByBARTSEIU_No" CssClass="cb_UnbundlingByBARTSEIU_No" runat="server" Text="No" />
                                    </li>
                                </ul>
                                <asp:TextBox runat="server" label="Analysis" placeholder="Analysis" TextMode="MultiLine"  CssClass="form-control txtMultipleLines txt_ByBARTSEIUAnalysis" ID="txt_ByBARTSEIUAnalysis" ></asp:TextBox>

                            </div>                 

                            

                            <div class="col-md-12 attachmentSectionDev" style="padding-left:15px;">
                                <h6 style="margin-left: 2px; font-weight:bold; color:black;">Contracting Plan Related Attachment(s) <span style="font-size:smaller; font-style:italic; font-weight:500; color:black;">(Please attach any related documents other than worlkplan / flowchart here)</span> </h6>
                                <telerik:RadAsyncUpload RenderMode="Lightweight" PostbackTriggers="btnRoute" AllowedFileExtensions="jpg,jpeg,png,gif,doc,docx,txt,xls,xlsx,ppt,pptx,pdf,mpp,mpt,xlsb,cvs,xer,prx" runat="server" ID="CtrlAttachment"  MultipleFileSelection="Automatic" TemporaryFolder="D:\BartApps\tempfiles" Skin="Material" Localization-Select="Click here to attach documents" />
                                <asp:Label ID="lblUploadedDocs" runat="server" Text=""></asp:Label>
                            </div>


                            <asp:Panel ID="pnl_OCRGeneralInfo" CssClass="row" runat="server">

                                <div class="col-md-12">
                                <hr style="margin-left:15px;" />
                                

                                <div class="col-md-6">
                                    <h6 class="lblforField"><b class="ocrlbl">OCR CCU Analysis (For OCR Only)</b></h6>
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-user-o" aria-hidden="true"></i></span> 
                                        <div id="div_OCRCCUAnalysis" data-toggle="tooltip"  label=" OCR CCU Analysis Prepared By" class="form-control divUserInputField txtreadonly txtnoinput ocronly"></div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <p>&nbsp;</p>
                                    <asp:CheckBox ID="cb_OCROver10M_Yes" ForeColor="#0066cc" Text="Select if any Public Works Contracts or any Contract impacted by prevailing wage are Included" data-toggle="tooltip" ToolTip="Check this box if this contracting plan includes a Construction contract. This will notify the Labor Compliance Unit" Enabled="false" runat="server" CssClass="cb_OCROver10M_Yes ocronly" />
                                </div>

                                <div class="col-md-12" style="margin-top:5px;">
                                    <asp:TextBox runat="server" label="Summary of Analysis (For OCR CCU only)" placeholder="Summary of Analysis (For OCR CCU only)" Enabled="false" TextMode="MultiLine"  CssClass="form-control txtMultipleLines txt_OCRCCUAnalysisSummary OCRAnalysis ocronly" ID="txt_OCRCCUAnalysisSummary" ></asp:TextBox>
                                </div>

                                </div>
                            


                                <div class="col-md-12 div_OCRLCUAnalysis ctrlHidden" style="margin-top:10px;">

                                <div class="col-md-6">
                                    <h6 class="lblforField"><b class="ocrlbl">OCR LCU Analysis (For OCR Only)</b></h6>
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-user-o" aria-hidden="true"></i></span>
                                        <div id="div_OCRLCUAnalysis" class="form-control divUserInputField txtreadonly txtnoinput ocronly"></div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                   
                                </div>

                                <div class="col-md-12" style="margin-top:5px;">
                                    <asp:TextBox runat="server" Enabled="false" label="Analysis (For OCR only)" placeholder=" Summary of Analysis (For OCR LCU only)" TextMode="MultiLine"  CssClass="form-control txtMultipleLines txt_OCRLCUAnalysisSummary OCRAnalysis ocronly" ID="txt_OCRLCUAnalysisSummary" ></asp:TextBox>
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

            

            <asp:Panel ID="UpdatePnl_Contracts" runat="server">
               <div class="row">
                 <div class="col-md-12">

                     <div style="margin-top:10px;">
                     <span class="lbllarge">Number of Contracts: </span> <asp:Label CssClass="lbllarge lblNoofContract"  ID="lblNoofContract" runat="server" Text="N/A"></asp:Label>
                    </div>

                    <asp:Repeater ID="rpt_Contracts" runat="server" OnItemDataBound="rpt_Contracts_ItemDataBound">
                        <HeaderTemplate>
                            
                        </HeaderTemplate>
                        <ItemTemplate>
                            

                     <div class="col-md-12 contractform" style="margin-bottom:10px;">

                        <div class="row">

                             <div class="row gridheader">
                               <h5 style="margin-left: 15px;" class="formTextSmallSectionTitleContract">CONTRACT <%# Container.ItemIndex + 1 %><span class="ctrOrderNo ctrlHidden"><%# Container.ItemIndex + 1 %></span> <span style="float:right; margin-right:10px;"><asp:LinkButton ID="hplDelete" CssClass="hplDelete" ToolTip="Remove contract" runat="server"><i style="font-size:18px;color: #b91919;" class="fa fa-trash-o fa-2" aria-hidden="true"></i></asp:LinkButton></span></h5>
                                       <span class="ctrHeaderID form-control ctrlHidden" runat="server"><%# Eval("ItemID")%></span>

                            </div>

                            <div class="col-md-4">

                            <label class="lblforField">
                                Funding Source
                            </label><span class="markRequired">*</span>
                            <asp:DropDownList runat="server" label="Funding Source" AppendDataBoundItems="true" ID="ddl_FundingSource" CssClass="form-control cssRequired">
                                <asp:ListItem Value="">----- Select One----</asp:ListItem>
                                 <asp:ListItem Value="FTA">FTA</asp:ListItem>
                                 <asp:ListItem Value="RR">RR</asp:ListItem>
                                 <asp:ListItem Value="Local/State">Local/State</asp:ListItem>
                                 <asp:ListItem Value="DHS">DHS</asp:ListItem>
                                 <asp:ListItem Value="ACTC">ACTC</asp:ListItem>
                                 <asp:ListItem Value="TBD">TBD</asp:ListItem>
                            </asp:DropDownList>   
                        </div>
                         <div class="col-md-4">
                            <label class="lblforField">
                                Target Completion Date
                            </label>
                            <asp:TextBox ID="txt_TargetCompletionDate" data-toggle="tooltip" Text='<%# BART.SP.OCR.CP.Common.ProjectUtilities.DisplayDateTimeMMDDYYYY(Eval("TargetCompletionDate"))%>' label="Target Completion Date" placeholder="MM/DD/YYYY" CssClass="form-control datepickertxt" runat="server"></asp:TextBox>
                         </div>
                        <div class="col-md-4">
                            <label class="lblforField">
                                Duration
                            </label>
                           <asp:TextBox CssClass="form-control" label="Duration" Text='<%# Eval("Duration")%>'  data-toggle="tooltip" ID="txt_Duration" placeholder="Duration" runat="server"></asp:TextBox>
                        </div>

                        </div>

                        <div class="row" style="margin-top:10px; margin-bottom:5px;">

                            <div class="col-md-4">
                            <label class="lblforField">
                                Contract No
                            </label><span class="markRequired">*</span>
                           <asp:TextBox CssClass="form-control cssRequired" label="Contract No" Text='<%# Eval("ContractNo")%>' data-toggle="tooltip" ID="txt_ContractNo" placeholder="Contract No" runat="server"></asp:TextBox>
                        </div>

                         <div class="col-md-4">
                            <label class="lblforField">
                                Dollar Amount
                            </label><span class="markRequired">*</span>
                           <asp:TextBox CssClass="form-control floatInput cssRequired" label="Dollar Amount" data-toggle="tooltip" Text='<%# Eval("DollarAmount")%>' ID="txt_DollarAmount" placeholder="$" runat="server"></asp:TextBox>
                        </div>
                         <div class="col-md-4">
                            <label class="lblforField">
                                Contract Status
                            </label>
                            <asp:DropDownList runat="server" AppendDataBoundItems="true" ID="ddl_Status" CssClass="form-control">
                               <asp:ListItem Value="Not Started">Not Started</asp:ListItem>
                               <asp:ListItem Value="In Review">In Review</asp:ListItem>
                               <asp:ListItem Value="Pre-Award">Pre-Award</asp:ListItem>
                               <asp:ListItem Value="Awarded">Awarded</asp:ListItem>
                               <asp:ListItem Value="Cancelled">Cancelled</asp:ListItem>
                               <asp:ListItem Value="Closed">Closed</asp:ListItem>
                               <asp:ListItem Value="No Bid Received">No Bid Received</asp:ListItem>
                               <asp:ListItem Value="Deferred">Deferred</asp:ListItem>
                            </asp:DropDownList>  
                         </div>

                        </div>
                          <div class="row" style="margin-top:10px; margin-bottom:5px;">

                          <div class="col-md-8">
                            <label class="lblforField">
                                Description
                            </label>
                            <asp:TextBox runat="server" Text='<%# Eval("Description")%>' label="Contract Description" placeholder="Contract Description" TextMode="MultiLine"  CssClass="form-control txtMultipleLines txt_Description" ID="txt_Description" ></asp:TextBox>
                          </div>
                        <div class="col-md-4">
                            <label class="lblforField">OCR analysis for this contract (For OCR Only)</label>
                            <asp:TextBox runat="server" Text='<%# Eval("OCRAnalysis")%>' label="OCR analysis for this contract" TextMode="MultiLine"  CssClass="form-control txtMultipleLines txt_OCRAnalysis OCRAnalysis ocronly" Enabled="false" ID="txt_OCRAnalysis" ></asp:TextBox>
                          </div>
                        </div>

                         <telerik:RadAsyncUpload RenderMode="Lightweight" PostbackTriggers="btnRoute" AllowedFileExtensions="jpg,jpeg,png,gif,doc,docx,txt,xls,xlsx,ppt,pptx,pdf,mpp,mpt,xlsb,cvs,xer,prx" runat="server" ID="contractAttachment"  MultipleFileSelection="Automatic" TemporaryFolder="D:\BartApps\tempfiles" Skin="Material" Localization-Select="Attach Contract's document(s)" />
                         <asp:Label ID="lblContractAttachmentUploaded" runat="server" Text='<%# Eval("DisplayFiles")%>'></asp:Label>
                         <asp:TextBox ID="txt_OrderInTable" data-toggle="tooltip" Text='<%# Eval("OrderInTable")%>' CssClass="form-control ctrlHidden" runat="server"></asp:TextBox>
                         <asp:TextBox ID="txt_ItemID" data-toggle="tooltip" Text='<%# Eval("ItemID")%>' CssClass="form-control ctrlHidden" runat="server"></asp:TextBox>
                         <asp:TextBox ID="hiddenStatus" data-toggle="tooltip" Text='<%# Eval("Status")%>' CssClass="form-control ctrlHidden" runat="server"></asp:TextBox>
                         <asp:TextBox ID="hiddenFundingSource" data-toggle="tooltip" Text='<%# Eval("FundingSource")%>' CssClass="form-control ctrlHidden" runat="server"></asp:TextBox>
                         <asp:TextBox ID="txtVisible" Visible='<%# Eval("Visible")%>' CssClass="ctrlHidden" runat="server"></asp:TextBox>
                     </div>

                        </ItemTemplate>
                    </asp:Repeater>
                     <asp:Panel ID="pnlAddNewLink" runat="server" CssClass="col-md-12 row">
                         <hr />
                        <asp:LinkButton ID="lbt_AddNew" OnClick="lbt_AddNew_Click" OnClientClick="updateApproversValue();" Font-Bold="true" ForeColor="#006600" runat="server"><i class="fa fa-plus" aria-hidden="true"></i> Add New Contract</asp:LinkButton>
                         <br /><br />

                     </asp:Panel>
                     
                    
                 </div>
                        

                <div class="modal fade in" id="DelCtrConfirmationModal" role="dialog" aria-hidden="false" >
                    <div class="modal-dialog" style="margin-top:12%;">
                        <!-- Modal content-->
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" style="display:none">X</button>
                                <h4 class="modal-title" style="color:whitesmoke" id="titleMyConfirmDelCtr">Important Message</h4>
                            </div>
                            <div class="modal-body">
                                <p style="text-align:left;" class="msg"></p>
                                <asp:TextBox ID="txtRowNo" CssClass="txtRowNo ctrlHidden" runat="server"></asp:TextBox>
                            </div>
                            <div class="modal-footer">
                                <button type="button" runat="server" id="btnRemoveContract" onclick="showLoadingDiv(); updateApproversValue();" onserverclick="btnRemoveContract_ServerClick" data-dismiss="modal" class="btn btn-primary commitAction btnCommitRoute" title="Remove Contract" >
                                    Yes
                                </button>
                                <button type="button" class="btn btn-primary btnDismissDlg btnNobox"  data-dismiss="modal" title="Cancel and close dialog box">No</button>
                            </div>
                        </div>
                    </div>
            </div>



            </div>

                    <asp:HiddenField ID="hdfNoOfContracts" Value="0" runat="server" />

            </asp:Panel>
            

            <%--<asp:UpdateProgress ID="LoadingPnl_Contracts" AssociatedUpdatePanelID="UpdatePnl_Contracts" runat="server">
                <ProgressTemplate>
                    <div class="col-md-12 text-center">
                        <span style="font-size:21px; color:sienna;">Loading ..............</span>
                    </div>
                    
                </ProgressTemplate>
            </asp:UpdateProgress>--%>


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
                


                        <ul class="nav nav-wizard row" role="tablist" >
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
                            
                                        <div class="col-md-4 task_title">
                                            Approval Task
                                        </div>
                                        <div class="col-md-2 task_status">
                                            Status
                                        </div>
                                        <div class="col-md-2 task_date">
                                            Mofified
                                        </div>
                                        <div class="col-md-2 task_assigned">
                                            Assigned To
                                        </div>
                                    </div>
                            
                                </HeaderTemplate>
                                <ItemTemplate>
                            
                                    <div class="row gridbody">
                                        <div class="col-md-4 item">
                                            <asp:Label ID="grid_lbl_Title" runat="server" Text='<%# Eval("Title")%>'></asp:Label>
                                        </div>
                                        <div class="col-md-2 item text-center">
                                            <asp:Label ID="grid_lbl_Status" runat="server" Text='<%# Eval("TaskStatus")%>'></asp:Label>
                                        </div>
                                        <div class="col-md-2 item text-center">
                                            <asp:Label ID="grid_lbl_ApprovedDate" runat="server" Text='<%# Eval("ApprovedDate")%>'></asp:Label>
                                        </div>
                                        <div class="col-md-4 item text-center">
                                            <asp:Label ID="grid_lbl_AssignedToName" runat="server" Text=""></asp:Label>
                                            <asp:TextBox ID="grid_txtOrderIntbl" data-toggle="tooltip" Text='<%# Eval("ApprovalOrder")%>' CssClass="form-control requiredInline ctrlHidden" runat="server"></asp:TextBox>
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
                            
                                        <div class="col-md-4 task_title">
                                            Approval Task
                                        </div>
                                        <div class="col-md-2 task_status">
                                            Status
                                        </div>
                                        <div class="col-md-2 task_date">
                                            Mofified
                                        </div>
                                        <div class="col-md-2 task_assigned">
                                            Assigned To
                                        </div>
                                    </div>
                            
                                </HeaderTemplate>
                                <ItemTemplate>
                            
                                    <div class="row gridbody">
                                        <div class="col-md-4 item">
                                            <asp:Label ID="grid_lbl_Title" runat="server" Text='<%# Eval("Title")%>'></asp:Label>
                                        </div>
                                        <div class="col-md-2 item text-center">
                                            <asp:Label ID="grid_lbl_Status" runat="server" Text='<%# Eval("TaskStatus")%>'></asp:Label>
                                        </div>
                                        <div class="col-md-2 item text-center">
                                            <asp:Label ID="grid_lbl_ApprovedDate" runat="server" Text='<%# Eval("ApprovedDate")%>'></asp:Label>
                                        </div>
                                        <div class="col-md-4 item text-center">
                                            <asp:Label ID="grid_lbl_AssignedToName" runat="server" Text=""></asp:Label>
                                            <asp:TextBox ID="grid_txtOrderIntbl" data-toggle="tooltip" Text='<%# Eval("ApprovalOrder")%>' CssClass="form-control requiredInline ctrlHidden" runat="server"></asp:TextBox>
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
                            
                                        <div class="col-md-4 task_title">
                                            Approval Task
                                        </div>
                                        <div class="col-md-2 task_status">
                                            Status
                                        </div>
                                        <div class="col-md-2 task_date">
                                            Mofified
                                        </div>
                                        <div class="col-md-2 task_assigned">
                                            Assigned To
                                        </div>
                                    </div>
                            
                                </HeaderTemplate>
                                <ItemTemplate>
                            
                                    <div class="row gridbody">
                                        <div class="col-md-4 item">
                                            <asp:Label ID="grid_lbl_Title" runat="server" Text='<%# Eval("Title")%>'></asp:Label>
                                        </div>
                                        <div class="col-md-2 item text-center">
                                            <asp:Label ID="grid_lbl_Status" runat="server" Text='<%# Eval("TaskStatus")%>'></asp:Label>
                                        </div>
                                        <div class="col-md-2 item text-center">
                                            <asp:Label ID="grid_lbl_ApprovedDate" runat="server" Text='<%# Eval("ApprovedDate")%>'></asp:Label>
                                        </div>
                                        <div class="col-md-4 item text-center">
                                            <asp:Label ID="grid_lbl_AssignedToName" runat="server" Text=""></asp:Label>
                                            <asp:TextBox ID="grid_txtOrderIntbl" data-toggle="tooltip" Text='<%# Eval("ApprovalOrder")%>' CssClass="form-control requiredInline ctrlHidden" runat="server"></asp:TextBox>
                                        </div>

                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>

                            </div>


                            </div>


                        </div>
</div>

                        <div class="col-md-12" style="text-align:right; padding-right:30px;">

                            <a data-toggle="tooltip" title="Save this report and submit for approval" runat="server" id="A1" onclick="return SavenRoute();" class="btn btn-primary btnAction btnMoveStep submit4Approval">Submit Contracting Plan for Approval</a>

                        </div>

                    </div>

                    

                </asp:Panel>

        
            </div>
        </div>
</div>

    
<%-- END Step 3 --%>

       
    <%-- Step 5 --%>
<div class="tab-pane fade" id="step5">
        <div class="row top-container">
    <div class="top-inner-container">
        

    </div>
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
      <div class="col-md-6 sub-nav__buttons" style="text-align:left">
          <%--<a id="printbottom" data-toggle="tooltip" class="badge printer" style="display:none;" href="#" title="Print SSWP"><i class="fa fa-print fa-2" aria-hidden="true"></i></a>--%>
          <%--<span data-toggle="tooltip" title="Save Changes & Preview SSWP"><asp:LinkButton ID="previewbottom" runat="server" OnClientClick="return SavePreviewSSWP();"  CssClass="badge floppy btnAction btnSavePreviewAction "><i class="fa fa-eye fa-2"></i></asp:LinkButton></span>--%>
          <%--<span data-toggle="tooltip" title="Save Changes and route to approvers"><asp:LinkButton ID="lbtRouteBottom" OnClientClick="return RouteSSWP();" runat="server" CssClass="badge floppy btnSaveAction"><i class="fa fa-random fa-rotate-270" style="color:#337ab7"></i></asp:LinkButton></span>--%>
          <span data-toggle="tooltip" title="Cancel changes and return to home page."><a href="#" class="badge printer lbtCancel btnAction" onclick="CancelChanges();" ><i class="fa fa-times fa-2"  aria-hidden="true"></i></a></span>
<%--          <a id="printbottom" data-toggle="tooltip" class="badge printer printsswplink" href="#" target="_blank" title="Print Contracting Plan"><i class="fa fa-print" aria-hidden="true"></i></a>--%>
          <span data-toggle="tooltip" title="Save Changes"><asp:LinkButton ID="lbtSaveBottom" runat="server" OnClientClick="return SaveObject('Draft');" CssClass="badge floppy btnSaveAction"><i class="fa fa-floppy-o"></i></asp:LinkButton></span>
          <%--<span data-toggle="tooltip" title="Copy this report(This feature allows PM to copy all data in this report to create a new report quickly.)"><asp:LinkButton ID="lbtCopy" OnClientClick="return CopyReport();" runat="server" CssClass="badge floppy btnAction btnCopy"><i style="color:#009688" class="fa fa-files-o"></i></asp:LinkButton></span>--%>
      </div>
          <div class="col-md-6" style="text-align:right">
    
          <asp:Panel ID="pnlButton" CssClass="btngroupactions" runat="server">
              <div style="padding-top:10px;">
                <a href="#top" class="btnnext hidelow" linkval="step1-tab" id="backbtn" role="tab" data-toggle="tab" aria-expanded="false"> <i class="fa fa-arrow-left" aria-hidden="true"></i> Back </a>
                  &nbsp;&nbsp;&nbsp;&nbsp;
                <a href="#top" class="btnnext" linkval="step2-tab" id="nextbtn" role="tab" data-toggle="tab" aria-expanded="false">Next <i class="fa fa-arrow-right" aria-hidden="true"></i></a>
                  <a data-toggle="tooltip" title="Save this report and submit for department's manager approval" runat="server" id="btnSubmitForApproval" onclick="return SavenRoute();" class="btn btn-primary btnAction hidelow btnMoveStep submit4Approval">Submit</a>
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

<asp:TextBox runat="server" ID="hdfCurrentTab" CssClass="hdfCurrentTab ctrlHidden"></asp:TextBox>

<asp:TextBox CssClass="txtnoinput form-control ctrlHidden" ID="txt_ServiceType" runat="server"></asp:TextBox>
<asp:TextBox CssClass="txtnoinput form-control ctrlHidden txt_SponsorDepartment" ID="txt_SponsorDepartment" runat="server"></asp:TextBox>

<asp:TextBox CssClass="txtnoinput form-control ctrlHidden txt_ProgramDes" ID="txt_ProgramDes" runat="server"></asp:TextBox>

<asp:TextBox runat="server" ID="txt_hdfdeletedFiles" CssClass="form-control ctrlHidden txthdfdeletedFiles"></asp:TextBox>
<asp:TextBox ID="txtCommitAction" CssClass="ctrlHidden txtCommitAction" runat="server"></asp:TextBox>
<%-- END hidden fields --%>


<%-- Start Dialog Box --%>

<div class="modal fade in" id="myConfirmationModal" role="dialog" aria-hidden="false" >
        <div class="modal-dialog boxConfirm">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" style="display:none">X</button>
                    <h4 class="modal-title" id="titleMyConfirm">Important Message</h4>
                </div>
                <div class="modal-body">
                    <p class="txtComfirmationMss"></p>
                </div>
                <div class="modal-footer">
                   <%-- <button type="button" runat="server" id="btnCommitSave" onclick="showLoadingDiv();" onserverclick="lbtSave_Click" data-dismiss="modal" class="btn btn-primary commitAction btnCommitSave" title="Save Changes">
                        Yes
                    </button>--%>
                  <%--   <button type="button" runat="server" id="btnSavePreview" onclick="showLoadingDiv();" data-dismiss="modal" class="btn btn-primary commitAction btnSavePreviewSubmit" onserverclick="btnCommitSave_ServerClick" title="Save changes">
                        Yes
                    </button>--%>
                    <button type="button" runat="server" id="btnRoute" onclick="showLoadingDiv();" data-dismiss="modal" onserverclick="btnRoute_ServerClick" class="btn btn-primary commitAction btnCommitRoute">
                        Yes
                    </button>
               <%--     <button type="button" runat="server" id="btnCopy" onclick="showLoadingDiv();" data-dismiss="modal" onserverclick="btnCopy_ServerClick" class="btn btn-primary commitAction btnCommitCopy" title="Copy this report" >
                        Yes
                    </button>--%>
                    <a class="btn btn-primary commitAction cancelCommit" id="btnCancelReport" runat="server" onserverclick="btnCancelReport_ServerClick" title="Cancel changes and return home">Yes</a>
                    <button type="button" class="btn btn-primary btnDismissDlg btnNobox"  data-dismiss="modal" title="Cancel and close dialog box">No</button>
                </div>
            </div>
        </div>
</div>

<%-- End Dialog Box --%>
<div class="modalbackdropDiv">
    <div>
        <i class="fa fa-circle-o-notch fa-spin" style="float: left;color: black;color: #fff;font-size: 100px;"></i>
    </div>
</div>
<asp:DropDownList runat="server" ID="ddlHiddenProjectList" CssClass="form-control ddlHiddenProjectList ctrlHidden">
</asp:DropDownList>
<asp:TextBox ID="txtHiddenSelectedPrjOption" CssClass="ctrlHidden txtHiddenSelectedPrjOption" runat="server"></asp:TextBox>
<asp:Button ID="btnLoadPopUp" CssClass="btnLoadPopUp ctrlHidden" OnClientClick="return ReloadPage();"  runat="server" Text="Button" OnClick="btnLoadPopUp_Click" />

