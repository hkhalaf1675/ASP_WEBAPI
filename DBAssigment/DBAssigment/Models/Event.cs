using Google.Apis.Calendar.v3.Data;

namespace DBAssigment.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string? Description { get; set; }
        public EventDateTime Start { get; set; }
        public EventDateTime End { get; set; }
        public string? Location { get; set; }

        public Event()
        {
            this.Start.TimeZone = "Africa/ Cairo";
            this.End.TimeZone = "Africa/ Cairo";
        }
    }
}
