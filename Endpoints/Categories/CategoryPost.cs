﻿using IWantApp.Domain.Products;
using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IWantApp.Endpoints.Categories;

public class CategoryPost
{
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static IResult Action(CategoryRequest categoryRequest, HttpContext http, ApplicationDbContext context)
    {
        var userId = http.User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value; // pega o usuário autenticado.
        var category = new Category(categoryRequest.Name, userId, userId);

        if (!category.IsValid) // IsValid é uma propriedade do FLunt
        {
            return Results.ValidationProblem(category.Notifications.ConvertToPromblemDetails());
        }

        context.Categories.Add(category);
        context.SaveChanges();
        return Results.Created($"/categories/{category.Id}", category.Id);
    }
}
