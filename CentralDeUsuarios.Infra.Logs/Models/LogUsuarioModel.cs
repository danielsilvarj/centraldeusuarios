using System;

namespace CentralDeUsuarios.Infra.Logs.Models;

public class LogUsuarioModel
{
    public Guid Id { get; set; }
    public string Operacao { get; set; }
    public string Detalhes { get; set; }
    public DateTime DataHora { get; set; }
    public Guid UsuarioId { get; set; }
}
