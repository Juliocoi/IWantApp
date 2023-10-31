using Flunt.Validations;
using Flunt.Notifications;
using IWantApp.Domain.Products;
using IWantApp.Infra.Data;

namespace IWantApp.Endpoints.Categories;

public class CategoryPost
{
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(CategoryRequest categoryRequest, ApplicationDbContext context)
    {
        //Validaçao
        var contract = new Contract<Category>()
            .IsNotNullOrEmpty(categoryRequest.Name, "Name", "Nome é Obrigatório")
            .IsGreaterOrEqualsThan(categoryRequest.Name, 3, "Name");
        
        if (!contract.IsValid)
        {
            return Results.ValidationProblem(contract.Notifications.ConvertToPromblemDetails());
        }

        var category = new Category(categoryRequest.Name, "teste", "teste");

        context.Categories.Add(category);
        context.SaveChanges();
        return Results.Created($"/categories/{category.Id}", category.Id);
    }
}
