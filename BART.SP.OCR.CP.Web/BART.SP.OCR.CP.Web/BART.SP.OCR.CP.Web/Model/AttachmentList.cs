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

    public class AttachmentList
    {
        public string MasterID { get; set; }
        public List<Attachment> ListObjects { get; set; }
        public AttachmentList() {
            ListObjects = new List<Attachment>();
        }

        public SPListItemCollection GetAllAttachmentsFiles(SPWeb dWeb, string reportId)
        {
            SPList list = dWeb.Lists[Common.ProjectSettings.SPListAttachment];
            StringBuilder sb = new StringBuilder();
            SPListItemCollection items = null;
            sb.Append("<Where><And><Eq><FieldRef Name = 'MasterID' /><Value Type = 'Text'>" + reportId + "</Value></Eq>");
            sb.Append("<Neq><FieldRef Name='Status'/><Value Type='Text'>" + ProjectSettings.StatusDeleted + "</Value></Neq>");
            sb.Append("</And></Where>");
            SPQuery query = new SPQuery();
            query.Query = sb.ToString();
            // Get data from a list.
            items = list.GetItems(query);
            return items;
        }

        public AttachmentList(SPWeb dWeb, string reportId)
        {

            try
            {
                this.MasterID = reportId;
                ListObjects = new List<Attachment>();
                SPListItemCollection items = GetAllAttachmentsFiles(dWeb, reportId);
                if (items != null && items.Count > 0)
                {
                    foreach (SPListItem item in items)
                        this.ListObjects.Add(new Attachment(item));
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
                    Attachment item = new Attachment();
                    UploadedFile file = files[i];
                    item.MasterID = this.MasterID;
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
                        item.Update(dWeb);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.ProjectUtilities.LogError(ex.ToString());
            }

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
