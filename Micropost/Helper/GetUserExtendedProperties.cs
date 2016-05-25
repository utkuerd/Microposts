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

        public static int GetFollowingCount(this System.Security.Principal.IPrincipal usr)
        {
            var followingCountClaim = ((ClaimsIdentity)usr.Identity).FindFirst("FollowingCount");
            if (followingCountClaim != null)
                return Convert.ToInt32(followingCountClaim.Value);

            return 0;
        }

        public static int GetFollowerCount(this System.Security.Principal.IPrincipal usr)
        {
            var followerCountClaim = ((ClaimsIdentity)usr.Identity).FindFirst("FollowerCount");
            if (followerCountClaim != null)
                return Convert.ToInt32(followerCountClaim.Value);

            return 0;
        }
    }
}