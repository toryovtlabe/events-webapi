using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Entity;
using DataAccess.Repository.Service;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    public class SubRepositoryTests
    {
        private readonly DbContextOptions<ApplicationContext> _dbContextOptions;
        private readonly ApplicationContext _context;

        public SubRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new ApplicationContext(_dbContextOptions);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        private ApplicationContext CreateDbContext()
        {
            return new ApplicationContext(_dbContextOptions);
        }

        [Fact]
        public void AddSubscription_ShouldAddSubscriptionToRepository()
        {
            using (var context = CreateDbContext())
            {
                var user = new User
                {
                    FirstName = "test",
                    Surname = "test",
                    BirthDay = new DateOnly(1990, 1, 1),
                    Email = "test@mail.ru",
                    Login = "userLogin",
                    Password = "test",
                    IsAdmin = false,
                    RefreshToken = "123a12f",
                    Subscriptions = new List<Subscription>()
                };

                var @event = new Event
                {
                    Name = "test",
                    Description = "testdesc",
                    MaxParticipants = 10,
                    Categories = new List<Category>(),
                    Subscriptions = new List<Subscription>(),
                    Date = new DateTime(2025, 1, 1),
                    Image = "test",
                    Place = new Place { Name = "test place" }
                };

                var subscription = new Subscription
                {
                    RegistrationDate = new DateTime(2025, 1, 1),
                    User = user,
                    Event = @event
                };

                var subRepository = new SubscriptionRepository(context);
                var eventRepository = new EventRepository(context);
                var userRepository = new UserRepository(context);

                userRepository.Create(user);
                eventRepository.Create(@event);
                subRepository.Create(subscription);
                context.SaveChanges();

                var savedSubscription = subRepository.GetById(subscription.Id);

                Assert.NotNull(savedSubscription);
                Assert.Equal(subscription.Id, savedSubscription.Id);
            }
        }


        [Fact]
        public void DeleteSubscription_ShouldRemoveSubscriptionFromRepository()
        {
            using (var context = CreateDbContext())
            {
                var user = new User
                {
                    FirstName = "test",
                    Surname = "test",
                    BirthDay = new DateOnly(1990, 1, 1),
                    Email = "test@mail.ru",
                    Login = "userLogin",
                    Password = "test",
                    IsAdmin = false,
                    RefreshToken = "123a12f",
                    Subscriptions = new List<Subscription>()
                };

                var @event = new Event
                {
                    Name = "test",
                    Description = "testdesc",
                    MaxParticipants = 10,
                    Categories = new List<Category>(),
                    Subscriptions = new List<Subscription>(),
                    Date = new DateTime(2025, 1, 1),
                    Image = "test",
                    Place = new Place { Id = 1, Name = "test place" }
                };

                var subscription = new Subscription
                {
                    RegistrationDate = new DateTime(2025, 1, 1),
                    User = user,
                    Event = @event
                };

                var subRepository = new SubscriptionRepository(context);
                var eventRepository = new EventRepository(context);
                var userRepository = new UserRepository(context);

                userRepository.Create(user);
                eventRepository.Create(@event);
                context.SaveChanges();
                subRepository.Create(subscription);
                context.SaveChanges();
                subRepository.Delete(subscription);
                context.SaveChanges();

                var deletedSubscription = subRepository.GetById(1);

                Assert.Null(deletedSubscription);
            }
        }
    }
}
