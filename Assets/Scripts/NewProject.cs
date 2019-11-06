using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewProject : MonoBehaviour
{
    public GameObject CONST;
    private string url;
    private string URLCreateCustomer = "/v1/createcustomer";

    //project (CanvasLeft)
    public InputField nameProject;
    public InputField referenceProject;
    public Dropdown idClient;
    public Button ButtonCreateCustomer;
    public Button ButtonCreateProject;

    //new client (CanvasRight)
    public InputField name;
    public InputField surname;
    public InputField roadNum;
    public InputField road;
    public InputField zipeCode;
    public InputField city;
    public InputField roadExtra;
    public InputField email;
    public InputField phone;
    public Button ButtonCreateNewCustomer;
    public Canvas canvasNewClient;

    //Top
    public Button ButtonReturn;


    // Start is called before the first frame update
    void Start()
    {
        CONST = GameObject.Find("CONST"); //Get the CONST gameObject

        url = CONST.GetComponent<CONST>().url;

        //it's not visible for now
        canvasNewClient.transform.gameObject.SetActive(false);

        //Active canvasRight for add new customer
        Button btnCC = ButtonCreateCustomer.GetComponent<Button>();
        btnCC.onClick.AddListener(DisplayCreateNewCustomer);
        Debug.Log(btnCC);

        //send new project
        Button btnCP = ButtonCreateProject.GetComponent<Button>();
        btnCP.onClick.AddListener(SendCreateProjet);
        Debug.Log(btnCP);

        //send new user
        Button btnNC = ButtonCreateNewCustomer.GetComponent<Button>();
        btnNC.onClick.AddListener(SendCreateUser);
        Debug.Log(btnNC);

        //return home page
        Button btnHP = ButtonReturn.GetComponent<Button>();
        btnHP.onClick.AddListener(ReturnHomePage);
        Debug.Log(btnHP);
    }

    void Update()
    {
        

    }

    void DisplayCreateNewCustomer()
    {
        //active CreateNewCient
        canvasNewClient.transform.gameObject.SetActive(true);

    }

    void SendCreateProjet()
    {

    }

    void SendCreateUser()
    {
        StartCoroutine(PostFormNewCustomer());

    }

    void ReturnHomePage()
    {
        //Send the previous scene (home page)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); 
    }

    IEnumerator PostFormNewCustomer()
    {
        WWWForm form = new WWWForm();                   // New form for web request
        form.AddField("name", name.text);
        Debug.Log(name.text);
        form.AddField("surname", surname.text);
        Debug.Log(surname.text);
        //form.AddField("roadNum", roadNum.int);
        //Debug.Log(roadNum.text);
        form.AddField("road", road.text);
        Debug.Log(road.text);
        //form.AddField("zipeCode", zipeCode.int);
        //Debug.Log(zipeCode.text);
        form.AddField("city", city.text);
        Debug.Log(city.text);
        form.AddField("roadExtra", roadExtra.text);
        Debug.Log(roadExtra.text);
        form.AddField("email", email.text);
        Debug.Log(email.text);
        //form.AddField("phone", phone.int);
        //Debug.Log(phone.text);

        using (UnityWebRequest request = UnityWebRequest.Post(url + URLCreateCustomer, form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                // error
            }
            else
            {
                if (request.isDone)
                {
                    //ok
                    DontDestroyOnLoad(CONST.transform);

                }
            }
        }
    }

    //return To acceuil page :         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); //Send the previous scene (connectionScene)


}
