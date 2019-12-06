using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateModule : MonoBehaviour
{
    public GameObject CONST;
    private string url;
    private string URLCreateModule = "v1/createmodule";                         // Specific url for create module
    private string URLEstimationModule = "v1/createmodulewithestimation";       // Specific url for estimation module
    private string URLRange = "v1/getallrange";                                 // Specific url for range
    private string URLgetAllModule = "v1/getAllModule";                         // Specific url for get module
    private string URLGetRangeById = "v1/getrangebyid";                         // Specific url for Post range id
    private string URLGetAllComponent = "v1/getallcomponent";
    private string URLGetAllModuleModel = "v1/getallmodulemodel";
    private string URLPostGetComponentbyId = "v1/getcomponentbyid";

    //Type (CanvasLeft)
    public Dropdown ddrange;                    //gamme
    public Dropdown ddmodel;                    //modele
    public Input name;                          //name of module
    public Button buttonModificationModule;
    public Button buttonCreateModule;
    public GameObject goodSendModule;
    public GameObject badSendModule;
    
    //Modification Module (CanvasRight)
    public Canvas canvasModificationModule;
    public Dropdown ddwoodenUpright;            //montantBois;
    public Dropdown ddinsulationPanels;         //panneauxIsolation;
    public Dropdown ddrainBarrier;              //parePluie;
    public Dropdown ddintermediatePanels;       //panneauxIntermediaires;
    public Dropdown ddhatchPanels;              //panneauxCouverture;
    public Dropdown ddfloor;                    //plancher;

    // Banner (CanvasTop)
    public Button buttonReturn;
    public Button buttonNext;
    string getIdRange;

    //Define Dropdown Right value
    List<string> dropdownWoodenUpright = new List<string>();        //montant en bois
    List<string> dropdownIsulationPanels = new List<string>();      //panneaux d'isolation
    List<string> dropdownRainBarrier = new List<string>();          //pare Pluie
    List<string> dropdownIntermediatePanels = new List<string>();   //panneaux intermediaires 
    List<string> dropdownHatchPanels = new List<string>();          //panneaux couverture
    List<string> dropdownFloor = new List<string>();                //plancher
    
    //Define for Serializer value
    string getName;
    string getType;
    string getRangeNameForForm;     //ddrange
    string getRangeForForm;         //ddrange
    string getModelForForm;         //ddmodel

    string getWUForForm;            //ddwoodenUpright
    string getIPForForm;            //ddinsulationPanels
    string getRBForForm;            //ddrainBarrier
    string getITPForForm;           //ddintermediatePanels
    string getHPForForm;            //ddhatchPanels
    string getFloorForForm;         //ddfloor

    string code;
    string unit;
    string description;

    string idModule;
    string nameModule;
    string costModule;
    string angleModule;
    string cutModule;

    //Define Dropdown Left value
    List<string> dropdownranges = new List<string>();
    List<string> dropdownModel = new List<string>();

    // 
    public List<Components> listComponents = new List<Components>();
    public List<string> listSelectedModules = new List<string>();

    void Start()
    {
        CONST = GameObject.Find("CONST"); //Get the CONST gameObject

        url = CONST.GetComponent<CONST>().url;

        //it's not visible for now
        canvasModificationModule.transform.gameObject.SetActive(false);

        //Modification module
        Button btnMM = buttonModificationModule.GetComponent<Button>();
        btnMM.onClick.AddListener(DisplayModificationModule);

        //Create module
        Button btnCM = buttonCreateModule.GetComponent<Button>();
        btnCM.onClick.AddListener(SendFullModule);

        goodSendModule.transform.gameObject.SetActive(false);
        badSendModule.transform.gameObject.SetActive(false);

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
                    string libelle = item.libelle;

                    //Poster all ranges
                    dropdownranges.Add(libelle);     
                }
                ddrange.options.Clear();
                ddrange.AddOptions(dropdownranges);
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
            Debug.Log("ERROR: " + request);
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
                    getRangeForForm = pRange;                    
                    if(module.rangeName == pRange)
                    {
                        //Poster all model
                        getRangeNameForForm = module.rangeName;
                        getModelForForm = module.name;
                        dropdownModel.Add(module.name);
                        idModule = module._id;
                        nameModule = module.name;
                        costModule = module.cost;
                        angleModule = module.angle;
                        cutModule = module.cut;
                    }
                }
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

                foreach (var item in entities.components)
                {
                    Components component = item;
                    listComponents.Add(component);

                    getName = item.name;
                    getType = item.type;
                    description = item.description;
                    unit = item.unit;
                    code = item.code;
                    
                    if (getType == "Montant en bois")
                    {
                        dropdownWoodenUpright.Add(getName);
                        getWUForForm = getName;
                    }
                    if(getType == "Panneaux d'isolations")
                    {
                        dropdownIsulationPanels.Add(getName);
                        getIPForForm = getName;
                    }
                    if (getType == "Pare-pluie")
                    {
                        dropdownRainBarrier.Add(getName);
                        getRBForForm = getName;
                    }
                    if (getType == "Panneaux intermediaires")
                    {
                        dropdownIntermediatePanels.Add(getName);
                        getITPForForm = getName;
                    }
                    if (getType == "Panneaux de couvertures")
                    {
                        dropdownHatchPanels.Add(getName);
                        getHPForForm = getName;
                    }
                    if (getType == "Planchers")
                    {
                        dropdownFloor.Add(getName);
                        getFloorForForm = getName;
                    }
                }

                ddwoodenUpright.AddOptions(dropdownWoodenUpright);
                ddinsulationPanels.AddOptions(dropdownIsulationPanels);
                ddrainBarrier.AddOptions(dropdownRainBarrier);
                ddintermediatePanels.AddOptions(dropdownIntermediatePanels);
                ddhatchPanels.AddOptions(dropdownHatchPanels);
                ddfloor.AddOptions(dropdownFloor);
            }
        }
    }

    public IEnumerator PostAllModuleModel()
    {
        /* First request to retrieve selected components */
        UnityWebRequest requestComponents = UnityWebRequest.Get(CONST.GetComponent<CONST>().url + URLGetAllComponent);

        requestComponents.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        requestComponents.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);
        yield return requestComponents.SendWebRequest();

        if (requestComponents.isNetworkError || requestComponents.isHttpError)
        {
            // error
            Debug.Log("erreur de la requete : " + requestComponents);
        }
        else
        {
            if (requestComponents.isDone)
            {
                //The database return a JSON file of all user infos
                string jsonResult = System.Text.Encoding.UTF8.GetString(requestComponents.downloadHandler.data);
                //Create a root object thanks to the JSON file
                RequestAllComponents entities = JsonUtility.FromJson<RequestAllComponents>(jsonResult);

                /* Retrieve all selected components: if selected component in Dropdwon is equals to current item, then affect ID */
                foreach (var item in entities.components)
                {
                    if (item.name == ddwoodenUpright.options[ddwoodenUpright.value].text ||
                            item.name == ddinsulationPanels.options[ddinsulationPanels.value].text ||
                            item.name == ddrainBarrier.options[ddrainBarrier.value].text ||
                            item.name == ddintermediatePanels.options[ddintermediatePanels.value].text ||
                            item.name == ddhatchPanels.options[ddhatchPanels.value].text ||
                            item.name == ddfloor.options[ddfloor.value].text)
                    {
                        listSelectedModules.Add(item.name);
                    }
                }
            }
        }

        /* Convert components selected from dropdowns on the right to string JSON */
        string componentsSelected = CreateJsonToSend(listComponents, listSelectedModules);

        WWWForm form = new WWWForm(); // New form for web request

        form.AddField("name", nameModule);
        form.AddField("cost", costModule);
        form.AddField("angle", angleModule);
        form.AddField("cut", cutModule);
        form.AddField("range", getRangeForForm);
        form.AddField("rangeName", getRangeNameForForm);
        form.AddField("components", componentsSelected);

        /* Second request to create new module */
        using (UnityWebRequest request = UnityWebRequest.Post(url + URLEstimationModule, form))
        {
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

            yield return request.SendWebRequest();

            // If connection failed
            if (request.isNetworkError || request.isHttpError)
            {
                badSendModule.transform.gameObject.SetActive(true);
                Debug.Log("ERROR: " + request.error);
            }
            // If connection succeeded
            else
            {
                if (request.isDone)
                {
                    // The database return a JSON file of all modules infos
                    string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                    Debug.Log("json result in post : " + jsonResult);

                    // Create a root object thanks to the JSON file
                    RootObject entity = JsonUtility.FromJson<RootObject>(jsonResult);
                    goodSendModule.transform.gameObject.SetActive(true);
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
    
    void SendFullModule()
    {
        StartCoroutine(PostAllModuleModel());
    }

    public string CreateJsonToSend(List<Components> pComponentsList, List<string> pSelectedModuleNameList)
    {
        string beginArray = "[";
        string endArray = "]";
        string beginObj = "{";
        string endObj = "}";
        string virgule = ",";
        string quote = "\"";

        StringBuilder sb = new StringBuilder();
        sb.Append(beginArray);// [
        for(int i = 0; i < pSelectedModuleNameList.Count; i++)
        {
            string name = pSelectedModuleNameList[i];

            for(int y = 0; y < pComponentsList.Count; y++)
            {
                string module = pComponentsList[y].name;

                if (module != null && module != "")
                {
                    if (module.Equals(name))
                    {
                        sb.Append(beginObj);// {
                        sb.Append(quote);//    "
                        sb.Append("id");//     id
                        sb.Append(quote);//    "
                        sb.Append(":");//      :
                        sb.Append(quote);//    "
                        sb.Append(pComponentsList[y]._id);// valeur de l'id
                        sb.Append(quote);//    "
                        sb.Append(virgule);//  ,
                        sb.Append(quote);//    "
                        sb.Append("qte");//    qte
                        sb.Append(quote);//    "
                        sb.Append(":");//      :
                        sb.Append(quote);//    "
                        sb.Append("1");//      1
                        sb.Append(quote);//    "
                        sb.Append(endObj);//   }
                        if (i < (pSelectedModuleNameList.Count - 1))
                        {
                            //ajoute une virgule si et seulement si il y a un suivant dans la liste des modules selectionné
                            sb.Append(virgule);// ,
                        }
                        break;
                    }
                }
            }
            if (i == (pSelectedModuleNameList.Count - 1))
            {
                //ajoute le crochet de fin de array si et seulement si c'est le dernier item de la liste
                sb.Append(endArray);// ]
            }
        }

        return sb.ToString();
    }
}