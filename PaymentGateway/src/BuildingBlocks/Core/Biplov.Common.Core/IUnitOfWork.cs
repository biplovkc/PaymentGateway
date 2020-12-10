using System;
using System.Threading;
using System.Threading.Tasks;

namespace Biplov.Common.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        Task<Result> SaveEntitiesAsync(CancellationToken cancellationToken = default);
    }
}
