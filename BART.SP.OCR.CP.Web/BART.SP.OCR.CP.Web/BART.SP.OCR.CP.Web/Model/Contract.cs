using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BART.SP.OCR.CP.Common;
using System.Reflection;
using Telerik.Web.UI;

namespace BART.SP.OCR.CP.Model
{
    [Serializable]
    public class Contract
    {
        public string MasterID { get; set; }
        public string ItemID { get; set; }
        public int OrderInTable { get; set; }
        public string FundingSource { get; set; }
        public string ContractNo { get; set; }
        public string DollarAmount { get; set; }
        public string Duration { get; set; }
        public string Status { get; set; }
        public string DisplayFiles { get; set; }
        public SPUser AddedBy { get; set; }
        public SPUser OCRUser { get; set; }
        public DateTime? DateAdded { get; set; }
        public string Description { get; set; }
        public string OCRAnalysis { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? TargetCompletionDate { get; set; }

        
        public AttachmentContractList ListContractAttachments { get; set; }

        public UploadedFileCollection Files { get; set; }

        public Contract()
        {
            ProjectUtilities.InitObjectDynamicDefault(this);
            this.ListContractAttachments = new AttachmentContractList();
        }
        public Contract(ContractDisplay c,SPWeb dWeb)
        {
            this.MasterID = c.MasterID;
            this.ItemID = c.ItemID;
            this.OrderInTable = c.OrderInTable;
            this.FundingSource = c.FundingSource;
            this.ContractNo = c.ContractNo;
            this.DollarAmount = c.DollarAmount;
            this.Duration = c.Duration;
            this.Status = c.Status;
            if (!string.IsNullOrEmpty(c.AddedByLogin))
                this.AddedBy = SPHelper.GetSPUserFromLoginName(dWeb, c.AddedByLogin.Trim());
            if (!string.IsNullOrEmpty(c.OCRUserLogin))
                this.OCRUser = SPHelper.GetSPUserFromLoginName(dWeb, c.OCRUserLogin.Trim());
            this.DateAdded = c.DateAdded;
            this.Description = c.Description;
            this.OCRAnalysis = c.OCRAnalysis;
            this.DateModified = c.DateModified;
            this.TargetCompletionDate = c.TargetCompletionDate;
            this.ListContractAttachments = new AttachmentContractList();
            this.ListContractAttachments = c.ListContractAttachments;
        }

        public Contract(string masterid="", string itemid="", int orderintable=-1, string fund="", string damount="", string duration="", 
            string status="",string des="", string ocranalysis="",string contactNo="", DateTime? targetDate=null,UploadedFileCollection files=null,SPUser addedby = null, SPUser ocruser = null)
        {
            ProjectUtilities.InitObjectDynamicDefault(this);
            this.MasterID = string.IsNullOrEmpty(masterid) ? string.Empty : masterid;
            this.ItemID = string.IsNullOrEmpty(itemid) ? string.Empty : itemid;
            this.OrderInTable = (orderintable!=-1) ?orderintable:-1;
            this.FundingSource = string.IsNullOrEmpty(fund) ? string.Empty : fund;
            this.DollarAmount = string.IsNullOrEmpty(damount) ? string.Empty : damount;
            this.Duration = string.IsNullOrEmpty(duration) ? string.Empty : duration;
            this.Description = string.IsNullOrEmpty(des) ? string.Empty : des;
            this.OCRAnalysis = string.IsNullOrEmpty(ocranalysis) ? string.Empty : ocranalysis;
            this.Status = string.IsNullOrEmpty(status) ? string.Empty : status;
            if (files != null)
                this.Files = files;
            this.ContractNo= string.IsNullOrEmpty(contactNo) ? string.Empty : contactNo;
            if (targetDate!=null)
            {
                try { this.TargetCompletionDate = Convert.ToDateTime(targetDate); } catch { }
            }
            else
                this.TargetCompletionDate = null;

            this.OCRUser = (ocruser!=null)? ocruser : null;
            this.AddedBy = (addedby != null) ? addedby : null;


            this.ListContractAttachments = new AttachmentContractList();
        }

        //
        public Contract(SPWeb dWeb, string itemId)
        {
            InitObject(dWeb, itemId);
        }
        public Contract(SPListItem item)
        {
            InitObject(item.Web, Convert.ToString(item.ID), item);
        }
        private void InitObject(SPWeb dWeb, string itemid, SPListItem oListItem=null)
        {
            if (oListItem == null)
            {
                SPList list = dWeb.Lists[Common.ProjectSettings.SPListProjectContracts];
                oListItem = list.GetItemById(Convert.ToInt32(itemid));
            }
            ProjectUtilities.InitObjectDynamic(this, dWeb, oListItem);
            if (!string.IsNullOrEmpty(itemid))
                this.ItemID = itemid;
            this.ListContractAttachments = new AttachmentContractList(dWeb, this.ItemID, this.MasterID);
            this.DisplayFiles = this.ListContractAttachments.DisplayFiles;
            //--------------------------
        }

        public bool New(SPWeb dWeb, UploadedFileCollection files = null)
        {
            try
            {
                SPList list = dWeb.Lists[Common.ProjectSettings.SPListProjectContracts];
                SPListItem oListItem = list.Items.Add();
                ProjectUtilities.UpdateItemDynamic(this, oListItem);
                this.ItemID = Convert.ToString(oListItem["ID"]);
                this.ListContractAttachments.ParentLevel2ID = this.ItemID;
                this.ListContractAttachments.MasterID = this.MasterID;
                //
                if (files != null)
                    this.ListContractAttachments.CreateAll(dWeb, files);
                return true;
            }
            catch (Exception ex)
            {
                ProjectUtilities.LogError(ex.ToString());
            }
            return false;
        }
        public bool Update(SPWeb dWeb, UploadedFileCollection files = null, string deletedFiles="")
        {
            try
            {
                SPList list = dWeb.Lists[Common.ProjectSettings.SPListProjectContracts];
                SPListItem oListItem = list.Items.GetItemById(Convert.ToInt32(this.ItemID));
                this.ListContractAttachments.ParentLevel2ID = this.ItemID;
                this.ListContractAttachments.MasterID = this.MasterID;
                //-----------------------------------------------------------------------------//
                ProjectUtilities.UpdateItemDynamic(this, oListItem);

                this.ListContractAttachments.ParentLevel2ID = this.ItemID;
                this.ListContractAttachments.MasterID = this.MasterID;
                if (files != null)
                    this.ListContractAttachments.CreateAll(dWeb, files);

                return true;
            }
            catch (Exception ex)
            {
                ProjectUtilities.LogError(ex.ToString());
            }
            return false;
        }
        public bool Delete(SPWeb dWeb)
        {
            try
            {
                SPList list = dWeb.Lists[Common.ProjectSettings.SPListProjectContracts];
                SPListItem oListItem = list.GetItemById(Convert.ToInt32(this.ItemID));
                oListItem.Recycle();
                this.ListContractAttachments.DeleteAll(dWeb);

                return true;
            }
            catch (Exception ex)
            {
                ProjectUtilities.LogError(ex.ToString());
            }
            return false;
        }
    }

