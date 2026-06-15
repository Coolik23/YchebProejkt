using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Data;
using YchebProejkt.Data;
using YchebProejkt.dto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YchebProejkt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructionController : ControllerBase
    {
        private readonly AppDbContext _db;
        public InstructionController(AppDbContext db)
        {
            _db = db;
        }
        // GET: api/<InstructionController>
        [HttpGet]
        public IEnumerable<Instruction> Get()
        {
            return _db.Instructions;
        }

        // GET api/<InstructionController>/5
        [HttpGet("{id}")]
        public Instruction Get(int id)
        {

            return _db.Instructions.FirstOrDefault(i => i.Id == id); 
        }
        [HttpGet("search")]
        public IActionResult Search(string? title, int? registryId, int? managementId, DateOnly? fromDate, DateOnly? toDate)
        {
            var query = _db.Instructions.AsQueryable();

            if (!string.IsNullOrEmpty(title))
                query = query.Where(i => i.Title.Contains(title));

            if (registryId.HasValue)
                query = query.Where(i => i.RegistryId == registryId);

            if (managementId.HasValue)
                query = query.Where(i => i.Registry!.ManagementId == managementId);

            if (fromDate.HasValue)
                query = query.Where(i => i.UploadDate >= fromDate);

            if (toDate.HasValue)
                query = query.Where(i => i.UploadDate <= toDate);

            return Ok(query.ToList());
        }
        // POST api/<InstructionController>
        [HttpPost]
        public void Post(CreateInstructionDto dto)
        {
            var instruction = new Instruction
            {
                Title = dto.Title,
                RegistryId = dto.RegistryId,
                UploadDate = DateOnly.FromDateTime(DateTime.Now)
            };

            _db.Instructions.Add(instruction);
            _db.SaveChanges();
        }

        // PUT api/<InstructionController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string newtitle)
        {
            var instruction = _db.Instructions.FirstOrDefault(i => i.Id == id);
            if (instruction == null)
                return;
            instruction.Title = newtitle;
            _db.SaveChanges();

        }

        // DELETE api/<InstructionController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file, int registryId, string title)
        {
            var directory = $"{Environment.CurrentDirectory}/Files";
            if (!Directory.Exists(directory)) 
            { 
                Directory.CreateDirectory(directory);
            }
            var fullFilePath = $"{directory}/{file.FileName}";
            if (!System.IO.File.Exists(fullFilePath))
            {
                using var fileStream = System.IO.File.Create(fullFilePath);
                await file.CopyToAsync(fileStream);
            }
            var instruction = new Instruction
            {
                Title = title,
                FilePath = fullFilePath,
                ContentType = file.ContentType,
                UploadDate = DateOnly.FromDateTime(DateTime.Now),
                RegistryId = registryId,
            };

            _db.Instructions.Add(instruction);
            await _db.SaveChangesAsync();

            return Ok(instruction);
        }
        [HttpGet("download")]
        public async Task<IActionResult> Download(int instructionId)
        {
            var instruction = await _db.Instructions.FirstOrDefaultAsync(i => i.Id == instructionId);
            if (instruction == null) return NotFound("Instruction not found");
            if (!System.IO.File.Exists(instruction.FilePath)) return NotFound("File missing on disk");

            var bytes = await System.IO.File.ReadAllBytesAsync(instruction.FilePath);

            return PhysicalFile(instruction.FilePath, instruction.ContentType, instruction.Title);
        }
    }
}
