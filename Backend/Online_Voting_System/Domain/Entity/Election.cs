using Online_Voting_System.Domain.Enum;

namespace Online_Voting_System.Domain.Entity
{
    public class Election
    {
        public int ElectionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime EndAt { get; set; }
        public ElectionStatus Status { get; set; }
    }
}
