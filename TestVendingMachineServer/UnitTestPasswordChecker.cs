using VendingMachine.Server.Autentification;

namespace TestVendingMachineServer
{
    public class UnitTestPasswordChecker
    {
        [Fact]
        public void Check_CorrectPassword_ReturnsTrue()
        {
            string password = "password123";
            PasswordChecker checker = new PasswordChecker(password);

            bool result = checker.Check(password);

            Assert.True(result);
        }

        [Fact]
        public void Check_IncorrectPassword_ReturnsFalse()
        {
            string password = "password123";
            string wrongPassword = "pass123";
            PasswordChecker checker = new PasswordChecker(password);

            bool result = checker.Check(wrongPassword);

            Assert.False(result);
        }
    }
}