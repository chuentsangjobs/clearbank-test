using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClearBank.DeveloperTest.Services;
using System;
using System.Collections.Generic;
using System.Text;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Tests.Mocks;

namespace ClearBank.DeveloperTest.Services.Tests
{
    [TestClass()]
    public class PaymentServiceTests
    {
        [TestMethod()]
        public void givenAccountWithFunds_whenFasterPayment_thenSuccess()
        {
            // Arange
            var service = new PaymentService(new AccountStoreFactoryMock());
            var request = new MakePaymentRequest();
            request.DebtorAccountNumber = "ukfp-123";
            request.PaymentScheme = PaymentScheme.FasterPayments;
            request.Amount = 50;

            // Act
            var result = service.MakePayment(request);

            // Asert
            Assert.IsTrue(result.Success);
        }

        [TestMethod()]
        public void givenUnspecifiedAccount_whenBacsPayment_thenFailure()
        {
            // Arange
            var service = new PaymentService();
            var request = new MakePaymentRequest();

            // Act
            var result = service.MakePayment(request);

            // Asert
            Assert.IsFalse(result.Success);
        }
    }
}