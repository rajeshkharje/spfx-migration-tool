<%@ Assembly Name="BART.SP.OCR.CP.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9abfeb7dc254e359" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrintUserControl.ascx.cs" Inherits="BART.SP.OCR.CP.Web.Print.PrintUserControl" %>

<script type="text/javascript" src="/_layouts/15/BART.SP.OCR.CP.Web/js/bootstrap-datepicker.min.js?refId=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>"></script>
<script type="text/javascript" src="/SiteAssets/AppCP/Core.js?refId=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>"></script>
<script type="text/javascript" src="/SiteAssets/AppCP/UI.js?versionview=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>"></script>
<script type="text/javascript" src="/SiteAssets/AppCP/Validation.js?versionview=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>"></script>
<script type="text/javascript" src="/SiteAssets/AppCP/LoadNEvents.js?versionview=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>"></script>





<script>

    $(document).ready(function () {
        Breaktext();
        //hideLoadingDiv();
        LoadFloatFormat();
        LoadCheckLCU();
        LoadCalculationFormat();
        cancelEnterOnSinglineText();
        //showRequiredScheduleFields();
        allLoadJSCreateNew();
        removeEmptyAcq();

        $('.ContentToPrintOut').hide(); $('.FileNameToInputWithoutExt').hide();
        var loadingString = '<div class="container" style="text-align: center;margin-top: 150px;font-size: 25px;color: #ababab;"><i class="fa fa-circle-o-notch fa-spin fa-3x fa-fw margin-bottom"></i><span>Loading ...</span></div>';
        $('div.loadingdivforprint').html(loadingString);
        $("input[id$='_btnHiddenPrint']").hide();
        //var txtSave2Print = 'input.ContentToPrintOut';
        //var cVal = $('.printManagementDocs').html();
        //$(txtSave2Print).val('<div class="container printManagementDocs">' + cVal + '</div>');
        ReplaceURLForRevisedDocsCopy();
        
        var isDone = $('.txtActionCompleted').val();
        if (isDone.indexOf('1') < 0)
        {
            setTimeout(function () {
                $("input[id$='_btnHiddenPrint']").trigger('click');
            }, 100);
        }
    });

    function fillDataToTxt()
    {
        var txtSave2Print = 'input.ContentToPrintOut';
        var cVal = $('.printManagementDocs').html();
        $(txtSave2Print).val('<div class="container printManagementDocs">' + cVal + '</div>');
    }

</script>

<style>
    #breadcrumbnavigation{display:none;}
</style>
<h2 style="text-align:center;">
    <br /><br />
    <p style="color:lightblue">&nbsp;&nbsp;&nbsp;&nbsp;Loading ....
        <br />
    <i class="fa fa-circle-o-notch fa-spin" style="font-size:90px; color:lightgray;"></i>
    </p>
    <br /><br />
</h2>
<div class="container displayview printview printManagementDocs hidelow">

<link rel="stylesheet" type="text/css" href="/SiteAssets/css/bootstrap.min.css" />
<link  rel="stylesheet" type="text/css" href="/_layouts/15/BART.SP.OCR.CP.Web/css/OriginalCSS.css?refId=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>" />
<link  rel="stylesheet" type="text/css" href="/_layouts/15/BART.SP.OCR.CP.Web/css/bootstrap-datepicker3.css?refId=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>" />
<link  rel="stylesheet" type="text/css" href="/SiteAssets/AppCP/print.css?refId=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>" />

   

    <style>
        div.row
        {
            padding-top: 0px !important;
        }
        .avoidbreak
        {
            page-break-inside:avoid;
        }
        li a.removeUploadedFile {
            display: none;
        }
        .sswpStatusValue {
            color: #333 !important;
            font-weight:700;
        }
        .sswpuploadedDocuments
        {
            float:left;
            padding-left:0px !important;
        }
        .top-container {
            padding: 0px;
            background-color:transparent !important;
            border:0px dashed White !important;
            border-radius: 4px;
        }
        span.form-control.txtMultipleLines
        {
	        height:auto !important;
        }
    </style>



    <asp:Panel runat="server" ID="pnlErrorMsg" Visible="false" CssClass="alert alert-danger alert-dismissable pnlErrorMsgCss"> 
        <a href="#" class="close" data-dismiss="alert" aria-label="close">×</a>
        <asp:Label ID="lblError" runat="server"></asp:Label>
