using BART.SP.OCR.CP.Common;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telerik.Web.UI;

namespace BART.SP.OCR.CP.Model
{
    [Serializable]
    public class MainObject
    {
        public string Title { get; set; }
        public string MasterID { get; set; }//Single line of text//
        public string ProgramName { get; set; }//Single line of text//

        public string ProgramDes { get; set; }//Single line of text//

        public string ProjectName { get; set; }//Single line of text//
        public string ServiceType { get; set; }//Choice//
        public string ProjectID { get; set; }//Single line of text//
        public string BusinessUnit { get; set; }//Single line of text//
        public string SponsorDepartment { get; set; }//Single line of text//
        public string Status { get; set; }//Choice//
        public string ProjectJustification { get; set; }//Multiple line of text//
        public string ProjectScopeOfWork { get; set; }//Multiple line of text//

        public string ByDollarAmountAnalysis { get; set; }//Multiple line of text//
        public string ByScopeOfWorkAnalysis { get; set; }//Multiple line of text//
        public string ByLocationAnalysis { get; set; }//Multiple line of text//
        public string ByBARTSEIUAnalysis { get; set; }//Multiple line of text//
        public string ByScheduleAnalysis { get; set; }//Multiple line of text//
        public string OCRCCUAnalysisSummary { get; set; }//Multiple line of text//
        public string OCRLCUAnalysisSummary { get; set; }//Multiple line of text//
        public string VersioningHistory { get; set; }//Multiple line of text//
        public string OriginalMasterID { get; set; }//Single line of text//
        public string CurrentVersion { get; set; }//Single line of text//
        public string ApprovalStep { get; set; }//Choice//


        public bool UnbundlingBySchedule { get; set; }//Yes/No//
        public bool UnbundlingByLocation { get; set; }//Yes/No//
        public bool UnbundlingByBARTSEIU { get; set; }//Yes/No//
        public bool UnbundlingByMultipleScopesOfWork { get; set; }//Yes/No//
        public bool UnbundlingByContractSeparated { get; set; }//Yes/No//
        public bool UnbundlingBySmallerProjects { get; set; }//Yes/No//
        public bool UnbundlingByDollarAmount { get; set; }//Yes/No//
        public bool WaitForRequestor { get; set; }//Yes/No//
        public bool OCROver10M { get; set; }//Yes/No//

        public DateTime? DateSubmitted { get; set; }//Date/Time//
        public DateTime? DateCreated { get; set; }//Date/Time//
        public DateTime? KickoffMeetingDate { get; set; }//Date/Time//
        public DateTime? ApprovedDate { get; set; }//Date/Time//
        public string DepartmentChiefName { get; set; }//Single line of text//
        public string DepartmentAGMName { get; set; }//Single line of text//
        public string RequesterName { get; set; }//Single line of text//
        public string GroupManagerName { get; set; }//Single line of text//
        public string OCRCCUAnalysisName { get; set; }//Single line of text//
        public string OCRLCUAnalysisName { get; set; }//Single line of text//
        public string OCRAnalystName { get; set; }//Single line of text//
        public string CurrentStep { get; set; }//Single line of text//
        public string ApprovalVersion { get; set; }//Single line of text//
        public string SponsorProjectManagerName { get; set; }//Single line of text//
        public string sDepartmentCode { get; set; }

        public SPUser Requester { get; set; }
        public SPUser SponsorProjectManager { get; set; }
        public SPUser GroupManager { get; set; }
        public SPUser DepartmentChief { get; set; }
        public SPUser DepartmentAGM { get; set; }
        public SPUser OCRAnalyst { get; set; }

        public SPUser UserCreated { get; set; }
        public SPUser PendingAt { get; set; }
        public SPUser UserModified { get; set; }
        public SPUser OCRCCUAnalysis { get; set; }
        public SPUser OCRLCUAnalysis { get; set; }

        public SPUser Requester_Assigned { get; set; }

        public SPUser OCRAnalyst_Assigned { get; set; }


        // List properties
        public ContractList Contracts { get; set; }
        public List<ContractDisplay> ContractDisplays { get; set; }
        public AttachmentList ListAttachments { get; set; }
        public List<TaskItemObject> Tasks { get; set; }

