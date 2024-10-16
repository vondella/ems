﻿using Evently.Modules.Users.IntegrationTests.Abstractions;
using Evently.Modules.Users.Presentation.Users;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Evently.Modules.Users.Application.Users.RegisterUser;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net.Http.Headers;
using Evently.Modules.Users.Application.Users.GetUser;

namespace Evently.Modules.Users.IntegrationTests.Users;

public class GetUserProfileTests : BaseIntegrationTest
{
    public  GetUserProfileTests(IntegrationTestWebAppFactory factory) : base(factory)
    {

    }

    [Fact]
    public async Task Should_ReturnUnauthorized_WhenAccessTokenNotProvided()
    {
        // Act
        HttpResponseMessage response = await HttpClient.GetAsync("/api/v1/users/profile");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Should_ReturnOk_WhenUserExists()
    {
        // Arrange
        string accessToken = await RegisterUserAndGetAccessTokenAsync("exists@test.com", Faker.Internet.Password());
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            JwtBearerDefaults.AuthenticationScheme,
            accessToken);

        // Act
        HttpResponseMessage response = await HttpClient.GetAsync("/api/v1/users/profile");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        UserResponse? user = await response.Content.ReadFromJsonAsync<UserResponse>();
        user.Should().NotBeNull();
    }
    private async Task<string> RegisterUserAndGetAccessTokenAsync(string email, string password)
    {
        var request = new RegisterUserCommand(email, password, Faker.Name.FirstName(), Faker.Name.LastName());
      

        await HttpClient.PostAsJsonAsync("/api/v1/users/register", request);

        string accessToken = await GetAccessTokenAsync(request.Email, request.Password);

        return accessToken;
    }
}