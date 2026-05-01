using FootballRadar.Business.Entities;
using MediatR;

namespace FootballRadar.Admin.Business.Services.Queries
{
    public sealed class GetCountriesQuery : IRequest<IEnumerable<Country>>
    {
    }
}
