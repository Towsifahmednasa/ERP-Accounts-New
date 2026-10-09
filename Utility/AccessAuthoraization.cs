// Ported unchanged from Accounts\CSTL_Pro_to_Pay\Utility\AccessAuthoraization.cs (framework-level edits only: usings, namespace, MVC 5 -> ASP.NET Core API names).
using System;
using New_Accounts.Compat;
using System.Collections.Generic;
using System.Linq;
using New_Accounts.Utility;
using New_Accounts.DAL;


namespace New_Accounts.Utility
{
    public class AccessAuthoraization
    {

        public static bool CheckAccess(string controllerName, string actionName)
        {

            try
            {
                if (HttpContext.Current.Session[SessionCollection.UserName] == null)
                {
                    return false;
                }
                if (HttpContext.Current.Session[SessionCollection.UserName].ToString().ToLower() == "admin")
                {
                    return true;
                }
                else
                {
                    UtilityDAL aUtilityDal = new UtilityDAL();
                    if (aUtilityDal.CheckMenuAccess(controllerName, actionName, HttpContext.Current.Session[SessionCollection.UserName].ToString().ToLower()))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            catch (Exception)
            {

                return false;
            }
        }

    }
}