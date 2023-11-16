using Api.Auxiliary;
using Api.Domain.UserAuthorizations;
using System.Net;
using System.Security.Claims;

namespace Api.Security
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _requestEndpoint;

        public AuthorizationMiddleware(RequestDelegate requestEndpoint)
        {
            _requestEndpoint = requestEndpoint;
        }

        public async Task Invoke(HttpContext httpContext, IUserAuthorizationService userAuthorizationService)
        {
            AuthorizationAttribute? authorizationAttribute = httpContext.GetEndpoint()
                ?.Metadata
                ?.FirstOrDefault(o => o is AuthorizationAttribute) as AuthorizationAttribute;

            if (authorizationAttribute is null || (!authorizationAttribute.Authorization.HasValue && !authorizationAttribute.OnlySuperUser))
            {
                await _requestEndpoint(httpContext);
                return;
            }

            int? userId = httpContext.User?.FindFirst(ClaimTypes.Sid)?.Value?.To<int?>();
            bool? superUser = httpContext.User?.FindFirst(ClaimTypes.Role)?.Value?.Equals(true.ToString());
            //httpContext.Request.Headers.Add("_userId", userId.HasValue ? userId.Value.ToString() : string.Empty);

            async Task triggerNotAuthorized()
            {
                string notAuthorizedText = "User not allowed to access this resource.";
                httpContext.Response.ContentType = "text/plain"; //"text/plain"; //"application/json; charset=utf-8"
                httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await httpContext.Response.WriteAsync(notAuthorizedText);
                return;
            }

            if (!userId.HasValue || userId.Value <= 0
                || !superUser.HasValue)
            {
                await triggerNotAuthorized();
                return;
            }

            if (authorizationAttribute.OnlySuperUser 
                && (!superUser.HasValue || !superUser.Value))
            {
                await triggerNotAuthorized();
                return;
            }

            if ((superUser.HasValue && superUser.Value)
                || !authorizationAttribute.Authorization.HasValue)
            {
                await _requestEndpoint(httpContext);
                return;
            }

            if (userAuthorizationService is null)
            {
                await triggerNotAuthorized();
                return;
            }

            bool? authorized = await userAuthorizationService.UserIsAuthorizedAsync(userId.Value, authorizationAttribute.Authorization.Value.ToString());
            if (!authorized.HasValue || !authorized.Value)
            {
                await triggerNotAuthorized();
                return;
            }

            await _requestEndpoint(httpContext);
        }
    }
}
