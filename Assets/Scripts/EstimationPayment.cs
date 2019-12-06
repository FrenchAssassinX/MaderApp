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
        btnRT.onClick.AddListener(ReturnToTechnicalFolder);

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

        StartCoroutine(UpdateEstimation());
    }

    //Poster all devis state
    public void StateEstimationModif(Dropdown pChangeEstimation)
    {
        //Add option in dropdown state Estimation
        stateEstimation.AddOptions(dropdownStateEstimation);
        //change state
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
        //change state
        changePayment = pChangePayment.options[pChangePayment.value].text;

        Debug.Log("change payment : " + changePayment);

        //max slider is 1, it's full position
        sliderStatePayment.maxValue = 1.0f;

        //the user can choose the state of progress which will advance the pregress bar
        if (changePayment == "A la signature")
        {
            //progress bar at 3%
            stepPayment = "1"; //number for step
            percentPayment = "3%"; //percent for advancement payment
            sliderStatePayment.value = 0.03f; //value to advance the progress bar
        }

        if (changePayment == "Obtension du permis de construire")
        {
            //progress bar at 10%
            stepPayment = "2"; //number for step
            percentPayment = "10%"; //percent for advancement payment
            sliderStatePayment.value = 0.1f; //value to advance the progress bar
        }

        if (changePayment == "Ouverture du chantier")
        {
            //progress bar at 15%
            stepPayment = "3"; //number for step
            percentPayment = "15%"; //percent for advancement payment
            sliderStatePayment.value = 0.15f; //value to advance the progress bar
        }

        if (changePayment == "Achèvement des fondations")
        {
            //progress bar at 25%
            stepPayment = "4"; //number for step
            percentPayment = "25%"; //percent for advancement payment
            sliderStatePayment.value = 0.25f; //value to advance the progress bar
        }

        if (changePayment == "Achèvement des murs")
        {
            //progress bar at 40%
            stepPayment = "5"; //number for step
            percentPayment = "40%"; //percent for advancement payment
            sliderStatePayment.value = 0.4f; //value to advance the progress bar
        }

        if (changePayment == "Mise hors d'eau/hors d'aire")
        {
            //progress bar at 75%
            stepPayment = "6"; //number for step
            percentPayment = "75%"; //percent for advancement payment
            sliderStatePayment.value = 0.75f; //value to advance the progress bar
        }

        if (changePayment == "Achèvement des travaux d'équipement")
        {
            //progress bar at 95%
            stepPayment = "7"; //number for step
            percentPayment = "95%"; //percent for advancement payment
            sliderStatePayment.value = 0.95f; //value to advance the progress bar
        }

        if(changePayment == "Remise des clés")
        {
            //progress bar at 100%
            stepPayment = "8"; //number for step
            percentPayment = "100%"; //percent for advancement payment
            sliderStatePayment.value = 1.0f; //value to advance the progress bar
        }


        //Poster purcentage
        percentageText.text = Mathf.RoundToInt(sliderStatePayment.value * 100) + "%";
    }

    //update devis
    public void SaveAdvancement()
    {
        StartCoroutine(CreatePayment());
    }

    // Function for create payment, it's send step and percent advancement payment
    public IEnumerator CreatePayment()
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

                    CreatePayement entities = JsonUtility.FromJson<CreatePayement>(jsonResult);         // Convert JSON file

                    Debug.Log("jsons result payment: " + jsonResult);
                }
            }
        }
    }


    public IEnumerator UpdateEstimation()
    {
        WWWForm form = new WWWForm();
        form.AddField("state", "");
        form.AddField("EstimationID", "");

        using (UnityWebRequest request = UnityWebRequest.Post(url + URLUpdateEstimation, form))
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

                    EstimationState entities = JsonUtility.FromJson<EstimationState>(jsonResult);         // Convert JSON file

                    Debug.Log("jsons result update project: " + jsonResult);
                }
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
