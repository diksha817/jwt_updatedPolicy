using Microsoft.AspNetCore.Authorization;

namespace jwt_token
{
    public class userRequirement: IAuthorizationRequirement
    {
        public userRequirement(string id)
        {
            Id = id;
        }
        public string Id { get; set; }
    }
}
