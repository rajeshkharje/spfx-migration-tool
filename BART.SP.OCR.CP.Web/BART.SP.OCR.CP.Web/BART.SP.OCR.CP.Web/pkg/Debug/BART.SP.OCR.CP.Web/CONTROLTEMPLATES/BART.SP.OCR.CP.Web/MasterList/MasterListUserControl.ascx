<%@ Assembly Name="BART.SP.OCR.CP.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9abfeb7dc254e359" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MasterListUserControl.ascx.cs" Inherits="BART.SP.OCR.CP.Web.MasterList.MasterListUserControl" %>
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

<script>

    $(document).ready(function () {
        $('[data-toggle="tooltip"]').tooltip();
    });

    function pageLoad()
    {
        cancelEnterOnSinglineText();
        InitJsFunctions();
        routeActionAssigned();
        $('.rgFilterRow td input.rgFilterBox').addClass('form-control');
        $('.rgFilterRow td .RadComboBox.RadComboBox_Metro span.rcbInner').addClass('form-control');
        $('[data-toggle="tooltip"]').tooltip();
    }
    function InitJsFunctions() {
        setDefaultValforSearchFilter();
        NoRecordDisplay();
        $(".sswpTopSearch").blur(function () {
            var searchTextInput = $('.sswpTopSearch').val().trim();
            var currentSearchVal = $('.txtHiddenSearchVal').val().trim();
            if (searchTextInput != currentSearchVal) {
                {
                    $('.txtHiddenSearchVal').val(searchTextInput);
                    $('.btnsswpSearch').click();
               }
            }
        });
        $(".sswpTopSearch").on('keydown', function (e) {
            if (e.which == 13) {
                $('.btnsswpSearch').click();
              return false;
            }
        });
    }
    function removeFilterSelection() {
        try {
            $("input[id$='FromSubmittedDatePicker_dateInput']").val('');
            $("input[id$='ToSubmittedDatePicker_dateInput']").val('');
        } catch (e)
        {

        }
    }

</script>



    

<asp:UpdateProgress runat="server" ID="ContainerLoading" AssociatedUpdatePanelID="updatePanelContainer">
    <ProgressTemplate>
       <div class="col-md-12 container" style="height:1000px; display:block; background-color:#ddd; position:absolute; text-align:center; width:100%; margin-right:15px; padding-top:100px; opacity:0.5">
    <img src="/SiteCollectionImages/loading.gif" />
</div>  
    </ProgressTemplate>
</asp:UpdateProgress>



<asp:UpdatePanel runat="server" ID="updatePanelContainer">

<ContentTemplate>


<div class="container listdataPage">
<div class="row sswpsecondnav sub-nav">
        <div class="col-md-3 sswpbreadcrumb sub-nav__link" ><ol class="breadcrumb"><li class="breadcrumb-item"><a href="MyCP.aspx">Contracting Plan Application</a></li><li class="breadcrumb-item active">
            <asp:Label ID="lblActiveNav" runat="server" Text=""></asp:Label></li></ol></div>
        <div class="col-md-3">
            <a id="linkClearAllfilters" class="linkRemoveFilters" runat="server" style="margin-top:7px;float:right;color: #0072ce; cursor:pointer;" onserverclick="linkClearAllfilters_ServerClick"><i class="fa fa-times" aria-hidden="true" style="font-size:20px; margin-right:3px;"></i>Clear all filters</a>
            
        </div>
        <div class="col-md-6" style="padding-right: 5px;">
            
            <div class="custom-search-input-SSWP"><div class="input-group col-md-12">
                    <input type="text" runat="server"  id="txtSearch" class="search-query form-control sswpTopSearch" placeholder="Program, Project Name, Project ID, Project Manager or Initiator" style="border-bottom-right-radius: 4px; border-top-right-radius: 4px;">
                    <span class="input-group-btn">
                        <button class="btnsswpSearch" runat="server" onserverclick="btnSSWPSearch_ServerClick" id="btnSSWPSearch" type="button" style="">
                            <span>
                                <i class="fa fa-search" aria-hidden="true"></i>
                            </span>
                        </button>
                    </span>
                    <div style="display:none !important;">
                      <asp:Button runat="server" ID="btnDefaultSearch" OnClick="btnDefaultSearch_Click" Text="HiddenSearch"/>
                    </div>
                </div>
             </div>
        </div>
    </div>

