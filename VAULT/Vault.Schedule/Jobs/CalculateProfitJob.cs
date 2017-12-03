using FluentScheduler;
using System;
using System.Diagnostics;
using Vault.Services;

namespace Vault.Schedule
{
    public class CalculateProfitJob : IJob
    {
        private readonly BankOperationService _service;

        public CalculateProfitJob(BankOperationService service)
        {
            _service = service;
        }

        public void Execute()
        {
            Debug.WriteLine(DateTime.Now.ToLongTimeString() + "profit");
            //_service.CalculateGoalsSumWithProfit();
        }
    }
}
