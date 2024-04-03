using Microsoft.AspNetCore.Authorization;

namespace jwt_token
{
    public class userRequirementHandler : AuthorizationHandler<userRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, userRequirement requirement)
        {
            if (!context.User.HasClaim(claim => claim.Type == "id"))
            {
                return Task.CompletedTask;
            }
            if (context.User.FindFirst(c => c.Type == "id").Value == requirement.Id)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
