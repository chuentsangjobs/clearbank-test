using ClearBank.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClearBank.DeveloperTest.Rules
{
    public class PaymentValidator
    {
        public static bool Validate(MakePaymentRequest request, Account account)
        {
            bool valid = true;
            if (request.PaymentScheme == PaymentScheme.Bacs)
            {
                if (account == null)
                {
                    valid = false;
                }
                else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
                {
                    valid = false;
                }
            }

            if (request.PaymentScheme == PaymentScheme.Chaps)
            {
                if (account == null)
                {
                    valid = false;
                }
                else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
                {
                    valid = false;
                }
                else if (account.Status != AccountStatus.Live)
                {
                    valid = false;
                }
            }

            if (request.PaymentScheme == PaymentScheme.FasterPayments)
            {
                if (account == null)
                {
                    valid = false;
                }
                else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
                {
                    valid = false;
                }
                else if (account.Balance < request.Amount)
                {
                    valid = false;
                }
            }

            return valid;
        }
    }
}
