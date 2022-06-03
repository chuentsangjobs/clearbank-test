using System;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Rules;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountStore _accountStore;
        private readonly IPaymentValidator _paymentValidator;

        public PaymentService(IAccountStore accountStore, IPaymentValidator paymentValidator)
        {
            _accountStore = accountStore;
            _paymentValidator = paymentValidator;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var accountNumber = request?.DebtorAccountNumber ?? throw new ArgumentNullException(nameof(request.DebtorAccountNumber));

            var result = new MakePaymentResult();
            Account account = _accountStore.GetAccount(accountNumber);

            if (account == null || request.Amount <= 0)
                return result;

            result.Success = _paymentValidator.Validate(request, account);

            if (result.Success)
            {
                account.Balance -= request.Amount;
                _accountStore.UpdateAccount(account);                
            }

            return result;
        }
    }
}
