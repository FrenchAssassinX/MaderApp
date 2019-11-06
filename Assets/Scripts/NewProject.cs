using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewProject : MonoBehaviour
{
    //project (CanvasLeft)
    public Input nameProject;
    public Input referenceProject;
    public Dropdown idClient;
    public Button ButtonCreateCustomer;
    public Button ButtonCreateProject;
    //new client (CanvasRight)
    public Input name;
    public Input surname;
    public Input roadNum;
    public Input road;
    public Input zipeCode;
    public Input roadExtra;
    public Input email;
    public Input phone;
    public Button ButtonCreateNewCustomer;
    public Canvas canvasNewClient;


    // Start is called before the first frame update
    void Start()
    {
        //it's not visible for now
        canvasNewClient.transform.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        //Active canvasRight for add new customer
        Button btn = ButtonCreateCustomer.GetComponent<Button>();
        btn.onClick.AddListener(DisplayCreateNewClient);
    }

    void DisplayCreateNewClient()
    {
        //active CreateNewCient
        canvasNewClient.transform.gameObject.SetActive(true);

    }
}
