namespace EventSphere.Domain.Entities
{
    public class Logg
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public string? MessageTemplate { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string? Level { get; set; }
        public string? Exception { get; set; }
        public string? Properties { get; set; }
    }
}