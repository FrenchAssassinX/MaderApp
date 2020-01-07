using UnityEngine;
using UnityEditor;

public class ComponentLine
{
    public string name;
    public int qte;
    public int price;

    public ComponentLine()
    {

    }

    public ComponentLine(string name, int qte, int price)
    {
        this.name = name;
        this.qte = qte;
        this.price = price;
    }
}