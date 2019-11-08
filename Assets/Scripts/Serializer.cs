using System.Collections.Generic;
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
    public string token;
    public Text text;
    public User user;
    internal string message;
}

/* Invoice object constructor */
[System.Serializable]
public class Invoice
{
    public string id;
}

/* Payement object constructor */
[System.Serializable]
public class Payement
{
    public string id;
}

/* Estimation object constructor */
[System.Serializable]
public class Estimation
{
    public string id;
}

[System.Serializable]
public class User2
{
    public string id;
    public bool incharge;
    public string matricule;
}

/* Project object constructor */
[System.Serializable]
public class Project
{
    public User2 user;
    public List<Invoice> invoice;
    public List<Payement> payement;
    public List<Estimation> estimation;
    public string _id;
    public string name;
    public string date;
    public string road;
    public string roadNum;
    public string zipcode;
    public string city;
    public string roadExtra;
    public string reference;
    public string customer;
}

/* Projects request constructor */
[System.Serializable]
public class RequestGetAllProject
{
    public string message;
    public List<Project> projects;
}