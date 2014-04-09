using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;
using MyFestival.Models;

namespace MyFestival.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        /*public class ExtendedSimpleMembershipProvider : SimpleMembershipProvider
        {
            public override bool ValidateUser(string login, string password)
            {
                // check to see if the login passed is an email address
                if (IsValidEmail(login))
                {
                    string actualUsername = base.GetUserNameByEmail(login);
                    return base.ValidateUser(actualUsername, password);
                }
                else
                {
                    return base.ValidateUser(login, password);
                }

            }

            bool IsValidEmail(string strIn)
            {
                // Return true if strIn is in valid e-mail format.
                return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            }

        }*/

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                Database.SetInitializer<MyFestivalDb>(null);

                try
                {
                    using (var context = new MyFestivalDb())
                    {
                        if (!context.Database.Exists())
                        {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                    }

                    WebSecurity.InitializeDatabaseConnection("MyFestivalDb", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }
    }
}
