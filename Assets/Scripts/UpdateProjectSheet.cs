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
    //project datas
    private string projectId;
    private string projectName;
    private string projectDate;
    private string projectSailorId;

    //client datas
    private string clientId;
    private string clientName;
    private string clientSurname;
    private string clientRoad;
    private string clientRoadNum;
    private string clientZipCode;
    private string clientCity;
    private string clientRoadExtra;
    private string clientPhone;
    private string clientEmail;

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

    public GameObject listItemPrefab;                           // Prefab item to display all elements in project list
    public GameObject estimationList;

    // Start is called before the first frame update
    void Start()
    {
        projectId = "testId";
        projectName = "testName";
        projectDate = "2019-10-16";
        projectSailorId ="marc du test";
        clientId = "mouton1Id";
        clientName = "mouton1";
        clientSurname = "chèvre";
        clientRoad = "rue de l'étable";
        clientRoadNum = "69";
        clientZipCode = "21000";
        clientCity = "farmville";
        clientRoadExtra ="";
        clientPhone ="0659263731";
        clientEmail = "bêêêêhh@bêmail.Com";

        //set the gameObjects content from the client and the project parameters 
        frameTitle.GetComponent<UnityEngine.UI.Text>().text =  "Projet "+projectId;
        projectIdGO.GetComponent<UnityEngine.UI.Text>().text = projectId;
        projectNameGO.GetComponent<UnityEngine.UI.Text>().text = projectName;
        projectDateGO.GetComponent<UnityEngine.UI.Text>().text = projectDate;
        projectSailorIdGO.GetComponent<UnityEngine.UI.Text>().text = projectSailorId;
        clientIdGO.GetComponent<UnityEngine.UI.Text>().text = clientId;
        clientNameGO.GetComponent<UnityEngine.UI.Text>().text = clientName;
        clientSurnameGO.GetComponent<UnityEngine.UI.Text>().text = clientSurname;
        clientRoadGO.GetComponent<UnityEngine.UI.Text>().text = clientRoad;
        clientRoadNumGO.GetComponent<UnityEngine.UI.Text>().text = clientRoadNum;
        clientZipCodeGO.GetComponent<UnityEngine.UI.Text>().text = clientZipCode;
        clientCityGO.GetComponent<UnityEngine.UI.Text>().text = clientCity;
        clientRoadExtraGO.GetComponent<UnityEngine.UI.Text>().text = clientRoadExtra;
        clientPhoneGO.GetComponent<UnityEngine.UI.Text>().text = clientPhone;
        clientEmailGO.GetComponent<UnityEngine.UI.Text>().text = clientEmail;

        estimationList = GameObject.Find("estimationListPanel");                 // Get grid of the list 

        GetAllEstimations();                       // Start script to find estimations on databse
    }

    private void GetAllEstimations()
    {
        string jsonResult = "{\"estimations\": [{\"id\" : \"1\", \"price\" : \"250 000 €\", \"state\" : \"En cours d\'édition\", \"date\": \"2019-11-11\"},{\"id\" : \"1\", \"price\" : \"250 000 €\", \"state\" : \"En cours d\'édition\", \"date\": \"2019-11-11\"},{\"id\" : \"1\", \"price\" : \"250 000 €\", \"state\" : \"En cours d\'édition\", \"date\": \"2019-11-11\"},{\"id\" : \"1\", \"price\" : \"250 000 €\", \"state\" : \"En cours d\'édition\", \"date\": \"2019-11-11\"}]}";

        EstimationList entities = JsonUtility.FromJson<EstimationList>(jsonResult);         // Convert JSON file

        // foreach to retrieve every estimations
        foreach (var item in entities.estimations)
        {
            Debug.Log("Test");
            // Create project with datas from database
            Estimation entity = item;

            // Create prefab
            GameObject listItem = Instantiate(listItemPrefab, estimationList.transform.position, Quaternion.identity);

            // Set estimationListPanel as parent of prefab in project hierarchy
            estimationList.transform.SetParent(estimationList.transform);

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

            // Change text value of the list item
            idValue.GetComponent<UnityEngine.UI.Text>().text = entity.id.ToString();
            priceValue.GetComponent<UnityEngine.UI.Text>().text = entity.price.ToString();
            stateValue.GetComponent<UnityEngine.UI.Text>().text = entity.state.ToString();
            dateValue.GetComponent<UnityEngine.UI.Text>().text = entity.date.ToString();
        }
    }

    
}
