using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IWantApp.Endpoints.Client;

public class ClientGetProfile
{
    public static string Templete = "/profile";
    public static string[] Methods = new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle = Action;


    public static IResult Action(HttpContext http)
    {
        var userOn = http.User;
        var result = new
        {
            Id = userOn.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value,
            Name = userOn.Claims.First(c => c.Type == "Name").Value,
            Cpf = userOn.Claims.First(c => c.Type == "Cpf").Value
        };

        return Results.Ok(result);
    }
}
