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



        // POST api/<ManagmentController>
        [HttpPost]
        public void Post([FromForm] CreateManagementDto dto)
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
        public IActionResult Put(int id, [FromBody] string newName)
        {
            var management = _db.Managements.FirstOrDefault(r => r.Id == id);

            if (management == null)
                return NotFound();

            management.Name = newName;
            return Ok();
        }



        // DELETE api/<ManagmentController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var management = _db.Managements.FirstOrDefault(r => r.Id == id);

            if (management == null)
                return NotFound();

            if (_db.Registries.Any(i => i.ManagementId == id))
                return BadRequest("Управление содержит регистры");

            _db.Managements.Remove(management);
            _db.SaveChanges();

            return Ok();
        }
    }
}
