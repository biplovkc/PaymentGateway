using System.Threading;
using System.Threading.Tasks;
using Biplov.BankService;
using Biplov.PaymentGateway.Application.Queries;
using Biplov.PaymentGateway.Domain.Enum;
using Biplov.PaymentGateway.Domain.Events;
using Biplov.PaymentGateway.Domain.Interfaces;
using MediatR;

namespace Biplov.PaymentGateway.Application.DomainEventHandler
{
    public class PaymentInitiatedDomainEventHandler : INotificationHandler<PaymentInitiatedDomainEvent>
    {
        private readonly IBankService _bankService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ICardQuery _cardQuery;

        public PaymentInitiatedDomainEventHandler(IBankService bankService, IPaymentRepository paymentRepository, ICardQuery cardQuery)
        {
            _bankService = bankService;
            _paymentRepository = paymentRepository;
            _cardQuery = cardQuery;
        }

        public async Task Handle(PaymentInitiatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var card = await _cardQuery.GetCardAsync(notification.CardToken);
            var recipient = new Biplov.BankService.PaymentRecipient
            {
                AccountNumber = notification.Recipient.AccountNumber,
                DateOfBirth = notification.Recipient.DateOfBirth.ToString("d"),
                FirstName = notification.Recipient.FirstName,
                LastName = notification.Recipient.LastName,
                Zip = notification.Recipient.ZipCode
            };

            var bankCard = new Card
            {
                Cvv = card.Cvv,
                ExpiryYear = card.ExpiryYear,
                ExpiryMonth = card.ExpiryMonth,
                Number = card.Number,
                Name = card.Name
            };
            var paymentResult = await _bankService.HandlePaymentAsync(notification.Amount, notification.Currency,
                recipient, notification.Description,
                bankCard);
            
            var payment = await _paymentRepository.GetByPaymentIdAsync(notification.PaymentId);
            if (paymentResult.IsSuccess)
            {
                payment.ChangePaymentStatus(PaymentStatus.Success);

                // TODO : Fire new command to notify merchant of success result
            }
            else
            {
                payment.ChangePaymentStatus(PaymentStatus.Rejected);

                // TODO : Fire new command to notify merchant of failed result
            }

            _paymentRepository.Update(payment);

            await _paymentRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
