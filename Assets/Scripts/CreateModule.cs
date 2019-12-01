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
    private string URLGetRangeById = "v1/getrangebyid"; // Specific url for Post range id
    private string URLGetAllComponent = "v1/getallcomponent";
    private string URLGetAllModuleModel = "v1/getallmodulemodel";

    //Type (CanvasLeft)
    public Dropdown ddrange; //gamme
    public Dropdown ddmodel; //modele
    public Input name; //name of module
    public Button ButtonModificationModule;
    public Button ButtonCreateModule;

    //Modification Module (CanvasRight)
    public Canvas canvasModificationModule;
    public Dropdown ddwoodenUpright; //montantBois;
    public Dropdown ddinsulationPanels; //panneauxIsolation;
    public Dropdown ddrainBarrier; //parePluie;
    public Dropdown ddintermediatePanels; //panneauxIntermediaires;
    public Dropdown ddhatchPanels; //panneauxCouverture;
    public Dropdown ddfloor; //plancher;

    // Banner (CanvasTop)
    public Button buttonReturn;
    public Button buttonNext;
    string getIdRange;

    //Define Dropdown Right value
    List<string> dropdownWoodenUpright = new List<string>();//montant en bois
    List<string> dropdownIsulationPanels = new List<string>();//panneaux d'isolation
    List<string> dropdownRainBarrier = new List<string>();//pare Pluie
    List<string> dropdownIntermediatePanels = new List<string>();//panneaux intermediaires 
    List<string> dropdownHatchPanels = new List<string>();//panneaux couverture
    List<string> dropdownFloor = new List<string>();//plancher
    
    //Define for Serializer value
    string getName;
    string getType;
    
    //Define Dropdown Left value
    List<string> dropdownranges = new List<string>();
    List<string> dropdownModel = new List<string>();

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

        ddrange.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(ddrange);
        });

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
                    
                        getIdRange = item._id;
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

    public IEnumerator GetAllModule(string pRange)
    {
        UnityWebRequest request = UnityWebRequest.Get(CONST.GetComponent<CONST>().url + URLGetAllModuleModel);

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
                //The database return a JSON file of all user infos
                string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                //Create a root object thanks to the JSON file
                RequestGetAllModule entities = JsonUtility.FromJson<RequestGetAllModule>(jsonResult);
                Debug.Log("json result model : " + jsonResult);
                dropdownModel.Clear();
                foreach (var item in entities.modules)
                {
                    Module module = item;
                    
                    if(module.rangeName == pRange)
                    {
                        //Poster all model
                        dropdownModel.Add(module.name);

                    }

                }
                ddmodel.options.Clear();
                ddmodel.AddOptions(dropdownModel);

                }
        }

    }

    void DropdownValueChanged(Dropdown pChange)
    {
        Debug.Log("pchange " + pChange.options[pChange.value].text);

        StartCoroutine(GetAllModule(pChange.options[pChange.value].text));
    }

    public IEnumerator GetAllComponent()
    {
        UnityWebRequest request = UnityWebRequest.Get(CONST.GetComponent<CONST>().url + URLGetAllComponent);

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
                //The database return a JSON file of all user infos
                string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                //Create a root object thanks to the JSON file
                RequestAllComponents entities = JsonUtility.FromJson<RequestAllComponents>(jsonResult);
                //clear dropdown list
                //dropdownWoodenUpright.Clear();
                //dropdownIsulationPanels.Clear();
                //dropdownRainBarrier.Clear();
                //dropdownIntermediatePanels.Clear();
                //dropdownHatchPanels.Clear();
                //dropdownFloor.Clear();

                foreach (var item in entities.components)
                {
                    getName = item.name;
                    getType = item.type;
                    
                    if (getType == "Montant en bois")
                    {
                        dropdownWoodenUpright.Add(getName);
                    }
                    if(getType == "Panneaux d'isolations")
                    {
                        dropdownIsulationPanels.Add(getName);
                    }
                    if (getType == "Pare-pluie")
                    {
                        dropdownRainBarrier.Add(getName);
                    }
                    if (getType == "Panneaux intermediaires")
                    {
                        dropdownIntermediatePanels.Add(getName);
                    }
                    if (getType == "Panneaux de couvertures")
                    {
                        dropdownHatchPanels.Add(getName);
                    }
                    if (getType == "Planchers")
                    {
                        dropdownFloor.Add(getName);
                    }


                }
                //ddwoodenUpright.options.Clear();
                ddwoodenUpright.AddOptions(dropdownWoodenUpright);
                //ddinsulationPanels.options.Clear();
                ddinsulationPanels.AddOptions(dropdownIsulationPanels);
                //ddrainBarrier.options.Clear();
                ddrainBarrier.AddOptions(dropdownRainBarrier);
                //ddintermediatePanels.options.Clear();
                ddintermediatePanels.AddOptions(dropdownIntermediatePanels);
                //ddhatchPanels.options.Clear();
                ddhatchPanels.AddOptions(dropdownHatchPanels);
                //ddfloor.options.Clear();
                ddfloor.AddOptions(dropdownFloor);

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
        StartCoroutine(GetAllComponent());

    }

}