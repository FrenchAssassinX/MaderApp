using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateFloorButtonPrefab : MonoBehaviour
{
    private GameObject middleCanvas;                                    // Parent canvas of the floor panels
    public GameObject gridList;                                         // Parent of the buttons

    private Button floorButton;                                         // Button on the list
    private GameObject panelFloor;                                      // Panel for the concerned floor
    public GameObject floorCount;                                       // Counter to set name of the floor

    private GameObject deleteButton;                                    // Button to delete prefab

    public bool isSelected;                                             // Boolean for onClick function

    void Start()
    {
        gridList = GameObject.Find("GridList");                                     // Retrieve gridList on the scene

        floorButton = GetComponent<Button>();                                       // Retrieve button element on the scene
        floorButton.onClick.AddListener(SelectItem);                                // Lauch item selected function
        isSelected = false;                                                         // Set to false on start

        middleCanvas = GameObject.Find("MiddleCanvas");                             // Retrieve the parent canvas on the scene
        panelFloor = GameObject.Find("panel" + floorButton.name);                   // Retrieve panel element on the scene

        deleteButton = GameObject.Find("ButtonDeleteFloor");                        // Retrieve button on the scene
    }

    void Update()
    {
        /* Launch function to display the concerned floor panel */
        if (isSelected)
        {
            /* Calling functions to reset displaying settings */
            UnselectAllButtons();
            DisplayAllFloorPanels();
            DisplaySelectedFloorPanel();

            deleteButton.GetComponent<Button>().onClick.AddListener(DeleteFloor);       // Event to delete floor
        }

    }

    /* Function for select item detection */
    public void SelectItem()
    {
        isSelected = true;
    }

    /* Function to unselected all buttons of the gridList */
    public void UnselectAllButtons()
    {
        foreach (Transform button in gridList.transform)
        {
            button.gameObject.GetComponent<UpdateFloorButtonPrefab>().isSelected = false;
        }
    }

    /* Function to enable all the panels, in case of changing button */
    public void DisplayAllFloorPanels()
    {
        /* Verify all the existing floor panels */
        foreach (Transform panel in middleCanvas.transform)
        {
            panel.gameObject.SetActive(true);
        }
    }

    /* Function to display only the panel of the current selected floor */
    public void DisplaySelectedFloorPanel()
    {
        /* Verify all the existing floor panels */
        foreach (Transform panel in middleCanvas.transform)
        {
            /* If the name of the panel is different from the currend selected floor: hide it */
            if (panel.name != panelFloor.name)
            {
                panel.gameObject.SetActive(false);
            }
        }
    }

    /* Function to delete floor */ 
    private void DeleteFloor()
    {
        // Cannot delete Floor0 and Rooftop
        if (this.gameObject.name != "Floor0" && this.gameObject.name != "Rooftop")
        {
            Destroy(this.gameObject);
            Destroy(panelFloor);

            floorCount.GetComponent<FloorCount>().floorCounter--;                        // Decrease counter of floors

            /* Rename all the floor buttons */
            // TO DO
        }
    }
}
