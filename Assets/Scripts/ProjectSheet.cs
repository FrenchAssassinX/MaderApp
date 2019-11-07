using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectSheet : MonoBehaviour
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

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
