using BART.SP.OCR.CP.Model;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Telerik.Web.UI;

namespace BART.SP.OCR.CP.Common
{
    public class ProjectHelper
    {

        public static string DataSiteURL
        {
            get { return string.Format("{0}{1}", GetServerURL(), Common.Settings.DataSiteRelativeURL); }
        }
        public static string DataWebRelativeURL
        {
            get { return Common.Settings.DataWebRelativeURL; }
        }
        public static string ArchivedDocsSiteURL
        {
            get { return string.Format("{0}{1}", GetServerURL(), Common.Settings.ArchivedDocsSiteRelativeURL); }
        }
        public static string ArchivedDocsWebURL
        {
            get { return Common.Settings.ArchivedDocsWebRelativeURL; }
        }
        public static SPWeb AppHostedWeb
        {
            get { return GetHostedWeb(); }
        }
        // Use this site through the project
        public static SPSite AppHostedSite
        {
            get { return GetHostedSite(); }
        }
        private static SPWeb GetHostedWeb()
        {
            return SPContext.Current.Web;
        }
        private static SPSite GetHostedSite()
        {
            return SPContext.Current.Site;
        }

        public static string GetServerURL()
        {
            try
            {
                return string.Format("{0}/", GetHostedSite().Url.ToLower().Substring(0, (GetHostedSite().Url.Length - GetHostedSite().ServerRelativeUrl.Length)));
            }
            catch
            {
                //
            }
            return (string.IsNullOrEmpty(GetHostedSite().ServerRelativeUrl) || GetHostedSite().ServerRelativeUrl == "/" || GetHostedSite().ServerRelativeUrl == "//") ? string.Format("{0}/", GetHostedSite().Url.ToLower().Trim()) : string.Format("{0}/", GetHostedSite().Url.ToLower().Trim().Replace(GetHostedSite().ServerRelativeUrl.ToLower().Trim(), string.Empty));
        }
        //
        /*protected string GetEmailFromLoginName(string account)
        {
            int index = account.LastIndexOf(@"\");
            index = index + 1;
            // Send email to first user
            return string.Format("{0}@bart.gov", account.Substring(index));
        }*/
      
        public static SPUser GetSPUserValueByFieldName(SPListItem item, string fieldName)
        {
            if (item != null)
            {
                if (item[fieldName] != null)
                {
                    SPFieldUserValue userValue = new SPFieldUserValue(item.ParentList.ParentWeb, Convert.ToString(item[fieldName]));
                    return userValue.User;
                }
            }
            //
            return null;
        }
             
        public static DataTable getSSWPAttachments(string sswpID)
        {
            DataTable docs = new DataTable();
            //
            using (SPSite site = new SPSite(DataSiteURL))
            {
                using (SPWeb web = site.OpenWeb(DataWebRelativeURL))
                {

                    docs = getSSWPAttachmentByID(sswpID, web);
                }
            }
            //
            return docs;
        }
        //
        public static DataTable getSSWPAttachmentByID(string sswpID, SPWeb web)
        {
            string fileIcon = "docIcon";
            string fileNameField = "FileName";
            string fileUrlField = "Url";
            string fileIdField = "DocId";

            DataTable fileTable = new DataTable();
            fileTable.Columns.Add(new DataColumn(fileNameField, typeof(string)));
            fileTable.Columns.Add(new DataColumn(fileUrlField, typeof(string)));
            fileTable.Columns.Add(new DataColumn(fileIdField, typeof(string)));
            fileTable.Columns.Add(new DataColumn(fileIcon, typeof(string)));
            // Build a query.
            string cQuery = string.Empty;
            cQuery = "<Where><Eq><FieldRef Name='SSWPID'/><Value Type='Text'>" + sswpID.Trim() + "</Value></Eq></Where>";
            SPQuery query = new SPQuery();
            query.Query = cQuery;
            SPListItemCollection items = SPHelper.GetItems(Settings.SSWPAttachments, query, web);
            if (items != null)
            {
                foreach (SPListItem item in items)
                {
                    DataRow dr = fileTable.NewRow();
                    string realFileName = Convert.ToString(item["FileLeafRef"]);
                    string fileName2Display = ProjectUtilities.GetSSWPFileNameToDisplay(realFileName);
                    dr[fileNameField] = fileName2Display;
                    dr[fileIdField] = Convert.ToString(item["DocID"]);
                    dr[fileUrlField] = string.Format("{0}/{1}/{2}", web.Url, item.ParentList.RootFolder, realFileName);
                    //
                    string docicon = SPUtility.ConcatUrls("/_layouts/images",
                    SPUtility.MapToIcon(item.Web, SPUtility.ConcatUrls(item.Web.Url, item.Url), "", IconSize.Size16));
                    dr[fileIcon] = string.Format("<img src='{0}' />", docicon);
                    fileTable.Rows.Add(dr);
                }
            }
            //
            return fileTable;
        }
        public static void AddDocumentsToReport(UploadedFileCollection files, string reportId, SPWeb dWeb, ref string valMss)
        {
            if (files != null && files.Count > 0)
            {
                foreach (UploadedFile f in files)
                {
                    if (!string.IsNullOrWhiteSpace(f.FileName.Trim()))
                    {
                        Stream fStream = f.InputStream;
                        if (fStream.Length <= Settings.FileSizeLimit)
                        {
                            byte[] contents = new byte[fStream.Length];
                            fStream.Read(contents, 0, (int) fStream.Length);
                            fStream.Close();
                            // Remove all special Characters
                            string filename = ProjectUtilities.TrimFileName(f.FileName);
                            SPFile fileAdded = dWeb.Lists[Settings.SSWPAttachments].RootFolder.Files.Add(ProjectUtilities.MakeSSWPFileName(filename), contents, true);
                            fileAdded.Item.Properties["MasterID"] = reportId;
                            fileAdded.Item.Properties["Title"] = string.Format("File_{0}_{1}", reportId, Convert.ToString(fileAdded.Item.ID));
                            fileAdded.Item.SystemUpdate();
                        }
                        else
                        {
                            valMss = "File size exceeds 1 GB";
                        }
                    }
                    else
                    {
                        valMss = "Only word, excel, ppt and pdf formats are allowed";
                    }
                }
            }
        }
        public static void RemoveAttachmentFromMainItem(string [] docIDs,string mID,SPWeb dWeb)
        {
            SPListItemCollection items = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<Where><And><Eq><FieldRef Name = 'MasterID' /><Value Type = 'Text'>" + mID + "</Value></Eq>");
                sb.Append("<In>");
                sb.Append("<FieldRef Name = 'ID' />");
                sb.Append("<Values>");
                int i = 1;
                foreach (string s in docIDs)
                {
                    if (i >= Settings.QueriesInValuesMax - 1)
                        break;
                    if (!string.IsNullOrWhiteSpace(s))
                        sb.Append(string.Format("<Value Type = 'Number'>{0}</Value>", s.Trim()));
                    i++;
                }
                sb.Append("</Values>");
                sb.Append("</In>");
                sb.Append("</And></Where>");
                // Build a query.
                SPQuery query = new SPQuery();
                query.Query = sb.ToString();
                // Get data from a list.                 
                SPList oAttachmentList = dWeb.Lists[ProjectSettings.SPListAttachment];
                items = oAttachmentList.GetItems(query);
                if (items != null && items.Count > 0)
                {
                    int totalItems = items.Count;
                    for (int k=0; k< totalItems; k++)
                    {
                        items[0].Recycle();
                    }
                }
            }
            catch (Exception e)
            {
                //
            }
        }

