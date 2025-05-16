using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BART.SP.OCR.CP.Common;
using System.Xml;
using System.Xml.Serialization;

namespace BART.SP.OCR.CP.Model
{
    [Serializable]
    public class ContractList
    {
        public string MasterID { get; set; }
        public List<Contract> ListObjects { get; set; }
        public ContractList() { this.ListObjects = new List<Contract>(); }
        public ContractList(SPWeb dWeb, string reportId)
        {

            try
            {
                this.ListObjects = new List<Contract>();
                this.MasterID = reportId;
                SPListItemCollection items = null;
                SPList mItems = dWeb.Lists[Common.ProjectSettings.SPListProjectContracts];
                StringBuilder sb = new StringBuilder();
                sb.Append("<Where><And><Eq><FieldRef Name = 'MasterID' /><Value Type = 'Text'>" + reportId + "</Value></Eq>");
                sb.Append("<Neq><FieldRef Name='MasterID'/><Value Type='Text'>-1</Value></Neq>");
                sb.Append("</And></Where>");
                SPQuery query = new SPQuery();
                query.Query = sb.ToString();
                // Get data from a list.
                items = mItems.GetItems(query);

                if (items != null && items.Count > 0)
                {
                    foreach (SPListItem item in items)
                        ListObjects.Add(new Contract(item));
                }
            }
            catch (Exception ex)
            {
                Common.ProjectUtilities.LogError(ex.ToString());
            }
        }
        public SPListItemCollection GetAllAttachmentsFiles(SPWeb dWeb, string reportId)
        {
            SPList list = dWeb.Lists[Common.ProjectSettings.SPListAttachmentContract];
            StringBuilder sb = new StringBuilder();
            SPListItemCollection items = null;
            sb.Append("<Where><And><Eq><FieldRef Name = 'MasterID' /><Value Type = 'Text'>" + reportId + "</Value></Eq>");
            sb.Append("<Neq><FieldRef Name='MasterID'/><Value Type='Text'>-1</Value></Neq>");
            sb.Append("</And></Where>");
            SPQuery query = new SPQuery();
            query.Query = sb.ToString();
            // Get data from a list.
            items = list.GetItems(query);
            return items;
        }
        //
        public void CreateAll(SPWeb dWeb)
        {
            try
            {
                if (ListObjects != null && ListObjects.Count > 0 && !string.IsNullOrEmpty(this.MasterID))
                {
                    foreach (var item in ListObjects)
                    {
                        item.MasterID = this.MasterID;
                        item.New(dWeb);
                    }
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
                if (ListObjects != null && ListObjects.Count > 0 && !string.IsNullOrEmpty(this.MasterID))
                {
                    foreach (var item in ListObjects)
                    {
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
                if (ListObjects != null && ListObjects.Count > 0 && !string.IsNullOrEmpty(this.MasterID))
                {
                    foreach (var item in ListObjects)
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
