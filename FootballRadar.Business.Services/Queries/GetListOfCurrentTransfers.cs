using FootballRadar.Business.Entities.Enums;
using MediatR;

namespace FootballRadar.Business.Services.Queries
{
    public sealed class GetListOfCurrentTransfers : IRequest<IReadOnlyCollection<TransferModel>>
    {

    }

    public class TransferModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SourceClub { get; set; }
        public string DestinationClub { get; set; }
        public TransferStatus TransferStatus { get; set; }
    }
}