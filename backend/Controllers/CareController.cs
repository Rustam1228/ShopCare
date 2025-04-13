using AutoMapper;
using backend.Context;
using backend.Core.Dto.CareDto;
using backend.Core.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CareController : ControllerBase
    {
        private readonly RepositoryCare _care;
        public CareController(ApplicationDbContext context, IMapper mapper)
        {
            _care = new RepositoryCare(context,mapper);
        }
                
        [HttpGet]
        [Route("Usrrr")]
        public async Task<ActionResult<List<User>>> GetUs()
        {
            var cares = await _care.GetUs();
            
            return Ok(cares);
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> AddCare([FromBody] CreateCare care)
        {
            var result = await _care.Create(care);

            if (result != string.Empty)
            {
                return NotFound(result);
            }
            return Ok(" Создана новая анкета ");
        }

        [HttpGet]
        [Route("Get")]              
        public async Task<ActionResult<List<GetCare>>> GetCare()
        {
            var cares = await _care.Get();
            if (cares == null)
            {
                return NotFound("Нет анкет");
            }
            return Ok(cares);
        }

        [HttpGet]
        [Route("Get{id}")]
        public async Task<ActionResult<GetCare>> GetCareId([FromRoute] Guid id)
        {
            var cares = await _care.GetId(id);
            if (cares == null)
            {
                return NotFound("Нет анкет");
            }
            return Ok(cares);
        }

        [HttpPatch]
        [Route("Update{id}")]
        public async Task<ActionResult> UpdateCare([FromRoute] Guid id, [FromBody] UpdateCare care)
        {
            var result = await _care.Update(id, care);

            if (result != string.Empty)
            {
                return NotFound(result);
            }
            return Ok(" Данные Обновлены ");
        }

        [HttpDelete]
        [Route("Delite{id}")]
        public async Task<ActionResult> DeleteCare([FromRoute] Guid id)
        {
            var result = await _care.Delete(id); 

            if (!result)
            {
                return NotFound("Введены не коренные данные");
            }

            return Ok(" Машина снята с продажи ");
        }
    }
}
