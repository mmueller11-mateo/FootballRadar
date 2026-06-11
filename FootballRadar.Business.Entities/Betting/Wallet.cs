using FootballRadar.Business.Entities.Exceptions;

namespace FootballRadar.Business.Entities.Betting
{
    public class Wallet
    {

        public Wallet(Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
        }
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public decimal Credits { get; private set; }
        public DateTimeOffset LastUpdated { get; private set; }

        public void Deposit(decimal credits)
        {
            Credits += credits;
            LastUpdated = DateTimeOffset.UtcNow;
        }

        public void Withdraw(decimal credits)
        {
            if (Credits >= credits)
            {
                Credits -= credits;
                LastUpdated = DateTimeOffset.UtcNow;
            }
            else
            {
                throw new InsufficientCreditException();
            }
        }
    }
}