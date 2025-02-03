using BlazorEventAppWebAssembly.Models;

namespace BlazorEventAppWebAssembly.Services
{
    public class EventService
    {
        private readonly List<Event> events = new List<Event>();

        public Task<List<Event>> GetFutureEvents()
        {
            var futureEvents = events.Where(e => e.StartDate >= DateTime.Now).ToList();
            return Task.FromResult(futureEvents);
        }

        public Task AddEvent(Event newEvent)
        {
            newEvent.Id = events.Count > 0 ? events.Max(e => e.Id) + 1 : 1;
            events.Add(newEvent);
            return Task.CompletedTask;
        }
    }
}
