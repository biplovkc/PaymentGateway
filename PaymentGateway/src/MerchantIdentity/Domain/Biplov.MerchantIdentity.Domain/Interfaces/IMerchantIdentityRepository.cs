using System;
using System.Threading.Tasks;
using Biplov.Common.Core;
using Biplov.MerchantIdentity.Domain.Entities;

namespace Biplov.MerchantIdentity.Domain.Interfaces
{
    public interface IMerchantIdentityRepository: IRepository<Merchant>
    {
        Task<Merchant> GetByIdAsync(Guid id);

        Task<Merchant> GetByEmailAsync(string email);
        Merchant Add(Merchant merchant);

        Merchant Update(Merchant merchant);
    }
}
