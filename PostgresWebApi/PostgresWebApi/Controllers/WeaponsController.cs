using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using Data.Repositories;
using Data.Errors;

namespace PostgresWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeaponsController : ControllerBase
    {
        //private readonly IConfiguration _configuration;
        private readonly IRepository<Weapon> context;

        public WeaponsController(/*IConfiguration configuration,*/ IRepository<Weapon> context)
        {
            //_configuration = configuration;
            this.context = context;
        }

        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return new JsonResult(context.GetAll());
            }
            catch 
            {
                return Problem();
            }
        }

        [HttpGet("{id}")]
        public ActionResult Get([FromRoute] long id)
        {
            try 
            {
                return new JsonResult(context.GetById(id));
            } 
            catch (RecordNotFoundException)
            {
                return NotFound();
            }            
            catch
            {
                return Problem();
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] Weapon weapon)
        {
            try
            {
                return new JsonResult(new ObjectId(context.Add(weapon)));
            }
            catch
            {
                return Problem();
            }
        }

        [HttpPut]
        public ActionResult Put([FromBody] Weapon weapon)
        {
            try
            {
                if (context.Update(weapon))
                    return Ok();
                return Problem();
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] long id)
        {
            try
            {
                if (context.Delete(id))
                    return Ok();
                return Problem();
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
