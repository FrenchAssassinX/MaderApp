using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemListEstimation : MonoBehaviour
{
    public Button rowListItem;                              // The item in the list to manipulate (this object)
    public bool isSelected;                                 // Boolean to detect when the item is select on the list

    public string idValue;                                       // Variable to pass the project on the next scene and find it by ID
    public string priceValue;
    public string stateValue;
    public string dateValue;

    /* All the buttons for make specific actions with the project */
    public GameObject buttonDelete;
    public GameObject buttonEstimation;
    public GameObject buttonPaymentTerms;
    public GameObject buttonTechnicalFile;
    public GameObject buttonShowEstimation;


    // Start is called before the first frame update
    void Start()
    {
        rowListItem = GetComponent<Button>();               // Retrieve the item in the list
        rowListItem.onClick.AddListener(SelectItem);        // Affect specific onClick behaviour to the button
        isSelected = false;                                 // Initialize the boolean to false

        /* Retrieve the buttons in the scene */
        buttonDelete = GameObject.Find("deleteButton");
        buttonEstimation = GameObject.Find("estimationButton");
        buttonPaymentTerms = GameObject.Find("paymentTermsButton");
        buttonTechnicalFile = GameObject.Find("technicalFileButton");
        buttonShowEstimation = GameObject.Find("showEstimationButton");

        buttonDelete.SetActive(false);
        buttonEstimation.SetActive(false);
        buttonPaymentTerms.SetActive(false);
        buttonTechnicalFile.SetActive(false);
        buttonShowEstimation.SetActive(false);

        /* Affect specific onClick behaviours to the buttons */
        buttonDelete.GetComponent<Button>().onClick.AddListener(DeleteEstimation);
        buttonEstimation.GetComponent<Button>().onClick.AddListener(GetEstimation);
        buttonPaymentTerms.GetComponent<Button>().onClick.AddListener(GetPaymentTerms);
        buttonTechnicalFile.GetComponent<Button>().onClick.AddListener(GetTechnicalFile);
        buttonShowEstimation.GetComponent<Button>().onClick.AddListener(ShowEstimation);
    }

    /* Function for select item detection */
    public void SelectItem()
    {
        buttonDelete.SetActive(true);
        buttonEstimation.SetActive(true);
        buttonPaymentTerms.SetActive(true);
        buttonTechnicalFile.SetActive(true);
        buttonShowEstimation.SetActive(true);

        isSelected = true;
        Debug.Log("SELECTED");
    }

    /* Function to show an estimation */
    public void ShowEstimation()
    {
        if (isSelected)
        {
            GameObject.Find("Canvas").GetComponent<UpdateProjectSheet>().ShowEstimation(this.gameObject);
        }
    }

    /* Function to add an estimation */
    public void DeleteEstimation()
    {
        if (isSelected)
        {
            GameObject.Find("Canvas").GetComponent<UpdateProjectSheet>().openDeleteEstimation(this.gameObject);
            
            Debug.Log("Devis supprimé");
        }
    }

    /* Function that gets estimation pdf*/
    public void GetEstimation()
    {
        if (isSelected)
        {
            Debug.Log("PDF du devis ouvert");
        }
    }

    /* Function that gets payment terms*/
    public void GetPaymentTerms()
    {
        if (isSelected)
        {
            Debug.Log("Fenêtre de modalités de paiement ouverte");
        }
    }

    /* Function that gets technical file pdf*/
    public void GetTechnicalFile()
    {
        if (isSelected)
        {
            Debug.Log("Dossier technique ouvert");
        }
    }
}
