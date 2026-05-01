using FootballRadar.Business.Entities.Enums;
using MediatR;

namespace FootballRadar.Business.Services.Queries
{
    public sealed class GetListOfCurrentTransfers : IRequest<IEnumerable<TransferModel>>
    {

    }

    public class TransferModel
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string SourceClub { get; set; }
        public required string DestinationClub { get; set; }
        public TransferStatus TransferStatus { get; set; }
    }
}