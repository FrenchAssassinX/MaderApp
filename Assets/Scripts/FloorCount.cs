using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCount : MonoBehaviour
{
    public int floorCounter;                    // Counting the number of floors on the scene
    public List<GameObject> listFloorButtons;   // List to find floor buttons: useful for delete or rename floor buttons
    public List<GameObject> listFloorPanels;    // List to find floor panels: useful for delete or rename floor panels

    public void Start()
    {
        /* Initializing variables of the Counter when scene start */
        floorCounter = 1;
        listFloorButtons = new List<GameObject>();
        listFloorPanels = new List<GameObject>();
    }
}
