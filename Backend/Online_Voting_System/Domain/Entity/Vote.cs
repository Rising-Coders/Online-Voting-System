namespace Online_Voting_System.Domain.Entity
{
    public class Vote
    {
        public int VoteId { get; set; }
        public int CandidateId { get; set; }
        public DateTime VoteAt { get; set; }
        public Candidate? Candidates { get; set; }
        public int VoterId { get; set; }
        public Voter? Voters { get; set; }
        public int ElectionId { get; set; }
        public Election? Elections { get; set; }
    }
}
