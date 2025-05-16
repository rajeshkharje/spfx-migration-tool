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

namespace BART.SP.OCR.CP.Web.CreateNew
{
    public partial class CreateNewUserControl : ProjectUserControlBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    //
                    using (SPSite dSite = new SPSite(this.DataSiteURL))
                    {
                        using (SPWeb dWeb = dSite.OpenWeb(this.DataWebRelativeURL))
                        {
                            this.loadAllDefaultInfo(dWeb);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ProjectUtilities.LogError(ex.ToString());
                }
            }
            this.lblNoofContract.Text = Convert.ToString(Convert.ToInt32(this.hdfNoOfContracts.Value) + 1);
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
                this.CurrentMainObject.Requester = this.HostedWeb.CurrentUser;
                this.CurrentMainObject.RequesterName = this.HostedWeb.CurrentUser.Name;

                this.CurrentMainObject.UserCreated = this.HostedWeb.CurrentUser;
                this.CurrentMainObject.UserModified = this.HostedWeb.CurrentUser;
                this.hdf_RequesterLogin.Value = this.CurrentMainObject.Requester.LoginName;

                this.lbl_RequesterName.Text = this.HostedWeb.CurrentUser.Name;
                //------------------------------------------------
                this.hdfNoOfContracts.Value = Convert.ToString(this.CurrentMainObject.ContractDisplays.Count - 1);
                ViewState[ProjectSettings.ViewStateContracts] = this.CurrentMainObject.ContractDisplays;
                this.AddHiddenContracts();
                this.rpt_Contracts.DataSource = this.ContractDisplays;
                this.rpt_Contracts.DataBind();
                //-----------------------------------------------

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
                this.loadProjectsToList(web, this.ddlProjectList, this.ddl_ProgramName);
                this.loadProjectsToList(web, this.ddlHiddenProjectList);


            }
            catch (Exception ex)
            {
                ProjectUtilities.LogError(ex.ToString());
            }
        }

        protected void UpdateMainObjectFields(SPWeb dWeb)
        {
            this.txt_ServiceType.Text= this.SaveServiceType(this.cbx_ServiceType);
            PropertyInfo[] properties = this.CurrentMainObject.GetType().GetProperties();
            foreach (var propertyInfo in properties)
            {
                this.CompareControlFields(propertyInfo, this.pnlStep1.Controls, dWeb);
                this.CompareControlFields(propertyInfo, this.Controls, dWeb);
                // ------------------------------------------------------------------------------------------------------------------------
            }

            try
            {
                this.CurrentMainObject.ProjectName = this.ddlProjectList.SelectedItem.Text.Trim();
                this.CurrentMainObject.ProjectID = this.ddlProjectList.SelectedValue.Split('|')[0];
                this.CurrentMainObject.ProgramName = this.ddlProjectList.SelectedValue.Split('|')[1];
            }
            catch { }

            #region --- NOT USE

            //this.CurrentMainObject.ConsultantBType = this.cbx_ConsultantBType.SelectedValue;

            ////--- Step 1

            //this.CurrentMainObject.ExecutionType = (string.IsNullOrEmpty(this.cbx_ExecutionType.SelectedValue.Trim())) ? string.Empty : this.cbx_ExecutionType.SelectedValue.Trim();
            //this.CurrentMainObject.FromDate = Convert.ToDateTime(this.txt_FromDate.Text.Trim());
            //this.CurrentMainObject.ToDate = Convert.ToDateTime(this.txt_ToDate.Text.Trim());
            //this.CurrentMainObject.RenewOption = (string.IsNullOrEmpty(this.ddl_RenewOption.SelectedValue.Trim())) ? string.Empty : this.ddl_RenewOption.SelectedValue.Trim();
            ////this.CurrentMainObject.FundingOption = (string.IsNullOrEmpty(this.ddl_FundingOption.SelectedValue.Trim())) ? string.Empty : this.ddl_FundingOption.SelectedValue.Trim();

            //this.CurrentMainObject.PMLogin = (string.IsNullOrEmpty(this.hdf_PMLogin.Value.Trim())) ? string.Empty : this.hdf_PMLogin.Value.Trim();
            //this.CurrentMainObject.SDepartmentCode = (string.IsNullOrEmpty(this.ddl_SDepartmentCode.SelectedValue.Trim())) ? string.Empty : this.ddl_SDepartmentCode.SelectedValue.Trim();
            //this.CurrentMainObject.SDepartmentName = (string.IsNullOrEmpty(this.ddl_SDepartmentCode.SelectedValue.Trim())) ? string.Empty : this.ddl_SDepartmentCode.SelectedItem.Text.Trim();
            //this.CurrentMainObject.DeptManagerLogin = (string.IsNullOrEmpty(this.hdf_DeptManagerLogin.Value.Trim())) ? string.Empty : this.hdf_DeptManagerLogin.Value.Trim();
            //this.CurrentMainObject.isProcureddinPast5Yr = this.cb_isProcureddinPast5Yr_Yes.Checked;
            //this.CurrentMainObject.FutureNeeds = this.cbx_FutureNeeds.SelectedValue.Trim();

            ////------------------ Step 2
            //this.CurrentMainObject.SBUtilizationForFederal = this.cbx_SBUtilizationForFederal.SelectedValue.Trim();
            //this.CurrentMainObject.DBEUtilizationForFederal = this.cbx_DBEUtilizationForFederal.SelectedValue.Trim();
            //this.CurrentMainObject.CompetitiveSource = this.cb_CompetitiveSource_Yes.Checked;
            //this.CurrentMainObject.isCertifiedDBE = this.cb_isCertifiedDBE_Yes.Checked;
            //this.CurrentMainObject.isSBOnDGS = this.cb_isSBOnDGS_Yes.Checked;
            //this.CurrentMainObject.CompetitiveNo = this.cbx_CompetitiveNo.SelectedValue.Trim();
            //if (!string.IsNullOrEmpty(this.txt_CompetitiveNoOtherExpl.Text.Trim()))
            //    this.CurrentMainObject.CompetitiveNoOtherExpl = this.txt_CompetitiveNoOtherExpl.Text.Trim();
            //if (!string.IsNullOrEmpty(this.txt_CompetitiveYesSolDate.Text.Trim()))
            //    this.CurrentMainObject.CompetitiveYesSolDate = this.txt_CompetitiveYesSolDate.Text.Trim();

            //// ---------------- Step 3
            //this.CurrentMainObject.CostAnalysisType = this.ddl_CostAnalysisType.SelectedValue.Trim();
            //this.CurrentMainObject.CertifyCostFair = this.cbx_CertifyCostFair.SelectedValue.Trim();
            //this.CurrentMainObject.estimatedByLogin = this.hdf_estimatedByLogin.Value.Trim();
            //if (!string.IsNullOrEmpty(this.txt_estimatedDate.Text.Trim()))
            //    this.CurrentMainObject.estimatedDate = Convert.ToDateTime(this.txt_estimatedDate.Text.Trim());
            //this.CurrentMainObject.CheckOneWorkPerform = this.cbx_CheckOneWorkPerform.SelectedValue.Trim();
            //this.CurrentMainObject.CheckOne0886 = this.cbx_CheckOne0886.SelectedValue.Trim();
            //this.CurrentMainObject.RequestedByLogin = dWeb.CurrentUser.LoginName.Trim();
            //this.CurrentMainObject.OptionyearFunding = this.cb_OptionyearFunding.Checked;
            //if (!string.IsNullOrEmpty(this.txt_ChangeAgreementExpiredOn.Text.Trim()))
            //    this.CurrentMainObject.ChangeAgreementExpiredOn = Convert.ToDateTime(this.txt_ChangeAgreementExpiredOn.Text.Trim());

            #endregion --- END NOT USE

        }

        protected void rpt_Contracts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var ddlStatus = e.Item.FindControl("ddl_Status") as DropDownList;
                var txtStatus = e.Item.FindControl("hiddenStatus") as TextBox;
                var txtVisible = e.Item.FindControl("txtVisible") as TextBox;

                if (txtVisible.Visible)
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
                    ContractList list = new ContractList();
                    list.ListObjects = new List<Contract>();
                    Telerik.Web.UI.RadAsyncUpload ctrUpload = null;
                    foreach (RepeaterItem repeaterItem in this.rpt_Contracts.Items)
                    {
                        if (repeaterItem.Visible)
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

                            list.ListObjects.Add(new Contract(this.CurrentMainObject.MasterID.Trim(), itmID, order, fund, damount, duration, status, des, ocranalysis, contractno, targetdate, ctrUpload.UploadedFiles));
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
                        this.UpdateMainObjectFields(dWeb);
                        this.CurrentMainObject.Status = status;
                        if (sAction == ProjectSettings.ActionCodeRoute)
                        {
                            this.CurrentMainObject.CreateNewAndRoute(dWeb, ProjectSettings.ProjectStatusUnderReview, this.CtrlAttachment.UploadedFiles);
                            this.UpdateContracts(dWeb);
                            Response.Redirect(ProjectSettings.PageHome);
                            //ProjectHelper.AddHistory(dWeb, this.CurrentMainObject, Settings.HistoryTypeRouted, Settings.HistoryActionRouted, dWeb.CurrentUser.LoginName, ProjectUtilities.GetCurrentDateTimeShortFormat(), this.hdf_RequestedByLogin.Value);
                        }
                        else
                        {
                            this.CurrentMainObject.New(dWeb, status, this.CtrlAttachment.UploadedFiles);
                            this.UpdateContracts(dWeb);
                            string editURL = Common.ProjectUtilities.EditReportURL(this.CurrentMainObject.MasterID);
                            string ctab = string.IsNullOrEmpty(this.hdfCurrentTab.Text.Trim()) ? "step1" : Convert.ToString(this.hdfCurrentTab.Text.Trim().Replace("#", string.Empty));
                            Response.Redirect(string.Format("{0}&{1}={2}", editURL, Settings.TabQueryString, ctab));
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
            this.lblNoofContract.Text = Convert.ToString(this.rpt_Contracts.Items.Count);
            RepeaterItem item = this.rpt_Contracts.Items[this.CalculateContract(hdfNoOfContracts, true)];
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
