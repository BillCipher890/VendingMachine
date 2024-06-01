namespace VendingMachine.Server.Autentification
{
    public interface IChecker<T>
    {
        T Right { get; set; }

        public bool Check(T value);
    }
}
