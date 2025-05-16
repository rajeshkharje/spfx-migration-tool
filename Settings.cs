using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BART.SP.OCR.CP.Common
{
    public class Settings
    {
        // Site and List Settings
        //public static string DataSiteRelativeURL
        //{
        //    get
        //    {
        //        string sURL = SPHelper.ReadSetting("SSWPDataSiteURL");
        //        return string.IsNullOrEmpty(sURL) ? "sites/sswp" : sURL;
        //    }
        //}
        //public static string DataWebRelativeURL
        //{
        //    get
        //    {
        //        string wURL = SPHelper.ReadSetting("SSWPDataWebURL");
        //        return string.IsNullOrEmpty(wURL) ? "data" : wURL;
        //    }
        //}

        //// UAT
        public static string DataSiteRelativeURL
        {
            get
            {
                string sURL = SPHelper.GetMappingVal("datasitecol");
                return string.IsNullOrEmpty(sURL) ? "sites/cp" : sURL;
            }
        }
        public static string DataWebRelativeURL
        {
            get
            {
                string wURL = SPHelper.GetMappingVal("dataspweb");
                return string.IsNullOrEmpty(wURL) ? "data" : wURL;
            }
        }
        public static string Ver2Date
        {
            get
            {
                string v2Date = SPHelper.GetMappingVal("v2_Date");
                return string.IsNullOrEmpty(v2Date) ? "03/26/2022" : v2Date;
            }
        }
        public static string ArchivedDocsSiteRelativeURL
        {
            get
            {
                string wURL = SPHelper.GetMappingVal("achiveddocssite");
                return string.IsNullOrEmpty(wURL) ? "sites/archivedcpdocs" : wURL;
            }
        }
        public static string ArchivedDocsWebRelativeURL
        {
            get
            {
                string wURL = SPHelper.GetMappingVal("achiveddocsweb");
                return string.IsNullOrEmpty(wURL) ? "data" : wURL;
            }
        }
        public static string ExportingURL
        {
            get
            {
                string wURL = SPHelper.GetMappingVal("exporturl");
                return string.IsNullOrEmpty(wURL) ? "#" : wURL;
            }
        }
        public static string ProjectListURL
        {
            get
            {
                string wURL = SPHelper.GetMappingVal("projectlist");
                return string.IsNullOrEmpty(wURL) ? "#" : wURL;
            }
        }
        //public static readonly string DataSiteRelativeURL = "sites/col";//QA CRP1
        //public static readonly string DataWebRelativeURL = "apps/sswp/data";//QA CRP1
        //public static readonly string DataSiteRelativeURL = "sites/col";//QA CRP2
        //public static readonly string DataWebRelativeURL = "apps/sswpcrp2/data";//QA CRP2
        //public static readonly string DataSiteRelativeURL = "sites/col";//QA CRP3
        //public static readonly string DataWebRelativeURL = "apps/sswpcrp3/data";//QA CRP3

        public static readonly string SSWPMasterList = "MasterReports";

        public static readonly string SSWPDeptApprovers = "Department_Approvers";

        public static readonly string SSWPDepartments = "SSWPDepartments";

        public static readonly string SSWPAttachments = "SSWPAttachments";

        public static readonly string SSWPApprovalTasks = "SSWP_Approval_Tasks";

        public static readonly string RRHistory = "RRHistory";

        public static readonly string SSWPComments = "SSWPComments";

        public static readonly string SSWPErrorHandling = "Error_Handling ";

        public static readonly string Proxies = "Proxies";
        public static readonly string spListOCRAnalyst = "OCRAnalyst";

        public static readonly string SSWP_Addendum = "AddMasterList";

        public static readonly string SSWP_Addendum_Attachments = "AddAttachments";

        public static readonly string SSWP_Addendum_Tasks = "AddApprovalTasks";

        public static readonly string SSWP_Addendum_History = "AddHistory";

        public static readonly string SSWP_Addendum_Comments = "AddComments";
        public static readonly string NoRecordFoundSearch = "No MDD approval task matches your search/filter";








        // SSWP Status Values
        public static readonly string SSWPStatusDraft_old = "Draft";
        public static readonly string SSWPStatusUnderReview_old = "Under Review";
        public static readonly string SSWPStatusOnHold_old = "On Hold";
        public static readonly string SSWPStatusCanceled_old = "Canceled";
        public static readonly string SSWPStatusCompleted_old = "Completed";
        public static readonly string SSWPStatusApproved_old = "Approved";
        //
        public static readonly int maxNumberCharsOfTaskTitle = 250;

        // SSWP Status Values
        public static readonly string AddStatusDraft = "Draft";
        public static readonly string AddStatusUnderReview = "Under Review";
        public static readonly string AddStatusOnHold = "On Hold";
        public static readonly string AddStatusCanceled = "Canceled";
        public static readonly string AddStatusCompleted = "Completed";
        public static readonly string AddStatusApproved = "Approved";


      

        //Emails and Notifications Settings

        //ListView Page State
        public static readonly string ViewAllAdmin = "ALL_ADMIN_a18b52134bf979a780f84eac70578f9e";
        public static readonly string ViewAllStandard = "ALL_STANDARD";
        public static readonly string ViewMy = "MY";


        //Common Settings
        public static readonly string PDFConverterKey = "SMbVx9LXx9/e18fRydfH1NbJ1tXJ3t7e3sfX";
        public static readonly string SpecKeyGetFileName = "__@__";
        public static readonly int FileSizeLimit = 1048576000;
        public static readonly char DeptApprovalsplit = ';';
        public static readonly int QueriesInValuesMax = 60;
        public static readonly string SpecialCharacterReplceEqualChar = "e7q9a9a";
        public static readonly string PageNotFound = "PageNotFoundError.aspx";
        public static readonly string EditCodepage = "33489";
        public static readonly string ViewCodepage = "84399";
        public static readonly string AdminGroupName = "CPAdmin";
        public static readonly string InternalGroupName = "CPAppsOwner";
        public static readonly string GroupAdminProject = "CPProjectListAdmin";
        public static readonly string GroupContractAdmin = "ContractAdmins";
        public static readonly string GroupExporting = "CPExporting";
        public static readonly string GroupOCRCompliance = "CPOCRCompliance";
        public static readonly string PMApprovalCode = "PMApr";
        public static readonly string REApprovalCode = "REApr";
        public static readonly string PMApprovalTitle = "Project Manager";
        public static readonly string REApprovalTitle = "Resident Engineer";
        public static readonly string LastDepartmentApprovalDeptGroup = "9";
        public static readonly string NoRecordFoundMyRR = "You have no RR Report";
        public static readonly string NoRecordFoundAllRR = "There is no shared RR Reports in the system at this moment.";
        public static readonly string WrongSSWPNoConNo = "<strong>* SSWP Number and Contract/Permit Number already used in the system. Please enter different value for SSWP Number and/or Contract/Permit Number.</strong>";
        public static readonly string cssHiddenDiv = "cssHiddenDiv";
        public static readonly string cssShowDiv = "cssShowDiv";
        public static readonly string HistoryTypeEdit = "Edit";
        public static readonly string HistoryTypeApproval = "Approval";
        public static readonly string HistoryTypeRouted = "Routed";
        public static readonly string EditPageViewStateTasks = "ViewSTasks";
        public static readonly string EditPageViewStateParentDepts = "ViewSPDepts";
        public static readonly string ItemTypeAddendum = "ADDType";
        public static readonly string ItemTypeSSWP = "SSWPType";
        public static readonly string DefaultNullValueForStringVal =string.Empty;

        // History Action Recorded
        public static readonly string HistoryActionRouted = "Submitted";
        public static readonly string HistoryActionReRouted = "Re-submitted";
        public static readonly string HistoryReroute = "re-routed to approver(s)";


        public static readonly string HistoryActionRevised = "Revision number {0} submitted and routed to all approvers";
        public static readonly string HistoryActionRoutedToNewApprovers = "Routed to new approver";
        public static readonly string HistoryActionUpdatedApprovers = "Modified and Routed to new approver";
        public static readonly string HistoryActionModified = "Modified";
        public static readonly string HistoryActionApproved = "Concurred";
        public static readonly string HistoryActionReject = "Revision requested";
        public static readonly string HistoryActionAbstain = "Abstained";
        public static readonly string HistoryActionRerouted = "Modified and Re-Routed to all approvers";
        // ViewState
        public static readonly string ViewStateStatusFilterValue = "SSWPStatusFilterVal";
        public static readonly string ViewStateStatusFilterSelected = "SSWPStatusFilterSelected";
        public static readonly string RevisionQueryValue = "531cc846a3bb4bbeb4f2a64fbea8dce7";

        //Addendum Pages
        public static readonly string PageAddViewDetails = "AddendumDetails.aspx";
        public static readonly string PageAddEdit = "EditAddendum.aspx";


        public static readonly string PageProjectList = "AdminProjectList.aspx";
        //

        public static readonly string MyPendingTaskOnly = "MyPendingTasks";
        public static readonly string AllPendingTasks = "AllPendingTasks";
        public static readonly string AllTasksAssignedToMe = "AllTasksAssignedToMeAndMyProxiesFor";
        //
        public static readonly string PendingTaskStatusText = "Pending";
        //
        public static readonly string CompletedTaskStatusText = "Taken";


        public static readonly string AddendumQueryString = "AddendumView";


        // Email templates
        // 1- Route
        public static readonly string RoutetoApproversEmailTitle = "Contracting Plan Approval Request: {0}";
        public static readonly string RoutetoApproversEmailBody = "A Contracting Plan has been routed to you for approval ({0}). For more details, please visit: {1}" + Environment.NewLine + Environment.NewLine + "Note: If the link above doesn't work, please copy and paste the URL into a browser.";
        public static readonly string RoutetoApproversEmailBodyStandard = "A Contracting Plan has been routed to you for approval. For more details, please visit: {0}" + Environment.NewLine + Environment.NewLine + "Note: If the link above doesn't work, please copy and paste the URL into a browser.";

        // 2- Notify Requestor
        public static readonly string NotiffyRequestorOfRoutingTitle = "Project Report Submission: [Project No: {0} - Report Date: {1}]";
        public static readonly string NotiffyRequestorOfRoutingBody = "Your RR Report has been routed to approver(s). For more details, please visit: {0}" + Environment.NewLine + Environment.NewLine + "Note: If the link above doesn't work, please copy and paste the URL into a browser.";
        
        // 3- Modification
        public static readonly string NotifyOfModificationTitle = "RR Report Attachment Update: [{0}]-{1}";
        public static readonly string NotifyOfModificationBody = "This SSWP's attachment has been updated. For more details, please visit: {0}" + Environment.NewLine + Environment.NewLine + "Note: If the link above doesn't work, please copy and paste the URL into a browser.";

        // 4- Concurences
        public static readonly string ConcurenceResultTitle = "Contracting Plan Reviewer Decision: [{0} - {1}]";
        public static readonly string ConcurenceResultBody = "{0} has {1} the Contracting Plan {2}. For more details, please visit: {3}" + Environment.NewLine + Environment.NewLine + "Note: If the link above doesn't work, please copy and paste the URL into a browser.";

        public static readonly string ConcurenceResultBodyWithComment = "{0} has {1} the report for project {2} - report date {3} ." + Environment.NewLine + "Approver's Comments: {4}" + Environment.NewLine + Environment.NewLine+ "For more details, please visit: {5}" + Environment.NewLine + Environment.NewLine + "Note: If the link above doesn't work, please copy and paste the URL into a browser.";

        // 4-1 Comments
        public static readonly string CommentEmailResultTitle = "Contracting Plan Reviewer Comment: [{0}]-{1}";
        public static readonly string CommentEmailResultBody = "{0} {1} on the Contracting Plan. For more details, please visit: {2}" + Environment.NewLine + Environment.NewLine + "Note: If the link above doesn't work, please copy and paste the URL into a browser.";

        // 4-2 Comments
        public static readonly string AssignAnalystTitle = "Contracting Plan OCR Analyst Assigned: [{0}]-{1}";
        public static readonly string AssignAnalystBody = "{0} has assigned {1} as OCR Analyst for this Contracting Plan. For more details, please visit: {2}" + Environment.NewLine + Environment.NewLine + "Note: If the link above doesn't work, please copy and paste the URL into a browser.";


        // 5- Delete approver
        public static readonly string RemoveApproverEmailTitle = "RR Report Approval Request Withdrawal: {0}";
        public static readonly string RemoveApproverEmailBody = "Requestor has withdrawn the approval request assigned to you ({0})." + Environment.NewLine + "For more details, please visit: {1}" + Environment.NewLine + Environment.NewLine + "Note: If the link above doesn't work, please copy and paste the URL into a browser.";

        //6- Revision 
        public static readonly string RevisionRoutingTitle = "Revised RR Report Submission: [{0}]-{1}";
        public static readonly string RevisionRoutingBody = "This RR Report has been revised and submitted successfully. For more details, please visit: {0}" + Environment.NewLine + Environment.NewLine + "Note: If the link above doesn't work, please copy and paste the URL into a browser.";

        //6- Approved 
        public static readonly string ApprovedSSWPNotificationTitle = "Contracting Plan form Approved: {0}";
        public static readonly string ApprovedSSWPNotificationBody = "This Contracting Plan form has been approved by all reviewers. For more details, please visit: {0}" + Environment.NewLine + Environment.NewLine + "Note: If the link above doesn't work, please copy and paste the URL into a browser.";

        //---7 ---//Notify Compliance Report
        public static readonly string OCRComplianceNotificationTitle = "Contracting Plan OCR Compliance Notification: {0}";
        public static readonly string OCRComplianceNotificationBody = "This contracting plan has Public Works Contracts or Contracts impacted by prevailing wage are included. For more details, please visit: {0}" + Environment.NewLine + Environment.NewLine + "Note: If the link above doesn't work, please copy and paste the URL into a browser.";

        // Email templates
        // 1- Route
        public static readonly string RoutetoApproversEmailTitleAdd = "RR Report Addendum Approval Request: {0}";
        public static readonly string RoutetoApproversEmailBodyAdd = "A RR Report Addendum has been routed to you for approval ({0}). For more details, please visit: {1}" + Environment.NewLine + Environment.NewLine + "Note: If the link above doesn't work, please copy and paste the URL into a browser.";
        public static readonly string RoutetoApproversEmailBodyStandardAdd = "An SSWP Addendum has been routed to you for approval. For more details, please visit: {0}" + Environment.NewLine + Environment.NewLine + "Note: If the link above doesn't work, please copy and paste the URL into a browser.";

        // 2- Notify Requestor
        public static readonly string NotiffyRequestorOfRoutingTitleAdd = "RR Report Addendum Submission:{0}";
        public static readonly string NotiffyRequestorOfRoutingBodyAdd = "Your SSWP Addendum has been submitted successfully. For more details, please visit: {0}" + Environment.NewLine + Environment.NewLine + "Note: If the link above doesn't work, please copy and paste the URL into a browser.";


        // 3- Modification
        public static readonly string NotifyOfModificationTitleAdd = "RR Report Addendum Attachment Update: [{0}]-{1}";
        public static readonly string NotifyOfModificationBodyAdd = "This SSWP Addendum's attachment has been updated. For more details, please visit: {0}" + Environment.NewLine + Environment.NewLine + "Note: If the link above doesn't work, please copy and paste the URL into a browser.";

        // 4- Concurences
        public static readonly string ConcurenceResultTitleAdd = "RR Report Addendum Reviewer Decision: {0}";
        public static readonly string ConcurenceResultBodyAdd = "{0} has {1} with the SSWP Addendum. For more details, please visit: {2}" + Environment.NewLine + Environment.NewLine + "Note: If the link above doesn't work, please copy and paste the URL into a browser.";

        // 4-1 Comments
        public static readonly string CommentEmailResultTitleAdd = "RR Report Addendum Reviewer Comment: {0}";
        public static readonly string CommentEmailResultBodyAdd = "{0} {1} on the SSWP Addendum. For more details, please visit: {2}" + Environment.NewLine + Environment.NewLine + "Note: If the link above doesn't work, please copy and paste the URL into a browser.";


        // 5- Delete approver
        public static readonly string RemoveApproverEmailTitleAdd = "RR Report Addendum Approval Request Withdrawal: {0}";
        public static readonly string RemoveApproverEmailBodyAdd = "Requestor has withdrawn the approval request assigned to you ({0})." + Environment.NewLine + "For more details, please visit: {1}" + Environment.NewLine + Environment.NewLine + "Note: If the link above doesn't work, please copy and paste the URL into a browser.";

        //6- Approved 
        public static readonly string ApprovedSSWPNotificationTitleAdd = "RR Report Addendum Approved: {0}";
        public static readonly string ApprovedSSWPNotificationBodyAdd = "This SSWP Addendum has been approved by all reviewer. For more details, please visit: {0}" + Environment.NewLine + Environment.NewLine + "Note: If the link above doesn't work, please copy and paste the URL into a browser.";

        
        //1- PROXIES EMAIL 
        public static readonly string ProxyAddedTitle = "MDD Proxy Creation";
        public static readonly string ProxyAddedBody = "{0} has added you as a proxy for the following time period: {1} to {2}.";

        //2
        public static readonly string ProxyRemoveTitle = "SSWP Proxy Withdrawal";
        public static readonly string ProxyRemoveBody = "{0} has withdrawn the delegation assigned to you for the following time period: {1} to {2}.";
        //



        //TimerJob
        //1 URL
        //public static string WebAppURL
        //{
        //    get
        //    {
        //        string aURL= SPHelper.ReadSetting("SSWPDataAppURL");
        //        return string.IsNullOrEmpty(aURL) ? "http://spsearch-c01:111" : aURL;
        //    }
        //}

        //UAT ONLY
        public static string WebAppURL
        {
            get
            {
                string aURL = SPHelper.ReadSetting("SSWPDataAppURL");
                return string.IsNullOrEmpty(aURL) ? "http://sharepoint.bart.domain" : aURL;
            }
        }
        
        //public static readonly string WebAppURL = string.IsNullOrEmpty(SPHelper.ReadSetting("SSWPDataAppURL"))? "http://spsearch-c01:111": SPHelper.ReadSetting("SSWPDataAppURL"); // Dev
        //2
        public static readonly string TaskRemiderTitle = "SSWP Approval Reminder: {0}";
        public static readonly string TaskRemiderBody = "This SSWP is due by {0} and waiting for your decision. For more details, please visit: {1}" + Environment.NewLine + Environment.NewLine + "Note: If the link above doesn't work, please copy and paste the URL into a browser.";

        public static readonly string QueryStringJSCSS = "V2019Build11202019v1";

        public static readonly string CurrentVersion = "1.0";

        public static readonly string ExecSumTitle = "Title";
        public static readonly string ExecSumControled = "isControled";
        public static readonly string ExecSumCaution = "isCaution";
        public static readonly string ExecSumCritical = "isCritical";
        public static readonly string ExecSumRemark = "Remark";
        public static readonly string TabQueryString = "ctab";
        

        //public static string QueryStringJSCSS()
        //{
        //    return string.Format("{0}{1}{2}{3}", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), DateTime.Now.Hour.ToString());
        //}

    }

}
