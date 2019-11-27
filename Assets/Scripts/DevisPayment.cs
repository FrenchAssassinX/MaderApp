using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DevisPayment : MonoBehaviour
{
    public GameObject CONST;
    private string url;
    private string URLInvoiceProject = "";
    private string URLPayment = "";

    public Dropdown stateDevis;
    public Dropdown stateAdvancement;
    public Image barStateAdvancement;
    public Button ButtonSave;
    public Button ButtonReturn;
    // Start is called before the first frame update

    RectTransform rt;

    void Start()
    {
        CONST = GameObject.Find("CONST"); //Get the CONST gameObject

        url = CONST.GetComponent<CONST>().url;

        Button btnSV = ButtonSave.GetComponent<Button>();
        btnSV.onClick.AddListener(SaveAdvancement);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Poster all devis state
    void StateDevis()
    {
        List<string> dropdownStateDevis = new List<string>() {"Brouillon", "En attente", "Accepté", "Refusé", "En commande", "Transfert en facturation" /*temporaire*/};
        stateDevis.AddOptions(dropdownStateDevis);
    }

    //Poster all devis advancement
    void StateAdvancement()
    {
        List<string> dropdownStateAdvancement = new List<string>() {/*step*/};
        stateAdvancement.AddOptions(dropdownStateAdvancement);
    }


    //progressBar for Advancement Payment
    void ProgressBar()
    {
        
    }

    //update devis
    void SaveAdvancement()
    {

    }

    IEnumerator GetInvoiceProject()
    {
        WWWForm form = new WWWForm();

        UnityWebRequest request = UnityWebRequest.Get(CONST.GetComponent<CONST>().url + URLInvoiceProject);
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("*** ERROR: " + request.error + " ***");
        }
        else
        {
            if (request.isDone)
            {

            }
        }
    }


    IEnumerable UpdatePayment()
    {
        WWWForm form = new WWWForm();

        UnityWebRequest request = UnityWebRequest.Get(CONST.GetComponent<CONST>().url + URLPayment);
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("*** ERROR: " + request.error + " ***");
        }
        else
        {
            if (request.isDone)
            {

            }
        }
    }





}
