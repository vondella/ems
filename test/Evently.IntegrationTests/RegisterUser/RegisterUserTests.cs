using Evently.IntegrationTests.Abstractions;
using Evently.Modules.Attendance.Application.Attendees.GetAttendee;
using Evently.Modules.Ticketing.Application.Customers.GetCustomer;
using Evently.Modules.Users.Application.Users.RegisterUser;
using FluentAssertions;

namespace Evently.IntegrationTests.RegisterUser;

public class RegisterUserTests : BaseIntegrationTest
{
    public RegisterUserTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task RegisterUser_Should_PropagateToTicketingModule()
    {
        // Register user
        var command = new RegisterUserCommand(
            Faker.Internet.Email(),
            Faker.Internet.Password(6),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        var  userResult = await Sender.Send(command);

        userResult.IsSuccessful.Should().BeTrue();

        // Get customer
        var  customerResult = await Poller.WaitAsync(
            TimeSpan.FromSeconds(15),
            async () =>
            {
                var query = new GetCustomerQuery(userResult.ResponseData);

                var  customerResult = await Sender.Send(query);

                return customerResult;
            });

        // Assert
        customerResult.IsSuccessful.Should().BeTrue();
        customerResult.ResponseData.Should().NotBeNull();
    }

    [Fact]
    public async Task RegisterUser_Should_PropagateToAttendanceModule()
    {
        // Register user
        var command = new RegisterUserCommand(
            Faker.Internet.Email(),
            Faker.Internet.Password(6),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        var  userResult = await Sender.Send(command);

        userResult.IsSuccessful.Should().BeTrue();

        // Get customer
        var  attendeeResult = await Poller.WaitAsync(
            TimeSpan.FromSeconds(15),
            async () =>
            {
                var query = new GetAttendeeQuery(userResult.ResponseData);

                var customerResult = await Sender.Send(query);

                return customerResult;
            });

        // Assert
        attendeeResult.IsSuccessful.Should().BeTrue();
        attendeeResult.ResponseData.Should().NotBeNull();
    }
}