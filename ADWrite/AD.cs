using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

using QueueLogic;

namespace ADWrite
{
    public class ADWriter
    {
        public void ADtoWrite (Data que)
        {
            PrincipalContext ouContex = new PrincipalContext
               (ContextType.Domain, "actived.com", "CN=Users, DC=actived, DC=com");
            //PrincipalContext ouContex = new PrincipalContext(ContextType.Domain, "23.101.62.36:389", "CN=Users, DC=actived, DC=com", "anytime", "Q1w2e3r4");
            try
            {   //create new user
                UserPrincipal up = new UserPrincipal(ouContex);
                up.SamAccountName = que.AdminUser;
                up.SetPassword(que.Password);
                up.Enabled = true;
                //up.ExpirePasswordNow();
                up.Save();

                //chech if group didn`t exist, create new and add user or only add user
                using (var checkgroup = GroupPrincipal.FindByIdentity(ouContex, IdentityType.SamAccountName, que.CloudServiceName))
                {
                    if (checkgroup != null)//if group exist, add user in it
                    {
                       var ip = GroupPrincipal.FindByIdentity(ouContex, que.CloudServiceName);
                       
                       ip.Members.Add(ouContex, IdentityType.SamAccountName, que.AdminUser);
                       ip.Save();
                    }
                    else //if isn`t exist create group end add user
                    {
                        var ip = new GroupPrincipal(ouContex);
                        ip.SamAccountName = que.CloudServiceName;
                        ip.Save();
                        ip.Members.Add(ouContex, IdentityType.SamAccountName, que.AdminUser);
                        ip.Save();
                    }
                }            
            }
            catch (Exception ex)
            {
                throw ex;             
            }
        }
    }
}
