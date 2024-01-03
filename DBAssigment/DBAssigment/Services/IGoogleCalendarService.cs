using DBAssigment.DTOs;

namespace DBAssigment.Services
{
    public interface IGoogleCalendarService
    {
        
        Task<EventDto> GetEventAsync(int eventId);
        Task<bool> CreateEventAsync(EventDto eventDto);
        Task<bool> DeleteEventAsync(int eventId);
    }
}
