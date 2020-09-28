using Health.API.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Health.API.Infra
{
    public class UserAutheticationEvent : CookieAuthenticationEvents
    {
        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var user = (from c in context.Principal.Claims
                               where c.Type == ClaimTypes.Email
                               select c.Value).FirstOrDefault();
            if (!user.Equals(PortalUser.Get(user).LoginId, System.StringComparison.InvariantCultureIgnoreCase)) {
                context.RejectPrincipal();

                context.Response.StatusCode = 401;

                await context.HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            }
            else{
                
            }
        }
    }
}