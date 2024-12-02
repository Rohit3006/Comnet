using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace Comnet.Common.Helpers
{
    /// <summary>
    /// Helper
    /// </summary>
    public class ContextHelper(IHttpContextAccessor iHttpContextAccessor)
    {
        private readonly IHttpContextAccessor _iHttpContextAccessor = iHttpContextAccessor;

        /// <summary>
        /// Fetch UserID from Token
        /// </summary>
        public string GetUserIDFromToken()
        {
            string? userID = string.Empty;
            string? authToken = _iHttpContextAccessor.HttpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authToken) && string.IsNullOrEmpty(userID))
            {
                authToken = authToken.Substring("Bearer ".Length);
                if (!string.IsNullOrEmpty(authToken) && authToken != "null")
                {
                    var jwtToken = new JwtSecurityToken(authToken);
                    JwtPayload tokenPayload = jwtToken.Payload;
                    if (tokenPayload != null)
                    {
                        if (tokenPayload.ContainsKey("UserID")) // Check if "emails" claim exists
                        {
                            userID = tokenPayload["UserID"]?.ToString() ?? "";
                        }
                        else if (tokenPayload.ContainsKey("email")) // Check if "preferred_username" claim exists
                        {
                            userID = tokenPayload["email"]?.ToString() ?? "";
                        }
                    }
                }
            }
            return userID;
        }
    }
}
