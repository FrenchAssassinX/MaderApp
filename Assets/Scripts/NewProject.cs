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
    private string URLGetCustomerByID = "v1/getcustomerbyid";       // Specific url for get customer

    /* Top Canvas */
    public Button buttonReturn;

    /* Left canvas */
    public InputField nameProject;              // Input Field for project name
    public InputField referenceProject;         // Readonly Input Field for project reference
    public Dropdown idCustomer;                 // Dropdown to get customer by id
    public Button buttonDisplayRightCanvas;     // Button to display right canvas
    public Button buttonCreateProject;          // Button to create new project
    public Button buttonCreateDestinationAdress;

    /* Right canvas */
    public Canvas canvasNewCustomer;            // Right canvas to display all fields to create new client
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

    public Canvas CanvasRightDestinationAdress; // Right canvas to display all fields to create new client destination adress
    public InputField roadNumDA;                // Input Field for client road number destination adress
    public InputField roadDA;                   // Input Field for client road destination adress
    public InputField zipcodeDA;                // Input Field for client zipecode destination adress
    public InputField cityDA;                   // Input Field for client city destination adress
    public InputField roadExtraDA;              // Input Field for client road extra destination adress

    /* Text messages displayed when create action are successful or failure */
    public GameObject createValideCustomer;     // Customer successfully created
    public GameObject createValideProject;      // Project successfully created
    public GameObject errorCreateCustomer;      // Customer creation failed
    public GameObject errorCreateProject;       // Project creation failed

    public string idClientForForm;
    public string IdCustomerGenerated;
    public List<string> dropdowncustomer = new List<string>();

    public string change;
    public string getId;
    public string getSurname;
    public string getName;
    public string newGetSurname;
    public string newGetName;
    public int timer = 120;

    void Start()
    {
        CONST = GameObject.Find("CONST");               // Get the CONST gameObject

        // By default don't display certain elements on start scene
        canvasNewCustomer.transform.gameObject.SetActive(false);
        CanvasRightDestinationAdress.transform.gameObject.SetActive(false);
        createValideCustomer.transform.gameObject.SetActive(false);
        errorCreateCustomer.transform.gameObject.SetActive(false);
        createValideProject.transform.gameObject.SetActive(false);
        errorCreateProject.transform.gameObject.SetActive(false);

        referenceProject.enabled = false;

        //Active canvasRight for add new customer
        buttonDisplayRightCanvas.onClick.AddListener(DisplayCreateNewCustomer);

        //Active canvasRight for add new destination adress
        buttonCreateDestinationAdress.onClick.AddListener(DisplayCreateDestinationAdress);

        //Send new project
        buttonCreateProject.onClick.AddListener(SendCreateProject);

        //Send new customer
        buttonCreateNewCustomer.onClick.AddListener(SendCreateCustomer);

        //Return home page
        buttonReturn.onClick.AddListener(ReturnHomePage);

        //Start get all customers
        StartCoroutine(GetAllCustomers());

        //Change value for dropdown
        idCustomer.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(idCustomer);
        });

    }

    void Update()
    {
        
        if (createValideCustomer.transform.gameObject.active)
        {
            if (timer > 0)
            {
                timer--;
            }
            else
            {
                createValideCustomer.transform.gameObject.SetActive(false);
                timer = 120;
            }
        }

        if (errorCreateCustomer.transform.gameObject.active)
        {
            if (timer > 0)
            {
                timer--;
            }
            else
            {
                errorCreateCustomer.transform.gameObject.SetActive(false);
                timer = 120;
            }
        }

        if (createValideProject.transform.gameObject.active)
        {
            if (timer > 0)
            {
                timer--;
            }
            else
            {
                createValideProject.transform.gameObject.SetActive(false);
                timer = 120;
            }
        }

        if (errorCreateProject.transform.gameObject.active)
        {
            if (timer > 0)
            {
                timer--;
            }
            else
            {
                errorCreateProject.transform.gameObject.SetActive(false);
                timer = 120;
            }
        }
    }

    //Active CreateNewCient
    public void DisplayCreateNewCustomer()
    {
        if (canvasNewCustomer.transform.gameObject.active)
        {
            canvasNewCustomer.transform.gameObject.SetActive(false);
        }
        else
        {
            canvasNewCustomer.transform.gameObject.SetActive(true);
        }

        //If canvas destination adress is open when you choise canvas destination adress so canvas new customer closed
        if (CanvasRightDestinationAdress.transform.gameObject.active)
        {
            CanvasRightDestinationAdress.transform.gameObject.SetActive(false);
        }
    }

    //Active Create desination adress
    public void DisplayCreateDestinationAdress()
    {
        if(CanvasRightDestinationAdress.transform.gameObject.active)
        {
            CanvasRightDestinationAdress.transform.gameObject.SetActive(false);
        }
        else
        {
            CanvasRightDestinationAdress.transform.gameObject.SetActive(true);
        }

        //If canvas new customer is open when you choise canvas destination adress so canvas new customer closed
        if (canvasNewCustomer.transform.gameObject.active)
        {
            canvasNewCustomer.transform.gameObject.SetActive(false);
        }

    }

    //add new project
    public void SendCreateProject()
    {
        if (nameProject.text.Length == 0 || referenceProject.text.Length == 0)
        {
            errorCreateProject.transform.gameObject.SetActive(true);
        }
        else
        {
            if (change == "Choisir un client")
            {
                errorCreateProject.transform.gameObject.SetActive(true);
            }
            else
            {
                StartCoroutine(PostFormNewProject());
                errorCreateProject.transform.gameObject.SetActive(false);
            }
        }
    }

    //add new customer
    public void SendCreateCustomer()
    {
        //Time of a text field is empty (except roadExtra) an error message will be displayed if the user clicks on the button
        if (name.text.Length == 0 || surname.text.Length == 0 || roadNum.text.Length == 0 || road.text.Length == 0 || zipcode.text.Length == 0 ||
                city.text.Length == 0 || email.text.Length == 0 || phone.text.Length == 0)
        {
            errorCreateCustomer.transform.gameObject.SetActive(true);
        }
        else
        {
            StartCoroutine(PostFormNewCustomer()); // Start create new customer
            errorCreateCustomer.transform.gameObject.SetActive(false);
        }

    }

    //return to home page
    public void ReturnHomePage()
    {
        //Send the previous scene (home page)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }


    public IEnumerator PostFormNewCustomer()
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
        form.AddField("_id", getId);
        form.AddField("__v", "0");


        /* New webrequest with: CONST url, local URLCreateCustomer and the form */
        using (UnityWebRequest request = UnityWebRequest.Post(CONST.GetComponent<CONST>().url + URLCreateCustomer, form))
        {
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

            request.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                // error
                //errorCreateCustomer.transform.gameObject.SetActive(true);
                Debug.Log("erreur");
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
                    //Actualize customer dropdown
                    StartCoroutine(GetAllCustomers());
                }
            }
        }
    }

    public IEnumerator PostFormNewProject()
    {
        /* Converting actual date to string to pass it on form for web request */
        string dateValueText = DateTime.Now.ToString("MM-dd-yyyy", CultureInfo.CreateSpecificCulture("fr-FR"));

        WWWForm form = new WWWForm(); //New form for web request
        form.AddField("userID", CONST.GetComponent<CONST>().userID);
        form.AddField("date", dateValueText);
        form.AddField("road", roadDA.text);
        form.AddField("roadNum", roadNumDA.text);
        form.AddField("roadExtra", roadExtraDA.text);
        form.AddField("zipcode", zipcodeDA.text);
        form.AddField("city", cityDA.text);
        form.AddField("customerID", idClientForForm);
        form.AddField("projectName", nameProject.text);
        form.AddField("reference", referenceProject.text);
        
        
        using (UnityWebRequest request = UnityWebRequest.Post(CONST.GetComponent<CONST>().url + URLCreateProject, form))
        {
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

            request.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

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
                    CONST.GetComponent<CONST>().projectName = project.name;

                    WWWForm formCustomer = new WWWForm();
                    formCustomer.AddField("customerID", project.customer);

                    using (UnityWebRequest requestCustomer = UnityWebRequest.Post(CONST.GetComponent<CONST>().url + URLGetCustomerByID, formCustomer))
                    {
                        requestCustomer.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                        requestCustomer.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

                        requestCustomer.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

                        yield return requestCustomer.SendWebRequest();
                        if (requestCustomer.isNetworkError || requestCustomer.isHttpError)
                        {
                            Debug.Log("ERROR: " + requestCustomer.error);
                        }
                        else
                        {
                            if (request.isDone)
                            {
                                string jsonResultCustomer = System.Text.Encoding.UTF8.GetString(requestCustomer.downloadHandler.data);
                                RequestACustomer requestACustomer = JsonUtility.FromJson<RequestACustomer>(jsonResultCustomer);
                                Customer customer = requestACustomer.customer;

                                CONST.GetComponent<CONST>().customerName = newGetName;
                            }
                        }
                    }

                    StartCoroutine(CreateNewEstimation(CONST.GetComponent<CONST>().selectedProjectID));

                    //message validate new customer
                    createValideProject.transform.gameObject.SetActive(true);
                }
            }
        }
    }

    private IEnumerator CreateNewEstimation(string pProjectID)
    {

        WWWForm form = new WWWForm();                               // New form for web request

        form.AddField("projectID", pProjectID);
        form.AddField("price", "0");
        form.AddField("discount", "0");
        form.AddField("module", "[]");
        form.AddField("floorNumber", "2");

        using (UnityWebRequest request = UnityWebRequest.Post(CONST.GetComponent<CONST>().url + URLCreateEstimation, form))
        {
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

            request.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

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
                    RequestAnEstimation entity = JsonUtility.FromJson<RequestAnEstimation>(jsonResult);

                    //Get estimation id in CONST
                    Estimation estimation = entity.estimation;
                    CONST.GetComponent<CONST>().selectedEstimationID = estimation._id;

                    DontDestroyOnLoad(CONST);                                               // Keep the CONST object between scenes
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);   // Load Create Module scene
                }
            }
        }
    }

    public IEnumerator GetAllCustomers()
    {
        UnityWebRequest request = UnityWebRequest.Get(CONST.GetComponent<CONST>().url + URLGetCustomers);
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);
        request.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("ERROR: " + request.error);
        }
        else
        {
            if (request.isDone)
            {
                string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);          // Get JSON file
                RequestGetAllCustomer entities = JsonUtility.FromJson<RequestGetAllCustomer>(jsonResult);       // Convert JSON file
                dropdowncustomer.Add("Choisir un client");

                foreach (var item in entities.customers)
                {
                    //recuperation values in customers
                    getId = item._id;
                    getSurname = item.surename;
                    getName = item.name;
                    
                    CONST.GetComponent<CONST>().customerName = newGetName;

                    //Poster all customers
                    dropdowncustomer.Add(getName + " " + getSurname);
                    
                    idClientForForm = getId;
                }
                idCustomer.options.Clear();
                idCustomer.AddOptions(dropdowncustomer);


            }
        }
    }

    //Function for create id of project
    public void DropdownValueChanged(Dropdown pChange)
    {
        //Get value in dropdown
        change = pChange.options[pChange.value].text;

        //Get timestanp
        var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

        //---------------------------Generate the ID Project-----------------------------------

        //Get 5 first letters of surname in dropdown
        newGetSurname = change.Substring(0, 5);
 
        //Get value after space in dropdown
        newGetName = change.Substring(change.IndexOf(" ")+" ".Length);

        //Create reference project
        IdCustomerGenerated = newGetSurname + newGetName.Substring(0, 1) + Timestamp;

        //Post reference project
        referenceProject.GetComponent<InputField>().text = IdCustomerGenerated;

    }
}