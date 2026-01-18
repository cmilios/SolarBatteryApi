using MediatR;
using SPCS.Application.Files.Abstractions;

namespace SPCS.Application.Applications.Commands
{
    public class CreateApplicationCommand : IRequest<SPCS.Files.Models.Application>
    {
        public string Name { get; init; } = default!;
    }

    public class CreateApplicationCommandHandler(IApplicationRepository repository) : IRequestHandler<CreateApplicationCommand, SPCS.Files.Models.Application>
    {
        private readonly IApplicationRepository _repository = repository;

        public async Task<SPCS.Files.Models.Application> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
        {
            var app = new SPCS.Files.Models.Application { Name = request.Name };
            await _repository.AddAsync(app);
            return app;
        }
    }
}
