using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UpdateEstimationCreation : MonoBehaviour
{
    public GameObject CONST;                                    // CONST object contains server route, token and user infos

    private string createModuleEstimationUrl = "v1/createmodulewithestimation";          // Specific route to get all projects
    private string deleteModuleUrl = "v1/deletemoduleonestimation";       // Specific route to delete one project

    public GameObject modulePrefab;                            // Prefab of component for 2D scene
    public GameObject middleCanvas;                               // Useful to set component prefab position 

    public GameObject deletePanel;                                // Panel to delete a component
    public GameObject deletePanelErrorMessage;
    public GameObject deletePanelMessage;

    public GameObject buttonFloorPrefab;                           // Prefab item to display all elements in project list
    public GameObject gridList;                                     // Grid to insert project prefab items
    public GameObject floorCount;                                     // Counter to set name of the floor

    public GameObject panelFloorPrefab;
    public GameObject panelsCanvas;
    public GameObject destinationPanel = null;                         // GameObject to set the destination scene of the new component
    public Button addModuleButton;

    void Start()
    {
        CONST = GameObject.Find("CONST");
        floorCount = GameObject.Find("FloorCount");                                                                      // Retrieve counter on the scene
        addModuleButton = GameObject.Find("ButtonAddModule").GetComponent<Button>();

        addModuleButton.onClick.AddListener(AddModuleOnScene);

        deletePanel.SetActive(false);

        /* Instantiate automatic floors */
        /* Ground floor */
        GameObject floor0 = Instantiate(buttonFloorPrefab, gridList.transform.position, Quaternion.identity);   // Creation of the prefab
        floor0.name = "Floor0";
        GameObject text = GameObject.Find("FloorButtonText");                                                   // Find text in the prefab
        text.name = floor0.name + "Text";                                                                       // Change name of the texte
        text.GetComponent<UnityEngine.UI.Text>().text = "Rez-de-chaussée";                                      // Change text in the prefab
        floor0.transform.SetParent(gridList.transform);                                                         // Set prefab as child of the gridList

        /* Rooftop */
        GameObject rooftop = Instantiate(buttonFloorPrefab, gridList.transform.position, Quaternion.identity);  // Creation of the prefab
        rooftop.name = "Rooftop";
        GameObject textRooftop = GameObject.Find("FloorButtonText");                                            // Find text in the prefab
        textRooftop.name = rooftop.name + "Text";                                                               // Change name of the texte
        textRooftop.GetComponent<UnityEngine.UI.Text>().text = "Toiture";                                       // Change text in the prefab
        rooftop.transform.SetParent(gridList.transform);                                                        // Set prefab as child of the gridList
        /* Not increase floor counter for the rooftop: avoid counter error */

        /* Adding panels for default floors */
        /* Floor 0 */
        GameObject panelFloor0 = Instantiate(panelFloorPrefab, panelsCanvas.transform.position, Quaternion.identity);   // Create new prefab
        panelFloor0.name = "panelFloor0";                                                                 // Change prefab name
        panelFloor0.transform.SetParent(panelsCanvas.transform);                                                        // Set prefab as child of MiddleCanvas

        /* Rooftop */
        GameObject panelRooftop = Instantiate(panelFloorPrefab, panelsCanvas.transform.position, Quaternion.identity);   // Create new prefab
        panelRooftop.name = "panelRooftop";                                                                   // Change prefab name
        panelRooftop.transform.SetParent(panelsCanvas.transform);                                                        // Set prefab as child of MiddleCanvas

    }

    void Update()
    {
        
    }


    public void AddModuleOnScene()
    {
        /* Search wich button is selected */
        foreach (Transform child in middleCanvas.transform)
        {
            GameObject panel = child.gameObject;

            if (panel.activeSelf)
            {
                destinationPanel = panel;
            }
        }

        if (destinationPanel != null)
        {
            GameObject newModule = Instantiate(modulePrefab, middleCanvas.transform.position, Quaternion.identity);   // Create new component
            newModule.transform.SetParent(destinationPanel.transform);                                                   // Change parent on scene hierarchy   
        }
    }

    public void AddFloor()
    {
        /* Adding button in the list on down panel */
        GameObject newFloor = Instantiate(buttonFloorPrefab, gridList.transform.position, Quaternion.identity);  // Creation of the prefab
        newFloor.name = "Floor" + floorCount.GetComponent<FloorCount>().floorCounter;                                                                    // Change prefab name
        GameObject textNewFloor = GameObject.Find("FloorButtonText");                                            // Find text in the prefab
        textNewFloor.name = newFloor.name + "Text";                                                               // Change name of the texte
        textNewFloor.GetComponent<UnityEngine.UI.Text>().text = "Etage " + floorCount.GetComponent<FloorCount>().floorCounter;                          // Change text in the prefab
        newFloor.transform.SetParent(gridList.transform);
        floorCount.GetComponent<FloorCount>().listFloorButtons.Add(newFloor);

        /* Adding specific panel for the floor */
        GameObject panelNewFloor = Instantiate(panelFloorPrefab, panelsCanvas.transform.position, Quaternion.identity);   // Create new prefab
        panelNewFloor.name = "panelFloor" + floorCount.GetComponent<FloorCount>().floorCounter;                                                                   // Change prefab name
        panelNewFloor.transform.SetParent(panelsCanvas.transform);                                                        // Set prefab as child of MiddleCanvas
        floorCount.GetComponent<FloorCount>().listFloorPanels.Add(panelNewFloor);

        floorCount.GetComponent<FloorCount>().floorCounter++;                                                                                           // Increase counter of floors
     
    }


    public void DisplayDeletePanel()
    {
        if (destinationPanel != null)
        {
            foreach (Transform child in destinationPanel.transform)
            {
                GameObject module = child.gameObject;

                if (module.GetComponent<UpdateModule2D>().isSelected)
                {
                    deletePanel.SetActive(true);
                    deletePanelErrorMessage.SetActive(false);
                }
            }
        }
    }

    public void HideDeletePanel()
    {
        deletePanel.SetActive(false);
    }
    
    public void DeleteModule()
    {
        if (destinationPanel != null)
        {
            foreach (Transform child in destinationPanel.transform)
            {
                GameObject module = child.gameObject;

                if (module.GetComponent<UpdateModule2D>().isSelected)
                {
                    HideDeletePanel();          // Hide delete panel
                    Destroy(module);         // Destroy module from scene
                }
            }
        }
    }

    public IEnumerator AddModulesToEstimation()
    {
        foreach (Transform child in destinationPanel.transform)
        {
            GameObject module = child.gameObject;

            WWWForm form = new WWWForm();                       // New form for web request
            form.AddField("name", module.name);    // Add to the form the value of the ID of the project to delete
            //form.AddField("cost", module.name);    // Add to the form the value of the ID of the project to delete
            //form.AddField("angle", module.name);    // Add to the form the value of the ID of the project to delete
            //form.AddField("cut", module.name);    // Add to the form the value of the ID of the project to delete
            //form.AddField("range", module.name);    // Add to the form the value of the ID of the project to delete
            //form.AddField("estimationID", module.name);    // Add to the form the value of the ID of the project to delete

            UnityWebRequest request = UnityWebRequest.Post(CONST.GetComponent<CONST>().url + createModuleEstimationUrl, form);     // Create new form
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                      // Complete form with authentication datas
            request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

            yield return request.SendWebRequest();                      // Send request                                                              

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log("*** ERROR: " + request.error + " ***");
            }
            else
            {
                if (request.isDone)
                {
                }
            }
        }
    }

    /*public IEnumerator DeleteComponent()
    {
        if (destinationPanel != null)
        {
            foreach (Transform child in destinationPanel.transform)
            {
                GameObject component = child.gameObject;

                if (component.GetComponent<UpdateComponent2D>().isSelected)
                {
                    WWWForm form = new WWWForm();                       // New form for web request
                    form.AddField("estimationID", "5dc98c411685521e3a11a237");    // Add to the form the value of the ID of the project to delete
                    form.AddField("moduleID", component.GetComponent<UpdateComponent2D>().id);    // Add to the form the value of the ID of the project to delete

                    UnityWebRequest request = UnityWebRequest.Post(CONST.GetComponent<CONST>().url + deleteModuleUrl, form);       // New request, passing url and form
                    request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                                  // Set request authentications
                    request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

                    yield return request.SendWebRequest();

                    if (request.isNetworkError || request.isHttpError)
                    {
                        // Display error message 
                        deletePanelMessage.SetActive(false);
                        deletePanelErrorMessage.SetActive(true);
                    }
                    else
                    {
                        if (request.isDone)
                        {
                            HideDeletePanel();          // Hide delete panel
                            Destroy(component);         // Destroy component from scene
                        }
                    }
                }
            }
        }
    }*/
}
