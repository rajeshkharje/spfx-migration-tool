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
    public class ChangeOrder
    {
        public string mddid { get; set; }
        public int OrderInTable { get; set; }
        public string Title { get; set; }
        public string Change { get; set; }
        public string Amount { get; set; }
        public string FY { get; set; }
        public string Note { get; set; }
        public string ItemID { get; set; }


        public ChangeOrder(int order, string itemid="" ,string reportId = "", string title="", string change = "", string amount = "", string fy = "", string note = "")
        {
            this.OrderInTable = order;
            if (!string.IsNullOrEmpty(title)) this.Title = title;
            if (!string.IsNullOrEmpty(change)) this.Change = change;
            if (!string.IsNullOrEmpty(amount)) this.Amount = amount;
            if (!string.IsNullOrEmpty(fy)) this.FY = fy;
            if (!string.IsNullOrEmpty(note)) this.Note = note;
            if (!string.IsNullOrEmpty(itemid)) this.ItemID = itemid;
            //
            
            if (!string.IsNullOrEmpty(reportId))
                this.mddid = reportId;
        }
        //
        public ChangeOrder(SPWeb dWeb, string itemId)
        {
            InitObject(dWeb, itemId);
        }
        public ChangeOrder(SPListItem item)
        {
            InitObject(item.Web, Convert.ToString(item.ID), item);
        }
        private void InitObject(SPWeb dWeb, string itemId, SPListItem oListItem = null)
        {
            if (oListItem == null)
            {
                SPList list = dWeb.Lists[Common.ProjectSettings.SPListProjectContracts];
                oListItem = list.GetItemById(Convert.ToInt32(itemId));
            }
            //-----------
            this.OrderInTable = Convert.ToInt32(oListItem["OrderInTable"]);
            this.mddid = Convert.ToString(oListItem["mddid"]);
            //-----------
            this.Change = Convert.ToString(oListItem["Change"]);
            this.Amount = Convert.ToString(oListItem["Amount"]);
            this.FY = Convert.ToString(oListItem["FY"]);
            this.Note = Convert.ToString(oListItem["Note"]);
            //
            if (!string.IsNullOrEmpty(itemId))
                this.ItemID = itemId;
            //--------------------------
        }

        public bool New(SPWeb dWeb)
        {
            try
            {
                SPList list = dWeb.Lists[Common.ProjectSettings.SPListChanges];
                SPListItem oListItem = list.Items.Add();
                //
                if (this.OrderInTable > 0)
                    oListItem["OrderInTable"] = this.OrderInTable;
                if (!string.IsNullOrEmpty(this.mddid))
                    oListItem["mddid"] = this.mddid;
                //-----------------------------------------------------------------------------//
                if (!string.IsNullOrEmpty(this.Change))
                    oListItem["Change"] = this.Change;
                if (!string.IsNullOrEmpty(this.Amount))
                    oListItem["Amount"] = this.Amount;
                if (!string.IsNullOrEmpty(this.FY))
                    oListItem["FY"] = this.FY;
                if (!string.IsNullOrEmpty(this.Note))
                    oListItem["Note"] = this.Note;

                // ------------------------------------------------------------------------//
                oListItem.SystemUpdate();
                this.ItemID = Convert.ToString(oListItem["ID"]);
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
                SPList list = dWeb.Lists[Common.ProjectSettings.SPListChanges];
                SPListItem oListItem = list.Items.GetItemById(Convert.ToInt32(this.ItemID));
                //
                //if (this.OrderInTable > 0)
                //    oListItem["OrderInTable"] = this.OrderInTable;
                //if (!string.IsNullOrEmpty(this.mddid))
                //    oListItem["mddid"] = this.mddid;
                //-----------------------------------------------------------------------------//
                oListItem["Change"] = !string.IsNullOrEmpty(this.Change) ? this.Change : string.Empty;
                oListItem["Amount"] = !string.IsNullOrEmpty(this.Amount) ? this.Amount : string.Empty;
                oListItem["FY"] = !string.IsNullOrEmpty(this.FY) ? this.FY : string.Empty;
                oListItem["Note"] = !string.IsNullOrEmpty(this.Note) ? this.Note : string.Empty;
                // ------------------------------------------------------------------------//
                oListItem.SystemUpdate();
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
                SPList list = dWeb.Lists[Common.ProjectSettings.SPListChanges];
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
