using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CommentForProductApi.Configuration;
using CommentForProductApi.Domain;
using CommentForProductApi.Models;
using CommentForProductApi.Models.Dto.Requests;
using CommentForProductApi.Models.Dto.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CommentForProductApi.Controllers
{
    [Route("api/[controller]")] // api/authmanagement
    [ApiController]
    public class AuthManagementController : ControllerBase
    {


        private readonly JwtConfig _jwtConfig;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly CommentForProductsContext _apiDbContext;

        public AuthManagementController(

            IOptionsMonitor<JwtConfig> optionsMonitor,
            TokenValidationParameters tokenValidationParameters,
            CommentForProductsContext apiDbContext)
        {
            _jwtConfig = optionsMonitor.CurrentValue;
            _tokenValidationParameters = tokenValidationParameters;
            _apiDbContext = apiDbContext;
        }

        public static string ComputeSHA256Hash(string text)
        {
            using (var sha256 = new SHA256Managed())
            {
                return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", "");
            }                
        }
        
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            if (ModelState.IsValid)
            {
                // check if the user with the same email exist
                var existingUser = await _apiDbContext.Users.FirstOrDefaultAsync(c=>c.Login==user.Login);

                if (existingUser == null)
                {
                    // We dont want to give to much information on why the request has failed for security reasons
                    return BadRequest(new RegistrationResponse()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Invalid authentication request"
                        }
                    });
                }

                // Now we need to check if the user has inputed the right password
                //СОНЯ ТВОЙ ВЫХОД!!!
                
                
                var isCorrect = existingUser.Password.Equals(ComputeSHA256Hash(user.Password));

                if (isCorrect)
                {
                    return Ok(await GenerateJwtToken(existingUser));
                }
                else
                {
                    // We dont want to give to much information on why the request has failed for security reasons
                    return BadRequest(new RegistrationResponse()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Invalid authentication request"
                        }
                    });
                }
            }


            return BadRequest(new RegistrationResponse()
            {
                Success = false,
                Errors = new List<string>()
                {
                    "Invalid payload"
                }
            });
        }
        //its ok)



        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto user)
        {
            // Check if the incoming request is valid
            if (ModelState.IsValid)
            {
                // check i the user with the same email exist
                var existingUser = await _apiDbContext.Users.FirstOrDefaultAsync(c=>c.Login==user.Login);

                if (existingUser != null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Email already exist"
                        }
                    });
                }

                var newUser = new User()
                {
                    Name = user.Name,
                    Surname = user.Surname,
                    Patronymic = user.Patronymic,
                    Password = ComputeSHA256Hash(user.Password),
                    Login = user.Login,
                    IdRole = 1
                };

                _apiDbContext.Users.Add(newUser);
                var isCreated = await _apiDbContext.SaveChangesAsync();
                if (isCreated == 1)
                {
                    return Ok(await GenerateJwtToken(newUser));
                }

                return new JsonResult(new RegistrationResponse()
                    {
                        Success = false,
                        Errors = new List<string> { "Возникли проблемы при создании аккаунта!" }
                    }
                ) { StatusCode = 500 };
            }

            return BadRequest(new RegistrationResponse()
            {
                Success = false,
                Errors = new List<string>()
                {
                    "Invalid payload"
                }
            });
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            if (ModelState.IsValid)
            {
                var res = await VerifyToken(tokenRequest);

                if (res == null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>()
                        {
                            "Invalid tokens"
                        },
                        Success = false
                    });
                }

                return Ok(res);
            }

            return BadRequest(new RegistrationResponse()
            {
                Errors = new List<string>()
                {
                    "Invalid payload"
                },
                Success = false
            });
        }

        private async Task<AuthResult> VerifyToken(TokenRequest tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // This validation function will make sure that the token meets the validation parameters
                // and its an actual jwt token not just a random string
                var principal = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters,
                    out var validatedToken);

                // Now we need to check if the token has a valid security algorithm
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)
                    {
                        return null;
                    }
                }

                // Will get the time stamp in unix time
                var utcExpiryDate =
                    long.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                // we convert the expiry date from seconds to the date
                var expDate = UnixTimeStampToDateTime(utcExpiryDate);

                if (expDate > DateTime.UtcNow)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "We cannot refresh this since the token has not expired" },
                        Success = false
                    };
                }

                // Check the token we got if its saved in the db
                var storedRefreshToken =
                    await _apiDbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

                if (storedRefreshToken == null)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "refresh token doesnt exist" },
                        Success = false
                    };
                }

                // Check the date of the saved token if it has expired
                if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "token has expired, user needs to relogin" },
                        Success = false
                    };
                }

                // check if the refresh token has been used
                if (storedRefreshToken.IsUsed)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "token has been used" },
                        Success = false
                    };
                }

                // Check if the token is revoked
                if (storedRefreshToken.IsRevoked)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "token has been revoked" },
                        Success = false
                    };
                }

                // we are getting here the jwt token id
                var jti = principal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                // check the id that the recieved token has against the id saved in the db
                if (storedRefreshToken.JwtId != jti)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "the token doenst mateched the saved token" },
                        Success = false
                    };
                }

                storedRefreshToken.IsUsed = true;
                _apiDbContext.RefreshTokens.Update(storedRefreshToken);
                await _apiDbContext.SaveChangesAsync();

                var dbUser = await _apiDbContext.Users.FindAsync(storedRefreshToken.IdUser);
                return await GenerateJwtToken(dbUser);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<AuthResult> GenerateJwtToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Login", user.Login),
                    new Claim(JwtRegisteredClaimNames.Sub, $"{user.Surname} {user.Name} {user.Patronymic}"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.Add(_jwtConfig.ExpiryTimeFrame),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsUsed = false,
                IdUser = user.Id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddYears(1),
                IsRevoked = false,
                Token = RandomString(25) + Guid.NewGuid()
            };

            await _apiDbContext.RefreshTokens.AddAsync(refreshToken);
            await _apiDbContext.SaveChangesAsync();

            return new AuthResult()
            {
                Token = jwtToken,
                Success = true,
                RefreshToken = refreshToken.Token
            };
        }

        public string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime;
        }
    }
}