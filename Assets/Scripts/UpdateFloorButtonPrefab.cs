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
        gridList = GameObject.Find("GridList");                         // Retrieve gridList on the scene

        floorButton = GetComponent<Button>();                           // Retrieve button element on the scene
        floorButton.onClick.AddListener(SelectItem);                    // Lauch item selected function
        isSelected = false;                                             // Set to false on start

        middleCanvas = GameObject.Find("MiddleCanvas");                 // Retrieve the parent canvas on the scene
        panelFloor = GameObject.Find("panel" + floorButton.name);       // Retrieve panel element on the scene

        floorCount = GameObject.Find("FloorCount");                     // Retrieve counter on the scene

        deleteButton = GameObject.Find("ButtonDeleteFloor");            // Retrieve button on the scene
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

            Debug.Log("Selected button:" + this.gameObject.name);

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
            button.gameObject.GetComponent<UpdateFloorButtonPrefab>().isSelected = false;       // Unselect all buttons except the current selected button
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
            floorCount.GetComponent<FloorCount>().listFloorButtons.Remove(this.gameObject);     // Remove button from the list
            floorCount.GetComponent<FloorCount>().listFloorPanels.Remove(panelFloor);           // Remove panel from the list

            Destroy(this.gameObject);                                                           // Destroy floor button
            Destroy(panelFloor);                                                                // Destroy floor panel

            RenameFloors();                                                                     // Launch function to rename the surviving floors
        }
    }

    /* Function to rename floors when a floor is delete by user */
    private void RenameFloors()
    {
        floorCount.GetComponent<FloorCount>().floorCounter--;                                               // Decrease counter of floors
        int counterForeach = 1;                                                                             // Start counter for renaming floors

        /* Rename all the floor buttons and panels */
        foreach (GameObject button in floorCount.GetComponent<FloorCount>().listFloorButtons)               
        {
            if (counterForeach <= floorCount.GetComponent<FloorCount>().floorCounter)                       // If the counter is less or equal of the number of existing floors: continue
            {
                GameObject panel = null;                                                                    // Declare empty panel

                /* Search panel associate to the button */
                foreach (GameObject itemPanel in floorCount.GetComponent<FloorCount>().listFloorPanels)
                {
                    if (itemPanel.name == "panel" + button.name)
                    {
                        panel = itemPanel;                                                                  // If the panel is found on the list, then affected to the empty panel
                    }
                }

                GameObject textButton = GameObject.Find(button.name + "Text");                              // Retrieve the text button on the list
                button.name = "Floor" + counterForeach;                                                     // Rename the button with existing floors
                panel.name = "panel" + button.name;                                                         // Rename panel with existing floors
                textButton.name = button.name + "Text";                                                     // Rename text button with existing floors
                textButton.GetComponent<UnityEngine.UI.Text>().text = "Etage" + counterForeach;             // Rename the text displayed in the button with exiting floors

                counterForeach++;                                                                           // Increase counter of floors
            }

        }
    }
}
