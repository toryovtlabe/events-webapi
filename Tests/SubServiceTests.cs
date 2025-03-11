using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Business.Services;
using DataAccess.Entity;
using DataAccess.Repository.Service;
using DataAccess;
using FluentValidation;

public class SubServiceTests
{
    private readonly DbContextOptions<ApplicationContext> _dbContextOptions;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ApplicationContext _context;


    public SubServiceTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _mapperMock = new Mock<IMapper>();
        _context = new ApplicationContext(_dbContextOptions);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    private ApplicationContext CreateDbContext()
    {
        return new ApplicationContext(_dbContextOptions);
    }

    [Fact]
    public void CreateSub_ShouldThrowException_WhenEventIdIsInvalid()
    {
        using (var context = CreateDbContext())
        {
            var subRepository = new SubscriptionRepository(context);
            var eventRepository = new EventRepository(context);
            var userRepository = new UserRepository(context);
            var subService = new SubService(subRepository, eventRepository, userRepository, _mapperMock.Object);

            Assert.Throws<ValidationException>(() => subService.CreateSub(0, "userLogin"));
        }
    }

    [Fact]
    public void CreateSub_ShouldThrowException_WhenUserLoginIsNull()
    {
        using (var context = CreateDbContext())
        {
            var subRepository = new SubscriptionRepository(context);
            var eventRepository = new EventRepository(context);
            var userRepository = new UserRepository(context);
            var subService = new SubService(subRepository, eventRepository, userRepository, _mapperMock.Object);

            Assert.Throws<ArgumentException>(() => subService.CreateSub(1, null));
        }
    }

    [Fact]
    public void CreateSub_ShouldThrowException_WhenEventOrUserNotFound()
    {
        using (var context = CreateDbContext())
        {
            var subRepository = new SubscriptionRepository(context);
            var eventRepository = new EventRepository(context);
            var userRepository = new UserRepository(context);
            var subService = new SubService(subRepository, eventRepository, userRepository, _mapperMock.Object);

            Assert.Throws<DirectoryNotFoundException>(() => subService.CreateSub(1, "userLogin"));
        }
    }

    [Fact]
    public void CreateSub_ShouldThrowException_WhenUserAlreadySubscribed()
    {
        using (var context = CreateDbContext())
        {
            var user = new User
            {
                Subscriptions = new List<Subscription>(),
                BirthDay = new DateOnly(1990, 1, 1),
                Email = "test@mail.ru",
                FirstName = "test",
                Surname = "test",
                Login = "userLogin",
                Password = "test",
                IsAdmin = false,
                RefreshToken = "123a12f"
            };

            var userRepository = new UserRepository(context);
            userRepository.Create(user);
            context.SaveChanges();

            var subRepository = new SubscriptionRepository(context);
            var eventRepository = new EventRepository(context);
            var subService = new SubService(subRepository, eventRepository, userRepository, _mapperMock.Object);

            Assert.Throws<DirectoryNotFoundException>(() => subService.CreateSub(1, "userLogin"));
        }
    }

    [Fact]
    public void CreateSub_ShouldCreateSubscription_WhenValid()
    {
        using (var context = CreateDbContext())
        {
            var user = new User
            {
                Subscriptions = new List<Subscription>(),
                BirthDay = new DateOnly(1990, 1, 1),
                Email = "test@mail.ru",
                FirstName = "test",
                Surname = "test",
                Login = "userLogin",
                Password = "test",
                IsAdmin = false,
                RefreshToken = "123a12f"
            };

            var place = new Place
            {
                Name = "test place",
                Event = new List<Event>()
            };

            var category = new Category
            {
                Name = "test category",
                Events = new List<Event>()
            };

            var @event = new Event
            {
                Name = "test",
                Description = "testdesc",
                MaxParticipants = 10,
                Categories = new List<Category> { category },
                Subscriptions = new List<Subscription>(),
                Date = new DateTime(2025, 1, 1),
                Image = "test",
                Place = place
            };

            var subscription = new Subscription
            {
                RegistrationDate = new DateTime(2025, 1, 1),
                Event = @event,
                User = user
            };
                        
            var subRepository = new SubscriptionRepository(context);
            var eventRepository = new EventRepository(context);
            var userRepository = new UserRepository(context);
            userRepository.Create(user);
            eventRepository.Create(@event);
            context.SaveChanges();

            var subService = new SubService(subRepository, eventRepository, userRepository, _mapperMock.Object);

            var result = subService.CreateSub(1, "userLogin");
            subRepository.Save();

            Assert.Single(subRepository.GetAll());
        }
    }

