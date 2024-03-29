﻿using System;
using System.Linq;
using System.Security.Claims;

namespace ServiceAPI.Helpers
{
    public static class ClaimsHelper
    {
        public static string GetClaimValueFromCurrentUserClaims(ClaimsPrincipal user, string claimType)
        {
            var claim = user.Claims.FirstOrDefault(c => c.Type == claimType);

            if (claim is null)
                throw new NullReferenceException(claimType);

            return claim.Value;
        }
    }
}
