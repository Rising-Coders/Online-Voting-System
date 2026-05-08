using Online_Voting_System.Domain.Dtos;

namespace Online_Voting_System.Application.Services
{
    public interface IAuthService
    {
        Task<AppResponseDto> Register(RegisterDto dto, string role);
        Task<AppResponseDto> Login(LoginDto dto);
    }
}
