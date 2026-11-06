
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicaOdontologica.Data;
using ClinicaOdontologica.Models;

namespace ClinicaOdontologica.Controllers;

public class PacientesController : Controller
{
    private readonly ClinicaContext _context;

    public PacientesController(ClinicaContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string busca)
    {
        var pacientes = _context.Pacientes.AsQueryable();

        if (!string.IsNullOrWhiteSpace(busca))
        {
            pacientes = pacientes.Where(p =>
                p.Nome.Contains(busca) ||
                p.Telefone.Contains(busca));
        }

        return View(await pacientes
            .OrderBy(p => p.Nome)
            .ToListAsync());
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Paciente paciente)
    {
        if (ModelState.IsValid)
        {
            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(paciente);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var paciente = await _context.Pacientes.FindAsync(id);

        if (paciente == null)
            return NotFound();

        return View(paciente);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Paciente paciente)
    {
        if (id != paciente.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            _context.Pacientes.Update(paciente);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(paciente);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var paciente = await _context.Pacientes.FindAsync(id);

        if (paciente == null)
            return NotFound();

        bool possuiAgendamento = await _context.Agendamentos
            .AnyAsync(a => a.PacienteId == id);

        if (possuiAgendamento)
        {
            TempData["Erro"] =
                "Não é possível excluir este paciente porque ele possui agendamentos.";

            return RedirectToAction(nameof(Index));
        }

        _context.Pacientes.Remove(paciente);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
