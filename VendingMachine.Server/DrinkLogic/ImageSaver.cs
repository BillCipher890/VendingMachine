namespace VendingMachine.Server.DrinkLogic
{
    public class ImageSaver : ISaver
    {
        public string Save(IFormCollection? form)
        {
            if (form == null)
                throw new Exception("Данные для сохранения отсутствуют.");

            var file = form.Files[0];
            var guid = form["guid"];
            var filePath = Path.Combine("./Images/", guid + '.' + file.FileName.Split('.').LastOrDefault());

            using var stream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(stream);

            return filePath;
        }
    }
}
