using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DevisPayment : MonoBehaviour
{
    public GameObject CONST;
    private string url;
    private string URLInvoiceProject = "v1/stateestimation";
    private string URLPayment = "v1/stateadvancement";

    public Dropdown stateEstimation;
    public Dropdown StatePayment;
    public Button buttonSave;
    public Button buttonReturn;
    public Slider sliderStatePayment;
    public Text percentageText;

    List<Dropdown.OptionData> dropdownStateEstimation = new List<Dropdown.OptionData>();
    List<Dropdown.OptionData> dropdownStatePayment = new List<Dropdown.OptionData>();

    public Canvas canvasPayment;
    public string changeEstimation;
    public string changePayment;

    void Start()
    {
        CONST = GameObject.Find("CONST"); //Get the CONST gameObject

        url = CONST.GetComponent<CONST>().url;

        Button btnSV = buttonSave.GetComponent<Button>();
        btnSV.onClick.AddListener(SaveAdvancement);

        //it's not visible for now
        canvasPayment.transform.gameObject.SetActive(false);

        //for dropdown estimation
        stateEstimation.onValueChanged.AddListener(delegate
        {
            StateEstimationModif(stateEstimation);
        });

        //for dropdown payment
        StatePayment.onValueChanged.AddListener(delegate
        {
            StateAdvancementModif(StatePayment);
        });

        //percentageText = GetComponent<Text>();

        StartCoroutine(GetInvoiceProject());
        StartCoroutine(UpdatePayment());
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Poster all devis state
    void StateEstimationModif(Dropdown pChangeEstimation)
    {
        stateEstimation.AddOptions(dropdownStateEstimation);
        changeEstimation = pChangeEstimation.options[pChangeEstimation.value].text;
        Debug.Log("change estimation : " + changeEstimation);

        if(changeEstimation == "Accepté")
        {
            canvasPayment.transform.gameObject.SetActive(true);

        }
        else
        {
            canvasPayment.transform.gameObject.SetActive(false);

        }
    }    

    //Poster all devis advancement
    //The Client select a state of payment and the slider folow.
    void StateAdvancementModif( Dropdown pChangePayment)
    {
        StatePayment.AddOptions(dropdownStatePayment);
        changePayment = pChangePayment.options[pChangePayment.value].text;
        Debug.Log("change payment : " + changePayment);
        sliderStatePayment.maxValue = 1.0f;

        if(changePayment == "A la signature")
        {
            sliderStatePayment.value = 0.03f;
        }

        if (changePayment == "Obtension du permis de construire")
        {
            sliderStatePayment.value = 0.1f;
        }

        if (changePayment == "Ouverture du chantier")
        {
            sliderStatePayment.value = 0.15f;
        }

        if (changePayment == "Achèvement des fondations")
        {
            sliderStatePayment.value = 0.25f;
        }

        if (changePayment == "Achèvement des murs")
        {
            sliderStatePayment.value = 0.4f;
        }

        if (changePayment == "Mise hors d'eau/hors d'aire")
        {
            sliderStatePayment.value = 0.75f;
        }

        if (changePayment == "Achèvement des travaux d'équipement")
        {
            sliderStatePayment.value = 0.95f;
        }

        if(changePayment == "Remise des clés")
        {
            sliderStatePayment.value = 1.0f;
        }

        //percentageText.text = Mathf.RoundToInt(sliderStatePayment.value * 100) + "%";
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
                string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);          // Get JSON file

                InvoiceProject entities = JsonUtility.FromJson<InvoiceProject>(jsonResult);         // Convert JSON file

                Debug.Log("jsons result invoice project: " + jsonResult);
            }
        }
    }


    IEnumerator UpdatePayment()
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
                string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);          // Get JSON file

                StateUpdatePayment entities = JsonUtility.FromJson<StateUpdatePayment>(jsonResult);         // Convert JSON file

                Debug.Log("jsons result update project: " + jsonResult);
            }
        }
    }

    // Function to return to Home scene
    public void ReturnToHomeScene()
    {
        DontDestroyOnLoad(CONST);                                                   // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 9);       // Load Home scene
    }

    // Function to return to TechnicalFolder scene
    public void ReturnToTechnicalFolder()
    {
        DontDestroyOnLoad(CONST);                                                   // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);       // Load TechnicalFolder scene
    }
}
