namespace Watcher.Data.Entities
{
    public class EventLog
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
