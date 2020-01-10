using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using System;

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
    internal object transform;

    internal object GetComponent<T>()
    {
        throw new NotImplementedException();
    }

    public static implicit operator Text(string v)
    {
        throw new NotImplementedException();
    }
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
public class RequestCreateProject
{
    public string message;
    public Project project;
}

[System.Serializable]
public class CreateProject
{
    public string message;
    public Project project;
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
    public string floorNumber;
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
    public string cut;
    public string range;
    public string createModule;
    public List<Component> components;
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
    public List<string> framequality;
    public List<string> insulating;
    public List<string> covering;
    public List<string> windowsframequality;
    public List<string> finishingint;
    public List<string> finishingext;
    public string libelle;
    public string __v;
}

[System.Serializable]
public class RequestGetAllModule
{
    public string message;
    public List<Module> modules;
}

[System.Serializable]
public class Module
{
    public List<Component> components;
    public List<ComponentId> componentsID;
    public string _id;
    public string name;
    public string cost;
    public string angle;
    public string type;
    public string cut;
    public string range;
    public string rangeName;
    public List<RangeAttribute> rangeAttributes;
    public string x;
    public string y;
    public string height;
    public string width;
    public string floorHouse;
    public string __v;
}

[System.Serializable]
public class GetRangeById
{
    public List<string> framequality;
    public List<string> insulating;
    public List<string> covering;
    public List<string> windowsframequality;
    public List<string> finishingint;
    public List<string> finishingext;
    public string _id;
    public string libelle;
    public string __V;
}

[System.Serializable]
public class InvoiceProject
{
    public string state;
    public string _id;
}

[System.Serializable]
public class StateUpdatePayment
{
    public string step;
    public string pourcentage;
    public string _id;
}

[System.Serializable] 
public class RequestAllComponents
{
    public string message;
    public List<Components> components;
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
public class Components
{
    public string _id;
    public string code;
    public string unit;
    public string description;
    public string name;
    public string type;
    public string cost;
    public Provide provide;
}

[System.Serializable]
public class Provide
{
    public string id;
    public string refProvider;
    public string stock;
}

[System.Serializable]
public class RangeAttribute
{
    public string frameQuality;
    public string windowsframequality;
    public string insulating;
    public string covering;
    public string finishingext;
    public string finishingint;
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
    public string cost;
    public Provide provide;
    public string __v;
}

[System.Serializable]
public class RequestAUser
{
    public string message;
    public User user;
}

[System.Serializable]
public class Cuts
{
    public string _id;
    public string name;
}

[System.Serializable]
public class RequestGetAllCuts
{
    public string message;
    public List<Cuts> cuts;
}

[System.Serializable] 
public class RequestCreatePayment
{
    public string message;
    public CreatePayement payement;
}

[System.Serializable]
public class CreatePayement
{
    public string _id;
    public string step;
    public string percentage;
    public string date;
    public string __v;
}

[System.Serializable]
public class GetPaymentById
{
    public string message;
    public GetPayment payement;
}

[System.Serializable]
public class GetPayment
{
    public string _id;
    public string step;
    public string percentage;
    public string date;
}

[System.Serializable]
public class RequestACustomer
{
    public string message;
    public Customer customer;
}

[System.Serializable]
public class ModuleGetModuleByEstimation
{
    public string _id;
    public string name;
    public string cost;
    public string angle;
    public string cut;
    public string type;
    public string range;
    public string rangeName;
    public string estimationID;
    public string __v;
    public List<ComponentId> components;
}

[System.Serializable]
public class RequestGetModuleByEstimation
{
    public string message;
    public List<ModuleGetModuleByEstimation> module;
}

[System.Serializable] 
public class ListComponent
{
    public List<ComponentId> components;
}