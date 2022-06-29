namespace HeimGuard.AutoPolicy
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Options;

    public class HeimGuardAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly IOptions<AuthorizationOptions> _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeimGuardAuthorizationPolicyProvider"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public HeimGuardAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {
            _options = options;
        }

        /// <summary>
        /// Gets a <see cref="T:Microsoft.AspNetCore.Authorization.AuthorizationPolicy" /> from the given <paramref name="policyName" />
        /// </summary>
        /// <param name="policyName">The policy name to retrieve.</param>
        /// <returns>
        /// The named <see cref="T:Microsoft.AspNetCore.Authorization.AuthorizationPolicy" />.
        /// </returns>
        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            // check static policies and add it if it isn't already there
            var policy = await base.GetPolicyAsync(policyName);
            if (policy is null)
            {
                policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new PermissionRequirement(policyName))
                    .Build();

                _options.Value.AddPolicy(policyName, policy);
            }

            return policy;
        }
    }
}
