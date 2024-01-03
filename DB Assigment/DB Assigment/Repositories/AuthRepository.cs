using DB_Assigment.DTOs;
using DB_Assigment.IRepository;
using DB_Assigment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace DB_Assigment.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        public AuthRepository(UserManager<User> _userManager,RoleManager<IdentityRole> _roleManager,IConfiguration _configuration)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            configuration = _configuration;
        }

        public async Task<AuthDto> Register(RegisterDto registerDto)
        {
            if(await userManager.FindByEmailAsync(registerDto.Email) != null)
            {
                return new AuthDto
                {
                    Message = "That Email is already exists"
                };
            }

            if(await userManager.FindByNameAsync(registerDto.UserName) != null)
            {
                return new AuthDto
                {
                    Message = "that username is already exists"
                };
            }

            User user = new User
            {
                FullName = registerDto.FullName,
                UserName = registerDto.UserName,
                Email = registerDto.Email,
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);

            if(!result.Succeeded)
            {
                string errors = "";
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description}";
                }
                return new AuthDto
                {
                    Message = errors
                };
            }

            // check if the role exists or not
            bool checkRoleExists = await EnsureRoleExists("Clinet");
            if (!checkRoleExists)
            {
                return new AuthDto
                {
                    Message = "Error on create Role"
                };
            }
            
            // add the role to the new user
            await userManager.AddToRoleAsync(user, "Client");

            // call the method that create the token
            var token = await CreateJwtToken(user);

            if (token == null)
            {
                return new AuthDto
                {
                    Message = "Error on Jwt Token Creation"
                };
            }

            // check of refresh token
            var refreshToken = await EnsureRefreshTokenExists(user);

            if(refreshToken is null)
            {
                return new AuthDto
                {
                    Message = "Error on Refresh Token Creation"
                };
            }

            return new AuthDto
            {
                Token = token,
                RefershToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.ExpiresOn
            };
        }

        public async Task<AuthDto> Login(LoginDto loginDto)
        {
            User? user = await userManager.FindByEmailAsync(loginDto.Email);
            if(user == null)
            {
                return new AuthDto
                {
                    Message = "The email or password is not correct"
                };
            }

            bool found = await userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!found)
            {
                return new AuthDto
                {
                    Message = "the email or password is not correct"
                };
            }

            // call the method that create token
            var token = await CreateJwtToken(user);

            if(token is null)
            {
                return new AuthDto
                {
                    Message = "error on jwt token creation"
                };
            }

            // check of refresh token
            var refreshToken = await EnsureRefreshTokenExists(user);

            if (refreshToken is null)
            {
                return new AuthDto
                {
                    Message = "Error on Refresh Token Creation"
                };
            }

            return new AuthDto
            {
                Token = token,
                RefershToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.ExpiresOn
            };
        }

        private async Task<string?> CreateJwtToken(User user)
        {
            // get and add the claims
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()));
            claims.Add(new Claim("UserName",user.UserName));

            var roles = await userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                claims.Add(new Claim("Role",role.ToString()));
            }

            // create the security key
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JWT:Key")));

            // create the signincredentials
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // create the jwt token
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
                (
                    claims: claims,
                    signingCredentials: signingCredentials,
                    expires: DateTime.Now.AddMinutes(configuration.GetValue<double>("JWT:DurationInMinutes"))
                );

            // return the token in format string
            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private async Task<RefreshToken> GenerateRefreshToken()
        {
            // generate the refreshToken
            var randomNumber = new byte[32];
            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddMinutes(configuration.GetValue<double>("JWT:DurationInMinutes"))
            };
        }

        private async Task<RefreshToken?> EnsureRefreshTokenExists(User user)
        {
            if(user.RefreshTokens.Any(t => t.IsActive))
            {
                return user.RefreshTokens.FirstOrDefault(t => t.IsActive);
            }

            var refreshToken = await GenerateRefreshToken();
            user.RefreshTokens.Add(refreshToken);

            var result = await userManager.UpdateAsync(user);
            if(!result.Succeeded)
            {
                return null;
            }

            return refreshToken;
        }

        private async Task<bool> EnsureRoleExists(string role)
        {
            if (await roleManager.RoleExistsAsync(role) == false)
            {
                var roleCheck = await roleManager.CreateAsync(new IdentityRole
                {
                    Name = role
                });

                if (!roleCheck.Succeeded)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
