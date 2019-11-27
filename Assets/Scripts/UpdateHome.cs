//using PdfSharp.Drawing;
//using PdfSharp.Pdf;
using sharpPDF;
using sharpPDF.Enumerators;
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
        // Keep the CONST gameObject between scenes
        DontDestroyOnLoad(CONST.transform);
        // Go to Home Scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Create project click"); 
    }

    //consult projects button function
    public void ConsultProjects()
    {
        DontDestroyOnLoad(CONST);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2); //Go to the Projects List Scene
    }

    //parameters button function
    public void Parameters()
    {
        Debug.Log("Parameters click");
        string osRunning = SystemInfo.operatingSystem;
        string[] osTab = osRunning.Split(' ');
        string OS = osTab[0];

        if (OS.Equals("Windows"))
        {
            string attachName = "hello.pdf";
            Debug.Log(OS);
            pdfDocument myDoc = new pdfDocument("Sample Application", "Me", false);
            pdfPage myFirstPage = myDoc.addPage();
            myFirstPage.addText("hello world", 10, 730, predefinedFont.csHelveticaOblique, 30, new pdfColor(predefinedColor.csBlack));
            /*Set Header's Style*/
            myDoc.createPDF(@"C:\Users\Public\"+attachName);
        }
        else if (OS.Equals("Mac"))
        {
            string attachName = "hello.pdf";
            Debug.Log(OS);
            pdfDocument myDoc = new pdfDocument("Sample Application", "Me", false);
            pdfPage myFirstPage = myDoc.addPage();
            myFirstPage.addText("hello world", 10, 730, predefinedFont.csHelveticaOblique, 30, new pdfColor(predefinedColor.csBlack));
            /*Set Header's Style*/
            myDoc.createPDF(@"\Users\Shared\" + attachName);
        }
        else if (OS.Equals("Android"))
        {
            string attachName = "hello.pdf";
            Debug.Log(OS);
            pdfDocument myDoc = new pdfDocument("Sample Application", "Me", false);
            pdfPage myFirstPage = myDoc.addPage();
            myFirstPage.addText("hello world", 10, 730, predefinedFont.csHelveticaOblique, 30, new pdfColor(predefinedColor.csBlack));
            /*Set Header's Style*/
            myDoc.createPDF(attachName);
        }
    }

    public void GoToEstimationScene()
    {
        DontDestroyOnLoad(CONST);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 4);
    }
}
