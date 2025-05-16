using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BART.SP.OCR.CP.Base;
using BART.SP.OCR.CP.Common;
using Telerik.Web.UI;
using Microsoft.SharePoint;
using System.Linq;

namespace BART.SP.OCR.CP.Web.MasterList
{
    public partial class MasterListUserControl : ProjectUserControlBase
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
            List<string> roles = new List<string>();
            string depts = string.Empty;
            string cURL = Request.Url.ToString().ToLower();
            if (cURL.Contains(ProjectSettings.PageHome.ToLower()))
            {
                this.hdfPageState.Value = ProjectSettings.ViewMy;
                this.lblActiveNav.Text = "My CP";
            }
            else if (cURL.Contains(ProjectSettings.PageAll.ToLower()))
            {
                // --------------------------------------------------------------//
                if (ProjectHelper.IfViewAllList(ref roles))
                {
                    this.hdfPageState.Value = ProjectSettings.ViewAllStandard;
                    this.lblActiveNav.Text = "All CP";
                    foreach(string s in roles)
                    {
                        if(!string.IsNullOrEmpty(s))
                        {
                            depts += string.Format("{0};", s.Trim());
                        }
                        //if (s.Trim().StartsWith("cp_") && s.Trim().EndsWith("_view"))
                        //{
                        //    depts += string.Format("{0};", s.Trim().Substring(3, s.Trim().Length - 8));
                        //}
                    }
                    //
                    this.hdfUserRoles.Value = depts.ToString().Replace(";;", ";").Trim(';');

                }
                
                else
                    Response.Redirect(ProjectSettings.PageHome);
            }
        }
        // -------------------------------------------------//
        private DataTable getItemsTable()
        {
            DataTable dt = new DataTable();
            using (SPSite dSite = new SPSite(this.DataSiteURL))
            {
                using (SPWeb dWeb = dSite.OpenWeb(this.DataWebRelativeURL))
                {
                    if (this.hdfPageState.Value == ProjectSettings.ViewMy)
                        dt = Common.ProjectHelper.GetPMItemsNMyItems(this.hdfCurrentUserLogin.Value, dWeb);
                    else if (this.hdfPageState.Value == ProjectSettings.ViewAllStandard)
                    {
                        List<string> depts = null;
                        if (!string.IsNullOrEmpty(this.hdfUserRoles.Value))
                        {
                            string roles = this.hdfUserRoles.Value.Trim();
                            if (!roles.Contains(Settings.AdminGroupName) && !roles.Contains(ProjectSettings.InternalGroupName))
                            {
                                depts = new List<string>();
                                foreach (string s in roles.Split(';'))
                                {
                                    if (s.Trim().StartsWith("cp_") && s.Trim().EndsWith("_view"))
                                    {
                                        depts.Add(string.Format("{0}", s.Trim().Substring(3, s.Trim().Length - 8)));
                                    }
                                }
                            }
                        }
                        dt = Common.ProjectHelper.GetReportsByStatusListnDepartmentTable(dWeb, new List<string>() { ProjectSettings.ProjectStatusApproved, ProjectSettings.ProjectStatusRejected, ProjectSettings.ProjectStatusUnderReview },depts);
                    }
                    if (dt == null || dt.Rows.Count < 1)
                    {
                        dt = ProjectUtilities.CreateDefaultPrjItemsTable();
                    }
                    dt.DefaultView.Sort = "Created DESC";
                    string filter = this.buildQuery();
                    if (!string.IsNullOrEmpty(filter))
                        dt.DefaultView.RowFilter = filter;
                }
            }
            return dt;
        }
        private void loadByCondition()
        {
            this.gridItem1.DataSource = this.getItemsTable();
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
            (sender as RadGrid).DataSource = this.getItemsTable();
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
    }
}
