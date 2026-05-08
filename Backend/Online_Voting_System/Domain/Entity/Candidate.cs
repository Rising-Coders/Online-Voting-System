namespace Online_Voting_System.Domain.Entity
{
    public class Candidate
    {
        public int CandidateId { get; set; }
        public required string CandidateName { get; set; }
        public int? PartyId { get; set; }
        public Party? Partys { get; set; }
        public int ElectionId { get; set; }
        public Election? Elections { get; set; }
        
        public ICollection<Vote>? Votes { get; set; }
    }
}
