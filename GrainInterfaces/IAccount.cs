using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GrainInterfaces
{
    public interface IAccount: Orleans.IGrainWithIntegerKey
    {
        Task<string> Deposito(int cantidad);
        Task<string> Debito(int cantidad);
    }
}
