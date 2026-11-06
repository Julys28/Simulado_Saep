
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicaOdontologica.Data;

namespace ClinicaOdontologica.Controllers;

public class HomeController : Controller
{
    private readonly ClinicaContext _context;

    public HomeController(ClinicaContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        ViewBag.UsuarioNome =
            HttpContext.Session.GetString("UsuarioNome");

        return View();
    }

    public IActionResult Erro()
    {
        return View();
    }

    // Teste de conexão com o banco
    public async Task<IActionResult> TestarBanco()
    {
        try
        {
            bool conectado =
                await _context.Database.CanConnectAsync();

            if (conectado)
            {
                return Content(
                    "SUCESSO! O Visual Studio está conectado ao SQL Server e ao banco saep_db."
                );
            }

            return Content(
                "Não foi possível conectar ao banco saep_db."
            );
        }
        catch (Exception ex)
        {
            return Content("Erro na conexão: " + ex.Message);
        }
    }
}
