using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Calendar.v3;
using GoogleCalendarEvents.DTOs;

namespace GoogleCalendarEvents.IServices
{
    public interface IGoogleServices
    {
        Task<TokenResponse?> GetTokenAsync();
        Task<CalendarService> GetCalendarServiceAsync();
        Task<ResponseMessageDto?> AddCalendarEvent(EventRequestDto eventDto);
        Task<List<EventResponseDto>> GetAllCalendarEventsAsync();
        Task<EventResponseDto?> GetCalendarEventById(string id);
        Task<ResponseMessageDto> DeleteCalendarEvent(string id);
        Task<List<EventResponseDto>> SearchCalendarEvents(QueryParamsDto queryParams);
    }
}
