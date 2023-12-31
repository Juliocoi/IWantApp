using IWantApp.Endpoints.Categories;
using IWantApp.Endpoints.Client;
using IWantApp.Endpoints.Employees;
using IWantApp.Endpoints.Orders;
using IWantApp.Endpoints.Products;
using IWantApp.Endpoints.Security;
using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqlServer<ApplicationDbContext>(
    builder.Configuration["ConnectionString:IWantDb"]);
//configuração framework Identity.EntityFrameworkCore
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 3;
    
}).AddEntityFrameworkStores<ApplicationDbContext>();

//configuração framework JWT
//Proteção dos endpoints
builder.Services.AddAuthorization(options => 
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
    options.AddPolicy("EmployeePolicy", p => 
        p.RequireAuthenticatedUser().RequireClaim("EmployeeCode"));
    options.AddPolicy("Employee010Policy", p =>
        p.RequireAuthenticatedUser().RequireClaim("EmployeeCode", "010"));
    options.AddPolicy("CpfPolicy", p =>
        p.RequireAuthenticatedUser().RequireClaim("Cpf"));
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JwtBearerTokenSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtBearerTokenSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtBearerTokenSettings:SecretKey"]))
    };
});

builder.Services.AddScoped<QueryAllUsersWithClaimName>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapMethods(CategoryPost.Template, CategoryPost.Methods, CategoryPost.Handle);
app.MapMethods(CategoryGetAll.Template, CategoryGetAll.Methods, CategoryGetAll.Handle);
app.MapMethods(CategoryPut.Template, CategoryPut.Methods, CategoryPut.Handle);
app.MapMethods(CategoryGetOne.Template, CategoryGetOne.Methods, CategoryGetOne.Handle);
app.MapMethods(CategoryDelete.Template, CategoryDelete.Methods, CategoryDelete.Handle);

app.MapMethods(EmployeePost.Template, EmployeePost.Methods, EmployeePost.Handle); 
app.MapMethods(EmployeeGetAll.Template, EmployeeGetAll.Methods, EmployeeGetAll.Handle);

app.MapMethods(TokenPost.Template, TokenPost.Methods, TokenPost.Handle);

app.MapMethods(ProductPost.Template, ProductPost.Methods,ProductPost.Handle);
app.MapMethods(ProductGetAll.Template, ProductGetAll.Methods, ProductGetAll.Handle);

app.MapMethods(ProductGetShowCase.Template, ProductGetShowCase.Methods, ProductGetShowCase.Handle);
app.MapMethods(OrderPost.Template, OrderPost.Methods, OrderPost.Handle);

app.MapMethods(ClientPost.Template, ClientPost.Methods, ClientPost.Handle);
app.MapMethods(ClientGetProfile.Templete, ClientGetProfile.Methods, ClientGetProfile.Handle);


app.UseExceptionHandler("/error");
app.Map("/error", (HttpContext http) =>
{
    var error = http.Features?.Get<IExceptionHandlerFeature>()?.Error;

    if (error != null)
    {
        if (error is SqlException)
            return Results.Problem(title: "Database out", statusCode: 500);
        else if (error is BadHttpRequestException)
            return Results.Problem(title: "Error to convert data", statusCode: 500);
    }
    return Results.Problem(title: "An error ocurred", statusCode: 500);
});

app.Run();
