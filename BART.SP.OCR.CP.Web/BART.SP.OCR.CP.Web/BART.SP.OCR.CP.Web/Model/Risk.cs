//using Microsoft.SharePoint;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using BART.SP.OCR.CP.Common;

//namespace BART.SP.OCR.CP.Model
//{
//    [Serializable]

//    public class Risk
//    {
//        public int OrderInTable { get; set; }
//        public string IssueType { get; set; }
//        public string Description { get; set; }
//        public string Rating { get; set; }
//        public bool CostImpact { get; set; }
//        public bool ScheduleImpact { get; set; }
//        public string Mitigation { get; set; }
//        public string RRReportId { get; set; }
//        public string ItemID { get; set; }

//        public Risk(int order, string issueType,string des,  string rating, bool cImpact, bool sImpact, string mitigation, string rrReportId = "")
//        {
//            this.OrderInTable = order;
//            if (!string.IsNullOrEmpty(issueType)) this.IssueType = issueType;
//            if (!string.IsNullOrEmpty(mitigation)) this.Mitigation = mitigation;
//            if (!string.IsNullOrEmpty(rating)) this.Rating = rating;
//            if (!string.IsNullOrEmpty(des)) this.Description = des;
//            this.CostImpact = cImpact;
//            this.ScheduleImpact = sImpact;
//            if(!string.IsNullOrEmpty(rrReportId))
//                this.RRReportId = rrReportId;
//        }

//        public Risk(SPListItem item)
//        {
//            InitObject(item.Web, Convert.ToString(item.ID), item);
//        }
//        public Risk(SPWeb dWeb, string itemId)
//        {
//            InitObject(dWeb, itemId);
//        }
//        private void InitObject(SPWeb dWeb, string itemId, SPListItem oListItem=null)
//        {
//            if (oListItem == null)
//            {
//                SPList list = dWeb.Lists[Common.ProjectSettings.SPListRRRisks];
//                oListItem = list.GetItemById(Convert.ToInt32(itemId));
//            }
//            this.OrderInTable = Convert.ToInt32(oListItem["OrderInTable"]);
//            this.RRReportId = Convert.ToString(oListItem["RRReportId"]);
//            //
//            this.IssueType = Convert.ToString(oListItem["IssueType"]);
//            this.Description = Convert.ToString(oListItem["Description"]);
//            this.Rating = Convert.ToString(oListItem["Rating"]);
//            this.CostImpact = Convert.ToBoolean(oListItem["CostImpact"]);
//            this.ScheduleImpact = Convert.ToBoolean(oListItem["ScheduleImpact"]);
//            this.Mitigation = Convert.ToString(oListItem["Mitigation"]);
//            if (!string.IsNullOrEmpty(itemId))
//                this.ItemID = itemId;
//        }

