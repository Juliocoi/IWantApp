using Flunt.Notifications;
using IWantApp.Domain.Products;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IWantApp.Infra.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser> // como o Itentity.Entity trabalha junto com o EntityFrameWork não usamos a classe DbContext do deste.
{// Como nossa classe herda do Identity.Entity, ñ precisamos criar uma propriedade DbSet para gerar a tabela de usuários, o frame fará isso. Após concluir a intalação do FW no projeto, rodar migrations para criar a tabela.
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); //base == Super. Aqui chamamos a classe pai, que é a IdentityDBContex, para que seja modelada no banco.

        builder.Ignore<Notification>(); //Notifications foi declarada na entidade genérica Entity. Ignorar ou dará erro no branco, não precisamos das propriedades de notification neste contexto..

        builder.Entity<Product>()
            .Property(p => p.Name).IsRequired();
        builder.Entity<Product>()
            .Property(p => p.Description).HasMaxLength(255);
        builder.Entity<Product>()
            .Property(p => p.Price).HasColumnType("decimal(10,2)").IsRequired();
            
        builder.Entity<Category>()  
            .Property(c => c.Name).IsRequired();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configuration)
    {
        configuration.Properties<string>()
            .HaveMaxLength(100);
    }
}
