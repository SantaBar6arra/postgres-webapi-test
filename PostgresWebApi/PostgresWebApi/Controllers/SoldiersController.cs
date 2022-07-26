using Data.Models;
using Data.Errors;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using Data.Repositories;

namespace PostgresWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SoldiersController : ControllerBase
    {
        //private readonly IConfiguration _configuration;
        private readonly IRepository<Soldier> context;

        public SoldiersController(/*IConfiguration configuration,*/ IRepository<Soldier> context)
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
        public ActionResult Post([FromBody] Soldier soldier)
        {
            try
            {
                return new JsonResult(new ObjectId(context.Add(soldier)));
            }
            catch
            {
                return Problem();
            }
        }

        [HttpPut]
        public ActionResult Put([FromBody] Soldier soldier)
        {
            try
            {
                if(context.Update(soldier))
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
                return NotFound();
            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
