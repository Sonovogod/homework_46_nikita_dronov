namespace HttpServer.States;

public class InSleepState : ICatState
{
    public string NameState { get; set; }
    public InSleepState()
    {
        NameState = "УСНУЛ";
    }
    
    public Cat? ToGame(Cat? cat)
    {
        cat.CatState = new InAwakeState();
        cat.TotalCatState = cat.CatState.NameState;
        cat.HappinessLevel -= 5;
        
        cat.Message = "Кот проснулся и слегка недоволен, что его разбудили";
        cat.Message += "\nУровень счастья понизился на 5";

        cat = UpdateCatParam(cat);
        return cat;
    }

    public Cat? ToSleep(Cat? cat)
    {
        cat.Message = "Кот уже спит, сильнее спать не получится";
        return cat;
    }

    public Cat? ToEat(Cat? cat)
    {
        cat.Message = "Кот не может спать и есть одновременно";
        return cat;
    }

    public Cat? UpdateCatParam(Cat? cat)
    {
        int minLevel = 0;
        int maxLevel = 100;
        if (cat.SatietyLevel > maxLevel)
            cat.HappinessLevel -= 30;
        
        cat.HappinessLevel = cat.HappinessLevel < minLevel ? 0 : cat.HappinessLevel;
        cat.SatietyLevel = cat.SatietyLevel < minLevel ? 0 : cat.SatietyLevel;

        cat.HappinessLevel = cat.HappinessLevel > maxLevel ? 100 : cat.HappinessLevel;
        cat.SatietyLevel = cat.SatietyLevel > maxLevel ? 100 : cat.SatietyLevel;

        if (cat.SatietyLevel <= minLevel)
        {
            cat.CatState = new DeathState();
            cat.TotalCatState = cat.CatState.NameState;
            cat.ImagePath =
                "https://upload.wikimedia.org/wikipedia/ru/0/02/%D0%9A%D0%BB%D0%B0%D0%B4%D0%B1%D0%B8%D1%89%D0%B5_%D0%B4%D0%BE%D0%BC%D0%B0%D1%88%D0%BD%D0%B8%D1%85_%D0%B6%D0%B8%D0%B2%D0%BE%D1%82%D0%BD%D1%8B%D1%85.jpg";
        }
        else if (cat.HappinessLevel >= 50 && cat.SatietyLevel >= 50)
        {
            cat.ImagePath = "https://celes.club/uploads/posts/2022-10/1666816047_1-celes-club-p-dovolnii-kotik-pinterest-1.jpg";
        }
        else if (cat.HappinessLevel < 50 && cat.SatietyLevel < 50)
        {
            cat.ImagePath = "https://icdn.lenta.ru/images/2020/09/30/13/20200930130228617/detail_9ad62f72eb0b24b9b8f76677d3a1c605.jpg";
        }

        return cat;
    }
}