</asp:Panel>

<asp:Panel runat="server" ID="pnlSuccessMsg" Visible="false" CssClass="alert alert-success alert-dismissable pnlSuccessMsgCss"> 
    <a href="#" class="close" data-dismiss="alert" aria-label="close">×</a>
    <asp:Label ID="lblSuccessMessage" runat="server"><strong>Your action has been submitted successfully!</strong></asp:Label>
</asp:Panel>

<div style="width: 100%; float:left; margin-top: 40px; margin-bottom: 35px;">
    <img src="/PublishingImages/BART_Logo_Color_Small_PROD.png" style="float:left;"> 
    <h3 class="text-center PrintProcTitle" style="margin-top:15px;font-size: 21px;font-weight: 700;color: #166db3;">CONTRACTING PLAN</h3>
</div>


<div data-example-id="togglable-tabs" class="tabingparentDiv EditReport"> 

<asp:Label ID="lblErrorMessage" ForeColor="Red" runat="server" Text=""></asp:Label>
<a name="top"></a>















    <div class="row top-container">
        <div class="top-inner-container">
        <asp:Panel ID="pnlStep1" CssClass="col-sm-12 steplevel" runat="server">


                <div class="form-group sswpFields" style="margin-bottom:0px;">

                <div class="row">
                    
                        <div class="col-sm-6">
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
                        <div class="col-sm-6">
                            <label class="lblforField">
                                Program Name</label><span class="markRequired">*</span>
                            <asp:Label CssClass="form-control" data-toggle="tooltip" ID="lbl_ProgramDes" placeholder="" runat="server"></asp:Label>
                        </div> 
                         

                </div>



                    <div class="row">
                        <div class="col-sm-6">
                            <label class="lblforField">
                                Project ID</label>
                          <asp:Label CssClass="txtreadonly form-control txt_ProjectID" data-toggle="tooltip" ID="lbl_ProjectID" placeholder="(Auto-Populated)" runat="server"></asp:Label>
                    </div>

                        <div class="col-sm-6">
                            <label class="lblforField">
                                Project Name</label><span class="markRequired">*</span>
                            
                            <asp:Label CssClass="form-control" data-toggle="tooltip" ID="lbl_ProjectName" placeholder="" runat="server"></asp:Label>
                        </div>


                    </div>


                <div class="row">


                    
                    <div class="col-sm-6">
                        <label class="lblforField">
                            Originating Business Unit
                        </label>
                        <asp:Label CssClass="form-control" label="Originating Business Unit" data-toggle="tooltip" ID="lbl_BusinessUnit" placeholder="Originating Business Unit" runat="server"></asp:Label>
                    </div>
                    <div class="col-sm-6">
                        <label class="lblforField">
                            Department/Sponsor
                        </label><span class="markRequired">*</span>
                        <asp:Label CssClass="form-control" data-toggle="tooltip" ID="lbl_SponsorDepartment" runat="server"></asp:Label>
                    </div> 


                </div>


                <div class="row">


                        <div class="col-sm-6">
                           
                            <label>
                                Project Manager
                            </label><span class="markRequired">*</span>
                            <asp:Label CssClass="form-control" data-toggle="tooltip" ID="lbl_SponsorProjectManagerName" runat="server"></asp:Label>
                        </div>




                        <div class="col-sm-6">
                           
                            <label>
                                Project Group Manager
                            </label><span class="markRequired">*</span>
                            <asp:Label CssClass="form-control" data-toggle="tooltip" ID="lbl_GroupManagerName" runat="server"></asp:Label>
                        </div>

                        




                </div>


                    <div class="row">

                        <div class="col-sm-6">
                           
                            <label>
                                Department Chief /Director
                            </label><span class="markRequired">*</span>
                            <asp:Label CssClass="form-control" data-toggle="tooltip" ID="lbl_DepartmentChiefName" runat="server"></asp:Label>
                        </div>


                        <div class="col-sm-6" style="z-index:1">
                           
                            <label>
                                Executive Office Sponsor
                            </label><span class="markRequired">*</span>
                            <asp:Label CssClass="form-control" data-toggle="tooltip" ID="lbl_DepartmentAGMName" runat="server"></asp:Label>
                        </div>

                          </div>


            
                <div class="row">

               

              

                        
                    
                        <div class="col-sm-6" style="z-index:1">
                           
                            <label>
                                OCR Analyst
                            </label>
                            <asp:Label CssClass="form-control" data-toggle="tooltip" ID="lbl_OCRAnalystName" runat="server"></asp:Label>
                        </div>

                        <div class="col-sm-6" style="z-index:1;">
                        <label class="lblforField">
                            Kickoff Meeting Date
                        </label>
                        <asp:Label ID="txt_KickoffMeetingDate" data-toggle="tooltip" CssClass="form-control datepickertxt" runat="server"></asp:Label>
                    </div>

                   </div>




                     <div class="row">


                            <div class="col-sm-4">
                                <p class="lblforField">Contracting Plan Status</p>
                                <asp:Label ID="lbl_Status" runat="server" CssClass="sswpStatusValue" Text="Draft"></asp:Label>
                            </div>

                            <div class="col-sm-4">
                                <p class="lblforField">Date Submitted</p>
                                <asp:Label class="sswpStatusValue" ID="lbl_DateSubmitted" runat="server" Text="N/A"></asp:Label>
                            </div>

                            <div class="col-sm-4">
                                <p class="lblforField">Initiated By</p>
                                <asp:Label class="sswpStatusValue" ID="lbl_RequesterName" runat="server" Text=""></asp:Label>
                            </div>


                     </div>

                    <div class="row">
                        
                            <div class="col-sm-12">
                                <label><b>Project Justification</b> (Outlines the need for the project, problem(s) addressed and risks being mitigated)</label>
                                <span class="markRequired">*</span>
                                <asp:Label ID="lbl_ProjectJustification" CssClass="form-control txtMultipleLines cssRequired lbl_ProjectJustification" runat="server" Text=""></asp:Label>
                                
                            </div>
                                <div class="col-sm-12">
                                <label><b>Project Scope Of Work</b></label>
                                <span class="markRequired">*</span>
                                <asp:Label runat="server"  TextMode="MultiLine" CssClass="form-control txtMultipleLines cssRequired lbl_ProjectScopeOfWork" ID="lbl_ProjectScopeOfWork" ></asp:Label>
                            </div>
                        
                    </div>




                    <div class="row">

                        <h5 style="margin-left: 2px;" class="formTextSmallSectionTitle">UNBUNDLING EVALUATION</h5>
                            <p style="font-style:initial;">The Project filled out the information (in italics below) required by the General Manager’s memo dated November 2, 2012.
                                BART Staff shall consider all of the following prior to determination of issuance of a solicitation:</p>
                            <div class="col-sm-12 bundling">
                                <h6><b>By Dollar Amount</b></h6>
                                <div class="checklistone" label="Can the contract be separated into two or more contacts based on the dollar value?">
                                        Can the contract be separated into two or more contacts based on the dollar value?
                                        <asp:CheckBox ID="cb_UnbundlingByDollarAmount_Yes" CssClass="cb_UnbundlingByDollarAmount_Yes" runat="server" Text="Yes" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="cb_UnbundlingByDollarAmount_No" CssClass="cb_UnbundlingByDollarAmount_No" runat="server" Text="No" />
                                </div>
                                <asp:Label runat="server"  placeholder="" TextMode="MultiLine"  CssClass="form-control txtMultipleLines txt_ByDollarAmountAnalysis" ID="lbl_ByDollarAmountAnalysis" ></asp:Label>

                            </div>                       
                            <div class="col-sm-12 bundling">
                                <h6><b>By scope of work</b></h6>
                                <ul>
                                    <li class="checklistone"  label="Can the contract be separated into multiple scopes of work?">
                                        Can the contract be separated into multiple scopes of work?
                                        <asp:CheckBox ID="cb_UnbundlingByMultipleScopesOfWork_Yes" CssClass="cb_UnbundlingByMultipleScopesOfWork_Yes" runat="server" Text="Yes" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="cb_UnbundlingByMultipleScopesOfWork_No" CssClass="cb_UnbundlingByMultipleScopesOfWork_No" runat="server" Text="No" />
                                    </li>
                                    <li class="checklistone" style="white-space:nowrap" label="Are there specific technical requirements in the scope of work where the contract can be separated?">
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
                                <asp:Label runat="server"  placeholder="" TextMode="MultiLine"  CssClass="form-control txtMultipleLines txt_ByScopeOfWorkAnalysis" ID="lbl_ByScopeOfWorkAnalysis" ></asp:Label>


                                <h6><b>By schedule</b></h6>
                                <ul>
                                    <li class="checklistone"  label="Can the project be separated into smaller phases?">
                                            Can the project be separated into smaller phases?
                                            <asp:CheckBox ID="cb_UnbundlingBySchedule_Yes" CssClass="cb_UnbundlingBySchedule_Yes" runat="server" Text="Yes" />
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="cb_UnbundlingBySchedule_No" CssClass="cb_UnbundlingBySchedule_No" runat="server" Text="No" />
                                    </li>
                                </ul>
                                <asp:Label runat="server" label="Analysis" placeholder="Analysis" TextMode="MultiLine"  CssClass="form-control txtMultipleLines txt_ByScheduleAnalysis" ID="lbl_ByScheduleAnalysis" ></asp:Label>



                                <h6><b>By geographical location</b></h6>
                                <ul>
                                    <li class="checklistone"  label="Are there any opportunities to separate the contract into geographic areas?">
                                            Are there any opportunities to separate the contract into geographic areas?
                                            <asp:CheckBox ID="cb_UnbundlingByLocation_Yes" CssClass="cb_UnbundlingByLocation_Yes" runat="server" Text="Yes" />
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="cb_UnbundlingByLocation_No" CssClass="cb_UnbundlingByLocation_No" runat="server" Text="No" />
                                    </li>
                                </ul>
                                <asp:Label runat="server"  placeholder="" TextMode="MultiLine"  CssClass="form-control txtMultipleLines txt_ByLocationAnalysis" ID="lbl_ByLocationAnalysis" ></asp:Label>




                                <h6><b>By BART SEIU maintenance forces</b></h6>
                                <ul>
                                    <li class="checklistone"  label="Can the BART SEIU Maintenance Force be used to accomplish unbundling or perform one of the unbundled segments?">
                                            Can the BART SEIU Maintenance Force be used to accomplish unbundling or perform one of the unbundled segments?
                                            <asp:CheckBox ID="cb_UnbundlingByBARTSEIU_Yes" CssClass="cb_UnbundlingByBARTSEIU_Yes" runat="server" Text="Yes" />
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="cb_UnbundlingByBARTSEIU_No" CssClass="cb_UnbundlingByBARTSEIU_No" runat="server" Text="No" />
                                    </li>
                                </ul>
                                <asp:Label runat="server"  TextMode="MultiLine"  CssClass="form-control txtMultipleLines txt_ByBARTSEIUAnalysis" ID="lbl_ByBARTSEIUAnalysis" ></asp:Label>

                            </div>                 

                            

                            <div class="col-sm-12 attachmentSectionDev" style="padding-left:15px;">
                                <h6 style="margin-left: 2px; font-weight:bold; text-transform:uppercase;color:#ab480b;">Contracting Plan Related Attachment(s) <span style="font-size:smaller; font-style:italic; font-weight:600; text-transform:capitalize; color:black;">(All related documents other than worlkplan / flowchart)</span> </h6>
                                <asp:Label ID="lblUploadedDocs" runat="server" Text=""></asp:Label>
                            </div>


                            <asp:Panel ID="pnl_OCRGeneralInfo" CssClass="row" runat="server">

                               <div class="col-sm-12">
                                <h5 style="margin-left: 2px;" class="formTextSmallSectionTitle">OCR ANALYSIS (FOR OCR ONLY)</h5>

                                

                                <div class="col-sm-8">
                                    <h6 ><b class="ocrlbl">OCR CCU Analysis (For OCR Only)</b></h6>
                                    <p style="margin-bottom:0px;">CCU Analyst: </p>
                                    <asp:Label CssClass="form-control" placeholder="OCR CCU Analyst" data-toggle="tooltip" ID="lbl_OCRCCUAnalysisName" runat="server"></asp:Label>
                                </div>
                                <div class="col-sm-4">
                                    <p>&nbsp;</p><p>&nbsp;</p>
                                    <%-- <asp:CheckBox ID="cb_OCROver10M_Yes"  ForeColor="#0066cc" Text="Over 10,000,000 ?" runat="server" CssClass="cb_OCROver10M_Yes" /> --%>
                                    <asp:CheckBox ID="cb_OCROver10M_Yes" ForeColor="#0066cc" Text="Select if any Public Works Contracts or any Contract impacted by prevailing wage are Included" Enabled="false" runat="server" CssClass="cb_OCROver10M_Yes" />
                                </div>

                                
                                    <div class="col-sm-12" style="margin-top:10px;">
                                    <div style="float:left;">
                                        CCU Analysis: 
                                    </div>
                                        <asp:Label runat="server"  TextMode="MultiLine"  CssClass="form-control txtMultipleLines txt_OCRCCUAnalysisSummary OCRAnalysis" ID="lbl_OCRCCUAnalysisSummary" ></asp:Label>
                                    </div>

                                </div>

                                <div class="col-sm-12 div_OCRLCUAnalysis" style="margin-top:10px;">

                                <div class="col-sm-8">
                                    <h6><b class="lblforField ocrlbl">OCR LCU Analysis (For OCR Only)</b></h6>
                                    <p style="margin-bottom:0px;">LCU Analyst: </p>
                                    <asp:Label CssClass="form-control" data-toggle="tooltip" ID="lbl_OCRLCUAnalysisName" runat="server"></asp:Label>
                                </div>
                                <div class="col-sm-4">
                                   
                                </div>

                                <div class="col-sm-12" style="margin-top:5px;">
                                    <div style="float:left;">
                                        LCU Analysis: 
                                    </div>
                                    <asp:Label runat="server"  TextMode="MultiLine"  CssClass="form-control txtMultipleLines txt_OCRLCUAnalysisSummary OCRAnalysis" ID="lbl_OCRLCUAnalysisSummary" ></asp:Label>
                                </div>

                                    
                                </div>


                            </asp:Panel>

                        

                    </div>

                          
                </div>

            

        </asp:Panel>


        <asp:Panel ID="pnlStep2" CssClass="col-sm-12 steplevel" runat="server">

            

          

            <div class="row">
                 
                     <h5 style="margin-left: 2px;" class="formTextSmallSectionTitle">CONTRACT(S)</h5>
                     <div style="margin-top:10px;">
                     <h6><b>Number of Contracts: </b><asp:Label CssClass="lblNoofContract" ID="lblNoofContract" runat="server" Text="N/A"></asp:Label></h6> 
                    </div>

                    <asp:Repeater ID="rpt_Contracts" runat="server">
                        <HeaderTemplate>
                            
                        </HeaderTemplate>
                        <ItemTemplate>
                            

                     <div class="col-sm-12 contractform" style="margin-bottom:10px;">

                        <div class="row avoidbreak">

                             <div class="row gridheader">
                               <h6 style="margin-left: 15px;" class="formTextSmallSectionTitleContract">CONTRACT <%# Container.ItemIndex + 1 %></h6>

                            </div>

                            <div class="col-sm-4">

                            <label class="lblforField">
                                Funding Source
                            </label><span class="markRequired">*</span>
                            <asp:Label CssClass="form-control" Text='<%# Eval("FundingSource")%>' runat="server"></asp:Label>
                                   
                        </div>
                         <div class="col-sm-4">
                            <label class="lblforField">
                                Target Completion Date
                            </label>
                            <asp:Label ID="txt_TargetCompletionDate" data-toggle="tooltip" Text='<%# BART.SP.OCR.CP.Common.ProjectUtilities.DisplayDateTimeMMDDYYYY(Eval("TargetCompletionDate"))%>' CssClass="form-control datepickertxt" runat="server"></asp:Label>
                         </div>
                        <div class="col-sm-4">
                            <label class="lblforField">
                                Duration
                            </label>
                           <asp:Label CssClass="form-control" Text='<%# Eval("Duration")%>'  data-toggle="tooltip" ID="lbl_Duration" placeholder="Duration" runat="server"></asp:Label>
                        </div>

                        </div>

                        <div class="row avoidbreak" style="margin-top:10px; margin-bottom:5px;">

                            <div class="col-sm-4">
                            <label class="lblforField">
                                Contract No
                            </label><span class="markRequired">*</span>
                           <asp:Label CssClass="form-control cssRequired" Text='<%# Eval("ContractNo")%>' data-toggle="tooltip" ID="lbl_ContractNo" placeholder="Contract No" runat="server"></asp:Label>
                        </div>

                         <div class="col-sm-4">
                            <label class="lblforField">
                                Dollar Amount
                            </label><span class="markRequired">*</span>
                           <asp:Label CssClass="form-control" label="Dollar Amount" data-toggle="tooltip" Text='<%# Eval("DollarAmount")%>' ID="lbl_DollarAmount" placeholder="$" runat="server"></asp:Label>
                        </div>
                         <div class="col-sm-4">
                            <label class="lblforField">
                                Contract Status
                            </label>
                            
                             <asp:Label CssClass="form-control" data-toggle="tooltip" Text='<%# Eval("Status")%>' ID="Label1" runat="server"></asp:Label>
                         </div>

                        </div>
                          <div class="row" style="margin-top:10px; margin-bottom:5px;">

                          <div class="col-sm-6">
                            <label class="lblforField">
                                Description
                            </label><span class="markRequired">*</span>
                            <asp:Label runat="server"  Text='<%# Eval("Description")%>' label="Contract Description" placeholder="Contract Description" TextMode="MultiLine"  CssClass="form-control txtMultipleLines txt_Description" ID="lbl_Description" ></asp:Label>
                          </div>
                        <div class="col-sm-6">
                            <label class="lblforField ocrlbl">OCR analysis (For OCR Only)</label>
                            <asp:Label runat="server" Text='<%# Eval("OCRAnalysis")%>' label="OCR analysis for this contract" TextMode="MultiLine"  CssClass="form-control txtMultipleLines txt_OCRAnalysis OCRAnalysis"  ID="lbl_OCRAnalysis" ></asp:Label>
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

                    <asp:HiddenField ID="hdfNoOfContracts" Value="0" runat="server" />
        </asp:Panel>


        <asp:Panel ID="pnlStep3" CssClass="pnlStep3 steplevel" runat="server">


                    <h5 style="margin-left: 2px;" class="formTextSmallSectionTitle">APPROVAL TASKS</h5>

                    <div class="row">
                


                       <%-- <ul class="nav nav-wizard row" id="approvalTabs" role="tablist" >
                            <li role="presentation" class="active"><a href="#listep1" id="listep1-tab" role="tab" data-toggle="tab" aria-controls="listep1tab" aria-expanded="false">Staff Approval</a></li>
                            <li role="presentation"><a href="#listep2" id="listep2-tab" role="tab" data-toggle="tab" aria-controls="listep2tab" aria-expanded="false">Management Approval</a></li>
                            <li role="presentation"><a href="#listep3" id="listep3-tab" role="tab" data-toggle="tab" aria-controls="listep3tab" aria-expanded="false">Executive Approval</a></li>
                          </ul>--%>


                <div class="tab-content">
            
                      

                        <div class="approvalTab">

                        <div class="col-sm-12 div_task avoidbreak">

                            <h6 style="" class="approvalTitle">Staff Approval</h6>

                          <div class="taskList gridview" >
                        
                            <asp:Repeater ID="rpt_Tasks_Staff" runat="server">
                                


                                                               <HeaderTemplate>
                                    <div class="row gridheader">
                            
                                        <div class="col-sm-4 task_title" style="width:22%">
                                            Task
                                        </div>
                                        <div class="col-sm-2 task_status" style="width:14%">
                                            Status
                                        </div>
                                        <div class="col-sm-2 task_date" style="width:16%">
                                            Mofified
                                        </div>
                                        <div class="col-sm-4 task_assigned text-left" style="width:24%">
                                            Assigned To
                                        </div>
                                        <div class="col-sm-4 task_assigned text-left" style="width:24%">
                                            Completed By
                                        </div>
                                    </div>
                            
                                </HeaderTemplate>
                                <ItemTemplate>
                            
                                    <div class="row gridbody">
                                        <div class="col-sm-4 item" style="width:22%">
                                            <asp:Label ID="grid_lbl_Title" runat="server" Text='<%# Eval("Title")%>'></asp:Label>
                                        </div>
                                        <div class="col-sm-2 item text-center" style="width:14%">
                                            <asp:Label ID="grid_lbl_Status" runat="server" Text='<%# Eval("TaskStatus")%>'></asp:Label>
                                        </div>
                                        <div class="col-sm-2 item text-center" style="width:16%">
                                            <asp:Label ID="grid_lbl_ApprovedDate" runat="server" Text='<%# Eval("ApprovedDate")%>'></asp:Label>
                                        </div>
                                        <div class="col-sm-4 item" style="width:24%">
                                            <asp:Label ID="grid_lbl_AssignedToName" runat="server" Text='<%# Eval("AssignedToName")%>'></asp:Label>
                                            <asp:TextBox ID="grid_txtOrderIntbl" data-toggle="tooltip" Text='<%# Eval("ApprovalOrder")%>' CssClass="form-control requiredInline ctrlHidden" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4 item" style="width:24%">
                                            <asp:Label ID="grid_lblApprovedBy" runat="server" Text='<%# Eval("ApprovedByName")%>'></asp:Label>
                                        </div>

                                    </div>
                                </ItemTemplate>




                            </asp:Repeater>

                            </div>


                            </div>


                            <div class="col-sm-12 div_task avoidbreak">
                                <h6 style="" class="approvalTitle">Management Approval</h6>
                          <div class="taskList gridview" >
                        


                              










                            <asp:Repeater ID="rpt_Tasks_Management" runat="server">
                                





                                <HeaderTemplate>
                                    <div class="row gridheader">
                            
                                        <div class="col-sm-4 task_title" style="width:22%">
                                            Task
                                        </div>
                                        <div class="col-sm-2 task_status" style="width:14%">
                                            Status
                                        </div>
                                        <div class="col-sm-2 task_date" style="width:16%">
                                            Mofified
                                        </div>
                                        <div class="col-sm-4 task_assigned text-left" style="width:24%">
                                            Assigned To
                                        </div>
                                        <div class="col-sm-4 task_assigned text-left" style="width:24%">
                                            Completed By
                                        </div>
                                    </div>
                            
                                </HeaderTemplate>
                                <ItemTemplate>
                            
                                    <div class="row gridbody">
                                        <div class="col-sm-4 item" style="width:22%">
                                            <asp:Label ID="grid_lbl_Title" runat="server" Text='<%# Eval("Title")%>'></asp:Label>
                                        </div>
                                        <div class="col-sm-2 item text-center" style="width:14%">
                                            <asp:Label ID="grid_lbl_Status" runat="server" Text='<%# Eval("TaskStatus")%>'></asp:Label>
                                        </div>
                                        <div class="col-sm-2 item text-center" style="width:16%">
                                            <asp:Label ID="grid_lbl_ApprovedDate" runat="server" Text='<%# Eval("ApprovedDate")%>'></asp:Label>
                                        </div>
                                        <div class="col-sm-4 item" style="width:24%">
                                            <asp:Label ID="grid_lbl_AssignedToName" runat="server" Text='<%# Eval("AssignedToName")%>'></asp:Label>
                                            <asp:TextBox ID="grid_txtOrderIntbl" data-toggle="tooltip" Text='<%# Eval("ApprovalOrder")%>' CssClass="form-control requiredInline ctrlHidden" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4 item" style="width:24%">
                                            <asp:Label ID="grid_lblApprovedBy" runat="server" Text='<%# Eval("ApprovedByName")%>'></asp:Label>
                                        </div>

                                    </div>
                                </ItemTemplate>





                            </asp:Repeater>

                            </div>


                            </div>


                          <div class="col-sm-12 div_task avoidbreak">
                          <h6 style="" class="formTextSmallSectionTitleContract approvalTitle">Executive Approval</h6>
                          <div class="taskList gridview" >
                        
                            <asp:Repeater ID="rpt_Tasks_Executive" runat="server">
                                



                            <HeaderTemplate>
                                    <div class="row gridheader">
                            
                                        <div class="col-sm-4 task_title" style="width:22%">
                                            Task
                                        </div>
                                        <div class="col-sm-2 task_status" style="width:14%">
                                            Status
                                        </div>
                                        <div class="col-sm-2 task_date" style="width:16%">
                                            Mofified
                                        </div>
                                        <div class="col-sm-4 task_assigned text-left" style="width:24%">
                                            Assigned To
                                        </div>
                                        <div class="col-sm-4 task_assigned text-left" style="width:24%">
                                            Completed By
                                        </div>
                                    </div>
                            
                                </HeaderTemplate>
                                <ItemTemplate>
                            
                                    <div class="row gridbody">
                                        <div class="col-sm-4 item" style="width:22%">
                                            <asp:Label ID="grid_lbl_Title" runat="server" Text='<%# Eval("Title")%>'></asp:Label>
                                        </div>
                                        <div class="col-sm-2 item text-center" style="width:14%">
                                            <asp:Label ID="grid_lbl_Status" runat="server" Text='<%# Eval("TaskStatus")%>'></asp:Label>
                                        </div>
                                        <div class="col-sm-2 item text-center" style="width:16%">
                                            <asp:Label ID="grid_lbl_ApprovedDate" runat="server" Text='<%# Eval("ApprovedDate")%>'></asp:Label>
                                        </div>
                                        <div class="col-sm-4 item" style="width:24%">
                                            <asp:Label ID="grid_lbl_AssignedToName" runat="server" Text='<%# Eval("AssignedToName")%>'></asp:Label>
                                            <asp:TextBox ID="grid_txtOrderIntbl" data-toggle="tooltip" Text='<%# Eval("ApprovalOrder")%>' CssClass="form-control requiredInline ctrlHidden" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4 item" style="width:24%">
                                            <asp:Label ID="grid_lblApprovedBy" runat="server" Text='<%# Eval("ApprovedByName")%>'></asp:Label>
                                        </div>

                                    </div>
                                </ItemTemplate>




                            </asp:Repeater>

                            </div>


                            </div>


                        </div>


                    


                </div>

                    <div class="col-sm-12" style="text-align:right; padding-right:30px;">

                            

                    </div>

                </div>

                    

                </asp:Panel>


    </div>
