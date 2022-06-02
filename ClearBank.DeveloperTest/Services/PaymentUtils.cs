using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using System.Configuration;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentUtils
    {
        IAccountStoreFactory _accountStoreFactory;
        public PaymentUtils(IAccountStoreFactory accountStoreFactory)
        {
            _accountStoreFactory = accountStoreFactory;
        }

        public void UpdateAccount(Account account)
        {
            IAccountStore store = GetAccountStore(_accountStoreFactory);
            store.UpdateAccount(account);
        }

        private static IAccountStore GetAccountStore(IAccountStoreFactory accountStoreFactory)
        {
            IAccountStore accountStore = null;
            if (accountStoreFactory != null)
            {
                accountStore = accountStoreFactory.GetAccountStore(null);
            }
            else
            {
                var dataStoreType = ConfigurationManager.AppSettings["DataStoreType"];

                if (dataStoreType == "Backup")
                {
                    accountStore = new BackupAccountDataStore();
                }
                else
                {
                    accountStore = new AccountDataStore();
                }
            }
            return accountStore;
        }

        public Account GetAccount(string debtorAccountNumber)
        {
            IAccountStore store = GetAccountStore(_accountStoreFactory);
            return store.GetAccount(debtorAccountNumber);
        }
    }
}
