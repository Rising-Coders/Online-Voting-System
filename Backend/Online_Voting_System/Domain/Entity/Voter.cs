using Microsoft.AspNetCore.Identity;

namespace Online_Voting_System.Domain.Entity
{
    public class Voter : IdentityUser
    {
        public required string VoterName { get; set; }
        public bool IsEligible { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
