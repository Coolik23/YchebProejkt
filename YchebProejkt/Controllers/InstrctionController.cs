using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Data;
using System.Security.Cryptography;
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
        
        //вычисление хэша файла
        private async Task<string> ComputeHash(IFormFile file)
        {
            using var sha = SHA256.Create();
            using var stream = file.OpenReadStream();

            var hash = await sha.ComputeHashAsync(stream);
            return Convert.ToHexString(hash);
        }

        // GET: api/<InstructionController>
        //для тестов
        [HttpGet]
        public IEnumerable<Instruction> Get()
        {
            return _db.Instructions;
        }




        // GET api/<InstructionController>/5
        //[HttpGet("{id}")]
        //public Instruction Get(int id)
        //{

        //    return _db.Instructions.FirstOrDefault(i => i.Id == id); 
        //}





        [HttpGet("search")]
        //Поиск с опциями
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
        // тестовый пост, сейчас не используется
        //[HttpPost]
        //public void Post(CreateInstructionDto dto)
        //{
        //    var instruction = new Instruction
        //    {
        //        Title = dto.Title,
        //        RegistryId = dto.RegistryId,
        //        UploadDate = DateOnly.FromDateTime(DateTime.Now)
        //    };

        //    _db.Instructions.Add(instruction);
        //    _db.SaveChanges();
        //}




        // PUT api/<InstructionController>/5
        [HttpPut("{id}")]
        public void Rename(int id, [FromBody]string newtitle)
        {
            var instruction = _db.Instructions.FirstOrDefault(i => i.Id == id);
            if (instruction == null)
                return;
            instruction.Title = newtitle;
            _db.SaveChanges();

        }




        // DELETE api/<InstructionController>/5
        //Для удаления инструкции вместе с файлом
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var instruction = _db.Instructions.FirstOrDefault(i => i.Id == id);

            if (instruction == null)
                return NotFound();

            if (System.IO.File.Exists(instruction.FilePath))
            {
                System.IO.File.Delete(instruction.FilePath);
            }

            _db.Instructions.Remove(instruction);
            _db.SaveChanges();

            return Ok();
        }




        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] CreateInstructionUploadDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("Файла нет");

            var hash = await ComputeHash(dto.File);

            var directory = Path.Combine(Environment.CurrentDirectory, "Files");
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            // ищем уже существующий файл
            var existing = _db.Instructions.FirstOrDefault(x => x.FileHash == hash);

            string filePath;

            if (existing != null)
            {
                filePath = existing.FilePath;
            }
            else
            {
                var fileName = $"{hash}_{dto.File.FileName}";
                filePath = $"Files/{fileName}";

                var fullFilePath = Path.Combine(directory, fileName);

                using (var fileStream = System.IO.File.Create(fullFilePath))
                {
                    await dto.File.CopyToAsync(fileStream);
                }
            }

            var instruction = new Instruction
            {
                Title = dto.Title,
                FilePath = filePath,
                ContentType = dto.File.ContentType,
                UploadDate = DateOnly.FromDateTime(DateTime.Now),
                RegistryId = dto.RegistryId,
                FileHash = hash
            };

            _db.Instructions.Add(instruction);
            await _db.SaveChangesAsync();

            return Ok(instruction);
        }



        [HttpGet("download")]
        //скачивать файлы
        public async Task<IActionResult> Download(int instructionId)
        {
            var instruction = await _db.Instructions.FirstOrDefaultAsync(i => i.Id == instructionId);
            if (instruction == null) return NotFound("Инструкция не найдена");
            if (!System.IO.File.Exists(instruction.FilePath)) return NotFound("Файла нет на диске");

            var bytes = await System.IO.File.ReadAllBytesAsync(instruction.FilePath);
            var fullPath = Path.Combine(Environment.CurrentDirectory, instruction.FilePath);

            return PhysicalFile(fullPath, instruction.ContentType, instruction.Title);
        }



    }
}
