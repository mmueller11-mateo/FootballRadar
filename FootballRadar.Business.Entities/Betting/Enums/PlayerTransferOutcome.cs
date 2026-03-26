namespace FootballRadar.Business.Entities.Betting.Enums
{
    public enum PlayerTransferOutcome
    {
        /** Permanent sale of a player for a fee. */
        PERMANENT_TRANSFER,
        /** Temporary move where the player returns to the parent club after the agreed period. */
        LOAN_SIMPLE,
        /** A loan with an agreed price to buy the player during or after the loan. */
        LOAN_WITH_OPTION_TO_BUY,
        /** A loan that becomes a permanent transfer automatically if certain conditions are met. */
        LOAN_WITH_OBLIGATION_TO_BUY,
        /** Signing a player whose contract has expired, resulting in no transfer fee. */
        FREE_AGENT_SIGNING,
        /** Agreement to sign a player whose contract expires in >6 months, for a future date. */
        PRE_CONTRACT_AGREEMENT,
        /** Player sold, but immediately loaned back to the selling club. */
        PERMANENT_WITH_LOAN_BACK,
        /** A sale involving a previously agreed-upon buy-back option. */
        BUY_BACK_TRIGGERED,
        /** Transfer terminated before completion due to failed medical, paperwork issues, or legal reasons. */
        TRANSFER_FAILED
    }
}