        public MainObject()
        {
            //
        }
        public MainObject(SPWeb dWeb,bool defaultInitial)
        {
            ProjectUtilities.InitDefaultValByType(this, typeof(string),string.Empty);
            this.Status = ProjectSettings.ProjectStatusDraft;
            // ---- LIST
            this.Contracts = new ContractList();
            this.ContractDisplays = new List<ContractDisplay>();
            this.ListAttachments = new AttachmentList();
            this.Tasks = new List<TaskItemObject>();
            
            if (defaultInitial)
            {
                this.Contracts.ListObjects = this.GetInitContracts();
                foreach (Contract c in this.Contracts.ListObjects)
                {
                    this.ContractDisplays.Add(new ContractDisplay(c));
                }
                this.getTaskDefaultList(dWeb);
            }
        }
        public MainObject(SPListItem item)
        {
            InitObject(item.Web, item);
        }
        public MainObject(SPWeb dWeb, string rrReportId,bool complete=false)
        {
            SPListItem item = getMasterReportItemByReportId(dWeb, rrReportId);
            InitObject(dWeb, item,complete);
        }
        private void InitObject(SPWeb dWeb, SPListItem oListItem, bool completedata=false)
        {
            this.ContractDisplays = new List<ContractDisplay>();
            ProjectUtilities.InitObjectDynamic(this,dWeb, oListItem);

            this.DepartmentChiefName = (this.DepartmentChief != null) ? this.DepartmentChief.Name : string.Empty;
            this.DepartmentAGMName = (this.DepartmentAGM != null) ? this.DepartmentAGM.Name : string.Empty;
            this.RequesterName = (this.Requester != null) ? this.Requester.Name : string.Empty;
            this.GroupManagerName = (this.GroupManager != null) ? this.GroupManager.Name : string.Empty;
            this.OCRCCUAnalysisName = (this.OCRCCUAnalysis != null) ? this.OCRCCUAnalysis.Name : string.Empty;
            this.OCRLCUAnalysisName = (this.OCRLCUAnalysis != null) ? this.OCRLCUAnalysis.Name : string.Empty;
            this.OCRAnalystName = (this.OCRAnalyst != null) ? this.OCRAnalyst.Name : string.Empty;
            this.SponsorProjectManagerName = (this.SponsorProjectManager != null) ? this.SponsorProjectManager.Name : string.Empty;
            
            this.Contracts = new ContractList(dWeb, this.MasterID);
            //-------------------------------------------------------------
            foreach (Contract c in this.Contracts.ListObjects)
            {
                this.ContractDisplays.Add(new ContractDisplay(c));
            }
            //-------------------------------------------------------------

            if (completedata)
            {
                this.ListAttachments = new AttachmentList(dWeb, this.MasterID);
                this.GetTasksList4Display(dWeb);
            }
        }
        public void New(SPWeb dWeb, string status="",UploadedFileCollection files=null)
        {
            try
            {
                //-------------------------------
                this.Requester_Assigned = this.Requester;
                this.DepartmentChiefName = (this.DepartmentChief != null) ? this.DepartmentChief.Name : string.Empty;
                this.DepartmentAGMName = (this.DepartmentAGM != null) ? this.DepartmentAGM.Name : string.Empty;
                this.RequesterName = (this.Requester != null) ? this.Requester.Name : string.Empty;
                this.GroupManagerName = (this.GroupManager != null) ? this.GroupManager.Name : string.Empty;
                this.OCRCCUAnalysisName = (this.OCRCCUAnalysis != null) ? this.OCRCCUAnalysis.Name : string.Empty;
                this.OCRLCUAnalysisName = (this.OCRLCUAnalysis != null) ? this.OCRLCUAnalysis.Name : string.Empty;
                this.OCRAnalystName = (this.OCRAnalyst != null) ? this.OCRAnalyst.Name : string.Empty;
                this.SponsorProjectManagerName = (this.SponsorProjectManager != null) ? this.SponsorProjectManager.Name : string.Empty;
                this.CurrentStep = ProjectSettings.Step_Staffs;
                this.DateCreated = DateTime.Now;
                //------------------------------------------------------------------
                SPList list = dWeb.Lists[Common.ProjectSettings.SPListMaster];
                SPListItem masterItem = list.Items.Add();
                masterItem.SystemUpdate();

                //---------------------------------------------------------------------------------
                DateTime d = DateTime.Now;
                this.MasterID = string.Format("{5}idy{0}m{1}d{2}t{3}{4}", d.Year.ToString(), d.Month.ToString(), d.Day.ToString(), d.Hour.ToString(), d.Minute.ToString(), Convert.ToString(masterItem.ID));
                this.UpdateFieldsItemOnly(masterItem);
                //---- 

                //
                this.Contracts.MasterID = this.MasterID;
                this.ListAttachments.MasterID = this.MasterID;
                this.Contracts.CreateAll(dWeb);
                if(files!=null)
                    this.ListAttachments.CreateAll(dWeb, files);
                 //Create Not Started Task
                //this.CreateApprovalTasks(masterItem.Web, masterItem);
                //-------------------------------------------------------------------------//
            }
            catch (Exception ex)
            {
                ProjectUtilities.LogError(ex.ToString());
            }
        }

        private void UpdateFieldsItemOnly(SPListItem masterItem)
        {
            if (!string.IsNullOrEmpty(this.MasterID))
            {
                masterItem["MasterID"] = this.MasterID;
                // -- Update all lists
                this.Contracts.MasterID = this.MasterID;
                this.ListAttachments.MasterID = this.MasterID;
            }
            ProjectUtilities.UpdateItemDynamic(this, masterItem);

            if (this.Contracts != null && this.Contracts.ListObjects != null && this.Contracts.ListObjects.Count > 0)
                this.Contracts.UpdateAll(masterItem.Web);

            masterItem.Update();
        }

