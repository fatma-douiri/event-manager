using MediatR;
using EventManager.Domain.Repositories;

using EventManager.Application.Users.DTOs;

namespace EventManager.Application.Events.Queries.GetParticipantsOfEvent
{
    public class GetParticipantsOfEventHandler : IRequestHandler<GetParticipantsOfEventQuery, List<UserDto>>
    {
        private readonly IEventRegistrationRepository _eventRegistrationRepository;

        public GetParticipantsOfEventHandler(IEventRegistrationRepository eventRegistrationRepository)
        {
            _eventRegistrationRepository = eventRegistrationRepository;
        }

        public async Task<List<UserDto>> Handle(GetParticipantsOfEventQuery request, CancellationToken cancellationToken)
        {
            var registrations = await _eventRegistrationRepository.GetEventRegistrationsAsync(request.EventId);

         
            return registrations.Select(r => new UserDto
            {
                Id = r.User.Id,
                FullName = $"{r.User.FirstName} {r.User.LastName}",
                Email = r.User.Email
            }).ToList();
        }
    }
}
