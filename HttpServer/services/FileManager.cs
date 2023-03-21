namespace HttpServer.services;

public class FileManager
{
    public string GetContent(string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException();
        return File.ReadAllText(filePath);
    }
}