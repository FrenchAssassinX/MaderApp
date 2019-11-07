using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class UpdateProjectList : MonoBehaviour
{
    public GameObject CONST;                                // CONST object contains server route, token and user infos

    private string getProjectUrl = "v1/getallproject";     // Specific route to get all projects

    public GameObject listItemPrefab;                       // Prefab item to display all elements in project list
    public GameObject gridList;                             // Grid to insert project prefab items

    //private List<Project> listProjects = new List<Project>();

    void Start()
    {
        CONST = GameObject.Find("CONST");                   // Get const object

        gridList = GameObject.Find("GridList");             // Get grid of the list 

        // TODO: Connection to bdd
        /*for (int i = 0; i < 10; i++)
        {
            // Place the prefab on the gridList
            GameObject listItem = Instantiate(listItemPrefab, gridList.transform.position, Quaternion.identity);
            // Set GridList as parent of prefab in project hierarchy
            listItem.transform.SetParent(gridList.transform);
        }*/

        StartCoroutine(GetAllProjects());
    }

    void Update()
    {
        
    }

    public void GoBackToMenu()
    {
        DontDestroyOnLoad(CONST);                                               // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);   //Go back to Home Scene
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
                Debug.Log("*** JsonResult: " + jsonResult + " ***");

                RequestGetAllProject entities = JsonUtility.FromJson<RequestGetAllProject>(jsonResult);
                Debug.Log("*** Entities: " + entities + " ***");

                foreach (var item in entities.projects)
                {
                    Project entity = item;
                    //listProjects.Add(entity);
                    GameObject listItem = Instantiate(listItemPrefab, gridList.transform.position, Quaternion.identity);
                    
                }
            }
        }
    }
}
