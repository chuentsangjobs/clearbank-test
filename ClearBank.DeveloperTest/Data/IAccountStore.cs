using ClearBank.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClearBank.DeveloperTest.Data
{
    public interface IAccountStore
    {
        Account GetAccount(string accountNumber);
        void UpdateAccount(Account account);
    }
}
