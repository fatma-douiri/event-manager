using MediatR;
using EventManager.Application.Events.DTOs;
using EventManager.Application.Users.DTOs;

namespace EventManager.Application.Events.Queries.GetParticipantsOfEvent
{
    public class GetParticipantsOfEventQuery : IRequest<List<UserDto>>
    {
        public int EventId { get; set; }
    }
}