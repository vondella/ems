using Bogus;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.TicketTypes;
using Evently.Modules.Events.test.Abstraction;
using FluentAssertions;

namespace Evently.Modules.Events.test.TicketTypes;

public class TicketTypesTests:BaseTest
{
    [Fact]
    public void Create_ShouldReturnValue_WhenTicketTypesIsCreated()
    {
        // arrange
        var category = Category.Create(faker.Music.Genre());
        DateTime startAtUtc = DateTime.UtcNow;
        var responseWrapper = Event.Create(category, faker.Music.Genre(),
            faker.Music.Genre(),
            faker.Address.StreetAddress(),startAtUtc,null);
        // act
        var result = TicketType.Create(responseWrapper.ResponseData, faker.Music.Genre(),
            faker.Random.Decimal(), faker.Random.String(), faker.Random.Decimal());

        result.Should().NotBeNull();
    }
    [Fact]
    public void UpdatePrice_ShouldRaiseDomainEvent_WhenTicketTypesIsUpdated()
    {
        // arrange
        var category = Category.Create(faker.Music.Genre());
        DateTime startAtUtc = DateTime.UtcNow;
        var responseWrapper = Event.Create(category, faker.Music.Genre(),
            faker.Music.Genre(),
            faker.Address.StreetAddress(), startAtUtc, null);
        // act
        var result = TicketType.Create(responseWrapper.ResponseData, faker.Music.Genre(),
            faker.Random.Decimal(), faker.Random.String(), faker.Random.Decimal());

        var ticketType = result;
        ticketType.UpdatePrice(faker.Random.Decimal());

        // assert

        TicketTypePriceChangedDomainEvent ticketTypePriceChangedDomain =
            AssertDomainEventWasPublished<TicketTypePriceChangedDomainEvent>(ticketType);
        ticketTypePriceChangedDomain.TicketTypeId.Should().Be(ticketType.Id);
    }

}