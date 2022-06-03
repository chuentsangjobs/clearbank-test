using AutoFixture.Xunit2;
using ClearBank.DeveloperTest.Rules;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Rules
{
    public class PaymentValidatorTests
    {
        private readonly PaymentValidator _sut;

        public PaymentValidatorTests()
        {
            _sut = new PaymentValidator();
        }

        [Theory]
        [InlineAutoData(PaymentScheme.Bacs)]
        [InlineAutoData(PaymentScheme.FasterPayments)]
        [InlineAutoData(PaymentScheme.Chaps)]
        public void ShouldReturnFalse_WhenAccountIsNull(PaymentScheme paymentScheme)
        {
            // Arrange
            Account account = null;
            var request = new MakePaymentRequest { PaymentScheme = paymentScheme };

            // Act
            var isValid = _sut.Validate(request, account);

            // Assert
            isValid.Should().BeFalse();
        }

        [Theory]
        [InlineAutoData(PaymentScheme.Bacs, AllowedPaymentSchemes.FasterPayments)]
        [InlineAutoData(PaymentScheme.Bacs, AllowedPaymentSchemes.Chaps)]
        [InlineAutoData(PaymentScheme.FasterPayments, AllowedPaymentSchemes.Bacs)]
        [InlineAutoData(PaymentScheme.FasterPayments, AllowedPaymentSchemes.Chaps)]
        [InlineAutoData(PaymentScheme.Chaps, AllowedPaymentSchemes.Bacs)]
        [InlineAutoData(PaymentScheme.Chaps, AllowedPaymentSchemes.FasterPayments)]
        public void ShouldReturnFalse_WhenRequestPaymentScheme_IsDifferentFromAccountAllowedPaymentScheme(PaymentScheme paymentScheme, AllowedPaymentSchemes allowedPaymentSchemes)
        {
            // Arrange
            var account = new Account { AllowedPaymentSchemes = allowedPaymentSchemes };
            var request = new MakePaymentRequest { PaymentScheme = paymentScheme };

            // Act
            var isValid = _sut.Validate(request, account);

            // Assert
            isValid.Should().BeFalse();
        }

        [Theory, AutoData]
        public void ShouldReturnTrue_WhenAccountIsBacs_AndPaymentRequestIsBacs(Account account, MakePaymentRequest paymentRequest)
        {
            // Arrange
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs;
            paymentRequest.PaymentScheme = PaymentScheme.Bacs;

            // Act
            var isValid = _sut.Validate(paymentRequest, account);

            // Assert
            isValid.Should().BeTrue();
        }

        [Theory, AutoData]
        public void ShouldReturnTrue_WhenAccountIsFasterPayment_AndPaymentRequestIsFasterPayment(Account account, MakePaymentRequest paymentRequest)
        {
            // Arrange
            account.Balance = 300;
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments;
            paymentRequest.PaymentScheme = PaymentScheme.FasterPayments;
            paymentRequest.Amount = account.Balance;

            // Act
            var isValid = _sut.Validate(paymentRequest, account);

            // Assert
            isValid.Should().BeTrue();
        }

        [Theory, AutoData]
        public void ShouldReturnFalse_WhenAccountIsFasterPayment_AndPaymentRequestIsFasterPayment_AndAccountHasInsufficentFunds(Account account, MakePaymentRequest paymentRequest)
        {
            // Arrange
            account.Balance = 300;
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments;
            paymentRequest.PaymentScheme = PaymentScheme.FasterPayments;
            paymentRequest.Amount = account.Balance + 1;

            // Act
            var isValid = _sut.Validate(paymentRequest, account);

            // Assert
            isValid.Should().BeFalse();
        }

        [Theory, AutoData]
        public void ShouldReturnTrue_WhenAccountIsChaps_AndPaymentRequestIsChaps(Account account, MakePaymentRequest paymentRequest)
        {
            // Arrange
            account.Status = AccountStatus.Live;
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps;
            paymentRequest.PaymentScheme = PaymentScheme.Chaps;

            // Act
            var isValid = _sut.Validate(paymentRequest, account);

            // Assert
            isValid.Should().BeTrue();
        }

        [Theory]
        [InlineAutoData(AccountStatus.Disabled)]
        [InlineAutoData(AccountStatus.InboundPaymentsOnly)]
        public void ShouldReturnFalse_WhenAccountIsChaps_AndPaymentRequestIsChaps_AndAccountStatusIsNotLive(AccountStatus accountStatus, MakePaymentRequest paymentRequest)
        {
            // Arrange
            var account = new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps, Status = accountStatus };
            paymentRequest.PaymentScheme = PaymentScheme.Chaps;

            // Act
            var isValid = _sut.Validate(paymentRequest, account);

            // Assert
            isValid.Should().BeFalse();
        }
    }
}
