
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicaOdontologica.Data;
using ClinicaOdontologica.Models;

namespace ClinicaOdontologica.Controllers;

public class ProcedimentosController : Controller
{
    private readonly ClinicaContext _context;

    public ProcedimentosController(ClinicaContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var procedimentos = await _context.Procedimentos
            .OrderBy(p => p.Nome)
            .ToListAsync();

        return View(procedimentos);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Procedimento procedimento)
    {
        if (ModelState.IsValid)
        {
            _context.Procedimentos.Add(procedimento);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(procedimento);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var procedimento = await _context.Procedimentos.FindAsync(id);

        if (procedimento == null)
            return NotFound();

        return View(procedimento);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Procedimento procedimento)
    {
        if (id != procedimento.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            _context.Procedimentos.Update(procedimento);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(procedimento);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var procedimento = await _context.Procedimentos.FindAsync(id);

        if (procedimento == null)
            return NotFound();

        bool possuiAgendamento = await _context.Agendamentos
            .AnyAsync(a => a.ProcedimentoId == id);

        if (possuiAgendamento)
        {
            TempData["Erro"] =
                "Não é possível excluir este procedimento porque ele possui agendamentos.";

            return RedirectToAction(nameof(Index));
        }

        _context.Procedimentos.Remove(procedimento);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
