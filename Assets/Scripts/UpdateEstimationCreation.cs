using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateEstimationCreation : MonoBehaviour
{
    //public GameObject CONST;                                    // CONST object contains server route, token and user infos

    //private string getProjectUrl = "v1/getallproject";          // Specific route to get all projects
    //private string deleteProjectUrl = "v1/deleteproject";       // Specific route to delete one project

    public GameObject buttonFloorPrefab;                           // Prefab item to display all elements in project list
    public GameObject gridList;                                 // Grid to insert project prefab items
    private int floorCount = 0;                                     // Counter to set name of the floor

    void Start()
    {
        /* Instantiate automatic floors */
        /* Groudn floor */
        GameObject floor0 = Instantiate(buttonFloorPrefab, gridList.transform.position, Quaternion.identity);   // Creation of the prefab
        floor0.name = "floor" + floorCount;
        GameObject text = GameObject.Find("FloorButtonText");                                                   // Find text in the prefab
        text.name = floor0.name + "Text";                                                                       // Change name of the texte
        text.GetComponent<UnityEngine.UI.Text>().text = "Rez-de-chaussée";                                      // Change text in the prefab
        floor0.transform.SetParent(gridList.transform);                                                         // Set prefab as children of the gridList
        floorCount++;                                                                                           // Increase counter of floors

        /* Rooftop */
        GameObject rooftop = Instantiate(buttonFloorPrefab, gridList.transform.position, Quaternion.identity);  // Creation of the prefab
        GameObject textRooftop = GameObject.Find("FloorButtonText");                                            // Find text in the prefab
        textRooftop.name = rooftop.name + "Text";                                                               // Change name of the texte
        textRooftop.GetComponent<UnityEngine.UI.Text>().text = "Toiture";                                       // Change text in the prefab
        rooftop.transform.SetParent(gridList.transform);                                                        // Set prefab as children of the gridList
        /* Not increase floor counter for the rooftop: avoid counter error */
    }

    void Update()
    {
        
    }

    public void AddFloor()
    {
        GameObject newFloor = Instantiate(buttonFloorPrefab, gridList.transform.position, Quaternion.identity);  // Creation of the prefab
        newFloor.name = "floor" + floorCount;                                                                    // Change prefab name
        GameObject textNewFloor = GameObject.Find("FloorButtonText");                                            // Find text in the prefab
        textNewFloor.name = newFloor.name + "Text";                                                               // Change name of the texte
        textNewFloor.GetComponent<UnityEngine.UI.Text>().text = "Etage " + floorCount;                          // Change text in the prefab
        newFloor.transform.SetParent(gridList.transform);
        floorCount++;                                                                                           // Increase counter of floors

    }
}
