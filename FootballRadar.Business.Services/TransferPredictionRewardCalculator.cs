using FootballRadar.Business.Entities.Enums;
using FootballRadar.Business.Entities.TransferEntities;

namespace FootballRadar.Business.Services
{
    internal sealed class TransferPredictionRewardCalculator : ITransferPredictionRewardCalculator
    {
        public Task<int> CalculateReward(TransferRumor transferRumor)
        {
            var result = -1;
            switch (transferRumor.Credibility)
            {
                case RumorCredibility.High:
                    result = 1;
                    break;
                case RumorCredibility.Medium:
                    result = 2;
                    break;
                case RumorCredibility.Low:
                    result = 3;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return Task.FromResult(result);
        }
    }
}
