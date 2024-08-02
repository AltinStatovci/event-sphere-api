using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSphere.Business.Services
{
    public class RCEventService : IRCEventService
    {
        private readonly IGenericRepository<RCEvent> _rcEventRepository;

        public RCEventService(IGenericRepository<RCEvent> rcEventRepository)
        {
            _rcEventRepository = rcEventRepository;
        }
        public async Task<RCEventDTO> GetRCEventByIdAsync(int id)
        {
            var rcEvent = await _rcEventRepository.GetByIdAsync(id);
            if (rcEvent == null) return null;

            return new RCEventDTO
            {
                Id = rcEvent.Id,
                UserId = rcEvent.UserId,
                EventId = rcEvent.EventId,
                Ecount = rcEvent.Ecount
            };
        }

        public async Task<IEnumerable<RCEventDTO>> GetRCEventsByUserIdAsync(int userId)
        {
            var rcEvents = await _rcEventRepository.FindAsync(r => r.UserId == userId);
            return rcEvents.Select(e => new RCEventDTO
            {
                Id = e.Id,
                UserId = e.UserId,
                EventId = e.EventId,
                Ecount = e.Ecount
            });
        }

        

        public async Task<RCEventDTO> CreateAsync(RCEventDTO rcEventDTO)
        {
            var rcEvent = new RCEvent
            {
                UserId = rcEventDTO.UserId,
                EventId = rcEventDTO.EventId,
                Ecount = rcEventDTO.Ecount
            };

            await _rcEventRepository.AddAsync(rcEvent);

            return new RCEventDTO
            {
                UserId = rcEvent.UserId,
                EventId = rcEvent.EventId,
                Ecount = rcEvent.Ecount
            };
        }

        public async Task UpdateAsync(int id, RCEventDTO rcEventDTO)
        {
            var rcEvent = await _rcEventRepository.GetByIdAsync(id);
            if (rcEvent == null) throw new KeyNotFoundException("RCEvent not found.");

            rcEvent.UserId = rcEventDTO.UserId;
            rcEvent.EventId = rcEventDTO.EventId;
            rcEvent.Ecount = rcEventDTO.Ecount;

            await _rcEventRepository.UpdateAsync(rcEvent);
        }

        public async Task DeleteAsync(int id)
        {
            await _rcEventRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<RCEventDTO>> GetAllRCEventsAsync()
        {
            var rcEvents = await _rcEventRepository.GetAllAsync();
            return rcEvents.Select(e => new RCEventDTO
            {
                Id = e.Id,
                UserId = e.UserId,
                EventId = e.EventId,
                Ecount = e.Ecount
            });
        }
        public async Task<RCEventDTO> GetRCEventByUserIdAndEventIdAsync(int userId, int eventId)
        {
            var rcEvent = await _rcEventRepository.FindAsync(r => r.UserId == userId && r.EventId == eventId);
            var rcEventEntity = rcEvent.FirstOrDefault();
            if (rcEventEntity == null) return null;

            return new RCEventDTO
            {
                Id = rcEventEntity.Id,
                UserId = rcEventEntity.UserId,
                EventId = rcEventEntity.EventId,
                Ecount = rcEventEntity.Ecount
            };
        }



    }
}





