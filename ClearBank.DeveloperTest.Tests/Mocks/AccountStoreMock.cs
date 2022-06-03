using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Tests.Mocks
{
    class AccountStoreMock : IAccountStore
    {
        Account IAccountStore.GetAccount(string accountNumber)
        {
            if (accountNumber == "ukfp-123")
            {
                Account acc = new Account();
                acc.AccountNumber = "ukfp-123";
                acc.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments;
                acc.Balance = 100;
                return acc;
            }
            return null;
        }

        void IAccountStore.UpdateAccount(Account account)
        {
        }
    }

    class AccountStoreFactoryMock : IAccountStoreFactory
    {
        IAccountStore IAccountStoreFactory.GetAccountStore(string key)
        {
            return new AccountStoreMock();
        }
    }
}
