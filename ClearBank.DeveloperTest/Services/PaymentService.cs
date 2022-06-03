using ClearBank.DeveloperTest.Rules;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentUtils _utils;
        private readonly IPaymentValidator _paymentValidator;

        public PaymentService(IPaymentUtils utils, IPaymentValidator paymentValidator)
        {
            _utils = utils;
            _paymentValidator = paymentValidator;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var result = new MakePaymentResult();
            Account account = _utils.GetAccount(request.DebtorAccountNumber);
            if (account == null || request.Amount <= 0)
                return result;

            result.Success = _paymentValidator.Validate(request, account);

            if (result.Success)
            {
                account.Balance -= request.Amount;
                _utils.UpdateAccount(account);                
            }

            return result;
        }
    }
}
