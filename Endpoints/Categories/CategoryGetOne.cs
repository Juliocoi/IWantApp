using IWantApp.Domain.Products;
using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Endpoints.Categories;

public class CategoryGetOne
{
    public static string Template => "/category/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute] Guid Id, ApplicationDbContext context)
    {
        
        var category = context.Categories.Find(Id);
                   
        if (category == null)
            return Results.NotFound();

        return Results.Ok(category);
    }
}
