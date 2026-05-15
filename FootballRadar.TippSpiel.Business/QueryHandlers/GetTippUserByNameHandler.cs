using FootballRadar.Business.Entities.TippSpiel;
using FootballRadar.TippSpiel.Business.Queries;
using FootballRadar.TippSpiel.Data;
using MediatR;

namespace FootballRadar.TippSpiel.Business.QueryHandlers
{
    public class GetTippUserByNameHandler : IRequestHandler<GetTippUserByNameQuery, TippUser?>
    {
        private readonly TippSpielDbContext db;

        public GetTippUserByNameHandler(TippSpielDbContext db)
        {
            this.db = db;
        }

        public Task<TippUser?> Handle(GetTippUserByNameQuery request, CancellationToken ct) =>
            Task.FromResult(db.TippUsers.FirstOrDefault(u => u.Name == request.Name));
    }
}
