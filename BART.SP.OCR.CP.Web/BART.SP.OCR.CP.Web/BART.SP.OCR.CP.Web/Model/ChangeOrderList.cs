using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BART.SP.OCR.CP.Common;

namespace BART.SP.OCR.CP.Model
{
    [Serializable]
    public class ChangeOrderList
    {
        public string mddid { get; set; }
        public List<ChangeOrder> ListObjects { get; set; }
        public ChangeOrderList(){ this.ListObjects = new List<ChangeOrder>(); }

        public ChangeOrderList(SPWeb dWeb, string reportId)
        {

            try
            {
                this.mddid = reportId;
                this.ListObjects = new List<ChangeOrder>();
                SPListItemCollection items = null;
                SPList changes = dWeb.Lists[Common.ProjectSettings.SPListChanges];
                StringBuilder sb = new StringBuilder();
                sb.Append("<Where><And><Eq><FieldRef Name = 'mddid' /><Value Type = 'Text'>" + reportId + "</Value></Eq>");
                sb.Append("<Neq><FieldRef Name='mddid'/><Value Type='Text'>-1</Value></Neq>");
                sb.Append("</And></Where>");
                SPQuery query = new SPQuery();
                query.Query = sb.ToString();
                // Get data from a list.
                items = changes.GetItems(query);

                if (items != null && items.Count > 0)
                {
                    foreach (SPListItem item in items)
                        ListObjects.Add(new ChangeOrder(item));
                }
            }
            catch (Exception ex)
            {
                Common.ProjectUtilities.LogError(ex.ToString());
            }
        }

        //
        public void CreateAll(SPWeb dWeb)
        {
            try
            {
                if (this.ListObjects != null && ListObjects.Count > 0 && !string.IsNullOrEmpty(this.mddid))
                {
                    foreach (var item in ListObjects)
                    {
                        item.mddid = this.mddid;
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
                if (ListObjects != null && ListObjects.Count > 0 && !string.IsNullOrEmpty(this.mddid))
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
                if (ListObjects != null && ListObjects.Count > 0 && !string.IsNullOrEmpty(this.mddid))
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
