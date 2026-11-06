
namespace ClinicaOdontologica.Models;

public class Procedimento
{
    public int Id { get; set; }

    public string Nome { get; set; } = "";

    public string Descricao { get; set; } = "";

    public decimal Valor { get; set; }
}
