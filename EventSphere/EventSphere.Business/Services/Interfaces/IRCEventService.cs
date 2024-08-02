using EventSphere.Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventSphere.Business.Services.Interfaces
{
    public interface IRCEventService
    {
        Task<RCEventDTO> CreateAsync(RCEventDTO rcEventDTO);
        Task DeleteAsync(int id);
        Task<IEnumerable<RCEventDTO>> GetAllRCEventsAsync();
        Task<RCEventDTO> GetRCEventByIdAsync(int id);
        Task<IEnumerable<RCEventDTO>> GetRCEventsByUserIdAsync(int userId);
        Task UpdateAsync(int id, RCEventDTO rcEventDTO);
        Task<RCEventDTO> GetRCEventByUserIdAndEventIdAsync(int userId, int eventId);

    }
}
