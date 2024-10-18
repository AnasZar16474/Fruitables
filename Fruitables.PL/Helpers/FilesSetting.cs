namespace Fruitables.PL.Helpers
{
    public class FilesSetting
    {
        public static string UploadFile(IFormFile File, string FolderName)
        {
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", FolderName);
            string FileName = $"{Guid.NewGuid()}{File.FileName}";
            string FilePath = Path.Combine(FolderPath, FileName);
            var FileStream = new FileStream(FilePath, FileMode.Create);
            File.CopyTo(FileStream);
            return FileName;
        }
        public static void DeleteFile(string FileName, string FolderName)
        {
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", FolderName, FileName);
            if (File.Exists(FilePath))
            {
                GC.Collect(); // يجبر جامع القمامة على تحرير مقابض الملفات
                GC.WaitForPendingFinalizers();
                File.Delete(FilePath);

            }
            else
            {
                Console.WriteLine("Anas");
            }
        }
    }
}