//        public bool New(SPWeb dWeb)
//        {
//            try
//            {
//                SPList list = dWeb.Lists[Common.ProjectSettings.SPListRisks];
//                SPListItem oListItem = list.Items.Add();
//                //
//                if (this.OrderInTable > 0)
//                    oListItem["OrderInTable"] = this.OrderInTable;
//                if (!string.IsNullOrEmpty(this.RRReportId))
//                    oListItem["RRReportId"] = this.RRReportId;
//                //
//                if (!string.IsNullOrEmpty(this.IssueType))
//                {
//                    oListItem["Title"] = ProjectUtilities.trimTitleField(this.IssueType);
//                    oListItem["IssueType"] = this.IssueType.Trim();
//                }
//                //
//                if (!string.IsNullOrEmpty(this.Description))
//                    oListItem["Description"] = this.Description;
//                if (!string.IsNullOrEmpty(this.Rating))
//                    oListItem["Rating"] = this.Rating;
//                if (!string.IsNullOrEmpty(this.Mitigation))
//                    oListItem["Mitigation"] = this.Mitigation;
//                oListItem["CostImpact"] = this.CostImpact;
//                oListItem["ScheduleImpact"] = this.ScheduleImpact;
//                // ------------------------------------------------------------------------//
//                oListItem.SystemUpdate();
//                this.ItemID = Convert.ToString(oListItem["ID"]);
//                return true;
//            }
//            catch (Exception ex)
//            {
//                ProjectUtilities.LogError(ex.ToString());
//            }
//            return false;
//        }
//        public bool Update(SPWeb dWeb)
//        {
//            try
//            {
//                SPList list = dWeb.Lists[Common.ProjectSettings.SPListRRRisks];
//                SPListItem oListItem = list.Items.GetItemById(Convert.ToInt32(this.ItemID));
//                //
//                if (this.OrderInTable > 0)
//                    oListItem["OrderInTable"] = this.OrderInTable;
//                if (!string.IsNullOrEmpty(this.RRReportId))
//                    oListItem["RRReportId"] = this.RRReportId;
//                //--------------------------------------------------
//                if (!string.IsNullOrEmpty(this.IssueType))
//                {
//                    oListItem["Title"] = ProjectUtilities.trimTitleField(this.IssueType);
//                    oListItem["IssueType"] = this.IssueType.Trim();
//                }
//                else
//                {
//                    oListItem["IssueType"] = string.Empty;
//                }
//                //
//                oListItem["Description"] = !string.IsNullOrEmpty(this.Description) ? this.Description:string.Empty;
//                oListItem["Rating"] = !string.IsNullOrEmpty(this.Rating) ? this.Rating : string.Empty;
//                oListItem["Mitigation"] = !string.IsNullOrEmpty(this.Mitigation) ? this.Mitigation : string.Empty;
//                oListItem["CostImpact"] = this.CostImpact;
//                oListItem["ScheduleImpact"] = this.ScheduleImpact;
//                // ------------------------------------------------------------------------//
//                oListItem.SystemUpdate();
//                return true;
//            }
//            catch (Exception ex)
//            {
//                ProjectUtilities.LogError(ex.ToString());
//            }
//            return false;
//        }
//        public bool Delete(SPWeb dWeb)
//        {
//            try
//            {
//                SPList list = dWeb.Lists[Common.ProjectSettings.SPListRRRisks];
//                SPListItem oListItem = list.GetItemById(Convert.ToInt32(this.ItemID));
//                oListItem.Recycle();
//                return true;
//            }
//            catch (Exception ex)
//            {
//                ProjectUtilities.LogError(ex.ToString());
//            }
//            return false;
//        }
//    }

//    [Serializable]
//    public class RiskDisplay
//    {
//        public int OrderInTable { get; set; }
//        public string IssueType { get; set; }
//        public string Description { get; set; }
//        public string Rating { get; set; }
//        public string CostImpact { get; set; }
//        public string ScheduleImpact { get; set; }
//        public string Mitigation { get; set; }
//        public string RRReportId { get; set; }
//        public string ItemID { get; set; }

//        public RiskDisplay()
//        {
//            this.OrderInTable = -1;
//            this.IssueType = string.Empty;
//            this.Mitigation = string.Empty;
//            this.Rating = string.Empty;
//            this.Description = string.Empty;
//            this.CostImpact = string.Empty;
//            this.ScheduleImpact = string.Empty;
//            this.RRReportId = string.Empty;
//            this.ItemID = string.Empty;
//        }

//        public RiskDisplay( Risk r)
//        {
//            this.IssueType = r.IssueType;
//            this.Description = r.Description;
//            this.OrderInTable = r.OrderInTable;
//            this.ItemID = r.ItemID;
//            this.Mitigation = r.Mitigation;
//            this.RRReportId = r.RRReportId;
//            this.ItemID = r.ItemID;
//            // ----------------------------------
//            if (string.IsNullOrEmpty(r.IssueType) && string.IsNullOrEmpty(r.Description))
//            {
//                this.CostImpact = string.Empty;
//                this.ScheduleImpact = string.Empty;
//                this.Rating = string.Empty;
//                this.Mitigation = string.Empty;
//            }
//            else
//            {
//                this.CostImpact = (r.CostImpact) ? "YES" : "NO";
//                this.ScheduleImpact = (r.ScheduleImpact) ? "YES" : "NO";
//                this.Rating = r.Rating;
//            }
//        }
       

//public RiskDisplay(int order, string issueType, string des, string rating, string cImpact, string sImpact, string mitigation, string rrReportId = "")
//        {
//            this.OrderInTable = order;
//            if (!string.IsNullOrEmpty(issueType)) this.IssueType = issueType;
//            if (!string.IsNullOrEmpty(mitigation)) this.Mitigation = mitigation;
//            if (!string.IsNullOrEmpty(rating)) this.Rating = rating;
//            if (!string.IsNullOrEmpty(des)) this.Description = des;
//            this.CostImpact = cImpact;
//            this.ScheduleImpact = sImpact;
//            if (!string.IsNullOrEmpty(rrReportId))
//                this.RRReportId = rrReportId;
//        }
//    }

//}



