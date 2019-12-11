using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewProject : MonoBehaviour
{
    public GameObject CONST;                                        // CONST Object to keep infos in all app
    private string URLCreateCustomer = "v1/createcustomer";         // Specific url for create customer
    private string URLCreateProject = "v1/createproject";           // Specific url for create project
    private string URLGetCustomers = "v1/getallcustomer";           // Specific url for get all customers
    private string URLCreateEstimation = "v1/createestimation";     // Specific url for create estimation

    /* Top Canvas */
    public Button buttonReturn;

    /* Left canvas */
    public InputField nameProject;              // Input Field for project name
    public InputField referenceProject;         // Readonly Input Field for project reference
    public Dropdown idCustomer;                 // Dropdown to get customer by id
    public Button buttonDisplayRightCanvas;     // Button to display right canvas
    public Button buttonCreateProject;          // Button to create new project

    /* Right canvas */
    public Canvas canvasNewClient;              // Right canvas to display all fields to create new client
    public InputField name;                     // Input Field for client name
    public InputField surname;                  // Input Field for client surename
    public InputField roadNum;                  // Input Field for client road number
    public InputField road;                     // Input Field for client road name
    public InputField zipcode;                  // Input Field for client zipcode
    public InputField city;                     // Input Field for client city
    public InputField roadExtra;                // Input Field for client road extra information
    public InputField email;                    // Input Field for client email
    public InputField phone;                    // Input Field for client phone number
    public Button buttonCreateNewCustomer;      // Button to create new client

    /* Text messages displayed when create action are successful or failure */
    public GameObject createValideCustomer;     // Customer successfully created
    public GameObject createValideProject;      // Project successfully created
    public GameObject errorCreateCustomer;      // Customer creation failed

    public string idClientForForm;
    string IdCustomerGenerated;
    List<string> dropdowncustomer = new List<string>();

    string change;
    string getSurname;
    string getName;
    string newGetSurname;
    string newGetName;

    void Start()
    {
        CONST = GameObject.Find("CONST");               // Get the CONST gameObject

        // By default don't display certain elements on start scene
        canvasNewClient.transform.gameObject.SetActive(false);
        createValideCustomer.transform.gameObject.SetActive(false);
        errorCreateCustomer.transform.gameObject.SetActive(false);
        createValideProject.transform.gameObject.SetActive(false);
        referenceProject.enabled = false;

        //Active canvasRight for add new customer
        buttonDisplayRightCanvas.onClick.AddListener(DisplayCreateNewCustomer);

        //send new project
        buttonCreateProject.onClick.AddListener(SendCreateProject);

        //send new customer
        buttonCreateNewCustomer.onClick.AddListener(SendCreateCustomer);

        //return home page
        buttonReturn.onClick.AddListener(ReturnHomePage);

        StartCoroutine(GetAllCustomers());

        idCustomer.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(idCustomer);
        });
    }

    //active CreateNewCient
    void DisplayCreateNewCustomer()
    {
        canvasNewClient.transform.gameObject.SetActive(true);
    }

    //add new project
    void SendCreateProject()
    {
        StartCoroutine(PostFormNewProject());
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
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
        using (UnityWebRequest request = UnityWebRequest.Post(CONST.GetComponent<CONST>().url + URLCreateCustomer, form))
        {
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

            request.certificateHandler = CONST.GetComponent<CONST>().certificateHandler;    // Bypass certificate for https

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
        /* Converting actual date to string to pass it on form for web request */
        string dateValueText = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CreateSpecificCulture("fr-FR"));
        Debug.Log("Date convert to string : " + dateValueText);

        WWWForm form = new WWWForm();
        form.AddField("userID", CONST.GetComponent<CONST>().userID);
        form.AddField("date", dateValueText);
        form.AddField("road", road.text);
        form.AddField("roadNum", roadNum.text);
        form.AddField("roadExtra", roadExtra.text);
        form.AddField("zipcode", zipcode.text);
        form.AddField("city", city.text);
        form.AddField("customerID", idClientForForm);
        form.AddField("reference", referenceProject.text);
        form.AddField("projectName", nameProject.text);

        using (UnityWebRequest request = UnityWebRequest.Post(CONST.GetComponent<CONST>().url + URLCreateProject, form))
        {
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

            request.certificateHandler = CONST.GetComponent<CONST>().certificateHandler;    // Bypass certificate for https

            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log("ERROR: " + request.error);
            }
            else
            {
                if (request.isDone)
                {
                    // The database return a JSON file of all user infos
                    string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                    // Create a root object thanks to the JSON file
                    RequestCreateProject entity = JsonUtility.FromJson<RequestCreateProject>(jsonResult);         // Convert JSON file

                    Project project = entity.project;
                    CONST.GetComponent<CONST>().selectedProjectID = project._id;

                    StartCoroutine(CreateNewEstimation(CONST.GetComponent<CONST>().selectedProjectID));

                    //message validate new customer
                    createValideProject.transform.gameObject.SetActive(true);
                }
            }
        }
    }

    private IEnumerator CreateNewEstimation(string pProjectID)
    {
        Debug.Log("CreateNewEstimation Start");

        WWWForm form = new WWWForm();                               // New form for web request

        form.AddField("projectID", pProjectID);
        form.AddField("price", "0");
        form.AddField("discount", "0");
        form.AddField("module", "{}");

        using (UnityWebRequest request = UnityWebRequest.Post(CONST.GetComponent<CONST>().url + URLCreateEstimation, form))
        {
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

            request.certificateHandler = CONST.GetComponent<CONST>().certificateHandler;    // Bypass certificate for https

            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log("ERROR: " + request.error);
            }
            else
            {
                if (request.isDone)
                {
                    // The database return a JSON file of all user infos
                    string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                    Debug.Log(jsonResult);
                    // Create a root object thanks to the JSON file
                    RequestAnEstimation entity = JsonUtility.FromJson<RequestAnEstimation>(jsonResult);

                    Estimation estimation = entity.estimation;
                    CONST.GetComponent<CONST>().selectedEstimationID = estimation._id;

                    DontDestroyOnLoad(CONST);                                               // Keep the CONST object between scenes
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);   // Load Create Module scene
                }
            }
        }
    }

    IEnumerator GetAllCustomers()
    {
        UnityWebRequest request = UnityWebRequest.Get(CONST.GetComponent<CONST>().url + URLGetCustomers);
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        request.certificateHandler = CONST.GetComponent<CONST>().certificateHandler;    // Bypass certificate for https

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {

        }
        else
        {
            if (request.isDone)
            {
                string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);          // Get JSON file
                RequestGetAllCustomer entities = JsonUtility.FromJson<RequestGetAllCustomer>(jsonResult);       // Convert JSON file

                foreach (var item in entities.customers)
                {
                    //recuperation values in customers
                    string getId = item._id;
                    getSurname = item.surename;
                    getName = item.name;

                    //Poster all customers
                    dropdowncustomer.Add(getName + getSurname);

                    //Select the first letters for name
                    idClientForForm = getId;
                }
                idCustomer.options.Clear();
                idCustomer.AddOptions(dropdowncustomer);
            }
        }
    }

    void DropdownValueChanged(Dropdown pChange)
    {
        Debug.Log(pChange.options[pChange.value].text);
        change = pChange.options[pChange.value].text;
        Debug.Log("change : " + change);

        //Select the timestanp
        var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

        //---------------------------Generate the ID Project-----------------------------------
        if (getSurname != "" && getSurname != null)
        {
            newGetSurname = getSurname.Substring(0, 3);
        }

        if (getName != "" && getName != null)
        {
            newGetName = getName.Substring(0, 1);
        }

        IdCustomerGenerated = change + Timestamp;

        //post ref project
        referenceProject.GetComponent<InputField>().text = IdCustomerGenerated;
    }
}