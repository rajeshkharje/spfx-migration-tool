using BART.SP.OCR.CP.Base;
using BART.SP.OCR.CP.Common;
using Microsoft.SharePoint;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Telerik.Web.UI;

namespace BART.SP.OCR.CP.Web.Proxies
{
   
    public partial class ProxiesUserControl : ProjectUserControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                loadAllDefaultOptions();
            }
            this.lblCompleteMessage.Text = "";
        }
        //
        private void loadAllDefaultOptions()
        {
            this.hdfCurrentUserLogin.Value = this.HostedWeb.CurrentUser.LoginName;
        }
        private void LoadFilters(object obj, RadComboBox ddl, string[] fields)
        {
            DataTable dt = (DataTable) obj;
            DataView allViews = dt.DefaultView;
            ddl.DataSource = allViews.ToTable(true, fields);
            ddl.DataBind();

        }
        //
        private DataTable getSSWPTable()
        {
            DataTable dt = new DataTable();
            using (SPSite dSite = new SPSite(this.DataSiteURL))
            {
                using (SPWeb dWeb = dSite.OpenWeb(this.DataWebRelativeURL))
                {
                    if (this.ddlSearchOption.SelectedValue.Trim() == "0")
                        dt = ProjectHelper.GetAllProxyTable(dWeb);
                    else
                        dt = ProjectHelper.FilterValidProxies(ProjectHelper.GetAllProxyTable(dWeb));
                    dt.DefaultView.Sort = "StartDate DESC";
                    string filter = this.buildQuery();
                    if (!string.IsNullOrEmpty(filter))
                        dt.DefaultView.RowFilter = filter;
                }
            }
            if (this.ddlSearchOption.SelectedValue == "0")
                this.gridProxy.MasterTableView.NoMasterRecordsText = "You have no delegation";
            else
                this.gridProxy.MasterTableView.NoMasterRecordsText = "There is no active delegation matches your search/filter";
            return dt;
        }

        private string buildQuery()
        {
            if (this.ddlSearchOption.SelectedValue.Trim() == "0")
            {
                return string.Format("PrimaryUserLogin ='{0}'", this.hdfCurrentUserLogin.Value.Trim());
            }
            return string.Empty;
        }

        protected void gridSSWP_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == "DeleteProxyItem")
            {
                using (SPSite dSite = new SPSite(this.DataSiteURL))
                {
                    using (SPWeb dWeb = dSite.OpenWeb(this.DataWebRelativeURL))
                    {
                        SPListItem item = dWeb.Lists[this.ProxiesListTitle].Items.GetItemById(Convert.ToInt32(e.CommandArgument));
                        item.Recycle();
                    }
                }
                this.gridProxy.Rebind();
                this.lblCompleteMessage.Text = "Proxy Deleted successfully !";
                this.lblCompleteMessage.ForeColor = System.Drawing.Color.Green;
            }
        }
        protected void gridProxy_PreRender(object sender, System.EventArgs e)
        {
            foreach (GridFilteringItem filterItem in gridProxy.MasterTableView.GetItems(GridItemType.FilteringItem))
            {
                RadComboBox ddlPrimary = null;
                RadComboBox ddlProxy = null;
                ddlPrimary = (RadComboBox) filterItem.FindControl("ddlPrimaryUserFilter");
                ddlProxy = (RadComboBox) filterItem.FindControl("ddlProxyFilter");
                //
                if (ddlPrimary != null)
                    ddlPrimary.SelectedValue = gridProxy.MasterTableView.GetColumn("PrimaryUser").CurrentFilterValue;
                //if (ddlProxy != null)
                //    ddlProxy.SelectedValue = gridProxy.MasterTableView.GetColumn("Proxy").CurrentFilterValue;
            }
            if (this.ddlSearchOption.SelectedValue == "0")
                this.gridProxy.MasterTableView.GetColumn("DeleteProxyCol").Display = true;
            else
                this.gridProxy.MasterTableView.GetColumn("DeleteProxyCol").Display = false;

        }
        protected void radGrid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            (sender as RadGrid).DataSource = this.getSSWPTable();
        }
        protected void gridSSWP_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.FilteringItem)
            {
                GridFilteringItem filterItem = (GridFilteringItem) e.Item;
                RadComboBox ddlusers = filterItem.FindControl("ddlPrimaryUserFilter") as RadComboBox;
                if (ddlusers != null)
                    this.LoadFilters(this.gridProxy.DataSource, ddlusers, new string[] { "PrimaryUser" });
                //RadComboBox ddlProxies = filterItem.FindControl("ddlProxyFilter") as RadComboBox;
                //if (ddlProxies != null)
                //    this.LoadFilters(this.gridProxy.DataSource, ddlProxies, new string[] { "Proxy" });

            }

        }

        private void DeleteAllFilters()
        {
            foreach (GridColumn column in gridProxy.MasterTableView.OwnerGrid.Columns)
            {
                column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                column.CurrentFilterValue = string.Empty;
            }
            gridProxy.MasterTableView.FilterExpression = string.Empty;
            this.lblCompleteMessage.Text = "";
        }
        private void newSearch()
        {
            this.DeleteAllFilters();
            this.gridProxy.Rebind();
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

        protected void ddlSearchOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.DeleteAllFilters();
            this.gridProxy.Rebind();
        }

        protected void lbtClearAllFilters_Click(object sender, EventArgs e)
        {

        }

        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            this.DeleteAllFilters();
            this.gridProxy.Rebind();
            this.lblCompleteMessage.Text = "";
        }

        private void resetContrl()
        {
            this.txtExpiredDate.Text = string.Empty;
            this.txtStartDate.Text = string.Empty;
        }

        protected void btnAddProx_Click(object sender, EventArgs e)
        {

            DataTable dt = this.getSSWPTable();
            DateTime sDate = new DateTime();
            DateTime eDate = new DateTime();
            bool isValidDateTime = false;
            // bool isDateTimeOverlap = false;
            //
            using (SPSite dSite = new SPSite(this.DataSiteURL))
            {
                using (SPWeb dWeb = dSite.OpenWeb(this.DataWebRelativeURL))
                {

                    try
                    {
                        sDate = Convert.ToDateTime(this.txtStartDate.Text.Trim());
                        eDate = Convert.ToDateTime(this.txtExpiredDate.Text.Trim()).AddHours(23).AddMinutes(59).AddSeconds(59);
                        if (eDate > DateTime.Now && eDate >= sDate)
                        {
                            isValidDateTime = true;

                            //dt.DefaultView.RowFilter = string.Format("PrimaryUserLogin ='{0}'", this.hdfCurrentUserLogin.Value.Trim());
                            //dt = dt.DefaultView.ToTable();
                            //DataTable dtExistingOverlap = ProjectHelper.FilterProxiesWithADateRange(dt, sDate, eDate);
                            //if (dtExistingOverlap.Rows.Count < 1)
                            //    isValidDateTime = true;
                            //else
                            //    isDateTimeOverlap = true;
                        }

                    }
                    catch
                    {
                    }

                    if (isValidDateTime)
                    {
                        SPListItem item = dWeb.Lists[this.ProxiesListTitle].Items.Add();
                        item["PrimaryUser"] = SPHelper.GetSPUserFromLoginName(dWeb, this.hdfCurrentUserLogin.Value.Trim());
                        item["Proxy"] = SPHelper.GetSPUserFromLoginName(dWeb, this.hdfProxyAddedLogin.Value.Trim());
                        item["StartDate"] = sDate;
                        item["EndDate"] = eDate;
                        item["Status"] = "Active";
                        item.Update();
                        this.lblCompleteMessage.Text = "Delegation added successfully !";
                        this.lblCompleteMessage.ForeColor = System.Drawing.Color.Green;
                        resetContrl();
                    }
                    else
                    {
                        string errMsg = (!isValidDateTime) ? "Invalid Dates. Please check Start Date and End Date fields !" : string.Empty;
                        this.lblCompleteMessage.Text = errMsg;
                        this.lblCompleteMessage.ForeColor = System.Drawing.Color.Red;
                    }

                }
            }
            this.gridProxy.Rebind();
        }
    }
}
