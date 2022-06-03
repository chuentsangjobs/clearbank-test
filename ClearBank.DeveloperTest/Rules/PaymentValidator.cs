using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Rules
{
    public class PaymentValidator : IPaymentValidator
    {
        public bool Validate(MakePaymentRequest request, Account account)
        {
            if (account == null)
            {
                return false;
            }

            if (request.PaymentScheme == PaymentScheme.Bacs
                && !account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
            {
                return false;
            }

            if (request.PaymentScheme == PaymentScheme.Chaps)
            {
                if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
                {
                    return false;
                }
                if (account.Status != AccountStatus.Live)
                {
                    return false;
                }
            }

            if (request.PaymentScheme == PaymentScheme.FasterPayments)
            {
                if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
                {
                    return false;
                }
                if (account.Balance < request.Amount)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
