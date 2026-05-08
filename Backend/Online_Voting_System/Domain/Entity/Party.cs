namespace Online_Voting_System.Domain.Entity
{
    public class Party
    {
        public int PartyId { get; set; }
        public required string PartyName { get; set; }

        public ICollection<Candidate>? Candiates { get; set; }
    }
}
