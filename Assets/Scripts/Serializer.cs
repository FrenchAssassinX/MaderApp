using UnityEngine;

public class Serializer
{
    /* Array for all the datas of the JSON file returned by the database */
    public static T[] getJsonArray<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}

/* Token constructor */
[System.Serializable]
public class Token
{
    public string token;
}

/* Text constructor */
[System.Serializable]
public class Text
{
    public string text;
}

/* User constructor */
[System.Serializable]
public class User
{
    public string email;
    public string _id;
    public string matricule;
    public string password;
    public string nom;
    public string prenom;
    public string created_at;
    public string updated_at;
    public string __v;
}

/* Root object constructor */
[System.Serializable]
public class RootObject
{
    public Token token;
    public Text text;
    public User user;
    internal string message;
}