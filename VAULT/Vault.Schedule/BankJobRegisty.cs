using FluentScheduler;

namespace Vault.Schedule
{
    public class BankJobRegisty : Registry
    {
        public BankJobRegisty()
        {
            Schedule<ExecuteDailyTransactionsJob>().ToRunEvery(1).Days().At(22, 20);
            Schedule<CalculateProfitJob>().ToRunEvery(1).Days().At(23, 20);
            //Schedule<ExecuteDailyTransactionsJob>().ToRunEvery(30).Seconds();
            //Schedule<CalculateProfitJob>().ToRunEvery(30).Seconds();
        }
    }
}
