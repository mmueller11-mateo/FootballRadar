using FootballRadar.Business.Entities.TippSpiel;
using FootballRadar.TippSpiel.Abstractions;
using FootballRadar.TippSpiel.Business.Commands;
using FootballRadar.TippSpiel.Data;
using MediatR;

namespace FootballRadar.TippSpiel.Business.CommandHandlers
{
    public class CreateTippUserHandler : IRequestHandler<CreateTippUserCommand, TippUser>
    {
        private readonly TippSpielDbContext db;
        private readonly IPasswordHasher hasher;

        public CreateTippUserHandler(TippSpielDbContext db, IPasswordHasher hasher)
        {
            this.db = db;
            this.hasher = hasher;
        }

        public async Task<TippUser> Handle(CreateTippUserCommand request, CancellationToken ct)
        {
            var user = new TippUser
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                PasswordHash = hasher.Hash(request.Password)
            };
            db.TippUsers.Add(user);
            await db.SaveChangesAsync(ct);
            return user;
        }
    }
}
