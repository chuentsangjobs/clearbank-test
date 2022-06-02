using System;
using System.Collections.Generic;
using System.Text;

namespace ClearBank.DeveloperTest.Data
{
    public interface IAccountStoreFactory
    {
        IAccountStore GetAccountStore(string key);
    }
}
