using Flunt.Validations;

namespace IWantApp.Domain.Products;

public class Product: Entity
{
    public string Name {  get; private set; } 
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; }
    public string Description { get; private set; }
    public bool HasStock { get; private set; }
    public bool Active { get; private set; } = true;
    public decimal Price { get; private set; }

    
    public Product(){ }

    public Product(string name, Category category, string description, bool hasStock, decimal price, string createdBy)
    {
        Name = name;
        Category = category;
        Description = description;
        Price = price;
        HasStock = hasStock;
        
        CreatedBy = createdBy;
        EditedBy = createdBy;
        CreatedOn = DateTime.Now;
        EditedOn = DateTime.Now;

        Validate();
    }

    private void Validate()
    {
        var contract = new Contract<Product>()
            .IsNotNullOrEmpty(Name, "Name", "Name is require")
            .IsGreaterOrEqualsThan(Name, 3, "Name", "The name is too short")
            .IsNotNull(Category, "Category", "Category not found")
            .IsNotNullOrEmpty(Description, "Description", "Description is require")
            .IsGreaterOrEqualsThan(Description, 3, "Description", "Description is too short")
            .IsNotNullOrEmpty(Price.ToString(),"Price", "The price is invalid")
            .IsNotNullOrEmpty(CreatedBy, "CreatedBy", "User invalid")
            .IsNotNullOrEmpty(EditedBy, "EditedBy", "User invalid");
        AddNotifications(contract);
    }
}
