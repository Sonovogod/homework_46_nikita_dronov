using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace HttpServer.json;

public class JsonManager
{
    public readonly string Path = "../../../json/catsJson.json";
    private JsonSerializerOptions? _options;

    public JsonManager()
    {
        _options = new JsonSerializerOptions()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };
    }
    
    public void CatsSerializer(List<Cat?> cats)
    {
        var CatSerialize = JsonSerializer.Serialize(cats, _options);
        File.WriteAllText(Path, CatSerialize);
    }

    public List<Cat?> CatsDeserializer()
    {
        List<Cat?> cats = new List<Cat?> { };
        try
        {
            var jsonFile = File.ReadAllText(Path);
            cats = JsonSerializer.Deserialize<List<Cat>>(jsonFile);
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine("Json файл не найден");
        }
        return cats;
    }
}