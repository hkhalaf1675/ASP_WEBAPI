namespace DBAssigment.DTOs
{
    public class EventDto
    {
        public string Subject { get; set; }
        public string? Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string? Location { get; set; }
    }
}
