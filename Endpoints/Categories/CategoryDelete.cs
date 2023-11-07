using IWantApp.Domain.Products;
using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Endpoints.Categories;

public class CategoryDelete
{
    public static string Template => "/category/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Delete.ToString() };
    public static Delegate Handle => Action;

    public static async Task<IResult> Action([FromRoute] Guid Id, ApplicationDbContext context)
    {
        
        var category = await context.Categories.FindAsync(Id);

        if (category == null)
            return Results.NotFound();
  
        context.Remove(category);
        await context.SaveChangesAsync();

        return Results.Ok("Categoria deletada com sucesso");
    }
}
