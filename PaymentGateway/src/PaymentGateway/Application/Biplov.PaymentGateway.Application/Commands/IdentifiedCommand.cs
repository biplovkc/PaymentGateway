using MediatR;

namespace Biplov.PaymentGateway.Application.Commands
{
    public class IdentifiedCommand<T, R> : IRequest<R>
        where T : IRequest<R>
    {
        public Command Command { get; }
        public IdentifiedCommand(Command command)
        {
            Command = command;
        }
    }
}
