using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.test.Abstraction;
using FluentAssertions;

namespace Evently.Modules.Events.test.Categories
{
    public  class CategoryTest:BaseTest
    {
        [Fact]
        public void Create_ShouldRaiseDomainEvent_WhenCategoryIsCreated()
        {
            // act
            var result = Category.Create(faker.Music.Genre());

            // assert
            CategoryCreatedDomainEvent categoryCreatedDomainEvent =
                AssertDomainEventWasPublished<CategoryCreatedDomainEvent>(result);
            categoryCreatedDomainEvent.CategoryId.Should().Be(result.Id);
        }
        [Fact]
        public void Archive_ShouldRaiseDomainEvent_WhenCategoryIsArchived()
        {
            // act
            var result = Category.Create(faker.Music.Genre());
            result.Archive();
            // assert
           CategoryArchivedDomainEvent categoryCreatedDomainEvent =
                AssertDomainEventWasPublished<CategoryArchivedDomainEvent>(result);
            categoryCreatedDomainEvent.CategoryId.Should().Be(result.Id);
        }

        [Fact]
        public void ChangeName_ShouldRaiseDomainEvent_WhenCategoryNameIsChanged()
        {
            // arrange 
            var result = Category.Create(faker.Music.Genre());
            result.ClearDomainEvents();
            string newName = faker.Music.Genre();

            // act
            result.ChangeName(newName);
            // assert
            CategoryNameChangedDomainEvent categoryCreatedDomainEvent =
                AssertDomainEventWasPublished<CategoryNameChangedDomainEvent>(result);
            categoryCreatedDomainEvent.CategoryId.Should().Be(result.Id);
        }
    }
}
