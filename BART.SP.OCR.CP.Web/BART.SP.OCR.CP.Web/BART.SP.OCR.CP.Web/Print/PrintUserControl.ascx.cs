using BART.SP.OCR.CP.Base;
using BART.SP.OCR.CP.Common;
using BART.SP.OCR.CP.Model;
using EvoPdf;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace BART.SP.OCR.CP.Web.Print
{
    public partial class PrintUserControl : ProjectUserControlBase
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString[Settings.TabQueryString] != null)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString[Settings.TabQueryString])))
                        {
                            this.hdfCurrentTab.Text = string.Format("#{0}", Convert.ToString(Request.QueryString[Settings.TabQueryString]));
                        }
                    }
                    catch (Exception ex)
                    {
                        //
                    }
                }
                this.LoadAll();
            }
            //
            this.lblNoofContract.Text = Convert.ToString(Convert.ToInt32(this.hdfNoOfContracts.Value) + 1);
        }

        private void LoadAll()
        {
            try
            {
                using (SPSite dSite = new SPSite(this.DataSiteURL))
                {
                    using (SPWeb dWeb = dSite.OpenWeb(this.DataWebRelativeURL))
                    {
                        this.loadAllDefaultInfo(dWeb);
                        this.LoadHistoryNComments(dWeb, this.CurrentMainObject);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ThrowError(ex.ToString(), this.lblError, this.pnlErrorMsg, this.pnlSuccessMsg);
            }
        }
        private void LoadHistoryNComments(SPWeb dWeb, MainObject obj)
        {
            this.LoadApproverCommentsHistoryNoneDraft(dWeb, obj, this.lblComments, this.lblHistory);
        }
        private bool IfRevisedCopy()
        {
            if (Request.QueryString[ProjectSettings.QueryRevise] != null)
            {
                if (Convert.ToString(Request.QueryString[ProjectSettings.QueryRevise]).Trim() == ProjectSettings.QueryReviseValue.Trim())
                {
                    return true;
                }
            }
            return false;
        }
        private void loadAllDefaultInfo(SPWeb web)
        {
            try
            {
                // --------
                this.hdf_MainObjID.Value = this.ReportId;
                this.hdfCurrentLogin.Value = this.HostedWeb.CurrentUser.LoginName;
                if (!string.IsNullOrEmpty(this.hdf_MainObjID.Value))
                {
                    this.CurrentMainObject = new MainObject(web, this.hdf_MainObjID.Value.Trim(), true);
                    string userLevel = string.Empty;
                    SPListItem mainItem = this.CurrentMainObject.getCurrentReportItem(web);
                    //------
                    this.CurrentMainObject.Requester = this.HostedWeb.CurrentUser;
                    //this.CurrentMainObject.RequesterName = this.HostedWeb.CurrentUser.Name;

                    this.CurrentMainObject.UserCreated = this.HostedWeb.CurrentUser;
                    this.CurrentMainObject.UserModified = this.HostedWeb.CurrentUser;

                    //this.lbl_RequesterName.Text = this.HostedWeb.CurrentUser.Name;

                    this.hdfInitialStatusValue.Value = this.CurrentMainObject.Status;

                    //---- Filter Task List
                    var staffTasks = this.CurrentMainObject.Tasks.Where(p => p.ApprovalLevel == ProjectSettings.ApprovalLevelStaff);
                    var managementTasks = this.CurrentMainObject.Tasks.Where(p => p.ApprovalLevel == ProjectSettings.ApprovalLevelManagement);
                    var executiveTasks = this.CurrentMainObject.Tasks.Where(p => p.ApprovalLevel == ProjectSettings.ApprovalLevelExecutive);

                    this.rpt_Tasks_Staff.DataSource = staffTasks;
                    this.rpt_Tasks_Staff.DataBind();

                    this.rpt_Tasks_Management.DataSource = managementTasks;
                    this.rpt_Tasks_Management.DataBind();

                    this.rpt_Tasks_Executive.DataSource = executiveTasks;
                    this.rpt_Tasks_Executive.DataBind();
                    //-------------------------------------------------------
                    //this.loadDepartmentsToList(web, this.ddl_SponsorDepartment);
                    ////--------------------------------------------------------
                    //this.loadProjectsToList(web, this.ddlProjectList, this.ddl_ProgramName);

                    PropertyInfo[] properties = this.CurrentMainObject.GetType().GetProperties();

                    this.LoadServiceType(this.CurrentMainObject.ServiceType, this.cbx_ServiceType);

                    foreach (var propertyInfo in properties)
                    {
                        this.LoadControlFields(propertyInfo, this.Controls);
                        this.LoadControlFields(propertyInfo, this.pnlStep1.Controls);
                        this.LoadControlFields(propertyInfo, this.pnl_OCRGeneralInfo.Controls);
                        // ------------------------------------------------------------------------------------------------------------------------
                    }
                    //this.ddlProjectList.SelectedValue = string.Format("{0}|{1}", this.CurrentMainObject.ProjectID, this.CurrentMainObject.ProgramName);
                    // Date time and user fields 
                    this.txt_KickoffMeetingDate.Text = ProjectUtilities.DisplayDateTimeMMDDYYYY(this.CurrentMainObject.KickoffMeetingDate);
                    string dateSM = ProjectUtilities.DisplayDateTimeMMDDYYYY(this.CurrentMainObject.DateSubmitted);
                    this.lbl_DateSubmitted.Text = string.IsNullOrEmpty(dateSM) ? "N/A" : dateSM;
                    //-------------
                    // --------------------------------------------------------------------------------------------------------------------------
                    this.DisplayAttachmentList(web, this.lblUploadedDocs, this.CurrentMainObject.ListAttachments);
                    //------------------------------------------------
                    this.hdfNoOfContracts.Value = Convert.ToString(this.CurrentMainObject.ContractDisplays.Count - 1);
                    //
                    ViewState[ProjectSettings.ViewStateContracts] = this.CurrentMainObject.ContractDisplays;
                    //this.AddHiddenContracts();
                    this.rpt_Contracts.DataSource = this.ContractDisplays;
                    this.rpt_Contracts.DataBind();
                    //-----------------------------------------------
                    this.txtHeaderOfCP.Text = string.Format("{0}-{1}",this.CurrentMainObject.ProjectName, this.CurrentMainObject.ProjectID);
                    if (this.IfRevisedCopy())
                    {
                        this.txtIfRevised.Text = ProjectSettings.MarkIfRevised;
                        this.txtOrgMasteratt.Text = string.Format("/{0}/{1}/{2}/", ProjectSettings.DataSiteRelativeURL.Trim(), ProjectSettings.DataWebRelativeURL, ProjectSettings.SPListAttachment.Trim());
                        this.txtOrgContractatt.Text = string.Format("/{0}/{1}/{2}/", ProjectSettings.DataSiteRelativeURL.Trim(), ProjectSettings.DataWebRelativeURL, ProjectSettings.SPListAttachmentContract.Trim());
                        //-------------------------------------------------
                        string cVersion=string.IsNullOrEmpty(this.CurrentMainObject.ApprovalVersion) || this.CurrentMainObject.ApprovalVersion=="0" ? "1" : string.Format(this.CurrentMainObject.ApprovalVersion);
                        this.txtArvMasteratt.Text= string.Format("/{0}/{1}/{2}/{3}/{4}/", ProjectSettings.ArchivedDocsSiteRelativeURL.Trim(), ProjectSettings.ArchivedDocsWebRelativeURL, ProjectSettings.SPListAttachment_Version.Trim()
                            ,this.CurrentMainObject.MasterID,cVersion);
                        this.txtArvContractatt.Text = string.Format("/{0}/{1}/{2}/{3}/{4}/", ProjectSettings.ArchivedDocsSiteRelativeURL.Trim(), ProjectSettings.ArchivedDocsWebRelativeURL, ProjectSettings.SPListAttachmentContract_Version.Trim()
                            ,this.CurrentMainObject.MasterID,cVersion); 

                    }

                    if (this.CurrentMainObject.DateCreated == null || this.CurrentMainObject.DateCreated < ProjectSettings.CB5MChangeDate)
                    {
                        this.cb_OCROver10M_Yes.Text = "Construction contract over $5M ?";
                        this.cb_OCROver10M_Yes.ToolTip = "Check this box if this contracting plan includes a construction contract that is $5M or more. This will notify the Labor Compliance Unit";
                    }

                }
            }
            catch (Exception ex)
            {
                ProjectUtilities.LogError(ex.ToString());
            }
        }
        //
        protected void btnHiddenPrint_Click(object sender, EventArgs e)
        {
            ConvertToPDFfunction();
        }

        void htmlToPdfConverter_PrepareRenderPdfPageEvent(PrepareRenderPdfPageParams eventParams)
        {
            // Set the header visibility in first, odd and even pages
            if (eventParams.PageNumber == 1)
                eventParams.Page.ShowHeader = false;
            else
                eventParams.Page.ShowHeader = true;
        }
        private PdfPageOrientation SelectedPdfPageOrientation(int type)
        {
            return (type == 0) ?
                PdfPageOrientation.Portrait : PdfPageOrientation.Landscape;
        }
        private PdfPageSize SelectedPdfPageSize(string val)
        {
            switch (val)
            {
                case "A0":
                    return PdfPageSize.A0;
                case "A1":
                    return PdfPageSize.A1;
                case "A10":
                    return PdfPageSize.A10;
                case "A2":
                    return PdfPageSize.A2;
                case "A3":
                    return PdfPageSize.A3;
                case "A4":
                    return PdfPageSize.A4;
                case "A5":
                    return PdfPageSize.A5;
                case "A6":
                    return PdfPageSize.A6;
                case "A7":
                    return PdfPageSize.A7;
                case "A8":
                    return PdfPageSize.A8;
                case "A9":
                    return PdfPageSize.A9;
                case "ArchA":
                    return PdfPageSize.ArchA;
                case "ArchB":
                    return PdfPageSize.ArchB;
                case "ArchC":
                    return PdfPageSize.ArchC;
                case "ArchD":
                    return PdfPageSize.ArchD;
                case "ArchE":
                    return PdfPageSize.ArchE;
                case "B0":
                    return PdfPageSize.B0;
                case "B1":
                    return PdfPageSize.B1;
                case "B2":
                    return PdfPageSize.B2;
                case "B3":
                    return PdfPageSize.B3;
                case "B4":
                    return PdfPageSize.B4;
                case "B5":
                    return PdfPageSize.B5;
                case "Flsa":
                    return PdfPageSize.Flsa;
                case "HalfLetter":
                    return PdfPageSize.HalfLetter;
                case "Ledger":
                    return PdfPageSize.Ledger;
                case "Legal":
                    return PdfPageSize.Legal;
                case "Letter":
                    return PdfPageSize.Letter;
                case "Letter11x17":
                    return PdfPageSize.Letter11x17;
                case "Note":
                    return PdfPageSize.Note;
                default:
                    return PdfPageSize.Letter;
            }
        }
        private void DrawHeader(HtmlToPdfConverter htmlToPdfConverter, bool drawHeaderLine)
        {

            // Create a text element with page numbering place holders &p; and & P;
            TextElement headerText = new TextElement(0, 10, string.Format("{0} (cont.)", this.txtHeaderOfCP.Text.Trim()), new System.Drawing.Font(new System.Drawing.FontFamily("Times New Roman"), 10, System.Drawing.GraphicsUnit.Point));

            // Align the text at the right of the footer
            headerText.TextAlign = HorizontalTextAlign.Left;

            // Set page numbering text color
            headerText.ForeColor = Color.Black;

            // Embed the text element font in PDF
            headerText.EmbedSysFont = true;
            htmlToPdfConverter.PdfHeaderOptions.HeaderHeight = 45;
            // Add the text element to footer
            htmlToPdfConverter.PdfHeaderOptions.AddElement(headerText);

            /*
            string headerHtmlUrl = "<span>" + this.hdfHeaderVal.Value + "</span>";//Server.MapPath("~/DemoAppFiles/Input/HTML_Files/Header_HTML.html");

            // Set the header height in points
            htmlToPdfConverter.PdfHeaderOptions.HeaderHeight = 30;

            // Set header background color
            htmlToPdfConverter.PdfHeaderOptions.HeaderBackColor = Color.White;
            // Create a HTML element to be added in header
            HtmlToPdfElement headerHtml = new HtmlToPdfElement(headerHtmlUrl, string.Empty);

            // Set the HTML element to fit the container height
            headerHtml.FitHeight = true;
            //headerHtml.FitWidth = true;

            // Add HTML element to header
            htmlToPdfConverter.PdfHeaderOptions.AddElement(headerHtml);*/


            if (drawHeaderLine)
            {
                //// Calculate the header width based on PDF page size and margins
                //float headerWidth = htmlToPdfConverter.PdfDocumentOptions.PdfPageSize.Width -
                //            htmlToPdfConverter.PdfDocumentOptions.LeftMargin - htmlToPdfConverter.PdfDocumentOptions.RightMargin;

                //// Calculate header height
                //float headerHeight = htmlToPdfConverter.PdfHeaderOptions.HeaderHeight;

                //// Create a line element for the bottom of the header
                //LineElement headerLine = new LineElement(0, headerHeight - 1, headerWidth, headerHeight - 1);

                //// Set line color
                //headerLine.ForeColor = Color.Transparent;

                //// Add line element to the bottom of the header
                //htmlToPdfConverter.PdfHeaderOptions.AddElement(headerLine);
            }
        }
        private void DrawFooter(HtmlToPdfConverter htmlToPdfConverter)
        {
            // Set the footer height in points
            htmlToPdfConverter.PdfFooterOptions.FooterHeight = 60;

            //// Create a text element with page numbering place holders &p; and & P;
            //TextElement footerText = new TextElement(0, 30, "Page &p; of &P;  ", new System.Drawing.Font(new System.Drawing.FontFamily("Times New Roman"), 10, System.Drawing.GraphicsUnit.Point));

            //// Align the text at the right of the footer
            //footerText.TextAlign = HorizontalTextAlign.Right;

            //// Set page numbering text color
            //footerText.ForeColor = Color.Navy;

            //// Embed the text element font in PDF
            //footerText.EmbedSysFont = true;


            //// Add the text element to footer
            //htmlToPdfConverter.PdfFooterOptions.AddElement(footerText);





            string footerHtmlUrl = "<span>&nbsp;&nbsp;</span>";//Server.MapPath("~/DemoAppFiles/Input/HTML_Files/Footer_HTML.html");

            // Set the footer height in points
            htmlToPdfConverter.PdfFooterOptions.FooterHeight = 65;

            // Set footer background color
            htmlToPdfConverter.PdfFooterOptions.FooterBackColor = Color.White;

            // Create a HTML element to be added in footer
            HtmlToPdfElement footerHtml = new HtmlToPdfElement(footerHtmlUrl, string.Empty);

            // Set the HTML element to fit the container height
            footerHtml.FitHeight = true;

            // Add HTML element to footer
            htmlToPdfConverter.PdfFooterOptions.AddElement(footerHtml);

        }
        private void ConvertToPDFfunction()
        {
            // Create a HTML to PDF converter object with default settings
            HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();
            Document pdfDocument = null;
            bool reviseApprovedCP = false;

            try
            {
                // Set an adddional delay in seconds to wait for JavaScript or AJAX calls after page load completed
                // Set this property to 0 if you don't need to wait for such asynchcronous operations to finish
                htmlToPdfConverter.ConversionDelay = 2;

                // Install a handler where you can set header and footer visibility or create a custom header and footer in each page
                htmlToPdfConverter.PrepareRenderPdfPageEvent += new PrepareRenderPdfPageDelegate(htmlToPdfConverter_PrepareRenderPdfPageEvent);


                // Set license key received after purchase to use the converter in licensed mode
                // Leave it not set to use the converter in demo mode
                htmlToPdfConverter.LicenseKey = "SMbVx9LXx9/e18fRydfH1NbJ1tXJ3t7e3sfX";

                // Set HTML Viewer width in pixels which is the equivalent in converter of the browser window width
                //htmlToPdfConverter.HtmlViewerWidth = int.Parse(htmlViewerWidthTextBox.Text);

                // Set HTML viewer height in pixels to convert the top part of a HTML page 
                // Leave it not set to convert the entire HTML
                //if (htmlViewerHeightTextBox.Text.Length > 0)
                //    htmlToPdfConverter.HtmlViewerHeight = int.Parse(htmlViewerHeightTextBox.Text);

                //htmlToPdfConverter.HtmlViewerHeight = 792;
                //htmlToPdfConverter.HtmlViewerWidth = 612;
                // Enable header in the generated PDF document
                htmlToPdfConverter.PdfDocumentOptions.ShowHeader = true;
                DrawHeader(htmlToPdfConverter, false);

                htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;
                this.DrawFooter(htmlToPdfConverter);


                // Set PDF page size which can be a predefined size like A4 or a custom size in points 
                // Leave it not set to have a default A4 PDF page
                htmlToPdfConverter.PdfDocumentOptions.PdfPageSize = SelectedPdfPageSize("Letter");
                htmlToPdfConverter.PdfDocumentOptions.TopMargin = 6;
                htmlToPdfConverter.PdfDocumentOptions.LeftMargin = 40;
                htmlToPdfConverter.PdfDocumentOptions.RightMargin = 40;
                htmlToPdfConverter.HtmlViewerWidth = 806;
                htmlToPdfConverter.ClipHtmlView = true;
                htmlToPdfConverter.PdfDocumentOptions.AutoSizePdfPage = true;
                //htmlToPdfConverter.PdfDocumentOptions.StretchToFit = true;
                //htmlToPdfConverter.ClipHtmlView = true;
                //htmlToPdfConverter.HtmlViewerWidth = Convert.ToInt32(htmlToPdfConverter.PdfDocumentOptions.PdfPageSize.Width);
                // Set PDF page orientation to Portrait or Landscape
                // Leave it not set to have a default Portrait orientation for PDF page
                htmlToPdfConverter.PdfDocumentOptions.PdfPageOrientation = SelectedPdfPageOrientation(0);
                // Set the maximum time in seconds to wait for HTML page to be loaded 
                // Leave it not set for a default 60 seconds maximum wait time
                //htmlToPdfConverter.NavigationTimeout = int.Parse(navigationTimeoutTextBox.Text);

                // Set an adddional delay in seconds to wait for JavaScript or AJAX calls after page load completed
                // Set this property to 0 if you don't need to wait for such asynchcronous operations to finish
                //if (conversionDelayTextBox.Text.Length > 0)
                //    htmlToPdfConverter.ConversionDelay = int.Parse(conversionDelayTextBox.Text);

                // The buffer to receive the generated PDF document
                byte[] outPdfBuffer = null;

                //if (convertUrlRadioButton.Checked)
                //{
                //    string url = urlTextBox.Text;

                //    // Convert the HTML page given by an URL to a PDF document in a memory buffer
                //    outPdfBuffer = htmlToPdfConverter.ConvertUrl(url);
                //}
                //else
                //{
                //    string htmlString = htmlStringTextBox.Text;
                //    string baseUrl = string.Empty;

                //    // Convert a HTML string with a base URL to a PDF document in a memory buffer
                //    outPdfBuffer = htmlToPdfConverter.ConvertHtml(htmlString, baseUrl);
                //}
                string htmlString = this.txtContentToPrint.Text.Trim();
                string baseUrl = getServerURL();
                // Convert a HTML string with a base URL to a PDF document in a memory buffer
                pdfDocument = htmlToPdfConverter.ConvertHtmlToPdfDocumentObject(htmlString, baseUrl);
                // Send the PDF as response to browser
                // JavaScript to open the print dialog
                string javaScript = "print()";
                // Set the JavaScript action
                pdfDocument.OpenAction.Action = new PdfActionJavaScript(javaScript);
                outPdfBuffer = pdfDocument.Save();

                if (this.IfRevisedCopy())
                {
                    this.StoreVersion(outPdfBuffer);
                    this.txtActionCompleted.Text = "1";
                    //Response.Redirect(ProjectSettings.PageHome);
                }
                else
                {
                    // Set response content type
                    Response.AddHeader("Content-Type", "application/pdf");
                    string fileName = string.IsNullOrEmpty(this.txtFileNameToInputWithoutExt.Text.Trim()) ? string.Format("Print_{0}{1}{2}-{3}{4}{5}",
                        DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), DateTime.Now.Second.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day, DateTime.Now.Year) : this.txtFileNameToInputWithoutExt.Text.Trim()
                        .Replace(",", "_").Replace(";", "_").Replace("#", "_").Replace("'", "_").Replace("\"", "_");
                    // Instruct the browser to open the PDF file as an attachment or inline
                    //Response.AddHeader("Content-Disposition", String.Format("{0}; filename=Getting_Started.pdf; size={1}", openInlineCheckBox.Checked ? "inline" : "attachment", outPdfBuffer.Length.ToString()));
                    Response.AddHeader("Content-Disposition", String.Format("{0}; filename=" + fileName + ".pdf; size={1}", "inline", outPdfBuffer.Length.ToString()));

                    // Write the PDF document buffer to HTTP response
                    Response.BinaryWrite(outPdfBuffer);

                    // End the HTTP response and stop the current page processing
                    Response.End();

                }
                //-----------------------------------------------------------------
            }
            finally
            {
                if (pdfDocument != null)
                    pdfDocument.Close();
            }
        }
        private string getServerURL()
        {
            try
            {
                return string.Format("{0}/", SPContext.Current.Web.Site.Url.ToLower().Substring(0, (SPContext.Current.Web.Site.Url.Length - SPContext.Current.Web.Site.ServerRelativeUrl.Length)));
            }
            catch
            {
            }
            return (string.IsNullOrEmpty(SPContext.Current.Web.Site.ServerRelativeUrl) || SPContext.Current.Web.Site.ServerRelativeUrl == "/" || SPContext.Current.Web.Site.ServerRelativeUrl == "//") ? string.Format("{0}/", SPContext.Current.Web.Site.Url.ToLower().Trim()) : string.Format("{0}/", SPContext.Current.Web.Site.Url.ToLower().Trim().Replace(SPContext.Current.Web.Site.ServerRelativeUrl.ToLower().Trim(), string.Empty));
        }
        private bool StoreVersion(byte[] contents)
        {
            try
            {

                //SPSecurity.RunWithElevatedPrivileges(delegate ()
                //{
                bool createPDFNeeded = false;

                using (SPSite dSite = new SPSite(this.DataSiteURL))
                {
                    using (SPWeb dWeb = dSite.OpenWeb(this.DataWebRelativeURL))
                    {

                        this.hdf_MainObjID.Value = this.ReportId;
                        this.hdfCurrentLogin.Value = this.HostedWeb.CurrentUser.LoginName;
                        if (!string.IsNullOrEmpty(this.hdf_MainObjID.Value))
                        {
                            this.CurrentMainObject = new MainObject(dWeb, this.hdf_MainObjID.Value.Trim(), true);
                        }

                        if(this.CurrentMainObject.Status==ProjectSettings.ProjectStatusApproved)
                        {
                            this.CurrentMainObject.ApprovalVersion = string.IsNullOrEmpty(this.CurrentMainObject.ApprovalVersion) || this.CurrentMainObject.ApprovalVersion=="0" ? "1" : string.Format(this.CurrentMainObject.ApprovalVersion);
                            // Remove all special Characters
                            string vName = string.Format("_V-{0}", this.CurrentMainObject.ApprovalVersion);
                            string apvDate = ProjectUtilities.DisplayDateTimeMMDDYYYYDash(this.CurrentMainObject.ApprovedDate);
                            apvDate = string.IsNullOrEmpty(apvDate) ? string.Empty : string.Format("_Approved_{0}", apvDate);
                            string filename = ProjectUtilities.TrimFileName(string.Format("{0}-{1}{2}{3}.pdf", this.CurrentMainObject.ProjectName, this.CurrentMainObject.ProjectID, vName, apvDate));

                            SPList mLib = dWeb.Lists[ProjectSettings.SPListMasterVersions];
                            SPFolder masterID = null;
                            try
                            {
                                masterID = mLib.RootFolder.SubFolders[this.CurrentMainObject.MasterID.Trim()];
                            }
                            catch { }
                            if (masterID == null)
                            {
                                createPDFNeeded = true;
                                masterID = mLib.RootFolder.SubFolders.Add(this.CurrentMainObject.MasterID.Trim());
                            }
                            // Version Folder
                            SPFolder subVersion = null;
                            try
                            {
                                subVersion = masterID.SubFolders[this.CurrentMainObject.ApprovalVersion.Trim()];
                            }
                            catch { }
                            if (subVersion == null)
                            {
                                createPDFNeeded = true;
                                subVersion = masterID.SubFolders.Add(this.CurrentMainObject.ApprovalVersion.Trim());
                            }
                            if (createPDFNeeded && subVersion != null && masterID != null) //&& this.CurrentMainObject.Status==ProjectSettings.ProjectStatusApproved)
                            {
                                SPFile fileAdded = subVersion.Files.Add(filename, contents, true);
                                //------------------------------
                                fileAdded.Item.Properties["Title"] = string.Format("{0}-{1}", this.CurrentMainObject.ProjectName, this.CurrentMainObject.ProjectID);
                                fileAdded.Item.Properties["MasterID"] = string.Format(this.CurrentMainObject.MasterID);
                                fileAdded.Item.Properties["ApprovalVersion"] = this.CurrentMainObject.ApprovalVersion;
                                if (this.CurrentMainObject.ApprovedDate != null)
                                    fileAdded.Item.Properties["ApprovedDate"] = this.CurrentMainObject.ApprovedDate;

                                fileAdded.Item.Properties["VersionHTML"] = this.txtContentToPrint.Text.Trim();
                                fileAdded.Item.SystemUpdate();
                            }
                        }
                        
                    }
                }
                //});
                return true;
            }
            catch { }
            return false;
           
        }

    }

}

/*

*/
