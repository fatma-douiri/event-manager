using AutoMapper;
using EventManager.Application.Events.DTOs;
using EventManager.Application.Users.DTOs;
using EventManager.Domain.Model;

namespace EventManager.Application.Common.Mappings;

/// <summary>
/// Profile de mappage pour AutoMapper.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="MappingProfile"/>.
    /// </summary>
    public MappingProfile()
    {
        CreateMap<Event, EventDto>()
            .ForMember(d => d.OrganizerName,
                opt => opt.MapFrom(s => $"{s.Organizer.FirstName} {s.Organizer.LastName}"))
            .ForMember(d => d.CurrentParticipants,
                opt => opt.MapFrom(s => s.Registrations.Count))
            .ForMember(d => d.Participants,
                opt => opt.MapFrom(s => s.Registrations.Select(r => r.User)));

        CreateMap<ApplicationUser, UserDto>()
            .ForMember(d => d.FullName,
                opt => opt.MapFrom(s => $"{s.FirstName} {s.LastName}"));
    }
}
