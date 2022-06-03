using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Rules
{
    public interface IPaymentValidator
    {
        bool Validate(MakePaymentRequest request, Account account);
    }
}