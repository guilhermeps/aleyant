namespace Aleyant.CategoryManager.Api.Domain;

public class Category : IEquatable<Category>
{
    public string Name { get; private set;}
    public HashSet<Category> ChildrenCategories { get; init; }

    public Category(string name)
    {
        Name = name;
        ChildrenCategories = new HashSet<Category>();
    }

    public Category(string name, HashSet<Category> childrenCategories)
    {
        Name = name;
        ChildrenCategories = childrenCategories;
    }

    public void AddChildCategory(string name) => ChildrenCategories.Add(new(name));

    public void AddChildCategory(Category category) => ChildrenCategories.Add(category);

    public Category GetCategory(string name) => FindCategory(name, ChildrenCategories)!;

    public bool Equals(Category? other)
    {
        if (other == null)
        {
            return false;
        }
        
        if (Name == other.Name)
        {
            return true;
        }

        return false;
    }

    public override bool Equals(Object obj)
    {
        if (obj == null)
        {
            return false;
        }

        Category category = obj as Category;
        return category == null ? false : Equals(category);
    }

    public override int GetHashCode() => Name.GetHashCode();

    private Category? FindCategory(string name, HashSet<Category> childrenCategories)
    {
        Category? category = childrenCategories.FirstOrDefault(c => c.Name.Equals(name));
        if (category is not null)
        {
            return category;
        }

        foreach (var childCategory in childrenCategories)
        {
            FindCategory(name, childCategory!.ChildrenCategories);
        }

        return null;     
    }
}
