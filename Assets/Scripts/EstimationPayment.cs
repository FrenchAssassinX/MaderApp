using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EstimationPayment : MonoBehaviour
{
    public GameObject CONST;
    private string url;
    private string URLCreatePayment = "v1/createpayement"; //Post
    private string URLUpdateEstimation = "v1/updateestimationstate"; //Post

    public Dropdown stateEstimation;
    public Dropdown StatePayment;
    public Button buttonSave;
    public Button buttonReturn;
    public Slider sliderStatePayment; //progressBar for Advancement Payment
    public Text percentageText;

    List<Dropdown.OptionData> dropdownStateEstimation = new List<Dropdown.OptionData>();
    List<Dropdown.OptionData> dropdownStatePayment = new List<Dropdown.OptionData>();

    public Canvas canvasPayment;
    public string changeEstimation;
    public string changePayment;
    public string stepPayment;
    public string percentPayment;

    void Start()
    {
        CONST = GameObject.Find("CONST"); //Get the CONST gameObject

        url = CONST.GetComponent<CONST>().url;

        //Button save states
        Button btnSV = buttonSave.GetComponent<Button>();
        btnSV.onClick.AddListener(SaveAdvancement);

        //return home page
        Button btnRT = buttonReturn.GetComponent<Button>();
        btnRT.onClick.AddListener(Return);

        //State payment is not visible for now
        canvasPayment.transform.gameObject.SetActive(false);

        //For dropdown estimation
        stateEstimation.onValueChanged.AddListener(delegate
        {
            StateEstimationModif(stateEstimation);
        });

        //For dropdown payment
        StatePayment.onValueChanged.AddListener(delegate
        {
            StateAdvancementModif(StatePayment);
        });

        //percentageText = GetComponent<Text>();


        //StartCoroutine(UpdateEstimation());
    }

    public void Return()
    {
        //Send the previous scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    //Poster all devis state
    public void StateEstimationModif(Dropdown pChangeEstimation)
    {
        //Add option in dropdown state Estimation
        stateEstimation.AddOptions(dropdownStateEstimation);
        changeEstimation = pChangeEstimation.options[pChangeEstimation.value].text;
        Debug.Log("change estimation : " + changeEstimation);

        //if you select "Accepté" in the dropdown canvasPyament will appear
        if (changeEstimation == "Accepté")
        {
            canvasPayment.transform.gameObject.SetActive(true); //poster payment state
            //start CreatePayment if "Accepté" is selected

        }
        else
        {
            canvasPayment.transform.gameObject.SetActive(false); //hidden payment state

        }
    }    

    //Poster all estimation advancement
    //The Client select a state of payment and the slider folow.
    public void StateAdvancementModif( Dropdown pChangePayment)
    {
        //Add option in dropdown state payment
        StatePayment.AddOptions(dropdownStatePayment);
        changePayment = pChangePayment.options[pChangePayment.value].text;

        Debug.Log("change payment : " + changePayment);

        //max slider is 1, it's full position
        sliderStatePayment.maxValue = 1.0f;
        if (changePayment == "A la signature")
        {
            stepPayment = "1";
            percentPayment = "0.03";
            sliderStatePayment.value = 0.03f;
        }

        if (changePayment == "Obtension du permis de construire")
        {
            stepPayment = "2";
            percentPayment = "0.1";
            sliderStatePayment.value = 0.1f;
        }

        if (changePayment == "Ouverture du chantier")
        {
            stepPayment = "3";
            percentPayment = "0.15";
            sliderStatePayment.value = 0.15f;
        }

        if (changePayment == "Achèvement des fondations")
        {
            stepPayment = "4";
            percentPayment = "0.025";
            sliderStatePayment.value = 0.25f;
        }

        if (changePayment == "Achèvement des murs")
        {
            stepPayment = "5";
            percentPayment = "0.4";
            sliderStatePayment.value = 0.4f;
        }

        if (changePayment == "Mise hors d'eau/hors d'aire")
        {
            stepPayment = "6";
            percentPayment = "0.75";
            sliderStatePayment.value = 0.75f;
        }

        if (changePayment == "Achèvement des travaux d'équipement")
        {
            stepPayment = "7";
            percentPayment = "0.95";
            sliderStatePayment.value = 0.95f;
        }

        if(changePayment == "Remise des clés")
        {
            stepPayment = "8";
            percentPayment = "1.0";
            sliderStatePayment.value = 1.0f;
        }


        //Poster purcentage
        percentageText.text = Mathf.RoundToInt(sliderStatePayment.value * 100) + "%";
    }

    //update devis
    void SaveAdvancement()
    {
        StartCoroutine(CreatePayment());
    }

    IEnumerator CreatePayment()
    {
        WWWForm form = new WWWForm();
        form.AddField("step", stepPayment);
        form.AddField("percent", percentPayment);

        Debug.Log("step : " + stepPayment);
        Debug.Log("percent : " + percentPayment);

        using (UnityWebRequest request = UnityWebRequest.Post(url + URLCreatePayment, form))
        {
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

                    payement entities = JsonUtility.FromJson<payement>(jsonResult);         // Convert JSON file

                    Debug.Log("jsons result payment: " + jsonResult);
                }
            }
        }
    }


    IEnumerator UpdateEstimation()
    {
        WWWForm form = new WWWForm();

        UnityWebRequest request = UnityWebRequest.Get(CONST.GetComponent<CONST>().url + URLUpdateEstimation);
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
