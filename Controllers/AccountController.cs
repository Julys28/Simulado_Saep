
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ClinicaOdontologica.Models;
using ClinicaOdontologica.Data;

namespace ClinicaOdontologica.Controllers;

public class AccountController : Controller
{
    private readonly ClinicaContext _context;

    public AccountController(ClinicaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string login, string senha)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Login == login);

        if (usuario != null)
        {
            var verificador = new PasswordHasher<Usuario>();

            var resultado = verificador.VerifyHashedPassword(
                usuario,
                usuario.Senha,
                senha
            );

            if (resultado != PasswordVerificationResult.Failed)
            {
                HttpContext.Session.SetString(
                    "UsuarioNome",
                    usuario.Nome
                );

                return RedirectToAction("Index", "Home");
            }
        }

        ViewBag.Erro = "Usuário ou senha incorretos.";

        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return RedirectToAction("Login");
    }
}
