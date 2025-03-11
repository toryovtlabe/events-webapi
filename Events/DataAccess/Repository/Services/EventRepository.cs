using System.ComponentModel.DataAnnotations;
using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace DataAccess.Repository.Service
{
    public class EventRepository(ApplicationContext context) : Repository<Event>(context)
    {
        readonly ApplicationContext _context = context;
        public IEnumerable<Event> GetEventsWithFilters(string? place, string? category, string? name, DateTime? date, int page)
        {
            var query = _context.Set<Event>().Include(e => e.Subscriptions).Include(e => e.Categories).Include(e=>e.Place).AsQueryable();

            if (!string.IsNullOrEmpty(place))
            {
                query = query.Where(e => e.Place.Name.Contains(place));
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(e => e.Categories.Any(c => c.Name.Contains(category)));
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.Name.Contains(name));
            }

            if (date.HasValue)
            {
                query = query.Where(e => e.Date.Date == date.Value.ToUniversalTime().Date);
            }

            if (page <= 0) throw new ValidationException("Номер страницы должен быть больше 0");

            return query.Skip((page-1)*10).Take(10);
        }

        public new Event? GetById(int id)
        {
            return _context.Set<Event>().Include(e => e.Subscriptions).ThenInclude(s => s.User).Include(e => e.Categories).Include(e => e.Place).FirstOrDefault(e=> e.Id == id);
        }

        public int GetPages(string? place, string? category, string? name, DateTime? date)
        {
            var query = _context.Set<Event>().Include(e => e.Subscriptions).Include(e => e.Categories).Include(e => e.Place).AsQueryable();

            if (!string.IsNullOrEmpty(place))
            {
                query = query.Where(e => e.Place.Name.Contains(place));
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(e => e.Categories.Any(c => c.Name.Contains(category)));
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.Name.Contains(name));
            }

            if (date.HasValue)
            {
                query = query.Where(e => e.Date.Date == date.Value.ToUniversalTime().Date);
            }

            return query.Count();
        }
    }
}
