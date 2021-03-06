﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpdateEstimationView_2 : MonoBehaviour
{
    public GameObject CONST;

    public GameObject listItemPrefab;                           // Prefab item to display all elements in component list
    public GameObject componentList;                   //panel wich will contain all the listItemPrefabs 

    public GameObject frameQuality;           //range attributes game objects to show when a module is selected
    public GameObject insulating;
    public GameObject finishingext;
    public GameObject finishingint;
    public GameObject rangePanel;

    // Start is called before the first frame update
    void Start()
    {
        CONST = GameObject.Find("CONST");
        rangePanel.SetActive(false);
        StartCoroutine(GetEstimation()); //Start the search of the estimation datas
    }

    //Get the Estimation datas
    public IEnumerator GetEstimation()
    {
        var urlToGetEstimation = CONST.GetComponent<CONST>().url + "v1/getestimationbyid"; //http road to get the estimation by giving its Id

        WWWForm estimationForm = new WWWForm();                       // New form for web request
        estimationForm.AddField("estimationID", CONST.GetComponent<CONST>().selectedEstimationID);    // Add to the form the value of the ID of the estimation to get

        UnityWebRequest requestForEstimation = UnityWebRequest.Post(urlToGetEstimation, estimationForm);     // Create new WebRequest
        requestForEstimation.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");           // Complete form with authentication datas
        requestForEstimation.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        requestForEstimation.certificateHandler = new CONST.BypassCertificate();    // Bypass certificate for https

        yield return requestForEstimation.SendWebRequest(); //execute the web request

        if (requestForEstimation.isNetworkError || requestForEstimation.isHttpError)
        {
            Debug.Log(requestForEstimation.error);
        }
        else
        {
            string jsonResultFromEstimation = System.Text.Encoding.UTF8.GetString(requestForEstimation.downloadHandler.data);          // Get JSON file

            RequestAnEstimation estimationEntity = JsonUtility.FromJson<RequestAnEstimation>(jsonResultFromEstimation);         // Convert JSON file

            Estimation estimation = estimationEntity.estimation; //Create a serialized estimation object 

            foreach (var moduleItem in estimation.module) //bubkle into all the modules of the current estimation
            {
                StartCoroutine(GetModules(moduleItem.id)); //start the search of the current module datas
            }
        }
    }

    //get the modules 
    public IEnumerator GetModules(string moduleId)
    {

        var urlToGetModule = CONST.GetComponent<CONST>().url + "v1/getmodulebyid"; //http road to get the module datas by giving its ID

        WWWForm moduleForm = new WWWForm();           // New form for web request
        moduleForm.AddField("moduleID", moduleId);    // Add to the form the value of the ID of the project to get

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
            Module module = moduleEntity.module;
            
            if (module.type == "custom")
            {
                RangeAttribute rangeAttribute = module.rangeAttributes[0]; //Instanciate range attribute object

                // Create prefab
                GameObject listItem = Instantiate(listItemPrefab, Vector3.zero, Quaternion.identity);

                // Set estimationListPanel as parent of prefab in project hierarchy
                listItem.transform.SetParent(componentList.transform);

                listItem.GetComponent<RectTransform>().localScale = componentList.GetComponent<RectTransform>().localScale;
                listItem.GetComponent<RectTransform>().sizeDelta = new Vector2(componentList.GetComponent<RectTransform>().sizeDelta.x, listItem.GetComponent<RectTransform>().sizeDelta.y);

                // Find children in listItem to use them
                GameObject nameValue = GameObject.Find("moduleName");

                // Customize props name of the prefab to find it when it will be create
                nameValue.name = nameValue.name + listItem.GetComponent<ItemListModules>().name;

                // Change text value of the list item
                nameValue.GetComponent<UnityEngine.UI.Text>().text = module.name;

                nameValue.GetComponent<RectTransform>().localScale = listItem.GetComponent<RectTransform>().localScale;

                nameValue.GetComponent<RectTransform>().sizeDelta = listItem.GetComponent<RectTransform>().sizeDelta;

                // ID to keep for view range datas
                listItem.GetComponent<ItemListModules>().frameQualityValue = rangeAttribute.frameQuality;
                listItem.GetComponent<ItemListModules>().windowsFrameQualityValue = rangeAttribute.windowsframequality;
                listItem.GetComponent<ItemListModules>().insulatingValue = rangeAttribute.insulating;
                listItem.GetComponent<ItemListModules>().coveringValue = rangeAttribute.covering;
                listItem.GetComponent<ItemListModules>().intFinishingValue = rangeAttribute.finishingext;
                listItem.GetComponent<ItemListModules>().extFinishingValue = rangeAttribute.finishingint;
            }
        }
    }

    //show the range content 
    public void ShowRange(GameObject pItemSelected)
    {
        rangePanel.SetActive(false);
        frameQuality.GetComponent<UnityEngine.UI.Text>().text ="";
        insulating.GetComponent<UnityEngine.UI.Text>().text = "";
        finishingext.GetComponent<UnityEngine.UI.Text>().text = "";
        finishingint.GetComponent<UnityEngine.UI.Text>().text = "";
        rangePanel.SetActive(true);
        frameQuality.GetComponent<UnityEngine.UI.Text>().text = pItemSelected.GetComponent<ItemListModules>().frameQualityValue;
        insulating.GetComponent<UnityEngine.UI.Text>().text = pItemSelected.GetComponent<ItemListModules>().insulatingValue;
        finishingext.GetComponent<UnityEngine.UI.Text>().text = pItemSelected.GetComponent<ItemListModules>().intFinishingValue;
        finishingint.GetComponent<UnityEngine.UI.Text>().text = pItemSelected.GetComponent<ItemListModules>().extFinishingValue;
    }

    public void GoToHomePage()
    {
        DontDestroyOnLoad(CONST);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 7);
    }

    //Get back page button function
    public void BackPage()
    {
        DontDestroyOnLoad(CONST);                                                   // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);       //Send the previous scene (ProjectSheet)
    }

    //Get next page button function
    public void NextPage()
    {
        DontDestroyOnLoad(CONST);                                                   // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);       //Send the next scene (estimationView_2)
    }

}
