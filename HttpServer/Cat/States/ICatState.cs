namespace HttpServer.States;

public interface ICatState
{
    public string NameState { get; set; }
    public Cat? ToGame(Cat? cat);
    public Cat? ToSleep(Cat? cat);
    public Cat? ToEat(Cat? cat);
    public Cat? UpdateCatParam(Cat? cat);
}