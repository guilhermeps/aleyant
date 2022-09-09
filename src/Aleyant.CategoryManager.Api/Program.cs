using Aleyant.CategoryManager.Api.Database;
using Aleyant.CategoryManager.Api.Domain;
using Aleyant.CategoryManager.Api.Service;
using Aleyant.CategoryManager.Domain;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<ICategoryService, CategoryService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/categories", async (ICategoryService categoryService) =>
{
    IEnumerable<Category> categories = await categoryService.GetAllCategories();
    
    return categories;
})
.Produces<IEnumerable<Category>>(StatusCodes.Status200OK, "application/json")
.Produces(StatusCodes.Status400BadRequest)
.WithName("GetAllCategories")
.WithTags("Category");

app.MapGet("/categories/{name}", async (
    string name,
    ICategoryService categoryService) =>
{
    try
    {
        Category? category = await categoryService.GetCategory(name);

        return category is not null
            ? Results.Ok(category!)
            : Results.NotFound();
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
})
.Produces<Category>(StatusCodes.Status200OK, "application/json")
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status404NotFound)
.WithName("GetCategory")
.WithTags("Category");

app.MapPost("/categories", async (
    List<CategoryVO> categories,
    ICategoryService categoryService) => 
{
    try
    {
        await categoryService.AddCategories(categories);

        return Results.CreatedAtRoute("GetAllCategories");
    }
    catch (ArgumentException ex)
    {
        return Results.UnprocessableEntity(ex.Message);
    }
})
.Produces<Category>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status422UnprocessableEntity)
.WithName("PostCategory")
.WithTags("Category");

app.MapPut("/categories", async (
    List<CategoryVO> categories,
    ICategoryService categoryService) => 
{
    try
    {
        await categoryService.UpdateCategories(categories);

        return Results.Ok();
    }
    catch (ArgumentException ex)
    {
        return Results.UnprocessableEntity(ex.Message);
    }
})
.Produces<Category>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status422UnprocessableEntity)
.WithName("PutCategory")
.WithTags("Category");

app.MapDelete("/categories/{name}", async (
    string name,
    ICategoryService categoryService) => 
{
    try
    {
        await categoryService.RemoveCategory(name);

        return Results.NoContent();
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch (KeyNotFoundException ex)
    {
        return Results.NotFound(ex.Message);
    }
})
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status400BadRequest)
.WithName("DeleteCategory")
.WithTags("Category");

app.Run();
