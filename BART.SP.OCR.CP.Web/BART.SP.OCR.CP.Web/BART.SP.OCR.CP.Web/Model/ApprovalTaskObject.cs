using BART.SP.OCR.CP.Common;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BART.SP.OCR.CP.Model
{
    [Serializable]
    public class ApprovalTaskObject
    {
        public string MasterID { get; set; }
        public string TaskId { get; set; }
        public string AssignedToLogin { get; set; }
        public string AssignedToName { get; set; }
        public string AssignedToEmailAddress { get; set; }
        public string TaskTitle { get; set; }
        public string Details { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ApprovedDate { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateToRemind { get; set; }
        public string Comment { get; set; }
        public string ApproverTypeCode { get; set; }
        public string TaskStatus { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
        public string ApprovedByLogin { get; set; }
        public string RequestorLogin { get; set; }
        public string SentNotification { get; set; }
        public string SentReminder { get; set; }
        public string Note { get; set; }
        
        public int ApprovalOrder { get; set; }
        public List<CommentAttachmentObject> attachments { get; set; }

        public ApprovalTaskObject(string tId)
        {

        }
        public ApprovalTaskObject()
        {

        }
       
    }
    public enum SSWPTaskStatus
    {
        None = 0, Routed = 1, Approved = 2, Rejected = 3
    }

    [Serializable]
    public class AttachmentObject
    {
        public string DocName { get; set; }
        public string DocId { get; set; }
        public string mddid { get; set; }
        public byte[] contents { get; set; }
    }

    public class CommentAttachmentObject
    {
        public string DocName { get; set; }
        public string DOcUrl { get; set; }
    }
}