    [Fact]
    public void GetSubsById_ShouldThrowException_WhenEventIdIsInvalid()
    {
        using (var context = CreateDbContext())
        {
            var subRepository = new SubscriptionRepository(context);
            var eventRepository = new EventRepository(context);
            var userRepository = new UserRepository(context);
            var subService = new SubService(subRepository, eventRepository, userRepository, _mapperMock.Object);

            Assert.Throws<ValidationException>(() => subService.GetSubsById(0));
        }
    }

    [Fact]
    public void GetSubsById_ShouldThrowException_WhenEventNotFound()
    {
        using (var context = CreateDbContext())
        {
            var subRepository = new SubscriptionRepository(context);
            var eventRepository = new EventRepository(context);
            var userRepository = new UserRepository(context);
            var subService = new SubService(subRepository, eventRepository, userRepository, _mapperMock.Object);

            Assert.Throws<DirectoryNotFoundException>(() => subService.GetSubsById(1));
        }
    }

    [Fact]
    public void GetUserById_ShouldThrowException_WhenUserNotFound()
    {
        using (var context = CreateDbContext())
        {
            var subRepository = new SubscriptionRepository(context);
            var eventRepository = new EventRepository(context);
            var userRepository = new UserRepository(context);
            var subService = new SubService(subRepository, eventRepository, userRepository, _mapperMock.Object);

            Assert.Throws<DirectoryNotFoundException>(() => subService.GetUserById(1));
        }
    }

    [Fact]
    public void GetUserById_ShouldReturnUser_WhenValid()
    {
        using (var context = CreateDbContext())
        {
            var user = new User
            {
                Id = 1,
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

            var subRepository = new SubscriptionRepository(context);
            var eventRepository = new EventRepository(context);
            var userRepository = new UserRepository(context);

            userRepository.Create(user);
            userRepository.Save();

            var subService = new SubService(subRepository, eventRepository, userRepository, _mapperMock.Object);

            var result = userRepository.GetById(1);

            Assert.Equal(1, result.Id);
        }
    }


    [Fact]
    public void DeleteSub_ShouldThrowException_WhenEventIdIsInvalid()
    {
        using (var context = CreateDbContext())
        {
            var subRepository = new SubscriptionRepository(context);
            var eventRepository = new EventRepository(context);
            var userRepository = new UserRepository(context);
            var subService = new SubService(subRepository, eventRepository, userRepository, _mapperMock.Object);

            Assert.Throws<ValidationException>(() => subService.DeleteSub(0, "userLogin"));
        }
    }

    [Fact]
    public void DeleteSub_ShouldThrowException_WhenUserLoginIsNull()
    {
        using (var context = CreateDbContext())
        {
            var subRepository = new SubscriptionRepository(context);
            var eventRepository = new EventRepository(context);
            var userRepository = new UserRepository(context);
            var subService = new SubService(subRepository, eventRepository, userRepository, _mapperMock.Object);

            Assert.Throws<ArgumentException>(() => subService.DeleteSub(1, null));
        }
    }

    [Fact]
    public void DeleteSub_ShouldThrowException_WhenEventOrUserNotFound()
    {
        using (var context = CreateDbContext())
        {
            var subRepository = new SubscriptionRepository(context);
            var eventRepository = new EventRepository(context);
            var userRepository = new UserRepository(context);
            var subService = new SubService(subRepository, eventRepository, userRepository, _mapperMock.Object);

            Assert.Throws<DirectoryNotFoundException>(() => subService.DeleteSub(1, "userLogin"));
        }
    }


}