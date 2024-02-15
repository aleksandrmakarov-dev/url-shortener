using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Server.Csharp.Business.Common;
using Server.Csharp.Business.Models.Common;
using Server.Csharp.Presentation.Common;
using Server.Csharp.Presentation.Exceptions;

namespace Server.Csharp.Presentation.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RoleAuthorize:Attribute,IAuthorizationFilter
    {
        private readonly IList<Roles> _roles;
        public RoleAuthorize(params Roles[] roles)
        {
            _roles = roles ?? new Roles[] { };
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isAllowAnonymous = context
                .ActionDescriptor
                .EndpointMetadata
                .OfType<AllowAnonymousAttribute>().Any();

            if (isAllowAnonymous)
            {
                return;
            }

            JwtTokenPayload? user = (JwtTokenPayload?)context.HttpContext.Items[Constants.User];

            if (user == null || (_roles.Any() && !_roles.Contains(user.Role)))
            {
                // not logged in or role not authorized
                ErrorResponse errorResponse = new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Error = "401 Unauthorized",
                    Message = "You don't have enough right to access."
                };

                context.Result = new JsonResult(errorResponse)
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
            }
        }
    }
}
