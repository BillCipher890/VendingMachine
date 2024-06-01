namespace VendingMachine.Server.Autentification
{
    public class PasswordChecker : IChecker<string>
    {
        public string Right { get; set; }

        public PasswordChecker(string right)
        {
            Right = right;
        }

        public bool Check(string password)
        {
            return Right == password;
        }
    }
}
