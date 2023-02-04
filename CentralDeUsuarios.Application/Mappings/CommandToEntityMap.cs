using AutoMapper;
using CentralDeUsuarios.Application.Commands;
using CentralDeUsuarios.Domain.Entities;

namespace CentralDeUsuarios.Application.Mappings;

public class CommandToEntityMap : Profile
{
    public CommandToEntityMap()
    {
        CreateMap<CriarUsuarioCommand, Usuario>()
        .AfterMap((command, entity) => 
        {
            entity.Id = Guid.NewGuid();
            entity.DataHoraCriacao = DateTime.Now;

        });
    }
}
