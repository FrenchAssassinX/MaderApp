using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpdateEstimationCreation : MonoBehaviour
{
    /* ------------------------------------     DECLARE DATAS PART     ------------------------------------ */
    public GameObject CONST;                            // CONST object contains server route, token and user infos

    private string createModuleEstimationUrl = "v1/createmodulewithestimation";         // Specific route to get all projects
    private string deleteModuleUrl = "v1/deletemoduleonestimation";                     // Specific route to delete one project
    private string getAllRangesUrl = "v1/getallrange";                                  // Specific route to get all ranges
    private string getAllCutsUrl = "v1/getallcut";                                      // Specific route to get all cuts
    private string getAllModulesUrl = "v1/getallmodule";                                // Specific route to get all modules
    private string getModuleByIDUrl = "v1/getmodulebyid";                               // Specific route to get one module
    private string getEstimationByIDUrl = "v1/getestimationbyid";
    private string updateEstimationFloorUrl = "v1/updateestimationfloor";               // Specific route to add number of floors in estimation

    public GameObject modulePrefab;                     // Prefab of component for 2D scene
    public GameObject middleCanvas;                     // Useful to set component prefab position 

    public GameObject deletePanel;                      // Panel to delete a component
    public GameObject deletePanelErrorMessage;          // Text message to display when an error appeared
    public GameObject deletePanelMessage;               // Text message to display confirm message before delete

    public GameObject list;                             // List containing gridList
    public GameObject gridList;                         // Grid to insert project prefab items
    public GameObject floorCount;                       // Counter to set name of the floor
    public GameObject buttonFloorPrefab;                // Prefab item to display all elements in project list

    public GameObject panelFloorPrefab;                 // Prefab item to add floor on scene 
    public GameObject destinationPanel = null;          // GameObject to set the destination scene of the new component

    public Button addModuleButton;                      // Button to add new module on scene                      

    public Dropdown dropdownRanges;                     // Dropdown for ranges
    public Dropdown dropdownModels;                     // Dropdown for modeles
    public Dropdown dropdownInsulatings;                // Dropdown for insulatings
    public Dropdown dropdownFrames;                     // Dropdown for frames quality
    public Dropdown dropdownFinishingExt;               // Dropdown for exterior finishings
    public Dropdown dropdownFinishingInt;               // Dropdown for interior finishings
    public Dropdown dropdownCuts;                       // Dropdown for cuts

    private List<string> listRanges;                    // String elements for dropdownRanges
    private List<string> listModels;                    // String elements for dropdownModels
    private List<string> listInsulatings;               // String elements for dropdownInsulatings
    private List<string> listFrames;                    // String elements for dropdownFrames
    private List<string> listFinishingExt;              // String elements for dropdownFinishingExt
    private List<string> listFinishingInt;              // String elements for dropdownFinishingInt
    private List<string> listCuts;                      // String elements for dropdownCuts

    private int moduleCounter;                          // Counter to rename module and retrieve easily on scene

    private Dictionary<string, string> dictModuleIDName = new Dictionary<string, string>();    // Dictionnary to keep ID and name of the modules 
    /* ------------------------------------     END DECLARE DATAS PART     ------------------------------------ */

    
    void Start()
    {
        CONST = GameObject.Find("CONST");                                                   // Get CONST object
        floorCount = GameObject.Find("FloorCount");                                         // Retrieve counter on the scene
        addModuleButton = GameObject.Find("ButtonAddModule").GetComponent<Button>();        // Retrieve button on scene
        addModuleButton.onClick.AddListener(AddModuleOnScene);                              // Add listener to button to launch AddModuleOnScene function

        moduleCounter = 1;                  // Starting counter for module name                                                       

        deletePanel.SetActive(false);       // Disable delete panel by default

        /* Add listener to dropdowns */
        dropdownRanges.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdownRanges);
        });

        /* Instantiate list for string results */
        listRanges = new List<string>();
        listModels = new List<string>();
        listInsulatings = new List<string>();
        listFrames = new List<string>();
        listFinishingExt = new List<string>();
        listFinishingInt = new List<string>();
        listCuts = new List<string>();        

        StartGetAllRanges();                        // Function launching on start to get all ranges on dropdown
        StartGetAllCuts();                          // Function launching on start to get all cuts on dropdown

        StartCoroutine(GetAllEstimationFloors());
    }

    /* ------------------------------------     DISPLAY ELEMENT PART     ------------------------------------ */
    /* Function to add floor on scene (button and panel) */
    public void AddFloor()
    {
        /* Adding button in the list on down panel */
        GameObject newFloor = Instantiate(buttonFloorPrefab, gridList.transform.position, Quaternion.identity);                     // Creation of the prefab
        newFloor.name = "Floor" + (floorCount.GetComponent<FloorCount>().floorCounter - 1);                                               // Change prefab name
        GameObject textNewFloor = GameObject.Find("FloorButtonText");                                                               // Find text in the prefab
        textNewFloor.name = newFloor.name + "Text";                                                                                 // Change name of text
        textNewFloor.GetComponent<UnityEngine.UI.Text>().text = "Etage " + (floorCount.GetComponent<FloorCount>().floorCounter - 1);      // Change text in the prefab
        newFloor.transform.SetParent(gridList.transform);                                                                           // Change parent on scene hierarchy 
        newFloor.GetComponent<RectTransform>().sizeDelta = new Vector2(newFloor.GetComponent<RectTransform>().sizeDelta.x, list.GetComponent<RectTransform>().sizeDelta.y);     // Set default size as parent size: useful for responsivity
        newFloor.GetComponent<RectTransform>().localScale = list.GetComponent<RectTransform>().localScale;                                                                      // Set default position as parent position: useful for responsivity
        floorCount.GetComponent<FloorCount>().listFloorButtons.Add(newFloor);                                                       // Add new button on listButtons

        /* Adding specific panel for the floor */
        GameObject panelNewFloor = Instantiate(panelFloorPrefab, middleCanvas.transform.position, Quaternion.identity);     // Create new prefab
        panelNewFloor.name = "panelFloor" + +(floorCount.GetComponent<FloorCount>().floorCounter - 1);                             // Change prefab name
        panelNewFloor.transform.SetParent(middleCanvas.transform);                                                          // Set prefab as child of MiddleCanvas
        panelNewFloor.GetComponent<RectTransform>().localScale = middleCanvas.GetComponent<RectTransform>().localScale;     // Set default position as parent position: useful for responsivity
        panelNewFloor.GetComponent<RectTransform>().sizeDelta = middleCanvas.GetComponent<RectTransform>().sizeDelta;       // Set default size as parent size: useful for responsivity
        floorCount.GetComponent<FloorCount>().listFloorPanels.Add(panelNewFloor);                                           // Add new panel on listPanels

        floorCount.GetComponent<FloorCount>().floorCounter++;                                                               // Increase counter of floors
    }

    /* Function to replace existing floor from database if floors were previously created for an estimation */
    public void RecreateFloors(int pNumberOfFloors)
    {
        Debug.Log("NUMBER OF FLOORS: " + pNumberOfFloors);

        if (pNumberOfFloors > 2)
        {
            for (int i = 0; i < pNumberOfFloors; i++)
            {
                /* Adding button in the list on down panel */
                GameObject newFloor = Instantiate(buttonFloorPrefab, gridList.transform.position, Quaternion.identity);                     // Creation of the prefab
                newFloor.name = "Floor" + i;                                                                                                // Change prefab name
                GameObject textNewFloor = GameObject.Find("FloorButtonText");                                                               // Find text in the prefab
                textNewFloor.name = newFloor.name + "Text";                                                                                 // Change name of text
                textNewFloor.GetComponent<UnityEngine.UI.Text>().text = "Etage " + i;                                                       // Change text in the prefab
                newFloor.transform.SetParent(gridList.transform);                                                                           // Change parent on scene hierarchy 
                newFloor.GetComponent<RectTransform>().sizeDelta = new Vector2(newFloor.GetComponent<RectTransform>().sizeDelta.x, list.GetComponent<RectTransform>().sizeDelta.y);     // Set default size as parent size: useful for responsivity
                newFloor.GetComponent<RectTransform>().localScale = list.GetComponent<RectTransform>().localScale;                                                                      // Set default position as parent position: useful for responsivity
                floorCount.GetComponent<FloorCount>().listFloorButtons.Add(newFloor);                                                       // Add new button on listButtons

                /* Adding specific panel for the floor */
                GameObject panelNewFloor = Instantiate(panelFloorPrefab, middleCanvas.transform.position, Quaternion.identity);     // Create new prefab
                panelNewFloor.name = "panelFloor" + i;                                                                              // Change prefab name
                panelNewFloor.transform.SetParent(middleCanvas.transform);                                                          // Set prefab as child of MiddleCanvas
                panelNewFloor.GetComponent<RectTransform>().localScale = middleCanvas.GetComponent<RectTransform>().localScale;     // Set default position as parent position: useful for responsivity
                panelNewFloor.GetComponent<RectTransform>().sizeDelta = middleCanvas.GetComponent<RectTransform>().sizeDelta;       // Set default size as parent size: useful for responsivity
                floorCount.GetComponent<FloorCount>().listFloorPanels.Add(panelNewFloor);                                           // Add new panel on listPanels

                /* Rename initial floors */
                if (newFloor.name == "Floor0")
                {
                    textNewFloor.GetComponent<UnityEngine.UI.Text>().text = "Rez-de-chaussée";
                }
                else if (newFloor.name == "Floor1")
                {
                    newFloor.name = "Rooftop";
                    textNewFloor.GetComponent<UnityEngine.UI.Text>().text = "Toiture";
                    panelNewFloor.name = "panelFloorRooftop";
                }
                /* Rename other floors */
                else
                {
                    newFloor.name = "Floor" + (i - 1);
                    textNewFloor.GetComponent<UnityEngine.UI.Text>().text = "Etage " + (i - 1);
                    panelNewFloor.name = "panelFloor" + (i - 1);
                }

            }
        }
        else
        {
            /* Instantiate automatic floors */
            /* Ground floor */
            GameObject floor0 = Instantiate(buttonFloorPrefab, gridList.transform.position, Quaternion.identity);   // Creation of the prefab
            floor0.name = "Floor0";                                                                                 // Change name of floor
            GameObject text = GameObject.Find("FloorButtonText");                                                   // Find text in the prefab
            text.name = floor0.name + "Text";                                                                       // Change name of text
            text.GetComponent<UnityEngine.UI.Text>().text = "Rez-de-chaussée";                                      // Change text in the prefab
            floor0.transform.SetParent(gridList.transform);                                                         // Set prefab as child of the gridList
            floor0.GetComponent<RectTransform>().sizeDelta = new Vector2(floor0.GetComponent<RectTransform>().sizeDelta.x, list.GetComponent<RectTransform>().sizeDelta.y);     // Set default size as parent size: useful for responsivity
            floor0.GetComponent<RectTransform>().localScale = list.GetComponent<RectTransform>().localScale;                                                                    // Set default position as parent position: useful for responsivity

            /* Rooftop */
            GameObject rooftop = Instantiate(buttonFloorPrefab, gridList.transform.position, Quaternion.identity);  // Creation of the prefab
            rooftop.name = "Rooftop";                                                                               // Change name of floor
            GameObject textRooftop = GameObject.Find("FloorButtonText");                                            // Find text in the prefab
            textRooftop.name = rooftop.name + "Text";                                                               // Change name of text
            textRooftop.GetComponent<UnityEngine.UI.Text>().text = "Toiture";                                       // Change text in the prefab
            rooftop.transform.SetParent(gridList.transform);                                                        // Set prefab as child of the gridList
            rooftop.GetComponent<RectTransform>().sizeDelta = new Vector2(floor0.GetComponent<RectTransform>().sizeDelta.x, list.GetComponent<RectTransform>().sizeDelta.y);    // Set default size as parent size: useful for responsivity
            rooftop.GetComponent<RectTransform>().localScale = list.GetComponent<RectTransform>().localScale;                                                                   // Set default position as parent position: useful for responsivity
                                                                                                                                                                                // Not increase floor counter for the rooftop: avoid counter error 

            /* Adding panels for default floors */
            /* Floor 0 */
            GameObject panelFloor0 = Instantiate(panelFloorPrefab, middleCanvas.transform.position, Quaternion.identity);       // Create new prefab
            panelFloor0.name = "panelFloor0";                                                                                   // Change prefab name
            panelFloor0.transform.SetParent(middleCanvas.transform);                                                            // Set prefab as child of MiddleCanvas
            panelFloor0.GetComponent<RectTransform>().localScale = middleCanvas.GetComponent<RectTransform>().localScale;       // Set default position as parent position: useful for responsivity
            panelFloor0.GetComponent<RectTransform>().sizeDelta = middleCanvas.GetComponent<RectTransform>().sizeDelta;         // Set default size as parent size: useful for responsivity
            panelFloor0.GetComponent<Button>().onClick.AddListener(UnselectModule);                                             // Add listener to panel to launch UnselectModule function

            /* Rooftop */
            GameObject panelRooftop = Instantiate(panelFloorPrefab, middleCanvas.transform.position, Quaternion.identity);      // Create new prefab
            panelRooftop.name = "panelRooftop";                                                                                 // Change prefab name
            panelRooftop.transform.SetParent(middleCanvas.transform);                                                           // Set prefab as child of MiddleCanvas
            panelRooftop.GetComponent<RectTransform>().sizeDelta = middleCanvas.GetComponent<RectTransform>().sizeDelta;        // Set default position as parent position: useful for responsivity
            panelRooftop.GetComponent<RectTransform>().localScale = middleCanvas.GetComponent<RectTransform>().localScale;      // Set default size as parent size: useful for responsivity
            panelRooftop.GetComponent<Button>().onClick.AddListener(UnselectModule);                                            // Add listener to panel to launch UnselectModule function

            floorCount.GetComponent<FloorCount>().floorCounter++;                                                               // Increase counter of floors
        }
    }

    /* Function to display delete panel */
    public void DisplayDeletePanel()
    {
        // If a panel is selected
        if (destinationPanel != null)
        {
            // Check all modules on panel
            foreach (Transform child in destinationPanel.transform)
            {
                GameObject module = child.gameObject;                   // Convert child to Module object

                // Check if is the module selected to delete
                if (module.GetComponent<UpdateModule2D>().isSelected)
                {
                    deletePanel.SetActive(true);                        // Display delete panel
                    deletePanelErrorMessage.SetActive(false);           // Don't display error message on delete panel
                }
            }
        }
    }

    /* Function to HideDeletePanel */
    public void HideDeletePanel()
    {
        deletePanel.SetActive(false);
    }
    /* -----------------------------------    END DISPLAY ELEMENT PART     ---------------------------------- */


    /* -----------------------------------    MODULE PART     ---------------------------------- *
    /* Function to add module on scene */
    public void AddModuleOnScene()
    {
        /* Search wich button is selected */
        foreach (Transform child in middleCanvas.transform)
        {
            GameObject panel = child.gameObject;        // Convert child to Panel object

            /* Detect wich panel is active */
            if (panel.activeSelf)
            {
                destinationPanel = panel;               // Affect panel as default panel for module destination
            }
        }

        /* Verify if a panel is selected */
        if (destinationPanel != null)
        {
            GameObject newModule = Instantiate(modulePrefab, destinationPanel.transform.position, Quaternion.identity);                     // Create new module
            newModule.transform.SetParent(destinationPanel.transform);                                                                      // Change parent on scene hierarchy
            newModule.GetComponent<RectTransform>().localScale = destinationPanel.GetComponent<RectTransform>().localScale;                 // Set default size as parent size: useful for responsivity
            newModule.GetComponent<RectTransform>().anchoredPosition = destinationPanel.GetComponent<RectTransform>().anchoredPosition;     // Set default achored position as parent anchored position: useful for responsivity
            newModule.name = "Module" + moduleCounter;                                                                                      // Change name of module
            newModule.GetComponent<UpdateModule2D>().modelName = dropdownModels.options[dropdownModels.value].text;                         // Keep model name of the module
            newModule.GetComponent<UpdateModule2D>().destinationFloor = destinationPanel.name;                                              // Keep destination floor of the module
            moduleCounter++;                                                                                                                // Increase counter after rename module
        }
    }

    /* Function to replace existing module from database if modules were previously created for an estimation */
    public void RecreateModule(string pModuleID, string pModuleName, string pDestinationPanel, string pPosX, string pPosY, string pWidth, string pHeight, string pAngle)
    {
        GameObject destPanel = GameObject.Find(pDestinationPanel);                                                              // Retrieve destination panel on scene with name

        GameObject newModule = Instantiate(modulePrefab, destPanel.transform.position, Quaternion.identity);                     // Create new module
        newModule.transform.SetParent(destPanel.transform);                                                                      // Change parent on scene hierarchy

        newModule.GetComponent<RectTransform>().localScale = destPanel.GetComponent<RectTransform>().localScale;                 // Set default size as parent size: useful for responsivity
        newModule.GetComponent<RectTransform>().anchoredPosition = destPanel.GetComponent<RectTransform>().anchoredPosition;     // Set default achored position as parent anchored position: useful for responsivity

        newModule.GetComponent<RectTransform>().position = new Vector3(float.Parse(pPosX), float.Parse(pPosY), 0f);             // Replace as good position on 2D scene
        newModule.GetComponent<RectTransform>().sizeDelta = new Vector2(float.Parse(pWidth), float.Parse(pHeight));             // Give saved size to the module
        newModule.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, float.Parse(pAngle));                       // Give saved rotation to module

        newModule.name = pModuleName;

        newModule.GetComponent<UpdateModule2D>().id = pModuleID;
    }

    /* Function to unselect module */
    public void UnselectModule()
    {
        /* Check all modules in panel */
        foreach (Transform child in destinationPanel.transform)
        {
            GameObject module = child.gameObject;                           // Convert child to module object
            module.GetComponent<UpdateModule2D>().isSelected = false;       // Unselect module
        }
    }

    /* Function to detect wich module is selected to apply modifications on this module */
    public void ModifyModule()
    {
        /* Check all modules in panel */
        foreach (Transform child in destinationPanel.transform)
        {
            GameObject module = child.gameObject;                               // Convert child to module object

            // If this module is selected
            if (module.GetComponent<UpdateModule2D>().isSelected)
            {
                module.GetComponent<UpdateModule2D>().ApplyModifications();     // Apply modifications on module
            }
        }
    }
    
    /* Function to delete module */
    public void DeleteModule()
    {
        // Verifiy wich panel is selected
        if (destinationPanel != null)
        {
            // Check all modules on selected panel
            foreach (Transform child in destinationPanel.transform)
            {
                GameObject module = child.gameObject;                       // Convert child on module object

                // If module is selected
                if (module.GetComponent<UpdateModule2D>().isSelected)
                {
                    HideDeletePanel();                                      // Hide delete panel
                    Destroy(module);                                        // Destroy module from scene
                }
            }
        }
    }

    /* Function to detect Dropdown select value event */
    private void DropdownValueChanged(Dropdown pDropdown)
    {
        StartCoroutine(GetAllRangesValues(pDropdown.options[pDropdown.value].text));        // Start function for get all ranges values
        StartCoroutine(GetAllModulesModel(pDropdown.options[pDropdown.value].text));        // Start function to get all modules equals to the range
    }
    /* -----------------------------------    END MODULE PART     ---------------------------------- */


    /* ------------------------------------     CHANGE SCENE PART     ------------------------------------ */
    // Function to Go to EstimationView scene
    public void GoToEstimationView()
    {
        DontDestroyOnLoad(CONST);                                                   // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);       // Load Estimation View scene
    }

    // Function to return to CreateModule scene
    public void ReturnToCreateModule()
    {
        DontDestroyOnLoad(CONST);                                                   // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);       // Load CreateModule scene
    }
    /* -----------------------------------    END CHANGE SCENE PART     ---------------------------------- */


    /* ------------------------------------     WEB REQUEST PART     ------------------------------------ */
    /* Intermediary function to start AddModulesToEstimation function */
    public void StartAddModulesToEstimation()
    {
        //StartCoroutine(AddModulesToEstimation());
        StartCoroutine(AddFloorNumberToEstimation());
    }

    /* Intermediary function to start GetAllRanges function */
    public void StartGetAllRanges()
    {
        StartCoroutine(GetAllRanges());
    }

    /* Intermediary function to start GetAllCuts function */
    public void StartGetAllCuts()
    {
        StartCoroutine(GetAllCuts());
    }

    public IEnumerator AddModulesToEstimation()
    {
        string idModule = "";                                                                       // String to keep id of the module
        string moduleComponents = "";                                                               // String to keep components of the module from previous scene

        string idFrameQuality = dropdownFrames.options[dropdownFrames.value].text;                  // String to keep id of frame quality before creating a json
        string idWindowFrameQuality = "";                                                           // String to keep id of window frame quality before creating a json
        string idInsulating = dropdownInsulatings.options[dropdownInsulatings.value].text;          // String to keep id of insulating before creating a json
        string idCovering = "";                                                                     // String to keep id of covering before creating a json
        string idFinishingExt = dropdownFinishingExt.options[dropdownFinishingExt.value].text;      // String to keep id of finishing exterior before creating a json
        string idFinishingInt = dropdownFinishingInt.options[dropdownFinishingInt.value].text;      // String to keep id of finishing interior before creating a json
        string rangeAttributesForm = "";                                                            // String to create a json file to send rangeAttributes in form

        /* Foreach instruction to verify all module added to 2D scene */
        foreach (Transform child in destinationPanel.transform)
        {
            GameObject module = child.gameObject;                                                                   // Retrieve the module GameObject in 2D scene

            /* Register module only if it not already exist on database */
            if (module.GetComponent<UpdateModule2D>().id == "" || module.GetComponent<UpdateModule2D>().id == null)
            {
                /* Foreach to retrieve the id of the module model using to create the module */
                foreach (KeyValuePair<string, string> item in dictModuleIDName)
                {
                    if (item.Value == child.GetComponent<UpdateModule2D>().modelName)
                    {
                        idModule = item.Key;
                    }
                }

                // First request to find module property
                WWWForm formModule = new WWWForm();                                                                                     // New form for web request for module with type basic                                                         
                formModule.AddField("moduleID", idModule);

                UnityWebRequest requestModule = UnityWebRequest.Post(CONST.GetComponent<CONST>().url + getModuleByIDUrl, formModule);   // Create new form
                requestModule.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                                    // Complete form with authentication datas
                requestModule.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

                requestModule.certificateHandler = new CONST.BypassCertificate();   // Bypass certificate for https

                yield return requestModule.SendWebRequest();          // Send request                                                              

                if (requestModule.isNetworkError || requestModule.isHttpError)
                {
                    Debug.Log("*** ERROR: " + requestModule.error + " ***");
                }
                else
                {
                    if (requestModule.isDone)
                    {
                        string jsonResultModule = System.Text.Encoding.UTF8.GetString(requestModule.downloadHandler.data);      // Get JSON file
                        RequestAModule entity = JsonUtility.FromJson<RequestAModule>(jsonResultModule);                         // Convert JSON to manipulating C# object
                        Module modelModule = entity.module;                                                                     // Convert list of C# object to a C# Module object

                        // Creating json as string with the id retrieve in the last foreach
                        rangeAttributesForm = CreateJSON(idFrameQuality, idInsulating, idCovering, idWindowFrameQuality, idFinishingInt, idFinishingExt);       // Convert all Range Attributes to a JSON string to pass it in form for web request

                        /* Retrieve components of the module with id of module in dictionnary */
                        foreach (KeyValuePair<string, string> item in CONST.GetComponent<CONST>().dictComponentsForModule)
                        {
                            if (item.Key == modelModule._id)
                            {
                                moduleComponents = item.Value;
                            }
                        }

                        WWWForm form = new WWWForm();                                                               // New form for web request to create new module                                                      
                        form.AddField("name", module.name);                                                         // Module name
                        form.AddField("cost", modelModule.cost);                                                    // Module cost
                        form.AddField("angle", module.GetComponent<RectTransform>().eulerAngles.z.ToString());      // Module angle in 2D scene
                        form.AddField("cut", dropdownCuts.options[dropdownCuts.value].text);                        // Module cut
                        form.AddField("range", modelModule.range);                                                  // Module range
                        form.AddField("components", moduleComponents);                                              // Module components from previous scene
                        form.AddField("estimationID", CONST.GetComponent<CONST>().selectedEstimationID);            // Estimation ID where the module is created
                        form.AddField("rangeName", modelModule.rangeName);                                          // Name of the range module
                        form.AddField("rangeAttributes", rangeAttributesForm);                                      // All values from dropdown (Finishing int..)
                        form.AddField("x", module.GetComponent<RectTransform>().localScale.x.ToString());           // Position X of the module in 2D scene
                        form.AddField("y", module.GetComponent<RectTransform>().localScale.y.ToString());           // Position Y of the module in 2D scene
                        form.AddField("floorHouse", module.GetComponent<UpdateModule2D>().destinationFloor);        // Floor where the module is in 2D scene
                        form.AddField("width", module.GetComponent<RectTransform>().sizeDelta.x.ToString());        // Width of the module in 2D scene
                        form.AddField("height", module.GetComponent<RectTransform>().sizeDelta.y.ToString());       // Height of the module in 2D scene

                        UnityWebRequest request = UnityWebRequest.Post(CONST.GetComponent<CONST>().url + createModuleEstimationUrl, form);  // Create new request to send new module
                        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                                      // Complete form with authentication datas
                        request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

                        request.certificateHandler = new CONST.BypassCertificate();    // Bypass certificate for https

                        yield return request.SendWebRequest();          // Send request                                                              

                        if (request.isNetworkError || request.isHttpError)
                        {
                            Debug.Log("*** ERROR: " + request.error + " ***");
                        }
                        else
                        {
                            if (request.isDone)
                            {
                                string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);          // Get JSON file
                                Debug.Log(jsonResult);

                                /* VERIFY UTILITY */
                                RequestAModule entityModule = JsonUtility.FromJson<RequestAModule>(jsonResult);                 // Convert JSON to entity object
                                Module moduleResult = entityModule.module;                                                      // Convert entity object to module object
                                module.GetComponent<UpdateModule2D>().id = moduleResult._id;
                                /* END VERIFY UTILITY */
                            }
                        }
                    }
                }
            }
        }
    }

    /* Function to add floor number to estimation */
    public IEnumerator AddFloorNumberToEstimation()
    {
        WWWForm form = new WWWForm();                                                                               // New form for web request for module with type basic                                                         
        form.AddField("estimationID", CONST.GetComponent<CONST>().selectedEstimationID);                            // ID of the estimation
        form.AddField("floorNumber", floorCount.GetComponent<FloorCount>().floorCounter.ToString());                // Number of floor in estimation

        UnityWebRequest request = UnityWebRequest.Post(CONST.GetComponent<CONST>().url + updateEstimationFloorUrl, form);   // Create new form
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                                    // Complete form with authentication datas
        request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        request.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

        yield return request.SendWebRequest();          // Send request                                                              

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("*** ERROR: " + request.error + " ***");
        }
        else
        {
            if (request.isDone)
            {
                string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);          // Get JSON file
                Debug.Log(jsonResult);
            }
        }
    }

    /* Function to Delete module from database */
    public IEnumerator DeleteModuleFromEstimation(string pModuleID)
    {
        WWWForm form = new WWWForm();                                                                                     // New form for web request for module with type basic                                                         
        form.AddField("estimationID", CONST.GetComponent<CONST>().selectedEstimationID);
        form.AddField("moduleID", pModuleID);

        UnityWebRequest request = UnityWebRequest.Post(CONST.GetComponent<CONST>().url + deleteModuleUrl, form);   // Create new form
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                                    // Complete form with authentication datas
        request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        request.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

        yield return request.SendWebRequest();          // Send request                                                              

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("*** ERROR: " + request.error + " ***");
        }
        else
        {
            if (request.isDone)
            {
                string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);          // Get JSON file
                Debug.Log(jsonResult);
            }
        }
    }

    /* Fucntion to get all ranges from database */
    public IEnumerator GetAllRanges()
    {
        UnityWebRequest request = UnityWebRequest.Get(CONST.GetComponent<CONST>().url + getAllRangesUrl);       // New request, passing url and form
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                          // Set request authentications
        request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        request.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            if (request.isDone)
            {
                string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);          // Get JSON file
                RequestGetAllRange entities = JsonUtility.FromJson<RequestGetAllRange>(jsonResult);             // Convert JSON file to serializable object
                
                /* Get all ranges */
                foreach (var item in entities.range)
                {
                    Range range = item;                     // Convert item to range object

                    listRanges.Add(range.libelle);          // Add range name to list
                }

                dropdownRanges.options.Clear();             // Clear dropdown 
                dropdownRanges.AddOptions(listRanges);      // Fill dropdown with new list
            }
        }
    }

    /* Fucntion to get values of each ranges from database */
    public IEnumerator GetAllRangesValues(string pRangeName)
    {
        UnityWebRequest request = UnityWebRequest.Get(CONST.GetComponent<CONST>().url + getAllRangesUrl);       // New request, passing url and form
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                                  // Set request authentications
        request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        request.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            if (request.isDone)
            {
                string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);          // Get JSON file
                RequestGetAllRange entities = JsonUtility.FromJson<RequestGetAllRange>(jsonResult);             // Convert JSON file to serializable object

                /* Get all ranges */
                foreach (var item in entities.range)
                {
                    Range range = item;                     // Convert item to range object

                    if (range.libelle == pRangeName)
                    {
                        /* Unfill all lists string */
                        listFrames.Clear();
                        listInsulatings.Clear();
                        listFinishingExt.Clear();
                        listFinishingInt.Clear();

                        /* Get all Frames Quality for range */
                        foreach (var frameQuality in range.framequality)
                        {
                            listFrames.Add(frameQuality);                       // Add frameQuality to string list  
                        }   

                        /* Get all Insulatings for range */
                        foreach (var insulating in range.insulating)
                        {
                            listInsulatings.Add(insulating);                    // Add insulating to string list  
                        }

                        /* Get all Exterior Finishings for range */
                        foreach (var finishingExt in range.finishingext)
                        {
                            listFinishingExt.Add(finishingExt);                 // Add exterior finishing to string list  
                        }

                        /* Get all Interior Finishings for range */
                        foreach (var finishingInt in range.finishingint)
                        {
                            listFinishingInt.Add(finishingInt);                 // Add interior finishing to string list  
                        }
                    }

                    /* Clearing dropdowns */
                    dropdownFrames.options.Clear();
                    dropdownInsulatings.options.Clear();
                    dropdownFinishingExt.options.Clear();
                    dropdownFinishingInt.options.Clear();

                    /* Pouplating dropdowns with values from request */
                    dropdownFrames.AddOptions(listFrames);
                    dropdownInsulatings.AddOptions(listInsulatings);
                    dropdownFinishingExt.AddOptions(listFinishingExt);
                    dropdownFinishingInt.AddOptions(listFinishingInt);
                }
            }
        }
    }

    /* Fucntion to get all cuts from database */
    public IEnumerator GetAllCuts()
    {
        UnityWebRequest request = UnityWebRequest.Get(CONST.GetComponent<CONST>().url + getAllCutsUrl);         // New request, passing url
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                          // Set request authentications
        request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        request.certificateHandler = new CONST.BypassCertificate();    // Bypass certificate for https

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            if (request.isDone)
            {
                string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);          // Get JSON file
                RequestGetAllCuts entities = JsonUtility.FromJson<RequestGetAllCuts>(jsonResult);             // Convert JSON file to serializable object

                Debug.Log(jsonResult);
                
                /* Get all cuts */
                foreach (var item in entities.cuts)
                {
                    Cuts cut = item;                    // Convert item to cut object

                    listCuts.Add(cut.name);             // Add cut name to list         
                }

                dropdownCuts.options.Clear();           // Clear dropdown   
                dropdownCuts.AddOptions(listCuts);      // fill dropdown with list of cuts name
            }
        }
    }

    /* Fucntion to get all modules models from database */
    public IEnumerator GetAllModulesModel(string pRangeName)
    {
        UnityWebRequest request = UnityWebRequest.Get(CONST.GetComponent<CONST>().url + getAllModulesUrl);     // New request, passing url and form
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                          // Set request authentications
        request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        request.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            if (request.isDone)
            {
                string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);          // Get JSON file
                Debug.Log(jsonResult);
                RequestGetAllModule entities = JsonUtility.FromJson<RequestGetAllModule>(jsonResult);           // Convert JSON file to serializable object

                listModels.Clear();                         // Unfill list before feeling it with new datas

                /* Get all modules */
                foreach (var item in entities.modules)
                {
                    Module module = item;                 // Convert root object item to module object

                    /* If module from database have the same range as the dropdown */
                    if (module.rangeName == pRangeName)
                    {
                        /* Verify all modules linked to estimation */
                        foreach (string idModulesInEstimation in CONST.GetComponent<CONST>().listModulesCreated)
                        {
                            // If id of module from database is equal to a module linked to estimation..
                            if (idModulesInEstimation == module._id)
                            {
                                listModels.Add(module.name);                        // Add name of the module in list
                                dictModuleIDName.Add(module._id, module.name);      // Keep module id on dictionnary 
                            }
                        }
                    }
                }
                dropdownModels.options.Clear();                    // Clear dropdown
                dropdownModels.AddOptions(listModels);            // Fill dropdown with module list
            }
        }
    }

    /* Function to retrieve all floors in estimation in progress */
    public IEnumerator GetAllEstimationFloors()
    {
        /* First web request to retrieve all datas of the selected estimation */
        WWWForm form = new WWWForm();                                                                                     // New form for web request for module with type basic                                                         
        form.AddField("estimationID", CONST.GetComponent<CONST>().selectedEstimationID);

        UnityWebRequest request = UnityWebRequest.Post(CONST.GetComponent<CONST>().url + getEstimationByIDUrl, form);   // Create new form
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                                    // Complete form with authentication datas
        request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        request.certificateHandler = new CONST.BypassCertificate();    // Bypass certificate for https

        yield return request.SendWebRequest();          // Send request                                                              

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("*** ERROR: " + request.error + " ***");
        }
        else
        {
            if (request.isDone)
            {
                string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);          // Get JSON file
                Debug.Log(jsonResult);

                RequestAnEstimation entity = JsonUtility.FromJson<RequestAnEstimation>(jsonResult);
                Estimation estimation = entity.estimation;

                /* If estimation don't have any floor, starting counter to 1 */
                if (estimation.floorNumber == "" || estimation.floorNumber == null)
                {
                    floorCount.GetComponent<FloorCount>().floorCounter = 1;
                } 
                else
                {
                    floorCount.GetComponent<FloorCount>().floorCounter = int.Parse(estimation.floorNumber);
                }

                RecreateFloors(floorCount.GetComponent<FloorCount>().floorCounter);
            }
        }
    }

    /* Function to retrieve all modules in estimation in progress */
    public IEnumerator GetAllEstimationModules()
    {
        /* First web request to retrieve all datas of the selected estimation */
        WWWForm form = new WWWForm();                                                                                     // New form for web request for module with type basic                                                         
        form.AddField("estimationID", CONST.GetComponent<CONST>().selectedEstimationID);

        UnityWebRequest request = UnityWebRequest.Post(CONST.GetComponent<CONST>().url + getEstimationByIDUrl, form);   // Create new form
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                                    // Complete form with authentication datas
        request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        request.certificateHandler = new CONST.BypassCertificate();   // Bypass certificate for https

        yield return request.SendWebRequest();          // Send request                                                              

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("*** ERROR: " + request.error + " ***");
        }
        else
        {
            if (request.isDone)
            {
                string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);          // Get JSON file
                Debug.Log(jsonResult);

                RequestAnEstimation entity = JsonUtility.FromJson<RequestAnEstimation>(jsonResult);
                Estimation estimation = entity.estimation;

                foreach (var item in estimation.module)
                {
                    /* Second web request to retrieve all properties of selected modules */
                    WWWForm formModule = new WWWForm();                                                                                     // New form for web request for module with type basic                                                         
                    formModule.AddField("moduleID", item.id);

                    UnityWebRequest requestModule = UnityWebRequest.Post(CONST.GetComponent<CONST>().url + getModuleByIDUrl, formModule);   // Create new form
                    requestModule.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                                    // Complete form with authentication datas
                    requestModule.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

                    requestModule.certificateHandler = new CONST.BypassCertificate();    // Bypass certificate for https

                    yield return requestModule.SendWebRequest();          // Send request                                                              

                    if (requestModule.isNetworkError || requestModule.isHttpError)
                    {
                        Debug.Log("*** ERROR: " + requestModule.error + " ***");
                    }
                    else
                    {
                        if (requestModule.isDone)
                        {
                            string jsonResultModule = System.Text.Encoding.UTF8.GetString(requestModule.downloadHandler.data);          // Get JSON file
                            Debug.Log(jsonResultModule);

                            RequestAModule entityModule = JsonUtility.FromJson<RequestAModule>(jsonResultModule);
                            Module myModule = entityModule.module;

                            RecreateModule(myModule._id, myModule.name, myModule.floorHouse, myModule.x, myModule.y, myModule.width, myModule.height, myModule.angle);
                        }
                    }
                }
            }
        }
    }

    /* -----------------------------------    END WEB REQUEST PART     ---------------------------------- */

    /* Function to create a string like JSON and pass it to request */
    public string CreateJSON(string pFrameQuality, string pInsulating, string pCovering, 
                                string pWindowFrameQuality, string pFinishingInt, string pFinishingExt)
    {
        string beginArray = "[";
        string endArray = "]";
        string beginObj = "{";
        string endObj = "}";
        string virgule = ",";
        string quote = "\"";
        string doublePoint = ":";

        StringBuilder sb = new StringBuilder();

        /* Start */
        sb.Append(beginArray);                      // [
        sb.Append(beginObj);                        // {

        /* Frame Quality */
        sb.Append(quote);                           // "
        sb.Append("frameQuality");                  // frameQuality
        sb.Append(quote);                           // "
        sb.Append(doublePoint);                     // :
        sb.Append(quote);                           // "
        sb.Append(pFrameQuality);                   // frameQuality value
        sb.Append(quote);                           // "
        sb.Append(virgule);                         // ,

        /* Insulating */
        sb.Append(quote);                           // "
        sb.Append("insulating");                    // insulating
        sb.Append(quote);                           // "
        sb.Append(doublePoint);                     // :
        sb.Append(quote);                           // "
        sb.Append(pInsulating);                     // insulating value
        sb.Append(quote);                           // "
        sb.Append(virgule);                         // ,

        /* Covering */
        sb.Append(quote);                           // "
        sb.Append("covering");                      // covering
        sb.Append(quote);                           // "
        sb.Append(doublePoint);                     // :
        sb.Append(quote);                           // "
        sb.Append(pCovering);                       // covering value
        sb.Append(quote);                           // "
        sb.Append(virgule);                         // ,

        /* Windows Frame Quality */
        sb.Append(quote);                           // "
        sb.Append("windowsframequality");           // windowsframequality
        sb.Append(quote);                           // "
        sb.Append(doublePoint);                     // :
        sb.Append(quote);                           // "
        sb.Append(pWindowFrameQuality);             // windowsframequality value
        sb.Append(quote);                           // "
        sb.Append(virgule);                         // ,

        /* Finishing Interior */
        sb.Append(quote);                           // "
        sb.Append("finishingint");                  // finishingint
        sb.Append(quote);                           // "
        sb.Append(doublePoint);                     // :
        sb.Append(quote);                           // "
        sb.Append(pFinishingInt);                   // finishingint value
        sb.Append(quote);                           // "
        sb.Append(virgule);                         // ,

        /* Finishing Exterior */
        sb.Append(quote);                           // "
        sb.Append("finishingext");                  // finishingext
        sb.Append(quote);                           // "
        sb.Append(doublePoint);                     // :
        sb.Append(quote);                           // "
        sb.Append(pFinishingExt);                   // finishingext value
        sb.Append(quote);                           // "

        /* End */
        sb.Append(endObj);                          // }
        sb.Append(endArray);                        // ]

        return sb.ToString();
    }
}
