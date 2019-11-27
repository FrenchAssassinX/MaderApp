using System;
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

    //Text conteners
    public GameObject customer;
    public GameObject projectName;
    public GameObject totalAfterDiscount;
    public GameObject totalBeforeDiscount;
    public GameObject discount;

    //actualise button
    public GameObject actualiser;

    // Start is called before the first frame update
    void Start()
    {
        CONST = GameObject.Find("CONST");


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
