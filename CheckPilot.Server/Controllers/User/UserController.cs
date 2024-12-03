using Microsoft.AspNetCore.Mvc;
using System.Net;

using CheckPilot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CheckPilot.Server.Repository;

namespace Checkpilot.Server.Controllers
{
    //[Authorize]
    [Route("api/[controller]")] 
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository<User, int> _controllerRepository;

        public UserController(IUserRepository<User, int> UserRepository)
        {
            _controllerRepository = UserRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            try
            {
                var entities = _controllerRepository.GetList();
                return Ok(entities);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            try
            {
                var entity = _controllerRepository.GetByKey(id);
                if (entity == null)
                {
                    return NotFound();
                }
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post([FromBody] User entity)
        {
            try
            {
                entity = await _controllerRepository.AddAsync(entity);
                return CreatedAtAction(nameof(Get), new { id = entity.UserId }, entity);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Put(int id, [FromBody] User entity)
        {
            try
            {
                var updatedEntity = await _controllerRepository.UpdateAsync(entity);
                return Ok(updatedEntity);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var entity = _controllerRepository.GetByKey(id);
                if (entity == null)
                {
                    return NotFound();
                }

                _controllerRepository.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    } 
}
