using BART.SP.OCR.CP.Common;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Web.UI;

namespace BART.SP.OCR.CP.Model
{
    [Serializable]
    public class AttachmentContractList
    {
        public string MasterID { get; set; }
        public string ParentLevel2ID { get; set; }
        public string DisplayFiles { get { return this.GetListURLs(); }  }
        public List<AttachmentContract> ListObjects { get; set; }
        public AttachmentContractList()
        { 
            ListObjects = new List<AttachmentContract>();
        }

        public AttachmentContractList(SPWeb dWeb, string reportId, string masterid)
        {

            try
            {
                this.ParentLevel2ID = reportId;
                this.MasterID = masterid;
                ListObjects = new List<AttachmentContract>();
                SPListItemCollection items = null;
                SPList list = dWeb.Lists[Common.ProjectSettings.SPListAttachmentContract];
                StringBuilder sb = new StringBuilder();
                sb.Append("<Where><And><Eq><FieldRef Name = 'ParentLevel2ID' /><Value Type = 'Text'>" + reportId + "</Value></Eq>");
                sb.Append("<Eq><FieldRef Name='MasterID'/><Value Type='Text'>" + masterid + "</Value></Eq>");
                sb.Append("</And></Where>");
                SPQuery query = new SPQuery();
                query.Query = sb.ToString();
                // Get data from a list.
                items = list.GetItems(query);

                if (items != null && items.Count > 0)
                {
                    foreach (SPListItem item in items)
                        this.ListObjects.Add(new AttachmentContract(item));
                }
            }
            catch (Exception ex)
            {
                Common.ProjectUtilities.LogError(ex.ToString());
            }
        }

        //
        public void CreateAll(SPWeb dWeb, UploadedFileCollection files)
        {
            try
            {
                //if (this.ListObjects != null && this.ListObjects.Count > 0 )
                //{

                //}
                for (int i = 0; i < files.Count; i++)
                {
                    AttachmentContract item = new AttachmentContract();
                    UploadedFile file = files[i];
                    item.MasterID = this.MasterID;
                    item.ParentLevel2ID = this.ParentLevel2ID;
                    item.New(dWeb, file);
                }
            }
            catch (Exception ex)
            {
                Common.ProjectUtilities.LogError(ex.ToString());
            }

        }

        public void UpdateAll(SPWeb dWeb)
        {
            try
            {
                if (this.ListObjects != null && this.ListObjects.Count > 0 && !string.IsNullOrEmpty(this.MasterID))
                {
                    foreach (var item in this.ListObjects)
                    {
                        item.MasterID = this.MasterID;
                        item.ParentLevel2ID = this.ParentLevel2ID;
                        item.Update(dWeb);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.ProjectUtilities.LogError(ex.ToString());
            }

        }
        string GetListURLs()
        {
            StringBuilder sb = new StringBuilder();
            if (this.ListObjects.Count > 0)
            {
                foreach (AttachmentContract att in this.ListObjects)
                {
                    sb.Append(string.Format("<li><img src='{0}' /><a class=\"UploadedDocsLink\" target=\"_blank\" href=\"{1}\">{2}</a><a class=\"removeUploadedFile\" title=\"Remove file\" docKey=\"{3}\">x<span class=\"sremovecontractatt\">Remove</span></a></li>", Convert.ToString(att.FileIcon), Convert.ToString(att.URL), Convert.ToString(att.FileName), Convert.ToString(att.ItemID)));
                }
                if (!string.IsNullOrEmpty(sb.ToString()))
                {
                    return string.Format("<ul class=\"sswpuploadedDocuments\">{0}</ul>", sb.ToString());
                }
            }
            return string.Empty;

        }
        public void DeleteAll(SPWeb dWeb)
        {
            try
            {
                if (this.ListObjects != null && this.ListObjects.Count > 0 && !string.IsNullOrEmpty(this.MasterID))
                {
                    foreach (var item in this.ListObjects)
                    {
                        item.Delete(dWeb);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.ProjectUtilities.LogError(ex.ToString());
            }

        }
    }
}
