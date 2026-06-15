using Microsoft.AspNetCore.Mvc;
using YchebProejkt.Data;
using YchebProejkt.dto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YchebProejkt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagmentController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ManagmentController(AppDbContext db)
        {
            _db = db;
        }
        // GET: api/<ManagmentController>
        [HttpGet]
        public IEnumerable<Management> Get()
        {
            return _db.Managements;
        }

        // GET api/<ManagmentController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ManagmentController>
        [HttpPost]
        public void Post(CreateManagementDto dto)
        {
            var management = new Management
            {
                Name = dto.Name,
            };

            _db.Managements.Add(management);
            _db.SaveChanges();
        }


        // PUT api/<ManagmentController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<ManagmentController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
