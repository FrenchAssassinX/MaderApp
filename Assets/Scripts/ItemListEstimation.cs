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
    public string discountValue;

    /* All the buttons for make specific actions with the project */
    private GameObject buttonDelete;
    private GameObject buttonEstimation;
    private GameObject buttonPaymentTerms;
    private GameObject buttonTechnicalFile;
    private GameObject buttonShowEstimation;
    private GameObject buttonUpdateEstimation;

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
        buttonUpdateEstimation = GameObject.Find("updateButton");

        //Hide the previous buttons and affect specific onClick behaviours
        if (buttonDelete != null)
        {
            buttonDelete.SetActive(false);
            buttonDelete.GetComponent<Button>().onClick.AddListener(DeleteEstimation);
        }
        if (buttonEstimation != null)
        {
            buttonEstimation.SetActive(false);
            buttonEstimation.GetComponent<Button>().onClick.AddListener(GetEstimation);
        }
        if (buttonPaymentTerms != null)
        {
            buttonPaymentTerms.SetActive(false);
            buttonPaymentTerms.GetComponent<Button>().onClick.AddListener(GetPaymentTerms);
        }
        if (buttonTechnicalFile != null)
        {
            buttonTechnicalFile.SetActive(false);
            buttonTechnicalFile.GetComponent<Button>().onClick.AddListener(GetTechnicalFile);
        }
        if (buttonShowEstimation != null)
        {
            buttonShowEstimation.SetActive(false);
            buttonShowEstimation.GetComponent<Button>().onClick.AddListener(ShowEstimation);
        }
        if (buttonUpdateEstimation != null)
        {
            buttonUpdateEstimation.SetActive(false);
            buttonUpdateEstimation.GetComponent<Button>().onClick.AddListener(UpdateEstimation);
        }
        
    }

    /* Function for select item detection */
    public void SelectItem()
    {
        Debug.Log("selected : " + this.gameObject.GetComponent<ItemListEstimation>().idValue);

        if (buttonDelete != null)
        {
            buttonDelete.SetActive(true);
        }
        if (buttonEstimation != null)
        {
            buttonEstimation.SetActive(true);
        }
        if (buttonPaymentTerms != null)
        {
            buttonPaymentTerms.SetActive(true);
        }
        if (buttonTechnicalFile != null)
        {
            buttonTechnicalFile.SetActive(true);
        }
        if (buttonShowEstimation != null)
        {
            buttonShowEstimation.SetActive(true);
        }
        if (buttonUpdateEstimation != null)
        {
            buttonUpdateEstimation.SetActive(true);
        }
        isSelected = true;
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
        }
    }

    /* Function that gets estimation pdf*/
    public void GetEstimation()
    {
        if (isSelected)
        {
            GameObject.Find("Canvas").GetComponent<UpdateProjectSheet>().EditEstimation(this.gameObject);
        }
    }

    /* Function that gets payment terms*/
    public void GetPaymentTerms()
    {
        if (isSelected)
        {
            GameObject.Find("Canvas").GetComponent<UpdateProjectSheet>().ShowPaymentTerms(this.gameObject);
        }
    }

    /* Function that gets technical file window*/
    public void GetTechnicalFile()
    {
        if (isSelected)
        {
            GameObject.Find("Canvas").GetComponent<UpdateProjectSheet>().ShowTechnicalFolder(this.gameObject);
        }
    }

    /* Function that opens create estimation window*/
    public void UpdateEstimation()
    {
        if (isSelected)
        {
            GameObject.Find("Canvas").GetComponent<UpdateProjectSheet>().ShowCreateEstimation(this.gameObject);
        }
    }
}
