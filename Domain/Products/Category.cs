using Flunt.Validations;
using IWantApp.Endpoints.Categories;

namespace IWantApp.Domain.Products;

public class Category : Entity
{
    public string Name { get; set; }
    public bool Active { get; set; }

    public Category(string name, string createdBy, string editedBy)
    {
        Name = name;
        Active = true;
        CreatedBy = createdBy;
        EditedBy = editedBy;
        CreatedOn = DateTime.Now;
        EditedOn = DateTime.Now;

        Validate(); // Atalho para refatorar ctrl + r + m 

    }

    public void EditInfo(string name, bool active, string editedBy)
    {
        Active = active;
        Name = name;
        EditedOn = DateTime.Now;
        EditedBy = editedBy;

        Validate();
    }

    private void Validate()
    {
        var contract = new Contract<Category>()
            .IsNotNull(Name, "Name", "Nome é Obrigatório")
            .IsGreaterOrEqualsThan(Name, 3, "Name", "O nome deve possuir mais que 3 caracteres")
            .IsNotNullOrEmpty(CreatedBy, "EditedBy", "Usuário inválido ou nulo")
            .IsNotNullOrEmpty(EditedBy, "EditedBy", "Alteração inválida, Usuário inválido ou nulo");
        AddNotifications(contract);
    }
}
