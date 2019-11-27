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
    public string road;
    public string roadNum;
    public string roadExtra;
    public string reference;
    public string zipcode;
    public string city;
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
    public string _id;
    public string price;
    public string state;
    public string date;
    public string discount;
    public List<ModuleId> module;
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
    public string createModule;
    public List<Component> composants;
}

[System.Serializable]
public class Component
{
    public string id;
    public string qte;
}

[System.Serializable]
public class RequestGetAllRange
{
    public string message;
    public List<Range> range;

}

[System.Serializable]
public class Range
{
    public string _id;
    public string libelle;
    public string __v;
}

[System.Serializable]
public class ModuleId
{
    public string id;
}

[System.Serializable]
public class RequestAModule
{
    public string message;
    public Module module;
}

[System.Serializable]
public class Module
{
    public List<ComponentId> components;
    public string _id;
    public string name;
    public string cost;
    public string angle;
    public string cut;
    public string range;
    public string __v; 
}

[System.Serializable]
public class ComponentId
{
    public string id;
    public string qte;
}

[System.Serializable]
public class RequestAComponent
{
    public string message;
    public ComponentToShow component;
}

[System.Serializable]
public class ComponentToShow
{
    public string _id;
    public string code;
    public string unit;
    public string description;
    public string name;
    public string type;
    public string price;
    public Provide provide;
    public string __v;
}

[System.Serializable]
public class Provide 
{
    public string refProvider;
    public string stock;
}