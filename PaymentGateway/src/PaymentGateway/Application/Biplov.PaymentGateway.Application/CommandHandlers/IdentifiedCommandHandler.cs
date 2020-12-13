using System.Threading;
using System.Threading.Tasks;
using Biplov.Common.Core;
using Biplov.EventBus.Extensions;
using Biplov.PaymentGateway.Application.Commands;
using Biplov.PaymentGateway.Infrastructure.Idempotency;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Biplov.PaymentGateway.Application.CommandHandlers
{
    /// <summary>
    /// Provides a base implementation for handling duplicate request and ensuring idempotent updates, in the cases where
    /// a requestid sent by client is used to detect duplicate requests.
    /// </summary>
    /// <typeparam name="T">Type of the command handler that performs the operation if request is not duplicated</typeparam>
    /// <typeparam name="R">Return value of the inner command handler</typeparam>
    public class IdentifiedCommandHandler<T, R> : IRequestHandler<IdentifiedCommand<T, R>, Result>
        where T : Command, IRequest<R>
        where R : Result

    {
        private readonly IMediator _mediator;
        private readonly IRequestManager _requestManager;
        private readonly ILogger<IdentifiedCommandHandler<T, R>> _logger;

        public IdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<T, R>> logger)
        {
            _mediator = mediator;
            _requestManager = requestManager;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Creates the result value to return if a previous request was found
        /// </summary>
        /// <returns></returns>
        protected virtual Result CreateResultForDuplicateRequest()
        {
            return default;
        }

        /// <summary>
        /// This method handles the command. It just ensures that no other request exists with the same ID, and if this is the case
        /// just enqueues the original inner command.
        /// </summary>
        /// <param name="message">IdentifiedCommand which contains both original command & request ID</param>
        /// <returns>Return value of inner command or default value if request same ID was found</returns>
        public async Task<Result> Handle(IdentifiedCommand<T, R> message, CancellationToken cancellationToken)
        {
            var alreadyExists = await _requestManager.ExistAsync(message.Command.CommandId);
            if (alreadyExists)
                return CreateResultForDuplicateRequest();

            await _requestManager.CreateRequestForCommandAsync<T>(message.Command.CommandId);

            var command = message.Command;
            var commandName = command.GetGenericTypeName();
            var commandId = message.Command.CommandId;

            _logger.LogInformation(
                "----- Sending command: {CommandName} : {CommandId} ({@Command}) and CorrlationId : {correlationId}", commandName, commandId,
                command, command.CorrelationId);

            // Send the embeded business command to mediator so it runs its related CommandHandler 
            var result = await _mediator.Send(command, cancellationToken);

            _logger.LogInformation(
                "----- Command result: {@Result} - {CommandName} : {CommandId} ({@Command}) and CorrelationId : {correlationId}", result,
                commandName, commandId, command, command.CorrelationId);

            return result;
        }
    }
}
