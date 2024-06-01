using Microsoft.AspNetCore.Mvc;

namespace VendingMachine.Server.Resources
{
    public class FileLoader : Controller, ILoader
    {
        public IActionResult TryLoad(string path, string type)
        {
            try
            {
                if (!System.IO.File.Exists(path))
                    return NotFound();

                var imageBytes = System.IO.File.ReadAllBytes(path);
                return File(imageBytes, type);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Произошла ошибка: {ex.Message}");
            }
        }
    }
}
