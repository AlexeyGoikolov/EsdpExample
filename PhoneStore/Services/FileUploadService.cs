namespace PhoneStore.Services;

public class FileUploadService
{
    public async Task UploadFile(string path, string fileName, IFormFile file)
    {
        await using var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
        await file.CopyToAsync(stream);
    }
}