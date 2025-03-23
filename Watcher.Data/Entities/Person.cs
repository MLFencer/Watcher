namespace Watcher.Data.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public byte[] FaceEmbedding { get; set; }
        public string Name { get; set; } = "Unknown";
    }
}
