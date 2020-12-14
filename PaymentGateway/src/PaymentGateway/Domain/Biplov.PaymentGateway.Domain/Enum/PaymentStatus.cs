namespace Biplov.PaymentGateway.Domain.Enum
{
    public enum PaymentStatus
    {
        InProcess = 0,
        Success = 1,
        Rejected = 2,
        FraudDetected = 3,
        InsufficientFund = 4
    }
}
