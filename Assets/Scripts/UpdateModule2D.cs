﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpdateModule2D : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public bool isSelected;                 // Boolean to detect when component is selected

    public string id;

    private InputField sectionInput;        // Input to change height of the component
    private InputField widthInput;          // Input to change width of the component
    private InputField angleInput;          // Input to change angle of the component

    public GameObject panelScene;           // Scene 2D to move component

    private Button button;                  // Button variable to get component on start

    void Start()
    {
        sectionInput = GameObject.Find("SectionInputField").GetComponent<InputField>();     // Retrieve input on scene
        widthInput = GameObject.Find("LengthInputField").GetComponent<InputField>();        // Retrieve input on scene
        angleInput = GameObject.Find("AngleInputField").GetComponent<InputField>();         // Retrieve input on scene

        button = this.GetComponent<Button>();                                               // Retrieve button on scene
        button.onClick.AddListener(SelectComponent);                                        // Add event to select component on scene

        panelScene = button.transform.parent.gameObject;                                    // Retrieve the parent panel of the component on scene
        Debug.Log("Parent name: " + panelScene.name);

        isSelected = false;                                                                 // On start, component is not selected
    }

    void Update()
    {
        /* Keep component on panel */
        /* Left */
        /*if (button.GetComponent<RectTransform>().localPosition.x < panelScene.GetComponent<RectTransform>().localPosition.x - panelScene.GetComponent<RectTransform>().localPosition.x / 2)
        {
            button.GetComponent<RectTransform>().localPosition = new Vector2(
                panelScene.GetComponent<RectTransform>().localPosition.x - panelScene.GetComponent<RectTransform>().localPosition.x / 2,
                button.GetComponent<RectTransform>().localPosition.y
            );
        }*/
        /* Right */
        if (button.GetComponent<RectTransform>().localPosition.x > 422)
        {
            button.GetComponent<RectTransform>().localPosition = new Vector2(
                                                                    422,
                                                                    button.GetComponent<RectTransform>().localPosition.y
            );
        }
        /* Down */
        if (button.GetComponent<RectTransform>().localPosition.y > 380)
        {
            button.GetComponent<RectTransform>().localPosition = new Vector2(
                                                                    button.GetComponent<RectTransform>().localPosition.x,
                                                                    380
                                                                    
            );
        }
        /* Top */
        if (button.GetComponent<RectTransform>().localPosition.y < -380)
        {
            button.GetComponent<RectTransform>().localPosition = new Vector2(
                                                                    button.GetComponent<RectTransform>().localPosition.x,
                                                                    -380

            );
        }
    }

    /* Function to select component on scene */
    private void SelectComponent()
    {
        isSelected = true;
    }

    public void ApplyModifications()
    {
        /* Displaying values of the component on UI interface */
        sectionInput.placeholder.GetComponent<UnityEngine.UI.Text>().text = button.GetComponent<RectTransform>().sizeDelta.y.ToString();
        widthInput.placeholder.GetComponent<UnityEngine.UI.Text>().text = button.GetComponent<RectTransform>().sizeDelta.x.ToString();
        angleInput.placeholder.GetComponent<UnityEngine.UI.Text>().text = button.GetComponent<RectTransform>().eulerAngles.z.ToString();

        /* If value of the component or not empty... */
        if (widthInput.text != "" && sectionInput.text != "" && angleInput.text != "")
        {
            /* Convert values of the input fields to float */
            float width = float.Parse(widthInput.text);
            float height = float.Parse(sectionInput.text);
            float angle = float.Parse(angleInput.text);

            /* Affect new values of input fields to the component */
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);        // width, height
            button.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, angle);    // angle

            /* Unfill input fields */
            widthInput.text = "";
            sectionInput.text = "";
            angleInput.text = "";
        }
    }

    /* Function to drag element on scene */
    public void OnDrag(PointerEventData pEventData)
    {
        if (isSelected)
        {
            GetComponent<RectTransform>().position = Input.mousePosition;       // If component is selected, then move component to mouse position
        }
    }

    /* Function to end dragging element */
    public void OnEndDrag(PointerEventData pEventData)
    {
        GetComponent<RectTransform>().position = Input.mousePosition;           // Let the component on the last mouse position before end dragging
    }
}
