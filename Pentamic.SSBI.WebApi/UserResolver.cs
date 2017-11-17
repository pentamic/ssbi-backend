using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Pentamic.SSBI.Services;

namespace Pentamic.SSBI.WebApi
{
    public class UserResolver: IUserResolver
    {
        private readonly IIdentity _userIdentity;
        public UserResolver(IHttpContextAccessor context)
        {
            _userIdentity = context.HttpContext.User.Identity;
        }

        public string GetUserId()
        {
            return (_userIdentity as ClaimsIdentity)?.Claims
                .First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                .Value;
        }

        public string GetUserName()
        {
            return _userIdentity.Name;
        }
    }
}