        public void Update(SPWeb dWeb, string status = "", bool historyLog=false)
        {
            SPListItem oListItem = this.getCurrentReportItem(dWeb);
            this.OCRAnalyst_Assigned = this.OCRAnalyst;
            //---------------------------------------------------------
            this.UpdateFieldsItemOnly(oListItem);
            if (this.Status != ProjectSettings.ProjectStatusDraft && historyLog)
                ProjectHelper.AddHistory(dWeb, this, Settings.HistoryActionModified, Settings.HistoryActionModified, dWeb.CurrentUser.LoginName, ProjectUtilities.GetCurrentDateTimeShortFormat(), this.Requester.LoginName);
        }
        public void UpdateStatusOnly(SPWeb dWeb, string status="", string currentStep="")
        {
            SPListItem oListItem = this.getCurrentReportItem(dWeb);
            if (!string.IsNullOrEmpty(status.Trim()))
            {
                if (status == ProjectSettings.ProjectStatusApproved)
                {
                    int cVer = 0;
                    if (int.TryParse(this.ApprovalVersion, out cVer))
                        cVer += 1;
                    this.ApprovalVersion = cVer.ToString();
                    oListItem["ApprovalVersion"] = this.ApprovalVersion;
                    oListItem["ApprovedDate"] = DateTime.Now;
                }
                //
                oListItem["Status"] = status;
            }
            if(!string.IsNullOrEmpty(currentStep.Trim()))
                oListItem["CurrentStep"] = currentStep;
            oListItem.SystemUpdate();
        }
        public void UpdateOCRAnalystOnly(SPWeb dWeb)
        {
            SPListItem oListItem = this.getCurrentReportItem(dWeb);
            oListItem["OCRAnalyst"] = this.OCRAnalyst;
            oListItem["OCRAnalyst_Assigned"] = this.OCRAnalyst_Assigned;
            oListItem.SystemUpdate();
        }
        public bool GetTasksList4Display(SPWeb dWeb)
        {
            this.Tasks = new List<TaskItemObject>();
            SPListItemCollection tasks = ProjectHelper.GetAllMasterItemTasksAssigned(dWeb, this.MasterID);
            if (tasks != null)
            {
                foreach (SPListItem t in tasks)
                {
                    this.Tasks.Add(new TaskItemObject(t));
                }
                return true;
            }
            //
            else
                this.getTaskDefaultList(dWeb);
            return false;

        }
       
