using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;

namespace Vault.Schedule
{
    public class BankJobFactory : IJobFactory
    {
        private readonly ServiceProvider provider;

        public BankJobFactory(ServiceProvider provider)
        {
            this.provider = provider;
        }

        public IJob GetJobInstance<T>() where T : IJob
        {
            return provider.GetService<T>();
        }
    }
}
