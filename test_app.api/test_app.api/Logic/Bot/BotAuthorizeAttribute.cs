using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using test_app.api.Data;

namespace test_app.api.Logic.Bot
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(string claimType, string claimValue) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { new Claim(claimType, claimValue) };
        }
    }

    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        readonly Claim _claim;
        private readonly ApplicationDbContext _context;

        public ClaimRequirementFilter(Claim claim, ApplicationDbContext context)
        {
            _claim = claim;
            _context = context;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var tokenRequest = context.HttpContext.Request.Headers["Authorization"];

                if (tokenRequest.Count == 0)
                {
                    throw new Exception("no token");
                }

                var token = tokenRequest.FirstOrDefault().Split("Bearer ")[1];
                context.HttpContext.Items.Add("token", token);

                #region Доп проверка на IP адрес
                var ip = "";

                #if DEBUG
                ip = context.HttpContext.Connection.LocalIpAddress.ToString();
                #endif
                #if DEBUG
                ip = context.HttpContext.Connection.RemoteIpAddress.ToString();
                #endif
                #endregion

                var bot = _context.Bots.FirstOrDefault(x => x.Token == token && x.IsHidden == false);
                // добавляем в HttpContext.Items, потокобезопасно
                context.HttpContext.Items.Add("botId", bot.Id);
            }
            catch (Exception ex)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
