using BART.SP.OCR.CP.Common;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BART.SP.OCR.CP.Model
{
    [Serializable]
    public class TaskItemObject
    {
        public string Title { get; set; }
        public string MasterID { get; set; }
        public string TaskStatus { get; set; }
        public string TaskId { get; set; }
        public SPUser AssignedTo { get; set; }
        public string AssignedToName { get; set; }
        public DateTime ApprovedDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime DateCreated { get; set; }
        public string ApprovalTypeCode { get; set; }
        public string ApprovalRole { get; set; }
        public string ApprovalLevel { get; set; }
        public SPUser ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
        
        public bool AssignToGroup { get; set; }
        
        //public string ApprovedByName { get; set; }
        //public string ApprovedByLogin { get; set; }
        public string Note { get; set; }
        public int ApprovalOrder { get; set; }
        public string ReAssignedFromTaskID { get; set; }

        public TaskItemObject()
        {

        }
        public TaskItemObject(SPWeb dWeb, string tId)
        {
            SPListItem task = dWeb.Lists[ProjectSettings.SPListTasks].GetItemById(Convert.ToInt32(tId));
            InitTaskObject(task);
        }
        public TaskItemObject(SPListItem t)
        {
            InitTaskObject(t);
        }

        public void InitTaskObject(SPListItem t)
        {
            this.TaskId = Convert.ToString(t.ID);
            this.MasterID = Convert.ToString(t["MasterID"]);
            this.TaskStatus = Convert.ToString(t["TaskStatus"]);
            this.ApprovalOrder = Convert.ToInt32(t["ApprovalOrder"]);
            this.Note = Convert.ToString(t["Note"]);
            this.Title = Convert.ToString(t["Title"]);
            this.ApprovalLevel = Convert.ToString(t["ApprovalLevel"]);
            this.ApprovalRole = Convert.ToString(t["ApprovalRole"]);
            this.ApprovalTypeCode= Convert.ToString(t["ApprovalTypeCode"]);
            //this.AssignToGroup = Convert.ToBoolean(t["AssignToGroup"]);
            //
            this.DueDate = Convert.ToDateTime(t["DueDate"]);
            this.DateCreated = Convert.ToDateTime(t["DateCreated"]);
            this.ApprovedDate = Convert.ToDateTime(t["ApprovedDate"]);
            if (this.ApprovedDate < this.DateCreated)
                this.ApprovedDate = this.DateCreated;
            this.AssignedTo = SPHelper.GetSPUserFromFieldInItem(t.Web, t, "AssignedTo");
            try
            {
                if(this.AssignedTo!=null)
                    this.AssignedToName = this.AssignedTo.Name;
                else
                    this.AssignedToName = Convert.ToString(t["AssignedToName"]);
            }
            catch {

            }
            // ------------------------------------------------------------------------------
            this.ApprovedBy = SPHelper.GetSPUserFromFieldInItem(t.Web, t, "ApprovedBy");
            try
            {
                if (this.ApprovedBy != null)
                    this.ApprovedByName = this.ApprovedBy.Name;
            }
            catch
            {
            }
        }

        public void CreateApprovalTaskItem(SPWeb dWeb)
        {
            // Tasks for Department Manager
            SPList taskList = dWeb.Lists[ProjectSettings.SPListTasks];
            SPListItem task = taskList.Items.Add();
            task["Title"] = this.Title;
            task["MasterID"] = this.MasterID;
            task["ApprovalOrder"] = this.ApprovalOrder;
            task.SystemUpdate();
            this.TaskId = task.ID.ToString();
            task["AssignedTo"] = this.AssignedTo;
            task["ApprovalTypeCode"] = this.ApprovalTypeCode;
            task["ApprovalRole"] = this.ApprovalRole;
            task["AssignToGroup"] = this.AssignToGroup;
            
            task["ApprovalLevel"] = this.ApprovalLevel;
            task["TaskStatus"] = this.TaskStatus;
            task["TaskId"] = this.TaskId;
            task["DateCreated"] = DateTime.Now;
            task["ApprovedDate"] = DateTime.Now;
            if(this.AssignedTo!=null)
                task["AssignedToName"] = this.AssignedTo.Name;
            if (!string.IsNullOrEmpty(this.ReAssignedFromTaskID))
            {
                task["ReAssignedFromTaskID"] = this.ReAssignedFromTaskID.Trim();
                
            }
            task.SystemUpdate();
        }
        public void UpdateApprovalTaskItem(SPWeb dWeb)
        {
            // Tasks for Department Manager
            SPList taskList = dWeb.Lists[ProjectSettings.SPListTasks];
            SPListItem task = taskList.Items.GetItemById(Convert.ToInt32(this.TaskId));
            task["Title"] = this.Title;
            task["AssignedTo"] = this.AssignedTo;
            task["AssignToGroup"] = this.AssignToGroup;
            task["TaskStatus"] = this.TaskStatus;
            task["DateCreated"] = DateTime.Now;
            task["ApprovedDate"] = DateTime.Now;
            task.SystemUpdate();
        }
        public void DeleteTaskItem(SPWeb dWeb)
        {
            SPListItem task = dWeb.Lists[ProjectSettings.SPListTasks].GetItemById(Convert.ToInt32(this.TaskId));
            task.Recycle();
        }
        public void RouteItem(SPWeb dWeb, string requestInfo,string requesterLogin,bool isOCRAnalystTask=false,SPUser ocrAnalyst=null)
        {
            SPListItem task = dWeb.Lists[ProjectSettings.SPListTasks].GetItemById(Convert.ToInt32(this.TaskId));
            if (isOCRAnalystTask)
            {
                if (ocrAnalyst != null)
                {
                    this.AssignedTo = ocrAnalyst;
                    task["AssignedTo"] = ocrAnalyst;
                }
            }
            task["TaskStatus"] = ProjectSettings.TaskStatusRouted;
            task.SystemUpdate();
            this.EmailRouted(dWeb, requestInfo, requesterLogin);
            //
        }
        public void EmailRouted(SPWeb dWeb, string requestInfo, string requesterLogin)
        {
            // Emails to all Approver and its proxies
            List<string> emails = new List<string>();
            string viewURL = ProjectUtilities.GetPagesFolderURL() + BART.SP.OCR.CP.Common.ProjectUtilities.ViewItemUrl(this.MasterID);
            string title = string.Format(Settings.RoutetoApproversEmailTitle, requestInfo);// string.Format("[MDD: {0} - {1}]", this.WorkTitle, this.ConsultantName));
            string body = string.Format(Settings.RoutetoApproversEmailBodyStandard, viewURL);
            emails = ProjectHelper.GetEmailsWithProxiesEmailsByLoginList(dWeb, this.AssignedTo.LoginName);
            ProjectUtilities.SendEmailToMultiple(SPHelper.GetEmailByUser(requesterLogin), string.Empty, title, body, string.Empty, ProjectUtilities.TrimEmailListToString(emails));
        }
        public void Vote(SPWeb dWeb, string decision)
        {
            SPListItem task = dWeb.Lists[ProjectSettings.SPListTasks].GetItemById(Convert.ToInt32(this.TaskId));
            task["TaskStatus"] = decision;
            task["ApprovedDate"] = DateTime.Now;
            task["ApprovedBy"] = dWeb.CurrentUser;
            task.SystemUpdate();
        }

    }


    




}
