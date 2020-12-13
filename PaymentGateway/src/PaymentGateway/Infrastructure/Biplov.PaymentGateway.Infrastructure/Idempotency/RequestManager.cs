using System;
using System.Threading.Tasks;
using Biplov.PaymentGateway.Infrastructure.Persistence;

namespace Biplov.PaymentGateway.Infrastructure.Idempotency
{
    public class RequestManager : IRequestManager
    {
        private readonly PaymentContext _context;

        public RequestManager(PaymentContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> ExistAsync(string id)
        {
            var request = await _context.FindAsync<ClientRequest>(id);

            return request != null;
        }

        public async Task CreateRequestForCommandAsync<T>(string id)
        {
            var exists = await ExistAsync(id);

            var request = exists
                ? throw new InvalidOperationException($"Request with {id} already exists")
                : new ClientRequest
                {
                    Id = id,
                    Name = typeof(T).Name,
                    Time = DateTime.UtcNow
                };

            _context.Add<ClientRequest>(request);

            await _context.SaveChangesAsync();
        }
    }
}
