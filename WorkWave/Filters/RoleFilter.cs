using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WorkWave.DbModels;
using WorkWave.DBModels;

namespace WorkWave.Filters
{
    public class RoleFilter: ActionFilterAttribute, IAsyncActionFilter
    {
        private readonly string _filterParameter;
        public RoleFilter(string filterParameter) {
            _filterParameter = filterParameter;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var serviceProvider = context.HttpContext.RequestServices;
            using var scope=serviceProvider.CreateScope();
            var _context = scope.ServiceProvider.GetRequiredService<WorkwaveContext>();
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            if (context.HttpContext.Request.Headers.TryGetValue("Authorization",out var authorizationHeader))
            {
                var token = authorizationHeader.ToString().Replace("Bearer ", "");
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetValue<string>("AppSettings:Token")))
                };

                try
                {
                    SecurityToken validatedToken;
                    var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                    var userClaim = principal.Claims.FirstOrDefault(e => e.Type.Contains("nameidentifier"));

                    if (userClaim != null)
                    {
                        var userId = userClaim.Value;
                        var roles = _filterParameter.ToLower().Split(",");
                        var user = await userManager.FindByIdAsync(userId);
                        var userRoles = await userManager.GetRolesAsync(user);
                        var count = roles.Intersect(userRoles).ToList().Count();
                        if (count == 0)
                        {
                            context.Result = new UnauthorizedResult();
                            return;
                        }

                    }
                    else
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }

                }
                catch
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            await next();
        }
    }
}