</div>



<div class="tab-content" style="display:none !important" id="ssswpTabContent">


<%-- Step 1 --%>

 
<%-- End Step 1 --%>
    
    <%-- Step 2 --%>
    <div class="tab-pane fade" id="step2">
        <div class="row top-container">
    <div class="top-inner-container">
        
        
    </div>
       </div>
   </div>
    
    <%-- END Step 2 --%>







<%-- Step 3 --%>
    
<%-- END Step 3 --%>


<div class="tab-pane fade" id="step4">
   
    <div class="row divcontainerComments ">
        <asp:Label ID="lblComments" runat="server" Text="(There is no comment)"></asp:Label>
    </div>

</div>


<%-- Step 5 --%>
<div class="tab-pane fade" id="step5">
   <div class="row divcontainerHistory">
      <asp:Label ID="lblHistory" runat="server" Text="(There is no history)"></asp:Label>
    </div>
</div>
<%-- END Step 5 --%>


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
<asp:HiddenField ID="HiddenField1" runat="server" />
<asp:HiddenField ID="HiddenField2" runat="server" />


<asp:Button ID="btnHiddenPrint" OnClientClick="return fillDataToTxt();" OnClick="btnHiddenPrint_Click" runat="server" Text="Print" />
<asp:TextBox ID="txtContentToPrint" CssClass="ContentToPrintOut" runat="server"></asp:TextBox>
<asp:TextBox ID="txtFileNameToInputWithoutExt" CssClass="FileNameToInputWithoutExt ctrlHidden" runat="server">Contracting-Plan</asp:TextBox>

<asp:TextBox ID="txtIfRevised" CssClass="ctrlHidden txtIfRevised" runat="server" Text="-1"></asp:TextBox>
<asp:TextBox ID="txtOrgMasteratt" CssClass="ctrlHidden txtOrgMasteratt" runat="server" Text="-1"></asp:TextBox>
<asp:TextBox ID="txtArvMasteratt" CssClass="ctrlHidden txtArvMasteratt" runat="server" Text="-1"></asp:TextBox>
<asp:TextBox ID="txtOrgContractatt" CssClass="ctrlHidden txtOrgContractatt" runat="server" Text="-1"></asp:TextBox>
<asp:TextBox ID="txtArvContractatt" CssClass="ctrlHidden txtArvContractatt" runat="server" Text="-1"></asp:TextBox>
<asp:TextBox ID="txtActionCompleted" CssClass="ctrlHidden txtActionCompleted" runat="server" Text="0"></asp:TextBox>

<asp:TextBox ID="txtHeaderOfCP" CssClass="ctrlHidden txtHeaderOfCP" runat="server" Text="Contracting-Plan"></asp:TextBox>
