using DB_Assigment.DTOs;

namespace DB_Assigment.IRepository
{
    public interface IAuthRepository
    {
        Task<AuthDto> Register(RegisterDto registerDto);
        Task<AuthDto> Login(LoginDto loginDto);
    }
}
