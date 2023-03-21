using System.Text.Json.Serialization;
using HttpServer.States;

namespace HttpServer;

public class Cat
{
    public string Name { get; set; }
    public int Age { get; set; }
    public int HappinessLevel { get; set; }
    public int SatietyLevel { get; set; }
    public string ImagePath { get; set; }
    public string Message { get; set; }
    public string TotalCatState { get; set; }
    [JsonIgnore]
    public ICatState CatState { get; set; }
    private Random _random;

    public Cat(string name)
    {
        _random = new Random();
        Name = name;
        Age = _random.Next(1, 6);
        HappinessLevel = _random.Next(1, 101);
        SatietyLevel = _random.Next(1, 101);
        CatState = new InAwakeState();
        TotalCatState = CatState.NameState;
    }
    
}