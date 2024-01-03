using DB_Assigment.DTOs;
using DB_Assigment.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DB_Assigment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAuthRepository authRepository;

        public AccountsController(IAuthRepository _authRepository)
        {
            authRepository = _authRepository;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AuthDto authDto = await authRepository.Register(registerDto);

            if(authDto.Message != null)
            {
                return BadRequest($"{authDto.Message}");
            }

            return Ok(authDto.Token);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            AuthDto authDto = await authRepository.Login(loginDto);

            if(authDto.Message != null)
                return BadRequest($"{authDto.Message}");

            return Ok(authDto.Token);
        }
    }
}
