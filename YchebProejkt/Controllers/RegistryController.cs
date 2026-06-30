using Microsoft.AspNetCore.Mvc;
using YchebProejkt.Data;
using YchebProejkt.dto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YchebProejkt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistryController : ControllerBase
    {
        private readonly AppDbContext _db;
        public RegistryController(AppDbContext db)
        {
            _db = db;
        }


        // GET: api/<RegistryController>
        [HttpGet]
        public IActionResult GetRegistries()
        {
            return Ok(_db.Registries.ToList());
        }



        // GET api/<RegistryController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}



        // POST api/<RegistryController>
        [HttpPost]
        public IActionResult Post([FromForm] CreateRegistryDto dto)
        {
            var registry = new Registry
            {
                Name = dto.Name,
                ManagementId = dto.ManagementId,
            };

            _db.Registries.Add(registry);
            _db.SaveChanges();

            return Ok(registry);
        }


        // PUT api/<RegistryController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}




        // DELETE api/<RegistryController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var registry = _db.Registries.FirstOrDefault(r => r.Id == id);

            if (registry == null)
                return NotFound();

            if (_db.Instructions.Any(i => i.RegistryId == id))
                return BadRequest("В регистре есть инструкции");

            _db.Registries.Remove(registry);
            _db.SaveChanges();

            return Ok();
        }
    }
}
