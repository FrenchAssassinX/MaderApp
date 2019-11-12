using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewProject : MonoBehaviour
{
    public GameObject CONST;
    private string url;
    private string URLCreateCustomer = "v1/createcustomer"; // Specific url for this scene
    private string URLCreateProject = "v1/createproject"; // Specific url for this scene

    //project (CanvasLeft)
    public InputField nameProject;
    public InputField referenceProject;
    public Button ButtonCreateCustomer;
    public Button ButtonCreateProject;

    //new client (CanvasRight)
    public InputField name;
    public InputField surname;
    public InputField roadNum;
    public InputField road;
    public InputField zipcode;
    public InputField city;
    public InputField roadExtra;
    public InputField email;
    public InputField phone;
    public Button ButtonCreateNewCustomer;
    public Canvas canvasNewClient;

    //text information
    public GameObject createValideCustomer;
    public GameObject createValideProject;
    public GameObject errorCreateCustomer;
    //Top
    public Button ButtonReturn;


    // Start is called before the first frame update
    void Start()
    {
        CONST = GameObject.Find("CONST"); //Get the CONST gameObject

        url = CONST.GetComponent<CONST>().url;

        //it's not visible for now
        canvasNewClient.transform.gameObject.SetActive(false);
        createValideCustomer.transform.gameObject.SetActive(false);
        errorCreateCustomer.transform.gameObject.SetActive(false);
        createValideProject.transform.gameObject.SetActive(false);
        //Active canvasRight for add new customer
        Button btnCC = ButtonCreateCustomer.GetComponent<Button>();
        btnCC.onClick.AddListener(DisplayCreateNewCustomer);
        Debug.Log(btnCC);

        //send new project
        Button btnCP = ButtonCreateProject.GetComponent<Button>();
        btnCP.onClick.AddListener(SendCreateProject);
        Debug.Log(btnCP);

        //send new customer
        Button btnNC = ButtonCreateNewCustomer.GetComponent<Button>();
        btnNC.onClick.AddListener(SendCreateCustomer);
        Debug.Log(btnNC);

        //return home page
        Button btnHP = ButtonReturn.GetComponent<Button>();
        btnHP.onClick.AddListener(ReturnHomePage);
        Debug.Log(btnHP);

        Debug.Log("URL :" + url + URLCreateCustomer);
    }

    //public static bool AllNull(InputField name, InputField surname, InputField road, InputField roadNum, InputField zipcode, InputField city, InputField phone, InputField email, InputField roadExtra, params string[] strings)
    //{
    //    return strings.All(s => s == "");
    //}

    void Update()
    {

        ////if form is empty ButtonCreateNewCustometr is disabled
        //if (AllNull(name, surname, road, roadNum, zipcode, city, phone, email, roadExtra))
        //{
        //    ButtonCreateNewCustomer.enabled = false;
        //}
        //else { 
        //    ButtonCreateNewCustomer.enabled = true;
        //}
    }

    void DisplayCreateNewCustomer()
    {
        //active CreateNewCient
        canvasNewClient.transform.gameObject.SetActive(true);

    }
    //add new project
    void SendCreateProject()
    {
        StartCoroutine(PostFormNewProject());
        Debug.Log("vous avez cliqué sur le boutton création du projet");
    }

    //add new customer
    void SendCreateCustomer()
    {

        StartCoroutine(PostFormNewCustomer());
        Debug.Log("vous avez cliqué sur le boutton création d'un utilisateur");

    }
    
    //return to home page
    void ReturnHomePage()
    {
        //Send the previous scene (home page)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    IEnumerator PostFormNewCustomer()
    {
        WWWForm form = new WWWForm();                   // New form for web request
        form.AddField("name", name.text);
        Debug.Log("name : " + name.text);
        form.AddField("surename", surname.text);
        Debug.Log("surename" + surname.text);
        form.AddField("road", road.text);
        Debug.Log("road" + road.text);
        form.AddField("roadNum", roadNum.text);
        Debug.Log("roadNum" + roadNum.text);
        form.AddField("zipcode", zipcode.text);
        Debug.Log("zipcode" + zipcode.text);
        form.AddField("city", city.text);
        Debug.Log("city" + city.text);
        form.AddField("roadExtra", roadExtra.text);
        Debug.Log("roadExtra" + roadExtra.text);
        form.AddField("phone", phone.text);
        Debug.Log("phone" + phone.text);
        form.AddField("email", email.text);
        Debug.Log("email" + email.text);
        form.AddField("_id", "12345");
        form.AddField("__v", "0");


        /* New webrequest with: CONST url, local URLCreateCustomer and the form */
        using (UnityWebRequest request = UnityWebRequest.Post(url + URLCreateCustomer, form))
        {
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);


            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                // error
                Debug.Log(request.error);
                errorCreateCustomer.transform.gameObject.SetActive(true);
            }
            else
            {
                if (request.isDone)
                {
                    Debug.Log("conexion au serveur");
                    // The database return a JSON file of all user infos
                    string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                    // Create a root object thanks to the JSON file
                    Customer entity = JsonUtility.FromJson<Customer>(jsonResult);
                    //message validate new customer
                    createValideCustomer.transform.gameObject.SetActive(true);
                    Debug.Log("send");

                }
                else
                {
                    Debug.Log("erreur conexion au serveur");

                }
            }
        }



    }

    IEnumerator PostFormNewProject()
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", "5dbee1c6e9b5241f704fdca9");
        form.AddField("date", "2019-01-21");
        form.AddField("road", "rue des trucs");
        form.AddField("roadNum", "25");
        form.AddField("roadExtra", "rien");
        form.AddField("zipcode", "71000");
        form.AddField("city", "sance");
        form.AddField("customerID", "5dbefd7e68ef8658edcde0c4");
        form.AddField("reference", "testTruc");
        form.AddField("projectName", "projectAlpha2");
        

        using (UnityWebRequest request = UnityWebRequest.Post(url + URLCreateProject, form))
        {
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                // error
                Debug.Log(request.error);
                //errorCreateCustomer.transform.gameObject.SetActive(true);
            }
            else
            {
                if (request.isDone)
                {
                    Debug.Log("conexion au serveur");
                    // The database return a JSON file of all user infos
                    string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                    // Create a root object thanks to the JSON file
                    CreateProject entity = JsonUtility.FromJson<CreateProject>(jsonResult);
                    //message validate new customer
                    createValideProject.transform.gameObject.SetActive(true);
                    Debug.Log("send");

                }
                else
                {
                    Debug.Log("erreur conexion au serveur");

                }
            }
        }


    }

}