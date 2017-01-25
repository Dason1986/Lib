using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Library.AD
{
    public class ADUser
    {
        public string EmailAddress { get; set; }

        public string UserPrincipalName { get; set; }

        public string GivenName { get; set; }

        public string MiddleName { get; set; }

        public string Surname { get; set; }

        public DateTime? LastLogon { get; set; }

        public DateTime? LastPasswordSet { get; set; }

        public SecurityIdentifier Sid { get; internal set; }
        public Guid Guid { get; internal set; }
        public string Displayname { get; internal set; }
        public string Description { get; internal set; }
        public string Name { get; internal set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class ActiveDirectory
    {
        #region Property

        /// <summary>
        ///
        /// </summary>
        public string LDAP { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Pwd { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Uid { get; set; }

        private string _customLoginField = "sAMAccountName";

        public string CustomLoginField
        {
            get { return _customLoginField; }
            set
            {
                if (string.IsNullOrEmpty(value)) throw new Exception();
                _customLoginField = value;
            }
        }

        #endregion Property

        public ADUser GetUser(string accountName)
        {
            if (string.IsNullOrEmpty(accountName)) return null;
            try
            {
                using (DirectoryEntry entry = new DirectoryEntry(LDAP, Uid, Pwd))
                {
                    using (DirectorySearcher searcher = new DirectorySearcher(entry))
                    {
                        searcher.Filter = string.Format("(&(objectCategory=person)(objectClass=user)(!objectClass=computer)({0}={1}))", CustomLoginField, accountName);
                        var one = searcher.FindOne();
                        if (one != null)
                        {
                            ADUser user = new ADUser();

                            if (one.Properties.Contains("objectsid"))
                            {
                                var sidInBytes = one.Properties["objectsid"][0] as byte[];
                                user.Sid = new SecurityIdentifier(sidInBytes, 0);
                            }
                            if (one.Properties.Contains("objectguid"))
                            {
                                var sidInBytes = one.Properties["objectguid"][0] as byte[];
                                user.Guid = new Guid(sidInBytes);
                            }
                            if (one.Properties.Contains("samAccountName"))
                            {
                                user.Surname = one.Properties["samAccountName"][0].ToString();
                            }
                            if (one.Properties.Contains("GivenName"))
                            {
                                user.GivenName = one.Properties["GivenName"][0].ToString();
                            }
                            if (one.Properties.Contains("Name"))
                            {
                                user.Name = one.Properties["Name"][0].ToString();
                            }
                            if (one.Properties.Contains("mail"))
                            {
                                user.EmailAddress = one.Properties["mail"][0].ToString();
                            }
                            if (one.Properties.Contains("description"))
                            {
                                user.Description = one.Properties["description"][0].ToString();
                            }
                            if (one.Properties.Contains("displayname"))
                            {
                                user.Displayname = one.Properties["displayname"][0].ToString();
                            }
                            if (one.Properties.Contains("UserPrincipalName"))
                            {
                                user.UserPrincipalName = one.Properties["UserPrincipalName"][0].ToString();
                            }
                            if (one.Properties.Contains("pwdlastset"))
                            {
                                var time = (long)one.Properties["pwdlastset"][0];
                                if (time != 0)
                                    user.LastPasswordSet = DateTime.FromFileTime(time);
                            }
                            if (one.Properties.Contains("lastlogon"))
                            {
                                var time = (long)one.Properties["lastlogon"][0];
                                if (time != 0)
                                    user.LastLogon = DateTime.FromFileTime(time);
                            }
                            return user;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return null;
        }

        public bool ValidateUser(string accountName, string password)
        {
            if (string.IsNullOrEmpty(accountName) || string.IsNullOrEmpty(password)) return false;
            bool result = false;

            using (DirectoryEntry entry = new DirectoryEntry(LDAP, accountName, password))
            {
                using (DirectorySearcher searcher = new DirectorySearcher(entry))
                {
                    string filter = string.Format("(&(objectCategory=user)({0}={1}))", CustomLoginField, accountName);
                    searcher.Filter = filter;
                    try
                    {
                        SearchResult adsSearchResult = searcher.FindOne();
                        if (adsSearchResult != null)
                        {
                            result = true;
                        }
                    }
                    catch (DirectoryServicesCOMException ex)
                    {
                        //0x8009030C;
                        if (ex.ExtendedError == -2146893044)
                        {
                            // Failed to authenticate.
                            result = false;
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            return result;
        }
    }
}