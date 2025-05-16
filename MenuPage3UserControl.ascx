<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuPage3UserControl.ascx.cs" Inherits="BART.SP.OCR.CP.Web.MenuPage3.MenuPage3UserControl" %>


<script type="text/javascript" src="/_layouts/15/BART.SP.OCR.CP.Web/js/bootstrap-datepicker.min.js?refId=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>"></script>
<script type="text/javascript" src="/SiteAssets/AppCP/Core.js?refId=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>"></script>
<script type="text/javascript" src="/SiteAssets/AppCP/UI.js?versionview=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>"></script>
<script type="text/javascript" src="/SiteAssets/AppCP/Validation.js?versionview=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>"></script>
<script type="text/javascript" src="/SiteAssets/AppCP/LoadNEvents.js?versionview=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>"></script>

<link  rel="stylesheet" type="text/css" href="/_layouts/15/BART.SP.OCR.CP.Web/css/OriginalCSS.css?refId=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>" />
<link  rel="stylesheet" type="text/css" href="/_layouts/15/BART.SP.OCR.CP.Web/css/bootstrap-datepicker3.css?refId=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>" />
<link  rel="stylesheet" type="text/css" href="/SiteAssets/AppCP/Core.css?refId=<%=BART.SP.OCR.CP.Common.Settings.QueryStringJSCSS %>" />



<style>
    div.row{
        padding-top:5px;
        padding-bottom:5px;
    }
    .ctrlHidden
    {
        display:none !important;
    }
    #s4-ribbonrow
    {
        display:none !important;
    }
    .btn-saveInfo
    {
        float: right;
        background-color: #077496 !important;
        color: white !important;
        border: 1px solid !important;
        min-width: 80px !important;
        min-height: 36px;
    }
</style>
<script>
    function CloseDlg() {
        SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.OK);
    }
    $(document).ready(function () {
        try {
            var isSubmit = $('.txtIsSubmit').val();
            if (isSubmit == 'Completed')
            {
                setTimeout(function () {
                    $("input[id$='_btnHiddenPrint']").trigger('click');
                }, 500);
            }
            // --- Load
            LoadEvent();
        }
        catch (e) { }
    });
    function validate()
    {
        var isval = validateWithoutGeneralMss('formnewproject');
        if (isval >0)
            return false;
    }
    function LockTyping()
    {
        $('.pID').on('keydown keyup', function (e) {
            var hasCss = $(this).attr('class');
            if (hasCss.indexOf('noType') > -1) {
                var code = e.keyCode || e.which;
                if (code != '9') {
                    e.preventDefault();
                }
            }

        });
    }
    function LoadEvent()
    {
        $('.listdf').on('input', function () {
            var val = $(this).val();
            if (val.toUpperCase() === 'N/A')
                $(this).val('N/A');

        });

        $('.pName').on('input', function () {
            var val = this.value;
            var sub=$('#programNames option').filter(function () {
                return this.value.toUpperCase() === val.toUpperCase();
            });
            if (sub.length) {
                $.each(sub, function () {
                    var opt = $(this);
                    var key = opt.attr('pkey');
                    var val = opt.val();
                    $('.pName').val(val);
                    $('.pID').val(key);
                    $('.pID').css("background-color", "#eee");
                    $('.pID').addClass('noType');
                    LockTyping();

                   

                });

            }
            else {
                $('.pID').val('');
                $('.pID').css("background-color", "#fff");
                $('.pID').removeClass('noType');
            }
        });
    }
</script>


<div class="col-md-12" id="formnewproject">

    <div class="row">
    <div class="col-md-12">
        <asp:Label ID="lblCompleteMessage" CssClass="errMsg" ForeColor="#cc3300" runat="server" Text=""></asp:Label>        
    </div></div>


    <div class="row">
    <div class="col-md-12">
        <label><b>Program Name</b></label><span class="markRequired">*</span><span> (Type or choose N/A if your project does not belong to any program)</span>
        <asp:TextBox ID="txtProgramName" list="programNames" CssClass="form-control pName cssRequired" runat="server"></asp:TextBox>
        <asp:Label ID="lblDataListProgramName" runat="server" Text=""></asp:Label>
    </div>
    </div>

    <div class="row">
    <div class="col-md-12">
        <label><b>Program ID</b> </label><span class="markRequired">*</span> <span> (Type or choose N/A if there is no program ID)</span>
        <asp:TextBox ID="txtProgramID" list="programIDs" CssClass="form-control pID listdf cssRequired" runat="server"></asp:TextBox>
        <datalist id="programIDs">
            <option>N/A</option>
        </datalist>
    </div>
    </div>

    <div class="row">
    <div class="col-md-12">
        <label><b>Project Name</b> (Type or choose N/A if this project does not have a specific name)</label><span class="markRequired">*</span>
        <asp:TextBox ID="txtProjectName" list="projectNames" CssClass="form-control ProjectName listdf cssRequired" runat="server"></asp:TextBox>
        <datalist id="projectNames">
            <option>N/A</option>
        </datalist>
    </div>
    </div>

    <div class="row">
    <div class="col-md-12">
        <label> <b> Project ID</b></label><span class="markRequired">*</span><span> (Type or choose N/A if this project does not have a Project ID)</span>
        <asp:TextBox ID="txtProjectID" list="projectIDs" CssClass="form-control ProjectID listdf cssRequired" runat="server"></asp:TextBox>
        <datalist id="projectIDs">
            <option>N/A</option>
        </datalist>
    </div>
    </div>


    <div class="row">
        <div class="col-md-10">
               
    </div>
    <div class="col-md-2">
        <asp:Button ID="btnSubmit" CssClass="btn btn-saveInfo" OnClientClick="return validate();" OnClick="btnSubmit_Click" runat="server" Text="Save & Reload Page" />
    </div>

   

    </div>

    <asp:HiddenField ID="hdfAllProjects" runat="server" />
    <asp:TextBox ID="txtIsSubmit" CssClass="txtIsSubmit ctrlHidden" runat="server" Text="-1"></asp:TextBox>

</div>
<input type="button" id="_btnHiddenPrint" class="ctrlHidden" onclick="CloseDlg(); return false;" value="Closebox" />