        public void Route(SPWeb dWeb)
        {
            // Delete all old tasks
            bool hasCurrentTask = GetTasksList4Display(dWeb);
            if (hasCurrentTask)
            {
                foreach (TaskItemObject t in this.Tasks)
                {
                    t.DeleteTaskItem(dWeb);
                }
            }
            // Create new Tasks
            this.CreateApprovalTasksObjects(dWeb);
            if (this.Tasks.Count > 0)
                this.Tasks[0].TaskStatus = ProjectSettings.TaskStatusRouted;
            foreach (TaskItemObject t in this.Tasks)
            {
                t.CreateApprovalTaskItem(dWeb);
                if (t.TaskStatus == ProjectSettings.TaskStatusRouted)
                    t.EmailRouted(dWeb,string.Format("{0}-{1}",this.ProjectName, this.ProjectID),this.Requester.LoginName);
            }
            //
        }
        //-----------------------------------------------------------
        public void CreateApprovalTasksObjects (SPWeb dWeb, int fromOrder=1, int toOrder=100)
        {
            // Tasks for Department Manager

            List<TaskMapping> mappings = ProjectHelper.GetAllTaskMapping(dWeb);
            //
            TaskItemObject task1 = new TaskItemObject();
            TaskMapping m1 = ProjectHelper.GetTaskMappingByCode(mappings, ProjectSettings.TaskCodePM);
            task1.Title = m1.DisplayLabel;
            task1.MasterID = this.MasterID;
            task1.ApprovalOrder = 1;
            task1.ApprovalLevel =ProjectSettings.ApprovalLevelStaff;
            if (this.SponsorProjectManager != null)
                task1.AssignedTo = this.SponsorProjectManager;
            task1.ApprovalTypeCode =m1.Code;
            task1.ApprovalRole = m1.DisplayLabel;
            //--------------------------------------------------

            TaskItemObject task2 = new TaskItemObject();
            TaskMapping m2 = ProjectHelper.GetTaskMappingByCode(mappings, ProjectSettings.TaskCodeOCRAnalystAssignment);
            task2.Title = m2.DisplayLabel;
            task2.MasterID = this.MasterID;
            task2.ApprovalOrder = 2;
            task2.ApprovalLevel = ProjectSettings.ApprovalLevelStaff;
            task2.AssignedTo = SPHelper.GetSPUserFromLoginName(dWeb, m2.Val);
            task2.ApprovalTypeCode = m2.Code;
            task2.ApprovalRole = m2.DisplayLabel;

            //-----------------------------------------------------------------
            TaskItemObject task3 = new TaskItemObject();
            TaskMapping m3 = ProjectHelper.GetTaskMappingByCode(mappings, ProjectSettings.TaskCodeOCRA);
            task3.Title = m3.DisplayLabel;
            task3.MasterID = this.MasterID;
            task3.ApprovalOrder = 3;
            task3.ApprovalLevel = ProjectSettings.ApprovalLevelStaff;
            task3.AssignedTo = SPHelper.GetSPUserFromLoginName(dWeb, m3.Val);
            if (this.OCRAnalyst != null)
                task3.AssignedTo = this.OCRAnalyst;
            task3.ApprovalTypeCode = m3.Code;
            task3.ApprovalRole = m3.DisplayLabel;
            //-----------------------------------------------------------------
            TaskItemObject task4 = new TaskItemObject();
            TaskMapping m4 = ProjectHelper.GetTaskMappingByCode(mappings, ProjectSettings.TaskCodeGroupMgr);
            task4.Title = m4.DisplayLabel;
            task4.MasterID = this.MasterID;
            task4.ApprovalOrder = 4;
            task4.ApprovalLevel = ProjectSettings.ApprovalLevelManagement;
            if (this.GroupManager != null)
                task4.AssignedTo = this.GroupManager;
            task4.ApprovalTypeCode = m4.Code;
            task4.ApprovalRole = m4.DisplayLabel;
            //-----------------------------------------------------------------
            TaskItemObject task5 = new TaskItemObject();
            TaskMapping m5 = ProjectHelper.GetTaskMappingByCode(mappings, ProjectSettings.TaskCodeOCRP1);
            task5.Title = m5.DisplayLabel;
            task5.MasterID = this.MasterID;
            task5.ApprovalOrder = 5;
            task5.ApprovalLevel = ProjectSettings.ApprovalLevelManagement;
            task5.AssignedTo = SPHelper.GetSPUserFromLoginName(dWeb, m5.Val);
            task5.ApprovalTypeCode = m5.Code;
            task5.ApprovalRole = m5.DisplayLabel;

            //-----------------------------------------------------------------
            TaskItemObject task6 = new TaskItemObject();
            TaskMapping m6 = ProjectHelper.GetTaskMappingByCode(mappings, ProjectSettings.TaskCodeOCRP2);
            task6.Title = m6.DisplayLabel;
            task6.MasterID = this.MasterID;
            task6.ApprovalOrder = 6;
            task6.ApprovalLevel = ProjectSettings.ApprovalLevelManagement;
            task6.AssignedTo = SPHelper.GetSPUserFromLoginName(dWeb, m6.Val);
            task6.ApprovalTypeCode = m6.Code;
            task6.ApprovalRole = m6.DisplayLabel;
            //-----------------------------------------------------------------
            TaskItemObject task7 = new TaskItemObject();
            TaskMapping m7 = ProjectHelper.GetTaskMappingByCode(mappings, ProjectSettings.TaskCodeMgrContract);
            task7.Title = m7.DisplayLabel;
            task7.MasterID = this.MasterID;
            task7.ApprovalOrder = 7;
            task7.ApprovalLevel = ProjectSettings.ApprovalLevelManagement;
            task7.AssignedTo = SPHelper.GetSPUserFromLoginName(dWeb, m7.Val);
            task7.ApprovalTypeCode = m7.Code;
            task7.ApprovalRole = m7.DisplayLabel;
            //-----------------------------------------------------------------
            TaskItemObject task8 = new TaskItemObject();
            TaskMapping m8 = ProjectHelper.GetTaskMappingByCode(mappings, ProjectSettings.TaskCodeProcumentChief);
            task8.Title = m8.DisplayLabel;
            task8.MasterID = this.MasterID;
            task8.ApprovalOrder = 8;
            task8.ApprovalLevel = ProjectSettings.ApprovalLevelManagement;
            task8.AssignedTo = SPHelper.GetSPUserFromLoginName(dWeb, m8.Val);
            task8.ApprovalTypeCode = m8.Code;
            task8.ApprovalRole = m8.DisplayLabel;
            //-----------------------------------------------------------------
            TaskItemObject task9 = new TaskItemObject();
            TaskMapping m9 = ProjectHelper.GetTaskMappingByCode(mappings, ProjectSettings.TaskCodeDeptChief);
            task9.Title = m9.DisplayLabel;
            task9.MasterID = this.MasterID;
            task9.ApprovalOrder = 9;
            task9.ApprovalLevel = ProjectSettings.ApprovalLevelManagement;
            if (this.DepartmentChief != null)
                task9.AssignedTo = this.DepartmentChief;
            task9.ApprovalTypeCode = m9.Code;
            task9.ApprovalRole = m9.DisplayLabel;
            //-----------------------------------------------------------------
            TaskItemObject task10 = new TaskItemObject();
            TaskMapping m10 = ProjectHelper.GetTaskMappingByCode(mappings, ProjectSettings.TaskCodeDeptAGM);
            task10.Title = m10.DisplayLabel;
            task10.MasterID = this.MasterID;
            task10.ApprovalOrder = 10;
            task10.ApprovalLevel = ProjectSettings.ApprovalLevelExecutive;
            if (this.DepartmentAGM != null)
                task10.AssignedTo = this.DepartmentAGM;
            task10.ApprovalTypeCode = m10.Code;
            task10.ApprovalRole = m10.DisplayLabel;
            //------------------------------------------------------------------
            //03/09/2022 ---------------------------------------------------- add new procurement AGM
            TaskItemObject task11 = new TaskItemObject();
            TaskMapping m11 = ProjectHelper.GetTaskMappingByCode(mappings, ProjectSettings.TaskCodeProcurementChief);
            task11.Title = m11.DisplayLabel;
            task11.MasterID = this.MasterID;
            task11.ApprovalOrder = 11;
            task11.ApprovalLevel = ProjectSettings.ApprovalLevelExecutive;
            task11.AssignedTo = SPHelper.GetSPUserFromLoginName(dWeb, m11.Val);
            task11.ApprovalTypeCode = m11.Code;
            task11.ApprovalRole = m11.DisplayLabel;
            //-----------------------------------------------------------------
            TaskItemObject task12 = new TaskItemObject();
            TaskMapping m12 = ProjectHelper.GetTaskMappingByCode(mappings, ProjectSettings.TaskCodeOCRChief);
            task12.Title = m12.DisplayLabel;
            task12.MasterID = this.MasterID;
            task12.ApprovalOrder = 12;
            task12.ApprovalLevel = ProjectSettings.ApprovalLevelExecutive;
            task12.AssignedTo = SPHelper.GetSPUserFromLoginName(dWeb, m12.Val);
            task12.ApprovalTypeCode = m12.Code;
            task12.ApprovalRole = m12.DisplayLabel;
            //-----------------------------------------------------------------

            //change Oct 31 2023 due to a new parameter - Re-route fromOrder to toOrder
            List<TaskItemObject> lTasks = new List<TaskItemObject>();
            lTasks.Add(task1); lTasks.Add(task2);
            lTasks.Add(task3); lTasks.Add(task4);
            lTasks.Add(task5); lTasks.Add(task6);
            lTasks.Add(task7); lTasks.Add(task8);
            lTasks.Add(task9); lTasks.Add(task10);
            //Only for CPs that created after V2 deployment
            try
            {
                DateTime v2Date = new DateTime();
                if (DateTime.TryParse(Common.ProjectSettings.Ver2Date, out v2Date) && this.DateCreated != null)
                {
                    if(this.DateCreated>=v2Date)
                        lTasks.Add(task11);
                }
            }
            catch (Exception ex)
            {
                //
            }

            lTasks.Add(task12);



            //change Oct 31 2023 due to a new parameter - Re-route fromOrder to toOrder
            this.Tasks = new List<TaskItemObject>();

            foreach (TaskItemObject t in lTasks)
            {
                if (t.ApprovalOrder >= fromOrder && t.ApprovalOrder <= toOrder)
                {
                    t.TaskStatus = ProjectSettings.TaskStatusNone;
                    t.ApprovedDate = DateTime.Now;
                    t.DateCreated = DateTime.Now;
                    if (t.AssignedTo != null)
                    {
                        t.AssignedToName = t.AssignedTo.Name;
                    }
                    //
                    this.Tasks.Add(t);
                }
            }

           /* foreach (TaskItemObject t in this.Tasks)
            {
                if (t.ApprovalOrder >= fromOrder && t.ApprovalOrder <= toOrder)
                {
                    t.TaskStatus = ProjectSettings.TaskStatusNone;
                    t.ApprovedDate = DateTime.Now;
                    t.DateCreated = DateTime.Now;
                    if (t.AssignedTo != null)
                    {
                        t.AssignedToName = t.AssignedTo.Name;
                    }
                }
            }*/
        }
        public void UpdateTasksObjects(SPWeb dWeb)
        {
            
        }

