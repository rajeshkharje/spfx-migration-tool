using BART.SP.OCR.CP.Model;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BART.SP.OCR.CP.Common
{
    public class ProjectUtilities
    {


        /// <summary>
        /// Get filename to display on the application. - only use this when we want to display file name as text on applcation - Not use for URL
        /// </summary>
        /// <param name="fileNamewithGuid"></param>
        /// <returns></returns>
        public static string GetSSWPFileNameToDisplay(string fileNamewithGuid)
        {

            string[] strs = Regex.Split(fileNamewithGuid, Settings.SpecKeyGetFileName);
            if (strs != null && strs.Length > 1)
                return strs[1].Trim();
            return fileNamewithGuid.Trim();
        }

        /// <summary>
        /// Make file name unique - with guid
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string MakeSSWPFileName(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
                return string.Format("{0}{1}{2}", Guid.NewGuid().ToString().Replace("-",string.Empty), Settings.SpecKeyGetFileName, fileName);
            return string.Empty;
        }
        public static string TrimFileName(string fileName)
        {
            //~;#;%;&;;*;:;<;>;?;/;{;|;};"
            string fName= fileName.Replace("~", "_").Replace("#", "_").Replace("%", "_").Replace("&", "_").Replace("/", "_").Replace("{", "_").Replace("}", "_")
                .Replace("*", "_").Replace(":", "_").Replace("<", "_").Replace(">", "_").Replace("?", "_").Replace("\"", "_").Trim();
            //
            if (fName.Count() > 90 && fName.Contains("."))
            {
                string ext = fName.Split('.').Last();
                fName = string.Format("{0}.{1}", fileName.Substring(0, (90 - (ext.Count() + 1))), ext);
            }
            return fName;
        }

        //Department_View_Group_Name
        public static string Get_DepartmentViewGroupName(string deptname)
        {
            if(!string.IsNullOrEmpty(deptname))
            {
                return string.Format("CP_{0}_View", deptname.Trim());
            }
            return string.Empty;
            
        }

        public static string DisplayStringonHeaderPrint(string textVal,int num=100)
        {
            if (string.IsNullOrWhiteSpace(textVal))
                return string.Empty;
            return (textVal.Length <= num) ? textVal + " (cont.)" : string.Format("{0} (cont.)", textVal.Substring(0, num));
        }
        public static string TrimTitleforHeaderPrint(string textVal, int num = 35)
        {
            if (string.IsNullOrWhiteSpace(textVal))
                return string.Empty;
            return (textVal.Length <= num) ? textVal : string.Format("{0}...", textVal.Substring(0, num));
        }
        public static string trimTitleField(string textVal)
        {
            if (string.IsNullOrWhiteSpace(textVal))
                return string.Empty;
            return (textVal.Length <=255) ? textVal: string.Format("{0}...", textVal.Substring(0, 245));
        }
        public static DateTime ResultLinQNullDateTime(object input)
        {
            if (input == null)
                return DateTime.MinValue;
            else
                return Convert.ToDateTime(input);
        }
        public static string GetIfCheckedView(object input)
        {
            if (Convert.ToBoolean(input))
                return "✔";
            else
                return string.Empty;
        }
        public static string GetIfYesNoView(object input)
        {
            if (Convert.ToBoolean(input))
                return "Yes";
            else
                return "No";
        }

        /// <summary>
        /// Get the files uplaoded for any PND by title 
        /// </summary>
        /// <param name="oWeb"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        /*public static DataTable GetFilesByPNDId(int id, string ApprovalStep)
        {
            string fileIcon = "docIcon";
            string fileNameField = "FileName";
            string fileUrlField = "Url";
            string fileIdField = "ID";
            SPWeb oWeb = PNDSharePointHelper.WorkingWeb;

            DataTable fileTable = new DataTable();
            fileTable.Columns.Add(new DataColumn(fileNameField, typeof(string)));
            fileTable.Columns.Add(new DataColumn(fileUrlField, typeof(string)));
            fileTable.Columns.Add(new DataColumn(fileIdField, typeof(string)));
            fileTable.Columns.Add(new DataColumn(fileIcon, typeof(string)));
            // Build a query.
            SPQuery query = new SPQuery();
            query.Query = string.Concat(
                           "<Where><And>" +
                             "<Eq><FieldRef Name='ApprovalStep'/><Value Type='Text'>" + ApprovalStep + "</Value></Eq>" +
                              "<Eq><FieldRef Name='PNDID'/><Value Type='Text'>" + id.ToString() + "</Value></Eq>" +
                           "</And></Where>"
                           );

            // Get data from a list.                 
            SPList oListPND = oWeb.Lists[PNDGlobalSetting.LibraryPNDAttachment];
            SPListItemCollection items = oListPND.GetItems(query);
            if (items != null)
            {
                foreach (SPListItem item in items)
                {
                    DataRow dr = fileTable.NewRow();
                    string realFileName = Convert.ToString(item["FileLeafRef"]);
                    string fileName2Display = GetPNDFileNameToDisplay(realFileName);
                    dr[fileNameField] = fileName2Display;
                    dr[fileIdField] = item.ID.ToString();
                    dr[fileUrlField] = string.Format("{0}/{1}/{2}", oWeb.Url, item.ParentList.RootFolder, realFileName);
                    //
                    string docicon = SPUtility.ConcatUrls("/_layouts/images",
                    SPUtility.MapToIcon(item.Web, SPUtility.ConcatUrls(item.Web.Url, item.Url), "", IconSize.Size16));
                    dr[fileIcon] = string.Format("<img src='{0}' />", docicon);
                    fileTable.Rows.Add(dr);



                }
            }
            //
            return fileTable;
        }*/

        public static SPListItemCollection GetFilesListByPNDId(int id, SPWeb oWeb, string ApprovalStep)
        {
            List<string> idList = new List<string>();
            SPQuery query = new SPQuery();
            query.Query = string.Concat(
                           "<Where><And>" +
                             "<Eq><FieldRef Name='ApprovalStep'/><Value Type='Text'>" + ApprovalStep + "</Value></Eq>" +
                              "<Eq><FieldRef Name='PNDID'/><Value Type='Text'>" + id.ToString() + "</Value></Eq>" +
                           "</And></Where>"
                           );
            //
            SPList oListPND = oWeb.Lists[Settings.SSWPAttachments];
            SPListItemCollection items = oListPND.GetItems(query);
            if (items != null)
            {
                return items;
            }
            return null;
            //
        }

        /*
        public DataTable GetFilesByPNDId(string pndID)
        {
            string fileNameField = "FileName";
            string fileUrlField = "Url";
            string fileIdField = "ID";
            SPWeb oWeb = PNDSharePointHelper.WorkingWeb;
            DataTable fileTable = new DataTable();
            fileTable.Columns.Add(new DataColumn(fileNameField, typeof(string)));
            fileTable.Columns.Add(new DataColumn(fileUrlField, typeof(string)));
            fileTable.Columns.Add(new DataColumn(fileIdField, typeof(string)));
            SPListItemCollection items = GetFileUploadedItemsByPNDId(oWeb, pndID);
            // Build a query.
            if (items != null)
            {
                foreach (SPListItem item in items)
                {
                    DataRow dr = fileTable.NewRow();
                    string fileName = GetPNDFileNameToDisplay(Convert.ToString(item["FileLeafRef"]));
                    dr[fileNameField] = fileName;
                    dr[fileIdField] = item.ID.ToString();
                    dr[fileUrlField] = string.Format("{0}/{1}/{2}", oWeb.Url, item.ParentList.RootFolder, fileName);
                    fileTable.Rows.Add(dr);
                }
            }
            //
            return fileTable;
        }*/
        /// <summary>
        /// Gets the files uploaded for any PND title
        /// </summary>
        /// <param name="oWeb"></param>
        /// <param name="pndID"></param>
        /// <returns></returns>
        public SPListItemCollection GetFileUploadedItemsBysswpId(SPWeb oWeb, string sswpId)
        {
            SPListItemCollection items = null;
            SPQuery query = new SPQuery();
            query.Query = string.Concat(
                           "<Where>" +

                              "<Eq><FieldRef Name='SSWPID'/><Value Type='Text'>" + sswpId + "</Value></Eq>" +
                           "</Where>"
                           );

            SPList oListPND = oWeb.Lists[Settings.SSWPAttachments];
            items = oListPND.GetItems(query);
            return items;
        }
        /// <summary>
        /// Builds the hash that is used as identifier to display the PND 
        /// </summary>
        /// <param name="PNDID"></param>
        /// <param name="author"></param>        
        /// <returns></returns>
        public static string BuildHashforDisplayPND(string PNDID, string author)
        {
            try
            {
                //string array for encryption                
                byte[] userBytes = Encoding.UTF8.GetBytes(PNDID + author);

                //hasher for sha256
                HashAlgorithm hash = new SHA256Managed();

                //build hash
                byte[] hashBytes = hash.ComputeHash(userBytes);


                string hashValue = Convert.ToBase64String(hashBytes);

                return hashValue;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string BuildTopMenu(string currentURL)
        {
            currentURL = currentURL.ToLower();
            //string now = string.Format("{0}{1}{2}{3}",DateTime.Now.Year.ToString(),DateTime.Now.Month.ToString(),DateTime.Now.Day.ToString(),DateTime.Now.Hour.ToString());
            //string randomQuery = string.Format("?view={0}", now);
            string activeLi = "<li class=\"active\">";
            string strSSWP = "<li><a href =\"" + ProjectSettings.PageHome + "\">My CP</a></li >";
            strSSWP = (currentURL.Contains(ProjectSettings.PageHome.ToLower().Trim())) ? strSSWP.Replace("<li>", activeLi) : strSSWP;

            string strSSWPAll = "<li><a href =\""+ ProjectSettings.PageAll + "\">All CP</a></li >";
            strSSWPAll = (currentURL.Contains(ProjectSettings.PageAll.ToLower().Trim())) ? strSSWPAll.Replace("<li>", activeLi) : strSSWPAll;
            string strCreateRR = "<li><a href =\""+ ProjectSettings.PageCreateNew + "\">Create New CP</a></li>";
            strCreateRR = (currentURL.Contains(ProjectSettings.PageCreateNew.ToLower().Trim())) ? strCreateRR.Replace("<li>", activeLi) : strCreateRR;
            strCreateRR = (currentURL.Contains(ProjectSettings.PageEdit.ToLower().Trim())) ? string.Format("{0}<a>Edit CP</a></ li >", activeLi) : strCreateRR;
            strCreateRR = (currentURL.Contains(ProjectSettings.PageEdit.ToLower().Trim()) && currentURL.Contains(Settings.RevisionQueryValue.Trim().ToLower())) ? string.Format("{0}<a>Revise CP</a></ li >", activeLi) : strCreateRR;
            string strProxies = "<li><a href =\""+ ProjectSettings.PageProxies + "\">Proxies</a></ li >";
            strProxies = (currentURL.Contains(ProjectSettings.PageProxies.ToLower().Trim())) ? strProxies.Replace("<li>", activeLi) : strProxies;
            string strSSWPApprovals = "<li><a href =\""+ ProjectSettings.PageApprovals + "\">My Approval</a></li>";
            strSSWPApprovals = (currentURL.Contains(ProjectSettings.PageApprovals.ToLower().Trim())) ? strSSWPApprovals.Replace("<li>", activeLi) : strSSWPApprovals;
            //string strAllSSWP = "<li><a href =\"" + Settings.PageAllSSWP + "\">All SSWP</a></li>";
            //strAllSSWP = (currentURL.Contains(Settings.PageAllSSWP.ToLower().Trim())) ? strAllSSWP.Replace("<li>", activeLi) : strAllSSWP;
            string lHelp = "<li><a class=\"helpcsslink\" target=\"_blank\" href=\""+ ProjectSettings.PageHelp+ "\">Help</a></li>";
            lHelp = (currentURL.Contains(ProjectSettings.PageHelp.ToLower().Trim())) ? lHelp.Replace("<li>", activeLi) : lHelp;

            string lExport = "<li><a class=\"helpcsslink\" target=\"_blank\" href=\"" + Settings.ExportingURL + "\" title=\"Export all approved and under review Contracting Plan to excel\">Export</a></li>";
            
            string lblProjectAdmin = "<li><a class=\"helpprojectlink\" href=\"" + Settings.ProjectListURL + "\" title=\"Manage Project List for Contracting Plan Application\">Manage Project List</a></li>";
            lblProjectAdmin = (currentURL.Contains(ProjectSettings.PageProjectList.ToLower().Trim())) ? lblProjectAdmin.Replace("<li>", activeLi) : lblProjectAdmin;


            string contractExport = "<li><a href =\"" + ProjectSettings.PageContractExport + "\">Contract Exporting</a></li >";
            contractExport = (currentURL.Contains(ProjectSettings.PageContractExport.ToLower().Trim())) ? contractExport.Replace("<li>", activeLi) : contractExport;

            StringBuilder sb = new StringBuilder();
            sb.Append("<nav class=\"sswptopmenu navbar navbar-default\">");
            sb.Append("<div class=\"navbar-collapse second-navbar\" id=\"navbar\"><ul class=\"nav navbar-nav main-nav\">");
            sb.Append(strSSWP);
            if (ProjectHelper.IfViewAllShow())
                sb.Append(strSSWPAll);
            sb.Append(strCreateRR); sb.Append(strSSWPApprovals); 
            sb.Append(strProxies); 
            sb.Append(lHelp);
            if (SPHelper.IsMemberOfGroup(Settings.GroupExporting) || SPHelper.IsMemberOfGroup(Settings.AdminGroupName))
            {
                sb.Append(lExport);
            }
            if (SPHelper.IsMemberOfGroup(Settings.GroupAdminProject) || SPHelper.IsMemberOfGroup(Settings.AdminGroupName))
            {
                sb.Append(lblProjectAdmin);
            }
            if (SPHelper.IsMemberOfGroup(Settings.GroupContractAdmin))
            {
                sb.Append(contractExport);
            }
                
                sb.Append("</ul>");
            sb.Append("</div></nav>");
            //-------------------------//
            return sb.ToString();
        }
        public static string DecryptString(string encrString)
        {
            byte[] b;
            string decrypted;
            if (encrString.Contains(Settings.SpecialCharacterReplceEqualChar))
                encrString = encrString.Replace(Settings.SpecialCharacterReplceEqualChar, "=");
            try
            {
                b = Convert.FromBase64String(encrString);
                decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
            }
            catch (FormatException fe)
            {
                decrypted = "";
            }
            return decrypted;
        }

        public static string EnryptString(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            if (encrypted.Contains("="))
                encrypted = encrypted.Replace("=",  Settings.SpecialCharacterReplceEqualChar);
            return encrypted;
        }

        public static string MakeQueryStringWithExpiry(object id, object sendToAccs)
        {
            return EnryptString(string.Format("{0}_{1}_{2}", MakeViewQueryString(id),sendToAccs.ToString(), DateTime.Now.AddHours(72).ToString()));
        }
        public static string GetMainItemIDFromQuery(string eQueryVal)
        {
            string deVal = DecryptString(eQueryVal);
            if (deVal.Contains("_"))
                return deVal.Split('_')[0];
            return string.Empty;
        }
        public static string MakeEditQueryString(object id)
        {
            return EnryptString(string.Format("{0}_{1}{2}",id.ToString(),Settings.EditCodepage, new Random().Next(100, 10000).ToString()));
        }
        public static string MakeViewQueryString(object id)
        {
            return EnryptString(string.Format("{0}_{1}{2}", id.ToString(), Settings.ViewCodepage, new Random().Next(100, 10000).ToString()));
        }
       
        public static string PrintReportURL(object id)
        {
            return string.Format("{0}?reportview={1}", ProjectSettings.PagePrint, MakeViewQueryString(id));
        }
        public static string ExportMasterItemURL(object id)
        {
            return string.Format("{0}?reportview={1}", ProjectSettings.PageExportPDF, MakeViewQueryString(id));
        }
        public static string ViewADDURL(object id)
        {
            return string.Format("{0}?{1}={2}", Settings.PageAddViewDetails, Settings.AddendumQueryString, MakeViewQueryString(id));
        }
        public static string PrintADDURL(object id)
        {
            //return string.Format("{0}?{1}={2}", ProjectSettings.PagePrintAddendum, Settings.AddendumQueryString, MakeViewQueryString(id));
            return string.Empty;
        }

        // Use as standard
        public static string PrintItemURL(object id)
        {
            return string.Format("{0}?reportview={1}", ProjectSettings.PagePrint, MakeViewQueryString(id));
        }
        // Use as standard
        public static string ViewItemUrl(object id)
        {
            return string.Format("{0}?reportview={1}", ProjectSettings.PageView, MakeViewQueryString(id));
        }
        // use as standard
        public static string EditItemURL(object id)
        {
            return string.Format("{0}?reportview={1}", ProjectSettings.PageEdit, MakeEditQueryString(id));
        }


        public static string EditReportURL(object id)
        {
            return string.Format("{0}?reportview={1}", ProjectSettings.PageEdit, MakeEditQueryString(id));
        }
        public static string EditADDURL(object id)
        {
            return string.Format("{0}?{1}={2}", Settings.PageAddEdit, Settings.AddendumQueryString, MakeEditQueryString(id));
        }
        public static bool IfRoutenDeleteAvailable(object status)
        {
            return (Convert.ToString(status).Trim() == ProjectSettings.ProjectStatusDraft.Trim()) ? true : false;
        }
        public static bool EditButtonVisibility(object state, object status, object ifMy)
        {
            return  (Convert.ToString(state).Trim() == Settings.ViewAllAdmin.Trim()) || (Convert.ToString(state).Trim() == Settings.ViewMy.Trim() && Convert.ToBoolean(ifMy) && Convert.ToString(status)!=ProjectSettings.ProjectStatusCompleted) ? true : false;
        }
        public static string Css4RouteButton(object status)
        {
            return (Convert.ToString(status).Trim() == ProjectSettings.ProjectStatusDraft.Trim()) ? "sswpRouteLinkInGrid circalIcon": "sswpRouteLinkInGrid circalIconDisabled";
        }
        public static string ToolTip4RouteButton(object status)
        {
            return (Convert.ToString(status).Trim() == ProjectSettings.ProjectStatusDraft.Trim()) ?"Route RR":"";
        }
        public static string DisplayStringonGrid(object textVal)
        {
            if (textVal == null || (string.IsNullOrWhiteSpace(Convert.ToString(textVal))))
                return "N/A";
            return (Convert.ToString(textVal).Length <= 65) ? textVal.ToString() : string.Format("{0}...", textVal.ToString().Substring(0, 65));
        }
        public static string DisplayDateTime(object textVal)
        {
            if (textVal == null || (string.IsNullOrWhiteSpace(Convert.ToString(textVal))))
                return string.Empty;
            DateTime d = new DateTime();
            try
            {
                d = Convert.ToDateTime(textVal);
                
            }
            catch
            {
                //
            }
            if (d != DateTime.MinValue)
                return string.Format("{0}/{1}/{2}",d.Month.ToString(),d.Day.ToString(), d.Year.ToString());
            return string.Empty;
      
        }
        public static string DisplayDateTimeMMDDYYYY(object textVal)
        {
            if (textVal == null || (string.IsNullOrWhiteSpace(Convert.ToString(textVal))))
                return string.Empty;
            try
            {
                return Convert.ToDateTime(textVal).ToString("MM/dd/yyyy");
            } catch{}
            //
            return string.Empty;

        }
        public static string DisplayDateTimeMMDDYYYYDash(object textVal)
        {
            if (textVal == null || (string.IsNullOrWhiteSpace(Convert.ToString(textVal))))
                return string.Empty;
            try
            {
                return Convert.ToDateTime(textVal).ToString("MM-dd-yyyy");
            }
            catch { }
            //
            return string.Empty;

        }
        public static string StringShowEmptyWithSpace(object textVal)
        {
            string val = string.Empty;
            if (textVal != null)
                val= Convert.ToString(textVal);
            if (string.IsNullOrEmpty(val))
                val = "&nbsp;";
            return val;

        }
        public static string CSSClassByOrder(object textVal)
        {
            if (textVal != null)
            {
                if (Convert.ToString(textVal) == "1")
                    return "form-control txtviewonly text-center schedulefirstrow";
                else
                    return "form-control";
            }
            return "form-control";

        }
        public static bool IsScheduleReadOnly(object textVal)
        {
            if (textVal != null)
            {
                if (Convert.ToString(textVal) == "1")
                    return true;
            }
            return false;
        }
        public static string GetCurrentDateTimeShortFormat()
        {
            return string.Format("{0} {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
        }
        public static List<string> GetListApproverCodeFromString(string input)
        {
            List<string> deptCodes = new List<string>();
            if (input.Contains(";"))
            {
                string[] strs = input.Split(';');
                foreach (string s in strs)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        deptCodes.Add(s.Trim());
                    }
                }
            }
            return deptCodes;
        }
        public static string DisplayDateonGrid(object textVal)
        {
            if (textVal == null || (string.IsNullOrWhiteSpace(Convert.ToString(textVal))))
                return "N/A";

            if (textVal.ToString().Trim().Contains(" "))
            {
                return textVal.ToString().Trim().Split(' ')[0];
            }
            return textVal.ToString().Trim();
        }

        public static DataTable CreateDefaultPrjItemsTable()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("ProjectName", typeof(string)));
            dt.Columns.Add(new DataColumn("ProjectID", typeof(string)));
            dt.Columns.Add(new DataColumn("MasterID", typeof(string)));
            dt.Columns.Add(new DataColumn("DateSubmitted", typeof(string)));
            dt.Columns.Add(new DataColumn("ProgramName", typeof(string)));
            dt.Columns.Add(new DataColumn("ProgramDes", typeof(string)));
            dt.Columns.Add(new DataColumn("UserCreated", typeof(string)));
            dt.Columns.Add(new DataColumn("Requester", typeof(string)));
            dt.Columns.Add(new DataColumn("Requester_Assigned", typeof(string)));
            dt.Columns.Add(new DataColumn("SponsorProjectManager", typeof(string)));
            dt.Columns.Add(new DataColumn("OCRAnalyst", typeof(string)));
            dt.Columns.Add(new DataColumn("OCRAnalyst_Assigned", typeof(string)));
            dt.Columns.Add(new DataColumn("SponsorDepartment", typeof(string)));
            dt.Columns.Add(new DataColumn("Status", typeof(string)));
            dt.Columns.Add(new DataColumn("KickoffMeetingDate", typeof(string)));
            dt.Columns.Add(new DataColumn("isEditable", typeof(bool)));
            dt.Columns.Add(new DataColumn("Modified", typeof(string)));
            dt.Columns.Add(new DataColumn("Created", typeof(string)));
           
            //dt.Columns.Add(new DataColumn("AssignedTo", typeof(string)));


            return dt;
        }
        public static DataTable CreateApprovalTable()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("ProjectName", typeof(string)));
            dt.Columns.Add(new DataColumn("MasterID", typeof(string)));
            dt.Columns.Add(new DataColumn("ProjectID", typeof(string)));
            dt.Columns.Add(new DataColumn("DateSubmitted", typeof(string)));
            dt.Columns.Add(new DataColumn("ProgramName", typeof(string)));
            dt.Columns.Add(new DataColumn("ProgramDes", typeof(string)));
            dt.Columns.Add(new DataColumn("UserCreated", typeof(string)));
            dt.Columns.Add(new DataColumn("Requester", typeof(string)));
            dt.Columns.Add(new DataColumn("Requester_Assigned", typeof(string)));
            dt.Columns.Add(new DataColumn("SponsorProjectManager", typeof(string)));
            dt.Columns.Add(new DataColumn("OCRAnalyst", typeof(string)));
            dt.Columns.Add(new DataColumn("OCRAnalyst_Assigned", typeof(string)));
            dt.Columns.Add(new DataColumn("SponsorDepartment", typeof(string)));
            dt.Columns.Add(new DataColumn("Status", typeof(string)));
            dt.Columns.Add(new DataColumn("KickoffMeetingDate", typeof(string)));
            dt.Columns.Add(new DataColumn("isEditable", typeof(bool)));
            dt.Columns.Add(new DataColumn("Modified", typeof(string)));
            dt.Columns.Add(new DataColumn("Created", typeof(string)));
            //dt.Columns.Add(new DataColumn("TypeIcon", typeof(string)));
            dt.Columns.Add(new DataColumn("TaskStatus", typeof(string)));
            dt.Columns.Add(new DataColumn("AssignedTo", typeof(string)));
            dt.Columns.Add(new DataColumn("AssignedToName", typeof(string)));
            dt.Columns.Add(new DataColumn("ApprovedBy", typeof(string)));
            dt.Columns.Add(new DataColumn("ApprovedDate", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("DateCreated", typeof(DateTime)));

            return dt;
        }
        public static DataTable CreateTableTasks()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("MasterID", typeof(string)));
            dt.Columns.Add(new DataColumn("TaskStatus", typeof(string)));
            dt.Columns.Add(new DataColumn("AssignedTo", typeof(string)));
            dt.Columns.Add(new DataColumn("AssignedToName", typeof(string)));
            dt.Columns.Add(new DataColumn("ApprovedBy", typeof(string)));
            dt.Columns.Add(new DataColumn("ApprovedDate", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("DateCreated", typeof(DateTime)));
            return dt;
        }

        

        public static DataTable CreateAddendumTableTasks()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("AddID", typeof(string)));
            dt.Columns.Add(new DataColumn("TaskStatus", typeof(string)));
            return dt;
        }
        public static DataTable CreateAddendumTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("AddID", typeof(string)));
            dt.Columns.Add(new DataColumn("SSWPID", typeof(string)));
            dt.Columns.Add(new DataColumn("AddStatus", typeof(string)));
            dt.Columns.Add(new DataColumn("AddNo", typeof(string)));
            dt.Columns.Add(new DataColumn("SubmittedDate", typeof(string)));
            dt.Columns.Add(new DataColumn("DueDate", typeof(string)));
            dt.Columns.Add(new DataColumn("AddNote", typeof(string)));
            return dt;
        }
        public static bool SendEmailToMultiple(string senderEmail, string to, string subject, string body, SPSite site, string cc="", string bcc="",bool isAcsync=false)
        {
            bool emailSent = false;
            try
            {
                MailMessage mailMessage = new MailMessage();
                if (!string.IsNullOrEmpty(senderEmail))
                    mailMessage.From = new MailAddress(senderEmail);//mailMessage.From = new MailAddress("spadmin@bart.gov");
                if (!string.IsNullOrEmpty(to))
                {
                    List<string> toEmails = TrimEmailList(to);
                    foreach (string t in toEmails)
                    {
                        mailMessage.To.Add(t);
                    }
                }
                if (!string.IsNullOrEmpty(cc))
                {
                    List<string> ccEmails = TrimEmailList(cc);
                    foreach (string t in ccEmails)
                    {
                        mailMessage.CC.Add(t);
                    }
                }
                if (!string.IsNullOrEmpty(bcc))
                {
                    List<string> bccEmails = TrimEmailList(bcc);
                    foreach (string t in bccEmails)
                    {
                        mailMessage.Bcc.Add(t);
                    }
                }
                SmtpClient smtpClient = new SmtpClient(GetGlobalInformtionValue(site));
                mailMessage.Subject = subject.Trim();
                mailMessage.Body = body;
                if (isAcsync)
                {
                    try
                    {
                        smtpClient.SendAsync(mailMessage, null);
                    }
                    catch
                    {
                        smtpClient.Send(mailMessage);
                    }
                }
                else
                    smtpClient.Send(mailMessage);
                emailSent = true;
            }
            catch (Exception ex)
            {
                //throw (ex);
            }
            return emailSent;
            
        }
        public static bool SendEmailToMultiple(string senderEmail, string to, string subject, string body, string cc = "", string bcc = "",bool isAcsync=false)
        {
            return SendEmailToMultiple(senderEmail, to, subject, body,ProjectHelper.AppHostedSite,cc,bcc, isAcsync);
        }
        public static string TrimEmailListToString(List<string> emails)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in emails)
            {
                sb.Append(string.Format("{0};",s.Trim()));
            }
            return sb.ToString().Trim().Trim(';');
        }

        private static List<string> TrimEmailList(string emails)
        {
            List<string> emailList = new List<string>();
            if (emails.Contains(";"))
            {
                string[] list = emails.Split(';');
                foreach (string s in list)
                {
                    if (!string.IsNullOrEmpty(s) && !emailList.Contains(s.Trim().ToLower()))
                        emailList.Add(s.ToLower());
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(emails))
                    emailList.Add(emails.ToLower());
            }
            //
            return emailList;
        }
        public static string GetGlobalInformtionValue()
        {
            string returnVal = string.Empty;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(ProjectHelper.AppHostedSite.ID))
                    {
                        returnVal = site.WebApplication.OutboundMailServiceInstance.Server.Address.Trim();
                    }
                });
            }
            catch (Exception ex)
            { }
            return returnVal;
        }
        public static string GetGlobalInformtionValue(SPSite siteInput)
        {
            string returnVal = string.Empty;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteInput.ID))
                    {
                        returnVal = site.WebApplication.OutboundMailServiceInstance.Server.Address.Trim();
                    }
                });
            }
            catch (Exception ex)
            { }
            return returnVal;
        }
        public static string GetEmailByUser(string fullUserName,SPWeb eventWeb=null)
        {
            //Previous
            //return (fullUserName.Contains("\\")) ? string.Format("{0}@bart.gov", Convert.ToString(fullUserName.Split('\\')[1]).Trim()) : string.Empty;
            if(eventWeb==null)
                return SPHelper.GetSPUserEmailLoginName(SPContext.Current.Web, fullUserName);
            else
                return SPHelper.GetSPUserEmailLoginName(eventWeb, fullUserName);
        }
        public static string GetPagesFolderURL()
        {
            return string.Format("{0}/{1}/", ProjectHelper.AppHostedWeb.Url, "SitePages");
        }
        // This is to structure that - Hosted Site is parent of Data Site
        public static string GetPagesFolderURLFromDataSite(SPWeb dWeb)
        {
            return string.Format("{0}/{1}/", dWeb.ParentWeb.Url, "SitePages");
        }
        public static char GetNextChar(char c)
        {
            // convert char to ascii
            int ascii = (int) c;
            // get the next ascii
            int nextAscii = ascii + 1;
            // convert ascii to char
            char nextChar = (char) nextAscii;
            return nextChar;
        }
        public static void LogError(string errorMessage)
        {
            //throw new Exception(errorMessage);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    using (SPSite dSite = new SPSite(ProjectHelper.DataSiteURL))
                    {
                        using (SPWeb dWeb = dSite.OpenWeb(ProjectHelper.DataWebRelativeURL))
                        {
                            SPList list = dWeb.Lists[ProjectSettings.SPListError];
                            SPListItem item = list.AddItem();
                            item["Title"] = "RR-Error-at" + DateTime.Now.ToString();
                            item["Details"] = errorMessage;
                            item.SystemUpdate();
                        }
                    }
                }
                catch
                {
                    //
                }
            }
            
        }

        public static string TrimBreakLineToHTML(object input)
        {
            if(input!=null)
                return Convert.ToString(input).Replace("\n", "<br>");
            return string.Empty;
        }
        public static string GetMonthStringByNumber(object input)
        {
            try
            {
                if (input == null)
                    return "N/A";
                else
                {
                    int val = Convert.ToInt32(input.ToString());
                    if (val < 10)
                        return string.Format("0{0}", val.ToString());
                    else
                        return val.ToString();
                }
            }
            catch (Exception ex)
            {

            }
            return "N/A";
        }

        public static void InitDefaultValByType(object obj,Type t, object value)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.PropertyType == t)
                {
                    propertyInfo.SetValue(obj, value);
                }
            }
        }

        public static string Serialize2XML(object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            StringBuilder result = new StringBuilder();
            using (var writer = XmlWriter.Create(result))
            {
                serializer.Serialize(writer, obj);
            }
            return result.ToString();
        }
        //public static AcquisitionHistoryList DeSerializeXMLAcquisitionHistoryList(string stringXML)
        //{
        //    XmlSerializer serializer = new XmlSerializer(typeof(AcquisitionHistoryList));
        //    StringReader stringReader = new StringReader(stringXML);
        //    AcquisitionHistoryList aList = (AcquisitionHistoryList)serializer.Deserialize(stringReader);
        //    return aList;
        //}
        public static ChangeOrderList DeSerializeXMLChangeOrderList(string stringXML)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ChangeOrderList));
            StringReader stringReader = new StringReader(stringXML);
            ChangeOrderList aList = (ChangeOrderList) serializer.Deserialize(stringReader);
            return aList;
        }


        public static string MakeConsultantID(string conlsltName)
        {
            if (!string.IsNullOrEmpty(conlsltName))
                return conlsltName.Replace(" ", string.Empty).Replace("&", "And").Replace("#", "Number")
                    .Replace("\\", string.Empty).Replace("$", "dollar").Replace("/", string.Empty).Replace("@", string.Empty);
            return string.Empty;
        }

        public static void InitObjectDynamic(object obj, SPWeb dWeb, SPListItem oListItem)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            SPFieldCollection fields = oListItem.Fields;
            List<string> internalNames = new List<string>();
            foreach (SPField f in fields)
            {
                if (f != null && !string.IsNullOrEmpty(f.InternalName))
                    internalNames.Add(f.InternalName);
            }

            foreach (var propertyInfo in properties)
            {
                string pName = propertyInfo.Name;
                if (internalNames.Contains(pName))
                {
                    Type pType = propertyInfo.PropertyType;
                    if (pType == typeof(string))
                    {
                        propertyInfo.SetValue(obj, Convert.ToString(oListItem[pName]));
                    }
                    //
                    if (pType == typeof(DateTime?))
                    {
                        if (oListItem[pName] != null)
                            propertyInfo.SetValue(obj, Convert.ToDateTime(oListItem[pName]));
                    }
                    if (pType == typeof(bool))
                    {
                        if (oListItem[pName] != null)
                            propertyInfo.SetValue(obj, Convert.ToBoolean(oListItem[pName]));
                    }
                    if (pType == typeof(SPUser))
                    {
                        try
                        {
                            SPUser user = ProjectHelper.GetSPUserValueByFieldName(oListItem, pName);
                            if (user != null)
                            {
                                propertyInfo.SetValue(obj, user);
                            }
                        }
                        catch
                        { }

                    }
                }
                
            }
         
        }


        public static void InitObjectDynamicDefault(object obj)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();

            foreach (var propertyInfo in properties)
            {
                string pName = propertyInfo.Name;
                Type pType = propertyInfo.PropertyType;
                if (pType == typeof(string))
                {
                    propertyInfo.SetValue(obj, string.Empty);
                }
                //
                if (pType == typeof(bool))
                {
                     propertyInfo.SetValue(obj,false);
                }
            }

        }
        
        public static void UpdateItemDynamic(object obj,SPListItem item)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            SPFieldCollection fields = item.Fields;
            foreach (var propertyInfo in properties)
            {
                try
                {
                    string pName = propertyInfo.Name;
                    Type pType = propertyInfo.PropertyType;
                    var value = propertyInfo.GetValue(obj);

                    if (pType == typeof(string))
                    {
                        item[pName] = Convert.ToString(value);
                    }
                    //
                    if (pType == typeof(DateTime?) || pType == typeof(DateTime))
                    {
                        if(value != null)
                            item[pName] = Convert.ToDateTime(value);//
                        else
                        {
                            try { item[pName] = null; } catch { }

                        }
                    }
                    if (pType == typeof(bool) && value != null)
                    {
                        item[pName] = Convert.ToBoolean(value);
                    }
                    if (pType == typeof(SPUser)) //&& value != null)
                    {
                        item[pName] = value;
                    }
                }
                catch
                { }
                
            }
            //
            item.Update();
        }
    }
}
