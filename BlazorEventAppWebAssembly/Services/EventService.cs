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

        public Task UpdateEvent(Event updatedEvent)
        {
            var existingEvent = events.FirstOrDefault(e => e.Id == updatedEvent.Id);
            if (existingEvent != null)
            {
                existingEvent.Name = updatedEvent.Name;
                existingEvent.Location = updatedEvent.Location;
                existingEvent.StartDate = updatedEvent.StartDate;
                existingEvent.EndDate = updatedEvent.EndDate;
                existingEvent.Description = updatedEvent.Description;
                existingEvent.EventType = updatedEvent.EventType;
                existingEvent.IsConfirmed = updatedEvent.IsConfirmed;
            }
            return Task.CompletedTask;
        }

        public Task DeleteEvent(int eventId)
        {
            var eventToDelete = events.FirstOrDefault(e => e.Id == eventId);
            if (eventToDelete != null)
            {
                events.Remove(eventToDelete);
            }
            return Task.CompletedTask;
        }

        public Task<Event> GetEventById(int eventId)
        {
            var existingEvent = events.FirstOrDefault(e => e.Id == eventId);
            return Task.FromResult(existingEvent);
        }

        public Task ConfirmEvent(int eventId) // P59fe
        {
            var existingEvent = events.FirstOrDefault(e => e.Id == eventId);
            if (existingEvent != null)
            {
                existingEvent.IsConfirmed = true;
            }
            return Task.CompletedTask;
        }

        public Task UnconfirmEvent(int eventId) // P357b
        {
            var existingEvent = events.FirstOrDefault(e => e.Id == eventId);
            if (existingEvent != null)
            {
                existingEvent.IsConfirmed = false;
            }
            return Task.CompletedTask;
        }
    }
}
