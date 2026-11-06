
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicaOdontologica.Data;
using ClinicaOdontologica.Models;

namespace ClinicaOdontologica.Controllers;

public class AgendamentosController : Controller
{
    private readonly ClinicaContext _context;

    public AgendamentosController(ClinicaContext context)
    {
        _context = context;
    }

    private async Task CarregarListas()
    {
        ViewBag.Pacientes = new SelectList(
            await _context.Pacientes.OrderBy(p => p.Nome).ToListAsync(),
            "Id",
            "Nome"
        );

        ViewBag.Procedimentos = new SelectList(
            await _context.Procedimentos.OrderBy(p => p.Nome).ToListAsync(),
            "Id",
            "Nome"
        );
    }

    public async Task<IActionResult> Index()
    {
        var agendamentos = await _context.Agendamentos
            .Include(a => a.Paciente)
            .Include(a => a.Procedimento)
            .OrderBy(a => a.DataAgendamento)
            .ToListAsync();

        return View(agendamentos);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await CarregarListas();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Agendamento agendamento)
    {
        bool pacienteExiste = await _context.Pacientes
            .AnyAsync(p => p.Id == agendamento.PacienteId);

        bool procedimentoExiste = await _context.Procedimentos
            .AnyAsync(p => p.Id == agendamento.ProcedimentoId);

        if (!pacienteExiste || !procedimentoExiste)
        {
            ModelState.AddModelError("",
                "Selecione um paciente e um procedimento válidos.");
        }

        if (ModelState.IsValid)
        {
            _context.Agendamentos.Add(agendamento);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        await CarregarListas();

        return View(agendamento);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var agendamento = await _context.Agendamentos.FindAsync(id);

        if (agendamento == null)
            return NotFound();

        await CarregarListas();

        return View(agendamento);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Agendamento agendamento)
    {
        if (id != agendamento.Id)
            return NotFound();

        bool pacienteExiste = await _context.Pacientes
            .AnyAsync(p => p.Id == agendamento.PacienteId);

        bool procedimentoExiste = await _context.Procedimentos
            .AnyAsync(p => p.Id == agendamento.ProcedimentoId);

        if (!pacienteExiste || !procedimentoExiste)
        {
            ModelState.AddModelError("",
                "Selecione um paciente e um procedimento válidos.");
        }

        if (ModelState.IsValid)
        {
            _context.Agendamentos.Update(agendamento);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        await CarregarListas();

        return View(agendamento);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var agendamento = await _context.Agendamentos.FindAsync(id);

        if (agendamento == null)
            return NotFound();

        _context.Agendamentos.Remove(agendamento);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
