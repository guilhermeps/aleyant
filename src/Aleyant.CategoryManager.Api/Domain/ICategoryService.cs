using Aleyant.CategoryManager.Domain;

namespace Aleyant.CategoryManager.Api.Domain;

public interface ICategoryService
{
    Task AddCategories(IEnumerable<CategoryVO> categories);
    Task UpdateCategories(IEnumerable<CategoryVO> categories);
    Task<Category> GetCategory(string name);
    Task<IEnumerable<Category>> GetAllCategories();
    Task RemoveCategory(string name);
}