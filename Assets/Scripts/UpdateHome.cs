﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpdateHome : MonoBehaviour
{
    public GameObject homeText; //Represents the title of the home scene
    public GameObject CONST; //Represents the const gameObject
    public string userName; //Represents the user name
    
    void Start()
    {
        CONST = GameObject.Find("CONST"); //Get the CONST gameObject
        userName = CONST.GetComponent<CONST>().userName; //get the username from the const gameObject
        homeText.GetComponent<UnityEngine.UI.Text>().text = "Bienvenue "+ userName.Substring(0, 1).ToUpper()+userName.Substring(1); //Set the home title to "Bienvenue + userName"
    }

    //Deconnexion button function
    public void Disconnect()
    {
        Destroy(CONST);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); //Send the previous scene (connectionScene)
    }

    //create project button function
    public void CreateProject()
    {
        // Keep the CONST gameObject between scenes
        DontDestroyOnLoad(CONST.transform);
        // Go to Home Scene
        CONST.GetComponent<CONST>().state = "create";
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //consult projects button function
    public void ConsultProjects()
    {
        DontDestroyOnLoad(CONST);
        CONST.GetComponent<CONST>().state = "consult";
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2); //Go to the Projects List Scene
    }

    //parameters button function
    public void Parameters()
    {
        Debug.Log("Parameters click");
    }

    public void GoToEstimationScene()
    {
        DontDestroyOnLoad(CONST);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 4);
    }
}
