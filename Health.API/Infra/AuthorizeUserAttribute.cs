using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Claims;

namespace Health.API.Infra
{
    public class AuthorizeUserAttribute : Attribute, IAuthorizationFilter
    {
        public AuthorizeUserAttribute()
        {
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var user = (from c in context.HttpContext.User.Claims
                            where c.Type == ClaimTypes.Name
                            select c.Value).FirstOrDefault();
                if (string.IsNullOrEmpty(user))
                    context.Result = new ForbidResult();
            }
            else
            {
                context.Result = new UnauthorizedResult();
            }
        }
        
    }

}
