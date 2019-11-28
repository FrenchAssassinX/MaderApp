using System.Collections;
using System.Collections.Generic;
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
    public Dropdown dropdownModeles;                    // Dropdown for modeles
    public Dropdown dropdownInsulatings;                // Dropdown for insulatings
    public Dropdown dropdownFrames;                     // Dropdown for frames quality
    public Dropdown dropdownFinishingExt;               // Dropdown for exterior finishings
    public Dropdown dropdownFinishingInt;               // Dropdown for interior finishings
    public Dropdown dropdownCuts;                       // Dropdown for cuts

    private List<string> listRanges;                    // String elements for dropdownRanges
    private List<string> listModeles;                   // String elements for dropdownModeles
    private List<string> listInsulatings;               // String elements for dropdownInsulatings
    private List<string> listFrames;                    // String elements for dropdownFrames
    private List<string> listFinishingExt;              // String elements for dropdownFinishingExt
    private List<string> listFinishingInt;              // String elements for dropdownFinishingInt
    private List<string> listCuts;                      // String elements for dropdownCuts

    private int moduleCounter;                          // Counter to rename module and retrieve easily on scene

    GameObject panelFloor0;
    BoxCollider2D boxCollider;
    /* ------------------------------------     END DECLARE DATAS PART     ------------------------------------ */

    
    void Start()
    {
        CONST = GameObject.Find("CONST");                                                   // Get CONST object
        floorCount = GameObject.Find("FloorCount");                                         // Retrieve counter on the scene
        addModuleButton = GameObject.Find("ButtonAddModule").GetComponent<Button>();        // Retrieve button on scene
        addModuleButton.onClick.AddListener(AddModuleOnScene);                              // Add listener to button to launch AddModuleOnScene function

        moduleCounter = 1;                  // Starting counter for module name                                                       

        deletePanel.SetActive(false);       // Disable delete panel by default

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
        panelFloor0 = Instantiate(panelFloorPrefab, middleCanvas.transform.position, Quaternion.identity);       // Create new prefab
        panelFloor0.name = "panelFloor0";                                                                                   // Change prefab name
        panelFloor0.transform.SetParent(middleCanvas.transform);                                                            // Set prefab as child of MiddleCanvas
        panelFloor0.GetComponent<RectTransform>().localScale = middleCanvas.GetComponent<RectTransform>().localScale;       // Set default position as parent position: useful for responsivity
        panelFloor0.GetComponent<RectTransform>().sizeDelta = middleCanvas.GetComponent<RectTransform>().sizeDelta;         // Set default size as parent size: useful for responsivity
        panelFloor0.GetComponent<Button>().onClick.AddListener(UnselectModule);                                             // Add listener to panel to launch UnselectModule function

        boxCollider = panelFloor0.GetComponent<BoxCollider2D>();

        /* Rooftop */
        GameObject panelRooftop = Instantiate(panelFloorPrefab, middleCanvas.transform.position, Quaternion.identity);      // Create new prefab
        panelRooftop.name = "panelRooftop";                                                                                 // Change prefab name
        panelRooftop.transform.SetParent(middleCanvas.transform);                                                           // Set prefab as child of MiddleCanvas
        panelRooftop.GetComponent<RectTransform>().sizeDelta = middleCanvas.GetComponent<RectTransform>().sizeDelta;        // Set default position as parent position: useful for responsivity
        panelRooftop.GetComponent<RectTransform>().localScale = middleCanvas.GetComponent<RectTransform>().localScale;      // Set default size as parent size: useful for responsivity
        panelRooftop.GetComponent<Button>().onClick.AddListener(UnselectModule);                                            // Add listener to panel to launch UnselectModule function

        /* Add listener to dropdowns */
        dropdownRanges.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdownRanges);
        });

        /* Instantiate list for string results */
        listRanges = new List<string>();
        listModeles = new List<string>();
        listInsulatings = new List<string>();
        listFrames = new List<string>();
        listFinishingExt = new List<string>();
        listFinishingInt = new List<string>();
        listCuts = new List<string>();        

        StartGetAllRanges();                        // Function launching on start to get all ranges on dropdown
    }

    void Update()
    {
        
    }

    /* ------------------------------------     DISPLAY ELEMENT PART     ------------------------------------ */
    /* Function to add floor on scene (button and panel) */
    public void AddFloor()
    {
        /* Adding button in the list on down panel */
        GameObject newFloor = Instantiate(buttonFloorPrefab, gridList.transform.position, Quaternion.identity);                     // Creation of the prefab
        newFloor.name = "Floor" + floorCount.GetComponent<FloorCount>().floorCounter;                                               // Change prefab name
        GameObject textNewFloor = GameObject.Find("FloorButtonText");                                                               // Find text in the prefab
        textNewFloor.name = newFloor.name + "Text";                                                                                 // Change name of text
        textNewFloor.GetComponent<UnityEngine.UI.Text>().text = "Etage " + floorCount.GetComponent<FloorCount>().floorCounter;      // Change text in the prefab
        newFloor.transform.SetParent(gridList.transform);                                                                           // Change parent on scene hierarchy 
        newFloor.GetComponent<RectTransform>().sizeDelta = new Vector2(newFloor.GetComponent<RectTransform>().sizeDelta.x, list.GetComponent<RectTransform>().sizeDelta.y);     // Set default size as parent size: useful for responsivity
        newFloor.GetComponent<RectTransform>().localScale = list.GetComponent<RectTransform>().localScale;                                                                      // Set default position as parent position: useful for responsivity
        floorCount.GetComponent<FloorCount>().listFloorButtons.Add(newFloor);                                                       // Add new button on listButtons

        /* Adding specific panel for the floor */
        GameObject panelNewFloor = Instantiate(panelFloorPrefab, middleCanvas.transform.position, Quaternion.identity);     // Create new prefab
        panelNewFloor.name = "panelFloor" + floorCount.GetComponent<FloorCount>().floorCounter;                             // Change prefab name
        panelNewFloor.transform.SetParent(middleCanvas.transform);                                                          // Set prefab as child of MiddleCanvas
        panelNewFloor.GetComponent<RectTransform>().localScale = middleCanvas.GetComponent<RectTransform>().localScale;     // Set default position as parent position: useful for responsivity
        panelNewFloor.GetComponent<RectTransform>().sizeDelta = middleCanvas.GetComponent<RectTransform>().sizeDelta;       // Set default size as parent size: useful for responsivity
        floorCount.GetComponent<FloorCount>().listFloorPanels.Add(panelNewFloor);                                           // Add new panel on listPanels

        floorCount.GetComponent<FloorCount>().floorCounter++;                                                               // Increase counter of floors
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
                GameObject module = child.gameObject;       // Convert child to Module object

                // Check if is the module selected to delete
                if (module.GetComponent<UpdateModule2D>().isSelected)
                {
                    deletePanel.SetActive(true);                    // Display delete panel
                    deletePanelErrorMessage.SetActive(false);       // Don't display error message on delete panel
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
            GameObject newModule = Instantiate(modulePrefab, destinationPanel.transform.position, Quaternion.identity); // Create new module
            newModule.transform.SetParent(destinationPanel.transform);                                                  // Change parent on scene hierarchy   
            newModule.name = "Module" + moduleCounter;                                                                  // Change name of module
            moduleCounter++;                                                                                            // Increase counter after rename module
        }
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
        StartCoroutine(AddModulesToEstimation());
    }

    /* Intermediary function to start GetAllRanges function */
    public void StartGetAllRanges()
    {
        StartCoroutine(GetAllRanges());
    }

    public IEnumerator AddModulesToEstimation()
    {
        foreach (Transform child in destinationPanel.transform)
        {
            GameObject module = child.gameObject;

            WWWForm form = new WWWForm();                                   // New form for web request
            form.AddField("name", module.name);                             // TO MODIFY                          
            form.AddField("cost", "12");                                    // TO MODIFY                                                             
            form.AddField("angle", module.GetComponent<RectTransform>().eulerAngles.z.ToString());      
            form.AddField("cut", "horizontal");                             // TO MODIFY                                                      
            form.AddField("range", "Luxe");                                 // TO MODIFY                                                           
            form.AddField("estimationID", "5ddbf64db8cb5a77a435e2fb");      // TO MODIFY                                            

            UnityWebRequest request = UnityWebRequest.Post(CONST.GetComponent<CONST>().url + createModuleEstimationUrl, form);      // Create new form
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                                          // Complete form with authentication datas
            request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

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

                    GoToEstimationView();
                }
            }
        }
    }

    /* Fucntion to get all ranges from database */
    public IEnumerator GetAllRanges()
    {
        UnityWebRequest request = UnityWebRequest.Get(CONST.GetComponent<CONST>().url + getAllRangesUrl);       // New request, passing url and form
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                                  // Set request authentications
        request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

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
                    Range range = item;                 // Convert item to range object

                    listRanges.Add(range.libelle);
                }

                dropdownRanges.options.Clear();
                dropdownRanges.AddOptions(listRanges);
            }
        }
    }

    /* Fucntion to get values of each ranges from database */
    public IEnumerator GetAllRangesValues(string pRangeName)
    {
        UnityWebRequest request = UnityWebRequest.Get(CONST.GetComponent<CONST>().url + getAllRangesUrl);       // New request, passing url and form
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                                  // Set request authentications
        request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

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
                            listFrames.Add(frameQuality);       // Add frameQuality to string list  
                        }

                        /* Get all Insulatings for range */
                        foreach (var insulating in range.insulating)
                        {
                            listInsulatings.Add(insulating);       // Add insulating to string list  
                        }

                        /* Get all Exterior Finishings for range */
                        foreach (var finishingExt in range.finishingext)
                        {
                            listFinishingExt.Add(finishingExt);       // Add exterior finishing to string list  
                        }

                        /* Get all Interior Finishings for range */
                        foreach (var finishingInt in range.finishingint)
                        {
                            listFinishingInt.Add(finishingInt);       // Add interior finishing to string list  
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
    /* -----------------------------------    END WEB REQUEST PART     ---------------------------------- */
}
