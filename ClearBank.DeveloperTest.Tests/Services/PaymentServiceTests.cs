using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using ClearBank.DeveloperTest.Rules;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Moq;
using Xunit;

namespace ClearBank.DeveloperTest.Services.Tests
{
    public class PaymentServiceTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IPaymentValidator> _paymentValidator;
        private readonly Mock<IPaymentUtils> _paymentUtils;

        public PaymentServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _paymentValidator = _fixture.Freeze<Mock<IPaymentValidator>>();
            _paymentUtils = _fixture.Freeze<Mock<IPaymentUtils>>();
        }

        [Theory, AutoData]
        public void GivenAnUnspecifiedAccount_ThenMakePayment_ShouldBeUnsuccessful(MakePaymentRequest paymentRequest)
        {
            // Arrange
            _paymentUtils.Setup(x => x.GetAccount(paymentRequest.DebtorAccountNumber))
                .Returns<Account>(null);

            var sut = _fixture.Create<PaymentService>();

            // Act
            var result = sut.MakePayment(paymentRequest);

            // Asert
            result.Success.Should().BeFalse();
            _paymentUtils.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Theory, AutoData]
        public void GivenAnInvalidPaymentAmount_ThenMakePayment_ShouldBeUnsuccessful(MakePaymentRequest paymentRequest)
        {
            // Arrange
            paymentRequest.Amount = 0;

            var sut = _fixture.Create<PaymentService>();

            // Act
            var result = sut.MakePayment(paymentRequest);

            // Asert
            result.Success.Should().BeFalse();
            _paymentUtils.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Theory, AutoData]
        public void GivenAValidAccount_ThenMakePayment_ShouldBeSuccessful(MakePaymentRequest paymentRequest)
        {
            // Arrange
            _paymentValidator.Setup(x => x.Validate(paymentRequest, It.IsAny<Account>()))
                .Returns(true);

            var sut = _fixture.Create<PaymentService>();    

            // Act
            var result = sut.MakePayment(paymentRequest);

            // Asert
            result.Success.Should().BeTrue();
            _paymentUtils.Verify(x => x.UpdateAccount(It.IsAny<Account>()));
        }

        [Theory, AutoData]
        public void GivenAValidAccount_ThenMakePayment_ShouldUpdateAccountBalance(MakePaymentRequest paymentRequest, Account account)
        {
            // Arrange
            paymentRequest.Amount = 100;
            account.Balance = 300;
            _paymentUtils.Setup(x => x.GetAccount(paymentRequest.DebtorAccountNumber))
                .Returns(account);
            _paymentValidator.Setup(x => x.Validate(paymentRequest, It.IsAny<Account>()))
                .Returns(true);

            var sut = _fixture.Create<PaymentService>();

            // Act
            var result = sut.MakePayment(paymentRequest);

            // Asert
            result.Success.Should().BeTrue();
            _paymentUtils.Verify(x => x.UpdateAccount(It.Is<Account>(x=> 
                x.AccountNumber.Equals(account.AccountNumber) 
                && x.Balance.Equals(200))));
        }

        [Theory, AutoData]
        public void GivenAnInvalidAccount_ThenMakePayment_ShouldBeUnsuccessful(MakePaymentRequest paymentRequest)
        {
            // Arrange
            _paymentValidator.Setup(x => x.Validate(paymentRequest, It.IsAny<Account>()))
                .Returns(false);

            var sut = _fixture.Create<PaymentService>();

            // Act
            var result = sut.MakePayment(paymentRequest);

            // Asert
            result.Success.Should().BeFalse();
            _paymentUtils.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }
    }
}