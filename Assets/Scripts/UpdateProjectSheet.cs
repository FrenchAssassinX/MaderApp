using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using sharpPDF;
using sharpPDF.Enumerators;

public class UpdateProjectSheet : MonoBehaviour
{
    public GameObject CONST;                                    // CONST object contains server route, token and user infos

    //backPage button
    public GameObject backPageButton;

    //gameObject that will containing the title of the window
    public GameObject frameTitle;

    //gameObjects that will containing the project datas
    public GameObject projectIdGO;
    public GameObject projectNameGO;
    public GameObject projectDateGO;
    public GameObject projectSailorIdGO;

    //gameObjects that will containing the client datas
    public GameObject clientIdGO;
    public GameObject clientNameGO;
    public GameObject clientSurnameGO;
    public GameObject clientRoadGO;
    public GameObject clientRoadNumGO;
    public GameObject clientZipCodeGO;
    public GameObject clientCityGO;
    public GameObject clientRoadExtraGO;
    public GameObject clientPhoneGO;
    public GameObject clientEmailGO;

    //Customer informations to show into the pdf
    private string customerSurename;
    private string customerFirstname;
    private string customerRoad;
    private string customerRoadNum;
    private string customerRoadExtra;
    private string customerCity;
    private string customerZipcode;

    //project informations to show into the pdf
    private string projectName;
    private string projectRef;
    private string projectRoad;
    private string projectRoadNum;
    private string projectRoadExtra;
    private string projectCity;
    private string projectZipcode;
    private string projectReferent;

    //estimation informations so show into the pdf
    private string estimationDate;
    private string estimationRef;
    private string estimationPriceWtotTaxes;
    private string estimationDiscount;
    private string estimationPriceWtTaxes;
    private string estimationPriceToPay;

    //updateButtons
    public GameObject updateClientButton;
    public GameObject updateProjectButton;

    //updateClientPanelButtons
    public GameObject updateClientConfirmButton;
    public GameObject updateClientCancelButton;

    //updateProjectPanelButtons
    public GameObject updateProjectConfirmButton;
    public GameObject updateProjectCancelButton;

    //updateClient inputs
    public GameObject clientNameInput;
    public GameObject clientSurnameInput;
    public GameObject clientRoadInput;
    public GameObject clientRoadNumInput;
    public GameObject clientZipCodeInput;
    public GameObject clientCityInput;
    public GameObject clientRoadExtraInput;
    public GameObject clientPhoneInput;
    public GameObject clientEmailInput;

    //updateProject inputs
    public GameObject projectNameInput;
    public GameObject projectSailorIdInput;

    public GameObject listItemPrefab;                           // Prefab item to display all elements in project list
    public GameObject estimationList;                   //panel wich will contain all the listItemPrefabs 

    //pop-ups panels
    public GameObject updateClientPanel;
    public GameObject updateProjectPanel;
    public GameObject confirmDeletePanel;

    public Button deletePanelConfirmButton;                     // Confirm button on Delete panel
    public Button deletePanelCancelButton;                      // Cancel button on Delete panel

    //parameters that will containing the project datas
    private string projectId;
    private string projectSailorId;
    private Project project;
    private Customer customer;
    private User2 user;

    public GameObject notifText;
    public Button okNotifyButton;
    public GameObject notifyCanvas;

    public Button addButton;

    EstimationSelected estimationSelected = new EstimationSelected();
    private List<ComponentToShow> components;

    // Start is called before the first frame update
    void Start()
    {
        //hide the pop-ups panels
        updateClientPanel.SetActive(false);
        updateProjectPanel.SetActive(false);
        confirmDeletePanel.SetActive(false);
        notifyCanvas.SetActive(false);

        CONST = GameObject.Find("CONST");                       // Get const object

        addButton.onClick.AddListener(AddNewEstimation);
        okNotifyButton.onClick.AddListener(CloseNotifyWindow);
        deletePanelCancelButton.onClick.AddListener(cancelDeleteEstimation);
        deletePanelConfirmButton.onClick.AddListener(confirmDeleteEstimation);

        projectId = CONST.GetComponent<CONST>().selectedProjectID; //Instanciate the projet ID from the CONST object
        projectSailorId = CONST.GetComponent<CONST>().userID; //Instanciate the sailor ID from the CONST 

        estimationList = GameObject.Find("gridWithOurElement");                 // Get grid of the list 

        StartCoroutine(GetAllEstimations());                       // Start script to find estimations on databse
    }

