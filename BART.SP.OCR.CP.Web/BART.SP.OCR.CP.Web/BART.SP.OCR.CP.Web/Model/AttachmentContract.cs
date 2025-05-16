using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BART.SP.OCR.CP.Common;
using Telerik.Web.UI;
using System.IO;
using Microsoft.SharePoint.Utilities;

namespace BART.SP.OCR.CP.Model
{
    [Serializable]

    public class AttachmentContract
    {
        public string MasterID { get; set; }
        public string ParentLevel2ID { get; set; }
        public string ItemID { get; set; }
        public string FileName { get; set; }
        public string URL { get; set; }
        public string FileIcon { get; set; }
        //public string Caption { get; set; }
        //public UploadedFile PhotoFile { get; set; }

        public AttachmentContract(string masterid = "")
        {
            if (!string.IsNullOrEmpty(masterid))
                this.MasterID = masterid;
        }

        public AttachmentContract(SPListItem item)
        {
            InitObject(item);
        }
        public AttachmentContract(SPWeb dWeb, string itemId)
        {
            SPList list = dWeb.Lists[Common.ProjectSettings.SPListAttachmentContract];
            SPListItem oListItem = list.GetItemById(Convert.ToInt32(itemId));
            InitObject(oListItem);
        }
        private void InitObject(SPListItem oListItem)
        {
            this.MasterID = Convert.ToString(oListItem["MasterID"]);
            this.ParentLevel2ID = Convert.ToString(oListItem["ParentLevel2ID"]);
            string realFileName= Convert.ToString(oListItem["FileLeafRef"]);
            this.FileName = ProjectUtilities.GetSSWPFileNameToDisplay(realFileName);
            if (!string.IsNullOrEmpty(oListItem.ID.ToString()))
                this.ItemID = oListItem.ID.ToString();
            this.URL= string.Format("{0}/{1}/{2}", oListItem.Web.Url, oListItem.ParentList.RootFolder, realFileName);
            this.FileIcon = SPUtility.ConcatUrls("/_layouts/images",
                    SPUtility.MapToIcon(oListItem.Web, SPUtility.ConcatUrls(oListItem.Web.Url, oListItem.Url), "", IconSize.Size16));
        }

        public bool New(SPWeb dWeb, UploadedFile photoFile)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(photoFile.FileName))
                {
                    Stream fStream = photoFile.InputStream;
                    if (fStream.Length <= Settings.FileSizeLimit)
                    {
                        byte[] contents = new byte[fStream.Length];
                        fStream.Read(contents, 0, (int) fStream.Length);
                        fStream.Close();
                        // Remove all special Characters
                        string filename = ProjectUtilities.TrimFileName(photoFile.FileName);
                        filename = ProjectUtilities.MakeSSWPFileName(filename);
                        SPFile fileAdded = dWeb.Lists[Common.ProjectSettings.SPListAttachmentContract].RootFolder.Files.Add(filename, contents, true);
                        if(!string.IsNullOrEmpty(this.MasterID))
                            fileAdded.Item.Properties["MasterID"] = this.MasterID;
                        if (!string.IsNullOrEmpty(this.ParentLevel2ID))
                            fileAdded.Item.Properties["ParentLevel2ID"] = this.ParentLevel2ID;
                        //if (!string.IsNullOrEmpty(this.Caption))
                        //    fileAdded.Item.Properties["Caption"] = this.Caption;
                        fileAdded.Item.Properties["Title"] = string.Format("File_{0}_{1}", this.MasterID, Convert.ToString(fileAdded.Item.ID));
                        fileAdded.Item.SystemUpdate();
                        this.FileName = filename;
                        this.ItemID = Convert.ToString(fileAdded.Item.ID);
                        this.URL = string.Format("{0}/{1}/{2}", dWeb.Url, fileAdded.Item.ParentList.RootFolder, this.FileName);
                    }
                }

                
                return true;
            }
            catch (Exception ex)
            {
                ProjectUtilities.LogError(ex.ToString());
            }
            return false;
        }
        public bool Update(SPWeb dWeb)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.ItemID))
                {
                    SPList list = dWeb.Lists[Common.ProjectSettings.SPListAttachmentContract];
                    SPListItem oListItem = list.Items.GetItemById(Convert.ToInt32(this.ItemID));
                    //oListItem["Caption"] = this.Caption;
                    oListItem["MasterID"] = this.MasterID;
                    // ------------------------------------------------------------------------//
                    oListItem.SystemUpdate();
                    return true;
                }
                
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
                SPList list = dWeb.Lists[Common.ProjectSettings.SPListAttachmentContract];
                SPListItem oListItem = list.GetItemById(Convert.ToInt32(this.ItemID));
                oListItem.Recycle();
                return true;
            }
            catch (Exception ex)
            {
                ProjectUtilities.LogError(ex.ToString());
            }
            return false;
        }
    }
}
