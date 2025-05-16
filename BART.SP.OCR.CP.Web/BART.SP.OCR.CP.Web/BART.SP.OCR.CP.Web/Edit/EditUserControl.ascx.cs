using BART.SP.OCR.CP.Base;
using BART.SP.OCR.CP.Common;
using BART.SP.OCR.CP.Model;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using System.Text;

namespace BART.SP.OCR.CP.Web.Edit
{
    public partial class EditUserControl : ProjectUserControlBase
    {
        public bool IfOCRAnalyst
        {
            get
            {
                if (this.txtAccessLevel2CP.Text.Trim().Contains(string.Format("|{0}|", ProjectSettings.UserLevelOCRAnalyst))
                    || this.txtAccessLevel2CP.Text.Trim().Contains(string.Format("|{0}|", ProjectSettings.UserLevelOCRAnalystProxy)))
                    return true;
                else return false;
            }
        }

        public bool IfRequester
        {
            get
            {
                if (this.txtAccessLevel2CP.Text.Trim().Contains(string.Format("|{0}|", ProjectSettings.UserLevelRequestor))
                        || this.txtAccessLevel2CP.Text.Trim().Contains(string.Format("|{0}|", ProjectSettings.UserLevelRequesterProxy)))
                    return true;
                else return false;
            }
        }
        public bool IfAdmin
        {
            get
            {
                if (this.txtAccessLevel2CP.Text.Trim().Contains(string.Format("|{0}|", ProjectSettings.UserLevelAdmin)))
                    return true;
                else return false;
            }
        }
        public bool IfLCU45MCons
        {
            get
            {
                if (this.txtAccessLevel2CP.Text.Trim().Contains(string.Format("|{0}|", ProjectSettings.UserLevelLCUGroup45MCons)))
                    return true;
                else return false;
            }
        }
        public bool IfLCU45MConsONLY
        {
            get
            {
                if (this.txtAccessLevel2CP.Text.Trim()==(string.Format("|{0}|", ProjectSettings.UserLevelLCUGroup45MCons)))
                    return true;
                else return false;
            }
        }
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
            this.lblNoofContract.Text = Convert.ToString(Convert.ToInt32(this.hdfNoOfContracts.Value)+1);
        }
        private void LoadHistoryNComments(SPWeb dWeb, MainObject obj)
        {
            this.LoadApproverCommentsHistoryNoneDraft(dWeb, obj, this.lblComments, this.lblHistory,this.lblPreviousVer);
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

        private void ShowError(bool changeDefaultMessage)
        {
            this.lblErrorMessage.Visible = true;
            if (changeDefaultMessage)
                this.lblErrorMessage.Text = "<strong>There was an unexpected error occurred please try again later</strong>";
        }
        private void loadAllDefaultInfo(SPWeb web)
        {
            try
            {
                // --------
                this.hdf_MainObjID.Value = this.ReportId;

                if (!string.IsNullOrEmpty(this.hdf_MainObjID.Value))
                {
                    this.CurrentMainObject = new MainObject(web, this.hdf_MainObjID.Value.Trim(), true);
                    List<string> userLevel = new List<string>();
                    SPListItem mainItem = this.CurrentMainObject.getCurrentReportItem(web);
                    if (!this.HasEditRight(this.CurrentMainObject,web, ref userLevel))
                    {
                        Response.Redirect(Settings.PageNotFound); return;
                    }
                    else
                    {
                        string access = string.Empty;
                        foreach(string acs in userLevel)
                        {
                            access += string.Format("|{0}|",acs);
                        }
                        this.txtAccessLevel2CP.Text =access;
                        //--------
                        //this.CurrentMainObject.Requester = this.HostedWeb.CurrentUser;
                        //this.CurrentMainObject.RequesterName = this.HostedWeb.CurrentUser.Name;

                        //this.CurrentMainObject.UserCreated = this.HostedWeb.CurrentUser;
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
                        this.loadDepartmentsToList(web, this.ddl_sDepartmentCode);
                        //--------------------------------------------------------
                        this.loadProjectsToList(web, this.ddlProjectList,this.ddl_ProgramName);
                        this.loadProjectsToList(web, this.ddlHiddenProjectList);

                        PropertyInfo[] properties = this.CurrentMainObject.GetType().GetProperties();

                        this.LoadServiceType(this.CurrentMainObject.ServiceType, this.cbx_ServiceType);

                        foreach (var propertyInfo in properties)
                        {
                            this.LoadControlFields(propertyInfo, this.Controls);
                            this.LoadControlFields(propertyInfo, this.pnlStep1.Controls);
                            this.LoadControlFields(propertyInfo, this.pnl_OCRGeneralInfo.Controls);
                            if(this.IfOCRAnalyst || this.IfAdmin)
                                this.LoadControlFields(propertyInfo, this.pnl_OCRGeneralInfo.Controls);
                            // ------------------------------------------------------------------------------------------------------------------------
                        }
                        string selectedProject = string.Format("{0}|{1}|{2}", this.CurrentMainObject.ProjectID.Trim(), this.CurrentMainObject.ProgramName.Trim(), this.CurrentMainObject.ProjectName.ToUpper().Trim());
                        this.ddlProjectList.SelectedValue = selectedProject;
                        this.txtHiddenSelectedPrjOption.Text = selectedProject;
                        //txtHiddenSelectedPrjOption
                        // Date time and user fields 
                        this.txt_KickoffMeetingDate.Text = ProjectUtilities.DisplayDateTimeMMDDYYYY(this.CurrentMainObject.KickoffMeetingDate);
                        string dateSM = ProjectUtilities.DisplayDateTimeMMDDYYYY(this.CurrentMainObject.DateSubmitted);
                        this.lbl_DateSubmitted.Text= string.IsNullOrEmpty(dateSM)?"N/A":dateSM;
                        //-------------
                        this.pnlInstruction1.Visible = false;
                        this.pnlInstruction2.Visible = false;
                        if (this.CurrentMainObject.Status == ProjectSettings.ProjectStatusDraft)
                        {
                            this.btnSubmitForApproval.Visible = true;
                            this.btnSubmitForApprovalTop.Visible = true;
                            this.btnReRouteForApproval.Visible = false;
                            this.btnReRouteForApprovalTop.Visible = false;
                            this.pnlInstruction1.Visible = true;
                            this.pnlInstruction2.Visible = true;
                        }
                        //else if (this.CurrentMainObject.Status == ProjectSettings.ProjectStatusUnderReview || this.CurrentMainObject.Status == ProjectSettings.ProjectStatusRejected)
                        //{
                        //    this.btnSubmitForApproval.Visible = false;
                        //    this.btnSubmitForApprovalTop.Visible = false;
                        //    this.btnReRouteForApproval.Visible = true;
                        //    this.btnReRouteForApprovalTop.Visible = true;
                        //}
                        //else if (this.CurrentMainObject.Status == ProjectSettings.ProjectStatusApproved)
                        //{
                        //    this.btnSubmitForApproval.Visible = false;
                        //    this.btnSubmitForApprovalTop.Visible = false;
                        //    this.btnReRouteForApproval.Visible = true;
                        //    this.btnReRouteForApprovalTop.Visible = true;
                        //    this.lbtSaveBottom.Visible = false;
                        //    this.lbtSaveTop.Visible = false;
                        //}
                        //else
                        //{
                        //    this.btnReRouteForApproval.Visible = false;
                        //    this.btnReRouteForApprovalTop.Visible = false;
                        //}
                        // --------------------------------------------------------------------------------------------------------------------------
                        this.DisplayAttachmentList(web, this.lblUploadedDocs, this.CurrentMainObject.ListAttachments);
                        //------------------------------------------------
                        this.hdfNoOfContracts.Value = Convert.ToString(this.CurrentMainObject.ContractDisplays.Count-1);
                        //
                        ViewState[ProjectSettings.ViewStateContracts] = this.CurrentMainObject.ContractDisplays;
                        this.AddHiddenContracts();
                        this.rpt_Contracts.DataSource = this.ContractDisplays;
                        this.rpt_Contracts.DataBind();
                        //-----------------------------------------------
                        this.HideActionsByUserLevel();
                        //
                        this.txtExportURL.Text= string.Format("{0}{1}&{2}={3}", ProjectUtilities.GetPagesFolderURLFromDataSite(web), BART.SP.OCR.CP.Common.ProjectUtilities.ExportMasterItemURL(this.hdf_MainObjID.Value.Trim()), ProjectSettings.QueryRevise, ProjectSettings.QueryReviseValue);

                        if(this.CurrentMainObject.DateCreated == null || this.CurrentMainObject.DateCreated < ProjectSettings.CB5MChangeDate)
                        {
                            this.cb_OCROver10M_Yes.Text = "Construction contract over $5M ?";
                            this.cb_OCROver10M_Yes.ToolTip = "Check this box if this contracting plan includes a construction contract that is $5M or more. This will notify the Labor Compliance Unit";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ProjectUtilities.LogError(ex.ToString());
            }
        }


        private void HideActionsByUserLevel()
        {
            //-------------------------------------------
            if(this.CurrentMainObject.Status != ProjectSettings.ProjectStatusDraft)
            {
                this.btnSubmitForApproval.Visible = false;
                this.btnSubmitForApprovalTop.Visible = false;
                this.btnReRouteForApprovalTop.Visible = false;
                this.btnReRouteForApproval.Visible = false;
                this.lbtSaveBottom.Visible = false;
                this.lbtSaveTop.Visible = false;
                //----------------------------------------
                this.cb_OCROver10M_Yes.Enabled = false;
                this.txt_OCRCCUAnalysisSummary.Enabled = false;
                this.txt_OCRLCUAnalysisSummary.Enabled = false;
                if (this.IfLCU45MCons)
                {
                    if (this.CurrentMainObject.Status != ProjectSettings.ProjectStatusApproved)
                    {
                        this.lbtSaveBottom.Visible = true;
                        this.lbtSaveTop.Visible = true;
                        this.txt_OCRLCUAnalysisSummary.Enabled = true;
                    }
                }
                if (this.IfOCRAnalyst)
                {
                    this.lbtSaveBottom.Visible = true;
                    this.lbtSaveTop.Visible = true;
                    this.cb_OCROver10M_Yes.Enabled = true;
                    this.txt_OCRCCUAnalysisSummary.Enabled = true;
                    this.txt_OCRLCUAnalysisSummary.Enabled = true;
                }
                if (this.IfRequester)
                {
                    this.btnReRouteForApprovalTop.Visible = true;
                    this.btnReRouteForApproval.Visible = true;
                    //
                    if (this.CurrentMainObject.Status != ProjectSettings.ProjectStatusApproved)
                    {
                        this.lbtSaveBottom.Visible = true;
                        this.lbtSaveTop.Visible = true;
                    }
                }
                if (this.IfAdmin)
                {
                    this.btnReRouteForApprovalTop.Visible = true;
                    this.btnReRouteForApproval.Visible = true;
                    this.lbtSaveBottom.Visible = true;
                    this.lbtSaveTop.Visible = true;
                }
                //
                if(this.IfLCU45MConsONLY)
                {
                    this.MakeReadOnlyPanel(this.pnlStep1.Controls);
                }

                if (this.CurrentMainObject.Status == ProjectSettings.ProjectStatusUnderReview || this.CurrentMainObject.Status == ProjectSettings.ProjectStatusApproved)
                {
                    this.txtOriginalPM_GPM_Cf_AGM.Text = string.Format("{0}_{1}_{2}_{3}", this.hdf_SponsorProjectManagerLogin.Value, this.hdf_GroupManagerLogin.Value,
                        this.hdf_DepartmentChiefLogin.Value, this.hdf_DepartmentAGMLogin.Value);
                }
            }
            
        }

        protected void UpdateMainObjectFields(SPWeb dWeb)
        {
            this.CurrentMainObject.ServiceType = this.SaveServiceType(this.cbx_ServiceType);
            PropertyInfo[] properties = this.CurrentMainObject.GetType().GetProperties();

            // Check of Current > 5 M
            bool currentMoreThan5M = this.CurrentMainObject.OCROver10M;
            //--------------
            if (!this.IfLCU45MConsONLY)
            {
                foreach (var propertyInfo in properties)
                {

                    this.CompareControlFields(propertyInfo, this.pnlStep1.Controls, dWeb);
                    this.CompareControlFields(propertyInfo, this.Controls, dWeb);
                    if (this.IfOCRAnalyst || this.IfAdmin)
                    {
                        this.CompareControlFields(propertyInfo, this.pnl_OCRGeneralInfo.Controls, dWeb);
                    }
                    //this.CompareControlFields(propertyInfo, this.pnlStep2.Controls);
                    //this.CompareControlFields(propertyInfo, this.pnlStep3.Controls);
                    // ------------------------------------------------------------------------------------------------------------------------
                }
            }
            // IF LCU Group ONLY added 10/8/2020
            if (this.IfLCU45MCons)
            {
                this.CurrentMainObject.OCRLCUAnalysis = SPHelper.GetSPUserFromLoginName(dWeb, this.hdf_OCRLCUAnalysisLogin.Value.Trim());
                this.CurrentMainObject.OCRLCUAnalysisSummary = this.txt_OCRLCUAnalysisSummary.Text.Trim();
            }
            // Set Project Related 
            //email if checked 
            if (!currentMoreThan5M && this.CurrentMainObject.OCROver10M)
            {
                // Send email to OCR Complience
                string fromEmail = SPHelper.GetEmailByUser(dWeb.CurrentUser.LoginName.Trim(), dWeb);
                List<string> toEmails = SPHelper.GetEmailListByGroupName(Settings.GroupOCRCompliance, dWeb);
                if (toEmails.Count > 0)
                {
                    string to = ProjectUtilities.TrimEmailListToString(toEmails);
                    string uniqueTitle = string.Format("[{0}]-{1}", this.CurrentMainObject.ProjectName, this.CurrentMainObject.ProjectID);
                    string viewUrl = ProjectUtilities.GetPagesFolderURLFromDataSite(dWeb) + BART.SP.OCR.CP.Common.ProjectUtilities.ViewItemUrl(this.CurrentMainObject.MasterID);
                    string title = string.Format(Settings.OCRComplianceNotificationTitle, uniqueTitle);
                    string body = string.Format(Settings.OCRComplianceNotificationBody, viewUrl);

                    ProjectUtilities.SendEmailToMultiple(fromEmail, to, title, body);
                }
            }
            try
            {
                this.CurrentMainObject.ProjectName = this.ddlProjectList.SelectedItem.Text.Trim();
                this.CurrentMainObject.ProjectID = this.ddlProjectList.SelectedValue.Split('|')[0];
                this.CurrentMainObject.ProgramName = this.ddlProjectList.SelectedValue.Split('|')[1];
            }
            catch{ }
            
        }

        protected void rpt_Contracts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var ddlStatus = e.Item.FindControl("ddl_Status") as DropDownList;
                var txtStatus = e.Item.FindControl("hiddenStatus") as TextBox;
                var txtVisible = e.Item.FindControl("txtVisible") as TextBox;

                if(txtVisible.Visible)
                {
                    string selectedVal = txtStatus.Text.Trim();

                    if (!string.IsNullOrEmpty(selectedVal))
                    {
                        ddlStatus.SelectedValue = selectedVal;
                    }

                    var ddl_FundingSource = e.Item.FindControl("ddl_FundingSource") as DropDownList;
                    var txtFundingSource = e.Item.FindControl("hiddenFundingSource") as TextBox;
                    string selectedFundingSource = txtFundingSource.Text.Trim();

                    if (!string.IsNullOrEmpty(selectedFundingSource))
                    {
                        ddl_FundingSource.SelectedValue = selectedFundingSource;
                    }
                }
                else
                {
                    e.Item.Visible = false;
                }

            }
        }

        

        private void UpdateContracts(SPWeb dWeb)
        {
            try
            {
                try
                {
                    //------------------- START Remove Deleted Contract's Docs
                    if (!string.IsNullOrEmpty(this.hdfDeletedContractDocMarkIDs.Value))
                        this.RemoveContractFiles(dWeb, this.hdfDeletedContractDocMarkIDs.Value.Split(';'));
                    //------------------- END Remove Deleted Contract's Docs

                    ContractList list = new ContractList();
                    list.ListObjects = new List<Contract>();
                    Telerik.Web.UI.RadAsyncUpload ctrUpload = null;
                    foreach (RepeaterItem repeaterItem in this.rpt_Contracts.Items)
                    {
                        if(repeaterItem.Visible)
                        {
                            TextBox txt_ItemID = (TextBox) repeaterItem.FindControl("txt_ItemID");
                            string itmID = string.IsNullOrEmpty(txt_ItemID.Text.Trim()) ? string.Empty : txt_ItemID.Text.Trim();

                            TextBox txt_TargetCompletionDate = (TextBox) repeaterItem.FindControl("txt_TargetCompletionDate");
                            DateTime? targetdate = null;
                            if (!string.IsNullOrEmpty(txt_TargetCompletionDate.Text.Trim()))
                                targetdate = Convert.ToDateTime(txt_TargetCompletionDate.Text.Trim());

                            TextBox txt_Duration = (TextBox) repeaterItem.FindControl("txt_Duration");
                            string duration = string.IsNullOrEmpty(txt_Duration.Text.Trim()) ? string.Empty : txt_Duration.Text.Trim();

                            TextBox txt_OrderInTable = (TextBox) repeaterItem.FindControl("txt_OrderInTable");
                            int order = Convert.ToInt32(txt_OrderInTable.Text.Trim());

                            TextBox txt_ContractNo = (TextBox) repeaterItem.FindControl("txt_ContractNo");
                            string contractno = string.IsNullOrEmpty(txt_ContractNo.Text.Trim()) ? string.Empty : txt_ContractNo.Text.Trim();

                            TextBox txt_DollarAmount = (TextBox) repeaterItem.FindControl("txt_DollarAmount");
                            string damount = txt_DollarAmount.Text.Trim();

                            TextBox txt_Description = (TextBox) repeaterItem.FindControl("txt_Description");
                            string des = txt_Description.Text.Trim();

                            TextBox txt_OCRAnalysis = (TextBox) repeaterItem.FindControl("txt_OCRAnalysis");
                            string ocranalysis = txt_OCRAnalysis.Text.Trim();

                            DropDownList ddl_Status = (DropDownList) repeaterItem.FindControl("ddl_Status");
                            string status = ddl_Status.SelectedValue.Trim();

                            DropDownList ddl_FundingSource = (DropDownList) repeaterItem.FindControl("ddl_FundingSource");
                            string fund = ddl_FundingSource.SelectedValue.Trim();
                            // ----------------------------------------------------------------
                            ctrUpload = (Telerik.Web.UI.RadAsyncUpload) repeaterItem.FindControl("contractAttachment");

                            list.ListObjects.Add(new Contract(this.hdf_MainObjID.Value.Trim(), itmID, order, fund, damount, duration, status, des, ocranalysis, contractno, targetdate, ctrUpload.UploadedFiles));
                        }
                        
                    }
                    //---------------------------------------------------------------------------------------------------------------------------------------//
                    if (list.ListObjects.Count > 0)
                    {
                        foreach (Contract c in list.ListObjects)
                        {
                            //Update -----------------------------------------------------------
                            if (!string.IsNullOrEmpty(c.ItemID))
                            {
                                if (c.Files != null)
                                    c.Update(dWeb, c.Files); // Need to address deleted files
                                else
                                    c.Update(dWeb);
                            }
                            //Add New ------------------------------------------------------
                            else
                            {
                                if (c.Files != null)
                                    c.New(dWeb, c.Files);
                                else
                                    c.New(dWeb);
                            }
                        }
                        // ---------------------------------------------------------------------
                        foreach (Contract c in this.CurrentMainObject.Contracts.ListObjects)
                        {
                            bool isDeleted = true;
                            foreach (Contract sc in list.ListObjects)
                            {
                                try
                                {
                                    if (c.ItemID == sc.ItemID)
                                        isDeleted = false;
                                }
                                catch { }
                            }
                            //
                            if (isDeleted)
                                c.Delete(dWeb);
                        }
                    }
                    // ---------------------------------------------------------------------
                    this.CurrentMainObject.Contracts = list;
                }
                catch (Exception ex)
                {
                    ProjectUtilities.LogError(ex.ToString());
                }
            }
            catch (Exception ex)
            {
                ProjectUtilities.LogError(ex.ToString());
            }
        }
        private void UpdateChangeOrder()
        {
            try
            {
                //ChangeOrderList list = new ChangeOrderList();
                //if (this.cbx_ExecutionType.SelectedValue == ProjectSettings.ExecutionTypeChangeOrder)
                //{
                //    list.ListObjects = new List<ChangeOrder>();
                //    list.ListObjects.Add(new ChangeOrder(1, "", "", Convert.ToString(this.txt_changeOrder1.Text).Trim()
                //        , Convert.ToString(this.txt_changeOrder1.Text).Trim(), Convert.ToString(this.txt_ChangeOrderAmount1.Text).Trim(),
                //        Convert.ToString(this.txt_ChangeOrderFY1.Text).Trim()));

                //    list.ListObjects.Add(new ChangeOrder(2, "", "", Convert.ToString(this.txt_changeOrder2.Text).Trim()
                //        , Convert.ToString(this.txt_changeOrder2.Text).Trim(), Convert.ToString(this.txt_ChangeOrderAmount2.Text).Trim(),
                //        Convert.ToString(this.txt_ChangeOrderFY2.Text).Trim()));

                //    list.ListObjects.Add(new ChangeOrder(3, "", "", Convert.ToString(this.txt_changeOrder3.Text).Trim()
                //        , Convert.ToString(this.txt_changeOrder3.Text).Trim(), Convert.ToString(this.txt_ChangeOrderAmount3.Text).Trim(),
                //        Convert.ToString(this.txt_ChangeOrderFY3.Text).Trim()));
                //}

                //this.CurrentMainObject.ListChanges = list; //

            }
            catch (Exception ex)
            {
                ProjectUtilities.LogError(ex.ToString());
            }
        }
        private void UpdateAll(string status, string sAction)
        {
            try
            {
                using (SPSite dSite = new SPSite(this.DataSiteURL))
                {
                    using (SPWeb dWeb = dSite.OpenWeb(this.DataWebRelativeURL))
                    {
                        this.CurrentMainObject = new MainObject(dWeb, this.hdf_MainObjID.Value, true);

                        //-------------------------------Move approved Docs------------------------------------------------
                        if(this.CurrentMainObject.Status==ProjectSettings.ProjectStatusApproved && sAction== ProjectSettings.ActionCodeReSubmit)
                        {
                            if (this.DataSiteURL.ToLower().Trim() != this.ArchivedDocsSiteURL.ToLower().Trim())
                            {
                                //Archive approved Docs
                                SPSecurity.RunWithElevatedPrivileges(delegate ()
                                {
                                    using (SPSite arvSite = new SPSite(this.ArchivedDocsSiteURL))
                                    {
                                        using (SPWeb arvWeb = arvSite.OpenWeb(this.ArchivedDocsWebRelativeURL))
                                        {
                                            this.CurrentMainObject.ArchiveAllApprovedDocuments(dWeb, arvWeb);
                                        }
                                    }
                                });
                            }
                            else
                            {
                                this.CurrentMainObject.ArchiveAllApprovedDocuments(dWeb, dWeb);
                            }
                        }
                        //-------------------------------------------------------------------------------

                        this.UpdateMainObjectFields(dWeb);

                        if (!this.IfLCU45MConsONLY)
                        {
                            //----attachments
                            if (!string.IsNullOrEmpty(this.hdfDeletedDocMarkIDs.Value))
                                this.RemoveFiles(dWeb, this.hdfDeletedDocMarkIDs.Value.Split(';'));
                            if (this.CtrlAttachment.UploadedFiles.Count > 0)
                                this.AddNewFiles(dWeb, this.CtrlAttachment.UploadedFiles, this.ReportId);
                            //----END attachent 


                            //this.UpdateAcqHistory();
                            //this.UpdateChangeOrder();

                            this.UpdateContracts(dWeb);
                        }
                        if (sAction == ProjectSettings.ActionCodeRoute)
                        {
                            this.CurrentMainObject.Status = status;
                            this.CurrentMainObject.UpdateAndRoute(dWeb);
                            //
                            Response.Redirect(ProjectSettings.PageHome);
                            
                            //ProjectHelper.AddHistory(dWeb, this.CurrentMainObject, Settings.HistoryTypeRouted, Settings.HistoryActionRouted, dWeb.CurrentUser.LoginName, ProjectUtilities.GetCurrentDateTimeShortFormat(), this.hdf_RequestedByLogin.Value);
                        }
                        else if (sAction == ProjectSettings.ActionCodeReSubmit)
                        {

                            string beforeStatus = this.CurrentMainObject.Status;
                            this.CurrentMainObject.Status = status;
                            this.CurrentMainObject.CurrentStep = ProjectSettings.Step_Staffs;
                            this.CurrentMainObject.ApprovedDate = null;

                            // Make cases for different status Oct 31 2023
                            if(beforeStatus==ProjectSettings.ProjectStatusApproved)
                                this.CurrentMainObject.UpdateAndRe_Route(dWeb,3,9);
                            else
                                this.CurrentMainObject.UpdateAndRe_Route(dWeb);

                            Response.Redirect(ProjectSettings.PageHome);
                            //----------------------------------------------------------------------//----------------------
                            //ProjectHelper.AddHistory(dWeb, this.CurrentMainObject, Settings.HistoryTypeRouted, Settings.HistoryActionReRouted, dWeb.CurrentUser.LoginName, ProjectUtilities.GetCurrentDateTimeShortFormat(), this.hdf_RequestedByLogin.Value);

                        }
                        else
                        {
                            this.CurrentMainObject.Update(dWeb, status, true);
                            //this.pnlSuccessMsg.Visible = true;
                            this.pnlErrorMsg.Visible = false;
                            this.LoadAll();
                            //if(this.CurrentMainObject.Status!=ProjectSettings.ProjectStatusDraft)
                            //    ProjectHelper.AddHistory(dWeb, this.CurrentMainObject, Settings.HistoryActionModified, Settings.HistoryActionModified, dWeb.CurrentUser.LoginName, ProjectUtilities.GetCurrentDateTimeShortFormat(), this.hdf_RequestedByLogin.Value);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ProjectUtilities.LogError(ex.ToString());
            }

        }
        protected void btnCancelReport_ServerClick(object sender, EventArgs e)
        {
            try
            {
                this.CancelChanges();
            }
            catch (Exception ex)
            {
                ProjectUtilities.LogError(ex.ToString());
            }

        }
        protected void btnCommitSave_ServerClick(object sender, EventArgs e)
        {
            //this.UpdateAll(ProjectSettings.ProjectStatusDraft);
        }
        protected void btnRoute_ServerClick(object sender, EventArgs e)
        {
            //this.UpdateAll(ProjectSettings.ProjectStatusUnderReview,true);
            string cAction = this.txtCommitAction.Text.Trim();
            if (!string.IsNullOrEmpty(cAction))
            {
                if (cAction == ProjectSettings.ActionCodeSave)
                    this.UpdateAll(ProjectSettings.ProjectStatusDraft, cAction);
                else
                    this.UpdateAll(ProjectSettings.ProjectStatusUnderReview, cAction);
            }
        }
        protected void btnCopy_ServerClick(object sender, EventArgs e)
        {
            string url = string.Format("{0}?RRView={1}", ProjectSettings.PageCreateNew, ProjectUtilities.MakeEditQueryString(this.CurrentMainObject.MasterID));
            Response.Redirect(url);
        }
        protected void btnSavePreview_ServerClick(object sender, EventArgs e)
        {

        }
        protected void lbtSave_Click(object sender, EventArgs e)
        {

        }
        protected void lbt_AddNew_Click(object sender, EventArgs e)
        {
            //this.SaveGrid();
            //this.ContractDisplays.Add(new ContractDisplay() { });
            //ViewState[ProjectSettings.ViewStateContracts] = this.ContractDisplays;
            //this.rpt_Contracts.DataSource = this.ContractDisplays;
            //this.rpt_Contracts.DataBind();

            this.lblNoofContract.Text = Convert.ToString(this.rpt_Contracts.Items.Count);
            RepeaterItem item= this.rpt_Contracts.Items[this.CalculateContract(hdfNoOfContracts, true)];
            item.Visible = true;
            this.ClearRptItem(item);
            this.lblNoofContract.Text = Convert.ToString(Convert.ToInt32(this.hdfNoOfContracts.Value) + 1);
            if (Convert.ToInt32(this.hdfNoOfContracts.Value.Trim()) >= ProjectSettings.MaxContract - 1)
                this.lbt_AddNew.Visible = false;
        }

        protected void btnRemoveContract_ServerClick(object sender, EventArgs e)
        {

            //ViewState[ProjectSettings.ViewStateContracts] = this.ContractDisplays;
            //this.rpt_Contracts.DataSource = this.ContractDisplays;
            //this.rpt_Contracts.DataBind();
            int rowNo = 0;
            int.TryParse(this.txtRowNo.Text, out rowNo);
            //if (rowNo > 0)
            this.rpt_Contracts.Items[rowNo].Visible = false;
            this.CalculateContract(this.hdfNoOfContracts, false);
            this.lblNoofContract.Text = Convert.ToString(Convert.ToInt32(this.hdfNoOfContracts.Value) + 1);
            this.lbt_AddNew.Visible = true;

        }
        protected void btnLoadPopUp_Click(object sender, EventArgs e)
        {
            try
            {
                //
                using (SPSite dSite = new SPSite(this.DataSiteURL))
                {
                    using (SPWeb dWeb = dSite.OpenWeb(this.DataWebRelativeURL))
                    {
                        this.loadProjectsToList(dWeb, this.ddlProjectList, this.ddl_ProgramName);
                        this.loadProjectsToList(dWeb, this.ddlHiddenProjectList);
                    }
                }
            }
            catch (Exception ex)
            {
                ProjectUtilities.LogError(ex.ToString());
            }
        }
    }
}
