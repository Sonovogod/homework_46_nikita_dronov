namespace HttpServer.States;

public class DeathState : ICatState
{
    public string NameState { get; set; }

    public DeathState()
    {
        NameState = "ПОМЕР";
    }

    public Cat? ToGame(Cat? cat)
    {
        cat.Message = "Кот мертв и не хочет играть с тобой, живодер";
        return cat;
    }

    public Cat? ToSleep(Cat? cat)
    {
        cat.Message = "Кот и так уснул, навсегда";
        return cat;
    }

    public Cat? ToEat(Cat? cat)
    {
        cat.Message = "Кот сыт твоей заботой настолько, что помер...";
        return cat;
    }

    public Cat? UpdateCatParam(Cat? cat)
    {
        return cat;
    }
}