using DBAssigment.DTOs;

namespace DBAssigment.Services
{
    public interface IUserManagerServices
    {
        Task<AuthModel> RegisterAsync(RegisterDto registerDto);
        Task<AuthModel> LoginAsync(LoginDto loginDto);
    }
}
