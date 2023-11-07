using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Authorization;

namespace IWantApp.Endpoints.Employees;

public class EmployeeGetAll
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "Employee010Policy")]
    public static async Task<IResult>Action(int? page, int? rows, QueryAllUsersWithClaimName query)// o IConfiguration permite acesso às configurações do App.
    {
        if (page == null) page = 1;
        if (rows == null || rows.Value > 10) rows = 10;

        var result = await query.Execute(page.Value, rows.Value);
        return Results.Ok(result);
    }
}
