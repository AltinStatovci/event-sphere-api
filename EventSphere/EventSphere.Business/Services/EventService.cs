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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using Microsoft.Extensions.Logging;
using EventSphere.Business.Helper;
using Azure;
using EventSphere.Business.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace EventSphere.Business.Services
{

    public class EventService : IEventService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<EventCategory> _eventCategoryRepository;
        private readonly IGenericRepository<Location> _locationRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IHubContext<TicketHub> _hubContext;

        public EventService(
            IEventRepository eventRepository,
            IGenericRepository<User> userRepository,
            IGenericRepository<EventCategory> eventCategoryRepository,
            IGenericRepository<Location> locationRepository,
            IHubContext<TicketHub> hubContext)
        {
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _locationRepository = locationRepository;
            _hubContext = hubContext;
        }

        public async Task<IEnumerable<Event>> GetEventsByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return await _eventRepository.GetAllAsync();
            }

            return await _eventRepository.GetAsync(e => e.EventName.Contains(name));
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
                string base64Image = await ResizeAndConvertToBase64Async(image);

                var user = await _userRepository.GetByIdAsync(eventDto.OrganizerID);
                var userName = user.Name;
                var category = await _eventCategoryRepository.GetByIdAsync(eventDto.CategoryID);
                var categoryName = category.CategoryName;
                var location = await _locationRepository.GetByIdAsync(eventDto.LocationId);

                var events = new Event
                {
                    EventName = eventDto.EventName,
                    Description = eventDto.Description,
                    Address = eventDto.Address,
                    Location = location,
                    StartDate = eventDto.StartDate,
                    EndDate = eventDto.EndDate,
                    CategoryID = eventDto.CategoryID,
                    Category = category,
                    CategoryName = categoryName,
                    OrganizerID = eventDto.OrganizerID,
                    Organizer = user,
                    OrganizerName = userName,
                    PhotoData = base64Image,
                    MaxAttendance = eventDto.MaxAttendance,
                    AvailableTickets = eventDto.AvailableTickets,
                    IsApproved = eventDto.IsApproved,
                    ScheduleDate = eventDto.ScheduleDate
                };

                await _eventRepository.AddAsync(events);
                return events;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while creating the event.", ex);
            }
        }


        public async Task<string> ResizeAndConvertToBase64Async(IFormFile image)
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
                    memoryStream.Position = 0;

                    using (var img = Image.Load<Rgba32>(memoryStream))
                    {
                        img.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new Size(500, 500),
                            Mode = ResizeMode.Max
                        }));

                        using (var outputMemoryStream = new MemoryStream())
                        {
                            img.Save(outputMemoryStream, new JpegEncoder { Quality = 100 });

                            byte[] imageBytes = outputMemoryStream.ToArray();
                            string base64String = Convert.ToBase64String(imageBytes);
                            return base64String;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while resizing and converting the image to Base64.", ex);
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
                eventById.LocationId = eventDto.LocationId;
                eventById.StartDate = eventDto.StartDate;
                eventById.EndDate = eventDto.EndDate;
                eventById.CategoryID = eventDto.CategoryID;
                eventById.OrganizerID = eventDto.OrganizerID;
                eventById.MaxAttendance = eventDto.MaxAttendance;
                eventById.AvailableTickets = eventDto.AvailableTickets;
                eventById.DateCreated = eventDto.DateCreated;

                if (newImage != null)
                {
                    string base64Image = await ResizeAndConvertToBase64Async(newImage);
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

        public async Task<IEnumerable<Event>> GetEventByCategoryIdAsync(int eventCategoryId)
        {
            return await _eventRepository.GetEventByCategoryId(eventCategoryId);
        }

        public async Task<IEnumerable<Event>> GetEventByOrganizerIdAsync(int organizerId)
        {
            return await _eventRepository.GetEventByOrganizerId(organizerId);
        }

        public async Task<IEnumerable<Event>> GetEventsByCityAsync(string city)
        {
            return await _eventRepository.GetEventsByCity(city);
        }

        public async Task<IEnumerable<Event>> GetEventsByCountryAsync(string country)
        {
            return await _eventRepository.GetEventsByCountry(country);
        }

        public async Task<Event> UpdateEventStatus(int id)
        {
            var eventById = await _eventRepository.GetByIdAsync(id);
            try
            {
                eventById.IsApproved = true;
                eventById.Message = "Approved";

                await _eventRepository.UpdateAsync(eventById);

                return eventById;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while approving the event.", ex);
            }
        }
        public async Task<Event> UpdateEventStatusToDisapproved(int id)
        {
            var eventById = await _eventRepository.GetByIdAsync(id);
            try
            {
                eventById.IsApproved = false;
                eventById.Message = "Disapproved";

                await _eventRepository.UpdateAsync(eventById);

                return eventById;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while disapproving the event.", ex);
            }
        }
        public async Task<IEnumerable<Event>> GetEventsByDateAsync(DateTime date)
        {
            return await _eventRepository.GetEventsByDate(date);
        }
        public async Task<IEnumerable<Event>> GetEventsByDateTimeAsync(DateTime date)
        {
            return await _eventRepository.GetEventsByDateTime(date);
        }

        public async Task<string> GetOrganizerEmailAsync(int id)
        {
            return await _eventRepository.GetOrganizerEmail(id);
  
        }

        public async Task UpdateMessage(int id, string message)
        {
            var eventById = await _eventRepository.GetByIdAsync(id);
            eventById.Message = message;
            await _eventRepository.UpdateAsync(eventById);
        }
        public async Task<IEnumerable<Event>> GetEventsNearbyAsync(double latitude, double longitude)
        {
            return await _eventRepository.GetEventsNearbyAsync(latitude, longitude, 20);
        }
        public async Task UpdateAvailableTickets(int eventId, int newTicketCount)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveTicketCountUpdate", eventId, newTicketCount);
        }
    }  

}
