
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicaOdontologica.Models;

public class Agendamento
{
    public int Id { get; set; }

    public int PacienteId { get; set; }

    public int ProcedimentoId { get; set; }

    public DateTime DataAgendamento { get; set; }

    [ForeignKey("PacienteId")]
    public Paciente? Paciente { get; set; }

    [ForeignKey("ProcedimentoId")]
    public Procedimento? Procedimento { get; set; }
}
