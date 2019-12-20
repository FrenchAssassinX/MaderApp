using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpdateProjectList : MonoBehaviour
{
    /* ------------------------------------     DECLARE DATAS PART     ------------------------------------ */
    public GameObject CONST;                                    // CONST object contains server route, token and user infos

    private string getProjectUrl = "v1/getallproject";          // Specific route to get all projects
    private string getAllCustomersUrl = "v1/getallcustomer";    // Specific route to get all customers
    private string getCustomerByIDUrl = "v1/getcustomerbyid";   // Specific route to get a customer
    private string deleteProjectUrl = "v1/deleteproject";       // Specific route to delete one project

    public GameObject listItemPrefab;                           // Prefab item to display all elements in project list
    public GameObject gridList;                                 // Grid to insert project prefab items

    public GameObject errorFormatDateText;                      // Text to display when date is not in correct format

    /*                      ---------- Delete panel props ----------                                */
    public GameObject confirmPanels;                            // Panels to display confirm actions
    public GameObject deletePanel;                              // Delete panel
    public Button deletePanelConfirmButton;                     // Confirm button on Delete panel
    public Button deletePanelCancelButton;                      // Cancel button on Delete panel
    public GameObject deletePanelText;                          // Text of Delete panel
    public GameObject deletePanelTextError;                     // Error text of Delete panel
    /*                      ---------- End delete panel props ----------                                */

    public List<Project> listProjects;                          // List useful for filtering

    ProjectSelected projectSelected = new ProjectSelected();    // Create the project as a GameObject to pass in another scene
    /* ------------------------------------     END DECLARE DATAS PART     ------------------------------------ */


    void Start()
    {
        CONST = GameObject.Find("CONST");                       // Get const object
        gridList = GameObject.Find("GridList");                 // Get grid of the list 

        errorFormatDateText.SetActive(false);                   // Don't display error message on start

        /* Don't display panels of confirm action on start */
        confirmPanels.SetActive(false);
        deletePanel.SetActive(false);
        deletePanelTextError.SetActive(false);

        /* Assign OnClick behaviours to delete panel buttons */
        deletePanelCancelButton.GetComponent<Button>();
        deletePanelCancelButton.onClick.AddListener(HideDeletePanel);
        deletePanelConfirmButton.onClick.AddListener(StartDeleteProject);

        StartCoroutine(GetAllProjects());                       // Start script to find projects on databse
    }


    /* ------------------------------------     CHANGE SCENE PART     ------------------------------------ */
    public void GoBackToMenu()
    {
        DontDestroyOnLoad(CONST);                                                   // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);       //Go back to Home Scene
    }

    /* Function useful when a project was selected on the list: Go to Project Sheet */
    public void GoToViewProject(GameObject pItemSelected)
    {
        CONST.GetComponent<CONST>().selectedProjectID = pItemSelected.GetComponent<ItemListProject>().id;   // Assign the ID for the next scene

        DontDestroyOnLoad(CONST);                                                   // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);       //Go to Project Sheet Scene
    }
    /* -----------------------------------    END CHANGE SCENE PART     ---------------------------------- */


    /* ------------------------------------     DISPLAY ELEMENT PART     ------------------------------------ */
    /* Function to display Delete project panel */
    public void DisplayDeleteProject(GameObject pItemSelected)
    {
        /* Make panels visible */
        confirmPanels.SetActive(true);
        deletePanel.SetActive(true);
        deletePanelText.SetActive(true);

        // Keep the ID of the selected project
        projectSelected.id = pItemSelected.GetComponent<ItemListProject>().id;
    }

    /* Function to hide Delete panel */
    public void HideDeletePanel()
    {
        /* Make panels not visible */
        confirmPanels.SetActive(false);
        deletePanel.SetActive(false);
        deletePanelText.SetActive(false);
        deletePanelTextError.SetActive(false);
    }
    /* -----------------------------------    END DISPLAY ELEMENT PART     ---------------------------------- */


    /* ------------------------------------     WEB REQUEST PART     ------------------------------------ */
    /* Intermediary function to run DeleteProject function. Necessary because OnClick don't support IEnumerator function as parameters */
    public void StartDeleteProject()
    {
        StartCoroutine(DeleteProject(projectSelected));             // Lauch function
    }

    /* Function to get all current projects on database */
    private IEnumerator GetAllProjects()
    {
        UnityWebRequest request = UnityWebRequest.Get(CONST.GetComponent<CONST>().url + getProjectUrl);     // Create new form

        request.certificateHandler = new CONST.BypassCertificate();    // Bypass certificate for https

        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                      // Complete form with authentication datas
        request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        yield return request.SendWebRequest();                      // Send request                                                              

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("*** ERROR: " + request.error + " ***");
        }
        else
        {
            if (request.isDone)
            {
                string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);          // Get JSON file

                RequestGetAllProject entities = JsonUtility.FromJson<RequestGetAllProject>(jsonResult);         // Convert JSON file

                // foreach to retrieve every projects
                foreach (var item in entities.projects)
                {
                    // Create project with datas from database
                    Project entity = item;

                    /* Retrieve customer linked to the current project */
                    WWWForm formCustomer = new WWWForm();                       // New form for web request
                    formCustomer.AddField("customerID", entity.customer);       // Add to the form the value of the ID of the project to delete

                    UnityWebRequest requestCustomer = UnityWebRequest.Post(CONST.GetComponent<CONST>().url + getCustomerByIDUrl, formCustomer);     // New request, passing url and form
                    requestCustomer.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                                          // Set request authentications
                    requestCustomer.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

                    requestCustomer.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

                    yield return requestCustomer.SendWebRequest();

                    if (requestCustomer.isNetworkError || requestCustomer.isHttpError)
                    {
                        Debug.Log("ERROR: " + requestCustomer.error);
                    }
                    else
                    {
                        if (requestCustomer.isDone)
                        {
                            string jsonResultCustomer = System.Text.Encoding.UTF8.GetString(requestCustomer.downloadHandler.data);          // Get JSON file
                            //Debug.Log(jsonResultCustomer);

                            RequestACustomer entityCustomer = JsonUtility.FromJson<RequestACustomer>(jsonResultCustomer);
                            Customer customer = entityCustomer.customer;

                            // Create prefab
                            GameObject listItem = Instantiate(listItemPrefab, gridList.transform.position, Quaternion.identity);
                            // Set GridList as parent of prefab in project hierarchy
                            listItem.transform.SetParent(gridList.transform);
                            listItem.GetComponent<RectTransform>().localScale = gridList.GetComponent<RectTransform>().localScale;
                            listItem.GetComponent<RectTransform>().sizeDelta = new Vector2(gridList.GetComponent<RectTransform>().sizeDelta.x, listItem.GetComponent<RectTransform>().sizeDelta.y);

                            // Find children in listItem to use their
                            GameObject dateValue = GameObject.Find("DateValue");
                            GameObject refValue = GameObject.Find("RefValue");
                            GameObject clientValue = GameObject.Find("ClientValue");
                            GameObject sellerValue = GameObject.Find("SellerValue");

                            /* Change size of element to fit on parent size */
                            /* Local scale */
                            dateValue.GetComponent<RectTransform>().localScale = listItem.GetComponent<RectTransform>().localScale;
                            refValue.GetComponent<RectTransform>().localScale = listItem.GetComponent<RectTransform>().localScale;
                            clientValue.GetComponent<RectTransform>().localScale = listItem.GetComponent<RectTransform>().localScale;
                            sellerValue.GetComponent<RectTransform>().localScale = listItem.GetComponent<RectTransform>().localScale;
                            /* Size delta */
                            dateValue.GetComponent<RectTransform>().sizeDelta = listItem.GetComponent<RectTransform>().sizeDelta;
                            refValue.GetComponent<RectTransform>().sizeDelta = listItem.GetComponent<RectTransform>().sizeDelta;
                            clientValue.GetComponent<RectTransform>().sizeDelta = listItem.GetComponent<RectTransform>().sizeDelta;
                            sellerValue.GetComponent<RectTransform>().sizeDelta = listItem.GetComponent<RectTransform>().sizeDelta;

                            // Customize props name of the prefab to find it when it will be create
                            dateValue.name = dateValue.name + listItem.GetComponent<ItemListProject>().name;
                            refValue.name = refValue.name + listItem.GetComponent<ItemListProject>().name;
                            clientValue.name = clientValue.name + listItem.GetComponent<ItemListProject>().name;
                            sellerValue.name = sellerValue.name + listItem.GetComponent<ItemListProject>().name;

                            // Formating date to French timeset
                            string dateValueText = entity.date.ToString();
                            dateValueText = dateValueText.Remove(10, 14);
                            DateTime dateTimeText = Convert.ToDateTime(dateValueText);
                            dateValueText = dateTimeText.ToString("dd-MM-yyyy", CultureInfo.CreateSpecificCulture("fr-FR"));

                            // Change text value of the list item
                            dateValue.GetComponent<UnityEngine.UI.Text>().text = dateValueText;
                            refValue.GetComponent<UnityEngine.UI.Text>().text = entity.reference.ToString();
                            clientValue.GetComponent<UnityEngine.UI.Text>().text = customer.name.ToString();
                            sellerValue.GetComponent<UnityEngine.UI.Text>().text = entity.user.matricule.ToString();

                            // ID to keep for view project sheet or deleting project
                            listItem.GetComponent<ItemListProject>().id = entity._id.ToString();
                            listItem.GetComponent<ItemListProject>().date = dateValueText;
                            listItem.GetComponent<ItemListProject>().clientName = clientValue.GetComponent<UnityEngine.UI.Text>().text;
                            listItem.GetComponent<ItemListProject>().referentName = sellerValue.GetComponent<UnityEngine.UI.Text>().text;

                            listProjects.Add(entity);
                        }
                    }
                }
            }
        }
    }

    /* Function to delete project in the database */
    private IEnumerator DeleteProject(ProjectSelected pProjectSelected)
    {
        WWWForm form = new WWWForm();                       // New form for web request
        form.AddField("projectID", pProjectSelected.id);    // Add to the form the value of the ID of the project to delete

        UnityWebRequest request = UnityWebRequest.Post(CONST.GetComponent<CONST>().url + deleteProjectUrl, form);       // New request, passing url and form
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                                  // Set request authentications
        request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        request.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            /* Display error message */
            deletePanelText.SetActive(false);
            deletePanelTextError.SetActive(true);
        }
        else
        {
            if (request.isDone)
            {
                HideDeletePanel();                          // Hide delete panel

                /* Refresh list of projects */
                foreach (Transform childList in gridList.transform)
                {
                    GameObject.Destroy(childList.gameObject);
                }

                StartCoroutine(GetAllProjects());           // Recall all projects from databse
            }
        }

    }
    /* -----------------------------------    END WEB REQUEST PART     ---------------------------------- */


    /* ------------------------------------     FILTERS PART     ------------------------------------ */
    public void ClearFilter()
    {
        /* Find the fields in scene */
        GameObject startingDate = GameObject.Find("StartingDateField");
        GameObject endingDate = GameObject.Find("EndDateField");
        GameObject clientName = GameObject.Find("SearchClientByName");
        GameObject referentName = GameObject.Find("SearchReferentByName");

        /* Clear text in fields */
        startingDate.GetComponent<InputField>().text = "";
        endingDate.GetComponent<InputField>().text = "";
        clientName.GetComponent<InputField>().text = "";
        referentName.GetComponent<InputField>().text = "";

        /* Refresh list of projects */
        foreach (Transform childList in gridList.transform)
        {
            GameObject.Destroy(childList.gameObject);
        }

        /* Call all projects */
        StartCoroutine(GetAllProjects());
    }

    public void StartFilter()
    {
        /* Verify if fields are filled or not */
        GameObject startingDate = GameObject.Find("StartingDateField");
        GameObject endingDate = GameObject.Find("EndDateField");
        GameObject clientName = GameObject.Find("SearchClientByName");
        GameObject referentName = GameObject.Find("SearchReferentByName");

        /* Booleans useful for run the filters function if datas are valid */
        bool bStartDateValid = false;
        bool bEndDateValid = false;

        /* Filters by date */
        if (!startingDate.GetComponent<InputField>().text.Equals(""))
        {
            /* Verify after formating date if the format is good */
            try
            {
                string formatingStartDate = startingDate.GetComponent<InputField>().text;
                DateTime startingDateText = Convert.ToDateTime(formatingStartDate, CultureInfo.CreateSpecificCulture("fr-FR"));
                formatingStartDate = startingDateText.ToString("dd-MM-yyyy", CultureInfo.CreateSpecificCulture("fr-FR"));
                startingDate.GetComponent<InputField>().text = formatingStartDate;

                bStartDateValid = true;
            }
            /* Else display error message on UI interface */
            catch (FormatException)
            {
                errorFormatDateText.SetActive(true);
            }

            /* If end date is empty, then autocomplete by date of the day */
            if (endingDate.GetComponent<InputField>().text.Equals(""))
            {
                DateTime date = DateTime.Now;
                string formatingDate = date.ToString("dd-MM-yyyy", CultureInfo.CreateSpecificCulture("fr-FR"));
                endingDate.GetComponent<InputField>().text = formatingDate;

                bEndDateValid = true;
            }
            else
            {
                /* Verify after formating date if the format is good */
                try
                {
                    string formatingEndDate = endingDate.GetComponent<InputField>().text;
                    DateTime endingDateText = Convert.ToDateTime(formatingEndDate, CultureInfo.CreateSpecificCulture("fr-FR"));
                    formatingEndDate = endingDateText.ToString("dd-MM-yyyy", CultureInfo.CreateSpecificCulture("fr-FR"));
                    endingDate.GetComponent<InputField>().text = formatingEndDate;

                    bEndDateValid = true;
                }
                /* Else display error message on UI interface */
                catch (FormatException)
                {
                    errorFormatDateText.SetActive(true);
                }
            }

            /* If dates are good, then run the FilterByDate function */
            if (bStartDateValid && bEndDateValid)
            {
                FilterByDate();
            }
        }

        /* If client name textfield is not empty */
        if (!clientName.GetComponent<InputField>().text.Equals(""))
        {
            FilterByClientName(clientName);
        }

        /* If referent name textfield is not empty */
        if (!referentName.GetComponent<InputField>().text.Equals(""))
        {
            FilterByReferentName(referentName);
        }
    }

    /* Function to filter projects between two dates */
    public void FilterByDate()
    {
        // Disable error message, the function run so, no errors detecting in StartFilter function
        errorFormatDateText.SetActive(false);

        // Find field on the scene
        GameObject startingDate = GameObject.Find("StartingDateField");
        GameObject endingDate = GameObject.Find("EndDateField");

        // Get text of the two fields
        string startingFilter = startingDate.GetComponent<InputField>().text;
        string endingFilter = endingDate.GetComponent<InputField>().text;

        // Convert string date value to DateTime
        DateTime startingDateFilter = Convert.ToDateTime(startingFilter, CultureInfo.CreateSpecificCulture("fr-FR"));
        DateTime endingDateFilter = Convert.ToDateTime(endingFilter, CultureInfo.CreateSpecificCulture("fr-FR"));

        // Foreach to verify all projects in the list 
        foreach (Transform childList in gridList.transform)
        {
            GameObject child = childList.gameObject;                                            // Convert Transform to Game Object to modify it

            // Convert date of the current project to DateTime
            DateTime childListDate = Convert.ToDateTime(child.GetComponent<ItemListProject>().date, CultureInfo.CreateSpecificCulture("fr-FR"));

            int earlierDateCompare = DateTime.Compare(childListDate, startingDateFilter);       // Compare previous date
            int laterDateCompare = DateTime.Compare(childListDate, endingDateFilter);           // Compare last date

            /* If date of the project is earlier than the StartDateFilter: destroy this element on the list */
            if (earlierDateCompare < 0)
            {
                Destroy(child);
            }
            /* If date of the project is later than the EndDateFilter: destroy this element on the list */
            if (laterDateCompare > 0)
            {
                Destroy(child);
            }
        }
    }

    /* Function to filtering projects by client name */
    public void FilterByClientName(GameObject pClientName)
    {
        // Foreach to verify all projects in the list 
        foreach (Transform childList in gridList.transform)
        {
            GameObject child = childList.gameObject;                                        // Convert Transform to Game Object to modify it
            string childClientName = child.GetComponent<ItemListProject>().clientName;      // Get client name
            childClientName = childClientName.ToLower();                                    // ToLower to avoid error of capital characters in the string when searching
            Debug.Log("Search: " + childClientName);

            /* If the client name doesn't contains the string search: delete the current item from the list */
            if (!childClientName.Contains(pClientName.GetComponent<InputField>().text.ToLower()))
            {
                Destroy(child);
            }
        }
    }

    /* Function to filtering projects by referent name */
    public void FilterByReferentName(GameObject pReferentName)
    {
        // Foreach to verify all projects in the list 
        foreach (Transform childList in gridList.transform)
        {
            GameObject child = childList.gameObject;                                        // Convert Transform to Game Object to modify it
            string childReferentName = child.GetComponent<ItemListProject>().referentName;  // Get referent name
            childReferentName = childReferentName.ToLower();                                // ToLower to avoid error of capital characters in the string when searching

            /* If the referent name doesn't contains the string search: delete the current item from the list */
            if (!childReferentName.Contains(pReferentName.GetComponent<InputField>().text.ToLower()))
            {
                Destroy(child);
            }
        }
    }
    /* -----------------------------------    END FILTERS PART     ---------------------------------- */
}
