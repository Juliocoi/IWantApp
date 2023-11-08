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
    
    public Product(){ }

    public Product(string name, Category category, string description, bool hasStock, string createdBy)
    {
        Name = name;
        Category = category;
        Description = description;
        HasStock = hasStock;
        
        CreatedBy = createdBy;
        EditedBy = createdBy;
        CreatedOn = DateTime.Now;
        EditedOn = DateTime.Now;

        Validate();
    }

    private void Validate()
    {
        var contract = new Contract<Category>()
            .IsNotNullOrEmpty(Name, "Name", "Nome é Obrigatório")
            .IsGreaterOrEqualsThan(Name, 3, "Name", "O nome deve possuir mais que 3 caracteres")
            .IsNotNull(Category, "verificar campos obrigatórios.")
            .IsNotNullOrEmpty(Description, "verificar campos obrigatórios")
            .IsGreaterOrEqualsThan(Description, 3, "Descrição inválida")
            .IsNotNullOrEmpty(CreatedBy, "CreatedBy", "Usuário inválido ou nulo")
            .IsNotNullOrEmpty(EditedBy, "EditedBy", "Alteração inválida, Usuário inválido ou nulo");
        AddNotifications(contract);
    }
}
