using System.Collections;
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
        Debug.Log("Create project click"); 
    }

    //consult projects button function
    public void ConsultProjects()
    {
        Debug.Log("Consult projects click");
    }

    //parameters button function
    public void Parameters()
    {
        Debug.Log("Parameters click");
    }
}
