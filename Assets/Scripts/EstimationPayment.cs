﻿using System;
using System.Collections;
using System.Collections.Generic;
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
    private string URLGetPaymentById = "v1/getpayementbyid"; //Specific route to Post and get payment state by id
    private string URLGetEstimationById = "v1/getestimationbyid"; //Specific route to Post and get for get estimation id 
    private string URLGetProjectById = "v1/getprojectbyid"; //Specific route to Post and get for get project id

    public Dropdown stateEstimation; //Dropdown for stateEstimation
    public Dropdown statePayment; //Dropdown for statePayment
    public Button buttonSave; //Button for save 
    public Button buttonReturn; //Button for to TechnicalFolder scene
    public Button buttonHome; //Button for going to home
    public Slider sliderStatePayment; //ProgressBar for Advancement Payment
    public GameObject percText; //Game object for percentage text
    public int timer = 120; // Timer for message

    public List<string> dropdownStateEstimation; //List for dropdown Estimation
    public List<string> dropdownStatePayment; //List for dropdown Payment

    public Canvas canvasPayment; //Canvas for statePayment
    public string changeEstimation; //Define string for get change estimation
    public string changePayment; //Define string for get change payment
    public string changeStep; //Define string for get step
    public string changePercent; //Define string for get percent
    public string stepPayment; //Define string for step in payment
    public int stepPaymentInt; //Define int for step payment
    public string percentPayment; //Define string for percent in payment
    public GameObject textSavePayment; //valide message
    public GameObject errorTextSavePayment; //error message

    public string getStep; //get step in database
    public string getPercent; //get percent in database
    public string getPaymentStep; //get step in database
    public string getPaymentPercent; //get percent in database
    public string getStateEstimation; //get entities to GetEstimationBYId
    public RequestAProject requestAProject; //get entities to RequestAProject

    /* ------------------------------------     END DECLARE DATAS PART     ------------------------------------ */

    void Start()
    {

        CONST = GameObject.Find("CONST"); //Get the CONST gameObject
        string state = CONST.GetComponent<CONST>().state; //Get state in const
        url = CONST.GetComponent<CONST>().url; //Url get CONST

        percText = GameObject.Find("TextPercent");
        //Add value in dropdown estimation and payment
        DropdownList();

        //if choise a existing project
        if (state == "consult")
        {
            //get estimation by id
            StartCoroutine(BigCoroutine());
            //if payment is existant
            
        }
        //if choise a new project
        else
        {
            //StartCoroutine(CreatePayment());

        }


        //Button save states
        Button btnSV = buttonSave.GetComponent<Button>();
        btnSV.onClick.AddListener(SaveAdvancement);

        //Return To TechnicalFolder
        Button btnRT = buttonReturn.GetComponent<Button>();
        btnRT.onClick.AddListener(ReturnToTechnicalFolder);

        //Return to home
        Button btnRH = buttonHome.GetComponent<Button>();
        btnRH.onClick.AddListener(ReturnToHomeScene);

        //State payment is not visible for now
        canvasPayment.transform.gameObject.SetActive(false);

        //Succès message for save advancement payment
        textSavePayment.transform.gameObject.SetActive(false);

        //Error message for save advancement payment
        errorTextSavePayment.transform.gameObject.SetActive(false);

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

    }

    void Update()
    {
        //if text message succes for save payment is active wait 3s before disabled message
        if (textSavePayment.transform.gameObject.active)
        {
            if(timer > 0)
            {
                timer--;
            }
            else
            {
                textSavePayment.transform.gameObject.SetActive(false);
                timer = 120;
            }
        }

        //if text message error for save payment is active wait 3s before disabled message
        if (errorTextSavePayment.transform.gameObject.active)
        {
            if (timer > 0)
            {
                timer--;
            }
            else
            {
                errorTextSavePayment.transform.gameObject.SetActive(false);
                timer = 120;
            }
        }
    }

    //Options for dropdown
    public void DropdownList()
    {
        List<string> dropdownStateEstimation = new List<string> { "Brouillons", "Accepté" }; //Create dropdown for state estimation
        //var dropdown = GetComponent<Dropdown>();
        stateEstimation.options.Clear();
        foreach (string option in dropdownStateEstimation)
        {
            //Add option in dropdown state Estimation
            stateEstimation.options.Add(new Dropdown.OptionData(option));
        }
        List<string> dropdownStatePayment = new List<string> {"A la signature", "Obtension du permis de construire", "Ouverture du chantier", "Achèvement des fondations", "Achèvement des murs", "Mise hors d'eau/hors d'aire", "Achèvement des travaux d'équipement", "Remise des clés" }; //Create dropdown for state estimation
        statePayment.options.Clear();
        foreach (string option in dropdownStatePayment)
        {
            //Add option in dropdown state Payment
            statePayment.options.Add(new Dropdown.OptionData(option));
        }
    }

    //Show all estimation state
    public void StateEstimationModification(Dropdown pChangeEstimation)
    {
        //Change state
        changeEstimation = pChangeEstimation.options[pChangeEstimation.value].text;



        //If you select "Accepté" in the dropdown canvasPyament will appear
        if (changeEstimation == "Accepté")
        {
            canvasPayment.transform.gameObject.SetActive(true); //Show payment state
            //Start CreatePayment if "Accepté" is selected
            //if "Accepté" is selected dropdown state esimation is disabled
            stateEstimation.enabled = false;
            //And value to "A la signature" id per default
            //sliderStatePayment.value = 0.03f;

        }
        else
        {
            canvasPayment.transform.gameObject.SetActive(false); //Hidden payment state

        }
    }

    //Show all estimation advancement
    //Customer select a state of payment and the slider folow.
    public void StateAdvancementModif(Dropdown pChangePayment)
    {
        //Add option in dropdown state payment
        statePayment.AddOptions(dropdownStatePayment);
        //Change state
        changePayment = pChangePayment.options[pChangePayment.value].text;
        changePercent = percentPayment;
        changeStep = stepPayment;
       
        //Max slider is 1, it's full position
        sliderStatePayment.maxValue = 1.0f;

        switch (changePayment)
        {
            case "A la signature":
                {
                    stepPayment = "1"; //Number for step
                    stepPaymentInt = Int32.Parse(stepPayment);
                    percentPayment = "3%"; //Percent for advancement payment
                    sliderStatePayment.value = 0.03f; //Value to advance the progress bar 
                    percText.GetComponent<UnityEngine.UI.Text>().text = percentPayment; //Text percentage
                    break;

                }
            case "Obtension du permis de construire":
                {
                    stepPayment = "2"; //Number for step
                    stepPaymentInt = Int32.Parse(stepPayment);
                    percentPayment = "10%"; //Percent for advancement payment
                    sliderStatePayment.value = 0.1f; //Value to advance the progress bar  
                    percText.GetComponent<UnityEngine.UI.Text>().text = percentPayment; //Text percentage
                    break;

                }
            case "Ouverture du chantier":
                {
                    stepPayment = "3"; //Number for step
                    stepPaymentInt = Int32.Parse(stepPayment);
                    percentPayment = "15%"; //Percent for advancement payment
                    sliderStatePayment.value = 0.15f; //Value to advance the progress bar  
                    percText.GetComponent<UnityEngine.UI.Text>().text = percentPayment; //Text percentage
                    break;

                }
            case "Achèvement des fondations":
                {
                    stepPayment = "4"; //Number for step
                    stepPaymentInt = Int32.Parse(stepPayment);
                    percentPayment = "25%"; //Percent for advancement payment
                    sliderStatePayment.value = 0.25f; //Value to advance the progress bar 
                    percText.GetComponent<UnityEngine.UI.Text>().text = percentPayment; //Text percentage
                    break;
                }
            case "Achèvement des murs":
                {
                    stepPayment = "5"; //Number for step
                    stepPaymentInt = Int32.Parse(stepPayment);
                    percentPayment = "40%"; //Percent for advancement payment
                    sliderStatePayment.value = 0.40f; //Value to advance the progress bar  
                    percText.GetComponent<UnityEngine.UI.Text>().text = percentPayment; //Text percentage
                    break;
                }
            case "Mise hors d'eau/hors d'aire":
                {
                    //Progress bar at 75%
                    stepPayment = "6"; //Number for step
                    stepPaymentInt = Int32.Parse(stepPayment);
                    percentPayment = "75%"; //Percent for advancement payment
                    sliderStatePayment.value = 0.75f; //Value to advance the progress bar
                    percText.GetComponent<UnityEngine.UI.Text>().text = percentPayment; //Text percentage
                    break;
                }
            case "Achèvement des travaux d'équipement":
                {
                    //Progress bar at 95%
                    stepPayment = "7"; //Number for step
                    stepPaymentInt = Int32.Parse(stepPayment);
                    percentPayment = "95%"; //Percent for advancement payment
                    sliderStatePayment.value = 0.95f; //Value to advance the progress bar
                    percText.GetComponent<UnityEngine.UI.Text>().text = percentPayment; //Text percentage
                    break;
                }
            case "Remise des clés":
                {
                    //Progress bar at 100%
                    stepPayment = "8"; //Number for step
                    stepPaymentInt = Int32.Parse(stepPayment);
                    percentPayment = "100%"; //Percent for advancement payment
                    sliderStatePayment.value = 1.0f; //Value to advance the progress bar
                    percText.GetComponent<UnityEngine.UI.Text>().text = percentPayment; //Text percentage
                    break;
                }
        }
    }

    //Function for update estimation
    public void SaveAdvancement()
    {
            StartCoroutine(CreatePayment()); //start create payment
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
        form.AddField("step", stepPayment); //Add to the form the value of the UI Element 'stepPayment'
        form.AddField("percent", percentPayment); //Add to the form the value of the UI Element 'percentPayment'
        form.AddField("projectID", CONST.GetComponent<CONST>().selectedProjectID); //Add to the form the value of the selectedProjectID in CONST
        form.AddField("estimationID", CONST.GetComponent<CONST>().selectedEstimationID); //Add to the form the value of the selectedEstimationID in CONST

        // New webrequest with: CONST url, local url and the form
        using (UnityWebRequest request = UnityWebRequest.Post(url + URLCreatePayment, form)) //Create new form
        {
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded"); //Complete form with authentication datas
            request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token); //Token

            request.certificateHandler = new CONST.BypassCertificate();    // Bypass certificate for https

            yield return request.SendWebRequest(); // Send request

            //If connection failed
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log("*** ERROR CREATE: " + request.error + " ***");
            }
            //If connection succeeded
            else
            {
                if (request.isDone)
                {
                    //The database return a JSON file of all user infos
                    string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data); //Get JSON file
                    //Create a create payment thanks to the JSON file
                    //Get id payment for convert in payment Id for update payment
                    RequestCreatePayment entity = JsonUtility.FromJson<RequestCreatePayment>(jsonResult); //Convert JSON file
                    // Get step & percentage
                    CreatePayement payement = entity.payement;
                    getPaymentStep = payement.step;
                    getPaymentPercent = payement.percentage;

                    textSavePayment.transform.gameObject.SetActive(true);

                }
            }
        }
    }

    //Function for update payment, it recovers step and percent advancement payment
    public IEnumerator GetPaymentByID()
    {
        WWWForm form = new WWWForm(); //New form for web request
        form.AddField("payementID", requestAProject.result.payement[requestAProject.result.payement.Count - 1].id); //Add to the form the value of the paymentID in CONST


        // New webrequest with: CONST url, local url and the form
        using (UnityWebRequest request = UnityWebRequest.Post(url + URLGetPaymentById, form))
        {
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded"); //Complete form with authentication datas
            request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token); //Token

            request.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

            yield return request.SendWebRequest(); //Send request

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
                    string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data); //Get JSON file

                    GetPaymentById entities = JsonUtility.FromJson<GetPaymentById>(jsonResult); //Convert JSON file

                    //Get step and percent in this customer for payment

                    getStep = entities.payement.step;
                    getPercent = entities.payement.percentage;
                   
                }
            }
        }
    }

    //Function for get estimation by id
    public IEnumerator GetEstimationByID()
    {
        WWWForm form = new WWWForm(); //New form for web request
        form.AddField("estimationID", CONST.GetComponent<CONST>().selectedEstimationID);//New form for web request
        // New webrequest with: CONST url, local url and the form
        using (UnityWebRequest request = UnityWebRequest.Post(url + URLGetEstimationById, form))
        {
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded"); //Complete form with authentication datas
            request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token); //Token

            request.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

            yield return request.SendWebRequest(); //Send request

            //If connection failed
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log("*** ERROR ESTIMATION: " + request.error + " ***");
            }
            //If connection succeeded
            else
            {
                string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data); //Get JSON file

                RequestAnEstimation entities = JsonUtility.FromJson<RequestAnEstimation>(jsonResult); //Convert JSON file
                Estimation estimation = entities.estimation; //Call estimation in Estimation
                getStateEstimation = estimation.state; //Get state in getStateEstimation

                 if (getStateEstimation == "1") //if estimation is "Accepté"
                {
                    //Get payement by id
                    int stepLevel = Int32.Parse(getStep); //Parse string step to int
                    //Depending on the step, you provision your dropdown only with the same step and all those above
                    List<string> stateList = new List<string> {"A la signature", "Obtension du permis de construire", "Ouverture du chantier", "Achèvement des fondations", "Achèvement des murs", "Mise hors d'eau/hors d'aire", "Achèvement des travaux d'équipement", "Remise des clés" };
                    statePayment.options.Clear(); //Clear dropdown payment 

                    //Loop for no regression in payment, if you choose a state of payment the elements of advent will disappear
                    for (int i = 0; i < stateList.Count; i++)
                    {
                        if ((i + 1) >= stepLevel)
                        {
                            statePayment.options.Add(new Dropdown.OptionData(stateList[i])); //Add new list in dropdown payment
                        }
                    }
                }
                //if no payment exist
                else
                {
                    SaveAdvancement();
                    //StartCoroutine(CreatePayment()); //Start create payment
                    Debug.Log("Estimation state isn't equals to 1");

                }

            }
        }
    }

    //Function for get project by id
    public IEnumerator GetProjetByID()
    {
        WWWForm form = new WWWForm(); //New form for web request
        form.AddField("projectID", CONST.GetComponent<CONST>().selectedProjectID);//New form for web request
        // New webrequest with: CONST url, local url and the form
        using (UnityWebRequest request = UnityWebRequest.Post(url + URLGetProjectById, form))
        {
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded"); //Complete form with authentication datas
            request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token); //Token

            request.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

            yield return request.SendWebRequest(); //Send request

            //If connection failed
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log("*** ERROR PROJECT: " + request.error + " ***");
            }
            //If connection succeeded
            else
            {
                string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data); //Get JSON file

                RequestAProject entities = JsonUtility.FromJson<RequestAProject>(jsonResult);

                //Get jsonresult in RequestAProject 
                requestAProject = entities;
            }
        }
    }

    //Function with get jsonresult RequestAProject and jsonresult RequestAnEstimation
    public IEnumerator BigCoroutine()
    {
        WWWForm form = new WWWForm(); //New form for web request
        form.AddField("projectID", CONST.GetComponent<CONST>().selectedProjectID);//New form for web request
        // New webrequest with: CONST url, local url and the form
        using (UnityWebRequest request = UnityWebRequest.Post(url + URLGetProjectById, form))
        {
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded"); //Complete form with authentication datas
            request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token); //Token

            request.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

            yield return request.SendWebRequest(); //Send request
             
            //If connection failed
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log("*** ERROR PROJECT: " + request.error + " ***");
            }
            //If connection succeeded
            else
            {
                string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data); //Get JSON file

                RequestAProject entities = JsonUtility.FromJson<RequestAProject>(jsonResult);
                //Get jsonresult in requestAProject
                requestAProject = entities;
                Project project = entities.result;
                List<Payement> payments = project.payement;
                int countPayment = 0;
                if(payments.Count >= 1)
                {
                    countPayment = payments.Count - 1;


                    Payement payment = payments[countPayment];

                    form = new WWWForm(); //New form for web request
                    form.AddField("payementID", payment.id); //Add to the form the value of the paymentID in CONST

                    // New webrequest with: CONST url, local url and the form
                    using (UnityWebRequest request2 = UnityWebRequest.Post(url + URLGetPaymentById, form))
                    {
                        request2.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded"); //Complete form with authentication datas
                        request2.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token); //Token

                        request2.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

                        yield return request2.SendWebRequest(); //Send request

                        //If connection failed
                        if (request2.isNetworkError || request2.isHttpError)
                        {
                            Debug.Log("*** ERROR: " + request2.error + " ***");
                        }

                        //If connection succeeded
                        else
                        {
                            if (request2.isDone)
                            {
                                string jsonResult2 = System.Text.Encoding.UTF8.GetString(request2.downloadHandler.data); //Get JSON file

                                GetPaymentById entities2 = JsonUtility.FromJson<GetPaymentById>(jsonResult2); //Convert JSON file
                                
                                //Get step and percent in this customer for payment
                                getStep = entities2.payement.step;
                                getPercent = entities2.payement.percentage;
                                
                                form = new WWWForm(); //New form for web request
                                form.AddField("estimationID", CONST.GetComponent<CONST>().selectedEstimationID);
                                using (UnityWebRequest request3 = UnityWebRequest.Post(url + URLGetEstimationById, form))
                                {
                                    request3.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded"); //Complete form with authentication datas
                                    request3.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token); //Token

                                    request3.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

                                    yield return request3.SendWebRequest(); //Send request

                                    //If connection failed
                                    if (request3.isNetworkError || request3.isHttpError)
                                    {
                                        Debug.Log("*** ERROR ESTIMATION: " + request3.error + " ***");
                                    }
                                    //If connection succeeded
                                    else
                                    {
                                        string jsonResult3 = System.Text.Encoding.UTF8.GetString(request3.downloadHandler.data); //Get JSON file

                                        RequestAnEstimation entities3 = JsonUtility.FromJson<RequestAnEstimation>(jsonResult3); //Convert JSON file
                                        Estimation estimation = entities3.estimation;
                                        //Get state of estimation
                                        getStateEstimation = estimation.state;
                                        if (getStateEstimation == "1")
                                        {
                                            //get project by id
                                            stateEstimation.value = 1;
                                            //Get last payment to list
                                            int stepLevel = Int32.Parse(getStep);
                                            //Depending on the step, you provision your dropdown only with the same step and all those above
                                            List<string> stateList = new List<string> { "A la signature", "Obtension du permis de construire", "Ouverture du chantier", "Achèvement des fondations", "Achèvement des murs", "Mise hors d'eau/hors d'aire", "Achèvement des travaux d'équipement", "Remise des clés" };
                                            statePayment.options.Clear();// Clear dropdown payment
                                            for (int i = 0; i < stateList.Count; i++)
                                            {
                                                if ((i + 1) >= stepLevel)
                                                {
                                                    statePayment.options.Add(new Dropdown.OptionData(stateList[i]));
                                                }
                                            }
                                            statePayment.value = 0; //start dropdown with "A la signature"
                                            StateAdvancementModif(statePayment); //Poster in dropdown payment
                                        }
                                        //if no payment
                                        else
                                        {
                                            //StartCoroutine(CreatePayment());
                                            Debug.Log("Estimation state isn't equals to 1");
                                        }

                                    }
                                }
                            }
                        }
                    }
                }

                
            }
        }
    }
}