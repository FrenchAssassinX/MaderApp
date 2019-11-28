using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateModule : MonoBehaviour
{
    public GameObject CONST;
    private string url;
    private string URLCreateModule = "v1/createmodule";  // Specific url for create module
    private string URLEstimationModule = "v1/createmodulewithestimation"; // Specific url for estimation module
    private string URLRange = "v1/getallrange"; // Specific url for range
    private string URLgetAllModule = "v1/getAllModule"; // Specific url for get module
    private string URLGetRangeById = "/v1/getrangebyid"; // Specific url for Post range id

    //Type (CanvasLeft)
    public Dropdown ddrange; //gamme
    public Dropdown ddmodel; //modele
    public Dropdown ddcut; //coupe
    public Input name; //name of module
    public Button ButtonModificationModule;
    public Button ButtonCreateModule;

    //Modification Module (CanvasRight)
    public Canvas canvasModificationModule;
    public Dropdown woodenUpright; //montantBois;
    public Dropdown insulationPanels; //panneauxIsolation;
    public Dropdown rainBarrier; //parePluie;
    public Dropdown intermediatePanels; //panneauxIntermediaires;
    public Dropdown hatchPanels; //panneauxCouverture;
    public Dropdown floor; //plancher;

    // Banner (CanvasTop)
    public Button buttonReturn;
    public Button buttonNext;


    List<string> dropdownranges = new List<string>();
    List<string> dropdownModel = new List<string>();
    List<string> dropdownCut = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        CONST = GameObject.Find("CONST"); //Get the CONST gameObject

        url = CONST.GetComponent<CONST>().url;

        //it's not visible for now
        canvasModificationModule.transform.gameObject.SetActive(false);

        //Modification module
        Button btnMM = ButtonModificationModule.GetComponent<Button>();
        btnMM.onClick.AddListener(DisplayModificationModule);

        //Create module
        //Button btnCM = ButtonCreateModule.GetComponent<Button>();
        //btnMM.onClick.AddListener();

        //return in create project page
        Button btnHP = buttonReturn.GetComponent<Button>();
        btnHP.onClick.AddListener(ReturnCreateProjectPage);

        // Go to Create Estimation Scene
        Button btnNext = buttonNext.GetComponent<Button>();
        btnNext.onClick.AddListener(GoToCreateEstimationScene);

        StartCoroutine(GetAllRange());
        StartCoroutine(GetAllModule());

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Function to go to Create Estimation scene
    public void GoToCreateEstimationScene()
    {
        DontDestroyOnLoad(CONST);                                                   // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);       // Load the next scene
    }

    //return to Project scene
    public void ReturnCreateProjectPage()
    {
        //Send the previous scene (create project)
        DontDestroyOnLoad(CONST);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
    }

    public void GoToHomePage()
    {
        DontDestroyOnLoad(CONST);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 4);
    }

    public IEnumerator GetAllRange()
    {

        UnityWebRequest request = UnityWebRequest.Get(CONST.GetComponent<CONST>().url + URLRange);
        
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);


            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                // error
            }
            else
            {
                if (request.isDone)
                {
                    // The database return a JSON file of all user infos
                    string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                    // Create a root object thanks to the JSON file
                    RequestGetAllRange entities = JsonUtility.FromJson<RequestGetAllRange>(jsonResult);

                    foreach (var item in entities.range)
                    {
                    
                        string getId = item._id;
                        string getLibelle = item.libelle;

                    //Poster all ranges
                        dropdownranges.Add(getLibelle);
                        
                    }
                ddrange.options.Clear();
                ddrange.AddOptions(dropdownranges);
            }
                else
                {
                //erreur request
                }
            }
        
    }

    public IEnumerator GetAllModule()
    {
        UnityWebRequest request = UnityWebRequest.Get(CONST.GetComponent<CONST>().url + URLgetAllModule);

        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            // error
            Debug.Log("erreur de la requete : " + request);
        }
        else
        {
            if (request.isDone)
            {
                // The database return a JSON file of all user infos
                string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                // Create a root object thanks to the JSON file
                RequestGetAllModule entities = JsonUtility.FromJson<RequestGetAllModule>(jsonResult);
                //Debug.Log(jsonResult);
                foreach (var item in entities.modules)
                {
                    Module module = item;
                    string getIdModule = module._id;
                    string getName = module.name;
                    string getCost = module.cost;
                    string getAngle = module.angle;
                    string getCut = module.cut;
                    string getRange = module.range;
                    //Debug.Log("modules : " + getIdModule + "name : " + getName + "cost : " + getCost + "Angle : " +getAngle + "cut : " + getCut + "" + getRange);

                    //Poster all model
                    dropdownModel.Add(getName);
                                        
                    //Poster all model
                    dropdownCut.Add(getCut);
                    
                    foreach (var item2 in module.components)
                    {
                        string getIdComponents = item2.id;
                        string getQte = item2.qte;
                        Debug.Log("qte : " + getQte);
                        Debug.Log("components : " + getIdComponents + getQte);
                    }
                }
                ddmodel.options.Clear();
                ddmodel.AddOptions(dropdownModel);

                ddcut.options.Clear();
                ddcut.AddOptions(dropdownCut);
            }
        }

    }

    public IEnumerator PostGetRangeById()
    {

        WWWForm form = new WWWForm(); // New form for web request


        using (UnityWebRequest request = UnityWebRequest.Post(url + URLGetRangeById, form))
        {
            yield return request.SendWebRequest();

            // If connection failed
            if (request.isNetworkError || request.isHttpError)
            {
            }
            // If connection succeeded
            else
            {
                if (request.isDone)
                {
                }
            }
        }
    }

    //active CreateNewCient
    void DisplayModificationModule()
    {
        canvasModificationModule.transform.gameObject.SetActive(true);

    }

}