        public bool ArchiveAllApprovedDocuments(SPWeb dWeb, SPWeb arvWeb)
        {
            // Step1 Move all Master Docs
            bool moveCompleted = false;
            bool moveMaster = false;
            bool moveContract = false;
            try
            {
                try {
                    if(!arvWeb.Site.AllowUnsafeUpdates)
                        arvWeb.Site.AllowUnsafeUpdates = true;
                    if(!arvWeb.AllowUnsafeUpdates)
                        arvWeb.AllowUnsafeUpdates = true;
                } catch { }
                //---------------------------------------------------------------
                this.ApprovalVersion = (string.IsNullOrEmpty(this.ApprovalVersion) || this.ApprovalVersion=="0") ? "1" : this.ApprovalVersion;

                SPList mVersionLib = arvWeb.Lists[ProjectSettings.SPListAttachment_Version];
                SPFolder master = null;
                try {
                    master=mVersionLib.RootFolder.SubFolders[this.MasterID.Trim()];
                } catch { }
                if (master==null)
                {
                    moveMaster = true;
                    master = mVersionLib.RootFolder.SubFolders.Add(this.MasterID);
                }
                //
                SPFolder mVersion = null;
                try { mVersion = master.SubFolders[this.ApprovalVersion]; } catch { }
                if (mVersion==null)
                {
                    moveMaster = true;
                    mVersion = master.SubFolders.Add(this.ApprovalVersion);
                }
                //
                if (moveMaster)
                {
                    SPListItemCollection mDocs = this.ListAttachments.GetAllAttachmentsFiles(dWeb, this.MasterID);
                    foreach (SPListItem item in mDocs)
                    {
                        byte[] fileBytes = item.File.OpenBinary();
                        SPFile fileAdded = mVersion.Files.Add(item.File.Name, fileBytes);
                        fileAdded.Item.Properties["MasterID"] = this.MasterID;
                        fileAdded.Item.Properties["ApprovalVersion"] = this.ApprovalVersion;
                        fileAdded.Item.Properties["Title"] = string.Format("{0}-{1}", this.ProjectID, this.ProjectName);
                        fileAdded.Item.SystemUpdate();
                    }
                }
                // Move Contract Files
                SPList cVersionLib = arvWeb.Lists[ProjectSettings.SPListAttachmentContract_Version];
                SPFolder cmaster = null;
                try
                {
                    cmaster = cVersionLib.RootFolder.SubFolders[this.MasterID.Trim()];
                }
                catch { }
                if (cmaster==null)
                {
                    moveContract = true;
                    cmaster = cVersionLib.RootFolder.SubFolders.Add(this.MasterID);
                }
                //
                SPFolder cVersion = null;
                try { cVersion = cmaster.SubFolders[this.ApprovalVersion]; } catch { }
                if (cVersion==null)
                {
                    moveContract = true;
                    cVersion = cmaster.SubFolders.Add(this.ApprovalVersion);
                }
                //
                if (moveContract)
                {
                    SPListItemCollection cDocs = this.Contracts.GetAllAttachmentsFiles(dWeb, this.MasterID);
                    foreach (SPListItem item in cDocs)
                    {
                        byte[] fileBytes = item.File.OpenBinary();
                        SPFile fileAdded = cVersion.Files.Add(item.File.Name, fileBytes);
                        fileAdded.Item.Properties["MasterID"] = this.MasterID;
                        fileAdded.Item.Properties["ApprovalVersion"] = this.ApprovalVersion;
                        fileAdded.Item.Properties["Title"] = string.Format("{0}-{1}", this.ProjectID, this.ProjectName);
                        fileAdded.Item.Properties["ParentLevel2ID"] = Convert.ToString(item["ParentLevel2ID"]);
                        fileAdded.Item.SystemUpdate();
                    }
                }
                return true;
            }
            catch { }
            //
            return false;
        }

