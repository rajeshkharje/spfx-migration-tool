using BART.SP.OCR.CP.Base;
using BART.SP.OCR.CP.Common;
using Microsoft.SharePoint;
using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace BART.SP.OCR.CP.Web.MenuPage3
{
    public partial class MenuPage3UserControl : ProjectUserControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    //
                    using (SPSite dSite = new SPSite(this.DataSiteURL))
                    {
                        using (SPWeb dWeb = dSite.OpenWeb(this.DataWebRelativeURL))
                        {
                            this.loadProjectsToList(dWeb);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ProjectUtilities.LogError(ex.ToString());
                }
            }
        }
        //--------------------------------------------
        protected void loadProjectsToList(SPWeb web)
        {
            DataTable dtProjects = Common.ProjectHelper.GetAllItemTableByListName(ProjectSettings.ProjectList, web);
            //dtProjects.DefaultView.Sort = "Project_x0020_Name ASC";

            StringBuilder sbAllProjects = new StringBuilder();
            foreach (DataRow r in dtProjects.Rows)
            {
                sbAllProjects.Append(string.Format("|{0}-{1}-{2}-{3}|", Convert.ToString(r["Program_x0020_Name"]).ToLower().Trim()
                    , Convert.ToString(r["Program"]).ToLower().Trim(), Convert.ToString(r["Project_x0020_Name"]).ToLower().Trim(), Convert.ToString(r["Title"]).ToLower().Trim()));
            }
            this.hdfAllProjects.Value = sbAllProjects.ToString().Trim();
            // ---- Program

            if (string.IsNullOrEmpty(this.lblDataListProgramName.Text))
            {
                DataView view = new DataView(dtProjects);
                string[] filter = { "Program", "Program_x0020_Name" };
                DataTable distinctValues = view.ToTable(true, filter);
                distinctValues.DefaultView.Sort = "Program_x0020_Name ASC";

                StringBuilder sbNames = new StringBuilder();
                sbNames.Append("<datalist id=\"programNames\">");

                //StringBuilder sbIDs = new StringBuilder();
                //sbIDs.Append("<datalist id=\"programIDs\">");
                
                foreach (DataRow row in distinctValues.Rows )
                {
                    string pName = Convert.ToString(row["Program_x0020_Name"]);
                    string pID = Convert.ToString(row["Program"]);

                    if (!string.IsNullOrEmpty(pName) && !string.IsNullOrEmpty(pID))
                    {
                        pName = string.Format("<option pKey=\"{0}\">{1}</option>",pID,pName);
                        //pID = string.Format("<option>{0}</option>", pID);
                        sbNames.Append(pName);
                        //sbIDs.Append(pID);
                    }
                }
                sbNames.Append("</datalist>");
                this.lblDataListProgramName.Text = sbNames.ToString();
                //this.lblDataListProgramID.Text = sbIDs.ToString();
            }

        }
        protected void Save()
        {
            bool isValidInput = false;
            // bool isDateTimeOverlap = false;
            //
            using (SPSite dSite = new SPSite(this.DataSiteURL))
            {
                using (SPWeb dWeb = dSite.OpenWeb(this.DataWebRelativeURL))
                {

                    if (!string.IsNullOrEmpty(this.txtProgramName.Text) && !string.IsNullOrEmpty(this.txtProgramID.Text)
                             && !string.IsNullOrEmpty(this.txtProjectName.Text) && !string.IsNullOrEmpty(this.txtProjectID.Text))
                    {
                        isValidInput = true;
                    }

                    if (isValidInput)
                    {
                        try
                        {
                            // If exist
                            string value= string.Format("|{0}-{1}-{2}-{3}|"
                            , Convert.ToString(this.txtProgramName.Text).ToLower().Trim()
                            , Convert.ToString(this.txtProgramID.Text).ToLower().Trim()
                            , Convert.ToString(this.txtProjectName.Text).ToLower().Trim()
                            , Convert.ToString(this.txtProjectID.Text).ToLower().Trim());
                            //----- Update 
                            value = value.Trim();
                            if(!this.hdfAllProjects.Value.Contains(value))
                            {
                                SPListItem item = dWeb.Lists[ProjectSettings.ProjectList].Items.Add();
                                item["Title"] = this.txtProjectID.Text.Trim();
                                item["Project_x0020_Name"] = this.txtProjectName.Text.Trim();
                                item["Program"] = this.txtProgramID.Text.ToUpper().Trim();
                                item["Program_x0020_Name"] = this.txtProgramName.Text.Trim();
                                item.Update();
                            }
                            //
                            this.lblCompleteMessage.Text = "Project added successfully !";
                            this.txtIsSubmit.Text = "Completed";

                        }
                        catch
                        {
                            isValidInput = false;
                        }
                        
                    }
                    if (!isValidInput)
                        this.lblCompleteMessage.Text = "* Invalid input. Please enter all required fields. ";

                }
            }
        }
        protected void Validate(TextBox txt)
        {
           
                
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.Save();
        }
    }
}
