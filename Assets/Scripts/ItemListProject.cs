using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemListProject : MonoBehaviour
{
    public Button rowListItem;

    public GameObject buttonPDF;
    public GameObject buttonEdit;
    public GameObject buttonView;
    public GameObject buttonDelete;

    public bool isSelected;

    void Start()
    {
        rowListItem = GetComponent<Button>();
        rowListItem.onClick.AddListener(SelectItem);

        isSelected = false;

        buttonPDF = GameObject.Find("PdfButton");
        buttonEdit = GameObject.Find("EditButton");
        buttonView = GameObject.Find("ViewButton");
        buttonDelete = GameObject.Find("DeleteButton");

        buttonPDF.GetComponent<Button>().onClick.AddListener(GeneratePDF);
        buttonEdit.GetComponent<Button>().onClick.AddListener(EditProject);
        buttonView.GetComponent<Button>().onClick.AddListener(ViewProject);
        buttonDelete.GetComponent<Button>().onClick.AddListener(DeleteProject);
    }

    void Update()
    {

    }
    
    public void SelectItem()
    {
        isSelected = true;
    }

    public void GeneratePDF()
    {
        if (isSelected)
        {
            Debug.Log("PDF Created");
        }
    }

    public void EditProject()
    {
        if (isSelected)
        {
            Debug.Log("Edit project");
        }
    }

    public void ViewProject()
    {
        if (isSelected)
        {
            Debug.Log("View project");
        }
    }

    public void DeleteProject()
    {
        if (isSelected)
        {
            Debug.Log("Delete project");
        }
    }
}
