using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace BART.SP.OCR.CP.Common
{
    public class SPHelper
    {
        public static SPList GetSPListByName(SPWeb oWeb, string listTitle)
        {
            return oWeb.Lists[listTitle];
        }
        //
        public static string GetFieldValueUser(SPListItem item, string fieldName)
        {
            if (item != null)
            {
                if (item[fieldName] != null)
                {
                    SPFieldUserValue userValue = new SPFieldUserValue(item.ParentList.ParentWeb, Convert.ToString(item[fieldName]));
                    return userValue.User.LoginName;
                }
            }
            //
            return string.Empty;
        }

        /// <summary>
        /// Gets the display Name of the user based on loginName
        /// </summary>
        /// <param name="oWeb"></param>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public string GetDisplayName(SPWeb oWeb, string loginName)
        {
            string displayName = string.Empty;

            try
            {
                SPUser oUser = oWeb.AllUsers["loginName"];
                displayName = oUser.Name;
            }
            catch (Exception e)
            {
                LogWrite(e.Message, EventLogEntryType.Error);
            }

            return displayName;
        }

        /// <summary>
        /// Gets the logged in user 
        /// </summary>        
        /// <returns></returns>
       /* public string GetLoggedInUserName(bool loggedInPerson)
        {
            string currentUserName = string.Empty;

            try
            {

                //get current service context
                SPSite site = SPContext.Current.Site;
                SPServiceContext serviceContext = SPServiceContext.GetContext(site);

                UserProfile currentUser = null;

                UserProfileManager upm = new UserProfileManager(serviceContext);
                currentUser = upm.GetUserProfile(false);

                if (loggedInPerson)
                {
                    currentUserName = getLoggedInPersonName(currentUser);
                }
                else
                {
                    currentUserName = getFullName(currentUser);
                }
            }
            catch (Exception e)
            {
                LogWrite(e.Message, EventLogEntryType.Error);
            }

            return currentUserName;
        }*/

        /// <summary>
        /// Gets the logged in user 
        /// </summary>        
        /// <returns></returns>
       /* public UserProfile GetLoggedInUser()
        {
            UserProfile currentUser = null;

            try
            {

                //get current service context
                SPSite site = SPContext.Current.Site;
                SPServiceContext serviceContext = SPServiceContext.GetContext(site);

                UserProfileManager upm = new UserProfileManager(serviceContext);
                currentUser = upm.GetUserProfile(false);
            }
            catch (Exception e)
            {
                LogWrite(e.Message, EventLogEntryType.Error);
            }
            return currentUser;
        }*/

        /// <summary>
        /// Gets the full name based on input User Profile
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        /*public string getFullName(UserProfile profile)
        {
            string firstname = "";
            string lastname = "";
            string fullname = "";

            try
            {
                firstname = GetProfilePropertyAsString(profile, "FirstName");

                lastname = GetProfilePropertyAsString(profile, "LastName");

                fullname = lastname + ", " + firstname;
            }
            catch (Exception e)
            {
                LogWrite(e.Message, EventLogEntryType.Error);
            }

            return fullname;
        }*/

        /// <summary>
        /// Gets the full name based on input User Profile
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        /*public string getLoggedInPersonName(UserProfile profile)
        {
            string firstName = "";
            string lastname = "";
            string fullname = "";

            try
            {
                firstName = GetProfilePropertyAsString(profile, "FirstName");

                lastname = GetProfilePropertyAsString(profile, "LastName");

                fullname = firstName + " " + lastname;
            }
            catch (Exception e)
            {
                LogWrite(e.Message, EventLogEntryType.Error);
            }

            return fullname;
        }*/


        /// <summary>
        /// Gets the logged in person's department
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        /*public string getLoggedInPersonDepartment()
        {
            string department = "";

            try
            {
                SPSite site = SPContext.Current.Site;
                SPServiceContext serviceContext = SPServiceContext.GetContext(site);

                UserProfile currentUser = null;

                UserProfileManager upm = new UserProfileManager(serviceContext);
                currentUser = upm.GetUserProfile(false);

                department = GetProfilePropertyAsString(currentUser, "Department");
            }
            catch (Exception e)
            {
                LogWrite(e.Message, EventLogEntryType.Error);
            }

            return department;
        }*/


        /// <summary>
        /// Gets the property name as a value
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /*public string GetProfilePropertyAsString(UserProfile profile, string propertyName)
        {
            string value = string.Empty;

            try
            {
                ProfileValueCollectionBase values = profile.GetProfileValueCollection(propertyName);

                if (values != null && values.Count != 0 && values[0].ToString().Trim().Length > 0)
                {
                    value = values[0].ToString();
                }
            }
            catch (Exception e)
            {
                LogWrite(e.Message, EventLogEntryType.Error);
            }
            return value;
        }*/



        /// <summary>
        /// Writes the exceptions to EventLog
        /// </summary>
        /// <param name="message"></param>
        /// <param name="eventType"></param>
        public static void LogWrite(object message, EventLogEntryType eventType)
        {
            try
            {
                string logSource = "CP";

                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    EventLog.WriteEntry(logSource, message.ToString(), eventType, 0);
                });

            }
            catch
            {
            }
        }

        /// <summary>
        /// Gets the value from PeoplePicker field
        /// </summary>
        /// <param name="people"></param>
        /// <param name="web"></param>
        /// <returns></returns>
        public SPFieldUserValue GetPeopleFromPickerControl(PeopleEditor people, SPWeb web)
        {
            SPFieldUserValue value = null;

            if (people.ResolvedEntities.Count > 0)
            {
                for (int i = 0; i < people.ResolvedEntities.Count; i++)
                {
                    try
                    {

                        PickerEntity user = (PickerEntity) people.ResolvedEntities[i];
                        SPUser webUser = web.EnsureUser(user.Key);
                        value = new SPFieldUserValue(web, webUser.ID, webUser.Name);
                    }
                    catch (Exception e)
                    {
                        LogWrite(e.Message, EventLogEntryType.Error);
                    }
                }
            }
            return value;
        }

        /// <summary>        
        /// Get multiple logins from People Picker control
        /// </summary>
        /// <param name="people"></param>
        /// <param name="web"></param>
        /// <returns></returns>
        public List<SPFieldUserValue> GetPeopleollectionsFromPickerControl(PeopleEditor people, SPWeb web)
        {
            List<SPFieldUserValue> collections = new List<SPFieldUserValue>();
            if (people.ResolvedEntities.Count > 0)
            {
                for (int i = 0; i < people.ResolvedEntities.Count; i++)
                {
                    try
                    {
                        PickerEntity user = (PickerEntity) people.ResolvedEntities[i];
                        SPUser webUser = web.EnsureUser(user.Key);
                        collections.Add(new SPFieldUserValue(web, webUser.ID, webUser.Name));
                    }
                    catch (Exception e)
                    {
                        LogWrite(e.Message, EventLogEntryType.Error);
                    }
                }
            }

            return collections;
        }

       

        /// Get the files uplaoded for any PND by title 
        /// </summary>
        /// <param name="oWeb"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public SPListItemCollection GetListItemCollectionByCalm(SPList list, string calm)
        {
            try
            {
                SPQuery query = new SPQuery();
                query.Query = calm;
                return list.GetItems(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Deletes any item from a list
        /// </summary>
        /// <param name="list"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <summary>
        /// Get all versions of a Multi text field in SharePoint
        /// </summary>
        /// <param name="item"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static string GetAllVersionsOfMultiTextField(SPListItem item, string fieldName)
        {

            SPListItemVersionCollection listItemVersions = item.Versions;
            StringBuilder sb = new StringBuilder();

            if (listItemVersions != null && listItemVersions.Count > 0)
            {
                foreach (SPListItemVersion vItem in listItemVersions)
                {
                    string singleVal = Convert.ToString(vItem[fieldName]);
                    if (!string.IsNullOrEmpty(singleVal))
                        sb.Append(string.Format("{0}<br />", singleVal));
                }
            }
            return sb.ToString();
        }


        /// <summary>
        /// Get latest version of a Multi text field in SharePoint
        /// </summary>
        /// <param name="item"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static string GetLatestVersionOfMultiTextField(SPListItem item, string fieldName)
        {

            SPListItemVersionCollection listItemVersions = item.Versions;
            if (listItemVersions != null && listItemVersions.Count > 0)
            {
                foreach (SPListItemVersion vItem in listItemVersions)
                {
                    string val = Convert.ToString(vItem[fieldName]);
                    if (!string.IsNullOrEmpty(val.Trim()))
                        return val;
                }
            }
            //
            return string.Empty;
        }

        /// <summary>
        /// Builds the hash that is used as identifier to display the PND 
        /// </summary>
        /// <param name="PNDID"></param>
        /// <param name="author"></param>        
        /// <returns></returns>
        public static bool IsMemberOfGroup(string groupName)
        {
            bool memberInGroup;

            SPWeb web = SPContext.Current.Site.RootWeb;

            if (CheckGroupExists(web.SiteGroups, groupName))
            {
                memberInGroup = web.IsCurrentUserMemberOfGroup(web.SiteGroups[groupName].ID);
            }
            else
            {
                memberInGroup = false;
            }

            return memberInGroup;
        }

        public static SPGroup GetGroup(string gName)
        {
            SPGroup group = null;
            try
            {
                SPWeb web = SPContext.Current.Site.RootWeb;
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    group = web.SiteGroups[gName];
                });

            }
            catch
            {

            }
            return group;
        }
        // Temporary use for Dev Environment to get email by login name
        public static string GetEmailByUser(string fullUserName, SPWeb eventWeb=null)
        {
            //return (fullUserName.Contains("\\")) ? string.Format("{0}@bart.gov", Convert.ToString(fullUserName.Split('\\')[1]).Trim()) : string.Empty;
            if(eventWeb==null)
                return ProjectUtilities.GetEmailByUser(fullUserName);
            else
                return ProjectUtilities.GetEmailByUser(fullUserName, eventWeb);
        }
        private static bool CheckGroupExists(SPGroupCollection groups, string name)
        {
            if (string.IsNullOrEmpty(name) ||
                (name.Length > 255) ||
                (groups == null) ||
                (groups.Count == 0))
                return false;
            else
                return (groups.GetCollection(new String[] { name }).Count > 0);
        }

        public static bool CheckIfUserHasFullControl(SPWeb web, SPUser user)
        {
            if (user.IsSiteAdmin || web.DoesUserHavePermissions(user.LoginName, SPBasePermissions.ManageLists)
                 || web.DoesUserHavePermissions(user.LoginName, SPBasePermissions.ManagePermissions)
                  || web.DoesUserHavePermissions(user.LoginName, SPBasePermissions.ManageSubwebs)
                   || web.DoesUserHavePermissions(user.LoginName, SPBasePermissions.ManageWeb)
                    || web.DoesUserHavePermissions(user.LoginName, SPBasePermissions.CreateGroups)
                )
                return true;
            return false;
        }

        // Get SPUser from field in item
        public static SPUser GetSPUserFromFieldInItem(SPWeb web, SPListItem item, string fieldInternalName)
        {
            try
            {
                SPFieldUserValue userValue = new SPFieldUserValue(web, Convert.ToString(item[fieldInternalName]));
                if (userValue.User != null)
                {
                    return userValue.User;
                }
            }
            catch (Exception ex)
            {
                //
            }
            return null;
        }

        public static object GetSPUserorGroupFromFieldInItem(SPWeb web, SPListItem item, string fieldInternalName)
        {
            try
            {
                SPFieldUserValue userValue = new SPFieldUserValue(web, Convert.ToString(item[fieldInternalName]));
                if (userValue.User != null)
                {
                    return userValue.User;
                }
                else
                {
                    return web.SiteGroups.GetByID(userValue.LookupId);

                }
            }
            catch (Exception ex)
            {
                //
            }
            return null;
        }


        // Get SPUser by Login Name
        public static SPUser GetSPUserFromLoginName(SPWeb web, string loginName)
        {
            SPUser user = null;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite dSite = new SPSite(web.Site.ID))
                    {
                        using (SPWeb dWeb = dSite.OpenWeb(web.ID))
                        {
                            web.AllowUnsafeUpdates = true;
                            user= web.EnsureUser(loginName);
                        }
                    }
                });
                return user;
            }
            catch (Exception ex)
            {
                //
            }
            return null;
        }

        //using (SPSite dSite = new SPSite(this.DataSiteURL))
        //{
        //    using (SPWeb dWeb = dSite.OpenWeb(this.DataWebRelativeURL))
        //    {
                
        //    }
        //}

        // New function to get email from User Info List  to resolve the issue with BPD user account and email
        public static string GetSPUserEmailLoginName(SPWeb web, string loginName)
        {
            SPUser user = null;
            try
            {
                if (web != null)
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate
                    {
                        using (SPSite dSite = new SPSite(web.Site.ID))
                        {
                            using (SPWeb dWeb = dSite.OpenWeb(web.ID))
                            {
                                web.AllowUnsafeUpdates = true;
                                user = web.EnsureUser(loginName);
                            }
                        }
                    });
                }
                else
                {
                    string siteURL = string.Format("{0}/{1}", Settings.WebAppURL, Settings.DataSiteRelativeURL);
                    SPSecurity.RunWithElevatedPrivileges(delegate
                    {
                        using (SPSite dSite = new SPSite(siteURL))
                        {
                            using (SPWeb dWeb = dSite.OpenWeb(Settings.DataWebRelativeURL))
                            {
                                web.AllowUnsafeUpdates = true;
                                user = web.EnsureUser(loginName);
                            }
                        }
                    });
                    
                }

                return (string.IsNullOrEmpty(user.Email)) ? string.Empty : user.Email.Trim();
            }
            catch (Exception ex)
            {
                //
            }
            return string.Empty;
            
        }

        public static SPListItemCollection GetItems(string listName, SPQuery query, SPWeb web)
        {
            SPList oList = web.Lists[listName];
            SPListItemCollection items = oList.GetItems(query);
            return items;
        }
        public static string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? string.Empty;
                return result;
            }
            catch (ConfigurationErrorsException)
            {

            }
            return string.Empty;
        }


        public static string GetMappingVal(string code)
        {
            string val = string.Empty;

            try
            {
                string cQuery = "<Where><Eq><FieldRef Name='code'/><Value Type='Text'>" + code.Trim() + "</Value></Eq></Where>";
                SPQuery query = new SPQuery();
                query.Query = cQuery;
                SPListItemCollection items = SPHelper.GetItems(ProjectSettings.OverallMapping, query, SPContext.Current.Web);
                if (items != null && items.Count > 0)
                {
                    val = Convert.ToString(items[0]["mapvalue"]).Trim();
                }
                //using (SPSite cSite = new SPSite(SPContext.Current.Site.ID))
                //{
                //    using (SPWeb cWeb = cSite.OpenWeb(SPContext.Current.Web.ID))
                //    {

                //    }
                //}
            }
            catch
            {

            }
            return val;
            
        }

        public static List<SPUser> GetUsersInGroup(SPGroup group)
        {
            List<SPUser> users = new List<SPUser>();
            try
            {

                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    foreach (SPUser user in group.Users)
                    {
                        // add all the group users to the list
                        if (!users.Contains(user))
                            users.Add(user);
                    }
                });

                
            }
            catch (Exception ex)
            {
                Common.ProjectUtilities.LogError(ex.ToString());
            }
            return users;
        }
        

        public static List<string> GetEmailsInGroup(SPGroup group)
        {
            List<string> emails = new List<string>();
            try
            {
                foreach (SPUser user in group.Users)
                {
                    // add all the group users to the list
                    if(!emails.Contains(user.Email))
                        emails.Add(user.Email);
                }
            }
            catch (Exception ex)
            {
                Common.ProjectUtilities.LogError(ex.ToString());
            }
            return emails;
        }

        public static List<string> GetEmailListByGroupName(string groupName, SPWeb web)
        {
            List<string> emails = new List<string>();
            //
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite dSite = new SPSite(web.ParentWeb.Site.ID))
                {
                    using (SPWeb dWeb = dSite.OpenWeb(web.ID))
                    {
                        try
                        {
                            SPGroup spGroup = dWeb.SiteGroups[groupName];
                            if (spGroup != null)
                            {
                                emails = GetEmailsInGroup(spGroup);
                            }
                        }
                        catch { }
                    }
                }
            });
            //----------------------------------------------------------------//
            return emails;

        }

        public static void UpdateMultipleUserField(SPListItem item, string multiUserField, List<SPUser> users, bool updateItem=true)
        {
            SPFieldUserValueCollection values = new SPFieldUserValueCollection(); //(SPFieldUserValueCollection) item[multiUserField];
            foreach (var user in users)
            {
                values.Add(new SPFieldUserValue(item.Web, user.ID, user.Name));
            }
            item[multiUserField] = values;
            if (updateItem)
                item.SystemUpdate();
        }
    }
}
