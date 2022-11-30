using AutoMapper;
using Task.Application.Common.Mappings;

namespace Task.Application.CommandsQueries.Lobby.Queries.GetFree;

public class LobbyDto : IMapWith<Domain.Lobby>
{
    public string Creator { get; set; }
    public Guid ConnectionId { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Lobby, LobbyDto>()
            .ForMember(f => f.Creator,
                o => o.MapFrom(f => f.PlayerNameStep))
            .ForMember(f => f.ConnectionId, 
                o => o.MapFrom(f => f.ConnectionId));
    }
}