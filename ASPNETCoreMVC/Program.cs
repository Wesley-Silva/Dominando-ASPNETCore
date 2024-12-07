using ASPNETCoreMVC.Configuration;
using ASPNETCoreMVC.Services;

var builder = WebApplication.CreateBuilder(args);

builder
       .AddGlobalizationConfig()
       .AddMvcConfiguration()
       .AddIdentityConfiguration()
       .AddElmahConfiguration()
       .AddDependencyInjectionConfiguration();

var app = builder.Build();

app.UseMvcConfiguration();

app.Run();
