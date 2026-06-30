namespace YchebProejkt
{
    public class Instruction
    {
        public int Id { get; set; } 
        public required string Title { get; set; } = "";
        public string FilePath { get; set; } = string.Empty;
        public string FileHash { get; set; } = "";
        public string ContentType { get; set; } = "";
        public DateOnly UploadDate { get; set; }
        public int RegistryId { get; set; }
        public Registry? Registry { get; set; }
    }
    public class Registry
    {
        public int Id { get; set; }
        public required string Name { get; set; } = "";
        public required int ManagementId { get; set; }
        public Management? Management { get; set; }
        public List<Instruction> Instructions { get; set; } = new();
    }
    public class Management
    {
        public int Id { get; set; }
        public required string Name { get; set; } = "";
        public List<Registry> Registries { get; set; } = new();
    }
}
