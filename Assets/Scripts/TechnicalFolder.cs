using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TechnicalFolder : MonoBehaviour
{
    public GameObject CONST;                                    // CONST object contains server route, token and user infos

    public GameObject modulesListItemPrefab;         // Prefab item to display all elements in module list
    public GameObject modulesList;                   //panel wich will contain all the listItemPrefabs 

    public GameObject componentListItemPrefab;         // Prefab item to display all elements in component list
    public GameObject componentsList;                   //panel wich will contain all the listItemPrefabs 

    void Start()
    {
        CONST = GameObject.Find("CONST");

        StartCoroutine(GetEstimation());
    }

    public IEnumerator GetEstimation()
    {
        var urlToGetEstimation = CONST.GetComponent<CONST>().url + "v1/getestimationbyid";

        WWWForm estimationForm = new WWWForm();                       // New form for web request
        estimationForm.AddField("estimationID", CONST.GetComponent<CONST>().selectedEstimationID);    // Add to the form the value of the ID of the project to get

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

            foreach (var moduleItem in estimation.module)
            {

                StartCoroutine(GetModules(moduleItem.id));
            }
        }
    }

    public IEnumerator GetModules(string moduleId)
    {

        var urlToGetModule = CONST.GetComponent<CONST>().url + "v1/getmodulebyid";

        WWWForm moduleForm = new WWWForm();           // New form for web request
        moduleForm.AddField("moduleID", moduleId);    // Add to the form the value of the ID of the project to get

        UnityWebRequest requestForModule = UnityWebRequest.Post(urlToGetModule, moduleForm);     // Create new WebRequest
        requestForModule.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                      // Complete form with authentication datas
        requestForModule.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        requestForModule.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

        yield return requestForModule.SendWebRequest();

        if (requestForModule.isNetworkError || requestForModule.isHttpError)
        {
            Debug.Log(requestForModule.error);
        }
        else
        {
            string jsonResultFromModule = System.Text.Encoding.UTF8.GetString(requestForModule.downloadHandler.data);          // Get JSON file

            RequestAModule moduleEntity = JsonUtility.FromJson<RequestAModule>(jsonResultFromModule);         // Convert JSON file
            Module module = moduleEntity.module;

            // Create prefab
            GameObject listItem = Instantiate(modulesListItemPrefab, Vector3.zero, Quaternion.identity);

            // Set estimationListPanel as parent of prefab in project hierarchy
            listItem.transform.SetParent(modulesList.transform);

            listItem.GetComponent<RectTransform>().localScale = modulesList.GetComponent<RectTransform>().localScale;
            listItem.GetComponent<RectTransform>().sizeDelta = new Vector2(modulesList.GetComponent<RectTransform>().sizeDelta.x, listItem.GetComponent<RectTransform>().sizeDelta.y);

            // Find children in listItem to use them
            GameObject nameValue = GameObject.Find("nameText");

            // Customize props name of the prefab to find it when it will be create
            nameValue.name = nameValue.name + listItem.GetComponent<ItemListModulesForTech>().name;

            // Change text value of the list item
            nameValue.GetComponent<UnityEngine.UI.Text>().text = module.name;

            nameValue.GetComponent<RectTransform>().localScale = listItem.GetComponent<RectTransform>().localScale;

            nameValue.GetComponent<RectTransform>().sizeDelta = listItem.GetComponent<RectTransform>().sizeDelta;

            // ID to keep for buckel in the module components 
            listItem.GetComponent<ItemListModulesForTech>().idValue = module._id;

        }
    }

    //function called when a module is selected
    //call a function that get the module object before showing its components
    public void UpdateComponents(GameObject pItemSelected)
    {
        string moduleId = pItemSelected.GetComponent<ItemListModulesForTech>().idValue;
        StartCoroutine(GetModulesForGettingComponents(moduleId));
    }

    //get the module object before showing its components
    public IEnumerator GetModulesForGettingComponents(string selectedID)
    {
        var urlToGetModule = CONST.GetComponent<CONST>().url + "v1/getmodulebyid"; //http road to get the module datas by giving its ID

        WWWForm moduleForm = new WWWForm();                       // New form for web request
        moduleForm.AddField("moduleID", selectedID);    // Add to the form the value of the ID of the module to get

        UnityWebRequest requestForModule = UnityWebRequest.Post(urlToGetModule, moduleForm);     // Create new WebRequest
        requestForModule.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                      // Complete form with authentication datas
        requestForModule.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        requestForModule.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

        yield return requestForModule.SendWebRequest(); //execute the web request

        if (requestForModule.isNetworkError || requestForModule.isHttpError)
        {
            Debug.Log(requestForModule.error);
        }
        else
        {
            string jsonResultFromModule = System.Text.Encoding.UTF8.GetString(requestForModule.downloadHandler.data);          // Get JSON file

            RequestAModule moduleEntity = JsonUtility.FromJson<RequestAModule>(jsonResultFromModule);         // Convert JSON file
            Module module = moduleEntity.module; //create a serealized module object 

            foreach (var componentItem in module.components)//bubkle into all the components of the current module
            {
                StartCoroutine(GetComponent(componentItem.id, componentItem.qte));//start the search of the current component datas, sending its id, and its quantity
            }
        }
    }

    //get the component datas and show them into the grid list
    public IEnumerator GetComponent(string componentId, string componentQte)
    {
        var urlToGetComponent = CONST.GetComponent<CONST>().url + "v1/getcomponentbyid";

        WWWForm componentForm = new WWWForm();                       // New form for web request
        componentForm.AddField("componentID", componentId);    // Add to the form the value of the ID of the project to get

        UnityWebRequest requestForComponent = UnityWebRequest.Post(urlToGetComponent, componentForm);     // Create new WebRequest
        requestForComponent.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                      // Complete form with authentication datas
        requestForComponent.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        requestForComponent.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

        yield return requestForComponent.SendWebRequest();

        if (requestForComponent.isNetworkError || requestForComponent.isHttpError)
        {
            Debug.Log(requestForComponent.error);
        }
        else
        {
            string jsonResultFromComponent = System.Text.Encoding.UTF8.GetString(requestForComponent.downloadHandler.data);          // Get JSON file

            RequestAComponent componentEntity = JsonUtility.FromJson<RequestAComponent>(jsonResultFromComponent);         // Convert JSON file

            ComponentToShow component = componentEntity.component;

            // Create prefab
            GameObject listItem = Instantiate(componentListItemPrefab, Vector3.zero, Quaternion.identity);

            // Set estimationListPanel as parent of prefab in project hierarchy
            listItem.transform.SetParent(componentsList.transform);

            listItem.GetComponent<RectTransform>().localScale = modulesList.GetComponent<RectTransform>().localScale;
            listItem.GetComponent<RectTransform>().sizeDelta = new Vector2(modulesList.GetComponent<RectTransform>().sizeDelta.x, listItem.GetComponent<RectTransform>().sizeDelta.y);

            // Find children in listItem to use them
            GameObject nameValue = GameObject.Find("nameText");
            GameObject quantityValue = GameObject.Find("quantityText");

            // Customize props name of the prefab to find it when it will be create
            nameValue.name = nameValue.name + listItem.GetComponent<ItemListComponentForTech>().name;
            quantityValue.name = quantityValue.name + listItem.GetComponent<ItemListComponentForTech>().name;

            nameValue.GetComponent<RectTransform>().localScale = listItem.GetComponent<RectTransform>().localScale;
            quantityValue.GetComponent<RectTransform>().localScale = listItem.GetComponent<RectTransform>().localScale;

            nameValue.GetComponent<RectTransform>().sizeDelta = listItem.GetComponent<RectTransform>().sizeDelta;
            quantityValue.GetComponent<RectTransform>().sizeDelta = listItem.GetComponent<RectTransform>().sizeDelta;

            // Change text value of the list item
            nameValue.GetComponent<UnityEngine.UI.Text>().text = component.name;
            quantityValue.GetComponent<UnityEngine.UI.Text>().text = componentQte;

        }
    }

    public void GoToHomePage()
    {
        DontDestroyOnLoad(CONST);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 8);
    }

    // Function to Go to EstimationView scene
    public void GoToEstimationModality()
    {
        DontDestroyOnLoad(CONST);                                                   // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);       // Load Estimation Modality scene
    }

    // Function to return to EstimationView_2 scene
    public void ReturnToEstimationView_2()
    {
        DontDestroyOnLoad(CONST);                                                   // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);       // Load EstimationView_2 scene
    }

}
