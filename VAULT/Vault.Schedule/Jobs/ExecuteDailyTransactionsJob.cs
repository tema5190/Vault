using FluentScheduler;
using System;
using System.Diagnostics;
using Vault.Services;

namespace Vault.Schedule
{
    public class ExecuteDailyTransactionsJob : IJob
    {
        private readonly BankOperationService _service;

        public ExecuteDailyTransactionsJob(BankOperationService service)
        {
            _service = service;
        }

        public void Execute()
        {
            //Debug.WriteLine(DateTime.Now.ToLongTimeString() + "trans");
            _service.PerformAllTransactionsInQueue();
        }
    }
}
