using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BART.SP.OCR.CP.Common
{
    public class ProjectSettings : Settings
    {
        public static readonly int MaxContract = 50;
        public static readonly string SPSiteData = "data";
        public static readonly string SPListMaster = "MasterList";
        public static readonly string SPListRRSchedules = "RRSchedules";
        public static readonly string SPListRR3MonthAhead = "3MonthAhead";
        public static readonly string SPGroupViewAllItems = "CPViewAllRequests";

        public static readonly string SPListProjectContracts = "Contracts";
        public static readonly string SPListChanges = "Changes";

        public static readonly string ActionCodeSave = "S2SPBARTDB";
        public static readonly string ActionCodeRoute = "2R21stApprSPBARTDB";
        public static readonly string ActionCodeReSubmit = "3RReApprBARTSPDB";

        //public static readonly string SPListRRRisks = "RRRisks";
        public static readonly string SPListAttachment = "Attachments_Master";
        public static readonly string SPListAttachmentContract = "Attachments_Contract";
        public static readonly string SPListMasterVersions = "MasterVersions";
        public static readonly string SPListAttachment_Version = "Attachments_Version_Master";
        public static readonly string SPListAttachmentContract_Version = "Attachments_Version_Contract";


        public static readonly string QueryRevise = "ReviseApprovedMaster";
        public static readonly string QueryReviseValue = "approvedversionrevision";

        public static readonly string SPListTasks = "ApprovalTasks";
        public static readonly string SPListTaskMapping = "TaskMapping";
        public static readonly string OverallMapping = "Mapping";

        public static readonly string SPListHistory = "History";
        public static readonly string SPListComment = "Comments";
        public static readonly string SPListError = "RRErrors";

        public static readonly string UserLevelRequestor = "AsRequester";
        public static readonly string UserLevelRequesterProxy = "AsProxy";
        public static readonly string UserLevelAdmin = "AsAdmin";
        public static readonly string UserLevelAppsOwner = "AsAppOwner";
        public static readonly string UserLevelLCUGroup45MCons = "AsLCUGroup45M";
        public static readonly string UserLevelDepartmentView = "asDepartmentView";

        public static readonly string UserLevelOCRAnalyst = "asOCRAnalyst";
        public static readonly string UserLevelOCRAnalystProxy = "asOCRAProxy";

        public static readonly string PageRouter = "Router.aspx";

        public static readonly string StatusDeleted = "DeletedNotDisplay";
        public static readonly string ViewStateObject = "ObjectViewState";
        public static readonly string ViewStateContracts = "DisplayContracts";
        public static readonly string MainObjectQueryString = "reportview";

        public static readonly string ProjectDepartments = "Departments";

        public static readonly string ProjectList = "CPProjects";

        public static readonly string RRApprovalPending = "PendingOnly";

        public static readonly string RRApprovalCompleted = "CompletedOnly";

        public static readonly string ProjectStatusDraft = "Draft";
        public static readonly string ProjectStatusDeleted = "Deleted";
        public static readonly string ProjectStatusUnderReview = "Under Review";
        public static readonly string ProjectStatusOnHold = "On Hold";
        public static readonly string ProjectStatusRejected = "Rejected";
        public static readonly string ProjectStatusCanceled = "Canceled";
        public static readonly string ProjectStatusCompleted = "Completed";
        public static readonly string ProjectStatusApproved = "Approved";

        public static readonly string ApprovalLevelStaff = "Staff";
        public static readonly string ApprovalLevelManagement = "Management";
        public static readonly string ApprovalLevelExecutive = "Executive";

        public static readonly string TaskCodeWaitfor = "Wait_for_Requester";
        public static readonly string TaskCodeWaitforName = "Requestor";

        public static readonly string TaskCodePM = "Project_Manager";
        public static readonly string TaskCodePMName = "Project Manager";

        public static readonly string TaskCodeOCRAnalystAssignment = "OCRAnalyst_Assignment";
        public static readonly string TaskCodeOCRAnalystAssignmentName = "OCR Managers Assign Analyst";

        public static readonly string TaskCodeGroupMgr = "Group_Manager";
        public static readonly string TaskCodeGroupMgrName = "Project Group Manager";

        public static readonly string TaskCodeOCRA = "OCR_Analyst";
        public static readonly string TaskCodeOCRAName = "OCR Analyst";

        public static readonly string TaskCodeOCRP1 = "OCR_Program_Manager_1";
        public static readonly string TaskCodeOCRP1Name = "OCR Program Manager I";

        public static readonly string TaskCodeOCRP2 = "OCR_Program_Manager_2";
        public static readonly string TaskCodeOCRP2Name = "OCR Program Manager II";

        public static readonly string TaskCodeMgrContract = "Manager_of_Contract";
        public static readonly string TaskCodeMgrContractName = "Manager of Contract Administration";

        public static readonly string TaskCodeProcumentChief = "Chief_Procurement_Officer";
        public static readonly string TaskCodeProcumentChiefName = "Director of Procurement";

        public static readonly string TaskCodeDeptChief = "Sponsoring_Department_Chief";
        public static readonly string TaskCodeDeptChiefName = "Department Chief /Director";

        public static readonly string TaskCodeDeptAGM = "Sponsoring_Department_AGM";
        public static readonly string TaskCodeDeptAGMName = "Executive Office Sponsor";

        public static readonly string TaskCodeOCRChief = "OCR_Chief";
        public static readonly string TaskCodeOCRChiefName = "OCR Department Manager";

        public static readonly string TaskCodeProcurementChief = "Procurement_AGM";
        public static readonly string TaskCodeProcurementChiefName = "Procurement AGM";

        public static readonly DateTime DateForV2 = new DateTime(2022, 3, 17);//PROD //new DateTime(2022,3,26)

        public static readonly string GroupUserOCRP1 = "OCR Program_Manager I";
        public static readonly string GroupUserOCR_Routers = "OCR Program Managers";

        public static readonly string TaskStatusNone = "Not Started";
        public static readonly string TaskStatusApproved = "Approved";
        public static readonly string TaskStatusRejected = "Rejected";
        public static readonly string TaskStatusWaitforInfo = "Waiting for more info";
        public static readonly string TaskStatusWaitforRequester = "Waiting for requester";
        public static readonly string TaskStatusPending = "Pending";
        public static readonly string TaskStatusRouted = "Routed";
        public static readonly string TaskStatusConcur = "Concurred";
        public static readonly string TaskStatusWriteComment = "WriteAComment";
        public static readonly string TaskStatusAbstain = "Abstained";
        public static readonly string TaskStatusCompleted = "Completed";

        public static readonly string Step_Staffs = "Staffs";
        public static readonly string Step_Managers = "Managers";
        public static readonly string Step_Executives = "Executives";

        public static readonly string HisAssignedOCRA = "OCR Analyst assigned";

        public static readonly string TaskHistoryTextNonConcur = "Requested a Revision";
        public static readonly string TaskHistoryWriteAComment = "Wrote a Comment";


        public static readonly string DeptManagerAppName = "Sponsoring Department Mgr/Designee Approval";
        public static readonly string DeptManagerAppCode = "DeptManagerApproval";

        public static readonly string ProcurementReviewName = "Procurement Review";
        public static readonly string ProcurementReviewCode = "ProcurementReview";
        public static readonly string ExecutionTypeChangeOrder = "Change Order";

        public static readonly string MarkIfRevised = "RevisedCopy";

        public static readonly string ErrorMessageFundingErrors = "<span style=\"color:#b31c1c;\">Funding is not available</span>";
        public static readonly string ErrorMessagePieChartErrors = "<span style=\"color:#b31c1c; text-align:center;\">Funding Sources <br> is not available</span>";
        public static readonly string ErrorMessageOtherErrors = "<span style=\"color:#b31c1c;\">Data is not available.</span>";

        // Pages
        public static readonly string PageHome = "MyCP.aspx";
        public static readonly string PageAll = "AllCP.aspx";
        public static readonly string PageContractExport = "ContractExport.aspx";
        public static readonly string PageCreateNew = "NewCP.aspx";
        public static readonly string PageCreateNewPrefix = "/step";
        public static readonly string PageEdit = "EditCP.aspx";
        public static readonly string PageView = "ViewCP.aspx";
        public static readonly string PageApprovals = "ApprovalTasks.aspx";
        public static readonly string PageApprovalForm = "TaskForm.aspx";
        public static readonly string PageViewOnly = "ViewReport.aspx";
        public static readonly string PageProxies = "Proxies.aspx";
        public static readonly string PagePrint = "PrintView.aspx";
        public static readonly string PageExportPDF = "RevisionProvisioning.aspx";
        public static readonly string PageHelp = "Help.aspx";

        public static readonly DateTime CB5MChangeDate = new DateTime(2022, 7, 22);



        public static readonly string S_Construction = "Construction";
        public static readonly string S_Design_Build = "Design Build";
        public static readonly string S_IFB = "IFB";
        public static readonly string S_NASPO = "NASPO";
        public static readonly string S_Procurement = "Procurement";
        public static readonly string S_Service_Agreement = "Service Agreement";
    }
}
