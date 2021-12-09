using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoMapper;
using WebAPI.AutoMapper;

namespace Tests.Extensions
{
    public abstract class Tester
    {
        protected readonly Fixture Fixture;
        protected readonly IMapper Mapper;

        protected Tester()
        {
            Fixture = new Fixture();
            Fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            Mapper = new MapperConfiguration(x => x.AddProfile(typeof(MappingProfile))).CreateMapper();
        }

        protected IQueryable<T> CreateQueryable<T>(int count = 5) where T : class
        {
            return Fixture.CreateMany<T>(count).AsQueryable();
        }

        protected IEnumerable<T> CreateEnumerable<T>(int count = 5) where T : class
        {
            return Fixture.CreateMany<T>(count).AsQueryable();
        }
    }
}