using System;
using System.Threading.Tasks;
using Biplov.Common.Core;
using Biplov.PaymentGateway.Domain.Entities;

namespace Biplov.PaymentGateway.Domain.Interfaces
{
    public interface IMerchantRepository : IRepository<Merchant>
    {
        Task<Merchant> GetByIdentityIdAsync(Guid id);

        Task<Merchant> GetByEmailAsync(string email);
        Merchant Add(Merchant merchant);

        Merchant Update(Merchant merchant);
    }
}
