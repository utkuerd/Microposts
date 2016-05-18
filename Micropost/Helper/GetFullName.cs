using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Microposts.Helper
{
    public static class IPrincipalExtension
    {

        public static string GetFullName(this System.Security.Principal.IPrincipal usr)
        {
            var fullNameClaim = ((ClaimsIdentity)usr.Identity).FindFirst("FullName");
            if (fullNameClaim != null)
                return fullNameClaim.Value;

            return "";
        }
    }
}