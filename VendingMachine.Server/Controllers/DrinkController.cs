using Microsoft.AspNetCore.Mvc;
using VendingMachine.Server.Models;
using VendingMachine.Server.Services;

namespace VendingMachine.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrinkController : Controller
    {
        private readonly IDrinkService _drinkService;

        public DrinkController(IDrinkService drinkService)
        {
            _drinkService = drinkService;
        }

        [HttpGet("AllDrink", Name = "allDrink")]
        public async Task<IEnumerable<Drink>> GetAll()
        {
            return await _drinkService.GetAll();
        }

        [HttpPost("UpdateDrink", Name = "updateDrink")]
        public async Task<IActionResult> UpdateDrink(Drink drink)
        {
            return await _drinkService.UpdateDrink(drink);
        }

        [HttpGet("GetImage", Name = "getImage")]
        public async Task<IActionResult> GetImage(string guid)
        {
            return await _drinkService.GetImage(guid);
        }

        [HttpPost("AddDrink", Name = "addDrink")]
        public async Task<IActionResult> AddDrink(Drink drink)
        {
            return await _drinkService.AddDrink(drink);
        }

        [HttpPost("UploadImage", Name = "uploadImage")]
        public async Task<IActionResult> UploadImage()
        {
            return await _drinkService.UploadImage();
        }

        [HttpDelete("DeleteDrink", Name = "deleteDrink")]
        public async Task<IActionResult> DeleteDrink(Drink drink)
        {
            return await _drinkService.DeleteDrink(drink);
        }

        [HttpPost("ClientGetDrink", Name = "clientGetDrink")]
        public async Task<IActionResult> ClientGetDrink(PurchaseData data)
        {
            return await _drinkService.ClientGetDrink(data);
        }
    }
}
