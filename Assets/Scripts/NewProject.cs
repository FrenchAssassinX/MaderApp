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
    private string URLCreateCustomer = "v1/createcustomer"; // Specific url for this scene

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
    public InputField zipcode;
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

        Debug.Log("URL :" + url + URLCreateCustomer);
    }

    void DisplayCreateNewCustomer()
    {
        //active CreateNewCient
        canvasNewClient.transform.gameObject.SetActive(true);

    }

    void SendCreateProjet()
    {
        Debug.Log("vous avez cliqué sur le boutton création du projet");
    }

    void SendCreateUser()
    {
        StartCoroutine(PostFormNewCustomer());
        Debug.Log("vous avez cliqué sur le boutton création d'un utilisateur");

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

        

        using (UnityWebRequest request = UnityWebRequest.Post(url + URLCreateCustomer, form))
        {
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                // error
                Debug.Log(request.error);
            }
            else
            {
                if (request.isDone)
                {
                    Debug.Log("conexion au serveur");
                    // The database return a JSON file of all user infos
                    string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                    // Create a root object thanks to the JSON file
                    resultNewProject entity = JsonUtility.FromJson<resultNewProject>(jsonResult);
                    //ok
                    
                        Debug.Log("send");

                }
                else
                {
                    Debug.Log("erreur conexion au serveur");

                }
            }
        }
            //using (UnityWebRequest request = UnityWebRequest.Post(url + URLCreateCustomer, form))
            //{
            //    yield return request.SendWebRequest();
            //    if (request.isNetworkError || request.isHttpError)
            //    {
            //        // error
            //        Debug.Log("erreur lors de la conexion au serveur");
            //    }
            //    else
            //    {
            //        if (request.isDone)
            //        {


            //        }
            //    }
            //}
        }

    }

