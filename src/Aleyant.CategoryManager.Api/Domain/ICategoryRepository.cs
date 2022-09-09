using Aleyant.CategoryManager.Domain;

namespace Aleyant.CategoryManager.Api.Domain;

public interface ICategoryRepository
{
    Task AddCategories(IEnumerable<CategoryVO> categories);
    Task UpdateCategories(IEnumerable<CategoryVO> categories);
    Task<IEnumerable<CategoryVO>> GetCategory(string name);
    Task<IEnumerable<CategoryVO>> GetAllCategories();
    Task RemoveCategory(string name);
}
