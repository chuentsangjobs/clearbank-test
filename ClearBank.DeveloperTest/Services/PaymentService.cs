using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Rules;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private PaymentUtils _utils;
        public PaymentService(IAccountStoreFactory accountStoreFactory)
        {
            _utils = new PaymentUtils(accountStoreFactory);
        }
        public PaymentService()
        {
            _utils = new PaymentUtils(null);
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            Account account = _utils.GetAccount(request.DebtorAccountNumber);

            var result = new MakePaymentResult();
            result.Success = PaymentValidator.Validate(request, account);

            if (result.Success)
            {
                account.Balance -= request.Amount;
                _utils.UpdateAccount(account);                
            }

            return result;
        }

    }
}
