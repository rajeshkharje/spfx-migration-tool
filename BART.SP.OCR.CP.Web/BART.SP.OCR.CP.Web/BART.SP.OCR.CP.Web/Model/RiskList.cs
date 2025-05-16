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

//    public class RiskList
//    {
//        public string RRReportId { get; set; }
//        public List<Risk> ListObjects { get; set; }
//        public RiskList() { this.ListObjects = new List<Risk>(); }
//        public RiskList(SPWeb dWeb, string reportId)
//        {

//            try
//            {
//                this.RRReportId = reportId;
//                this.ListObjects = new List<Risk>();
//                SPListItemCollection items = null;
//                SPList executiveSummaries = dWeb.Lists[Common.ProjectSettings.SPListRisks];
//                StringBuilder sb = new StringBuilder();
//                sb.Append("<Where><And><Eq><FieldRef Name = 'RRReportId' /><Value Type = 'Text'>" + reportId + "</Value></Eq>");
//                sb.Append("<Neq><FieldRef Name='Status'/><Value Type='Text'>"+ ProjectSettings.StatusDeleted+"</Value></Neq>");
//                sb.Append("</And></Where>");
//                SPQuery query = new SPQuery();
//                query.Query = sb.ToString();
//                // Get data from a list.
//                items = executiveSummaries.GetItems(query);

//                if (items != null && items.Count > 0)
//                {
//                    foreach (SPListItem item in items)
//                        ListObjects.Add(new Risk(item));
//                }
//            }
//            catch (Exception ex)
//            {
//                Common.ProjectUtilities.LogError(ex.ToString());
//            }
//        }

//        //
//        public void CreateAll(SPWeb dWeb)
//        {
//            try
//            {
//                if (ListObjects != null && ListObjects.Count > 0 && !string.IsNullOrEmpty(this.RRReportId))
//                {
//                    foreach (var item in ListObjects)
//                    {
//                        item.RRReportId = this.RRReportId;
//                        item.New(dWeb);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Common.ProjectUtilities.LogError(ex.ToString());
//            }

//        }
//        public void UpdateAll(SPWeb dWeb)
//        {
//            try
//            {
//                if (ListObjects != null && ListObjects.Count > 0 && !string.IsNullOrEmpty(this.RRReportId))
//                {
//                    foreach (var item in ListObjects)
//                    {
//                        item.Update(dWeb);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Common.ProjectUtilities.LogError(ex.ToString());
//            }

//        }
//        public void DeleteAll(SPWeb dWeb)
//        {
//            try
//            {
//                if (ListObjects != null && ListObjects.Count > 0 && !string.IsNullOrEmpty(this.RRReportId))
//                {
//                    foreach (var item in ListObjects)
//                    {
//                        item.Delete(dWeb);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Common.ProjectUtilities.LogError(ex.ToString());
//            }

//        }

//    }
//}
