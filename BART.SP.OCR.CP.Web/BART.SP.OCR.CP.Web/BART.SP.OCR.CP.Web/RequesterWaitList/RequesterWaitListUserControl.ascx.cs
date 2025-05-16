using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BART.SP.OCR.CP.Base;
using BART.SP.OCR.CP.Common;
using Microsoft.SharePoint;
using System.Linq;
using Telerik.Web.UI;

namespace BART.SP.OCR.CP.Web.RequesterWaitList
{
    public partial class RequesterWaitListUserControl : ProjectUserControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //DataTable dt = getMainItemTable();

            }
        }

        private DataTable CreateNewDataTableResult()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Program", typeof(string)));
            dt.Columns.Add(new DataColumn("Project", typeof(string)));
            dt.Columns.Add(new DataColumn("Project Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Submitted", typeof(string)));
            dt.Columns.Add(new DataColumn("Department", typeof(string)));
            dt.Columns.Add(new DataColumn("Initiator", typeof(string)));
            dt.Columns.Add(new DataColumn("OCR Analyst", typeof(string)));
            dt.Columns.Add(new DataColumn("PM", typeof(string)));
            dt.Columns.Add(new DataColumn("Status", typeof(string)));
            dt.Columns.Add(new DataColumn("Final Concurrance Date", typeof(string)));
            dt.Columns.Add(new DataColumn("Contract No", typeof(string)));
            dt.Columns.Add(new DataColumn("Dollar Amount", typeof(string)));
            dt.Columns.Add(new DataColumn("Funding Source", typeof(string)));
            dt.Columns.Add(new DataColumn("Target Completion Date", typeof(string)));
            dt.Columns.Add(new DataColumn("Contract Status", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("MasterID", typeof(string)));
            dt.Columns.Add(new DataColumn("Created", typeof(string)));
            return dt;

        }
        //-------------------------------------------------------------------------
        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            (sender as RadGrid).DataSource = this.getMainItemTable();
        }
        //-------------------------------------------------------------------------

        private DataTable getMainItemTable()
        {
            DataTable dtMainItems = new DataTable();
            DataTable dtContracts = new DataTable();
            DataTable result = CreateNewDataTableResult();

            List<string> listStatusAdded = new List<string>();

            using (SPSite dSite = new SPSite(this.DataSiteURL))
            {
                using (SPWeb dWeb = dSite.OpenWeb(this.DataWebRelativeURL))
                {

                    listStatusAdded.Add(ProjectSettings.ProjectStatusOnHold);
                    listStatusAdded.Add(ProjectSettings.ProjectStatusApproved);
                    listStatusAdded.Add(ProjectSettings.ProjectStatusCompleted);
                    listStatusAdded.Add(ProjectSettings.ProjectStatusCanceled);
                    listStatusAdded.Add(ProjectSettings.ProjectStatusUnderReview);

                    dtMainItems = ProjectHelper.GetReportsByStatusListnDepartmentTable(dWeb, listStatusAdded);
                    if (dtMainItems == null || dtMainItems.Rows.Count < 1)
                        dtMainItems = ProjectUtilities.CreateDefaultPrjItemsTable();

                    dtContracts = dWeb.Lists[Common.ProjectSettings.SPListProjectContracts].Items.GetDataTable();

                    try
                    {
                        var contracts = (from c in dtContracts.AsEnumerable()
                                         join s in dtMainItems.AsEnumerable() on c.Field<string>("MasterID") equals s.Field<string>("MasterID")
                                         select new { MasterID = s.Field<string>("MasterID"),
                                             ProgramDes = s.Field<string>("ProgramDes"),
                                             ProgramName = s.Field<string>("ProgramName"),
                                             ProjectID = s.Field<string>("ProjectID"),
                                             ProjectName = s.Field<string>("ProjectName"),
                                             DateSubmitted = s.Field<DateTime?>("DateSubmitted"),
                                             Created = s.Field<DateTime?>("Created"),
                                             ApprovedDate = s.Field<DateTime?>("ApprovedDate"),
                                             SponsorDepartment = s.Field<string>("SponsorDepartment"),
                                             Requester_Assigned = s.Field<string>("Requester_Assigned"),
                                             OCRAnalyst_Assigned = s.Field<string>("OCRAnalyst_Assigned"),
                                             SponsorProjectManager = s.Field<string>("SponsorProjectManager"),
                                             Status = s.Field<string>("Status"),
                                             ContractNo = c.Field<string>("ContractNo"),
                                             DollarAmount = c.Field<string>("DollarAmount"),
                                             FundingSource = c.Field<string>("FundingSource"),
                                             TargetCompletionDate = c.Field<DateTime?>("TargetCompletionDate"),
                                             ContractStatus = c.Field<string>("Status"),
                                             Description = c.Field<string>("Description"),
                                         });

                        foreach (var item in contracts)
                        {
                            DataRow dr = result.NewRow();
                            string mid = item.MasterID;
                            dr["MasterID"] = mid;
                            dr["Program"] = item.ProgramDes;
                            dr["Project"] = item.ProjectID;
                            dr["Project Name"] = item.ProjectName;
                            DateTime? sDate = item.DateSubmitted;
                            DateTime? dDate = item.Created;
                            DateTime? approvedDate = item.ApprovedDate;
                            DateTime? TargetCompletionDate = item.TargetCompletionDate;

                            dr["Submitted"] = (sDate == null) ? string.Empty : Convert.ToDateTime(sDate).ToShortDateString();
                            dr["Created"] = (dDate == null) ? string.Empty : Convert.ToDateTime(dDate).ToShortDateString();
                            dr["Final Concurrance Date"] = (approvedDate == null) ? string.Empty : Convert.ToDateTime(approvedDate).ToShortDateString();
                            dr["Target Completion Date"] = (TargetCompletionDate == null) ? string.Empty : Convert.ToDateTime(TargetCompletionDate).ToShortDateString();

                            dr["Department"] = item.SponsorDepartment;
                            dr["Initiator"] = item.Requester_Assigned;
                            dr["OCR Analyst"] = item.OCRAnalyst_Assigned;
                            dr["PM"] = item.SponsorProjectManager;
                            dr["Status"] = item.Status;

                            /////////---------------------------------------------------

                            //dr["Final Concurrance Date"] = item.ApprovedDate;
                            dr["Contract No"] = item.ContractNo;
                            dr["Dollar Amount"] = item.DollarAmount;
                            dr["Funding Source"] = item.FundingSource;
                            
                            dr["Contract Status"] = item.ContractStatus;
                            dr["Description"] = item.Description;

                            result.Rows.Add(dr);
                        }
                    }
                    catch(Exception ex)
                    {
                        result = ProjectUtilities.CreateApprovalTable();
                    }
                    //--------------------------------------------------------------------------------------------------------------//
                    result.DefaultView.Sort = "MasterID DESC";
                }
            }
            return result;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            //RadGrid1.ExportSettings.ExportOnlyData = true;
            //RadGrid1.ExportSettings.Excel.Format = (GridExcelExportFormat) Enum.Parse(typeof(GridExcelExportFormat), "Xlsx");
            //RadGrid1.ExportSettings.OpenInNewWindow = true;
            RadGrid1.MasterTableView.ExportToExcel();
        }
    }
}
