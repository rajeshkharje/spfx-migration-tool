using BART.SP.OCR.CP.Base;
using BART.SP.OCR.CP.Common;
using BART.SP.OCR.CP.Model;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Telerik.Web.UI;

namespace BART.SP.OCR.CP.Web.View
{
    public partial class ViewUserControl : ProjectUserControlBase
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
                this.hdfCurrentLogin.Value = this.HostedWeb.CurrentUser.LoginName;
                if (!string.IsNullOrEmpty(this.hdf_MainObjID.Value))
                {
                    this.CurrentMainObject = new MainObject(web, this.hdf_MainObjID.Value.Trim(), true);
                    string userLevel = string.Empty;
                    SPListItem mainItem = this.CurrentMainObject.getCurrentReportItem(web);
                    //------
                    //this.CurrentMainObject.Requester = this.HostedWeb.CurrentUser;
                    //this.CurrentMainObject.RequesterName = this.HostedWeb.CurrentUser.Name;

                    //this.CurrentMainObject.UserCreated = this.HostedWeb.CurrentUser;
                    //this.CurrentMainObject.UserModified = this.HostedWeb.CurrentUser;

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
                    this.LoadPermission(web, this.CurrentMainObject);

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

        protected void UpdateMainObjectFields(SPWeb dWeb)
        {
            this.CurrentMainObject.ServiceType = this.SaveServiceType(this.cbx_ServiceType);
            PropertyInfo[] properties = this.CurrentMainObject.GetType().GetProperties();
            foreach (var propertyInfo in properties)
            {
                this.CompareControlFields(propertyInfo, this.pnlStep1.Controls, dWeb);
                this.CompareControlFields(propertyInfo, this.Controls, dWeb);
                //this.CompareControlFields(propertyInfo, this.pnlStep2.Controls);
                //this.CompareControlFields(propertyInfo, this.pnlStep3.Controls);
                // ------------------------------------------------------------------------------------------------------------------------
            }
        }

        protected void rpt_Contracts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //{
            //    var ddlStatus = e.Item.FindControl("ddl_Status") as DropDownList;
            //    var txtStatus = e.Item.FindControl("hiddenStatus") as TextBox;
            //    var txtVisible = e.Item.FindControl("txtVisible") as TextBox;

            //    if (txtVisible.Visible)
            //    {
            //        string selectedVal = txtStatus.Text.Trim();

            //        if (!string.IsNullOrEmpty(selectedVal))
            //        {
            //            ddlStatus.SelectedValue = selectedVal;
            //        }

            //        var ddl_FundingSource = e.Item.FindControl("ddl_FundingSource") as DropDownList;
            //        var txtFundingSource = e.Item.FindControl("hiddenFundingSource") as TextBox;
            //        string selectedFundingSource = txtFundingSource.Text.Trim();

            //        if (!string.IsNullOrEmpty(selectedFundingSource))
            //        {
            //            ddl_FundingSource.SelectedValue = selectedFundingSource;
            //        }
            //    }
            //    else
            //    {
            //        e.Item.Visible = false;
            //    }

            //}
        }
        protected void Editbottom_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect(Common.ProjectUtilities.EditReportURL(this.hdf_MainObjID.Value));
        }
        protected TaskItemObject GetCurrentPendingTask(MainObject obj)
        {
            List<TaskItemObject> allTasks = obj.Tasks;
            TaskItemObject currentPending = new TaskItemObject();
            bool cUserHasPendingTasks = false;
            //
            foreach(TaskItemObject task in allTasks)
            {
                if (task.TaskStatus == ProjectSettings.TaskStatusRouted)
                {
                    currentPending = task; break;
                }
            }

            return currentPending;
        }

        private void LoadPermission(SPWeb dWeb, MainObject obj)
        {


            this.ddlSubmitPermission.Items.Clear();
            List<string> takenActionTasks = new List<string>();
            List<string> allCheckedTasks = new List<string>();

            bool cUserHasPendingTasks = false;

            Dictionary<string, string> proxies = this.GetCurrentUserProxyPrimary(dWeb);
            TaskItemObject currentPending = this.GetCurrentPendingTask(obj);
            if (currentPending.AssignedTo != null)
            {
                if (proxies.Values.Contains(currentPending.AssignedTo.LoginName.Trim()))
                    cUserHasPendingTasks = true;
            }
            //---------- Check if user can access

            if (!this.HasViewAccess(dWeb, obj, proxies, this.Editbottom, this.Edittop))
            {
                Response.Redirect(Settings.PageNotFound);
                return;
            }
            //
            if (cUserHasPendingTasks)
            {
                if (dWeb.CurrentUser.LoginName.ToLower().Trim() == currentPending.AssignedTo.LoginName.ToLower().Trim())
                    this.ddlSubmitPermission.Items.Insert(0, new ListItem(string.Format("as my self ({0})", dWeb.CurrentUser.Name.Trim()), dWeb.CurrentUser.LoginName.Trim()));
                else
                {
                    this.ddlSubmitPermission.Items.Insert(0, new ListItem("--- Select a proxy to proceed ---", string.Empty));
                    this.ddlSubmitPermission.Items.Add(new ListItem(string.Format("on behalf of {0}", currentPending.AssignedToName), currentPending.AssignedTo.LoginName.Trim()));

                }
                this.hdfCurrentTaskID.Value = currentPending.TaskId;
                if (currentPending.ApprovalTypeCode == ProjectSettings.TaskCodeOCRAnalystAssignment)
                {
                    this.pnlApprovalOCRAnalyst.Visible = true;
                    this.pnlApprovalComments.Visible = false;
                    this.pnlApprovalDecision.Visible = false;
                    this.pnlAsign.Visible = true;
                    this.pndVoting.Visible = false;
                    this.pndApprovalProxy.Visible = false;
                }
                else
                {
                    this.pnlApprovalOCRAnalyst.Visible = false;
                    this.pnlApprovalComments.Visible = true;
                    this.pnlApprovalDecision.Visible = true;
                    this.pnlAsign.Visible = false;
                    this.pndVoting.Visible = true;
                    this.pndApprovalProxy.Visible = true;
                }
            }
            else
            {
                this.hideApprovalForm();
            }
            //
            if (obj.Status != ProjectSettings.ProjectStatusUnderReview)
            {
                this.hideApprovalForm();
            }
            //


        }
        private void hideApprovalForm()
        {
            this.pndVoting.Visible = false;
            this.btnActualClickActionApproval.Visible = false;
            this.pnlAsign.Visible = false;
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
            //this.lblNoofContract.Text = Convert.ToString(this.rpt_Contracts.Items.Count);
            //RepeaterItem item = this.rpt_Contracts.Items[this.CalculateContract(hdfNoOfContracts, true)];
            //item.Visible = true;
            //this.ClearRptItem(item);
            //this.lblNoofContract.Text = Convert.ToString(Convert.ToInt32(this.hdfNoOfContracts.Value) + 1);
            //if (Convert.ToInt32(this.hdfNoOfContracts.Value.Trim()) >= ProjectSettings.MaxContract - 1)
            //    this.lbt_AddNew.Visible = false;
        }

        protected void btnRemoveContract_ServerClick(object sender, EventArgs e)
        {

            //ViewState[ProjectSettings.ViewStateContracts] = this.ContractDisplays;
            //this.rpt_Contracts.DataSource = this.ContractDisplays;
            //this.rpt_Contracts.DataBind();
            //int rowNo = 0;
            //int.TryParse(this.txtRowNo.Text, out rowNo);
            //if (rowNo > 0)
            //    this.rpt_Contracts.Items[rowNo].Visible = false;
            //this.CalculateContract(this.hdfNoOfContracts, false);
            //this.lblNoofContract.Text = Convert.ToString(Convert.ToInt32(this.hdfNoOfContracts.Value) + 1);
            //this.lbt_AddNew.Visible = true;

        }
        private string GetApprovalDecision()
        {
            if (this.RadioConcur.Checked)
                return ProjectSettings.TaskStatusConcur;
            else if (this.RadioWriteAComment.Checked)
                return ProjectSettings.TaskStatusWriteComment;
            else
                return ProjectSettings.TaskStatusRejected;
        }

        private int FindNextTask()
        {
            int iCount = this.CurrentMainObject.Tasks.Count;
            for (int i = 0; i < iCount; i++)
            {
                if (this.CurrentMainObject.Tasks[i].TaskId == this.hdfCurrentTaskID.Value.Trim() && i < iCount - 1)
                {
                    if (this.CurrentMainObject.Tasks[i + 1].TaskStatus == ProjectSettings.TaskStatusNone)
                    { return i + 1; break; }

                }
            }
            return -1;
        }


        private void SubmitNormalTasks(SPWeb dWeb, TaskItemObject cTask)
        {
            string toEmails = string.Empty;
            string mddStatusAfter = string.Empty;
            string uniqueTitle = string.Format("[{0}]-{1}", this.CurrentMainObject.ProjectName, this.CurrentMainObject.ProjectID);
            string comments = this.txtApprovalComments.Text.Trim();
            string status = GetApprovalDecision();
            string historyVal = status;
            string currentStep = string.Empty;
            UploadedFileCollection cFiles = (this.CtrlAttachment.UploadedFiles != null && this.CtrlAttachment.UploadedFiles.Count > 0) ? this.CtrlAttachment.UploadedFiles : null;
            toEmails = GetAllRelatedEmailFromPrimaryAccount(this.CurrentMainObject.Requester.LoginName.Trim().ToLower(), dWeb);
            //
            if (status == ProjectSettings.TaskStatusWriteComment)
            {
                historyVal = ProjectSettings.TaskHistoryWriteAComment;
                ProjectHelper.AddComments(dWeb, this.CurrentMainObject.MasterID, SPHelper.GetSPUserFromLoginName(dWeb, this.ddlSubmitPermission.SelectedValue.Trim()), DateTime.Now, comments, cFiles);
            }
            else if (status == ProjectSettings.TaskStatusConcur)
            {
                cTask.Vote(dWeb, status);
                int nextTaskIndex = this.FindNextTask();
                if (nextTaskIndex != -1)
                {
                    this.CurrentMainObject.Tasks[nextTaskIndex].RouteItem(dWeb, uniqueTitle, this.CurrentMainObject.Requester.LoginName);
                    
                    //--------------------------- Move Approval Step 
                    if (cTask.ApprovalTypeCode == ProjectSettings.TaskCodeOCRA)
                        currentStep = ProjectSettings.Step_Managers;
                    if (cTask.ApprovalTypeCode == ProjectSettings.TaskCodeDeptChief)
                        currentStep = ProjectSettings.Step_Executives;
                    //--------------------------- END Move Approval Step 
                    this.CurrentMainObject.UpdateStatusOnly(dWeb, string.Empty, currentStep);
                }
                else
                {
                    mddStatusAfter = ProjectSettings.ProjectStatusApproved;
                }
            }
            else if (status == ProjectSettings.TaskStatusRejected)
            {
                mddStatusAfter = ProjectSettings.ProjectStatusRejected;
                this.lbl_Status.Text = ProjectSettings.ProjectStatusRejected;

            }
            //-------------------------
            if (status == ProjectSettings.TaskStatusRejected || status == ProjectSettings.TaskStatusConcur)
            {
                //Vote 
                cTask.Vote(dWeb, status);
                //Add comments
                if (!string.IsNullOrEmpty(comments.Trim()))
                    ProjectHelper.AddComments(dWeb, this.CurrentMainObject.MasterID, SPHelper.GetSPUserFromLoginName(dWeb, this.ddlSubmitPermission.SelectedValue.Trim()), DateTime.Now, comments, cFiles);
                // Log to history
                this.AddHistory(dWeb, this.CurrentMainObject, Settings.HistoryActionApproved, historyVal, this.hdfCurrentLogin.Value.Trim(), ProjectUtilities.GetCurrentDateTimeShortFormat(), this.ddlSubmitPermission.SelectedValue.Trim());
            }
            // Update final status based on approval decision and current status
            if (!string.IsNullOrEmpty(mddStatusAfter))
            {
                // update current status to approved or rejected
                this.CurrentMainObject.UpdateStatusOnly(dWeb, mddStatusAfter);

                if (mddStatusAfter == ProjectSettings.ProjectStatusApproved)
                {
                    try
                    {
                        string viewUrl = ProjectUtilities.GetPagesFolderURLFromDataSite(dWeb) + BART.SP.OCR.CP.Common.ProjectUtilities.ViewItemUrl(this.CurrentMainObject.MasterID);
                        //-- Before
                        //string title = string.Format(Settings.ApprovedSSWPNotificationTitle, uniqueTitle, this.CurrentMainObject.ProjectID);
                        string title = string.Format(Settings.ApprovedSSWPNotificationTitle, uniqueTitle);
                        string body = string.Format(Settings.ApprovedSSWPNotificationBody, viewUrl);
                        List<string> bccs = new List<string>(); bccs.Add(this.CurrentMainObject.Requester.LoginName.ToLower().Trim());
                        if (this.CurrentMainObject.OCRAnalyst!=null)
                            bccs.Add(this.CurrentMainObject.OCRAnalyst.LoginName);
                        string emails = ProjectUtilities.TrimEmailListToString(ProjectHelper.GetEmailsWithProxiesEmailsByLoginList(dWeb, bccs));
                        //----------------------------------------------------------------------------------------------------------------------
                        ProjectUtilities.SendEmailToMultiple(SPHelper.GetEmailByUser(dWeb.CurrentUser.LoginName.Trim(), dWeb), emails, title, body, dWeb.Site);
                    }
                    catch
                    {
                        //----------------------------------------------------------
                    }
                }
            }
            //
            if ( this.CurrentMainObject.OCRAnalyst!=null)
            {
                toEmails = string.Format("{0};{1}", GetAllRelatedEmailFromPrimaryAccount(this.CurrentMainObject.Requester.LoginName.Trim().ToLower(), dWeb),
                    GetAllRelatedEmailFromPrimaryAccount(this.CurrentMainObject.OCRAnalyst.LoginName.Trim().ToLower(), dWeb)).Replace(";;",";").TrimEnd(';');
            }
            string titleM = (status == ProjectSettings.TaskStatusWriteComment) ? string.Format(Settings.CommentEmailResultTitle, this.CurrentMainObject.ProjectName, this.CurrentMainObject.ProjectID) : string.Format(Settings.ConcurenceResultTitle, this.CurrentMainObject.ProjectName, this.CurrentMainObject.ProjectID);
            string bodyM = string.Empty;
            string presentName = SPHelper.GetSPUserFromLoginName(dWeb, this.hdfCurrentLogin.Value).Name;
            if (string.IsNullOrEmpty(this.ddlSubmitPermission.SelectedValue.Trim()) || (this.hdfCurrentLogin.Value.ToLower().Trim() == this.ddlSubmitPermission.SelectedValue.ToLower().Trim()))
                bodyM = (status == ProjectSettings.TaskStatusWriteComment) ? string.Format(Settings.CommentEmailResultBody, presentName, historyVal.ToLower(), ProjectUtilities.GetPagesFolderURL() + BART.SP.OCR.CP.Common.ProjectUtilities.ViewItemUrl(this.ReportId)) : string.Format(Settings.ConcurenceResultBody, presentName, historyVal.ToLower(), this.CurrentMainObject.ProjectName, ProjectUtilities.GetPagesFolderURL() + BART.SP.OCR.CP.Common.ProjectUtilities.ViewItemUrl(this.ReportId));
            else
            {
                string proxyForDisplay = string.Format("{0} (acting on behalf of {1})", presentName, SPHelper.GetSPUserFromLoginName(dWeb, this.ddlSubmitPermission.SelectedValue.Trim()).Name);
                bodyM = (status == ProjectSettings.TaskStatusWriteComment) ? string.Format(Settings.CommentEmailResultBody, proxyForDisplay, historyVal.ToLower(), ProjectUtilities.GetPagesFolderURL() + BART.SP.OCR.CP.Common.ProjectUtilities.ViewItemUrl(this.ReportId)) : string.Format(Settings.ConcurenceResultBody, proxyForDisplay, historyVal.ToLower(), this.CurrentMainObject.ProjectName, ProjectUtilities.GetPagesFolderURL() + BART.SP.OCR.CP.Common.ProjectUtilities.ViewItemUrl(this.ReportId));
            }
            //
            ProjectUtilities.SendEmailToMultiple(ProjectUtilities.GetEmailByUser(this.HostedWeb.CurrentUser.LoginName.Trim()), toEmails, titleM, bodyM, dWeb.Site, ProjectUtilities.GetEmailByUser(this.CurrentMainObject.Requester.LoginName.Trim()));
        }

        private void SubmitAssignOCR(SPWeb dWeb, TaskItemObject cTask)
        {
            if (!string.IsNullOrEmpty(this.hdf_OCRAnalystLogin.Value))
            {
                string toEmails = string.Empty;
                string ccEmails = string.Empty;
                string status = ProjectSettings.TaskStatusCompleted;
                SPUser ocrAnalyst = SPHelper.GetSPUserFromLoginName(dWeb, this.hdf_OCRAnalystLogin.Value.Trim());

                toEmails = GetAllRelatedEmailFromPrimaryAccount(this.CurrentMainObject.Requester.LoginName.Trim().ToLower(), dWeb);
                ccEmails = string.Format("{0};{1}", GetAllRelatedEmailFromPrimaryAccount(cTask.AssignedTo.LoginName, dWeb),
                    GetAllRelatedEmailFromPrimaryAccount(ocrAnalyst.LoginName, dWeb));

                // Nov02 2023 - email OCR Manager 2 if an OCR Analyst assigned - can be changed depends on requirement #1// sjtc

                // Find manager2 of the OCR Analyst: 
                string ccEmailOCRM2 = GetAllRelatedEmailFromPrimaryAccount("i:0#.w|bart\\cp_program_mgr_2", dWeb);

                Dictionary<string,string> manager2List = ProjectHelper.GetAllManager2DictionaryByOCRAnalyst(ocrAnalyst.LoginName, dWeb);
                if (manager2List.Count > 0)
                {
                    ccEmailOCRM2= GetAllRelatedEmailFromPrimaryAccount(manager2List.Values.First(), dWeb);
                }
                //
                if (!string.IsNullOrEmpty(ccEmailOCRM2))
                    ccEmails = string.Format("{0};{1}", ccEmails, ccEmailOCRM2);
                // END Nov2-2023 fix

                    cTask.Vote(dWeb, status);
                int nextTaskIndex = this.FindNextTask();
                string uniqueTitle = string.Format("[{0}]-{1}", this.CurrentMainObject.ProjectName, this.CurrentMainObject.ProjectID);
                if (nextTaskIndex != -1)
                    this.CurrentMainObject.Tasks[nextTaskIndex].RouteItem(dWeb, uniqueTitle,this.CurrentMainObject.Requester.LoginName, true, ocrAnalyst);
                this.CurrentMainObject.OCRAnalyst = ocrAnalyst;
                this.CurrentMainObject.OCRAnalyst_Assigned = ocrAnalyst;
                this.CurrentMainObject.UpdateOCRAnalystOnly(dWeb);
                // Log
                string titleM = string.Format(Settings.AssignAnalystTitle, this.CurrentMainObject.ProjectName, this.CurrentMainObject.ProjectID);
                string bodyM = string.Empty;
                string presentName = SPHelper.GetSPUserFromLoginName(dWeb, this.hdfCurrentLogin.Value).Name;

                this.AddHistory(dWeb, this.CurrentMainObject, Settings.HistoryActionApproved, ProjectSettings.HisAssignedOCRA, this.hdfCurrentLogin.Value.Trim(), ProjectUtilities.GetCurrentDateTimeShortFormat(), this.ddlSubmitPermission.SelectedValue.Trim());

                bodyM = string.Format(Settings.AssignAnalystBody, presentName, ocrAnalyst.Name, ProjectUtilities.GetPagesFolderURL() + BART.SP.OCR.CP.Common.ProjectUtilities.ViewItemUrl(this.ReportId));
                ProjectUtilities.SendEmailToMultiple(ProjectUtilities.GetEmailByUser(this.HostedWeb.CurrentUser.LoginName.Trim()), toEmails,
                    titleM, bodyM, dWeb.Site, ccEmails);

               
            }
        }

        private void SubmitApprovalDecision()
        {

            try
            {
                string toEmails = string.Empty;
                string mddStatusAfter = string.Empty;
                //----------------------------------------------------------
                using (SPSite dSite = new SPSite(this.DataSiteURL))
                {
                    using (SPWeb dWeb = dSite.OpenWeb(this.DataWebRelativeURL))
                    {
                        this.CurrentMainObject = new MainObject(dWeb, this.hdf_MainObjID.Value.Trim(), true);
                        this.CurrentMainObject.MasterID = this.hdf_MainObjID.Value.Trim();
                        //--------------------------------------------------------------------------------------------------
                        bool hasApproveAccess = false;

                        TaskItemObject cTask = this.GetCurrentPendingTask(this.CurrentMainObject);
                        Dictionary<string, string> proxies = this.GetCurrentUserProxyPrimary(dWeb);
                        SPUser taskUser = cTask.AssignedTo;
                        if (taskUser != null)
                        {
                            if (proxies.Values.Contains(taskUser.LoginName.Trim()))
                                hasApproveAccess = true;
                        }
                        //--------------------------------------------------------------------------------------------------
                        if (hasApproveAccess)
                        {
                            //------
                            if (cTask.ApprovalTypeCode != ProjectSettings.TaskCodeOCRAnalystAssignment)
                            {
                                if (!string.IsNullOrEmpty(this.ddlSubmitPermission.SelectedValue))
                                {
                                    this.SubmitNormalTasks(dWeb, cTask);
                                }
                            }
                            else
                                this.SubmitAssignOCR(dWeb, cTask);
                            //---
                            this.LoadAll();
                            this.pnlSuccessMsg.Visible = true;
                        }
                        else
                        {
                            Response.Redirect(ProjectSettings.PageHome);
                        }
                        //
                        this.txtApprovalComments.Text = "";

                    }
                }
            }
            catch
            {
                //
            }
        }
        protected void btnActualClickActionApproval_Click(object sender, EventArgs e)
        {
            this.SubmitApprovalDecision();
        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("{0}&{1}={2}", BART.SP.OCR.CP.Common.ProjectUtilities.ExportMasterItemURL(this.hdf_MainObjID.Value.Trim()), ProjectSettings.QueryRevise, ProjectSettings.QueryReviseValue));
        }
    }
}
