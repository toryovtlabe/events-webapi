using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using DataAccess.Entity;
using DataAccess.Repository.Service;
using DataAccess.Repository.Services;
using FluentValidation;

namespace Business.Services
{
    public class EventService(EventRepository eventRepository,CategoryRepository categoryRepository, PlaceRepository placeRepository, IMapper mapper, IValidator<EventDTO> eventValidator, IWebRootPath webRootPath)
    {
        private readonly EventRepository _eventRepository = eventRepository;
        private readonly CategoryRepository _categoryRepository = categoryRepository;
        private readonly PlaceRepository _placeRepository = placeRepository;

        private readonly IMapper _mapper = mapper;
        private readonly IValidator<EventDTO> _eventValidator = eventValidator;
        private readonly IWebRootPath _webRootPath = webRootPath;

        public IEnumerable<EventDTO> GetEventsWithFilters(FiltersEventDTO filtersEventDTO)
        {
            IEnumerable<EventDTO> events = _eventRepository
                .GetEventsWithFilters(
                    filtersEventDTO.Place, 
                    filtersEventDTO.Category,
                    filtersEventDTO.Name,
                    filtersEventDTO.Date,
                    filtersEventDTO.Page
                )
                .Select(e => _mapper.Map<EventDTO>(e));
            return events;
        }

        public int GetPages(FiltersEventDTO filtersEventDTO)
        {
            return _eventRepository.GetPages(
                filtersEventDTO.Place,
                filtersEventDTO.Category,
                filtersEventDTO.Name,
                filtersEventDTO.Date
                );
        }

        public EventDTO GetById(int id)
        {
            EventDTO? ev = _mapper.Map<EventDTO>(_eventRepository.GetById(id));
            if (ev == null)
            {
                throw new DirectoryNotFoundException("Событие не найдено");
            }
            return ev;
        }

        public EventDTO CreateEvent(EventDTO eventDTO)
        {

            FluentValidation.Results.ValidationResult result = _eventValidator.Validate(eventDTO);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
            
            Event @event = _mapper.Map<Event>(eventDTO);

            Place? place = _placeRepository.GetByName(eventDTO.Place);
            List<Place> places = _placeRepository.GetAll();
            if (place != null)
            {
                @event.Place = place;
            }
            else
            {
                _placeRepository.Create(@event.Place);
            }

            var dbCategories = _categoryRepository.GetAll().ToDictionary(c => c.Name, c => c);
            for (int i = 0;i<@event.Categories.Count; i++)
            {
                string categoryName = @event.Categories[i].Name;
                if (dbCategories.TryGetValue(categoryName, out var dbCategory))
                {
                    @event.Categories[i] = dbCategory;
                }
                else
                {
                    _categoryRepository.Create(@event.Categories[i]);
                }
            }
            try
            {
                _eventRepository.Create(@event);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибка при добавлении события");
            }
            _eventRepository.Save();
            return _mapper.Map<EventDTO>(@event);
        }

        public EventDTO UpdateEvent(int eventId, EventDTO eventDTO)
        {
            FluentValidation.Results.ValidationResult result = _eventValidator.Validate(eventDTO);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            Event? existingEvent = _eventRepository.GetById(eventId);
            if (existingEvent == null)
            {
                throw new InvalidOperationException("Событие с указанным ID не найдено.");
            }
            eventDTO.Id = eventId;
            existingEvent = _mapper.Map(eventDTO, existingEvent);

            Place? place = _placeRepository.GetByName(eventDTO.Name);
            if (place != null) {
                existingEvent.Place = place;
            }
            else
            {
                existingEvent.Place = new Place { Name = eventDTO.Name };
                _placeRepository.Create(existingEvent.Place);
            }

            var dbCategories = _categoryRepository.GetAll().ToDictionary(c => c.Name, c => c);
            for (int i = 0; i < existingEvent.Categories.Count; i++)
            {
                string categoryName = existingEvent.Categories[i].Name;
                if (dbCategories.TryGetValue(categoryName, out var dbCategory))
                {
                    existingEvent.Categories[i] = dbCategory;
                }
                else
                {
                    _categoryRepository.Create(existingEvent.Categories[i]);
                }
            }

            try
            {
                _eventRepository.Update(existingEvent);
            }
            catch (Exception ex) 
            {
                throw new InvalidOperationException("Ошибка при обновлении события");
            }
            _eventRepository.Save();
            return _mapper.Map<EventDTO>(existingEvent);
        }

        public void DeleteEvent(int id)
        {
            Event? @event = _eventRepository.GetById(id);
            _eventRepository.Delete(@event);
            _eventRepository.Save();
        }

        public async Task UploadImage(int id, string imageBase64)
        {
            var eventEntity = _eventRepository.GetById(id);
            if (eventEntity == null) throw new DirectoryNotFoundException("Событие не найдено.");

            if (!string.IsNullOrEmpty(imageBase64))
            {
                if (imageBase64.Contains(","))
                {
                    imageBase64 = imageBase64.Split(',')[1];
                }
                
                var filePath = Path.Combine(_webRootPath.RootPath.ToString(), "uploads", $"{Guid.NewGuid()}.jpg");
                try
                {
                    var bytes = Convert.FromBase64String(imageBase64);
                    await File.WriteAllBytesAsync(filePath, bytes);
                }
                catch
                {
                    throw new AggregateException("Неверный формат строки base64");
                }
                
                eventEntity.Image = "/uploads/" + Path.GetFileName(filePath);
                _eventRepository.Update(eventEntity);
                _eventRepository.Save();
            }
        }
    }
}
