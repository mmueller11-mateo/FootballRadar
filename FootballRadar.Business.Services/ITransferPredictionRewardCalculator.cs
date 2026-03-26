using FootballRadar.Business.Entities.TransferEntities;

namespace FootballRadar.Business.Services
{
    internal interface ITransferPredictionRewardCalculator
    {
        Task<int> CalculateReward(TransferRumor transfeRumor);

    }
}
