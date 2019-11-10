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
    public GameObject CONST;                                    // CONST object contains server route, token and user infos

    private string getProjectUrl = "v1/getallproject";          // Specific route to get all projects
    private string deleteProjectUrl = "v1/deleteproject";       // Specific route to delete one project

    public GameObject listItemPrefab;                           // Prefab item to display all elements in project list
    public GameObject gridList;                                 // Grid to insert project prefab items

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

    void Start()
    {
        CONST = GameObject.Find("CONST");                       // Get const object
        gridList = GameObject.Find("GridList");                 // Get grid of the list 

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

    public void GoBackToMenu()
    {
        DontDestroyOnLoad(CONST);                                                   // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);       //Go back to Home Scene
    }

    /* Function useful when a project was selected on the list: Go to Project Sheet */
    public void GoToViewProject(GameObject pItemSelected)
    {
        projectSelected.name = "ProjectSelected";                                   // Change name of the GameObject to find it easely ine the hierarchy
        projectSelected.id = pItemSelected.GetComponent<ItemListProject>().id;      // Assign the ID for the next scene

        DontDestroyOnLoad(projectSelected);                                         // Pass the project selected to the next scene
        DontDestroyOnLoad(CONST);                                                   // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);       //Go to Project Sheet Scene
    }

    /* Function to get all current projects on database */
    private IEnumerator GetAllProjects()
    {
        UnityWebRequest request = UnityWebRequest.Get(CONST.GetComponent<CONST>().url + getProjectUrl);     // Create new form
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

                    // Create prefab
                    GameObject listItem = Instantiate(listItemPrefab, gridList.transform.position, Quaternion.identity);
                    // Set GridList as parent of prefab in project hierarchy
                    listItem.transform.SetParent(gridList.transform);

                    // Find children in listItem to use their
                    GameObject dateValue = GameObject.Find("DateValue");
                    GameObject refValue = GameObject.Find("RefValue");
                    GameObject clientValue = GameObject.Find("ClientValue");
                    GameObject sellerValue = GameObject.Find("SellerValue");

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
                    clientValue.GetComponent<UnityEngine.UI.Text>().text = entity.customer.ToString();
                    sellerValue.GetComponent<UnityEngine.UI.Text>().text = entity.user.matricule.ToString();

                    // ID to keep for view project sheet or deleting project
                    listItem.GetComponent<ItemListProject>().id = entity._id.ToString();

                    listProjects.Add(entity);
                }
            }
        }
    }

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

    /* Intermediary function to run DeleteProject function. Necessary because OnClick don't support IEnumerator function as parameters */
    public void StartDeleteProject()
    {
        StartCoroutine(DeleteProject(projectSelected));             // Lauch function
    }

    /* Function to delete project in the database */
    private IEnumerator DeleteProject(ProjectSelected pProjectSelected)
    {
        WWWForm form = new WWWForm();                       // New form for web request
        form.AddField("projectID", pProjectSelected.id);    // Add to the form the value of the ID of the project to delete

        UnityWebRequest request = UnityWebRequest.Post(CONST.GetComponent<CONST>().url + deleteProjectUrl, form);       // New request, passing url and form
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                                  // Set request authentications
        request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

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

    public void StartFilter()
    {
        /* Verify if dates fields are filled or not */
        GameObject startingDate = GameObject.Find("StartingDateField");
        GameObject endingDate = GameObject.Find("EndDateField");

        if (!startingDate.GetComponent<InputField>().text.Equals(""))
        {
            if (endingDate.GetComponent<InputField>().text.Equals(""))
            {
                endingDate.GetComponent<InputField>().text = DateTime.Now.ToString();
                Debug.Log("Ending date = " + endingDate.GetComponent<InputField>().text);
            }

            //FilterByDate();
        }
    }

    public void FilterByDate()
    {
        /* Refresh list of projects */
        foreach (Transform childList in gridList.transform)
        {
            GameObject.Destroy(childList.gameObject);
        }

        GameObject startingDate = GameObject.Find("StartingDateText");
        GameObject endingDate = GameObject.Find("EndDateText");

        string startingFilter = startingDate.GetComponent<Text>().text;
        string endingFilter = endingDate.GetComponent<Text>().text;

        DateTime startingDateFilter = Convert.ToDateTime(startingFilter);
        DateTime endingDateFilter = Convert.ToDateTime(endingFilter);

        /*foreach (Project project in listProjects)
        {
            if (project.date > )
            {

            }
        }*/

        foreach (Transform childList in gridList.transform)
        {
            DateTime childListDate = Convert.ToDateTime(childList.GetComponent<Project>().date);

            if (childListDate < startingDateFilter || 
                childListDate > endingDateFilter)
            {
                GameObject.Destroy(childList.gameObject);
            }
        }
    }
}
