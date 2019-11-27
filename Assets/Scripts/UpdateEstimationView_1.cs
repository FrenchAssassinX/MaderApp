using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UpdateEstimationView_1 : MonoBehaviour
{
    public GameObject CONST;

    //Text conteners
    public GameObject customer;
    public GameObject projectName;
    public GameObject totalAfterDiscount;
    public GameObject totalBeforeDiscount;
    public GameObject discount;


    public GameObject listItemPrefab;                           // Prefab item to display all elements in component list
    public GameObject componentList;                   //panel wich will contain all the listItemPrefabs 

    //actualise button
    public GameObject actualiser;

    // Start is called before the first frame update
    void Start()
    {
        CONST = GameObject.Find("CONST");

        string totalBeforeDiscountSt = CONST.GetComponent<CONST>().estimationPrice;
        string discountSt = CONST.GetComponent<CONST>().estimationDiscount;
        int discountInt = Int32.Parse(discountSt);
        int totalBeforeDiscountInt = Int32.Parse(totalBeforeDiscountSt);

        int totalAfterDiscountInt = totalBeforeDiscountInt;
        if (discountInt != 0)
        {
            totalAfterDiscountInt = totalBeforeDiscountInt / discountInt;
        }

        totalBeforeDiscount.GetComponent<UnityEngine.UI.Text>().text = totalBeforeDiscountSt;
        discount.GetComponent<UnityEngine.UI.Text>().text = discountSt;
        totalAfterDiscount.GetComponent<UnityEngine.UI.Text>().text = totalAfterDiscountInt.ToString();

        customer.GetComponent<UnityEngine.UI.Text>().text = CONST.GetComponent<CONST>().customerName;
        projectName.GetComponent<UnityEngine.UI.Text>().text = CONST.GetComponent<CONST>().projectName;

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
            
            foreach(var moduleItem in estimation.module)
            {
                
                StartCoroutine(GetModules(moduleItem.id));
            }
        }
    }

    public IEnumerator GetModules(string moduleId)
    {
        Debug.Log("moduleItem : " + moduleId);

        var urlToGetModule = CONST.GetComponent<CONST>().url + "v1/getmodulebyid";

        WWWForm moduleForm = new WWWForm();                       // New form for web request
        moduleForm.AddField("moduleID", moduleId);    // Add to the form the value of the ID of the project to get

        UnityWebRequest requestForModule = UnityWebRequest.Post(urlToGetModule, moduleForm);     // Create new WebRequest
        requestForModule.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                      // Complete form with authentication datas
        requestForModule.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

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

            foreach (var componentItem in module.components)
            {
                Debug.Log("componentId : "+componentItem.id);
                StartCoroutine(GetComponent(componentItem.id, componentItem.qte));
            }
        }
    }

    public IEnumerator GetComponent(string componentId, string componentQte)
    {
        var urlToGetComponent = CONST.GetComponent<CONST>().url + "v1/getcomponentbyid";

        WWWForm componentForm = new WWWForm();                       // New form for web request
        componentForm.AddField("componentID", componentId);    // Add to the form the value of the ID of the project to get

        UnityWebRequest requestForComponent = UnityWebRequest.Post(urlToGetComponent, componentForm);     // Create new WebRequest
        requestForComponent.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                      // Complete form with authentication datas
        requestForComponent.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

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

            Debug.Log("name : " + component.name + " quantity : " + componentQte + " unit : " + component.unit + " price : " + component.price);
            // Create prefab
            GameObject listItem = Instantiate(listItemPrefab, Vector3.zero, Quaternion.identity);

            // Set estimationListPanel as parent of prefab in project hierarchy
            listItem.transform.SetParent(componentList.transform);

            // Find children in listItem to use them
            GameObject nameValue = GameObject.Find("nameText");
            GameObject quantityValue = GameObject.Find("quantitytext");
            GameObject unitValue = GameObject.Find("unitText");
            GameObject priceValue = GameObject.Find("priceText");

            // Customize props name of the prefab to find it when it will be create
            nameValue.name = nameValue.name + listItem.GetComponent<ItemListComponent>().name;
            quantityValue.name = quantityValue.name + listItem.GetComponent<ItemListComponent>().name;
            unitValue.name = unitValue.name + listItem.GetComponent<ItemListComponent>().name;
            priceValue.name = priceValue.name + listItem.GetComponent<ItemListComponent>().name;

            // Change text value of the list item
            nameValue.GetComponent<UnityEngine.UI.Text>().text = component.name;
            quantityValue.GetComponent<UnityEngine.UI.Text>().text = componentQte;
            unitValue.GetComponent<UnityEngine.UI.Text>().text = component.unit;
            priceValue.GetComponent<UnityEngine.UI.Text>().text = component.price;

            //// ID to keep for view components  or deleting estimation
            //listItem.GetComponent<ItemListComponent>().nameValue = component.name;
            //listItem.GetComponent<ItemListComponent>().quantityValue = component.quantity;
            //listItem.GetComponent<ItemListComponent>().unitValue = componentQte;
            //listItem.GetComponent<ItemListComponent>().priceValue = component.price;
        }
    }

    //Get back  button function
    public void BackPage()
    {
        DontDestroyOnLoad(CONST);                                                   // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);       //Send the previous scene (ProjectSheet)
    }

    //Get back  button function
    public void NextPage()
    {
        DontDestroyOnLoad(CONST);                                                   // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);       //Send the next scene (estimationView_2)
    }

}
