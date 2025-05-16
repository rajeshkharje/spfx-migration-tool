<%@ Assembly Name="BART.SP.OCR.CP.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9abfeb7dc254e359" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProxiesUserControl.ascx.cs" Inherits="BART.SP.OCR.CP.Web.Proxies.ProxiesUserControl" %>
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
    function pageLoad()
    {
        InitPicker();
        $('[data-toggle="tooltip"]').tooltip();
    }
    function InitPicker() {

        var date_input = $('div.input-group.date'); //our date input has the name "date"
        var container = $('.bootstrap-iso form').length > 0 ? $('.bootstrap-iso form').parent() : "body";
        var options = {
            format: 'mm/dd/yyyy',
            container: container,
            todayHighlight: true,
            autoclose: true,
        };
        date_input.datepicker(options);


        $('.divUserInputField').each(function (index) {
            InitaUserPickerSingle($(this).attr('id'));
        });
        createNewSaveAction();
        loadDeleteProxyInGrid();
        DeleteProxySelected();
        $('#ProxyDiv_TopSpan_InitialHelpText').html('Proxy');
    }
    function updateVal()
    {
        $('.lblCompleteMsg').html('');
        var valid = validateVal();
        if (valid == 0) {
            $('.btnNobox').html('No');
            getUserInfo('ProxyDiv', '<%= this.hdfProxyAddedLogin.ClientID %>');
            if ($('#' + '<%= this.hdfProxyAddedLogin.ClientID %>').val() == $('#' + '<%= this.hdfCurrentUserLogin.ClientID %>').val()) {
                callBox();
                $('.txtComfirmationMss').html('<span style="color:#cd3e10;">&nbsp; - &nbsp;Proxy and Requestor can not be same.</span><br>');
                $('.btnNobox').html('OK');
                return false;
            }
            else {


                $('.btnActionCustomCall').removeAttr('data-toggle');
                $('.btnActionCustomCall').removeAttr('data-target');
                $('.btnMustPrimaryActuallClickButton').click(); return false;
            }
        }
        else {
            callBox();
            $('.txtComfirmationMss').html('<span style="color:#cd3e10;">&nbsp; - &nbsp;Please check the highlighted field(s).</span><br>');
            $('.btnNobox').html('OK');
            return false;
        }
        
    }
    function callBox()
    {
        $('.btnActionCustomCall').attr('data-toggle', 'modal');
        $('.btnActionCustomCall').attr('data-target', '#myConfirmationModal');
        DefaultMessageBox();
    }
    function validateVal()
    {
        var inValid = 0;
        var errorMess;
        $(".cssRequired").each(function () {
            var lbl = $(this);
            var value = lbl.val().replace(' ', '');
            if (!value) {
                $(this).css({
                    "border-color": "#ef8969",
                    "border-width": "1px",
                    "border-style": "solid"
                });
                inValid = 1;

            }
            else {
                $(this).css({
                    "border-color": "#ccc",
                    "border-width": "1px",
                    "border-style": "solid"
                });
            }
        });
        if (!GetValueofPicker('ProxyDiv')) {
            $('#ProxyDiv').css({
                "border-color": "#ef8969",
                "border-width": "1px",
                "border-style": "solid"
            });
            inValid = 1;
        }
        else {
            $('#ProxyDiv').css({
                "border-color": "#ccc",
                "border-width": "1px",
                "border-style": "solid"
            });
        }
        $('.txtComfirmationMss').html(errorMess);
        return inValid;
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
        <div class="col-md-2 sswpbreadcrumb sub-nav__link" ><ol class="breadcrumb"><li class="breadcrumb-item"><a href="MyMDD.aspx">MDD</a></li><li class="breadcrumb-item active">Proxies</li></ol></div>
        <div class="col-md-8">
            <asp:LinkButton ID="lbtClearAllFilters" Visible="false" CssClass="linkRemoveFilters filterButtonClearAll" OnClientClick="return callRemoveFilterButtonClick();"  runat="server"><i class="fa fa-times" aria-hidden="true" style="font-size:20px; margin-right:3px;"></i>Clear all filters</asp:LinkButton>
        </div>
        <div class="col-md-2" style="padding-right: 5px;">
            
            <div class="custom-search-input-SSWP">
                <div class="input-group col-md-12">
                    
                    <asp:DropDownList ID="ddlSearchOption" AutoPostBack="true" OnSelectedIndexChanged="ddlSearchOption_SelectedIndexChanged" CssClass="form-control" runat="server">
                        <asp:ListItem Value="0">My Proxies</asp:ListItem>
                        <asp:ListItem Value="1">All Proxies</asp:ListItem>
                    </asp:DropDownList>
                      
                </div>
             </div>
        </div>
    </div>
 

    <div class="row">
        <p style="margin-top: 15px; margin-bottom: 2px; font-weight: 600; color: #337ab7;">
            ADD YOUR PROXY
        </p>

    </div>

    <div class="row" style="/* margin-bottom: 7px; *//* margin-left: -25px; */background-color: #fafafa;padding-bottom: 15px;padding-top: 15px;border-radius: 4px;border: 1px solid #e6e6e6;">

        <div class="col-md-12">
            
            <hr style="margin-top: 2px; margin-bottom: 2px; border-top: none;">
        </div>
           <div class="col-md-3">
            <div class="input-group">
                <span class="input-group-addon"><i class="fa fa-user-o" aria-hidden="true"></i></span>
                <div id="ProxyDiv" class="form-control divUserInputField"></div>
            </div>
          </div>
          <div class="col-md-2">
            <div class="input-group date" data-provide="datepicker">
                        <asp:TextBox ID="txtStartDate" data-toggle="tooltip" placeholder="Start Date (mm/dd/yyyy)" CssClass="form-control cssRequired" runat="server"></asp:TextBox>
                        <span class="input-group-addon"><i class="fa fa-calendar" aria-hidden="true"></i></span>
            </div>
          </div>
        <div class="col-md-2">
            <div class="input-group date" data-provide="datepicker">
                        <asp:TextBox ID="txtExpiredDate" data-toggle="tooltip" placeholder="End Date (mm/dd/yyyy)" CssClass="form-control cssRequired" runat="server"></asp:TextBox>
                        <span class="input-group-addon"><i class="fa fa-calendar" aria-hidden="true"></i></span>
            </div>
          </div>
          <div class="col-md-1">
              <asp:Button ID="btnAddProx" runat="server" OnClientClick="return updateVal();" CssClass="btn btn-primary btnActionCustomCall btnMustPrimary" Text="Add Proxy" />
              <asp:Button ID="btnAddProxClickHidden" runat="server" CssClass="btnMustPrimaryActuallClickButton" OnClick="btnAddProx_Click" Text="Add as Your Proxy" />
          </div>
          <div class="col-md-4" style=" padding-top: 7px;">
                <asp:Label ID="lblCompleteMessage" CssClass="lblCompleteMsgProxy" runat="server" Text=""></asp:Label>
          </div>
        
    </div>


    <div class="row">
        <div class="col-md-12" style="/* margin-top: 10px; */padding-left: 0px;">
            <p style="margin-top: 25px; margin-bottom: 0px; font-weight: 600; color: #337ab7;">
                PROXIES LIST
            </p>
            <hr style="margin-top: 2px; margin-bottom: 2px; border-top: none;">
        </div>
    </div>


<div class="row">
<telerik:RadGrid ID="gridProxy" OnPreRender="gridProxy_PreRender" Skin="Default" OnNeedDataSource="radGrid_NeedDataSource" runat="server" OnItemCommand="gridSSWP_ItemCommand" RenderMode="Lightweight" HeaderStyle-Font-Bold="true"  AllowSorting="True" AllowAutomaticInserts="True" AutoGenerateColumns="False" AllowAutomaticDeletes="True" 
    AllowFilteringByColumn="True" AllowAutomaticUpdates="True" OnItemDataBound="gridSSWP_ItemDataBound" >
    <GroupingSettings CollapseAllTooltip="Collapse all groups" CaseSensitive="false"></GroupingSettings>
        <MasterTableView CommandItemDisplay="None" AllowPaging="true" AllowFilteringByColumn="true" PageSize="15" EnableColumnsViewState="true" InsertItemDisplay="Bottom" InsertItemPageIndexAction="ShowItemOnFirstPage" PagerStyle-Mode="NumericPages" PagerStyle-VerticalAlign="NotSet" PagerStyle-Visible="True">
            <Columns>
                <telerik:GridBoundColumn DataField="PrimaryUser"  AllowFiltering="true" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" FilterControlAltText="Filter Primary User column" HeaderText="Requestor" MaxLength="150" UniqueName="PrimaryUser">
                
                    <FilterTemplate>
                        <telerik:RadComboBox RenderMode="Lightweight" Skin="Metro"  ID="ddlPrimaryUserFilter" CssClass="sswpFilterddlistPrimaryUser" AppendDataBoundItems="true" DataTextField="PrimaryUser" DataValueField="PrimaryUser" runat="server" OnClientSelectedIndexChanged="PrimaryUserFilterComboIndexChanged" InputCssClass="form-control" LabelCssClass="form-control">
                            <Items>
                                <telerik:RadComboBoxItem Text="All" Value="" />
                            </Items>
                        </telerik:RadComboBox>
                        <telerik:RadScriptBlock ID="RadScriptBlockPrimaryUser" runat="server">
                            <script type="text/javascript">
                                function PrimaryUserFilterComboIndexChanged(sender, args) {
                                    $('.lblCompleteMsg').html('');
                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                    tableView.filter("PrimaryUser", args.get_item().get_value(), "EqualTo");
                            }
                            </script>
                        </telerik:RadScriptBlock>
                    </FilterTemplate>
                </telerik:GridBoundColumn>
               

                <telerik:GridBoundColumn DataField="Proxy"  AllowFiltering="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" FilterControlAltText="Filter Proxy column" HeaderText="Proxy" MaxLength="150" UniqueName="Proxy">
                
                    <FilterTemplate>
                        <telerik:RadComboBox RenderMode="Lightweight" Skin="Metro"  ID="ddlProxyFilter" CssClass="sswpFilterddlistProxy" AppendDataBoundItems="true" DataTextField="Proxy" DataValueField="Proxy" runat="server" OnClientSelectedIndexChanged="ProxyFilterComboIndexChanged" InputCssClass="form-control" LabelCssClass="form-control">
                            <Items>
                                <telerik:RadComboBoxItem Text="All" Value="" />
                            </Items>
                        </telerik:RadComboBox>
                        <telerik:RadScriptBlock ID="RadScriptBlockProxy" runat="server">
                            <script type="text/javascript">
                                function ProxyFilterComboIndexChanged(sender, args) {
                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                    tableView.filter("Proxy", args.get_item().get_value(), "EqualTo");
                            }
                            </script>
                        </telerik:RadScriptBlock>
                    </FilterTemplate>
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="StartDate" ItemStyle-CssClass="" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-CssClass="" AllowFiltering="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" FilterControlAltText="Filter Start Date" HeaderText="Start Date" MaxLength="150" UniqueName="StartDate">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="EndDate" ItemStyle-CssClass="" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="" DataFormatString="{0:MM/dd/yyyy}" AllowFiltering="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" FilterControlAltText="Filter End Date" HeaderText="End Date" MaxLength="150" UniqueName="EndDate">
                </telerik:GridBoundColumn>

                 <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-CssClass="sswpEditTDDelete" ItemStyle-Width="30" HeaderStyle-Width="30" ShowFilterIcon="false" HeaderText="Delete" HeaderStyle-ForeColor="#993300" UniqueName="DeleteProxyCol">
                    <ItemTemplate>
                        <span data-toggle="tooltip" title="Delete Proxy"><asp:LinkButton runat="server" OnClientClick="DeleteProxy();" ID="deleteProx" CssClass="sswpDeleteProx btnAction"><i class="fa fa-trash-o" aria-hidden="true"></i></asp:LinkButton></span>
                        <asp:Button ID="btnActuallDeleteHidden" CommandArgument='<%#Eval("ID")%>' CommandName="DeleteProxyItem" CssClass="actualClickAction" runat="server" Text="Delete" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

            </Columns>
     </MasterTableView>
    <HeaderStyle Font-Bold="True"></HeaderStyle>
    </telerik:RadGrid>
</div>
</div>
<asp:HiddenField ID="hdfCurrentUserLogin" runat="server" />
<asp:HiddenField ID="hdfProxyAddedLogin" runat="server" />


    <div style="display:none !important">
        <asp:Button ID="btnClearAll" CssClass="btnClearAllSearches" OnClick="btnClearAll_Click" runat="server" Text="Clear" />
    </div>




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
                    <a href="#" id="btnCommitSaveProxyDeletion" keyvalue="-1" class="btn btn-primary commitAction deleteProxybtn" title="Save Changes" data-dismiss="modal">
                        Yes
                    </a>
                    <button type="button" class="btn btn-primary btnDismissDlg btnNobox"  data-dismiss="modal" title="close dialog box">No</button>
                </div>
            </div>
        </div>
</div>
</ContentTemplate>
</asp:UpdatePanel>