    private IEnumerator GetAllEstimations()
    {
        //http road to get the project datas
        var urlToGetProject = CONST.GetComponent<CONST>().url + "v1/getprojectbyid";

        WWWForm form = new WWWForm();                       // New form for web request
        form.AddField("projectID", projectId);    // Add to the form the value of the ID of the project to get

        UnityWebRequest request = UnityWebRequest.Post(urlToGetProject, form);     // Create new form
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                      // Complete form with authentication datas
        request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        request.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("*** ERROR: " + request.error + " ***");
        }
        else
        {
            if (request.isDone)
            {
                string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);          // Get JSON file

                RequestAProject entityProject = JsonUtility.FromJson<RequestAProject>(jsonResult);         // Convert JSON file
                project = entityProject.result; //Instanciate the project object
                customer = entityProject.customer; //Instanciate the customer object
                user = project.user;

                List<EstimationId> estimationIdList = project.estimation;

                // Formating date to French timeset
                string dateValueText = project.date;
                dateValueText = dateValueText.Remove(10, 14);
                DateTime dateTimeText = Convert.ToDateTime(dateValueText);
                dateValueText = dateTimeText.ToString("dd-MM-yyyy", CultureInfo.CreateSpecificCulture("fr-FR"));

                //set the gameObjects content from the client and the project parameters 
                frameTitle.GetComponent<UnityEngine.UI.Text>().text = "Projet " + project.reference;
                projectIdGO.GetComponent<UnityEngine.UI.Text>().text = project.reference;
                projectNameGO.GetComponent<UnityEngine.UI.Text>().text = project.name;
                projectDateGO.GetComponent<UnityEngine.UI.Text>().text = dateValueText;
                projectSailorIdGO.GetComponent<UnityEngine.UI.Text>().text = user.matricule;
                //clientIdGO.GetComponent<UnityEngine.UI.Text>().text = customer._id;
                clientNameGO.GetComponent<UnityEngine.UI.Text>().text = customer.name;
                clientSurnameGO.GetComponent<UnityEngine.UI.Text>().text = customer.surename;
                clientRoadGO.GetComponent<UnityEngine.UI.Text>().text = customer.road;
                clientRoadNumGO.GetComponent<UnityEngine.UI.Text>().text = customer.roadNum;
                clientZipCodeGO.GetComponent<UnityEngine.UI.Text>().text = customer.zipcode;
                clientCityGO.GetComponent<UnityEngine.UI.Text>().text = customer.city;
                clientRoadExtraGO.GetComponent<UnityEngine.UI.Text>().text = customer.roadExtra;
                clientPhoneGO.GetComponent<UnityEngine.UI.Text>().text = customer.phone;
                clientEmailGO.GetComponent<UnityEngine.UI.Text>().text = customer.email;

                // foreach to retrieve every estimations
                foreach (var item in estimationIdList)
                {
                    StartCoroutine(WorkOnEstimation(item));
                }
            }
        }
    }

    public IEnumerator WorkOnEstimation(EstimationId item)
    {
        var urlToGetEstimation = CONST.GetComponent<CONST>().url + "v1/getestimationbyid";

        WWWForm estimationForm = new WWWForm();                       // New form for web request
        estimationForm.AddField("estimationID", item.id);    // Add to the form the value of the ID of the project to get

        UnityWebRequest requestForEstimation = UnityWebRequest.Post(urlToGetEstimation, estimationForm);     // Create new WebRequest
        requestForEstimation.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                      // Complete form with authentication datas
        requestForEstimation.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        requestForEstimation.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

        yield return requestForEstimation.SendWebRequest();

        if (requestForEstimation.isNetworkError || requestForEstimation.isHttpError)
        {
            Debug.Log(requestForEstimation.error);
        }
        else
        {
            string jsonResultFromEstimation = System.Text.Encoding.UTF8.GetString(requestForEstimation.downloadHandler.data);          // Get JSON file

            RequestAnEstimation estimationEntity = JsonUtility.FromJson<RequestAnEstimation>(jsonResultFromEstimation);         // Convert JSON file

            Estimation estimation = estimationEntity.estimation;

            // Create prefab
            GameObject listItem = Instantiate(listItemPrefab, Vector3.zero, Quaternion.identity);

            // Set estimationListPanel as parent of prefab in project hierarchy
            listItem.transform.SetParent(estimationList.transform);


            listItem.GetComponent<RectTransform>().localScale = estimationList.GetComponent<RectTransform>().localScale;
            listItem.GetComponent<RectTransform>().sizeDelta = new Vector2(estimationList.GetComponent<RectTransform>().sizeDelta.x, listItem.GetComponent<RectTransform>().sizeDelta.y);

            // Find children in listItem to use them
            GameObject idValue = GameObject.Find("idText");
            GameObject priceValue = GameObject.Find("priceText");
            GameObject stateValue = GameObject.Find("stateText");
            GameObject dateValue = GameObject.Find("dateText");

            // Customize props name of the prefab to find it when it will be create
            idValue.name = idValue.name + listItem.GetComponent<ItemListEstimation>().name;
            priceValue.name = priceValue.name + listItem.GetComponent<ItemListEstimation>().name;
            stateValue.name = stateValue.name + listItem.GetComponent<ItemListEstimation>().name;
            dateValue.name = dateValue.name + listItem.GetComponent<ItemListEstimation>().name;

            // Formating date to French timeset
            string dateValueText = estimation.date;
            dateValueText = dateValueText.Remove(10, 14);
            DateTime dateTimeText = Convert.ToDateTime(dateValueText);
            dateValueText = dateTimeText.ToString("dd-MM-yyyy", CultureInfo.CreateSpecificCulture("fr-FR"));

            string matriculeToShow = estimation._id.Remove(9);
            string stateValueSt=""; 
            if(estimation.state == "1")
            {
                stateValueSt = "Signé";
            }
            else if (estimation.state == "0")
            {
                stateValueSt = "Brouillon";
            }
            string price = estimation.price;
            
            if (price.Contains("."))
            {
                price = price.Replace(".", ",");
            }
           
            if (!price.Contains(","))
            {
                price += ",00";
            }
            
            // Change text value of the list item
            idValue.GetComponent<UnityEngine.UI.Text>().text = matriculeToShow;
            priceValue.GetComponent<UnityEngine.UI.Text>().text = price+"€";
            stateValue.GetComponent<UnityEngine.UI.Text>().text = stateValueSt;
            dateValue.GetComponent<UnityEngine.UI.Text>().text = dateValueText;

            idValue.GetComponent<RectTransform>().localScale = listItem.GetComponent<RectTransform>().localScale;
            priceValue.GetComponent<RectTransform>().localScale = listItem.GetComponent<RectTransform>().localScale;
            stateValue.GetComponent<RectTransform>().localScale = listItem.GetComponent<RectTransform>().localScale;
            dateValue.GetComponent<RectTransform>().localScale = listItem.GetComponent<RectTransform>().localScale;

            idValue.GetComponent<RectTransform>().sizeDelta = listItem.GetComponent<RectTransform>().sizeDelta;
            priceValue.GetComponent<RectTransform>().sizeDelta = listItem.GetComponent<RectTransform>().sizeDelta;
            stateValue.GetComponent<RectTransform>().sizeDelta = listItem.GetComponent<RectTransform>().sizeDelta;
            dateValue.GetComponent<RectTransform>().sizeDelta = listItem.GetComponent<RectTransform>().sizeDelta;

            // ID to keep for view estimation sheet or deleting estimation
            listItem.GetComponent<ItemListEstimation>().idValue = estimation._id;
            listItem.GetComponent<ItemListEstimation>().dateValue = dateValueText;
            listItem.GetComponent<ItemListEstimation>().priceValue = estimation.price;
            listItem.GetComponent<ItemListEstimation>().stateValue = estimation.state;
            listItem.GetComponent<ItemListEstimation>().discountValue = estimation.discount;
        }
    }

    /* Function to delete project in the database */
    private IEnumerator DeleteEstimation(EstimationSelected pEstimationSelected)
    {
        string url = CONST.GetComponent<CONST>().url + "v1/deleteestimation"; //delete estimation url

        WWWForm form = new WWWForm();                       // New form for web request
        form.AddField("estimationID", pEstimationSelected.id);    // Add to the form the value of the ID of the project to delete
        form.AddField("projectID", projectId);

        UnityWebRequest request = UnityWebRequest.Post(url, form);       // New request, passing url and form
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                                  // Set request authentications
        request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        request.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("*** ERROR: " + request.error + " ***");
        }
        else
        {
            if (request.isDone)
            {
                /* Refresh list of projects */
                foreach (Transform childList in estimationList.transform)
                {
                    GameObject.Destroy(childList.gameObject);
                }
                StartCoroutine(GetAllEstimations());           // Recall all projects from databse
            }
        }
    }

    /* Function to show an estimation */
    public void ShowEstimation(GameObject pItemSelected)
    {
        CONST.GetComponent<CONST>().selectedEstimationID = pItemSelected.GetComponent<ItemListEstimation>().idValue;   // Assign the values for the next scene
        CONST.GetComponent<CONST>().customerName = clientNameGO.GetComponent<UnityEngine.UI.Text>().text + " " + clientSurnameGO.GetComponent<UnityEngine.UI.Text>().text;
        CONST.GetComponent<CONST>().projectName = projectNameGO.GetComponent<UnityEngine.UI.Text>().text;
        CONST.GetComponent<CONST>().estimationPrice = pItemSelected.GetComponent<ItemListEstimation>().priceValue;
        CONST.GetComponent<CONST>().estimationDiscount = pItemSelected.GetComponent<ItemListEstimation>().discountValue;


        DontDestroyOnLoad(CONST);                                                   // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3); //load the next scene
    }

    /* Function to show the create estimation window */
    public void ShowCreateEstimation(GameObject pItemSelected)
    {
        CONST.GetComponent<CONST>().selectedEstimationID = pItemSelected.GetComponent<ItemListEstimation>().idValue;   // Assign the values for the next scene

        DontDestroyOnLoad(CONST);                                                   // Keep the CONST object between scenes
        SceneManager.LoadScene(6); //load the next scene
    }

    /* Function to show the payment terms window */
    public void ShowPaymentTerms(GameObject pItemSelected)
    {
        CONST.GetComponent<CONST>().selectedEstimationID = pItemSelected.GetComponent<ItemListEstimation>().idValue;   // Assign the values for the next scene

        DontDestroyOnLoad(CONST);                                                   // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 6); //load the next scene
    }

    /* Function to show the technical folder */
    public void ShowTechnicalFolder(GameObject pItemSelected)
    {
        CONST.GetComponent<CONST>().selectedEstimationID = pItemSelected.GetComponent<ItemListEstimation>().idValue;   // Assign the values for the next scene

        DontDestroyOnLoad(CONST);                                                   // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 5); //load the next scene
    }

    public void AddNewEstimation()
    {
        StartCoroutine(CreateNewEstimation(projectId));
    }

    /*Function to create a new Estimation */
    private IEnumerator CreateNewEstimation(string pProjectID)
    {

        WWWForm form = new WWWForm();                               // New form for web request

        form.AddField("projectID", pProjectID);
        form.AddField("price", "0");
        form.AddField("discount", "0");
        form.AddField("module", "[]");
        form.AddField("floorNumber", "2");

        UnityWebRequest request = UnityWebRequest.Post(CONST.GetComponent<CONST>().url + "v1/createestimation", form);

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
                SceneManager.LoadScene(5);   // Load Create Module scene
            }
        }

    }

    public void GoToHomePage()
    {
        DontDestroyOnLoad(CONST);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
    }

    //Get back  button function
    public void BackPage()
    {
        DontDestroyOnLoad(CONST);                                                   // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); //Send the previous scene (ProjectList)
    }

    /* Function to display Delete project panel */
    public void openDeleteEstimation(GameObject pItemSelected)
    {
        /* Make panel visible */
        confirmDeletePanel.SetActive(true);

        // Keep the ID of the selected project
        estimationSelected.id = pItemSelected.GetComponent<ItemListEstimation>().idValue;
    }

    //Function called when the user clicks in the update client button
    public void openUpdateClientPanel()
    {
        updateClientPanel.SetActive(true);//set active the update client panel
    }

    //Function called when the user clicks in the update project button
    public void openUpdateProjectPanel()
    {
        updateProjectPanel.SetActive(true);//set active the update project panel
    }

    //function called when the user clicks in the cancel button of the update project pop-up
    public void cancelUpdateProjectPanel()
    {
        updateProjectPanel.SetActive(false); //set non active the update project panel
    }

    //function called when the user clicks in the confirm button of the update project pop-up
    public void confirmUpdateProjectPanel()
    {
        StartCoroutine(UpdateProject());
    }

    public IEnumerator UpdateProject()
    {
        string projectName; //project name value to send into the update
        string sailorID; //project sailor id value to send into the update

        if (projectNameInput.GetComponent<UnityEngine.UI.Text>().text != "") //If we wrote something into the project name input, we wrote its value into the projectName parameter
        {
            projectName = projectNameInput.GetComponent<UnityEngine.UI.Text>().text;
            projectNameGO.GetComponent<UnityEngine.UI.Text>().text = projectName;
        }
        else //else, set the old projectName value into the parameter
        {
            projectName = projectNameGO.GetComponent<UnityEngine.UI.Text>().text;
        }

        if (projectSailorIdInput.GetComponent<UnityEngine.UI.Text>().text != "") //same treatment but for the sailorID parameter
        {
            sailorID = projectSailorIdInput.GetComponent<UnityEngine.UI.Text>().text;
            projectSailorIdGO.GetComponent<UnityEngine.UI.Text>().text = sailorID;
        }
        else
        {
            sailorID = projectSailorIdGO.GetComponent<UnityEngine.UI.Text>().text;
        }

        updateProjectPanel.SetActive(false); //set non active the update project panel

        string url = CONST.GetComponent<CONST>().url + "v1/updateproject"; //update project url

        WWWForm form = new WWWForm();                       // New form for web request

        form.AddField("projectID", projectId);    // Add to the form the values of the project to update
        form.AddField("userID", sailorID);
        form.AddField("road", project.road);
        form.AddField("roadNum", project.roadNum);
        form.AddField("roadExtra", project.roadExtra);
        form.AddField("zipcode", project.zipcode);
        form.AddField("city", project.city);
        form.AddField("customerID", project.customer);
        form.AddField("projectName", projectName);
        form.AddField("reference", project.reference);

        UnityWebRequest request = UnityWebRequest.Post(url, form);       // New request, passing url and form
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                                  // Set request authentications
        request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        request.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("*** ERROR: " + request.error + " ***");
        }
        else
        {
            Debug.Log("project updated");
        }
    }

    //function called when the user clicks in the cancel button of the update client pop-up
    public void cancelUpdateClientPanel()
    {
        updateClientPanel.SetActive(false); //set non active the update client panel
    }

    //function called when the user clicks in the confirm button of the update client pop-up
    public void confirmUpdateClientPanel()
    {
        StartCoroutine(UpdateCustomer());
    }

    //function that update the customer datas
    public IEnumerator UpdateCustomer()
    {
        string clientName;
        string clientSurname;
        string road;
        string roadNum;
        string roadExtra;
        string zipcode;
        string city;
        string phone;
        string email;

        if (clientNameInput.GetComponent<UnityEngine.UI.Text>().text != "")
        {
            clientName = clientNameInput.GetComponent<UnityEngine.UI.Text>().text;
            clientNameGO.GetComponent<UnityEngine.UI.Text>().text = clientName;
        }
        else
        {
            clientName = clientNameGO.GetComponent<UnityEngine.UI.Text>().text;
        }

        if (clientSurnameInput.GetComponent<UnityEngine.UI.Text>().text != "")
        {
            clientSurname = clientSurnameInput.GetComponent<UnityEngine.UI.Text>().text;
            clientSurnameGO.GetComponent<UnityEngine.UI.Text>().text = clientSurname;
        }
        else
        {
            clientSurname = clientSurnameGO.GetComponent<UnityEngine.UI.Text>().text;
        }

        if (clientRoadInput.GetComponent<UnityEngine.UI.Text>().text != "")
        {
            road = clientRoadInput.GetComponent<UnityEngine.UI.Text>().text;
            clientRoadGO.GetComponent<UnityEngine.UI.Text>().text = road;
        }
        else
        {
            road = clientRoadGO.GetComponent<UnityEngine.UI.Text>().text;
        }

        if (clientRoadNumInput.GetComponent<UnityEngine.UI.Text>().text != "")
        {
            roadNum = clientRoadNumInput.GetComponent<UnityEngine.UI.Text>().text;
            clientRoadNumGO.GetComponent<UnityEngine.UI.Text>().text = roadNum;
        }
        else
        {
            roadNum = clientRoadNumGO.GetComponent<UnityEngine.UI.Text>().text;
        }

        if (clientZipCodeInput.GetComponent<UnityEngine.UI.Text>().text != "")
        {
            zipcode = clientZipCodeInput.GetComponent<UnityEngine.UI.Text>().text;
            clientZipCodeGO.GetComponent<UnityEngine.UI.Text>().text = zipcode;
        }
        else
        {
            zipcode = clientZipCodeGO.GetComponent<UnityEngine.UI.Text>().text;
        }

        if (clientCityInput.GetComponent<UnityEngine.UI.Text>().text != "")
        {
            city = clientCityInput.GetComponent<UnityEngine.UI.Text>().text;
            clientCityGO.GetComponent<UnityEngine.UI.Text>().text = city;
        }
        else
        {
            city = clientCityGO.GetComponent<UnityEngine.UI.Text>().text;
        }

        if (clientRoadExtraInput.GetComponent<UnityEngine.UI.Text>().text != "")
        {
            roadExtra = clientRoadExtraInput.GetComponent<UnityEngine.UI.Text>().text;
            clientRoadExtraGO.GetComponent<UnityEngine.UI.Text>().text = roadExtra;
        }
        else
        {
            roadExtra = clientRoadExtraGO.GetComponent<UnityEngine.UI.Text>().text;
        }

        if (clientPhoneInput.GetComponent<UnityEngine.UI.Text>().text != "")
        {
            phone = clientPhoneInput.GetComponent<UnityEngine.UI.Text>().text;
            clientPhoneGO.GetComponent<UnityEngine.UI.Text>().text = phone;
        }
        else
        {
            phone = clientPhoneGO.GetComponent<UnityEngine.UI.Text>().text;
        }
        if (clientEmailInput.GetComponent<UnityEngine.UI.Text>().text != "")
        {
            email = clientEmailInput.GetComponent<UnityEngine.UI.Text>().text;
            clientEmailGO.GetComponent<UnityEngine.UI.Text>().text = email;
        }
        else
        {
            email = clientEmailGO.GetComponent<UnityEngine.UI.Text>().text;
        }

        updateClientPanel.SetActive(false); //set non active the update client panel

        string url = CONST.GetComponent<CONST>().url + "v1/updatecustomer"; //update project url

        WWWForm form = new WWWForm();                       // New form for web request

        form.AddField("name", clientName);    // Add to the form the values of the project to update
        form.AddField("surename", clientSurname);
        form.AddField("road", road);
        form.AddField("roadNum", roadNum);
        form.AddField("roadExtra", roadExtra);
        form.AddField("zipcode", zipcode);
        form.AddField("city", city);
        form.AddField("phone", phone);
        form.AddField("email", email);
        form.AddField("id", customer._id);


        UnityWebRequest request = UnityWebRequest.Post(url, form);       // New request, passing url and form
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                                  // Set request authentications
        request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        request.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("*** ERROR: " + request.error + " ***");
        }
        else
        {
            Debug.Log("customer updated");
        }
    }

    //function called when the user clicks in the cancel button of the delete estimation pop-up
    public void cancelDeleteEstimation()
    {
        confirmDeletePanel.SetActive(false); //set non active the  delete estimatio panel
    }

    //function called when the user clicks in the confirm button of the delete estimatio pop-up
    public void confirmDeleteEstimation()
    {
        StartCoroutine(DeleteEstimation(estimationSelected));
        confirmDeletePanel.SetActive(false); //set non active the delete estimation panel
    }

    public void EditEstimation(GameObject pItemSelected)
    {
        StartCoroutine(InstanciateUserDatas(project.user.id, pItemSelected));
    }

    //Instanciate the referent informations to show on the estimation pdf
    public IEnumerator InstanciateUserDatas(string userId, GameObject pItemSelected)
    {
        //http road to get the project datas
        var urlToGetUser = CONST.GetComponent<CONST>().url + "v1/getuserbyid";

        WWWForm form = new WWWForm();                       // New form for web request
        form.AddField("userID", userId);    // Add to the form the value of the ID of the project to get

        UnityWebRequest request = UnityWebRequest.Post(urlToGetUser, form);     // Create new form
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                      // Complete form with authentication datas
        request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        request.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("*** ERROR: " + request.error + " ***");
        }
        else
        {
            if (request.isDone)
            {
                string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);          // Get JSON file

                RequestAUser entity = JsonUtility.FromJson<RequestAUser>(jsonResult);         // Convert JSON file
                User user = entity.user; //Instanciate the customer object

                projectReferent = user.nom + " " + user.prenom; //Instanciate the referent informations to show on the estimation pdf

                components = new List<ComponentToShow>();
                urlToGetUser = CONST.GetComponent<CONST>().url + "v1/getmodulebyestimation";

                WWWForm form2 = new WWWForm(); // Add to the form the value of the ID of the project to get
                form2.AddField("estimationID", CONST.GetComponent<CONST>().selectedEstimationID.ToString());

                UnityWebRequest request2 = UnityWebRequest.Post(urlToGetUser, form2);     // Create new form
                request2.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                      // Complete form with authentication datas
                request2.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

                request2.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

                yield return request2.SendWebRequest();

                if (request2.isNetworkError || request2.isHttpError)
                {
                    Debug.Log("*** ERROR: " + request2.error + " ***");
                }
                else
                {
                    if (request2.isDone)
                    {
                        string jsonResult2 = System.Text.Encoding.UTF8.GetString(request2.downloadHandler.data);          // Get JSON file
                        RequestGetModuleByEstimation entity2 = JsonUtility.FromJson<RequestGetModuleByEstimation>(jsonResult2);         // Convert JSON file
                        List<ModuleGetModuleByEstimation> moduleList = entity2.module;


                        urlToGetUser = CONST.GetComponent<CONST>().url + "v1/getcomponentbyid";

                        foreach (ModuleGetModuleByEstimation module in moduleList)
                        {
                            if (module.type == "custom")
                            {
                                List<ComponentId> componentList = module.components;

                                for (int i = 0; i < componentList.Count; i++)
                                {
                                    WWWForm form3 = new WWWForm(); // Add to the form the value of the ID of the project to get
                                    form3.AddField("componentID", componentList[i].id);

                                    UnityWebRequest request3 = UnityWebRequest.Post(urlToGetUser, form3);     // Create new form
                                    request3.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                      // Complete form with authentication datas
                                    request3.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

                                    request3.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

                                    yield return request3.SendWebRequest();

                                    if (request3.isNetworkError || request3.isHttpError)
                                    {
                                        Debug.Log("*** ERROR: " + request3.error + " ***");
                                    }
                                    else
                                    {
                                        string jsonResult3 = System.Text.Encoding.UTF8.GetString(request3.downloadHandler.data);
                                        RequestAComponent entity3 = JsonUtility.FromJson<RequestAComponent>(jsonResult3);

                                        components.Add(entity3.component);
                                    }
                                }
                            }
                        }


                        customerSurename = clientSurnameGO.GetComponent<UnityEngine.UI.Text>().text;
                        customerFirstname = clientNameGO.GetComponent<UnityEngine.UI.Text>().text;
                        customerRoad = clientRoadGO.GetComponent<UnityEngine.UI.Text>().text;
                        customerRoadNum = clientRoadNumGO.GetComponent<UnityEngine.UI.Text>().text;
                        customerRoadExtra = clientRoadExtraGO.GetComponent<UnityEngine.UI.Text>().text;
                        customerCity = clientCityGO.GetComponent<UnityEngine.UI.Text>().text;
                        customerZipcode = clientZipCodeGO.GetComponent<UnityEngine.UI.Text>().text;
                        projectName = projectNameGO.GetComponent<UnityEngine.UI.Text>().text;
                        projectRef = projectIdGO.GetComponent<UnityEngine.UI.Text>().text;
                        projectRoad = project.road;
                        projectRoadNum = project.roadNum;
                        projectRoadExtra = project.roadExtra;
                        projectCity = project.city;
                        projectZipcode = project.zipcode;
                        estimationDate = pItemSelected.GetComponent<ItemListEstimation>().dateValue;
                        estimationRef = pItemSelected.GetComponent<ItemListEstimation>().idValue;
                        estimationPriceWtTaxes = pItemSelected.GetComponent<ItemListEstimation>().priceValue;
                        estimationDiscount = pItemSelected.GetComponent<ItemListEstimation>().discountValue;

                        string moneyUnit = " euros";

                        if (estimationPriceWtTaxes.Contains("."))
                        {
                            estimationPriceWtTaxes = estimationPriceWtTaxes.Replace(".", ",");
                        }

                        if (!estimationPriceWtTaxes.Contains(","))
                        {
                            estimationPriceWtTaxes += ",00";
                        }

                        double priceWtTaxes = Convert.ToDouble(estimationPriceWtTaxes); //price calculated with taxes. int parameter used for the calculate of the price without taxes and the discounted price
                        double discount = Convert.ToDouble(estimationDiscount.ToString());  //int value of the discount used for the calculations
                        double priceWtotTaxes = priceWtTaxes / 1.2; //double that contain the result of the price without taxes
                                                                    //calculation of the price discounted
                        double mult = priceWtTaxes * discount; //firstly we multiply the original price with the discount number
                        double sub = mult / 100; //secondly we divide the result per 100. It wil give the amount of the discount
                        double priceToPay = priceWtTaxes - sub;  //we substract the amout of the discount from the original price, and we have the price after the discounting

                        estimationPriceWtTaxes = priceWtTaxes.ToString();//string object that contains the price with the taxes
                        estimationPriceToPay = priceToPay.ToString(); //string object that contains the price discounted

                        string pdfEditor = CONST.GetComponent<CONST>().userName; //string object that contains the connected user name

                        string osRunning = SystemInfo.operatingSystem; //value that contains the current os
                        string[] osTab = osRunning.Split(' ');
                        string OS = osTab[0];
                        var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

                        estimationPriceWtotTaxes = priceWtotTaxes.ToString();
                        if (estimationPriceWtotTaxes.Contains("."))
                        {
                            estimationPriceWtotTaxes = estimationPriceWtTaxes.Replace(".", ",");
                        }

                        if (!estimationPriceWtotTaxes.Contains(","))
                        {
                            estimationPriceWtotTaxes += ",00";
                        }

                        string[] parts = estimationPriceWtotTaxes.Split(',');
                        estimationPriceWtotTaxes = parts[0] +","+ parts[1].Remove(2);

                        estimationRef = estimationRef.Remove(9);
                        estimationPriceWtotTaxes = estimationPriceWtotTaxes + moneyUnit;
                        estimationPriceWtTaxes  = estimationPriceWtTaxes + moneyUnit;

                        estimationPriceToPay  = estimationPriceToPay + moneyUnit;
                        if (estimationPriceToPay.Contains("."))
                        {
                            estimationPriceToPay = estimationPriceWtTaxes.Replace(".", ",");
                        }

                        if (!estimationPriceToPay.Contains(","))
                        {
                            estimationPriceToPay += ",00";
                        }


                        //pdf creation with windows os
                        if (OS.Equals("Windows"))
                        {
                            int normalCaracFont = 11; //font tall for the classic content
                            int titleCaracFont = 13; //font tall for the titles
                            int leftPage = 10; //x position for the left page content
                            int rightPage = 330; //x position for the right page content
                            string attachName = "Devis_" + pItemSelected.GetComponent<ItemListEstimation>().idValue + "_" + Timestamp + ".pdf"; //name of the document
                            pdfDocument myDoc = new pdfDocument("Sample Application", "Me", false); //creation of the pdf entity object
                            pdfPage myFirstPage = myDoc.addPage(); //creation of the first page entity object
                            pdfColor color = new pdfColor(predefinedColor.csBlack); //font color parameter

                            //text adding of all the pdf content. 
                            myFirstPage.addText("Devis de maison modulaire", 200, 772, predefinedFont.csHelveticaBold, 20, color);
                            myFirstPage.addText("Entreprise MADERA", leftPage, 740, predefinedFont.csHelveticaBold, titleCaracFont, color);
                            myFirstPage.addText("Numéro de voie : 70 ", leftPage, 726, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("Adresse : avenue Charles De Gaules", leftPage, 713, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("Supplément d'adresse : ------ ", leftPage, 699, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("Ville : Lille" + "  |  Code postal : " + customerZipcode, leftPage, 685, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("SIRET : ------- ", leftPage, 671, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("___________________________________________________________________________________________", leftPage, 659, predefinedFont.csHelvetica, normalCaracFont, color);


                            myFirstPage.addText("CLIENT", leftPage, 645, predefinedFont.csHelveticaBold, titleCaracFont, color);
                            myFirstPage.addText("Nom : " + customerSurename + "   |     Prénom : " + customerFirstname, leftPage, 630, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("Numéro de voie : " + customerRoadNum, leftPage, 616, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("Adresse : " + customerRoad, leftPage, 602, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("Supplément d'adresse : " + customerRoadExtra, leftPage, 588, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("Ville : " + customerCity + "  |  Code postal : " + customerZipcode, leftPage, 574, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("___________________________________________________________________________________________", leftPage, 560, predefinedFont.csHelvetica, normalCaracFont, color);

                            myFirstPage.addText("PROJET", leftPage, 545, predefinedFont.csHelveticaBold, titleCaracFont, color);
                            myFirstPage.addText("Projet : " + projectName, leftPage, 531, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("Référence du projet : " + projectRef, leftPage, 517, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("Numéro de voie : " + projectRoadNum, leftPage, 503, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("Adresse : " + projectRoad, leftPage, 489, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("Complément d'adresse : " + projectRoadExtra, leftPage, 475, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("Ville : " + projectCity + "  |  Code Postal : " + projectZipcode, leftPage, 461, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("Commercial référent : " + projectReferent, leftPage, 447, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("___________________________________________________________________________________________", leftPage, 443, predefinedFont.csHelvetica, normalCaracFont, color);

                            myFirstPage.addText("DEVIS", rightPage, 545, predefinedFont.csHelveticaBold, titleCaracFont, color);
                            myFirstPage.addText("Date : " + estimationDate, rightPage, 531, predefinedFont.csHelvetica, titleCaracFont, color);
                            myFirstPage.addText("Référence : " + estimationRef, rightPage, 517, predefinedFont.csHelvetica, titleCaracFont, color);
                            myFirstPage.addText("Prix HT : " + estimationPriceWtotTaxes , rightPage, 503, predefinedFont.csHelvetica, titleCaracFont, color);
                            myFirstPage.addText("TVA : 20% ", rightPage, 489, predefinedFont.csHelvetica, titleCaracFont, color);
                            myFirstPage.addText("Prix TTC : " + estimationPriceWtTaxes , rightPage, 475, predefinedFont.csHelvetica, titleCaracFont, color);
                            myFirstPage.addText("Remise : " + estimationDiscount + "%", rightPage, 461, predefinedFont.csHelvetica, titleCaracFont, color);
                            myFirstPage.addText("Coût final : " + estimationPriceToPay , rightPage, 447, predefinedFont.csHelvetica, titleCaracFont, color);

                            myFirstPage.addText("Date de la signature : ", leftPage, 403, predefinedFont.csHelveticaBold, titleCaracFont, color);
                            myFirstPage.addText("Signature du client : ", leftPage, 389, predefinedFont.csHelveticaBold, titleCaracFont, color);
                            myFirstPage.addText("Signature du commercial : ", rightPage, 389, predefinedFont.csHelveticaBold, titleCaracFont, color);
                            myFirstPage.addText(pdfEditor, 340, 375, predefinedFont.csHelvetica, titleCaracFont, color);

                            /*Set Header's Style*/

                            pdfPage mySecondPage = myDoc.addPage();

                            mySecondPage.addText("Liste des composants", 200, 772, predefinedFont.csHelveticaBold, 20, color);
                            mySecondPage.addText("_____________________________________________________________________________________________", leftPage, 740, predefinedFont.csHelveticaBold, titleCaracFont, color);
                            mySecondPage.addText("Nom du composants", leftPage, 726, predefinedFont.csHelvetica, normalCaracFont, color);
                            mySecondPage.addText("Quantité", leftPage + 150, 726, predefinedFont.csHelvetica, normalCaracFont, color);
                            mySecondPage.addText("Prix par unité", leftPage + 300, 726, predefinedFont.csHelvetica, normalCaracFont, color);
                            mySecondPage.addText("Prix total", leftPage + 450, 726, predefinedFont.csHelvetica, normalCaracFont, color);
                            mySecondPage.addText("_____________________________________________________________________________________________", leftPage, 712, predefinedFont.csHelveticaBold, titleCaracFont, color);



                            List<ComponentLine> componentLines = new List<ComponentLine>();
                            foreach (ComponentToShow compo in components)
                            {
                                if (componentLines.Count == 0)
                                {

                                    componentLines.Add(new ComponentLine(compo.name, 1, Int32.Parse(compo.cost)));
                                }
                                else
                                {
                                    bool newLine = false;
                                    foreach (ComponentLine compoLine in componentLines)
                                    {

                                        if (compo.name.Equals(compoLine.name))
                                        {
                                            newLine = false;
                                            compoLine.qte++;
                                            break;
                                        }
                                        else
                                        {
                                            newLine = true;
                                        }
                                    }
                                    if (newLine)
                                    {
                                        componentLines.Add(new ComponentLine(compo.name, 1, Int32.Parse(compo.cost)));
                                    }
                                }
                            }

                            int y = 712 - 14;
                            foreach (ComponentLine line in componentLines)
                            {
                                string unitPrice = line.price.ToString() + moneyUnit;
                                string totalPrice = (line.price * line.qte).ToString() + moneyUnit;
                                mySecondPage.addText(line.name, leftPage, y, predefinedFont.csHelvetica, normalCaracFont, color);
                                mySecondPage.addText(line.qte.ToString(), leftPage + 150, y, predefinedFont.csHelvetica, normalCaracFont, color);
                                mySecondPage.addText(unitPrice, leftPage + 300, y, predefinedFont.csHelvetica, normalCaracFont, color);
                                mySecondPage.addText(totalPrice, leftPage + 450, y, predefinedFont.csHelvetica, normalCaracFont, color);

                                y = y - 14;
                            }
                            myDoc.createPDF(@"C:\Users\Public\" + attachName);

                            string pathNotif = "C:\\Users\\Public\\" + attachName;
                            
                            notifyCanvas.SetActive(true);
                            notifText.GetComponent<UnityEngine.UI.Text>().text = pathNotif;

                        }
                        
                        //pdf creation with Android os
                        else if (OS.Equals("Android"))
                        {
                            int normalCaracFont = 11; //font tall for the classic content
                            int titleCaracFont = 13; //font tall for the titles
                            int leftPage = 10; //x position for the left page content
                            int rightPage = 330; //x position for the right page content
                            string attachName = "Devis_" + pItemSelected.GetComponent<ItemListEstimation>().idValue + "_" + Timestamp + ".pdf"; //name of the document
                            pdfDocument myDoc = new pdfDocument("Sample Application", "Me", false); //creation of the pdf entity object
                            pdfPage myFirstPage = myDoc.addPage(); //creation of the first page entity object
                            pdfColor color = new pdfColor(predefinedColor.csBlack); //font color parameter

                            //text adding of all the pdf content. 
                            myFirstPage.addText("Devis de maison modulaire", 200, 772, predefinedFont.csHelveticaBold, 20, color);
                            myFirstPage.addText("Entreprise MADERA", leftPage, 740, predefinedFont.csHelveticaBold, titleCaracFont, color);
                            myFirstPage.addText("Numéro de voie : 70 ", leftPage, 726, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("Adresse : avenue Charles De Gaules", leftPage, 713, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("Supplément d'adresse : ------ ", leftPage, 699, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("Ville : Lille" + "  |  Code postal : 59000" + customerZipcode, leftPage, 685, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("SIRET : ------- ", leftPage, 671, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("___________________________________________________________________________________________", leftPage, 659, predefinedFont.csHelvetica, normalCaracFont, color);


                            myFirstPage.addText("CLIENT", leftPage, 645, predefinedFont.csHelveticaBold, titleCaracFont, color);
                            myFirstPage.addText("Nom : " + customerSurename + "   |     Prénom : " + customerFirstname, leftPage, 630, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("Numéro de voie : " + customerRoadNum, leftPage, 616, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("Adresse : " + customerRoad, leftPage, 602, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("Supplément d'adresse : " + customerRoadExtra, leftPage, 588, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("Ville : " + customerCity + "  |  Code postal : " + customerZipcode, leftPage, 574, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("___________________________________________________________________________________________", leftPage, 560, predefinedFont.csHelvetica, normalCaracFont, color);

                            myFirstPage.addText("PROJET", leftPage, 545, predefinedFont.csHelveticaBold, titleCaracFont, color);
                            myFirstPage.addText("Projet : " + projectName, leftPage, 531, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("Référence du projet : " + projectRef, leftPage, 517, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("Numéro de voie : " + projectRoadNum, leftPage, 503, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("Adresse : " + projectRoad, leftPage, 489, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("Complément d'adresse : " + projectRoadExtra, leftPage, 475, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("Ville : " + projectCity + "  |  Code Postal : " + projectZipcode, leftPage, 461, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("Commercial référent : " + projectReferent, leftPage, 447, predefinedFont.csHelvetica, normalCaracFont, color);
                            myFirstPage.addText("___________________________________________________________________________________________", leftPage, 443, predefinedFont.csHelvetica, normalCaracFont, color);

                            myFirstPage.addText("DEVIS", rightPage, 545, predefinedFont.csHelveticaBold, titleCaracFont, color);
                            myFirstPage.addText("Date : " + estimationDate, rightPage, 531, predefinedFont.csHelvetica, titleCaracFont, color);
                            myFirstPage.addText("Référence : " + estimationRef.Remove(9), rightPage, 517, predefinedFont.csHelvetica, titleCaracFont, color);
                            myFirstPage.addText("Prix HT : " + estimationPriceWtotTaxes , rightPage, 503, predefinedFont.csHelvetica, titleCaracFont, color);
                            myFirstPage.addText("TVA : 20% ", rightPage, 489, predefinedFont.csHelvetica, titleCaracFont, color);
                            myFirstPage.addText("Prix TTC : " + estimationPriceWtTaxes , rightPage, 475, predefinedFont.csHelvetica, titleCaracFont, color);
                            myFirstPage.addText("Remise : " + estimationDiscount + "%", rightPage, 461, predefinedFont.csHelvetica, titleCaracFont, color);
                            myFirstPage.addText("Coût final : " + estimationPriceToPay , rightPage, 447, predefinedFont.csHelvetica, titleCaracFont, color);

                            myFirstPage.addText("Date de la signature : ", leftPage, 403, predefinedFont.csHelveticaBold, titleCaracFont, color);
                            myFirstPage.addText("Signature du client : ", leftPage, 389, predefinedFont.csHelveticaBold, titleCaracFont, color);
                            myFirstPage.addText("Signature du commercial : ", rightPage, 389, predefinedFont.csHelveticaBold, titleCaracFont, color);
                            myFirstPage.addText(pdfEditor, 340, 375, predefinedFont.csHelvetica, titleCaracFont, color);


                            pdfPage mySecondPage = myDoc.addPage();

                            mySecondPage.addText("Liste des composants", 200, 772, predefinedFont.csHelveticaBold, 20, color);
                            mySecondPage.addText("_____________________________________________________________________________________________", leftPage, 740, predefinedFont.csHelveticaBold, titleCaracFont, color);
                            mySecondPage.addText("Nom du composants", leftPage, 726, predefinedFont.csHelvetica, normalCaracFont, color);
                            mySecondPage.addText("Quantité", leftPage + 150, 726, predefinedFont.csHelvetica, normalCaracFont, color);
                            mySecondPage.addText("Prix par unité", leftPage + 300, 726, predefinedFont.csHelvetica, normalCaracFont, color);
                            mySecondPage.addText("Prix total", leftPage + 450, 726, predefinedFont.csHelvetica, normalCaracFont, color);
                            mySecondPage.addText("_____________________________________________________________________________________________", leftPage, 712, predefinedFont.csHelveticaBold, titleCaracFont, color);



                            List<ComponentLine> componentLines = new List<ComponentLine>();
                            foreach (ComponentToShow compo in components)
                            {
                                if (componentLines.Count == 0)
                                {

                                    componentLines.Add(new ComponentLine(compo.name, 1, Int32.Parse(compo.cost)));
                                }
                                else
                                {
                                    bool newLine = false;
                                    foreach (ComponentLine compoLine in componentLines)
                                    {

                                        if (compo.name.Equals(compoLine.name))
                                        {
                                            newLine = false;
                                            compoLine.qte++;
                                            break;
                                        }
                                        else
                                        {
                                            newLine = true;
                                        }
                                    }
                                    if (newLine)
                                    {
                                        componentLines.Add(new ComponentLine(compo.name, 1, Int32.Parse(compo.cost)));
                                    }
                                }
                            }

                            int y = 712 - 14;
                            foreach (ComponentLine line in componentLines)
                            {
                                string unitPrice = line.price.ToString() + moneyUnit;
                                string totalPrice = (line.price * line.qte).ToString() + moneyUnit;
                                mySecondPage.addText(line.name, leftPage, y, predefinedFont.csHelvetica, normalCaracFont, color);
                                mySecondPage.addText(line.qte.ToString(), leftPage + 150, y, predefinedFont.csHelvetica, normalCaracFont, color);
                                mySecondPage.addText(unitPrice, leftPage + 300, y, predefinedFont.csHelvetica, normalCaracFont, color);
                                mySecondPage.addText(totalPrice, leftPage + 450, y, predefinedFont.csHelvetica, normalCaracFont, color);

                                y = y - 14;
                            }

                            /*Set Header's Style*/
                            myDoc.createPDF(@"C:\Users\Public\" + attachName);
                            /*Set Header's Style*/
                            myDoc.createPDF(attachName);

                            string pathNotif = "@\"\\\"" + attachName;
                            
                            notifyCanvas.SetActive(true);
                            notifText.GetComponent<UnityEngine.UI.Text>().text = pathNotif;

                        }
                    }
                }
            }
        }
    }

    public void CloseNotifyWindow()
    {
        notifyCanvas.SetActive(false);
    }
}