        public void CreateNewAndRoute(SPWeb dWeb, string status="", UploadedFileCollection files = null)
        {
            this.DateSubmitted = DateTime.Now;
            this.New(dWeb, status,files);
            this.Route(dWeb);
            ProjectHelper.AddHistory(dWeb, this, Settings.HistoryTypeRouted, Settings.HistoryActionRouted, dWeb.CurrentUser.LoginName, ProjectUtilities.GetCurrentDateTimeShortFormat(), this.Requester.LoginName);
        }
        public void UpdateAndRoute(SPWeb dWeb)
        {
            this.DateSubmitted = DateTime.Now;
            this.Update(dWeb);
            this.Route(dWeb);
            ProjectHelper.AddHistory(dWeb, this, Settings.HistoryTypeRouted, Settings.HistoryActionRouted, dWeb.CurrentUser.LoginName, ProjectUtilities.GetCurrentDateTimeShortFormat(), this.Requester.LoginName);
        }

        // From Step allows user to flexibaly re-route from a certain order - default with 1 - 1 order is first step
        public void UpdateAndRe_Route(SPWeb dWeb, int fromOrder=1, int toOrder=100)
        {
            this.DateSubmitted = DateTime.Now;
            //Keep the same OCR Analyst if re-route.
            //this.OCRAnalyst = null;
            this.Update(dWeb);
            bool hasCurrentTask = GetTasksList4Display(dWeb);
            if (hasCurrentTask)
            {
                foreach (TaskItemObject t in this.Tasks)
                {
                    // Only delete task that after fromOrder
                    if(t.ApprovalOrder>=fromOrder && t.ApprovalOrder<=toOrder)
                        t.DeleteTaskItem(dWeb);
                }
            }
            // Create New Tasks for after fromOrder until toOrder
            this.CreateApprovalTasksObjects(dWeb, fromOrder,toOrder);
            //
            foreach (TaskItemObject t in this.Tasks)
            {
                if (t.ApprovalOrder == fromOrder)
                    t.TaskStatus = ProjectSettings.TaskStatusRouted;
            }
            // Deleted 10/25/2023
            //if (this.Tasks.Count > 0)
            //    this.Tasks[0].TaskStatus = ProjectSettings.TaskStatusRouted;
            //
            foreach (TaskItemObject t in this.Tasks)
            {
                t.CreateApprovalTaskItem(dWeb);
                if (t.TaskStatus == ProjectSettings.TaskStatusRouted)
                    t.EmailRouted(dWeb, string.Format("{0}-{1}", this.ProjectName, this.ProjectID), this.Requester.LoginName);
            }
            //
            ProjectHelper.AddHistory(dWeb, this, Settings.HistoryTypeRouted, Settings.HistoryActionReRouted, dWeb.CurrentUser.LoginName, ProjectUtilities.GetCurrentDateTimeShortFormat(), this.Requester.LoginName);
        }

        // Re-route after full approval


        public bool Delete(SPWeb dWeb)
        {
            try
            {
                if (this.Contracts != null && this.Contracts.ListObjects != null && this.Contracts.ListObjects.Count > 0)
                {
                    this.Contracts.DeleteAll(dWeb);
                }
                if (this.ListAttachments != null && this.ListAttachments.ListObjects != null && this.ListAttachments.ListObjects.Count > 0)
                {
                    this.ListAttachments.DeleteAll(dWeb);
                }

                SPListItemCollection Taskitems=ProjectHelper.GetAllMasterItemTasksAssigned(dWeb,this.MasterID);
                if (Taskitems != null && Taskitems.Count > 0)
                {
                    int totalItems = Taskitems.Count;
                    for (int k = 0; k < totalItems; k++)
                    {
                        Taskitems.Delete(0);
                    }
                }
                SPListItem oListItem = this.getCurrentReportItem(dWeb);
                //
                oListItem.Recycle();

                return true;
            }
            catch (Exception ex)
            {
                ProjectUtilities.LogError(ex.ToString());
            }
            return false;
        }
        
        public void AddFilestoSSWP(UploadedFileCollection files, SPWeb dWeb, ref string mss)
        {
            Common.ProjectHelper.AddDocumentsToReport(files, this.MasterID, dWeb, ref mss);
        }

        public SPListItem getCurrentReportItem(SPWeb dWeb)
        {
            return this.getMasterReportItemByReportId(dWeb,this.MasterID);

        }
        public SPListItem getMasterReportItemByReportId(SPWeb dWeb, string reportId)
        {
            try
            {
                SPListItemCollection items = null;
                SPList list = dWeb.Lists[Common.ProjectSettings.SPListMaster];
                StringBuilder sb = new StringBuilder();
                sb.Append("<Where><And><Eq><FieldRef Name = 'MasterID' /><Value Type = 'Text'>" + reportId + "</Value></Eq>");
                sb.Append("<Neq><FieldRef Name='Status'/><Value Type='Text'>" + ProjectSettings.StatusDeleted + "</Value></Neq>");
                sb.Append("</And></Where>");
                SPQuery query = new SPQuery();
                query.Query = sb.ToString();
                // Get data from a list.
                items = list.GetItems(query);

                if (items != null && items.Count > 0)
                {
                    return items[0];
                }
            }
            catch (Exception ex)
            {
                ProjectUtilities.LogError(ex.ToString());
            }
            return null;

        }

