using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    EstimationSelected estimationSelected = new EstimationSelected();

    // Start is called before the first frame update
    void Start()
    {
        //hide the pop-ups panels
        updateClientPanel.SetActive(false);
        updateProjectPanel.SetActive(false);
        confirmDeletePanel.SetActive(false);

        CONST = GameObject.Find("CONST");                       // Get const object

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

                List <EstimationId> estimationIdList = project.estimation;

                //set the gameObjects content from the client and the project parameters 
                frameTitle.GetComponent<UnityEngine.UI.Text>().text = "Projet " + projectId;
                projectIdGO.GetComponent<UnityEngine.UI.Text>().text = projectId;
                projectNameGO.GetComponent<UnityEngine.UI.Text>().text = project.name;
                projectDateGO.GetComponent<UnityEngine.UI.Text>().text = project.date;
                projectSailorIdGO.GetComponent<UnityEngine.UI.Text>().text = project.reference;
                clientIdGO.GetComponent<UnityEngine.UI.Text>().text = customer._id;
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
                foreach(var item in estimationIdList)
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

            // Change text value of the list item
            idValue.GetComponent<UnityEngine.UI.Text>().text = estimation._id;
            priceValue.GetComponent<UnityEngine.UI.Text>().text = estimation.price;
            stateValue.GetComponent<UnityEngine.UI.Text>().text = estimation.state;
            dateValue.GetComponent<UnityEngine.UI.Text>().text = dateValueText;

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

    /* Function to show the technical folder */
    public void ShowTechnicalFolder(GameObject pItemSelected)
    {
        CONST.GetComponent<CONST>().selectedEstimationID = pItemSelected.GetComponent<ItemListEstimation>().idValue;   // Assign the values for the next scene
        
        DontDestroyOnLoad(CONST);                                                   // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 5); //load the next scene
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
        form.AddField("userID", projectSailorId);
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
        Debug.Log("deleted estimation : " + estimationSelected.id);
        StartCoroutine(DeleteEstimation(estimationSelected));
        confirmDeletePanel.SetActive(false); //set non active the delete estimation panel
    }
}
