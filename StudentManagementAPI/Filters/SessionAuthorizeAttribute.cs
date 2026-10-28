using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StudentManagementAPI.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class SessionAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _allowedRoles;


public SessionAuthorizeAttribute(params string[] allowedRoles)
    {
        _allowedRoles = allowedRoles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userId = context.HttpContext.Session.GetInt32("UserId");
        var role = context.HttpContext.Session.GetString("Role");

        if (userId == null || string.IsNullOrEmpty(role))
        {
            context.Result = new UnauthorizedObjectResult(
                "Please log in to access this resource.");
            return;
        }

        if (_allowedRoles.Length > 0 &&
            !_allowedRoles.Contains(role, StringComparer.OrdinalIgnoreCase))
        {
            context.Result = new ObjectResult("You do not have permission to access this resource.")
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }
    }


}
