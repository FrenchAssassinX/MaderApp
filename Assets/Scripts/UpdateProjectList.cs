using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class UpdateProjectList : MonoBehaviour
{
    public GameObject CONST;                                // CONST object contains server route, token and user infos

    private string getProjectUrl = "v1/getallproject";     // Specific route to get all projects

    public GameObject listItemPrefab;                       // Prefab item to display all elements in project list
    public GameObject gridList;                             // Grid to insert project prefab items

    void Start()
    {
        CONST = GameObject.Find("CONST");                   // Get const object
        gridList = GameObject.Find("GridList");             // Get grid of the list 

        StartCoroutine(GetAllProjects());                   // Start script to find projects on databse
    }

    void Update()
    {
        
    }

    public void GoBackToMenu()
    {
        DontDestroyOnLoad(CONST);                                               // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);   //Go back to Home Scene
    }

    public void GoToViewProject(GameObject pItemSelected)
    {
        GameObject projectSelected = new GameObject();                                  // Create the project as a GameObject to pass in another scene
        projectSelected.name = "ProjectSelected";                                       // Change name of the GameObject to find it easely ine the hierarchy
        projectSelected.AddComponent<ProjectSelected>();                                // Assign Project script to the new GameObject
        projectSelected.GetComponent<ProjectSelected>().id = pItemSelected.GetComponent<ItemListProject>().id;      // Assign the ID for the next scene

        DontDestroyOnLoad(projectSelected);                                             // Pass the project selected to the next scene
        DontDestroyOnLoad(CONST);                                               // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);   //Go to Project Sheet Scene
    }

    private IEnumerator GetAllProjects()
    {
        UnityWebRequest request = UnityWebRequest.Get(CONST.GetComponent<CONST>().url + getProjectUrl);
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
                Debug.Log("*** Request Succeed :D ***");

                string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                Debug.Log(jsonResult);

                RequestGetAllProject entities = JsonUtility.FromJson<RequestGetAllProject>(jsonResult);
                Debug.Log("*** Entities: " + entities + " ***");

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

                    // Formating date to French time
                    string dateValueText = entity.date.ToString();
                    dateValueText = dateValueText.Remove(10, 14);
                    DateTime dateTimeText = Convert.ToDateTime(dateValueText);
                    dateValueText = dateTimeText.ToString("dd-MM-yyyy", CultureInfo.CreateSpecificCulture("fr-FR"));

                    // Change text value of the list item
                    dateValue.GetComponent<UnityEngine.UI.Text>().text = dateValueText;
                    refValue.GetComponent<UnityEngine.UI.Text>().text = entity.reference.ToString();
                    clientValue.GetComponent<UnityEngine.UI.Text>().text = entity.customer.ToString();
                    sellerValue.GetComponent<UnityEngine.UI.Text>().text = entity.user.matricule.ToString();

                    listItem.GetComponent<ItemListProject>().id = entity._id.ToString();
                }
            }
        }
    }

    private IEnumerator DeleteProject()
    {
        return null;
    }
}
