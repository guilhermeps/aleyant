using System.Data.SqlClient;
using Aleyant.CategoryManager.Api.Domain;
using Aleyant.CategoryManager.Domain;
using Dapper;

namespace Aleyant.CategoryManager.Api.Database;

public class CategoryRepository : ICategoryRepository
{
    private readonly string _connectionString;
    
    public CategoryRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("CategoryDatabase");
    }
    
    public async Task AddCategories(IEnumerable<CategoryVO> categories)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        string command = @"
        INSERT INTO category
        VALUES(@categoryName, @childCategoryName)";
        foreach (CategoryVO category in categories)
        {
            await connection.ExecuteAsync(command, new 
            {
                categoryName = category.CategoryName,
                childCategoryName = category.SubCategoryName
            });
        }
    }

    public async Task UpdateCategories(IEnumerable<CategoryVO> categories)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        string command = @"
        UPDATE category
        SET categoryName = @categoryName, childCategoryName = @childCategoryName
        WHERE categoryName = @categoryName";
        foreach (CategoryVO category in categories)
        {
            await connection.ExecuteAsync(command, new 
            {
                categoryName = category.CategoryName,
                childCategoryName = category.SubCategoryName
            });
        }
    }

    public async Task<IEnumerable<CategoryVO>> GetCategory(string name)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = @"
        SELECT
            categoryName as CategoryName,
            childCategoryName as SubCategoryName
        FROM category
        WHERE categoryName = @name || childCategoryName = @name";

        return await connection.QueryAsync<CategoryVO>(query, new
        {
            name = name
        });
    }

    public async Task<IEnumerable<CategoryVO>> GetAllCategories()
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = @"
        SELECT
            categoryName as CategoryName,
            childCategoryName as SubCategoryName
        FROM category";

        return await connection.QueryAsync<CategoryVO>(query);
    }

    public async Task RemoveCategory(string name)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        string command = @"
        DELETE FROM category
        WHERE categoryName = @name || childCategoryName = @name";
        
        await connection.ExecuteAsync(command, new { name = name });
    }
}
