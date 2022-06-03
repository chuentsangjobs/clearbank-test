using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Data
{
    public interface IAccountStore
    {
        Account GetAccount(string accountNumber);
        void UpdateAccount(Account account);
    }
}
