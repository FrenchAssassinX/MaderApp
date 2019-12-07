using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EstimationPayment : MonoBehaviour
{
    /* ------------------------------------     DECLARE DATAS PART     ------------------------------------ */
    public GameObject CONST; // CONST object contains server route, token and user infos
    private string url;
    private string URLCreatePayment = "v1/createpayement"; //Specific route to Post create payment
    private string URLUpdateEstimation = "v1/getpayementbyid"; //Specific route to Post to get payment state by id

    public Dropdown stateEstimation; //Dropdown for stateEstimation
    public Dropdown statePayment; //Dropdown for statePayment
    public Button buttonSave; //Button for save 
    public Button buttonReturn; //Button for to TechnicalFolder scene
    public Slider sliderStatePayment; //ProgressBar for Advancement Payment
    public Text percentageText; //Text for poster avencement in % for user

    List<Dropdown.OptionData> dropdownStateEstimation = new List<Dropdown.OptionData>(); //Create dropdown for state estimation
    List<Dropdown.OptionData> dropdownStatePayment = new List<Dropdown.OptionData>(); //Create dropdown for state payment

    public Canvas canvasPayment; //Canvas for statePayment
    public string changeEstimation; //Define string for take change estimation
    public string changePayment; //Define string for take change payment
    public string stepPayment; //Define string for step in payment
    public string percentPayment; //Define string for percent in payment

    /* ------------------------------------     END DECLARE DATAS PART     ------------------------------------ */

    void Start()
    {
        CONST = GameObject.Find("CONST"); //Get the CONST gameObject

        url = CONST.GetComponent<CONST>().url; //url take CONST

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
            StateEstimationModification(stateEstimation);
        });

        //For dropdown payment
        statePayment.onValueChanged.AddListener(delegate
        {
            StateAdvancementModif(statePayment);
        });



        StartCoroutine(UpdateEstimation());
    }

    //Poster all estimation state
    public void StateEstimationModification(Dropdown pChangeEstimation)
    {
        //Add option in dropdown state Estimation
        stateEstimation.AddOptions(dropdownStateEstimation);
        //Change state
        changeEstimation = pChangeEstimation.options[pChangeEstimation.value].text;
        Debug.Log("change estimation : " + changeEstimation);

        //If you select "Accepté" in the dropdown canvasPyament will appear
        if (changeEstimation == "Accepté")
        {
            canvasPayment.transform.gameObject.SetActive(true); //Poster payment state
            //Start CreatePayment if "Accepté" is selected

        }
        else
        {
            canvasPayment.transform.gameObject.SetActive(false); //Hidden payment state

        }
    }    

    //Poster all estimation advancement
    //The Client select a state of payment and the slider folow.
    public void StateAdvancementModif( Dropdown pChangePayment)
    {
        //Add option in dropdown state payment
        statePayment.AddOptions(dropdownStatePayment);
        //Change state
        changePayment = pChangePayment.options[pChangePayment.value].text;

        Debug.Log("change payment : " + changePayment);

        //Max slider is 1, it's full position
        sliderStatePayment.maxValue = 1.0f;

        //The user can choose the state of progress which will advance the pregress bar
        if (changePayment == "A la signature")
        {
            //Progress bar at 3%
            stepPayment = "1"; //Number for step
            percentPayment = "3%"; //Percent for advancement payment
            sliderStatePayment.value = 0.03f; //Value to advance the progress bar                                           

        }

        if (changePayment == "Obtension du permis de construire")
        {
            //Progress bar at 10%
            stepPayment = "2"; //Number for step
            percentPayment = "10%"; //Percent for advancement payment
            sliderStatePayment.value = 0.1f; //Value to advance the progress bar                                          

        }

        if (changePayment == "Ouverture du chantier")
        {
            //Progress bar at 15%
            stepPayment = "3"; //Number for step
            percentPayment = "15%"; //Percent for advancement payment
            sliderStatePayment.value = 0.15f; //Value to advance the progress bar                                              

        }

        if (changePayment == "Achèvement des fondations")
        {
            //Progress bar at 25%
            stepPayment = "4"; //Number for step
            percentPayment = "25%"; //Percent for advancement payment
            sliderStatePayment.value = 0.25f; //Value to advance the progress bar                                             

        }

        if (changePayment == "Achèvement des murs")
        {
            //Progress bar at 40%
            stepPayment = "5"; //Number for step
            percentPayment = "40%"; //Percent for advancement payment
            sliderStatePayment.value = 0.4f; //Value to advance the progress bar                    

        }

        if (changePayment == "Mise hors d'eau/hors d'aire")
        {
            //Progress bar at 75%
            stepPayment = "6"; //Number for step
            percentPayment = "75%"; //Percent for advancement payment
            sliderStatePayment.value = 0.75f; //Value to advance the progress bar                                            

        }

        if (changePayment == "Achèvement des travaux d'équipement")
        {
            //Progress bar at 95%
            stepPayment = "7"; //Number for step
            percentPayment = "95%"; //Percent for advancement payment
            sliderStatePayment.value = 0.95f; //Value to advance the progress bar

        }

        if (changePayment == "Remise des clés")
        {
            //Progress bar at 100%
            stepPayment = "8"; //Number for step
            percentPayment = "100%"; //Percent for advancement payment
            sliderStatePayment.value = 1.0f; //Value to advance the progress bar

        }


    }

    //Function for update estimation
    public void SaveAdvancement()
    {
        StartCoroutine(CreatePayment());
    }

    //Function to return to Home scene
    public void ReturnToHomeScene()
    {
        DontDestroyOnLoad(CONST);                                                   //Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 9);       //Load Home scene
    }

    //Function to return to TechnicalFolder scene
    public void ReturnToTechnicalFolder()
    {
        DontDestroyOnLoad(CONST);                                                   //Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);       //Load TechnicalFolder scene
    }

    //Function for create payment, it's send step and percent advancement payment
    public IEnumerator CreatePayment()
    {
        WWWForm form = new WWWForm(); //New form for web request
        form.AddField("step", stepPayment); //Add to the form the value of the UI Element 'stepPyament'
        form.AddField("percent", percentPayment); //Add to the form the value of the UI Element 'percentPyament'
        form.AddField("projectID", CONST.GetComponent<CONST>().selectedProjectID);

        Debug.Log("step : " + stepPayment);
        Debug.Log("percent : " + percentPayment);
        Debug.Log("const create payment: " + CONST.GetComponent<CONST>().selectedProjectID);

        /* New webrequest with: CONST url, local url and the form */
        using (UnityWebRequest request = UnityWebRequest.Post(url + URLCreatePayment, form)) //Create new form
        {
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded"); //Complete form with authentication datas
            request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token); //Token

            yield return request.SendWebRequest(); // Send request

            //If connection failed
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log("*** ERROR: " + request.error + " ***");
            }
            //If connection succeeded
            else
            {
                if (request.isDone)
                {
                    //The database return a JSON file of all user infos
                    string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);          //Get JSON file
                    //Create a create payment thanks to the JSON file
                    RequestCreatePayment entities = JsonUtility.FromJson<RequestCreatePayment>(jsonResult);         //Convert JSON file
                    Debug.Log("jsons result payment: " + jsonResult);
                    //Create ID for paymentID
                    Project project = entities.project;
                    CONST.GetComponent<CONST>().paymentID = project._id;
                }
            }
        }
    }

    //Function for update payment, it recovers step and percent advancement payment
    public IEnumerator UpdateEstimation()
    {
        WWWForm form = new WWWForm();
        form.AddField("payementID", CONST.GetComponent<CONST>().paymentID);
        Debug.Log("const update estimation : " + CONST.GetComponent<CONST>().paymentID);

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
                    string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data); //Get JSON file

                    GetPayment entities = JsonUtility.FromJson<GetPayment>(jsonResult); //Convert JSON file

                    Debug.Log("jsons result update project: " + jsonResult);
                }
            }
        }
    }
}
