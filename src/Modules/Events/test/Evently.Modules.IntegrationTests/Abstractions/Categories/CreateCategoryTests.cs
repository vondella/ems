using System.Net;
using System.Net.Http.Json;
using Evently.Modules.Events.Application.Categories.ArchiveCategory;
using Evently.Modules.Events.Application.Categories.CreateCategory;
using Evently.Modules.Events.Application.Categories.GetCategories;
using Evently.Modules.Events.Application.Categories.UpdateCategory;
using Evently.Modules.Events.Domain.Categories;
using FluentAssertions;

namespace Evently.Modules.IntegrationTests.Abstractions.Categories;

public class CreateCategoryTests:BaseIntegrationTest
{
    public  CreateCategoryTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public  async Task  Create_category_shouldReturnOkWhenValidRequest()
    {
        // arrange 
        var command = new CreateCategoryCommand("test");

        // act 
        var response = await Sender.Send(command);

        // assert
        response.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public async Task Create_category_shouldReturnOkFromHttpRequest()
    {
        // arrange
        var request = new CreateCategoryCommand("test");
        // act
        var response = await HttpClient.PostAsJsonAsync("/api/v1/categories", request);
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Get_categoriesLists()
    {
        // arrange
        var command = new CreateCategoryCommand("testlists");

        // act 
        var response = await Sender.Send(command);
        var query = new GetCategoriesQuery();
        var list = await Sender.Send(query);

        // assert
        list.ResponseData.Should().NotBeNull();
    }

    [Fact]
    public async Task Categories_ShouldRetuenErrorWhenAlreadyArchived()
    {
        // arrange 
        var command = new CreateCategoryCommand("testlists2");
        // act
        var response = await Sender.Send(command);
        var command2 = new ArchiveCategoryCommand(response.ResponseData);
        await Sender.Send(command2);

        var result = await Sender.Send(new ArchiveCategoryCommand(response.ResponseData));
        // assert
        result.Error.Should().Be(CategoryErrors.AlreadyArchived);
    }

    [Fact]
    public async Task Category_shouldReturnSuccessAfterUpdate()
    {
        // arrange 
        var command = new CreateCategoryCommand("testlists22");
        // act
        var response = await Sender.Send(command);
        var command2 = new UpdateCategoryCommand(response.ResponseData, "updated test");
        var response2=await Sender.Send(command2);

        // assert
        response.IsSuccessful.Should().BeTrue();
    }



}