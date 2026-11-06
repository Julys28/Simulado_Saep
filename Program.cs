
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using ClinicaOdontologica.Data;
using ClinicaOdontologica.Models;

var builder = WebApplication.CreateBuilder(args);

// Serviços do sistema
builder.Services.AddControllersWithViews();

// Conexão com SQL Server
var connectionString = builder.Configuration
    .GetConnectionString("ConexaoClinica");

builder.Services.AddDbContext<ClinicaContext>(options =>
    options.UseSqlServer(connectionString)
);

// Configuração da sessão
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Construção da aplicação
var app = builder.Build();

// Criar administrador automaticamente
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<ClinicaContext>();

    var administrador = context.Usuarios
        .FirstOrDefault(u => u.Login == "admin");

    if (administrador == null)
    {
        var usuario = new Usuario
        {
            Nome = "Administrador",
            Login = "admin"
        };

        var gerador = new PasswordHasher<Usuario>();

        usuario.Senha = gerador.HashPassword(
            usuario,
            "admin123"
        );

        context.Usuarios.Add(usuario);
        context.SaveChanges();
    }
}

// Configuração do ambiente
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Erro");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

// Configuração das rotas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"
);

// Iniciar aplicação
app.Run();
