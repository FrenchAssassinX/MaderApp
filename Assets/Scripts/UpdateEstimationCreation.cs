using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateEstimationCreation : MonoBehaviour
{
    //public GameObject CONST;                                    // CONST object contains server route, token and user infos

    //private string getProjectUrl = "v1/getallproject";          // Specific route to get all projects
    //private string deleteProjectUrl = "v1/deleteproject";       // Specific route to delete one project

    public GameObject componentPrefab;                            // Prefab of component for 2D scene
    public GameObject middleCanvas;                               // Useful to set component prefab position 

    public GameObject buttonFloorPrefab;                           // Prefab item to display all elements in project list
    public GameObject gridList;                                     // Grid to insert project prefab items
    public GameObject floorCount;                                     // Counter to set name of the floor

    public GameObject panelFloorPrefab;
    public GameObject panelsCanvas;

    void Start()
    {
        floorCount = GameObject.Find("FloorCount");                                                                      // Retrieve counter on the scene

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

    public void AddComponentOnScene()
    {
        GameObject destinationPanel = null;                                         // GameObject to set the destination scene of the new component

        /* Search wich button is selected */
        foreach (Transform child in middleCanvas.transform)
        {
            GameObject panel = child.gameObject;

            if (panel.activeSelf)
            {
                destinationPanel = panel;
            }
        }

        GameObject newComponent = Instantiate(componentPrefab, middleCanvas.transform.position, Quaternion.identity);   // Create new component
        newComponent.transform.SetParent(destinationPanel.transform);               // Change parent on scene hierarchy                   
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
}
