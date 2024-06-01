namespace VendingMachine.Server.DrinkLogic
{
    public interface ISaver
    {
        string Save(IFormCollection? form);
    }
}
