using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public interface IPaymentUtils
    {
        Account GetAccount(string debtorAccountNumber);
        void UpdateAccount(Account account);
    }
}