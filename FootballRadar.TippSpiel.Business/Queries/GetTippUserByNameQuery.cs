using FootballRadar.Business.Entities.TippSpiel;
using MediatR;

namespace FootballRadar.TippSpiel.Business.Queries
{
    public class GetTippUserByNameQuery : IRequest<TippUser?>
    {
        public string Name { get; set; } = string.Empty;
    }

}
