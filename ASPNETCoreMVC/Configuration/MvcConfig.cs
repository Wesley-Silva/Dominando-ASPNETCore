using ASPNETCoreMVC.Data;
using ASPNETCoreMVC.Extensions;
using ASPNETCoreMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ASPNETCoreMVC.Configuration
{
    public static class MvcConfig
    {
        public static WebApplicationBuilder AddMvcConfiguration(this WebApplicationBuilder builder)
        {
            builder.Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables()
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true);

            // Add services to the container.
            //builder.Services.AddControllersWithViews();

            // ativar globalmente o ForgeryToken autómatico
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                options.Filters.Add(typeof(FiltroAuditoria));

                MvcOptionsConfig.ConfigurarMensagensDeModelBinding(options.ModelBindingMessageProvider);
            });

            //builder.Services.AddRouting(options =>
            //    options.ConstraintMap["slugify"] = typeof(RouteSlugifyParameterTransformer));

            // Adicionado suporte a mudança de convenção da rota de areas
            builder.Services.Configure<RazorViewEngineOptions>(options =>
            {
                options.AreaViewLocationFormats.Clear();
                options.AreaViewLocationFormats.Add("/Modulos/{2}/Views/{1}/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Modulos/{2}/Views/Shared/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
            });

            builder.Services.AddDbContext<AppDbContext>(o =>
                o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
                options.ExcludedHosts.Add("example.com");
                options.ExcludedHosts.Add("www.example.com");
            });

            // obter dados do arquivo de configuração globalmente
            builder.Services.Configure<ApiConfiguration>(
                builder.Configuration.GetSection(ApiConfiguration.ConfigName));

            return builder;
        }

        public static WebApplication UseMvcConfiguration(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/erro/500");
                //redirect para outros status code(400,404, 403, etc...)
                app.UseStatusCodePagesWithRedirects("/erro/{0}");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseGlobalizationConfig();

            app.UseElmahIo();
            app.UseElmahIoExtensionsLogging();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // Rota de área especializada
            app.MapAreaControllerRoute("AreaProdutos", "Produtos", "Produtos/{controller=Cadastro}/{action=Index}/{id?}");
            app.MapAreaControllerRoute("AreaVendas", "Vendas", "Vendas/{controller=Gestao}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            // Passando uma instancia antes da aplicação start
            using (var scopeService = app.Services.CreateScope())
            {
                var services = scopeService.ServiceProvider;

                var singService = services.GetRequiredService<IOperacaoSingleton>();

                Console.WriteLine("Direto da Program.cs" + singService.OperacaoId);
            }


            return app;
        }
    }
}
