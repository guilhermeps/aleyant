using System.Collections.Generic;
using Aleyant.CategoryManager.Api.Domain;
using Xunit;

namespace Aleyant.CategoryManager.Tests;

public class CategoryUnitTest
{
    [Fact]
    public void AddChildCategory_Duplicates_ShouldEnsureUniqueCategoryName()
    {
        Category category = new("Postcards");
        
        category.AddChildCategory("5 inch x 6 inch Postcards");
        category.AddChildCategory("5 inch x 6 inch Postcards");

        Assert.Equal(expected: 1, actual: category.ChildrenCategories.Count);
    }

    [Theory]
    [InlineData("5 inch x 6 inch Postcards")]
    [InlineData("Fun Postcards")]
    public void GetCategory_MultipleCategories_ShouldFindACategory(string categoryToGet)
    {
        Category category = new("Postcards");
        category.AddChildCategory("5 inch x 6 inch Postcards");
        Category categoryWithChildren = new(
            "6 inch x 7 inch Postcards", 
            new HashSet<Category>() { new("Fun Postcards")});
        category.AddChildCategory(categoryWithChildren);

        
    }
}
