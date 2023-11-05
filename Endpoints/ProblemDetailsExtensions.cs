using Flunt.Notifications;
using Microsoft.AspNetCore.Identity;

namespace IWantApp.Endpoints;
//Esta é um recurso chamado Método de extensão. É possível criar métodos e extendê-los para uma classe primitiva do C#. Como no Swift
//Criamos esta classe para usar seu método nos endpoints, evitando repetição de código.
//Invocamos o método no endpoint através da classe Notifications, do Flunt.
public static class ProblemDetailsExtensions
{
    public static Dictionary<string, string[]> ConvertToPromblemDetails(this IReadOnlyCollection<Notification> notifications)
    {
        return notifications
                .GroupBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.Select(x => x.Message).ToArray());
    }

    public static Dictionary<string, string[]> ConvertToPromblemDetails(this IEnumerable<IdentityError> error)
    {
        var dictionary = new Dictionary<string, string[]>();
        dictionary.Add("Error", error.Select(e => e.Description).ToArray());

        return dictionary;
    }
}