        public static void RemoveContractAttachmentDocs(string[] docIDs, string mID, SPWeb dWeb)
        {
            SPListItemCollection items = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<Where><And><Eq><FieldRef Name = 'MasterID' /><Value Type = 'Text'>" + mID + "</Value></Eq>");
                sb.Append("<In>");
                sb.Append("<FieldRef Name = 'ID' />");
                sb.Append("<Values>");
                int i = 1;
                foreach (string s in docIDs)
                {
                    if (i >= Settings.QueriesInValuesMax - 1)
                        break;
                    if (!string.IsNullOrWhiteSpace(s))
                        sb.Append(string.Format("<Value Type = 'Number'>{0}</Value>", s.Trim()));
                    i++;
                }
                sb.Append("</Values>");
                sb.Append("</In>");
                sb.Append("</And></Where>");
                // Build a query.
                SPQuery query = new SPQuery();
                query.Query = sb.ToString();
                // Get data from a list.                 
                SPList oAttachmentList = dWeb.Lists[ProjectSettings.SPListAttachmentContract];
                items = oAttachmentList.GetItems(query);
                if (items != null && items.Count > 0)
                {
                    int totalItems = items.Count;
                    for (int k = 0; k < totalItems; k++)
                    {
                        items[0].Recycle();
                    }
                }
            }
            catch (Exception e)
            {
                //
            }
        }

        public static void RemoveTasks(List<string> deptsCodes, string sswpID, SPWeb dWeb)
        {
            SPListItemCollection items = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<Where><And><Eq><FieldRef Name = 'SSWPID' /><Value Type = 'Text'>" + sswpID + "</Value></Eq>");
                sb.Append("<In>");
                sb.Append("<FieldRef Name = 'ApproverTypeCode' />");
                sb.Append("<Values>");
                int i = 1;
                foreach (string s in deptsCodes)
                {
                    if (i >= Settings.QueriesInValuesMax - 1)
                        break;
                    if (!string.IsNullOrWhiteSpace(s))
                        sb.Append(string.Format("<Value Type = 'Text'>{0}</Value>", s.Trim()));
                    i++;
                }
                sb.Append("</Values>");
                sb.Append("</In>");
                sb.Append("</And></Where>");
                // Build a query.
                SPQuery query = new SPQuery();
                query.Query = sb.ToString();
                // Get data from a list.                 
                SPList sswpTasksList = dWeb.Lists[Settings.SSWPApprovalTasks];
                items = sswpTasksList.GetItems(query);
                if (items != null && items.Count > 0)
                {
                    int totalItems = items.Count;
                    for (int k = 0; k < totalItems; k++)
                    {
                        items.Delete(0);
                    }
                }
            }
            catch (Exception e)
            {
                //
            }
        }

        public static void UpdateTasksByApprovalValues(List<string> deptsCodes, string sswpID, SPWeb dWeb, string statusUpdated, string comment, UploadedFileCollection files=null)
        {
            SPListItemCollection items = null;
            bool isUpdatedOneComments = false;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<Where><And><Eq><FieldRef Name = 'SSWPID' /><Value Type = 'Text'>" + sswpID + "</Value></Eq>");
                sb.Append("<In>");
                sb.Append("<FieldRef Name = 'ApproverTypeCode' />");
                sb.Append("<Values>");
                int i = 1;
                foreach (string s in deptsCodes)
                {
                    if (i >= Settings.QueriesInValuesMax - 1)
                        break;
                    if (!string.IsNullOrWhiteSpace(s))
                        sb.Append(string.Format("<Value Type = 'Text'>{0}</Value>", s.Trim()));
                    i++;
                }
                sb.Append("</Values>");
                sb.Append("</In>");
                sb.Append("</And></Where>");
                // Build a query.
                SPQuery query = new SPQuery();
                query.Query = sb.ToString();
                // Get data from a list.                 
                SPList sswpTasksList = dWeb.Lists[Settings.SSWPApprovalTasks];
                items = sswpTasksList.GetItems(query);
                if (items != null && items.Count > 0)
                {
                    foreach (SPListItem t in items)
                    {
                        t["ModifiedDate"] = DateTime.Now;
                        t["Status"] = statusUpdated;
                        SPUser assignedTo = SPHelper.GetSPUserFromFieldInItem(dWeb,t, "AssignedTo");
                        if (assignedTo.LoginName.ToLower().Trim() != SPContext.Current.Web.CurrentUser.LoginName.ToLower().Trim())
                        {
                            t["ActedBy"] = SPContext.Current.Web.CurrentUser.Name;
                            t["ActedByLogin"] = SPContext.Current.Web.CurrentUser.LoginName;
                        }
                        t.Update();
                        if (!isUpdatedOneComments)
                        {
                            AddComments(dWeb, sswpID, assignedTo, DateTime.Now, comment, files);
                            isUpdatedOneComments = true;
                        }

                    }
                }
            }
            catch (Exception e)
            {
                //
            }
        }

        public static SPListItem getSSWPItemById(string sswpId, SPWeb oWeb)
        {
            string cQuery = "<Where><Eq><FieldRef Name='SSWPID'/><Value Type='Text'>" + sswpId.Trim() + "</Value></Eq></Where>";
            SPQuery query = new SPQuery();
            query.Query = cQuery;
            SPListItemCollection items = SPHelper.GetItems(Settings.SSWPMasterList, query, oWeb);
            return items[0];
        }

        public static SPListItem GetTaskMappingByCode(string approvalCode, SPWeb oWeb)
        {
            string cQuery = "<Where><Eq><FieldRef Name='code'/><Value Type='Text'>" + approvalCode.Trim() + "</Value></Eq></Where>";
            SPQuery query = new SPQuery();
            query.Query = cQuery;
            SPListItemCollection items = SPHelper.GetItems(ProjectSettings.SPListTaskMapping, query, oWeb);
            return items[0];
        }

        public static List<TaskMapping> GetAllTaskMapping(SPWeb oWeb)
        {
            SPListItemCollection items = oWeb.Lists[ProjectSettings.SPListTaskMapping].Items;
            List<TaskMapping> tObjects = new List<TaskMapping>();

            if (items.Count > 0)
            {
                foreach (SPListItem item in items)
                {
                    TaskMapping m = new TaskMapping();
                    m.Code = Convert.ToString(item["Code"]);
                    m.DisplayLabel= Convert.ToString(item["DisplayLabel"]);
                    m.Level = Convert.ToString(item["Level"]);
                    m.valType = Convert.ToString(item["valType"]);
                    SPUser u = ProjectHelper.GetSPUserValueByFieldName(item, "Val");
                    if (u != null)
                        m.Val = u.LoginName;
                    else
                        m.Val = string.Empty;
                    //
                    tObjects.Add(m);
                }
            }
            return tObjects;
        }
        public static TaskMapping GetTaskMappingByCode(List<TaskMapping> mappings, string taskCode)
        {
            foreach (TaskMapping t in mappings)
            {
                if (t.Code.ToLower().Trim() == taskCode.ToLower().Trim())
                    return t;
            }
            return null;
        }
        public static SPUser GetAssignedToByApprovalCode(string approvalCode, SPWeb oWeb)
        {
            SPListItem item = GetTaskMappingByCode(approvalCode, oWeb);
            try
            {
                return SPHelper.GetSPUserFromFieldInItem(oWeb, item, "Val");
            }
            catch
            { } return null;
        }

        public static void RecycleSSWPDraftRecordBySSWPID(string sswpId, SPWeb oWeb)
        {
            try
            {
                SPListItem sswpItem = getSSWPItemById(sswpId, oWeb);
                sswpItem.Recycle();
            }
            catch
            {
                //
            }
        }

        private static string GetRevisedNumber(ref string currentRevisedNo, string originalSSWPNo)
        {
            int nVer = 0; string sVer ="1";
            if (int.TryParse(currentRevisedNo.Trim(), out nVer))
            {
                sVer = Convert.ToString(nVer + 1);
            }
            currentRevisedNo = sVer;
            return string.Format("{0}.{1}", originalSSWPNo.Trim(), sVer);
        }
        //
        public static DataTable GetAllItemTableByListName(string listTitle,SPWeb web)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = SPHelper.GetSPListByName(web, listTitle).Items.GetDataTable();
            }
            catch (Exception ex)
            {
                //
            }
            return dt;
        }
        public static DataTable GetActiveItemTableByListName(string listTitle, SPWeb web)
        {
            DataTable dt = new DataTable();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<Where><And><Eq><FieldRef Name = 'Status' /><Value Type = 'Text'>Active</Value></Eq>");
                sb.Append("<Neq><FieldRef Name='Code'/><Value Type='Text'>-1</Value></Neq>");
                sb.Append("</And></Where>");
                SPQuery query = new SPQuery();
                query.Query = sb.ToString();
                // Get data from a list.                 
                SPList list = web.Lists[listTitle];
                dt = list.GetItems(query).GetDataTable();
            }
            catch (Exception ex)
            {
                //
            }
            return dt;
        }

        public static SPListItemCollection GetPendingTasksByAllUsers(SPWeb web)
        {
            SPListItemCollection items = null;
            try
            {

                StringBuilder sb = new StringBuilder();
                sb.Append("<Where><And><Eq><FieldRef Name = 'Status' /><Value Type = 'Text'>" + ProjectSettings.TaskStatusRouted + "</Value></Eq>");
                sb.Append("<Neq><FieldRef Name='SSWPID'/><Value Type='Text'>-1</Value></Neq>");
                sb.Append("</And></Where>");
                SPQuery query = new SPQuery();
                query.Query = sb.ToString();
                // Get data from a list.                 
                SPList oSSWPList = web.Lists[Settings.SSWPApprovalTasks];
                items = oSSWPList.GetItems(query);
            }
            catch (Exception e)
            {
                //
            }
            return items;
        }
       
        public static SPListItemCollection GetReportsByStatusList(SPWeb web, List<string> statusValues, List<string> listUserValueInputs=null,string userFieldToRequest= "Requester")
        {
            SPListItemCollection items = null;
            try
            {
                StringBuilder sb = new StringBuilder();

                //sb.Append("<Where><And><Eq><FieldRef Name = 'Status' /><Value Type = 'Text'>" + ProjectSettings.RRStatusDeleted + "</Value></Neq>");
                //-----------------------------------------------------------------
                sb.Append("<Where><And>");

                if (listUserValueInputs != null)
                {
                    sb.Append("<In>");
                    sb.Append("<FieldRef Name = '"+userFieldToRequest+"' />");
                    sb.Append("<Values>");
                    int k = 1;
                    foreach (string u in listUserValueInputs)
                    {
                        if (k >= Settings.QueriesInValuesMax - 1)
                            break;
                        if (!string.IsNullOrWhiteSpace(u))
                            sb.Append(string.Format("<Value Type = 'User'>{0}</Value>", u.Trim()));
                        k++;
                    }
                    sb.Append("</Values>");
                    sb.Append("</In>");
                }
                else
                {
                    sb.Append("<Neq><FieldRef Name = 'Status' /><Value Type = 'Text'>" + ProjectSettings.StatusDeleted + "</Value></Neq>");
                }
                //-----------------------------------------------------------------
                sb.Append("<In>");
                sb.Append("<FieldRef Name = 'Status' />");
                sb.Append("<Values>");
                int i = 1;
                foreach (string s in statusValues)
                {
                    if (i >= Settings.QueriesInValuesMax - 1)
                        break;
                    if (!string.IsNullOrWhiteSpace(s))
                        sb.Append(string.Format("<Value Type = 'Text'>{0}</Value>", s.Trim()));
                    i++;
                }
                sb.Append("</Values>");
                sb.Append("</In>");
                sb.Append("</And></Where>");
                SPQuery query = new SPQuery();
                query.Query = sb.ToString();
                // Get data from a list.                 
                SPList oSSWPList = web.Lists[ProjectSettings.SPListMaster];
                items = oSSWPList.GetItems(query);

            }
            catch (Exception e)
            {
                //
            }
            return items;
        }
        //
        public static SPListItemCollection GetReportsByStatusListAndOver5M(SPWeb web, List<string> statusValues)
        {
            SPListItemCollection items = null;
            try
            {
                StringBuilder sb = new StringBuilder();

                //sb.Append("<Where><And><Eq><FieldRef Name = 'Status' /><Value Type = 'Text'>" + ProjectSettings.RRStatusDeleted + "</Value></Neq>");
                //-----------------------------------------------------------------
                sb.Append("<Where><And>");
                sb.Append("<Eq><FieldRef Name = 'OCROver10M' /><Value Type = 'Integer'>1</Value></Eq>");
                sb.Append("<In>");
                sb.Append("<FieldRef Name = 'Status' />");
                sb.Append("<Values>");
                int i = 1;
                foreach (string s in statusValues)
                {
                    if (i >= Settings.QueriesInValuesMax - 1)
                        break;
                    if (!string.IsNullOrWhiteSpace(s))
                        sb.Append(string.Format("<Value Type = 'Text'>{0}</Value>", s.Trim()));
                    i++;
                }
                sb.Append("</Values>");
                sb.Append("</In>");
                sb.Append("</And></Where>");
                SPQuery query = new SPQuery();
                query.Query = sb.ToString();
                // Get data from a list.                 
                SPList oSSWPList = web.Lists[ProjectSettings.SPListMaster];
                items = oSSWPList.GetItems(query);

            }
            catch (Exception e)
            {
                //
            }
            return items;
        }
        //
        public static SPListItemCollection GetItemsForCurrentUserAndProxies(string userName, SPWeb web, string field2Search="Requester")
        {
            SPListItemCollection items = null;
            try
            {
                Dictionary<string, string> values = new Dictionary<string, string>();
                values = GetAllProxyOwnersByName(userName, web);
                values.Add(new Random().Next(1000, 100000000).ToString(), userName);
                StringBuilder sb = new StringBuilder();
                sb.Append("<Where><And><Neq><FieldRef Name = 'Status' /><Value Type = 'Text'>"+ProjectSettings.ProjectStatusDeleted+"</Value></Neq>");
                sb.Append("<In>");
                sb.Append("<FieldRef Name = '"+ field2Search + "' />");
                sb.Append("<Values>");
                int i = 1;
                foreach (string s in values.Values)
                {
                    if (i >= Settings.QueriesInValuesMax - 1)
                        break;
                    if (!string.IsNullOrWhiteSpace(s))
                        sb.Append(string.Format("<Value Type = 'User'>{0}</Value>", s.Trim()));
                    i++;

                }
                sb.Append("</Values>");
                sb.Append("</In>");
                sb.Append("</And></Where>");
                // Build a query.
                SPQuery query = new SPQuery();
                query.Query = sb.ToString();
                // Get data from a list.                 
                SPList mList = web.Lists[ProjectSettings.SPListMaster];
                items = mList.GetItems(query);
                if (items != null && items.Count > 0)
                    return items;
            }
            catch (Exception e)
            {
                //
            }
            return null;
        }

        public static SPListItemCollection GetRequestedByItemsForCurrentUserAndProxies(string userName, SPWeb web)
        {
            return GetItemsForCurrentUserAndProxies(userName, web, "Requester");
        }
        public static SPListItemCollection GetPMItemsForCurrentUserAndProxies(string userName, SPWeb web)
        {
            return GetItemsForCurrentUserAndProxies(userName, web, "OCRAnalyst");
        }
        // Get All LCU More than 5M
        public static SPListItemCollection GetAllLCUOver5MCons(SPWeb web, List<string> statusList)
        {
            return GetReportsByStatusListAndOver5M(web, statusList);
        }

        public static DataTable GetPMItemsNMyItems(string userName, SPWeb web)
        {
            DataTable dMy = new DataTable();
            SPListItemCollection myCol = GetRequestedByItemsForCurrentUserAndProxies(userName, web);
            List<string> midList = new List<string>();
            if(myCol !=null)
            {
                dMy = myCol.GetDataTable();
                dMy.Columns.Add(new DataColumn("isEditable", typeof(bool)));
                foreach(DataRow r in dMy.Rows)
                {
                    r["isEditable"] = true;
                    string mid = Convert.ToString(r["MasterID"]).Trim();
                    if(!string.IsNullOrEmpty(mid))
                        midList.Add(mid);
                }
            }

            // Get list to display for OCR Analyst
            DataTable dPM = new DataTable();
            SPListItemCollection pmCol = GetPMItemsForCurrentUserAndProxies(userName, web);
            if (pmCol != null)
            {
                dPM = pmCol.GetDataTable();
                dPM.Columns.Add(new DataColumn("isEditable", typeof(bool)));
                int count = dPM.Rows.Count;
                for(int i=0; i<count; i++)
                {
                    DataRow r = dPM.Rows[i];
                    string tmid = Convert.ToString(r["MasterID"]).Trim();
                    if (midList.Contains(tmid))
                    {
                        r.Delete();
                        i--; count--;
                    }
                    else
                        r["isEditable"] = true;
                }
            }
            // For OCR
            dMy.Merge(dPM);

            if (IfMemberOfLCU())
            {
                // Get All Pending LCU Over 5M Cons
                DataTable dOver5MLCU = new DataTable();
                List<string> LCUStatusList = new List<string> { ProjectSettings.ProjectStatusUnderReview, ProjectSettings.ProjectStatusApproved };
                SPListItemCollection dOver5MLCUCol = GetAllLCUOver5MCons(web, LCUStatusList);
                if (dOver5MLCUCol != null && dOver5MLCUCol.Count>0)
                {
                    dOver5MLCU = dOver5MLCUCol.GetDataTable();
                    dOver5MLCU.Columns.Add(new DataColumn("isEditable", typeof(bool)));
                    int count = dOver5MLCU.Rows.Count;
                    for (int i = 0; i < count; i++)
                    {
                        DataRow r = dOver5MLCU.Rows[i];
                        string tmid = Convert.ToString(r["MasterID"]).Trim();
                        if (midList.Contains(tmid))
                        {
                            r.Delete();
                            i--; count--;
                        }
                        else
                            r["isEditable"] = true;
                    }
                }
                //Add pending Over 5 M Cons for LCU
                dMy.Merge(dOver5MLCU);
            }
            
            return dMy;
        }

        public static List<MainObject> GetMainObjectCollectionsByRequesterNProxies(string userName, SPWeb web)
        {
            SPListItemCollection items = GetRequestedByItemsForCurrentUserAndProxies(userName,web);
            List<MainObject> list = new List<MainObject>();
            if (items != null)
            {
                foreach(SPListItem item in items)
                {
                    list.Add(new MainObject(item));
                }
            }
            return list;
        }
        public static List<MainObject> GetMainObjectCollectionsByPMNProxies(string userName, SPWeb web)
        {
            SPListItemCollection items = GetPMItemsForCurrentUserAndProxies(userName, web);
            List<MainObject> list = new List<MainObject>();
            if (items != null)
            {
                foreach (SPListItem item in items)
                {
                    list.Add(new MainObject(item));
                }
            }
            return list;
        }
        //
        public static SPListItemCollection GetReportsByStatusListnDepartment(SPWeb web, List<string> statusValues, List<string> departments = null)
        {
            SPListItemCollection items = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<Where><And>");

                if (departments != null)
                {
                    sb.Append("<In>");
                    sb.Append("<FieldRef Name = 'sDepartmentCode' />");
                    sb.Append("<Values>");
                    int k = 1;
                    foreach (string u in departments)
                    {
                        if (k >= Settings.QueriesInValuesMax - 1)
                            break;
                        if (!string.IsNullOrWhiteSpace(u))
                            sb.Append(string.Format("<Value Type = 'Text'>{0}</Value>", u.Trim()));
                        k++;
                    }
                    sb.Append("</Values>");
                    sb.Append("</In>");
                }
                else
                {
                    sb.Append("<Neq><FieldRef Name = 'Status' /><Value Type = 'Text'>" + ProjectSettings.StatusDeleted + "</Value></Neq>");
                }
                //-----------------------------------------------------------------
                sb.Append("<In>");
                sb.Append("<FieldRef Name = 'Status' />");
                sb.Append("<Values>");
                int i = 1;
                foreach (string s in statusValues)
                {
                    if (i >= Settings.QueriesInValuesMax - 1)
                        break;
                    if (!string.IsNullOrWhiteSpace(s))
                        sb.Append(string.Format("<Value Type = 'Text'>{0}</Value>", s.Trim()));
                    i++;
                }
                sb.Append("</Values>");
                sb.Append("</In>");
                sb.Append("</And></Where>");
                SPQuery query = new SPQuery();
                query.Query = sb.ToString();
                // Get data from a list.                 
                SPList masterList = web.Lists[ProjectSettings.SPListMaster];
                items = masterList.GetItems(query);
                if (items != null && items.Count > 0)
                    return items;
            }
            catch (Exception e)
            {
                //
            }
            return null;
        }

        public static DataTable GetReportsByStatusListnDepartmentTable(SPWeb web, List<string> statusValues, List<string> departments = null)
        {
            DataTable dt = new DataTable();
            try
            {
                SPListItemCollection items = GetReportsByStatusListnDepartment(web, statusValues, departments);
                if (items != null)
                {
                    dt = items.GetDataTable();
                    dt.Columns.Add(new DataColumn("isEditable", typeof(bool)));
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["isEditable"] = false;
                    }
                }
            }
            catch (Exception e)
            {
                //
            }
            return dt;
        }

        public static DataTable GetReportsToTable(SPWeb web, List<string> statusValues, List<string> listRequesters = null,bool approvalPage=false,List<string> departments=null, List<string> approvedBy=null)
        {
            DataTable dt = new DataTable();
            SPListItemCollection items = null;
            if (approvalPage)
                items = GetReportsByStatusListnDepartment(web, statusValues,departments);
            else if(approvedBy!=null)
                items = GetReportsByStatusList(web, statusValues, approvedBy, "ApprovedBy");
            else
                items = GetReportsByStatusList(web, statusValues, listRequesters);
            try
            {
                if (items != null && items.Count > 0)
                {
                    dt = items.GetDataTable();
                    dt.Columns.Add("ReportMonth"); dt.Columns.Add("ReportYear");
                    foreach (DataRow dr in dt.Rows)
                    {
                        DateTime rDate = DateTime.MinValue;
                        try
                        {
                            rDate = Convert.ToDateTime(dr["LastDayofReportMonth"]);
                            if (rDate != DateTime.MinValue)
                            {
                                //dr["ReportMonth"] = string.Format("{0}/{1}", rDate.Month.ToString(), rDate.Year.ToString());
                                dr["ReportYear"] = rDate.Year.ToString();
                                dr["ReportMonth"] = string.Format("{0}", ProjectUtilities.GetMonthStringByNumber(rDate.Month));
                            }
                        }
                        catch
                        {
                            //
                        }

                    }
                }
                else
                {
                    dt = ProjectUtilities.CreateDefaultPrjItemsTable();
                }
            }
            catch
            {
                dt = ProjectUtilities.CreateDefaultPrjItemsTable();
            }
            return dt;
        }

        public static SPListItemCollection GetNonDraftSSWP(SPWeb web)
        {
            SPListItemCollection items = null;
            try
            {
                // Build a query.
                SPQuery query = new SPQuery();
                query.Query = "<Where><Neq><FieldRef Name='SSWPStatus'/><Value Type='Text'>" +ProjectSettings.ProjectStatusDraft+ "</Value></Neq></Where>";
                // Get data from a list.                 
                SPList oSSWPList = web.Lists[Settings.SSWPMasterList];
                items = oSSWPList.GetItems(query);
            }
            catch (Exception e)
            {
                //
            }
            return items;
        }


        public static SPListItemCollection GetAllNoneReports(SPWeb web)
        {
            SPListItemCollection items = null;
            try
            {
                // Build a query.
                SPQuery query = new SPQuery();
                query.Query = "<Where><Neq><FieldRef Name='SSWPStatus'/><Value Type='Text'>" + ProjectSettings.ProjectStatusDraft + "</Value></Neq></Where>";
                SPList oSSWPList = web.Lists[ProjectSettings.SPListMaster];
                items = oSSWPList.GetItems(query);
            }
            catch (Exception e)
            {
                //
            }
            return items;
        }


        public static Dictionary<string, string> GetAllProxyOwnersByName(string userName, SPWeb web, bool filterValid=true)
        {
            SPListItemCollection items = null;
            Dictionary<string, string> proxyOwners = new Dictionary<string, string>();
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    // Build a query.
                    SPQuery query = new SPQuery();
                    query.Query = string.Concat(
                                   "<Where><And>" +
                                      "<Eq><FieldRef Name='Proxy'/><Value Type='User'>" + userName + "</Value></Eq>" +
                                      "<Eq><FieldRef Name='Status'/><Value Type='Text'>Active</Value></Eq>" +
                                   "</And></Where>" +
                                    "<OrderBy>" +
                                    "<FieldRef Name='PrimaryUser' Ascending='TRUE' />" +
                                    "</OrderBy>"
                                   );
                    //query.ViewFields = string.Concat("<FieldRef Name='PrimaryUser' />");
                    //query.ViewFieldsOnly = true;

                    // Get data from a list.                 
                    items = SPHelper.GetItems(Settings.Proxies, query, web);
                    if (items != null && items.Count > 0)
                    {
                        foreach (SPListItem item in items)
                        {
                            try
                            {
                                SPUser pUser = GetSPUserValueByFieldName(item, "PrimaryUser");
                                string loginName = pUser.LoginName;
                                string name = pUser.Name;
                                if (filterValid)
                                {
                                    DateTime start = Convert.ToDateTime(item["StartDate"]);
                                    DateTime end = Convert.ToDateTime(item["EndDate"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                                    if (start <= DateTime.Now && end >= DateTime.Now)
                                    {
                                        proxyOwners.Add(loginName.Trim(), loginName.Trim());
                                    }
                                }
                                else
                                {
                                    proxyOwners.Add(loginName.Trim(), loginName.Trim());
                                }
                            }
                            catch (Exception ex)
                            {
                                //
                            }
                        }
                    }

                });
            }
            catch (Exception e)
            {
                //
            }
            return proxyOwners;
        }

        public static Dictionary<string, string> GetAllProxiesDictionaryByOwnerNameBy(string logName, SPWeb web)
        {
            Dictionary<string, string> proxies = new Dictionary<string, string>();
            SPListItemCollection items = GetAllProxiesByOwnerName(logName, web);
            if (items != null && items.Count > 0)
            {
                foreach (SPListItem item in items)
                {
                    SPUser pUser = GetSPUserValueByFieldName(item, "Proxy");
                    string loginName = pUser.LoginName;
                    string name = pUser.Name;
                    try
                    {
                        proxies.Add(loginName.Trim(), loginName.Trim());
                    }
                    catch
                    {
                        // Prevent - Eleminate dublication
                    }
                }
            }
            return proxies;
        }

        public static Dictionary<string, string> GetAllManager2DictionaryByOCRAnalyst(string logName, SPWeb web)
        {
            Dictionary<string, string> manager2List = new Dictionary<string, string>();
            SPListItemCollection items = GetAllOCRManagerByOCRAnalystLoginname(logName, web);
            if (items != null && items.Count > 0)
            {
                foreach (SPListItem item in items)
                {
                    SPUser pUser = GetSPUserValueByFieldName(item, "Manager2");
                    string loginName = pUser.LoginName;
                    string name = pUser.Name;
                    try
                    {
                        manager2List.Add(loginName.Trim(), loginName.Trim());
                    }
                    catch
                    {
                        // Prevent - Eleminate dublication
                    }
                }
            }
            return manager2List;
        }

        public static SPListItemCollection GetAllOCRManagerByOCRAnalystLoginname(string logName, SPWeb web)
        {
            SPListItemCollection items = null;
            try
            {
                // Build a query.
                SPQuery query = new SPQuery();
                query.Query = string.Concat(
                               "<Where><And>" +
                                  "<Eq><FieldRef Name='OCRAnalyst'/><Value Type='User'>" + logName + "</Value></Eq>" +
                                  "<Eq><FieldRef Name='UserStatus'/><Value Type='Text'>Active</Value></Eq>" +
                               "</And></Where>" +
                                "<OrderBy>" +
                                "<FieldRef Name='OCRAnalyst' Ascending='TRUE' />" +
                                "</OrderBy>"
                               );
                query.ViewFields = string.Concat("<FieldRef Name='Manager' /><FieldRef Name='Manager2' /><FieldRef Name='UserStatus' /><FieldRef Name='OCRAnalyst' /><FieldRef Name='ID' />");
                query.ViewFieldsOnly = true;
                // Get data from a list.                 
                items = SPHelper.GetItems(Settings.spListOCRAnalyst, query, web);
                if (items.Count > 0)
                    return items;
            }
            catch (Exception e)
            {
                //
            }
            return items;
        }

        public static SPListItemCollection GetAllProxiesByOwnerName(string logName, SPWeb web)
        {
            SPListItemCollection items = null;
            try
            {
                // Build a query.
                SPQuery query = new SPQuery();
                query.Query = string.Concat(
                               "<Where><And>" +
                                  "<Eq><FieldRef Name='PrimaryUser'/><Value Type='User'>" + logName + "</Value></Eq>" +
                                  "<Eq><FieldRef Name='Status'/><Value Type='Text'>Active</Value></Eq>" +
                               "</And></Where>" +
                                "<OrderBy>" +
                                "<FieldRef Name='PrimaryUser' Ascending='TRUE' />" +
                                "</OrderBy>"
                               );
                query.ViewFields = string.Concat("<FieldRef Name='PrimaryUser' /><FieldRef Name='Proxy' /><FieldRef Name='StartDate' /><FieldRef Name='EndDate' /><FieldRef Name='ID' /><FieldRef Name='Status' />");
                query.ViewFieldsOnly = true;
                // Get data from a list.                 
                items = SPHelper.GetItems(Settings.Proxies, query, web);
                if (items.Count > 0)
                    return items;
            }
            catch (Exception e)
            {
                //
            }
            return items;
        }

        public static DataTable GetAllProxiesByOwnerNameAllFieldsToTable(string dispName, SPWeb web)
        {
            return GetProxiesTableFromItemCollection(GetAllProxiesByOwnerName(dispName, web));
        }


        public static DataTable CreateEmptyProxiesTable()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("ID", typeof(string)));
            dt.Columns.Add(new DataColumn("PrimaryUser", typeof(string)));
            dt.Columns.Add(new DataColumn("PrimaryUserLogin", typeof(string)));
            dt.Columns.Add(new DataColumn("Proxy", typeof(string)));
            dt.Columns.Add(new DataColumn("ProxyLogin", typeof(string)));
            dt.Columns.Add(new DataColumn("Status", typeof(string)));
            dt.Columns.Add(new DataColumn("StartDate", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("EndDate", typeof(DateTime)));
            return dt;
        }


        public static DataTable GetAllProxyTable(SPWeb web)
        {
            return GetProxiesTableFromItemCollection(web.Lists[Settings.Proxies].Items);
        }
        public static DataTable GetProxiesTableFromItemCollection(SPListItemCollection items)
        {
            DataTable dt = CreateEmptyProxiesTable();
            try
            {
                foreach (SPListItem item in items)
                {
                    string id = Convert.ToString(item["ID"]);
                    SPUser priUser = SPHelper.GetSPUserFromFieldInItem(item.Web, item, "PrimaryUser");
                    string primaryUser = priUser.Name;
                    string primaryUserLogin = priUser.LoginName;

                    SPUser proxyUser = SPHelper.GetSPUserFromFieldInItem(item.Web, item, "Proxy");
                    string proxy = proxyUser.Name;
                    string proxyLogin = proxyUser.LoginName;
                    string status = Convert.ToString(item["Status"]);
                    DateTime? sDate = Convert.ToDateTime(item["StartDate"]);
                    DateTime? eDate = Convert.ToDateTime(item["EndDate"]);
                    dt.Rows.Add(new object[] { id, primaryUser, primaryUserLogin, proxy, proxyLogin, status, sDate, eDate });
                }

            }
            catch (Exception e)
            {
                //
            }
            return dt;
        }


        public static bool checkIfSSWPNoPlusContractNo(string oldSSWPNo, string oldConNo,string sswpNo, string conNo, SPWeb web)
        {
            string comOld = oldSSWPNo + oldConNo;
            string comNew = sswpNo + conNo;
            if (comOld == comNew)
                return false;
            SPListItemCollection items = null;
            try
            {
                // Build a query.
                SPQuery query = new SPQuery();
                query.Query = string.Concat(
                               "<Where><And>" +
                                  "<Eq><FieldRef Name='SSWPNo'/><Value Type='Text'>" + sswpNo + "</Value></Eq>" +
                                  "<Eq><FieldRef Name='ContractNo'/><Value Type='Text'>"+ conNo + "</Value></Eq>" +
                               "</And></Where>" +
                                "<OrderBy>" +
                                "<FieldRef Name='SSWPNo' Ascending='TRUE' />" +
                                "</OrderBy>"
                               );
                query.ViewFields = string.Concat("<FieldRef Name='SSWPNo' />");
                query.ViewFieldsOnly = true;
                // Get data from a list.                 
                items = SPHelper.GetItems(Settings.SSWPMasterList, query, web);
                return (items.Count > 0) ? true : false;
            }
            catch (Exception e)
            {
                //
            }
            return false;
        }



        public static void AddComments(SPWeb dWeb,string mID, SPUser assignedToLogin, DateTime modifiedDate, string comments, UploadedFileCollection files = null)
        {
            // this mark for the only comment function
            if (files == null && string.IsNullOrEmpty(comments))
                return;
            else
            {
                SPList sswpList = dWeb.Lists[ProjectSettings.SPListComment];
                // 1 - Create item
                SPListItem oListItem = sswpList.Items.Add();
                //
                oListItem["AssignedTo"] = assignedToLogin;
                oListItem["MasterID"] = mID;
                oListItem["ModifiedDate"] = modifiedDate;
                oListItem["AppVersion"] = Settings.CurrentVersion;
                if (!string.IsNullOrEmpty(comments.Trim()))
                    oListItem["aComment"] = comments;
                if (files != null)
                {
                    foreach (UploadedFile f in files)
                    {
                        if (!string.IsNullOrWhiteSpace(f.FileName.Trim()))
                        {
                            Stream fStream = f.InputStream;
                            if (fStream.Length <= Settings.FileSizeLimit)
                            {
                                byte[] contents = new byte[fStream.Length];
                                fStream.Read(contents, 0, (int) fStream.Length);
                                fStream.Close();
                                string filename = ProjectUtilities.TrimFileName(f.FileName);
                                oListItem.Attachments.Add(filename, contents);
                            }
                        }
                    }
                }
                oListItem.Update();
            }
           
        }

        public static SPListItemCollection GetAllCommentItems(SPWeb dWeb, string mid)
        {
            SPListItemCollection items = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<OrderBy><FieldRef Name='ModifiedDate' Ascending='TRUE'/></OrderBy>");
                sb.Append("<Where><And><Neq><FieldRef Name = 'MasterID' /><Value Type = 'Text'>-1</Value></Neq>");
                sb.Append("<Eq><FieldRef Name='MasterID'/><Value Type='Text'>" + mid + "</Value></Eq>");
                sb.Append("</And></Where>");
                SPQuery query = new SPQuery();
                query.Query = sb.ToString();
                // Get data from a list.                 
                SPList oSSWPListComments = dWeb.Lists[Settings.SSWPComments];
                items = oSSWPListComments.GetItems(query);
            }
            catch
            {
                //
            }
            return items;
        }
        public static void MoveAllSSWPCommentsToNewSSWP(SPWeb dWeb, string sswpId, string newSSWPID)
        {
            SPList oSSWPListComments = dWeb.Lists[Settings.SSWPComments];
            SPListItemCollection items = GetAllCommentItems(dWeb, sswpId);
            if (items != null && items.Count > 0)
            {
                foreach (SPListItem item in items)
                {
                    string iVersion = string.Empty;
                    try
                    {
                        SPListItem targetItem = oSSWPListComments.Items.Add();
                        string aCom = Convert.ToString(item["aComment"]);
                        SPUser assignedTo = SPHelper.GetSPUserFromFieldInItem(dWeb, item, "AssignedTo");
                        // ----- fix error
                        SPUser authorOfComment= SPHelper.GetSPUserFromFieldInItem(dWeb, item, "Author");
                        try
                        {
                            iVersion = Convert.ToString(item["AppVersion"]);
                        }
                        catch { }
                        //------
                        DateTime modified = Convert.ToDateTime(item["ModifiedDate"]);
                        
                        targetItem["aComment"] = aCom;
                        targetItem["ModifiedDate"] = modified;
                        targetItem["AssignedTo"] = assignedTo;
                        targetItem["MasterID"] = newSSWPID;
                        //
                        targetItem["AppVersion"] = string.IsNullOrEmpty(iVersion)?Settings.CurrentVersion: iVersion;
                        //

                        //
                        // --- fix display error
                        targetItem["Author"] = authorOfComment;
                        //---------
                        
                        //copy attachments
                        foreach (string fileName in item.Attachments)
                        {
                            SPFile file = item.ParentList.ParentWeb.GetFile(item.Attachments.UrlPrefix + fileName);
                            byte[] imageData = file.OpenBinary();
                            targetItem.Attachments.Add(fileName, imageData);
                        }
                        targetItem.UpdateOverwriteVersion();
                    }
                    catch
                    {
                        //
                    }
                }
            }

        }


        public static void MoveAllHistoryToNewReport(SPWeb dWeb, string reportID, string newReportId)
        {
            SPListItemCollection items = getAllReportHistoryItems(dWeb, reportID);
            SPList oSSWPList = dWeb.Lists[Settings.RRHistory];
            foreach (SPListItem item in items)
            {
                try
                {
                    SPListItem targetItem = oSSWPList.Items.Add();
                    string title = Convert.ToString(item["Title"]);
                    string historyType = Convert.ToString(item["HistoryType"]);
                    targetItem["Title"] = title;
                    targetItem["HistoryType"] = historyType;
                    targetItem["Details"] = Convert.ToString(item["Details"]);
                    targetItem["RRReportId"] = newReportId;
                    targetItem.SystemUpdate();
                }
                catch (Exception ex)
                {
                    //
                }
            }
           
        }

        public static bool CheckIfSSWPApprovedBySSWPID(string sswpId,SPWeb dWeb)
        {
            SPListItemCollection items = null;
            StringBuilder sb = new StringBuilder();
            bool hasRouted = false;
            bool hasApproved = false;
            bool hasRejected = false;
            sb.Append("<Where><And><Neq><FieldRef Name='Status'/><Value Type='Text'>"+ ProjectSettings.TaskStatusNone + "</Value></Neq>");
            sb.Append("<Eq><FieldRef Name='MasterID'/><Value Type='Text'>" + sswpId+"</Value></Eq>");
            sb.Append("</And></Where>");
            SPQuery query = new SPQuery();
            query.Query = sb.ToString();
            // Get data from a list.                 
            SPList oSSWPList = dWeb.Lists[Settings.SSWPApprovalTasks];
            items = oSSWPList.GetItems(query);
            if (items != null && items.Count > 0)
            {
                foreach (SPListItem i in items)
                {
                    string iStatus = Convert.ToString(i["Status"]);
                    if (iStatus.Trim().Equals(ProjectSettings.TaskStatusRouted))
                        hasRouted = true;
                    if (iStatus.Trim().Equals(ProjectSettings.TaskStatusConcur))
                        hasApproved = true;
                    //if (iStatus.Trim().Equals(Settings.TaskStatusNonConcur))
                    //    hasRejected = true;
                }
            }
            if (!hasRouted && hasApproved && !hasRejected)
                return true;
            else
                return false;
        }


        // Get RR Department Approver by Department code
        public static SPListItem getRRDepartmentItembyDepartmentCode(SPWeb dWeb, string deptCode)
        {
            SPListItem item = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<Where><And><Neq><FieldRef Name = 'Status' /><Value Type = 'Text'>"+ ProjectSettings.StatusDeleted+ "</Value></Neq>");
                sb.Append("<Eq><FieldRef Name='Code'/><Value Type='Text'>" + deptCode + "</Value></Eq>");
                sb.Append("</And></Where>");
                SPQuery query = new SPQuery();
                query.Query = sb.ToString();
                // Get data from a list.                 
                SPList list = dWeb.Lists[ProjectSettings.ProjectDepartments];
                item =list.GetItems(query)[0];
            }
            catch
            {
                //
            }
            return item;
        }


        public static SPGroup GetApproverGroupByDepartmentCode(SPWeb web, string deptCode, string fieldName)
        {
            SPListItem item = getRRDepartmentItembyDepartmentCode(web, deptCode);
            SPGroup g = null;

            if (item != null)
            {
                if (item[fieldName] != null)
                {
                    SPFieldUserValue userValue = new SPFieldUserValue(item.ParentList.ParentWeb, Convert.ToString(item[fieldName]));
                    if (userValue.User == null)
                    {
                        g = web.Groups[userValue.LookupValue];
                    }
                }
            }
            //
            return g;
        }
        public static SPGroup GetSPGroupByItemFieldName(SPListItem item, string fieldName)
        {
            SPGroup g = null;
            if (item != null)
            {
                if (item[fieldName] != null)
                {
                    SPFieldUserValue userValue = new SPFieldUserValue(item.ParentList.ParentWeb, Convert.ToString(item[fieldName]));
                    if (userValue.User == null)
                    {
                        g = item.Web.Groups[userValue.LookupValue];
                    }
                }
            }
            //
            return g;
        }

        public static SPListItemCollection GetTaskItemsByReportID(SPWeb dWeb, string reportId)
        {
            SPListItemCollection items = null;
            try
            {
                SPList list = dWeb.Lists[Common.ProjectSettings.SPListTasks];
                StringBuilder sb = new StringBuilder();
                sb.Append("<Where><And><Eq><FieldRef Name = 'MasterID' /><Value Type = 'Text'>" + reportId + "</Value></Eq>");
                sb.Append("<Neq><FieldRef Name='Status'/><Value Type='Text'>Inactive</Value></Neq>");
                sb.Append("</And></Where>");
                SPQuery query = new SPQuery();
                query.Query = sb.ToString();
                // Get data from a list.
                items = list.GetItems(query);
                if (items.Count < 1)
                    items = null;
            }
            catch (Exception ex)
            {
                //Common.ProjectUtilities.LogError(ex.ToString());
            }
            // --------
            return items;

        }
        public static SPListItem GetTaskByTaskID(SPWeb dWeb, string taskId)
        {
            SPListItem item = null;
            try
            {
                SPList list = dWeb.Lists[Common.ProjectSettings.SPListTasks];
                StringBuilder sb = new StringBuilder();
                sb.Append("<Where><And><Eq><FieldRef Name = 'TaskId' /><Value Type = 'Text'>" + taskId + "</Value></Eq>");
                sb.Append("<Neq><FieldRef Name='Status'/><Value Type='Text'>Inactive</Value></Neq>");
                sb.Append("</And></Where>");
                SPQuery query = new SPQuery();
                query.Query = sb.ToString();
                // Get data from a list.
                SPListItemCollection items = list.GetItems(query);
                if (items.Count < 1)
                    items = null;
                else
                    item = items[0];
            }
            catch (Exception ex)
            {
                //Common.ProjectUtilities.LogError(ex.ToString());
            }
            // --------
            return item;

        }


        public static SPListItemCollection getAllReportHistoryItems(SPWeb dWeb, string reportId)
        {
            SPListItemCollection items = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<OrderBy><FieldRef Name='Created' Ascending='TRUE'/></OrderBy>");
                sb.Append("<Where><And><Neq><FieldRef Name = 'RRReportId' /><Value Type = 'Text'>-1</Value></Neq>");
                sb.Append("<Eq><FieldRef Name='RRReportId'/><Value Type='Text'>" + reportId + "</Value></Eq>");
                sb.Append("</And></Where>");
                SPQuery query = new SPQuery();
                query.Query = sb.ToString();
                // Get data from a list.                 
                SPList oSSWPList = dWeb.Lists[ProjectSettings.SPListHistory];
                items = oSSWPList.GetItems(query);
            }
            catch
            {
                //
            }
            return items;
        }

        public static SPListItemCollection getAllFiles(SPWeb dWeb,string reportId)
        {
            SPListItemCollection items = null;
            try
            {
                
                SPList list = dWeb.Lists[Common.ProjectSettings.SPListAttachment];
                StringBuilder sb = new StringBuilder();
                sb.Append("<Where><And><Eq><FieldRef Name = 'RRReportId' /><Value Type = 'Text'>" + reportId + "</Value></Eq>");
                sb.Append("<Neq><FieldRef Name='Status'/><Value Type='Text'>" + ProjectSettings.StatusDeleted + "</Value></Neq>");
                sb.Append("</And></Where>");
                SPQuery query = new SPQuery();
                query.Query = sb.ToString();
                // Get data from a list.
                items = list.GetItems(query);

                if (items != null && items.Count > 0)
                    return items;
            }
            catch
            {
                //
            }
            return null;
        }
        public static SPListItemCollection GetAllNonDraftReportByProjectNo(SPWeb web, string projectNo)
        {
            SPListItemCollection items = null;
            try
            {
                // Build a query.
                SPQuery query = new SPQuery();
                query.Query = string.Concat(
                               "<Where><And>" +
                                  "<Eq><FieldRef Name='ProjectNo'/><Value Type='Text'>" + projectNo + "</Value></Eq>" +
                                  "<Neq><FieldRef Name='Status'/><Value Type='Text'>" + ProjectSettings.ProjectStatusDraft + "</Value></Neq>" +
                               "</And></Where>");
                // Get data from a list.                 
                items = SPHelper.GetItems(Settings.SSWPMasterList, query, web);
                if (items.Count > 0)
                return items;
            }
            catch (Exception e)
            {
                //
            }
            return items;
        }
        public static void AddHistory(SPWeb dWeb, MainObject obj, string hisType, string action, string userDisplayLogin, string timeAt, string proxyforLogin)
        {
            string returnHis = string.Empty;
            try
            {
                SPList sswpHistory = dWeb.Lists[ProjectSettings.SPListHistory];
                SPListItem oListItem = sswpHistory.Items.Add();
                oListItem["Title"] = obj.ProgramName;
                oListItem["MasterID"] = obj.MasterID;
                oListItem["HistoryType"] = hisType;
                oListItem["UserCreated"] = dWeb.CurrentUser;
                string presentName = SPHelper.GetSPUserFromLoginName(dWeb, userDisplayLogin).Name;
                if (string.IsNullOrEmpty(proxyforLogin) || (userDisplayLogin.ToLower().Trim() == proxyforLogin.ToLower().Trim()))
                {
                    returnHis = string.Format("{0}: {1} by {2}", timeAt, action, presentName);
                }
                else
                {
                    string proxyForDisplay = SPHelper.GetSPUserFromLoginName(dWeb, proxyforLogin).Name;
                    returnHis = string.Format("{0}: {1} by {2} (acted on behalf of {3})", timeAt, action, presentName, proxyForDisplay);
                }
                oListItem["Details"] = returnHis;
                oListItem.Update();
            }
            catch (Exception ex)
            {
                //
            }
        }
        public static bool IfReportExistForProjectByDate(SPWeb dWeb,string projectNo, DateTime dateReport)
        {
            SPListItemCollection items = null;
            try
            {
                // Build a query.
                SPQuery query = new SPQuery();
                query.Query = string.Concat(
                               "<Where><And>" +
                                  "<Eq><FieldRef Name='MDYProjID'/><Value Type='Text'>" + string.Format("{0}-{1}-{2}_{3}",dateReport.Month.ToString(),dateReport.Day.ToString(),dateReport.Year.ToString(),projectNo) + "</Value></Eq>" +
                                  "<Neq><FieldRef Name='Status'/><Value Type='Text'>" + ProjectSettings.ProjectStatusDraft + "</Value></Neq>" +
                               "</And></Where>");
                // Get data from a list.                 
                items = SPHelper.GetItems(ProjectSettings.SPListMaster, query, dWeb);
                if (items.Count > 0)
                return true;
            }
            catch (Exception e)
            {
                //
            }
            return false;
        }
        public static bool IfReportExistForProjectByDateEleminateCurrent(SPWeb dWeb, string projectNo, DateTime dateReport, string rrReportId)
        {
            SPListItemCollection items = null;
            try
            {
                // Build a query.
                SPQuery query = new SPQuery();
                query.Query = string.Concat(
                               "<Where><And>" +
                                  "<Eq><FieldRef Name='MDYProjID'/><Value Type='Text'>" + string.Format("{0}-{1}-{2}_{3}", dateReport.Month.ToString(), dateReport.Day.ToString(), dateReport.Year.ToString(), projectNo) + "</Value></Eq>" +
                                  "<Neq><FieldRef Name='Status'/><Value Type='Text'>" + ProjectSettings.ProjectStatusDraft + "</Value></Neq>" +
                               "</And></Where>");
                // Get data from a list.                 
                items = SPHelper.GetItems(ProjectSettings.SPListMaster, query, dWeb);
                if (items.Count > 0)
                {
                    foreach (SPListItem item in items)
                    {
                        string rID = Convert.ToString(item["RRReportId"]);
                        if (rID.Trim() != rrReportId.Trim())
                            return true;
                    }
                }
            }
            catch (Exception e)
            {
                //
            }
            return false;
        }

        public static SPListItemCollection GetAllMasterItemTasksAssigned(SPWeb dWeb, string mID)
        {
            SPListItemCollection items = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<OrderBy><FieldRef Name=\"ApprovalOrder\" Ascending=\"TRUE\"/></OrderBy><Where><And><Neq><FieldRef Name = 'MasterID' /><Value Type = 'Text'>" + ProjectSettings.StatusDeleted + "</Value></Neq>");
                sb.Append("<Eq><FieldRef Name='MasterID'/><Value Type='Text'>" + mID + "</Value></Eq>");
                sb.Append("</And></Where>");
                SPQuery query = new SPQuery();
                query.Query = sb.ToString();
                // Get data from a list.                 
                SPList list = dWeb.Lists[ProjectSettings.SPListTasks];
                items = list.GetItems(query);
                if (items.Count > 0)
                    return items;
            }
            catch
            {
                //
            }
            return null;
        }
        public static SPListItemCollection getAllHistoryItems(SPWeb dWeb, string mdi)
        {
            SPListItemCollection items = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<OrderBy><FieldRef Name='Created' Ascending='TRUE'/></OrderBy>");
                sb.Append("<Where><And><Neq><FieldRef Name = 'MasterID' /><Value Type = 'Text'>-1</Value></Neq>");
                sb.Append("<Eq><FieldRef Name='MasterID'/><Value Type='Text'>" + mdi + "</Value></Eq>");
                sb.Append("</And></Where>");
                SPQuery query = new SPQuery();
                query.Query = sb.ToString();
                // Get data from a list.                 
                SPList oSSWPList = dWeb.Lists[ProjectSettings.SPListHistory];
                items = oSSWPList.GetItems(query);
            }
            catch
            {
                //
            }
            return items;
        }
        public static SPListItemCollection getAllPreviousVersion(SPWeb dWeb, string mdi)
        {
            SPListItemCollection items = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<OrderBy><FieldRef Name='Created' Ascending='FALSE'/></OrderBy>");
                sb.Append("<Where><And><Neq><FieldRef Name = 'MasterID' /><Value Type = 'Text'>-1</Value></Neq>");
                sb.Append("<Eq><FieldRef Name='MasterID'/><Value Type='Text'>" + mdi + "</Value></Eq>");
                sb.Append("</And></Where>");
                SPQuery query = new SPQuery();
                query.Query = sb.ToString();
                query.ViewAttributes = "Scope=\"Recursive\"";
                // Get data from a list.                 
                SPList oSSWPList = dWeb.Lists[ProjectSettings.SPListMasterVersions];
                items = oSSWPList.GetItems(query);
            }
            catch
            {
                //
            }
            return items;
        }
        public static List<string> getApproverLoginsByMainObjectID(SPWeb dWeb, string mID)
        {
            List<ApprovalTaskObject> listTasks = GetApproversListByMainObjectID(dWeb, mID);
            List<string> approverLogins = new List<string>();
            foreach (ApprovalTaskObject ta in listTasks)
            {
                if (ta.TaskStatus != ProjectSettings.TaskStatusNone && !string.IsNullOrEmpty(ta.AssignedToLogin.Trim()) && !approverLogins.Contains(ta.AssignedToLogin.ToLower().Trim()))
                {
                    approverLogins.Add(ta.AssignedToLogin.ToLower().Trim());
                }

            }

            return approverLogins;
        }
        public static List<string> getApproverEmailsByMainObjectID(SPWeb dWeb, string mID)
        {
            List<string> logins = getApproverLoginsByMainObjectID(dWeb, mID);
            List<string> emails = new List<string>();
            foreach (string login in logins)
            {
                emails.Add(ProjectUtilities.GetEmailByUser(login));
            }
            return emails;
        }
        public static List<ApprovalTaskObject> GetApproversListByMainObjectID(SPWeb dWeb, string mID, string taskStatus = "")
        {
            SPListItemCollection items = null;
            List<ApprovalTaskObject> listTasks = new List<ApprovalTaskObject>();
            try
            {
                StringBuilder sb = new StringBuilder();
                if (string.IsNullOrEmpty(taskStatus))
                    sb.Append("<Where><And><Neq><FieldRef Name='Status' /><Value Type ='Text'>Draft</Value></Neq>");
                else
                    sb.Append("<Where><And><Eq><FieldRef Name='Status'/><Value Type='Text'>" + taskStatus + "</Value></Eq>");
                sb.Append("<Eq><FieldRef Name='MasterID'/><Value Type='Text'>" + mID + "</Value></Eq>");
                sb.Append("</And></Where>");
                SPQuery query = new SPQuery();
                query.Query = sb.ToString();
                // Get data from a list.                 
                SPList oSSWPList = dWeb.Lists[ProjectSettings.SPListTasks];
                items = oSSWPList.GetItems(query);
                if (items != null && items.Count > 0)
                {
                    foreach (SPListItem oListItem in items)
                    {
                        ApprovalTaskObject aObj = new ApprovalTaskObject();
                        aObj.TaskTitle = Convert.ToString(oListItem["Title"]);
                        //aObj.DueDate = obj.DueDate;
                        aObj.MasterID = mID;
                        SPUser AssignedToUser = SPHelper.GetSPUserFromFieldInItem(dWeb, oListItem, "AssignedTo");
                        aObj.AssignedToLogin = AssignedToUser.LoginName.Trim();
                        aObj.AssignedToName = AssignedToUser.Name.Trim();
                        aObj.AssignedToEmailAddress = AssignedToUser.Email.Trim();
                        aObj.TaskStatus = Convert.ToString(oListItem["TaskStatus"]);
                        aObj.ApproverTypeCode = Convert.ToString(oListItem["ApprovalTypeCode"]);
                        //aObj.RequestorLogin = Convert.ToString(oListItem["Requestor"]);
                        aObj.ApprovedDate = Convert.ToDateTime(oListItem["ApprovedDate"]);
                        aObj.ApprovalOrder = Convert.ToInt32(oListItem["ApprovalOrder"]);
                        aObj.Comment = Convert.ToString(oListItem["Note"]);
                        aObj.ApprovedBy = Convert.ToString(oListItem["ApprovedByName"]);
                        aObj.ApprovedByLogin = Convert.ToString(oListItem["ApprovedByLogin"]);
                        if (oListItem.Attachments != null)
                            aObj.attachments = GetAttachmentByItem(oListItem);
                        //
                        listTasks.Add(aObj);
                    }
                }
            }
            catch (Exception e)
            {
                //
            }
            return listTasks;
        }
        public static List<CommentAttachmentObject> GetAttachmentByItem(SPListItem item)
        {
            List<CommentAttachmentObject> attachmentList = new List<CommentAttachmentObject>();
            if (item.Attachments != null && item.Attachments.Count > 0)
            {
                for (int i = 0; i < item.Attachments.Count; i++)
                {
                    CommentAttachmentObject at = new CommentAttachmentObject();
                    at.DocName = Convert.ToString(item.Attachments[i]);
                    at.DOcUrl = SPUrlUtility.CombineUrl(item.Attachments.UrlPrefix, at.DocName);
                    attachmentList.Add(at);
                }
            }
            return attachmentList;
        }


        // Get all task that with status # none - 
        public static SPListItemCollection GetTakenTasksByMeNProxiedFors(string accName, SPWeb web)
        {
            SPListItemCollection items = null;
            try
            {
                Dictionary<string, string> values = new Dictionary<string, string>();
                values = GetAllProxyOwnersByName(accName, web);
                values.Add(new Random().Next(1000, 100000000).ToString(), accName);
                if (values.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<Where><And><Neq><FieldRef Name = 'TaskStatus' /><Value Type = 'Text'>" + ProjectSettings.TaskStatusRouted + "</Value></Neq>");
                    sb.Append("<In>");
                    sb.Append("<FieldRef Name = 'AssignedTo' />");
                    sb.Append("<Values>");
                    int i = 1;
                    foreach (string s in values.Values)
                    {
                        if (i >= Settings.QueriesInValuesMax - 1)
                            break;
                        if (!string.IsNullOrWhiteSpace(s))
                            sb.Append(string.Format("<Value Type = 'User'>{0}</Value>", s.Trim()));
                        i++;
                    }
                    sb.Append("</Values>");
                    sb.Append("</In>");
                    sb.Append("</And></Where>");
                    // Build a query.
                    SPQuery query = new SPQuery();
                    query.Query = sb.ToString();
                    // Get data from a list.                 
                    SPList oList = web.Lists[ProjectSettings.SPListTasks];
                    items = oList.GetItems(query);
                    if (items.Count > 0)
                        return items;
                }
            }
            catch (Exception e)
            {
                //
            }
            return items;
        }
        public static SPListItemCollection GetPendingTasksCurrentUser(string login, SPWeb web)
        {
            SPListItemCollection items = null;
            try
            {
                Dictionary<string, string> values = new Dictionary<string, string>();
                values.Add(new Random().Next(1000, 100000000).ToString(), login);
                values = GetAllProxyOwnersByName(login, web);
                StringBuilder sb = new StringBuilder();
                sb.Append("<Where><And><Eq><FieldRef Name = 'TaskStatus' /><Value Type = 'Text'>" + ProjectSettings.TaskStatusRouted + "</Value></Eq>");
                sb.Append("<In>");
                sb.Append("<FieldRef Name = 'AssignedTo' />");
                sb.Append("<Values>");
                int i = 1;
                foreach (string s in values.Values)
                {
                    if (i >= Settings.QueriesInValuesMax - 1)
                        break;
                    if (!string.IsNullOrWhiteSpace(s))
                        sb.Append(string.Format("<Value Type = 'User'>{0}</Value>", s.Trim()));
                    i++;
                }
                sb.Append("</Values>");
                sb.Append("</In>");
                sb.Append("</And></Where>");
                // Build a query.
                SPQuery query = new SPQuery();
                query.Query = sb.ToString();
                // Get data from a list.                 
                SPList oList = web.Lists[ProjectSettings.SPListTasks];
                items = oList.GetItems(query);
            }
            catch (Exception e)
            {
                //
            }
            return items;
        }
        //----------------------------------------------------------------------
        public static List<string> GetDepartmentGroup(SPWeb dWeb)
        {
            DataTable dtDepts = Common.ProjectHelper.GetAllItemTableByListName(ProjectSettings.ProjectDepartments, dWeb);
            List<string> deptGroupList = new List<string>();
            foreach(DataRow dr in dtDepts.Rows)
            {
                string gCode = Convert.ToString(dr["DeptGroup"]);
                if(!string.IsNullOrEmpty(gCode.Trim()))
                {
                    deptGroupList.Add(gCode.Trim());
                }
            }
            return deptGroupList;
        }

        public static List<string> GetDepartmentGroup()
        {
            List<string> deptGroupList = new List<string>();
            using (SPSite dSite = new SPSite(ProjectHelper.DataSiteURL))
            {
                using (SPWeb dWeb = dSite.OpenWeb(ProjectHelper.DataWebRelativeURL))
                {
                    DataTable dtDepts = Common.ProjectHelper.GetAllItemTableByListName(ProjectSettings.ProjectDepartments, dWeb);
                    foreach (DataRow dr in dtDepts.Rows)
                    {
                        string gCode = Convert.ToString(dr["DeptGroup"]);
                        if (!string.IsNullOrEmpty(gCode.Trim()))
                        {
                            deptGroupList.Add(gCode.Trim());
                        }
                    }
                }
            }
            return deptGroupList;
        }

        public static bool IfViewAllShow()
        {
            if (SPHelper.IsMemberOfGroup(Settings.InternalGroupName) || SPHelper.IsMemberOfGroup(Settings.AdminGroupName))
                return true;
            List<string> DeptGroups = GetDepartmentGroup();
            foreach(string g in DeptGroups)
            {
                if (SPHelper.IsMemberOfGroup(g))
                    return true;
            }
            return false;
        }

        public static bool IfMemberOfLCU()
        {
            if (SPHelper.IsMemberOfGroup(Settings.GroupOCRCompliance))
                return true;

            return false;
        }

        public static bool IfViewAllList(ref List<string> accessList)
        {
            if(SPHelper.IsMemberOfGroup(Settings.AdminGroupName))
            {
                accessList.Add(Settings.AdminGroupName);
            }
            if (SPHelper.IsMemberOfGroup(Settings.InternalGroupName))
            {
                accessList.Add(Settings.InternalGroupName);
            }
            List<string> DeptGroups = GetDepartmentGroup();
            foreach (string g in DeptGroups)
            {
                if (SPHelper.IsMemberOfGroup(g))
                    accessList.Add(g);
            }
            if (accessList.Count > 0)
                return true;

            return false;
        }
        //----------------------------------------------------------------------
        public static SPListItemCollection GetPendingTasksByMeNProxiedFors(string login, SPWeb web)
        {
            SPListItemCollection items = null;
            try
            {
                Dictionary<string, string> values = new Dictionary<string, string>();
                values = GetAllProxyOwnersByName(login, web);
                values.Add(new Random().Next(1000, 100000000).ToString(), login);
                if (values.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<Where><And><Eq><FieldRef Name = 'TaskStatus' /><Value Type = 'Text'>" + ProjectSettings.TaskStatusRouted + "</Value></Eq>");
                    sb.Append("<In>");
                    sb.Append("<FieldRef Name = 'AssignedTo' />");
                    sb.Append("<Values>");
                    int i = 1;
                    foreach (string s in values.Values)
                    {
                        if (i >= Settings.QueriesInValuesMax - 1)
                            break;
                        if (!string.IsNullOrWhiteSpace(s))
                            sb.Append(string.Format("<Value Type = 'User'>{0}</Value>", s.Trim()));
                        i++;
                    }
                    sb.Append("</Values>");
                    sb.Append("</In>");
                    sb.Append("</And></Where>");
                    // Build a query.
                    SPQuery query = new SPQuery();
                    query.Query = sb.ToString();
                    // Get data from a list.                 
                    SPList oSSWPList = web.Lists[ProjectSettings.SPListTasks];
                    items = oSSWPList.GetItems(query);
                }
            }
            catch (Exception e)
            {
                //
            }
            return items;
        }
        public static DataTable GetTasksForTasksPage(string loginName, SPWeb web, string filterOptions)
        {
            SPListItemCollection pendingItems = null;
            DataTable dt = ProjectUtilities.CreateTableTasks();
            string taskStatus = Settings.PendingTaskStatusText;
            //if (filterOptions == Settings.MyPendingTaskOnly)
            //    pendingItems = GetPendingTasksCurrentUser(loginName, web);
            if (filterOptions == Settings.AllPendingTasks)
                pendingItems = GetPendingTasksByMeNProxiedFors(loginName, web);
            else if (filterOptions.Equals(Settings.AllTasksAssignedToMe))
            {
                pendingItems = GetTakenTasksByMeNProxiedFors(loginName, web);
                taskStatus = Settings.CompletedTaskStatusText;
            }
            if (pendingItems != null && pendingItems.Count > 0)
            {
                foreach (SPListItem item in pendingItems)
                {
                    string rStatus = Convert.ToString(item["TaskStatus"]);
                    if (rStatus != ProjectSettings.TaskStatusNone)
                    {
                        DataRow dr = dt.NewRow();
                        string mid = Convert.ToString(item["MasterID"]);
                        dr["MasterID"] = mid.Trim();
                        dr["TaskStatus"] = taskStatus;
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt.DefaultView.ToTable(true, "MasterID", "TaskStatus");
        }
        //public static SPListItem GetMDDTaskByApprovalCode(SPWeb dWeb, string mID, string approvalCode)
        //{
            
        //    try
        //    {
        //        SPListItemCollection items = GetAllMasterItemTasksAssigned(dWeb, mID);
        //        foreach(SPListItem item in items)
        //        {
        //            string appCode = Convert.ToString(item["ApprovalTypeCode"]).Trim();
        //            if (appCode == approvalCode)
        //                return item;
        //        }
        //    }
        //    catch
        //    {
        //        //
        //    }
        //    return null;
        //}
        public static DataTable FilterValidProxies(DataTable dtProxies)
        {
            try
            {
                return (from t in dtProxies.AsEnumerable() where (Convert.ToDateTime(t.Field<DateTime?>("StartDate")) <= DateTime.Now && Convert.ToDateTime(t.Field<DateTime?>("EndDate")) >= DateTime.Now) select t).CopyToDataTable();
            }
            catch { }
            return CreateEmptyProxiesTable();
        }

        public static List<string> GetEmailsWithProxiesEmailsByLoginList(SPWeb web, string login)
        {
            List<string> approverLogins = new List<string>();
            approverLogins.Add(login);
            return GetEmailsWithProxiesEmailsByLoginList(web,approverLogins);
        }

        public static List<string> GetEmailsWithProxiesEmailsByLoginList(SPWeb web, List<string> approverLogins)
        {
            SPListItemCollection items = null;
            List<string> initialApprovalLogins = new List<string>();
            foreach (string s in approverLogins)
            {
                initialApprovalLogins.Add(s.Trim().ToLower());
            }
            List<string> emails = new List<string>();
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    // Build a query.
                    SPQuery query = new SPQuery();
                    query.Query = string.Concat(
                                   "<Where><And>" +
                                      "<Neq><FieldRef Name='Proxy'/><Value Type='User'>-1</Value></Neq>" +
                                      "<Eq><FieldRef Name='Status'/><Value Type='Text'>Active</Value></Eq>" +
                                   "</And></Where>" +
                                    "<OrderBy>" +
                                    "<FieldRef Name='PrimaryUser' Ascending='TRUE' />" +
                                    "</OrderBy>"
                                   );
                    query.ViewFields = string.Concat("<FieldRef Name='PrimaryUser' /><FieldRef Name='Proxy' /><FieldRef Name='StartDate' /><FieldRef Name='EndDate' /><FieldRef Name='ID' /><FieldRef Name='Status' />");
                    query.ViewFieldsOnly = true;

                    // Get data from a list.                 
                    items = SPHelper.GetItems(Settings.Proxies, query, web);
                    DataTable dt = FilterValidProxies(GetProxiesTableFromItemCollection(items));
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string loginNameProxy = Convert.ToString(dr["ProxyLogin"]).Trim();
                            string loginNamePrimaryUser = Convert.ToString(dr["PrimaryUserLogin"]).Trim();
                            try
                            {
                                if (approverLogins.Contains(loginNamePrimaryUser.ToLower().Trim()) && !initialApprovalLogins.Contains(loginNameProxy.Trim().ToLower()))
                                    initialApprovalLogins.Add(loginNameProxy.Trim().ToLower());
                            }
                            catch
                            {
                                // Prevent - Eleminate dublication
                            }
                        }
                    }
                    //
                    foreach (string str in initialApprovalLogins)
                    {
                        string email = ProjectUtilities.GetEmailByUser(str);
                        if (!emails.Contains(email) && !string.IsNullOrEmpty(email))
                            emails.Add(email);

                    }


                });
            }
            catch (Exception e)
            {
                //
            }
            return emails;
        }
    }
}
