using DBAssigment.DTOs;
using DBAssigment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DBAssigment.Services
{
    public class UserManagerServices : IUserManagerServices
    {
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;

        public UserManagerServices(UserManager<User> _userManager,IConfiguration _configuration)
        {
            userManager = _userManager;
            configuration = _configuration;
        }
        #region Register Methdo Logic
        public async Task<AuthModel> RegisterAsync(RegisterDto registerDto)
        {
            #region Check if there is user with the same email or username
            if (await userManager.FindByEmailAsync(registerDto.Email) is not null)
                return new AuthModel { Message = "Email is already registered!" };

            if (await userManager.FindByNameAsync(registerDto.Username) is not null)
                return new AuthModel { Message = "Username is already registered!" };
            #endregion

            var user = new User
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                FullName = registerDto.FullName,
            };
            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return new AuthModel { Message = errors };
            }

            var jwtSecurityToken = await CreateJwtTokenAsync(user);


            return new AuthModel
            {
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName,
            };
        }
        #endregion

        #region Login Method Logic
        public async Task<AuthModel> LoginAsync(LoginDto loginDto)
        {
            AuthModel authModel = new AuthModel();

            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user is null || !await userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                authModel.Message = "The Email or the password is incorrect";
                return authModel;
            }
            JwtSecurityToken jwtSecurityToken = await CreateJwtTokenAsync(user);

            authModel.IsAuthenticated = true;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            var roles = await userManager.GetRolesAsync(user);
            authModel.Roles = roles.ToList();


            return authModel;
        }
        #endregion

        #region create The JwtToken
        private async Task<JwtSecurityToken> CreateJwtTokenAsync(User user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JWT:Key")));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken
                (
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(configuration.GetValue<double>("JWT:DurationInMinutes")),
                    signingCredentials: signingCredentials
                );

            return jwtSecurityToken;
        } 
        #endregion
    }
}
