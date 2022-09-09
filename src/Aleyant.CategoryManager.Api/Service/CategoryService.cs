using Aleyant.CategoryManager.Api.Domain;
using Aleyant.CategoryManager.Domain;

namespace Aleyant.CategoryManager.Api.Service;

public sealed class CategoryService : ICategoryService
{
    public const string CategoryNameMessageValidation = "Category must have a name";
    public const string CategoryNotFound = "The specified category was not found";
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task AddCategories(IEnumerable<CategoryVO> categories)
    {
        if (categories.Any(category => string.IsNullOrWhiteSpace(category.CategoryName)))
        {
            throw new ArgumentException(CategoryNameMessageValidation);   
        }

        await _categoryRepository.AddCategories(categories);
    }

    public async Task UpdateCategories(IEnumerable<CategoryVO> categories)
    {
        if (categories.Any(category => string.IsNullOrWhiteSpace(category.CategoryName)))
        {
            throw new ArgumentException(CategoryNameMessageValidation);   
        }

        await _categoryRepository.UpdateCategories(categories);
    }

    public async Task<Category> GetCategory(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(CategoryNameMessageValidation);
        }

        IEnumerable<CategoryVO> categoriesVO = await _categoryRepository.GetCategory(name);
        IList<Category> categoriesTree = BuildCategoriesTree(categoriesVO);

        return categoriesTree.FirstOrDefault()!;
    }

    public async Task<IEnumerable<Category>> GetAllCategories()
    {
        IEnumerable<CategoryVO> categoriesVO = await _categoryRepository.GetAllCategories();
        IList<Category> categories = BuildCategoriesTree(categoriesVO);

        return categories;
    }

    public async Task RemoveCategory(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(CategoryNameMessageValidation);
        }

        IEnumerable<CategoryVO> categoriesVo = await _categoryRepository.GetCategory(name);
        if (categoriesVo.Any())
        {
            await _categoryRepository.RemoveCategory(name);
        }

        throw new KeyNotFoundException(CategoryNotFound);
    }

    private IList<Category> BuildCategoriesTree(IEnumerable<CategoryVO> categories)
    {
        IList<Category> listOfCategories = new List<Category>();
        foreach (CategoryVO categoryVO in categories)
        {
            HashSet<Category> subAcategory = new HashSet<Category>();
            subAcategory.Add(new(categoryVO.SubCategoryName));
            Category newCategory = new(categoryVO.CategoryName, subAcategory);
            Category existentCategory = listOfCategories.FirstOrDefault(category => 
                category.Name.Equals(newCategory.Name))!;
            if (existentCategory is not null)
            {
                existentCategory.AddChildCategory(categoryVO.SubCategoryName);
                
                continue;
            }

            bool subCategoryFound = false;
            foreach (Category? category in listOfCategories)
            {
                Category? existentSubCategory = category.GetCategory(newCategory.Name);
                if (existentSubCategory is not null)
                {
                    subCategoryFound = true;
                    existentSubCategory.AddChildCategory(categoryVO.SubCategoryName);
                    
                    break;
                }
            }

            if (subCategoryFound)
            {
                continue;
            }

            listOfCategories.Add(newCategory);
        }

        return listOfCategories;
    }
}
