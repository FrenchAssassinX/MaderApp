using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serializer
{
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

[System.Serializable]
public class Token
{
    public string token;
}

[System.Serializable]
public class Text
{
    public string text;
}

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

[System.Serializable]
public class RootObject
{
    public Token token;
    public Text text;
    public User user;
}
