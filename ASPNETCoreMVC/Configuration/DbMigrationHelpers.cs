using ASPNETCoreMVC.Data;
using ASPNETCoreMVC.Models;
using Microsoft.AspNetCore.Identity;

namespace ASPNETCoreMVC.Configuration
{
    public static class DbMigrationHelpers
    {
        public static async Task EnsureSeedData(WebApplication serviceScope)
        {
            var services = serviceScope.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope(); 
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (env.IsDevelopment() || env.IsEnvironment("Docker"))
            {
                await context.Database.EnsureCreatedAsync();
                await EnsureSeedProducts(context);
            }
        }

        private static async Task  EnsureSeedProducts(AppDbContext context)
        {
            if (context.Produtos.Any())
            {
                return;
            }

            await context.Produtos.AddAsync(new Produto { Nome = "Livro CSS", Imagem = "CSS.jpg", Valor = 50 });
            await context.Produtos.AddAsync(new Produto { Nome = "Livro jQuery", Imagem = "JQuery.jpg", Valor = 150 });
            await context.Produtos.AddAsync(new Produto { Nome = "Livro HTML", Imagem = "HTML.jpg", Valor = 90 });
            await context.Produtos.AddAsync(new Produto { Nome = "Livro Razor", Imagem = "Razor.jpg", Valor = 50 });

            await context.SaveChangesAsync();

            if (context.Users.Any())
            {
                return;
            }

            await context.Users.AddAsync(new IdentityUser()
            { 
                Id = "83539904-9FAF-4364-A37F-94589509732A",
                UserName = "teste@teste.com",
                NormalizedUserName = "TESTE@TESTE.COM",
                Email = "teste@teste.com",
                NormalizedEmail = "TESTE@TESTE.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEGxIrbKmSz2pMRuvhrYxY57ALa3Y/MKExY+I11SI4IRdgdZH/XAT2b5XzB9uRzNdgA==",
                SecurityStamp = "MH75F22ZLBVRF2A6D2AQQUBJOW4OROZR",
                ConcurrencyStamp = "1331820e-7f16-4571-a1d2-807c6473a91e",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                AccessFailedCount = 0
            });

            await context.SaveChangesAsync();

            if (context.UserClaims.Any())
            {
                return;
            }

            await context.UserClaims.AddAsync(new IdentityUserClaim<string>()
            {
                UserId = "83539904-9FAF-4364-A37F-94589509732A",
                ClaimType = "Produtos",
                ClaimValue = "VI,ED,AD,EX"
            });

            await context.SaveChangesAsync();
        }
    }
}
