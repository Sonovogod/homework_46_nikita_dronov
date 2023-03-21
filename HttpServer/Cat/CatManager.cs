using HttpServer.json;
using HttpServer.States;

namespace HttpServer;

public class CatManager
{
    public List<Cat?> Cats { get; set; }
    private JsonManager _jsonManager;

    public CatManager()
    {
        _jsonManager = new JsonManager();
    }

    public Cat? CreteNewCat (string name, out bool isCatExist)
    {
        foreach (var cat in Cats)
        {
            if (string.Equals(cat.Name, name, StringComparison.InvariantCultureIgnoreCase))
            {
                isCatExist = true;
                return cat;
            }
        }
        Cat? newCat = new Cat(name);
        newCat = newCat.CatState.UpdateCatParam(newCat);
        Cats.Add(newCat);
        isCatExist = false;
        _jsonManager.CatsSerializer(Cats);
        
        return newCat;
    }

    public void GetJsonCats()
    {
        Cats = _jsonManager.CatsDeserializer();
        for (int i = 0; i < Cats.Count; i++)
        {
            Cats[i].CatState.NameState = Cats[i].TotalCatState;
        }
    }

    public void PushJsonCats(Cat? cat)
    {
        for (int i = 0; i < Cats.Count; i++)
        {
            if (string.Equals(cat.Name, Cats[i].Name, StringComparison.InvariantCultureIgnoreCase))
            {
                Cats[i] = cat;
            }
        }
        _jsonManager.CatsSerializer(Cats);
    }
}