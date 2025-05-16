<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequesterWaitListUserControl.ascx.cs" Inherits="BART.SP.OCR.CP.Web.RequesterWaitList.RequesterWaitListUserControl" %>
<%@ Register Assembly="Telerik.Web.UI, Version=2016.2.504.45, Culture=neutral, PublicKeyToken=121fae78165ba3d4" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<script type="text/javascript" src="/_layouts/15/BART.SP.OCR.CP.Web/js/bootstrap-datepicker.min.js?refId=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>"></script>
<script type="text/javascript" src="/SiteAssets/AppCP/Core.js?refId=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>"></script>
<script type="text/javascript" src="/SiteAssets/AppCP/UI.js?versionview=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>"></script>
<script type="text/javascript" src="/SiteAssets/AppCP/Validation.js?versionview=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>"></script>
<script type="text/javascript" src="/SiteAssets/AppCP/LoadNEvents.js?versionview=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>"></script>

<link  rel="stylesheet" type="text/css" href="/_layouts/15/BART.SP.OCR.CP.Web/css/OriginalCSS.css?refId=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>" />
<link  rel="stylesheet" type="text/css" href="/_layouts/15/BART.SP.OCR.CP.Web/css/bootstrap-datepicker3.css?refId=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>" />
<link  rel="stylesheet" type="text/css" href="/SiteAssets/AppCP/Core.css?refId=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>" />

<style>
    .exportBtn {
        background-color:#0062cc !important;
        color:white !important;
        border-radius:8px !important;
        font-size:12px;
        float:right;
        margin:10px !important;
    }

</style>

<span>
    <%=BART.SP.OCR.CP.Common.ProjectUtilities.BuildTopMenu(Request.Url.ToString().ToLower())%>
</span>


<asp:Button ID="btnExport" runat="server" CssClass="btn btn-primary commitAction exportBtn" OnClick="btnExport_Click" Text="Export to Excel" />

<telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="false" OnNeedDataSource="RadGrid1_NeedDataSource">
    <ExportSettings>
        
    </ExportSettings>
    <MasterTableView>
        <Columns>
            <telerik:GridBoundColumn DataField="Program" AllowFiltering="false" FilterControlAltText="Filter Program column" HeaderText="Program" UniqueName="Program"></telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="Project" AllowFiltering="false" FilterControlAltText="Filter Project column" HeaderText="Project" UniqueName="Project"></telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="Project Name" AllowFiltering="false" FilterControlAltText="Filter Project Name column" HeaderText="Project Name" UniqueName="ProjectName"></telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="Submitted" AllowFiltering="false" FilterControlAltText="Filter Submitted column" HeaderText="Submitted" UniqueName="Submitted"></telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="Department" AllowFiltering="false" FilterControlAltText="Filter Department column" HeaderText="Department" UniqueName="Department"></telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="Initiator" AllowFiltering="false" FilterControlAltText="Filter Initiator column" HeaderText="Initiator" UniqueName="Initiator"></telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="OCR Analyst" AllowFiltering="false" FilterControlAltText="Filter OCR Analyst column" HeaderText="OCR Analyst" UniqueName="OCRAnalyst"></telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="PM" AllowFiltering="false" FilterControlAltText="Filter PM column" HeaderText="PM" UniqueName="PM"></telerik:GridBoundColumn>

            <telerik:GridBoundColumn DataField="Status" AllowFiltering="false" FilterControlAltText="Filter Status column" HeaderText="Status" UniqueName="Status"></telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="Final Concurrance Date" AllowFiltering="false" FilterControlAltText="Filter Final Concurrance Date column" HeaderText="Final Concurrance Date" UniqueName="FinalConcurranceDate"></telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="Contract No" AllowFiltering="false" FilterControlAltText="Filter Contract No column" HeaderText="Contract No" UniqueName="ContractNo"></telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="Dollar Amount" AllowFiltering="false" FilterControlAltText="Filter Dollar Amount column" HeaderText="Dollar Amount" UniqueName="DollarAmount"></telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="Funding Source" AllowFiltering="false" FilterControlAltText="Filter Funding Source column" HeaderText="Funding Source" UniqueName="PM"></telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="Target Completion Date" AllowFiltering="false" FilterControlAltText="Filter Target Completion Date column" HeaderText="Target Completion Date" UniqueName="TargetCompletionDate"></telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="Contract Status" AllowFiltering="false" FilterControlAltText="Filter Contract Status column" HeaderText="Contract Status" UniqueName="ContractStatus"></telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="Description" AllowFiltering="false" FilterControlAltText="Filter Description column" HeaderText="Description" UniqueName="Description"></telerik:GridBoundColumn>
        </Columns>
    </MasterTableView>
</telerik:RadGrid>