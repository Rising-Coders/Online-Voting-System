namespace Online_Voting_System.Domain.Dtos
{
    public class AppResponseDto
    {
        // It is used to return in Register and login like other Dtos
            public bool Success { get; set; }
            public string? Token { get; set; }
            public IEnumerable<string>? Errors { get; set; }
    }
}
