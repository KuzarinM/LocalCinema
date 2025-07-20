using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdstractHelpers.Identity.Providers
{
    public class CustomAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public CustomAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options) { }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            // Обрабатываем только политики с префиксом "OneOfRoles:"
            if (policyName.StartsWith("OneOfRoles:", StringComparison.OrdinalIgnoreCase))
            {
                var roles = policyName["OneOfRoles:".Length..].Split(',');
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new OneOfRolesRequirement(roles));
                return policy.Build();
            }

            return await base.GetPolicyAsync(policyName);
        }
    }
}
