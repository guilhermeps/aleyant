using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aleyant.CategoryManager.Api.Domain;
using Aleyant.CategoryManager.Api.Service;
using Aleyant.CategoryManager.Domain;
using NSubstitute;
using Xunit;

namespace Aleyant.CategoryManager.Tests;

public class CategoryServiceUnitTest
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryServiceUnitTest()
    {
        _categoryRepository = Substitute.For<ICategoryRepository>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task AddCategories_CategoryNameEmptyNullOrFilledWithSpaces_ShouldNotAdd(string categoryName)
    {
        IList<CategoryVO> categoriesVO = new List<CategoryVO>
        {
            new(categoryName, "5 inch x 6 inch Postcards")
        };
        ICategoryService categoryService = new CategoryService(_categoryRepository);

        ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            categoryService.AddCategories(categoriesVO));

        Assert.Equal(CategoryService.CategoryNameMessageValidation, exception.Message);
    }

    [Fact]
    public async Task GetAll_WithDuplicateCategories_ShouldReturnTreeWithoutDuplicates()
    {
        IList<CategoryVO> categoriesVO = new List<CategoryVO>
        {
            new("Postcards", "5 inch x 6 inch Postcards"),
            new("Postcards", "5 inch x 6 inch Postcards")
        };
        _categoryRepository.GetAllCategories().Returns(categoriesVO);
        ICategoryService categoryService = new CategoryService(_categoryRepository);

        IEnumerable<Category> categories = await categoryService.GetAllCategories();
        
        Assert.Equal(expected: 1, actual: categories.Count());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task GetCategory_CategoryNameEmptyNullOrFilledWithSpaces_ShouldNotGet(string categoryName)
    {
        ICategoryService categoryService = new CategoryService(_categoryRepository);

        ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            categoryService.GetCategory(categoryName));

        Assert.Equal(CategoryService.CategoryNameMessageValidation, exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task UpdateCategories_CategoryNameEmptyNullOrFilledWithSpaces_ShouldNotUpdate(string categoryName)
    {
        IList<CategoryVO> categoriesVO = new List<CategoryVO>
        {
            new(categoryName, "5 inch x 6 inch Postcards")
        };
        ICategoryService categoryService = new CategoryService(_categoryRepository);

        ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            categoryService.AddCategories(categoriesVO));

        Assert.Equal(CategoryService.CategoryNameMessageValidation, exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task RemoveCategory_CategoryNameEmptyNullOrFilledWithSpaces_ShouldNotRemove(string categoryName)
    {
        ICategoryService categoryService = new CategoryService(_categoryRepository);

        ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            categoryService.RemoveCategory(categoryName));

        Assert.Equal(CategoryService.CategoryNameMessageValidation, exception.Message);
    }

    [Fact]
    public async Task RemoveCategory_CategoryDoesNotExist_ShouldNotRemove()
    {
        IList<CategoryVO> categoriesVO = new List<CategoryVO>
        {
            new("Postcards", "5 inch x 6 inch Postcards"),
            new("Postcards", "5 inch x 6 inch Postcards")
        };
        _categoryRepository.GetAllCategories().Returns(categoriesVO);
        ICategoryService categoryService = new CategoryService(_categoryRepository);

        KeyNotFoundException exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            categoryService.RemoveCategory("Banner"));

        Assert.Equal(CategoryService.CategoryNotFound, exception.Message);
    }
}
