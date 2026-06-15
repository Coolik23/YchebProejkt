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
        public IEnumerable<Registry> Get()
        {
            return _db.Registries;
        }

        // GET api/<RegistryController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RegistryController>
        [HttpPost]
        public void Post(CreateRegistryDto dto)
        {
            var registry = new Registry
            {
                Name = dto.Name,
                ManagementId = dto.ManagementId,
            };

            _db.Registries.Add(registry);
            _db.SaveChanges();
        }

        // PUT api/<RegistryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<RegistryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