        protected List<Contract> GetInitContracts()
        {
            List<Contract> contracts = new List<Contract>();
            contracts.Add(new Contract { OrderInTable = 1});
            return contracts;
        }
        protected void getTaskDefaultList(SPWeb dWeb)
        {
            List<TaskMapping> mappings = ProjectHelper.GetAllTaskMapping(dWeb);
            //
            TaskItemObject task1 = new TaskItemObject();
            TaskMapping m1 = ProjectHelper.GetTaskMappingByCode(mappings, ProjectSettings.TaskCodePM);
            task1.Title = m1.DisplayLabel;
            task1.MasterID = this.MasterID;
            task1.ApprovalOrder = 1;
            task1.ApprovalLevel = ProjectSettings.ApprovalLevelStaff;
            task1.ApprovalTypeCode = m1.Code;
            task1.ApprovalRole = m1.DisplayLabel;
            //--------------------------------------------------
            TaskItemObject task2 = new TaskItemObject();
            TaskMapping m2 = ProjectHelper.GetTaskMappingByCode(mappings, ProjectSettings.TaskCodeOCRAnalystAssignment);
            task2.Title = m2.DisplayLabel;
            task2.MasterID = this.MasterID;
            task2.ApprovalOrder = 2;
            task2.ApprovalLevel = ProjectSettings.ApprovalLevelStaff;
            //task2.AssignedTo = SPHelper.GetSPUserFromLoginName(dWeb, m2.Val);
            task2.ApprovalTypeCode = m2.Code;
            task2.ApprovalRole = m2.DisplayLabel;
            //-----------------------------------------------------------------
            TaskItemObject task3 = new TaskItemObject();
            TaskMapping m3 = ProjectHelper.GetTaskMappingByCode(mappings, ProjectSettings.TaskCodeOCRA);
            task3.Title = m3.DisplayLabel;
            task3.MasterID = this.MasterID;
            task3.ApprovalOrder = 3;
            task3.ApprovalLevel = ProjectSettings.ApprovalLevelStaff;
            task3.AssignedTo = SPHelper.GetSPUserFromLoginName(dWeb, m3.Val);
            //if (this.OCRAnalyst != null)
            //    task3.AssignedTo = this.OCRAnalyst;
            task3.ApprovalTypeCode = m3.Code;
            task3.ApprovalRole = m3.DisplayLabel;
            //-----------------------------------------------------------------
            TaskItemObject task4 = new TaskItemObject();
            TaskMapping m4 = ProjectHelper.GetTaskMappingByCode(mappings, ProjectSettings.TaskCodeGroupMgr);
            task4.Title = m4.DisplayLabel;
            task4.MasterID = this.MasterID;
            task4.ApprovalOrder = 4;
            task4.ApprovalLevel = ProjectSettings.ApprovalLevelManagement;
            //if (this.GroupManager != null)
            //    task4.AssignedTo = this.GroupManager;
            task4.ApprovalTypeCode = m4.Code;
            task4.ApprovalRole = m4.DisplayLabel;
            //-----------------------------------------------------------------
            TaskItemObject task5 = new TaskItemObject();
            TaskMapping m5 = ProjectHelper.GetTaskMappingByCode(mappings, ProjectSettings.TaskCodeOCRP1);
            task5.Title = m5.DisplayLabel;
            task5.MasterID = this.MasterID;
            task5.ApprovalOrder = 5;
            task5.ApprovalLevel = ProjectSettings.ApprovalLevelManagement;
            //task5.AssignedTo = SPHelper.GetSPUserFromLoginName(dWeb, m5.Val);
            task5.ApprovalTypeCode = m5.Code;
            task5.ApprovalRole = m5.DisplayLabel;

            //-----------------------------------------------------------------
            TaskItemObject task6 = new TaskItemObject();
            TaskMapping m6 = ProjectHelper.GetTaskMappingByCode(mappings, ProjectSettings.TaskCodeOCRP2);
            task6.Title = m6.DisplayLabel;
            task6.MasterID = this.MasterID;
            task6.ApprovalOrder = 6;
            task6.ApprovalLevel = ProjectSettings.ApprovalLevelManagement;
            //task6.AssignedTo = SPHelper.GetSPUserFromLoginName(dWeb, m6.Val);
            task6.ApprovalTypeCode = m6.Code;
            task6.ApprovalRole = m6.DisplayLabel;
            //-----------------------------------------------------------------
            TaskItemObject task7 = new TaskItemObject();
            TaskMapping m7 = ProjectHelper.GetTaskMappingByCode(mappings, ProjectSettings.TaskCodeMgrContract);
            task7.Title = m7.DisplayLabel;
            task7.MasterID = this.MasterID;
            task7.ApprovalOrder = 7;
            task7.ApprovalLevel = ProjectSettings.ApprovalLevelManagement;
            //task7.AssignedTo = SPHelper.GetSPUserFromLoginName(dWeb, m7.Val);
            task7.ApprovalTypeCode = m7.Code;
            task7.ApprovalRole = m7.DisplayLabel;
            //-----------------------------------------------------------------
            TaskItemObject task8 = new TaskItemObject();
            TaskMapping m8 = ProjectHelper.GetTaskMappingByCode(mappings, ProjectSettings.TaskCodeProcumentChief);
            task8.Title = m8.DisplayLabel;
            task8.MasterID = this.MasterID;
            task8.ApprovalOrder = 8;
            task8.ApprovalLevel = ProjectSettings.ApprovalLevelManagement;
            //task8.AssignedTo = SPHelper.GetSPUserFromLoginName(dWeb, m8.Val);
            task8.ApprovalTypeCode = m8.Code;
            task8.ApprovalRole = m8.DisplayLabel;
            //-----------------------------------------------------------------
            TaskItemObject task9 = new TaskItemObject();
            TaskMapping m9 = ProjectHelper.GetTaskMappingByCode(mappings, ProjectSettings.TaskCodeDeptChief);
            task9.Title = m9.DisplayLabel;
            task9.MasterID = this.MasterID;
            task9.ApprovalOrder = 9;
            task9.ApprovalLevel = ProjectSettings.ApprovalLevelManagement;
            //if (this.DepartmentChief != null)
            //    task9.AssignedTo = this.DepartmentChief;
            task9.ApprovalTypeCode = m9.Code;
            task9.ApprovalRole = m9.DisplayLabel;
            //-----------------------------------------------------------------
            TaskItemObject task10 = new TaskItemObject();
            TaskMapping m10 = ProjectHelper.GetTaskMappingByCode(mappings, ProjectSettings.TaskCodeDeptAGM);
            task10.Title = m10.DisplayLabel;
            task10.MasterID = this.MasterID;
            task10.ApprovalOrder = 10;
            task10.ApprovalLevel = ProjectSettings.ApprovalLevelExecutive;
            //if (this.DepartmentAGM != null)
            //    task10.AssignedTo = this.DepartmentAGM;
            task10.ApprovalTypeCode = m10.Code;
            task10.ApprovalRole = m10.DisplayLabel;
            //-----------------------------------------------------------------
            //03/09/2022 ---------------------------------------------------- add new procurement AGM

            TaskItemObject task11 = new TaskItemObject();
            TaskMapping m11 = ProjectHelper.GetTaskMappingByCode(mappings, ProjectSettings.TaskCodeProcurementChief);
            task11.Title = m11.DisplayLabel;
            task11.MasterID = this.MasterID;
            task11.ApprovalOrder = 11;
            task11.ApprovalLevel = ProjectSettings.ApprovalLevelExecutive;
            //task11.AssignedTo = SPHelper.GetSPUserFromLoginName(dWeb, m11.Val);
            task11.ApprovalTypeCode = m11.Code;
            task11.ApprovalRole = m11.DisplayLabel;

            //---------------------------------------------------------------
            TaskItemObject task12 = new TaskItemObject();
            TaskMapping m12 = ProjectHelper.GetTaskMappingByCode(mappings, ProjectSettings.TaskCodeOCRChief);
            task12.Title = m12.DisplayLabel;
            task12.MasterID = this.MasterID;
            task12.ApprovalOrder = 12;
            task12.ApprovalLevel = ProjectSettings.ApprovalLevelExecutive;
            //task12.AssignedTo = SPHelper.GetSPUserFromLoginName(dWeb, m11.Val);
            task12.ApprovalTypeCode = m12.Code;
            task12.ApprovalRole = m12.DisplayLabel;
            //-----------------------------------------------------------------
            this.Tasks = new List<TaskItemObject>();
            this.Tasks.Add(task1); this.Tasks.Add(task2);
            this.Tasks.Add(task3); this.Tasks.Add(task4);
            this.Tasks.Add(task5); this.Tasks.Add(task6);
            this.Tasks.Add(task7); this.Tasks.Add(task8);
            this.Tasks.Add(task9); this.Tasks.Add(task10); this.Tasks.Add(task11); this.Tasks.Add(task12);

            foreach (TaskItemObject t in this.Tasks)
            {
                t.TaskStatus = ProjectSettings.TaskStatusNone;
                t.ApprovedDate = DateTime.Now;
                t.DateCreated = DateTime.Now;
            }

        }
        protected List<ChangeOrder> getChangeOrderDefaultList()
        {
            List<ChangeOrder> listChanges = new List<ChangeOrder>();
            listChanges.Add(new ChangeOrder(1));
            listChanges.Add(new ChangeOrder(2));
            listChanges.Add(new ChangeOrder(3));
            //listChanges.Add(new ChangeOrder(4));
            //listChanges.Add(new ChangeOrder(5));
            //listChanges.Add(new ChangeOrder(6));
            //
            return listChanges;
        }

    }

    public class TaskMapping
    {
        public string Code { get; set; }
        public string Val { get; set; }
        public string valType { get; set; }
        public string DisplayN { get; set; }
        public string DisplayLabel { get; set; }
        public string Level { get; set; }
    }

    public enum MasterJobjectStatus
    {
       Draft=0, UnderReview=1, Approved=2, OnHold=3, Canceled=4, Completed=5
    }
    public enum UpdateType
    {
        SaveOnly = 0, RouteToNewApproversOnly 
            = 1, SaveNRouteToAll = 2, SaveAndRouteToNewApprovers
    }
    

}
