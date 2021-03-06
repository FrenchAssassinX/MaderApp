﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemListComponent : MonoBehaviour
{
    public Button rowListItem;                              // The item in the list to manipulate (this object)
    public bool isSelected;                                 // Boolean to detect when the item is select on the list

    public string nameValue;                                       // Variable to pass the project on the next scene and find it by ID
    public string quantityValue;
    public string unitValue;
    public string priceValue;

    // Start is called before the first frame update
    void Start()
    {
        rowListItem = GetComponent<Button>();               // Retrieve the item in the list
        rowListItem.onClick.AddListener(SelectItem);        // Affect specific onClick behaviour to the button
        isSelected = false;
    }

    /* Function for select item detection */
    public void SelectItem()
    {
        isSelected = true;
    }
}
