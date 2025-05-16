using BART.SP.OCR.CP.Base;
using BART.SP.OCR.CP.Common;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Telerik.Web.UI;

namespace BART.SP.OCR.CP.Web.ApprovalList
{
      public partial class ApprovalListUserControl : ProjectUserControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                loadAllDefaultOptions();
            }
        }
        //
        private void loadAllDefaultOptions()
        {
            this.hdfCurrentUserDisplayName.Value = this.HostedWeb.CurrentUser.Name;
            this.hdfCurrentUserLogin.Value = this.HostedWeb.CurrentUser.LoginName;
            string cURL = Request.Url.ToString().ToLower();
            this.hdfPageState.Value = this.ddlFilterByOptions.SelectedValue;
            this.LoadDefaultStatus();
        }
        private void LoadDefaultStatus()
        {
            List<string> defVal = new List<string>();
            if (this.hdfPageState.Value == Settings.MyPendingTaskOnly)
            {
                defVal.Add(ProjectSettings.ProjectStatusUnderReview);
            }
            else if (this.hdfPageState.Value == Settings.AllPendingTasks)
            {
                defVal.Add(ProjectSettings.ProjectStatusUnderReview);
            }
            else
            {
                defVal.Add(ProjectSettings.ProjectStatusUnderReview);
                defVal.Add(ProjectSettings.ProjectStatusRejected);
                defVal.Add(ProjectSettings.ProjectStatusOnHold);
                defVal.Add(ProjectSettings.ProjectStatusRejected);
                defVal.Add(ProjectSettings.ProjectStatusCompleted);
            }
            //this.LoadInfo(defVal);
        }
        //
        private DataTable getMainItemTable()
        {
            DataTable dtMainItems = new DataTable();
            DataTable dtTasks = new DataTable();
            DataTable result = ProjectUtilities.CreateApprovalTable();
            List<string> listStatusAdded = new List<string>();
            using (SPSite dSite = new SPSite(this.DataSiteURL))
            {
                using (SPWeb dWeb = dSite.OpenWeb(this.DataWebRelativeURL))
                {

                    if (this.ddlFilterByOptions.SelectedValue.Equals(Settings.MyPendingTaskOnly) || this.ddlFilterByOptions.SelectedValue.Equals(Settings.AllPendingTasks))
                    {
                        listStatusAdded.Add(ProjectSettings.ProjectStatusUnderReview);
                    }
                    else
                    {
                        listStatusAdded.Add(ProjectSettings.ProjectStatusUnderReview);
                        listStatusAdded.Add(ProjectSettings.ProjectStatusOnHold);
                        listStatusAdded.Add(ProjectSettings.ProjectStatusApproved);
                        listStatusAdded.Add(ProjectSettings.ProjectStatusCompleted);
                        listStatusAdded.Add(ProjectSettings.ProjectStatusCanceled);
                    }

                    dtMainItems = ProjectHelper.GetReportsByStatusListnDepartmentTable(dWeb, listStatusAdded);
                    if (dtMainItems == null || dtMainItems.Rows.Count < 1)
                        dtMainItems = ProjectUtilities.CreateDefaultPrjItemsTable();
                    try
                    {
                        dtTasks = ProjectHelper.GetTasksForTasksPage(this.hdfCurrentUserLogin.Value, dWeb, this.ddlFilterByOptions.SelectedValue.Trim());
                    }
                    catch
                    {
                        dtTasks = ProjectUtilities.CreateTableTasks();
                    }

                    try
                    {
                        var approvals = (from s in dtMainItems.AsEnumerable()
                                         join t in dtTasks.AsEnumerable() on s.Field<string>("MasterID") equals t.Field<string>("MasterID")
                                         select s);
                        foreach (var app in approvals)
                        {
                            DataRow dr = result.NewRow();
                            string mid = Convert.ToString(app.Field<string>("MasterID")).Trim();
                            dr["MasterID"] = mid;
                            dr["ProjectName"] = app.Field<string>("ProjectName");
                            DateTime? sDate = app.Field<DateTime?>("DateSubmitted");
                            DateTime? dDate = app.Field<DateTime?>("Created");
                            dr["DateSubmitted"] = (sDate == null) ? string.Empty : Convert.ToDateTime(sDate).ToShortDateString();
                            dr["ProgramName"] = app.Field<string>("ProgramName");
                            dr["ProgramDes"] = app.Field<string>("ProgramDes");
                            dr["ProjectName"] = app.Field<string>("ProjectName");
                            dr["ProjectID"] = app.Field<string>("ProjectID");

                            dr["Status"] = app.Field<string>("Status");
                            dr["SponsorDepartment"] = app.Field<string>("SponsorDepartment");
                            dr["SponsorProjectManager"] = app.Field<string>("SponsorProjectManager");

                            dr["Requester"] = app.Field<string>("Requester");
                            dr["Requester_Assigned"] = app.Field<string>("Requester_Assigned");
                            dr["OCRAnalyst"] = app.Field<string>("OCRAnalyst");
                            dr["OCRAnalyst_Assigned"] = app.Field<string>("OCRAnalyst_Assigned");
                            dr["Created"] = Convert.ToDateTime(dDate); 
                            dr["Status"] = app.Field<string>("Status");
                            dr["Modified"] = app.Field<DateTime?>("Modified");
                            dr["isEditable"] = false;
                            result.Rows.Add(dr);
                        }
                    }
                    catch
                    {
                        result = ProjectUtilities.CreateApprovalTable();
                    }
                    //-------------------------------------------------------------------------------------------------------------//
                    if (result == null || result.Rows == null || result.Rows.Count < 1)
                    {
                        if (this.ddlFilterByOptions.SelectedValue.Trim() == Settings.AllPendingTasks)
                            this.gridItem1.MasterTableView.NoMasterRecordsText = "You have no pending Contracting Plan approval Task";
                        else
                            this.gridItem1.MasterTableView.NoMasterRecordsText = "You have not taken any Contracting Plan approval Task";
                    }
                    else
                    {
                        this.gridItem1.MasterTableView.NoMasterRecordsText = Settings.NoRecordFoundSearch;
                    }
                    //--------------------------------------------------------------------------------------------------------------//
                    result.DefaultView.Sort = "Created DESC";
                    string filter = this.buildQuery();
                    if (!string.IsNullOrEmpty(filter))
                        result.DefaultView.RowFilter = filter;
                }
            }
            return result;
        }
        private void loadByCondition()
        {
            this.gridItem1.DataSource = this.getMainItemTable();
            this.gridItem1.MasterTableView.Rebind();
        }
        protected void gridItem1_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.FilterCommandName)
            {
                Pair filterPair = (Pair) e.CommandArgument;

                switch (filterPair.Second.ToString())
                {
                    case "DateSubmitted":
                        this.startDate = ((e.Item as GridFilteringItem)[filterPair.Second.ToString()].FindControl("FromSubmittedDatePicker") as RadDatePicker).SelectedDate;
                        this.endDate = ((e.Item as GridFilteringItem)[filterPair.Second.ToString()].FindControl("ToSubmittedDatePicker") as RadDatePicker).SelectedDate.Value;
                        break;
                }
            }
        }
        protected void gridItem1_PreRender(object sender, System.EventArgs e)
        {
            //if (gridItem1.MasterTableView.FilterExpression != string.Empty)
            //{
            //    loadSSWPByCondition();
            //}

            foreach (GridFilteringItem filterItem in gridItem1.MasterTableView.GetItems(GridItemType.FilteringItem))
            {
                RadComboBox ddlRequestors = null;
                ddlRequestors = (RadComboBox) filterItem.FindControl("ddlRequestorFilter");
                if (ddlRequestors != null)
                    ddlRequestors.SelectedValue = gridItem1.MasterTableView.GetColumn("Requester").CurrentFilterValue;

                RadComboBox ddlPM = null;
                ddlPM = (RadComboBox) filterItem.FindControl("ddlPM");
                if (ddlPM != null)
                    ddlPM.SelectedValue = gridItem1.MasterTableView.GetColumn("SponsorProjectManager").CurrentFilterValue;


                RadComboBox ddlOCRAnalyst = null;
                ddlOCRAnalyst = (RadComboBox) filterItem.FindControl("ddlOCRAnalyst");
                if (ddlOCRAnalyst != null)
                    ddlOCRAnalyst.SelectedValue = gridItem1.MasterTableView.GetColumn("OCRAnalyst").CurrentFilterValue;

                //
                RadComboBox ddlDepts = null;
                ddlDepts = (RadComboBox) filterItem.FindControl("ddlDeptNoFilter");
                if (ddlDepts != null)
                    ddlDepts.SelectedValue = gridItem1.MasterTableView.GetColumn("SponsorDepartment").CurrentFilterValue;


                RadComboBox ddlStatus = null;
                ddlStatus = (RadComboBox) filterItem.FindControl("ddlStatusNoFilter");
                if (ddlStatus != null)
                    ddlStatus.SelectedValue = gridItem1.MasterTableView.GetColumn("Status").CurrentFilterValue;


                RadComboBox ddlProgramName = null;
                ddlProgramName = (RadComboBox) filterItem.FindControl("ddlProgramNameFilter");
                if (ddlProgramName != null)
                    ddlProgramName.SelectedValue = gridItem1.MasterTableView.GetColumn("ProgramDes").CurrentFilterValue;
            }

            //------
            try
            {
                if (this.hdfPageState.Value.Equals(Settings.ViewAllStandard))
                    gridItem1.MasterTableView.GetColumn("EditInfo").Display = false;
            }
            catch (Exception ex)
            {
                //
            }

        }
        private void LoadFilters(object obj, RadComboBox ddl, string[] fields, string filterby = "", bool sortASC = true)
        {
            DataTable dt = (DataTable) obj;
            DataView allViews = dt.DefaultView;
            try
            {
                if (!string.IsNullOrEmpty(filterby))
                {
                    if (sortASC)
                        allViews.Sort = filterby + " ASC";
                    else
                        allViews.Sort = filterby + " DESC";
                }
            }
            catch
            { }
            ddl.DataSource = allViews.ToTable(true, fields);
            ddl.DataBind();

        }
        protected void radGrid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            (sender as RadGrid).DataSource = this.getMainItemTable();
        }
        protected void gridItem_DataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.FilteringItem)
            {
                GridFilteringItem filterItem = (GridFilteringItem) e.Item;

                RadComboBox ddlDept = filterItem.FindControl("ddlDeptNoFilter") as RadComboBox;
                if (ddlDept != null)
                    this.LoadFilters(this.gridItem1.DataSource, ddlDept, new string[] { "SponsorDepartment" });

                RadComboBox ddlRequestor = filterItem.FindControl("ddlRequestorFilter") as RadComboBox;
                if (ddlRequestor != null)
                    this.LoadFilters(this.gridItem1.DataSource, ddlRequestor, new string[] { "Requester_Assigned" });

                RadComboBox ddlPM = filterItem.FindControl("ddlPM") as RadComboBox;
                if (ddlPM != null)
                    this.LoadFilters(this.gridItem1.DataSource, ddlPM, new string[] { "SponsorProjectManager" });

                RadComboBox ddlOCRAnalyst = (RadComboBox) filterItem.FindControl("ddlOCRAnalyst");
                if (ddlOCRAnalyst != null)
                    this.LoadFilters(this.gridItem1.DataSource, ddlOCRAnalyst, new string[] { "OCRAnalyst_Assigned" });

                RadComboBox ddlStatus = filterItem.FindControl("ddlStatusNoFilter") as RadComboBox;
                if (ddlStatus != null)
                    this.LoadFilters(this.gridItem1.DataSource, ddlStatus, new string[] { "Status" });

                RadComboBox ddlProgramName = filterItem.FindControl("ddlProgramNameFilter") as RadComboBox;
                if (ddlProgramName != null)
                    this.LoadFilters(this.gridItem1.DataSource, ddlProgramName, new string[] { "ProgramDes" });
            }

        }
        private string buildQuery()
        {
            string filterVal = string.Empty;
            if (this.hdfSearchVal.Text.Trim() != string.Empty)
            {
                filterVal = string.Format("ProjectName Like '%{0}%' OR UserCreated Like '%{0}%' OR SponsorProjectManager Like '%{0}%' OR ProjectID Like '%{0}%' OR ProgramDes Like '%{0}%'", this.hdfSearchVal.Text.Trim());
            }
            return filterVal;
        }

        protected DateTime? startDate
        {
            set
            {
                ViewState["strD"] = value;
            }
            get
            {
                if (ViewState["strD"] != null)
                {
                    return ((DateTime) ViewState["strD"]);
                }

                else
                    return new DateTime?();
            }
        }
        protected DateTime? endDate
        {
            set
            {
                ViewState["endD"] = value;
            }
            get
            {
                if (ViewState["endD"] != null)
                    return ((DateTime) ViewState["endD"]);
                else
                    return new DateTime?();
            }
        }
        protected void linkClearAllfilters_ServerClick(object sender, EventArgs e)
        {
            this.hdfSearchVal.Text = string.Empty;
            this.txtSearch.Value = string.Empty;
            this.DeleteAllFilters();
            this.gridItem1.Rebind();
        }
        private void DeleteAllFilters()
        {
            foreach (GridColumn column in gridItem1.MasterTableView.OwnerGrid.Columns)
            {
                column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                column.CurrentFilterValue = string.Empty;
            }
            ViewState["strD"] = null;
            ViewState["endD"] = null;
            gridItem1.MasterTableView.FilterExpression = string.Empty;
        }
        private void newSearch()
        {
            this.hdfSearchVal.Text = this.txtSearch.Value.Trim();
            this.DeleteAllFilters();
            this.gridItem1.Rebind();
        }
        protected void btnSSWPSearch_ServerClick(object sender, EventArgs e)
        {
            newSearch();
        }
        bool isFilteredStatus = false;
        protected void btnDefaultSearch_Click(object sender, EventArgs e)
        {
            newSearch();
        }
        protected void ddlFilterByOptions_SelectedIndexChanged(object sender, EventArgs e)
        {


            this.loadAllDefaultOptions();
            this.gridItem1.DataSource = getMainItemTable();
            this.gridItem1.DataBind();
        }
    }
}
