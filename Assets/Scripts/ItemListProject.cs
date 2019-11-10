using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemListProject : MonoBehaviour
{
    public Button rowListItem;                              // The item in the list to manipulate (this object)
    public bool isSelected;                                 // Boolean to detect when the item is select on the list
    public string id;                                // Variable to pass the project on the next scene and find it by ID

    /* All the buttons for make specific actions with the project */
    public GameObject buttonPDF;                            
    public GameObject buttonEdit;
    public GameObject buttonView;
    public GameObject buttonDelete;

    void Start()
    {
        rowListItem = GetComponent<Button>();               // Retrieve the item in the list
        rowListItem.onClick.AddListener(SelectItem);        // Affect specific onClick behaviour to the button
        isSelected = false;                                 // Initialize the boolean to false

        /* Retrieve the buttons in the scene */
        buttonPDF = GameObject.Find("PdfButton");
        buttonEdit = GameObject.Find("EditButton");
        buttonView = GameObject.Find("ViewButton");
        buttonDelete = GameObject.Find("DeleteButton");

        /* Affect specific onClick behaviours to the buttons */
        buttonPDF.GetComponent<Button>().onClick.AddListener(GeneratePDF);
        buttonEdit.GetComponent<Button>().onClick.AddListener(EditProject);
        buttonView.GetComponent<Button>().onClick.AddListener(ViewProject);
        buttonDelete.GetComponent<Button>().onClick.AddListener(DeleteProject);
    }

    void Update()
    {

    }
    
    /* Function for select item detection */
    public void SelectItem()
    {
        isSelected = true;
    }

    /* Function to generate a PDF file of the project */
    public void GeneratePDF()
    {
        if (isSelected)
        {
            Debug.Log("PDF Created");
        }
    }

    /* Function to edit the selected project */
    public void EditProject()
    {
        if (isSelected)
        {
            Debug.Log("Edit project");
        }
    }
    
    /* Function to view the selected project */
    public void ViewProject()
    {
        if (isSelected)
        {
            Debug.Log("View project");

            GameObject.Find("Panel").GetComponent<UpdateProjectList>().GoToViewProject(this.gameObject);   // Call function to change scene in UpdateListProject.cs
        }
    }

    /* Function to delete the selected project */
    public void DeleteProject()
    {
        if (isSelected)
        {
            Debug.Log("DeleteProject button pressed");
            GameObject.Find("Panel").GetComponent<UpdateProjectList>().DisplayDeleteProject(this.gameObject);             // Display Delete panel
        }
    }
}
