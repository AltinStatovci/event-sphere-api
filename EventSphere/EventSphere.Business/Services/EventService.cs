using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventSphere.Business.Services.Interfaces;
using EventSphere.Domain.DTOs;
using EventSphere.Domain.Entities;
using EventSphere.Infrastructure;
using EventSphere.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EventSphere.Business.Services
{
    public class EventServiceBase
    {
        protected readonly EventSphereDbContext _context;

        public EventServiceBase(EventSphereDbContext context)
        {
            _context = context;
        }
    }

    public class EventService : EventServiceBase, IEventService
    {
        private readonly IGenericRepository<Event> _eventRepository;
        private readonly IGenericRepository<User> _userRepository;

        public EventService(EventSphereDbContext context, IGenericRepository<Event> eventRepository, IGenericRepository<User> userRepository) : base(context)
        {
            _eventRepository = eventRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _eventRepository.GetAllAsync();
        }

        public async Task<Event> GetEventsByIdAsync(int id)
        {
            return await _eventRepository.GetByIdAsync(id);
        }

        public async Task<Event> CreateEventsAsync(EventDTO eventDto, IFormFile image)
        {
            if (eventDto == null || image == null || image.Length == 0)
            {
                throw new ArgumentException("Event DTO or image is null or empty.");
            }

            try
            {
                string base64Image = await ConvertToBase64Async(image);

                var user = await _userRepository.GetByIdAsync(eventDto.OrganizerID);
                var userName = user.Name;

                var events = new Event
                {
                    EventName = eventDto.EventName,
                    Description = eventDto.Description,
                    Location = eventDto.Location,
                    StartDate = eventDto.StartDate,
                    EndDate = eventDto.EndDate,
                    CategoryID = eventDto.CategoryID,
                    OrganizerID = eventDto.OrganizerID,
                    Organizer = userName,
                    PhotoData = base64Image, 
                    MaxAttendance = eventDto.MaxAttendance,
                    AvailableTickets = eventDto.AvailableTickets,
                    DateCreated = eventDto.DateCreated
                };

                await _eventRepository.AddAsync(events);
                return events;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while creating the event.", ex);
            }
        }

        public static async Task<string> ConvertToBase64Async(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                throw new ArgumentException("Image file is null or empty.");
            }

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);

                    byte[] imageBytes = memoryStream.ToArray();

                    string base64String = Convert.ToBase64String(imageBytes);

                    return base64String;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while converting the image to Base64.", ex);
            }
        }

        public async Task<Event> UpdateEventsAsync(int id, EventDTO eventDto, IFormFile newImage)
        {
            var eventById = await _eventRepository.GetByIdAsync(id);
            if (eventById == null)
            {
                throw new ArgumentException($"Event with ID {id} not found.");
            }

            try
            {
                var eventPhotoData = eventById.PhotoData;

                eventById.EventName = eventDto.EventName;
                eventById.Description = eventDto.Description;
                eventById.Location = eventDto.Location;
                eventById.StartDate = eventDto.StartDate;
                eventById.EndDate = eventDto.EndDate;
                eventById.CategoryID = eventDto.CategoryID;
                eventById.OrganizerID = eventDto.OrganizerID;
                eventById.MaxAttendance = eventDto.MaxAttendance;
                eventById.AvailableTickets = eventDto.AvailableTickets;
                eventById.DateCreated = eventDto.DateCreated;

                if (newImage != null)
                {
                    string base64Image = await ConvertToBase64Async(newImage);
                    eventById.PhotoData = base64Image;
                }
                else
                {
                    eventById.PhotoData = eventPhotoData;
                }

                await _eventRepository.UpdateAsync(eventById);

                return eventById;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while updating the event.", ex);
            }
        }



        public async Task DeleteEventsAsync(int id)
        {
            await _eventRepository.DeleteAsync(id);
        }

        public async Task<int> GetEventCountAsync()
        {
            return await _eventRepository.CountAsync();
        }

        public async Task<IEnumerable<Event>> GetEventByCategoryId(int eventCategoryId)
        {
            return await _context.Events.Where(u => u.CategoryID == eventCategoryId).ToListAsync();
        }
    }
}
