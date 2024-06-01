using Microsoft.AspNetCore.Mvc;
using VendingMachine.Server.Models;

namespace VendingMachine.Server.Services
{
    public interface IDrinkService
    {
        Task<IEnumerable<Drink>> GetAll();

        Task<IActionResult> UpdateDrink(Drink drink);

        Task<IActionResult> GetImage(string guid);

        Task<IActionResult> AddDrink(Drink drink);

        Task<IActionResult> UploadImage();

        Task<IActionResult> DeleteDrink(Drink drink);

        Task<IActionResult> ClientGetDrink(PurchaseData data);
    }
}
