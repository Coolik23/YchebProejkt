namespace YchebProejkt.dto
{
    public class CreateInstructionUploadDto
    {
        public string Title { get; set; } = "";
        public int RegistryId { get; set; }
        public IFormFile File { get; set; } = default!;
    }
}