    [Serializable]
    public class ContractDisplay
    {
        public string MasterID { get; set; }
        public string ItemID { get; set; }
        public int OrderInTable { get; set; }
        public string FundingSource { get; set; }
        public string ContractNo { get; set; }
        public string DollarAmount { get; set; }
        public string Duration { get; set; }
        public string Status { get; set; }
        public bool Visible { get; set; }
        public string AddedByLogin { get; set; }
        public string OCRUserLogin { get; set; }
        public string DisplayFiles { get; set; }
        public DateTime? DateAdded { get; set; }
        public string Description { get; set; }
        public string OCRAnalysis { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? TargetCompletionDate { get; set; }
        public AttachmentContractList ListContractAttachments { get; set; }

        public ContractDisplay()
        {
            ProjectUtilities.InitObjectDynamicDefault(this);
            this.ListContractAttachments = new AttachmentContractList();
        }
        public ContractDisplay(string masterid = "", string itemid = "", int orderintable = -1, string fund = "", string damount = "", string duration = "",
           string status = "", string des = "", string ocranalysis = "", string contactNo = "", DateTime? targetDate = null,bool visible=false, UploadedFileCollection files = null, SPUser addedby = null, SPUser ocruser = null)
        {
            ProjectUtilities.InitObjectDynamicDefault(this);
            this.MasterID = string.IsNullOrEmpty(masterid) ? string.Empty : masterid;
            this.ItemID = string.IsNullOrEmpty(itemid) ? string.Empty : itemid;
            this.OrderInTable = (orderintable != -1) ? orderintable : -1;
            this.FundingSource = string.IsNullOrEmpty(fund) ? string.Empty : fund;
            this.DollarAmount = string.IsNullOrEmpty(damount) ? string.Empty : damount;
            this.Duration = string.IsNullOrEmpty(duration) ? string.Empty : duration;
            this.Description = string.IsNullOrEmpty(des) ? string.Empty : des;
            this.OCRAnalysis = string.IsNullOrEmpty(ocranalysis) ? string.Empty : ocranalysis;
            this.Status = string.IsNullOrEmpty(status) ? string.Empty : status;
            //if (files != null)
            //    this.Files = files;
            this.ContractNo = string.IsNullOrEmpty(contactNo) ? string.Empty : contactNo;
            if (targetDate != null)
            {
                try { this.TargetCompletionDate = Convert.ToDateTime(targetDate); } catch { }
            }
            else
                this.TargetCompletionDate = null;

            //this.OCRUser = (ocruser != null) ? ocruser : null;
            //this.AddedBy = (addedby != null) ? addedby : null;

            this.ListContractAttachments = new AttachmentContractList();
            this.Visible = visible;
        }

        public ContractDisplay(Contract c)
        {
            this.MasterID = c.MasterID;
            this.ItemID = c.ItemID;
            this.OrderInTable = c.OrderInTable; 
            this.FundingSource = c.FundingSource;
            this.DisplayFiles = c.DisplayFiles;
            this.ContractNo = c.ContractNo;
            this.DollarAmount = c.DollarAmount;
            this.Duration = c.Duration;
            this.Status = c.Status;
            if(c.AddedBy != null)
                this.AddedByLogin = c.AddedBy.LoginName;
            if (c.OCRUser != null)
                this.OCRUserLogin = c.OCRUser.LoginName;
            this.DateAdded = c.DateAdded;
            this.Description = c.Description;
            this.OCRAnalysis = c.OCRAnalysis;
            this.DateModified = c.DateModified;
            this.TargetCompletionDate = c.TargetCompletionDate;
            this.Visible = true;
            this.ListContractAttachments = new AttachmentContractList();
            this.ListContractAttachments = c.ListContractAttachments;
        }

              
    }
}
