using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

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
    public List<EstimationId> estimation;
    public string _id;
    public string name;
    public string date;
    public string phone;
    public string email;
    public string __v;
    public string reference;
    public string customer;
}

/* Project request constructor */
[System.Serializable]
public class RequestAProject
{
    public string message;
    public Project result;
    public Customer customer;
}

/* Projects request constructor */
[System.Serializable]
public class RequestGetAllProject
{
    public string message;
    public List<Project> projects;
}

[System.Serializable]
public class CreateProject
{
    public string userID;
    public string date;
    public string road;
    public string roadNum;
    public string roadExtra;
    public string zipcode;
    public string city;
    public string customerID;
}

/* Estimation request constructor */
[System.Serializable]
public class RequestAnEstimation
{
    public string message;
    public Estimation estimation;
}

/* Estimation object constructor */
[System.Serializable]
public class Estimation
{
    public string id;
    public string price;
    public string state;
    public string date; 
}

/* Estimation object constructor */
[System.Serializable]
public class EstimationId
{
    public string id;
}

[System.Serializable]
public class EstimationList
{
    public List<Estimation> estimations;
}

/*Customer constructor*/
 [System.Serializable] 
public class CreateCustomer
{
    public string _id;
    public string name;
    public string surename;
    public string road;
    public string roadNum;
    public string zipcode;
    public string city;
    public string roadExtra;
    public string phone;
    public string email;
    public string __v;
}

[System.Serializable]
public class RequestGetAllCustomer
{
    public string message;
    public List<Customer> customers;

}

/*Customer constructor*/
[System.Serializable]
public class Customer
{
    public string _id;
    public string name;
    public string surename;
    public string road;
    public string roadNum;
    public string zipcode;
    public string city;
    public string roadExtra;
    public string phone;
    public string email;
    public string __v;
}

[System.Serializable]
public class CreateModules
{
    public string _id;
    public string name;
    public string cost;
    public string angle;
    public string cctp;
    public string cut;
    public string range;
    public List<Component> composants;
}

[System.Serializable]
public class Component
{
    public string id;
    public string qte;
}