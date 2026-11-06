
using Microsoft.EntityFrameworkCore;
using ClinicaOdontologica.Models;

namespace ClinicaOdontologica.Data;

public class ClinicaContext : DbContext
{
    public ClinicaContext(DbContextOptions<ClinicaContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Paciente> Pacientes { get; set; }
    public DbSet<Procedimento> Procedimentos { get; set; }
    public DbSet<Agendamento> Agendamentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>().ToTable("usuarios");
        modelBuilder.Entity<Paciente>().ToTable("pacientes");
        modelBuilder.Entity<Procedimento>().ToTable("procedimentos");
        modelBuilder.Entity<Agendamento>().ToTable("agendamentos");

        modelBuilder.Entity<Agendamento>()
            .Property(a => a.PacienteId)
            .HasColumnName("paciente_id");

        modelBuilder.Entity<Agendamento>()
            .Property(a => a.ProcedimentoId)
            .HasColumnName("procedimento_id");

        modelBuilder.Entity<Agendamento>()
            .Property(a => a.DataAgendamento)
            .HasColumnName("data_agendamento");

        modelBuilder.Entity<Agendamento>()
            .HasOne(a => a.Paciente)
            .WithMany()
            .HasForeignKey(a => a.PacienteId);

        modelBuilder.Entity<Agendamento>()
            .HasOne(a => a.Procedimento)
            .WithMany()
            .HasForeignKey(a => a.ProcedimentoId);
    }
}
