using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Tests.Integration.Helpers
{
    public class TestClaimsProvider
    {
        public IList<Claim> Claims { get; } = new List<Claim>();

        public TestClaimsProvider(string role)
        {
            if (role is null)
                throw new ArgumentNullException(nameof(role));

            Claims.Add(new Claim(ClaimTypes.Name, "Test user"));
            Claims.Add(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
            Claims.Add(new Claim(ClaimTypes.Role, role));
        }
    }
}
