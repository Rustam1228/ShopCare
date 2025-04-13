using AutoMapper;
using backend.Context;
using backend.Core.Dto.UserDto;
using backend.Core.JwtOp;
using backend.Repositories;
using backend.TokenGeneration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly RepositoryUser _user;        
        public HomeController(ApplicationDbContext context, IMapper mapper, IOptions<JwtOptions> options )
        {
            CreateTokin createTokin = new CreateTokin(options);
            _user = new RepositoryUser(context, mapper, createTokin);           
        }

        [HttpPost]
        [Route("AddUser")]

        public async Task<ActionResult> AddUser([FromBody] UserRegistries user)
        {
            var result = await _user.Add(user);

            if (result != string.Empty)
            {
                return NotFound(result);
            }
            return Ok(" Создана новая анкета ");
        }

        [HttpPatch]        
        [Route("Update")]

        public async Task<ActionResult> UpdateUser([FromRoute] UserUpdate user)
        {
            var result = await _user.Update(user);

            if (result != string.Empty)
            {
                return NotFound(result);
            }
            return Ok(" анкета обновлена");
        }

        [HttpDelete]
        [Route("Delite{login}")]
        public async Task<ActionResult> DeleteUser([FromRoute] string login )
        {
            var result = await _user.Delete(login);

            if (result != string.Empty)
            {
                return NotFound(result);
            }
            return Ok(" анкета Удалена ");
        }

        [HttpPost]

        public async Task<ActionResult> Token([FromBody] UserRegistries user)
        {            
            var result = await _user.GenerateToken(user.Login, user.Password);

            if (result == string.Empty) 
            {
                return NotFound("не правильный логин или пароль");
            }
            HttpContext.Response.Cookies.Append("test-Cookies",result);
            return Ok(result);
        }

    }
    
}
