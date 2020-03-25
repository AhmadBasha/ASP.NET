using System;
using a3_Client.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace a3_Client.Attributes
{
    public class AuthorizeAdminAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // if admin admin  correct , this one will be true 
            var hasAuth = Helper.HasAuth();
            if(!hasAuth)
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }
    }
}
