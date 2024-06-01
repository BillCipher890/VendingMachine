using Microsoft.AspNetCore.Mvc;

namespace VendingMachine.Server.Resources
{
    public interface ILoader
    {
        IActionResult TryLoad(string path, string type);
    }
}
