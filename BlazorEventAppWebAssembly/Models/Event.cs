namespace BlazorEventAppWebAssembly.Models
{
    public class Event
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Description { get; set; }

        public EventType EventType { get; set; }
    }
}
