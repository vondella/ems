using System.Reflection.Metadata;
using Bogus;
using Evently.Common.Domain;

namespace Evently.Modules.Events.test.Abstraction
{
    public abstract  class BaseTest
    {
        protected static Faker faker = new();

        public static T AssertDomainEventWasPublished<T>(Entity entity)
        where T: IDomainEvent
        {
            T? domainEvent = entity.DomainEvents.OfType<T>().SingleOrDefault();

            if (domainEvent is null)
            {
                throw new Exception($"{typeof(T).Name} was not published");
            }

            return domainEvent;
        }
    }
}