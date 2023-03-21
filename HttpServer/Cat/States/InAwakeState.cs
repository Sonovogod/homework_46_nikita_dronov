namespace HttpServer.States;

public class InAwakeState : ICatState
{
    public string NameState { get; set; }
    private Random _random;

    public InAwakeState()
    {
        NameState = "БОДРСТВУЕТ";
        _random = new Random();
    }
    public Cat? ToGame(Cat? cat)
    {
        int chance =_random.Next(1, 4);
        if (chance == 1)
        {
            cat.HappinessLevel = 0;
            cat.SatietyLevel -= 10;
            cat.Message = "Котан в ярости!!!!";
            cat.Message += "\nУровень счастья понижен до 0";
            cat.Message += "\nУровень стытости понижен на 10";
        }
        else
        {
            cat.HappinessLevel += 15;
            cat.SatietyLevel -= 10;
            cat.Message = "Уровень счастья повышен на 15";
            cat.Message += "\nУровень стытости понижен на 10";
        }
        cat = UpdateCatParam(cat);
        
        return cat;
    }

    public Cat? ToSleep(Cat? cat)
    {
        cat.HappinessLevel += 15;
        cat.Message = "Кот спит, уровень счастья повышен на 15";
        cat.CatState = new InSleepState();
        cat = UpdateCatParam(cat);
        
        return cat;
    }

    public Cat? ToEat(Cat? cat)
    {
        cat.HappinessLevel += 5;
        cat.SatietyLevel += 15;
        cat.Message = "Кот плотно покушал, уровень счастья повышен на 5";
        cat.Message += "\nУровень стытости повышен на 15";
        cat = UpdateCatParam(cat);

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
            cat.HappinessLevel = minLevel;
            cat.SatietyLevel = minLevel;
            cat.Message = "Спасибо хозяин за заботу...";
            cat.TotalCatState = cat.CatState.NameState;
            cat.ImagePath =
                "https://upload.wikimedia.org/wikipedia/ru/0/02/%D0%9A%D0%BB%D0%B0%D0%B4%D0%B1%D0%B8%D1%89%D0%B5_%D0%B4%D0%BE%D0%BC%D0%B0%D1%88%D0%BD%D0%B8%D1%85_%D0%B6%D0%B8%D0%B2%D0%BE%D1%82%D0%BD%D1%8B%D1%85.jpg";
        }
        else if (cat.HappinessLevel >= 50 && cat.SatietyLevel >= 50)
        {
            cat.ImagePath = "https://celes.club/uploads/posts/2022-10/1666816047_1-celes-club-p-dovolnii-kotik-pinterest-1.jpg";
        }
        else if (cat.HappinessLevel < 50 || cat.SatietyLevel < 50)
        {
            cat.ImagePath = "https://icdn.lenta.ru/images/2020/09/30/13/20200930130228617/detail_9ad62f72eb0b24b9b8f76677d3a1c605.jpg";
        }

        return cat;
    }
}