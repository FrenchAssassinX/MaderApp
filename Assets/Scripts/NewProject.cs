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
    private string URLCreateCustomer = "v1/createcustomer"; // Specific url for create customer
    private string URLCreateProject = "v1/createproject"; // Specific url for create project
    private string URLGetCustomers = "v1/getallcustomer"; // Specific url for get all customers

    //project (CanvasLeft)
    public InputField nameProject;
    public InputField referenceProject;
    public Dropdown idCustomer;
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

    public string idClientForForm;
    string IdCustomerGenerated;

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
        referenceProject.enabled = false;
        //Active canvasRight for add new customer
        Button btnCC = ButtonCreateCustomer.GetComponent<Button>();
        btnCC.onClick.AddListener(DisplayCreateNewCustomer);

        //send new project
        Button btnCP = ButtonCreateProject.GetComponent<Button>();
        btnCP.onClick.AddListener(SendCreateProject);

        //send new customer
        Button btnNC = ButtonCreateNewCustomer.GetComponent<Button>();
        btnNC.onClick.AddListener(SendCreateCustomer);

        //return home page
        Button btnHP = ButtonReturn.GetComponent<Button>();
        btnHP.onClick.AddListener(ReturnHomePage);

        StartCoroutine(GetAllCustomers());


    }

    //void Update()
    //{
    //    if (!idCustomer.GetComponent<Dropdown>().options.Equals("Client"))
    //    {
    //        referenceProject.GetComponent<InputField>().text = IdCustomerGenerated;
    //    }
    //}         



//active CreateNewCient
void DisplayCreateNewCustomer()
    {
        canvasNewClient.transform.gameObject.SetActive(true);

    }

    //add new project
    void SendCreateProject()
    {
        StartCoroutine(PostFormNewProject());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +3);

    }

    //add new customer
    void SendCreateCustomer()
    {

        StartCoroutine(PostFormNewCustomer());

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
        form.AddField("surename", surname.text);
        form.AddField("road", road.text);
        form.AddField("roadNum", roadNum.text);
        form.AddField("zipcode", zipcode.text);
        form.AddField("city", city.text);
        form.AddField("roadExtra", roadExtra.text);
        form.AddField("phone", phone.text);
        form.AddField("email", email.text);
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
                errorCreateCustomer.transform.gameObject.SetActive(true);
            }
            else
            {
                if (request.isDone)
                {
                    // The database return a JSON file of all user infos
                    string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                    // Create a root object thanks to the JSON file
                    CreateCustomer entity = JsonUtility.FromJson<CreateCustomer>(jsonResult);
                    //message validate new customer
                    createValideCustomer.transform.gameObject.SetActive(true);

                }
                
            }
        }



    }

    IEnumerator PostFormNewProject()
    {
        
        WWWForm form = new WWWForm();
        form.AddField("userID", "5dbee1c6e9b5241f704fdca9");
        form.AddField("date", "2019-11-22");
        form.AddField("road", road.text);
        form.AddField("roadNum", roadNum.text);
        form.AddField("roadExtra", roadExtra.text);
        form.AddField("zipcode", zipcode.text);
        form.AddField("city", city.text);
        form.AddField("customerID", idClientForForm); //not .text in the end because is a string
        form.AddField("reference", referenceProject.text);
        form.AddField("projectName", nameProject.text);


        using (UnityWebRequest request = UnityWebRequest.Post(url + URLCreateProject, form))
        {
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                // error
                Debug.Log("network erreor :" + request.isNetworkError);
                Debug.Log("http erreor :" + request.isHttpError);
                Debug.Log(form);

            }
            else
            {
                if (request.isDone)
                {
                    Debug.Log("envoi du projet");
                    // The database return a JSON file of all user infos
                    string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                    // Create a root object thanks to the JSON file
                    CreateProject entity = JsonUtility.FromJson<CreateProject>(jsonResult);
                    //message validate new customer
                    createValideProject.transform.gameObject.SetActive(true);
                    
                }
                
            }
        }


    }

    public IEnumerator GetAllCustomers()
    {
        UnityWebRequest request = UnityWebRequest.Get(CONST.GetComponent<CONST>().url + URLGetCustomers);
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                      
        request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("*** ERROR: " + request.error + " ***");
        }
        else
        {
            if (request.isDone)
            {
                Debug.Log("ok");
                string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);          // Get JSON file

                Debug.Log(jsonResult);

                RequestGetAllCustomer entities = JsonUtility.FromJson<RequestGetAllCustomer>(jsonResult);         // Convert JSON file

                Debug.Log("entities : " + entities.customers);

                foreach (var item in entities.customers)
                {
                    Debug.Log("item : " + item.city + item.surename + item._id);
                    //string IdCustomerGenerated = "CHOISIR UN CLIENT";
                    string getId = item._id;
                    string getSurname = item.surename;
                    string getName = item.name;
                    
                    Debug.Log("surname : " + getSurname);
                    Debug.Log("name : " + getName);
                    Debug.Log("id : " + getId);

                    //Poster all customers
                    List<string> dropdowncustomer = new List<string>() { getName + " " + getSurname };
                    idCustomer.AddOptions(dropdowncustomer);
                    //Debug.Log("Dropdown customer :" + idCustomer);

                    //Select the 5 firsts letters for surname
                    string test = "dfdfgdfg";

                    string newtest = test.Substring(0, 4);
                    Debug.Log(newtest);

                    string newGetSurname = getSurname.Substring(0, 3);
                    Debug.Log("modif get surname : " + newGetSurname);

                    //Select the first letters for name
                    string newGetName = getName.Substring(0, 1);
                    Debug.Log("modif get name : " + newGetName);

                    //Select the timestanp
                    var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                    //Debug.Log(Timestamp);

                    //---------------------------Generate the ID Project-----------------------------------

                    IdCustomerGenerated = newGetSurname + newGetName + Timestamp;
                    idClientForForm = getId;
                    Debug.Log("id generated" + IdCustomerGenerated);
                    //post ref project
                    referenceProject.GetComponent<InputField>().text = IdCustomerGenerated;
                              
                    //-------------------------------------------------------------------------------------
                }

            }
        }
    }

    //Add reference for a new project
    public void GenerateReferenceProject()
    {
        

        //Poster the ID of customer

    }

}