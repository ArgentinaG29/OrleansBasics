using System;
using System.Collections.Generic;
using System.Text;
using GrainInterfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Grains
{
    public class AccountGrain : Orleans.Grain, IAccount
    {
        private readonly ILogger _logger;
        public int saldo = 1000;

        public AccountGrain(ILogger<AccountGrain> logger) => _logger = logger;

        Task<string> IAccount.Debito(int cantidad)
        {
            if(saldo < cantidad)
            {
                return Task.FromResult($"\n Saldo insuficiente");
            }
            saldo -= cantidad;
            return Task.FromResult($"\n Se han debitado '{cantidad}', ahora el saldo es:  "+ saldo);
        }

        Task<string> IAccount.Deposito(int cantidad)
        {
            saldo += cantidad;
            return Task.FromResult($"\n Se han depositado '{cantidad}', ahora el saldo es:  " + saldo);
        }
    }
}
