using System;

namespace CentralUsuarios.Infra.Messages.Models;

public class MessageQueueModel
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public TipoMensagem Tipo { get; set; }
    public string Conteudo { get; set; }
    public DateTime DataHoraCriacao { get; set; } = DateTime.Now;
}

public enum TipoMensagem
{
    CONFIRMACAO_DE_CADASTRO = 1,
    RECUPERACAO_DE_SENHA = 2
}
