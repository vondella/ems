using System.Net;
using System.Net.Http.Json;
using Evently.Modules.Users.IntegrationTests.Abstractions;
using Evently.Modules.Users.Presentation.Users;
using FluentAssertions;

namespace Evently.Modules.Users.IntegrationTests.Users;

public class RegisterUserTests : BaseIntegrationTest
{
    public  RegisterUserTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    public static readonly TheoryData<string, string, string, string> InvalidRequests = new()
    {
        { "", Faker.Internet.Password(), Faker.Name.FirstName(), Faker.Name.LastName() },
        { Faker.Internet.Email(), "", Faker.Name.FirstName(), Faker.Name.LastName() },
        { Faker.Internet.Email(), "12345", Faker.Name.FirstName(), Faker.Name.LastName() },
        { Faker.Internet.Email(), Faker.Internet.Password(), "", Faker.Name.LastName() },
        { Faker.Internet.Email(), Faker.Internet.Password(), Faker.Name.FirstName(), "" }
    };

    [Theory]
    [MemberData(nameof(InvalidRequests))]
    public async Task Should_ReturnBadRequest_WhenRequestIsNotValid(
        string email,
        string password,
        string firstName,
        string lastName)
    {
        // Arrange
        var request = new RegisterUserRequest(email, password, firstName, lastName);
        
        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("/api/v1/users/register", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_ReturnOk_WhenRequestIsValid()
    {
        // Arrange
        var request = new RegisterUserRequest("create@test.com", Faker.Internet.Password(), Faker.Name.FirstName(),
            Faker.Name.LastName());
    

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("/api/v1/users/register", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

}