<div class="row">
<telerik:RadGrid ID="gridItem1" OnPreRender="gridItem1_PreRender" OnItemDataBound="gridItem_DataBound" OnNeedDataSource="radGrid_NeedDataSource" runat="server" OnItemCommand="gridItem1_ItemCommand" RenderMode="Lightweight" HeaderStyle-Font-Bold="true"  AllowSorting="True" AllowAutomaticInserts="True" AutoGenerateColumns="False" AllowAutomaticDeletes="True" 
    AllowFilteringByColumn="True" AllowAutomaticUpdates="True" >
    <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
        <MasterTableView CommandItemDisplay="None" AllowPaging="true" AllowFilteringByColumn="true" PageSize="15" EnableColumnsViewState="true" InsertItemDisplay="Bottom" InsertItemPageIndexAction="ShowItemOnFirstPage" PagerStyle-Mode="NumericPages" PagerStyle-VerticalAlign="NotSet" PagerStyle-Visible="True">
            <Columns>

                <telerik:GridBoundColumn DataField="ProgramDes" AllowFiltering="true" ItemStyle-CssClass="gridStatus gridProgramDes" HeaderStyle-CssClass="gridStatus gridProgramDes" FilterControlAltText="Filter By Program Name" HeaderText="Program" MaxLength="150" UniqueName="ProgramDes">
                    <FilterTemplate>
                                    <telerik:RadComboBox RenderMode="Lightweight" Skin="Metro" ID="ddlProgramNameFilter" Width="100%" AppendDataBoundItems="true" CssClass="sswpFilterddlistRequestor ddl100" DataTextField="ProgramDes" DataValueField="ProgramDes" runat="server" OnClientSelectedIndexChanged="ProgramNameFilterComboIndexChanged" InputCssClass="form-control" LabelCssClass="form-control">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="All" Value="" />
                                        </Items>
                                    </telerik:RadComboBox>
                                    <telerik:RadScriptBlock ID="RadScriptBlockRProgramName" runat="server">
                                        <script type="text/javascript">
                                            function ProgramNameFilterComboIndexChanged(sender, args) {
                                                var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                tableView.filter("ProgramDes", args.get_item().get_value(), "EqualTo");
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="ProjectID" AllowFiltering="false" ItemStyle-CssClass="gridStatus" FilterControlAltText="Filter Project ID column" HeaderText="Project ID" MaxLength="150" UniqueName="ProjectID">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn ItemStyle-CssClass="gridtitle" HeaderStyle-CssClass="gridtitle" SortExpression="ProjectName"  AllowFiltering="false"  ShowFilterIcon="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" FilterControlAltText="Filter Project Name column" HeaderText="Project Name" UniqueName="ProjectName">
                    <ItemTemplate>
                        <a data-toggle="tooltip" href="<%# BART.SP.OCR.CP.Common.ProjectUtilities.ViewItemUrl(Eval("MasterID"))%>"  class="sswpTitleLink" title="<%# Eval("ProjectID") %>"> <%# BART.SP.OCR.CP.Common.ProjectUtilities.DisplayStringonGrid(Eval("ProjectName"))%></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridBoundColumn DataField="DateSubmitted" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-CssClass="gridsubmittedDate" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  FilterControlAltText="Filter by Date Submitted" HeaderText="Submitted" MaxLength="150" UniqueName="DateSubmitted">

                    <FilterTemplate>
                        <telerik:RadDatePicker RenderMode="Lightweight" ID="FromSubmittedDatePicker" CssClass="calSubmittedFrom" runat="server" Width="100px" ClientEvents-OnDateSelected=""
                            FocusedDate='<%# DateTime.Now%>'  DbSelectedDate='<%# this.startDate %>' />
                        <telerik:RadDatePicker RenderMode="Lightweight" CssClass="calSubmittedTo" ID="ToSubmittedDatePicker" runat="server" Width="100px" ClientEvents-OnDateSelected="ToDateSelected"
                            FocusedDate='<%# DateTime.Now%>'  DbSelectedDate='<%# this.endDate %>' />
                            <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                <script type="text/javascript">
                                    function FromDateSelected(sender, args) {
                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                    var ToPicker = $find('<%# ((GridItem)Container).FindControl("ToSubmittedDatePicker").ClientID %>');
 
                                    var fromDate = FormatSelectedDate(sender);
                                    var toDate = FormatSelectedDate(ToPicker);
 

                                    var tdt = new Date(toDate);
                                    tdt.setDate(tdt.getDate() + 1);
                                    var toFilter = (tdt.getMonth() + 1) + '/' + tdt.getDate() + '/' + tdt.getFullYear();

                                    tableView.filter("DateSubmitted", fromDate + " " + toFilter, "Between");
 
                                }
                                function ToDateSelected(sender, args) {
                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                    var FromPicker = $find('<%# ((GridItem)Container).FindControl("FromSubmittedDatePicker").ClientID %>');
 
                                    var fromDate = FormatSelectedDate(FromPicker);
                                    var toDate = FormatSelectedDate(sender);

                                    var tdt = new Date(toDate);
                                    tdt.setDate(tdt.getDate() + 1);
                                    var toFilter = (tdt.getMonth() + 1) + '/' + tdt.getDate() + '/' + tdt.getFullYear();

                                    tableView.filter("DateSubmitted", fromDate + " " + toFilter, "Between");
                                }
                                function FormatSelectedDate(picker) {
                                    var date = picker.get_selectedDate(); 
                                    var dateInput = picker.get_dateInput();
                                    var formattedDate = dateInput.get_dateFormatInfo().FormatDate(date, dateInput.get_displayDateFormat());
 
                                    return formattedDate;
                                }
                                </script>
                            </telerik:RadScriptBlock>
                        </FilterTemplate>
                    </telerik:GridBoundColumn>

                   <telerik:GridBoundColumn DataField="SponsorDepartment" AllowFiltering="true"   ItemStyle-CssClass="gridDept" HeaderStyle-CssClass="gridDept" FilterControlAltText="Filter By Sponsor Department" HeaderText="Department" MaxLength="150" UniqueName="SponsorDepartment">
                    <FilterTemplate>
                                    <telerik:RadComboBox RenderMode="Lightweight" Skin="Metro" ID="ddlDeptNoFilter" AppendDataBoundItems="true" CssClass="sswpFilterddlistRequestor" DataTextField="SponsorDepartment" DataValueField="SponsorDepartment" runat="server" OnClientSelectedIndexChanged="DeptNoFilterComboIndexChanged" InputCssClass="form-control" LabelCssClass="form-control">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="All" Value="" />
                                        </Items>
                                    </telerik:RadComboBox>
                                    <telerik:RadScriptBlock ID="RadScriptBlockRDeptNo" runat="server">
                                        <script type="text/javascript">
                                            function DeptNoFilterComboIndexChanged(sender, args) {
                                                var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                tableView.filter("SponsorDepartment", args.get_item().get_value(), "EqualTo");
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="Requester_Assigned" ItemStyle-CssClass="gridInit" HeaderStyle-CssClass="gridInit"  AllowFiltering="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FilterControlAltText="Filter Initiator column" HeaderText="Initiator" MaxLength="150" UniqueName="Requester">

                    <FilterTemplate>
                                    <telerik:RadComboBox RenderMode="Lightweight" Skin="Metro"  ID="ddlRequestorFilter" CssClass="sswpFilterddlistRequestor" AppendDataBoundItems="true" DataTextField="Requester_Assigned" DataValueField="Requester_Assigned" runat="server" OnClientSelectedIndexChanged="RequestorFilterComboIndexChanged" InputCssClass="form-control" LabelCssClass="form-control">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="All" Value="" />
                                        </Items>
                                    </telerik:RadComboBox>
                                    <telerik:RadScriptBlock ID="RadScriptBlockRequestor" runat="server">
                                        <script type="text/javascript">
                                            function RequestorFilterComboIndexChanged(sender, args) {
                                                var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                tableView.filter("Requester", args.get_item().get_value(), "EqualTo");
                                        }
                                        </script>
                                    </telerik:RadScriptBlock>
                    </FilterTemplate>
               
                </telerik:GridBoundColumn>

                 <telerik:GridBoundColumn DataField="OCRAnalyst_Assigned" ItemStyle-CssClass="gridInit" HeaderStyle-CssClass="gridInit"  AllowFiltering="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FilterControlAltText="Filter OCR Analyst" HeaderText="OCR Analyst" MaxLength="150" UniqueName="OCRAnalyst">

                    <FilterTemplate>
                                    <telerik:RadComboBox RenderMode="Lightweight" Skin="Metro"  ID="ddlOCRAnalyst" CssClass="sswpFilterddlistRequestor" AppendDataBoundItems="true" DataTextField="OCRAnalyst_Assigned" DataValueField="OCRAnalyst_Assigned" runat="server" OnClientSelectedIndexChanged="OCRAnalystFilterComboIndexChanged" InputCssClass="form-control" LabelCssClass="form-control">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="All" Value="" />
                                        </Items>
                                    </telerik:RadComboBox>
                                    <telerik:RadScriptBlock ID="RadScriptBlock2" runat="server">
                                        <script type="text/javascript">
                                            function OCRAnalystFilterComboIndexChanged(sender, args) {
                                                var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                tableView.filter("OCRAnalyst", args.get_item().get_value(), "EqualTo");
                                        }
                                        </script>
                                    </telerik:RadScriptBlock>
                    </FilterTemplate>
               
                </telerik:GridBoundColumn>


                 <telerik:GridBoundColumn DataField="SponsorProjectManager" ItemStyle-CssClass="gridPM" HeaderStyle-CssClass="gridPM"  AllowFiltering="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FilterControlAltText="Filter PM column" HeaderText="PM" MaxLength="150" UniqueName="SponsorProjectManager">
                    <FilterTemplate>
                                    <telerik:RadComboBox RenderMode="Lightweight" Skin="Metro"  ID="ddlPM" CssClass="sswpFilterddlistRequestor" AppendDataBoundItems="true" DataTextField="SponsorProjectManager" DataValueField="SponsorProjectManager" runat="server" OnClientSelectedIndexChanged="PMFilterComboIndexChanged" InputCssClass="form-control" LabelCssClass="form-control">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="All" Value="" />
                                        </Items>
                                    </telerik:RadComboBox>
                                    <telerik:RadScriptBlock ID="RadScriptBlockddlPM" runat="server">
                                        <script type="text/javascript">
                                            function PMFilterComboIndexChanged(sender, args) {
                                                var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                tableView.filter("SponsorProjectManager", args.get_item().get_value(), "EqualTo");
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                    </FilterTemplate>
                </telerik:GridBoundColumn>


                <telerik:GridBoundColumn DataField="Status" AllowFiltering="true"   ItemStyle-CssClass="gridStatus" HeaderStyle-CssClass="gridStatus" FilterControlAltText="Filter Status column" HeaderText="Status" MaxLength="150" UniqueName="Status">
                    <FilterTemplate>
                                    <telerik:RadComboBox RenderMode="Lightweight" Skin="Metro" ID="ddlStatusNoFilter" AppendDataBoundItems="true" CssClass="sswpFilterddlistRequestor" DataTextField="Status" DataValueField="Status" runat="server" OnClientSelectedIndexChanged="StatusFilterComboIndexChanged" InputCssClass="form-control" LabelCssClass="form-control">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="All" Value="" />
                                        </Items>
                                    </telerik:RadComboBox>
                                    <telerik:RadScriptBlock ID="RadScriptBlockStatus" runat="server">
                                        <script type="text/javascript">
                                            function StatusFilterComboIndexChanged(sender, args) {
                                                var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                tableView.filter("Status", args.get_item().get_value(), "EqualTo");
                                        }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                </telerik:GridBoundColumn>

                
                 <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-CssClass="grid sswpEditTDLink" ShowFilterIcon="false" HeaderText="Action" HeaderStyle-ForeColor="#993300" UniqueName="EditInfo">
                    <ItemTemplate>
                        <a class="ctrlHidden"></a>
                        <asp:Panel ID="grid_pnlEdit" runat="server" Visible='<%# BART.SP.OCR.CP.Common.ProjectUtilities.EditButtonVisibility(this.hdfPageState.Value,Eval("Status"),Eval("isEditable"))%>'>
                            <a data-toggle="tooltip" href="<%# BART.SP.OCR.CP.Common.ProjectUtilities.EditItemURL(Eval("MasterID"))%>" class="sswpEditTitleLink" title="Edit CP"><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>
                        </asp:Panel>
                        </div>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                
            </Columns>
     </MasterTableView>
    <HeaderStyle Font-Bold="True"></HeaderStyle>
    </telerik:RadGrid>
</div>
</div>
<asp:HiddenField ID="hdfCurrentUserLogin" runat="server" />
<asp:HiddenField ID="hdfCurrentUserDisplayName" runat="server" />
<asp:TextBox runat="server" ID="hdfSearchVal" CssClass="txtHiddenSearchVal ctrlHidden"></asp:TextBox>
<asp:HiddenField ID="hdfPageState" runat="server" />
<asp:HiddenField ID="hdfUserRoles" runat="server" />

</ContentTemplate>

</asp:UpdatePanel>
