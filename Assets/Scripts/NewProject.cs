using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NewProject : MonoBehaviour
{
    public GameObject CONST;
    private string url;
    private string URLCreateCustomer = "/v1/createcustomer";

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
    public Input city;
    public Input roadExtra;
    public Input email;
    public Input phone;
    public Button ButtonCreateNewCustomer;
    public Canvas canvasNewClient;


    // Start is called before the first frame update
    void Start()
    {

        url = CONST.GetComponent<CONST>().url;

        //it's not visible for now
        canvasNewClient.transform.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        //Active canvasRight for add new customer
        Button btnCC = ButtonCreateCustomer.GetComponent<Button>();
        btnCC.onClick.AddListener(DisplayCreateNewCustomer);

        //send new project
        Button btnCP = ButtonCreateProject.GetComponent<Button>();
        btnCP.onClick.AddListener(SendCreateProjet);

        //send new 
        Button btnNC = ButtonCreateNewCustomer.GetComponent<Button>();
        btnNC.onClick.AddListener(PostFormNewCustomer());
    }

    void DisplayCreateNewCustomer()
    {
        //active CreateNewCient
        canvasNewClient.transform.gameObject.SetActive(true);

    }

    void SendCreateProjet()
    {

    }

    IEnumerator PostFormNewCustomer()
    {
        WWWForm form = new WWWForm();                   // New form for web request
        form.AddField("name", name.text);
        form.AddField("surname", surname.text);
        form.AddField("roadNum", roadNum.int);
        form.AddField("road", road.text);
        form.AddField("zipeCode", zipeCode.int);
        form.AddField("city", city.text);
        form.AddField("roadExtra", roadExtra.text);
        form.AddField("email", email.text);
        form.AddField("phone", phone.int);
        

        using (UnityWebRequest request = UnityWebRequest.Post(url + URLCreateCustomer, form))
        {

        }
    }

}
