using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vault.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Vault.API.Infrastructure.JWT;

namespace Vault.API.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService service) : base()
        {
            _authService = service;
        }

        [HttpPost("")]
        [AllowAnonymous]
        public async Task Login([FromBody] dynamic loginData)
        {
            var login = (string)loginData.login;
            var password = (string)loginData.password;

            var identity = await _authService.Login(login, password);

            if(identity == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid username or password.");
                return;
            }

            var token = JwtHelper.CreateToken(identity);
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(token, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }
    }
}
