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

    Vector2 moduleInitialPos;               // Keep initial position of module to avoid to place out of panel
    public bool outOfSection;               // boolean to detect

    public GameObject colliderObject;       // Child GameObject to change collider        
    public BoxCollider2D collider;          // Collider to control collisions of module

    public string destinationFloor;         // String to keep floor of module 2D: useful for loading scene
    public string modelName;                // String to keep name of the model using to create the module

    public string idCuts = "";                          // String to keep id of frame quality before creating a json
    public string idFrameQuality = "";                  // String to keep id of frame quality before creating a json
    public string idWindowFrameQuality = "";            // String to keep id of window frame quality before creating a json
    public string idInsulating = "";                    // String to keep id of insulating before creating a json
    public string idCovering = "";                      // String to keep id of covering before creating a json
    public string idFinishingExt = "";                  // String to keep id of finishing exterior before creating a json
    public string idFinishingInt = "";                  // String to keep id of finishing interior before creating a json
    public string rangeAttributesForm = "";

    void Start()
    {
        moduleInitialPos = this.GetComponent<RectTransform>().position;                     // Get the iniital position of the component
        outOfSection = false;                                                               // Instantiate the boolean to false

        sectionInput = GameObject.Find("SectionInputField").GetComponent<InputField>();     // Retrieve input on scene
        widthInput = GameObject.Find("LengthInputField").GetComponent<InputField>();        // Retrieve input on scene
        angleInput = GameObject.Find("AngleInputField").GetComponent<InputField>();         // Retrieve input on scene

        button = this.GetComponent<Button>();                                               // Retrieve button on scene
        button.onClick.AddListener(SelectComponent);                                        // Add event to select component on scene

        panelScene = button.transform.parent.gameObject;                                    // Retrieve the parent panel of the component on scene
        Debug.Log("Parent name: " + panelScene.name);

        isSelected = false;                                                                 // On start, component is not selected

        collider = colliderObject.GetComponent<BoxCollider2D>();                            // Retrieve box collider of module
    }

    void Update()
    {
        UpdateCollider();
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

    /* Function to update size and rotation of the collider */
    public void UpdateCollider()
    {
        colliderObject.GetComponent<RectTransform>().sizeDelta = button.GetComponent<RectTransform>().sizeDelta;
        collider.size = button.GetComponent<RectTransform>().sizeDelta;
        colliderObject.GetComponent<RectTransform>().rotation = button.GetComponent<RectTransform>().rotation;

    }

    /* Function to drag element on scene */
    public void OnDrag(PointerEventData pEventData)
    {
        if (isSelected)
        {
            button.GetComponent<RectTransform>().position = Input.mousePosition;       // If component is selected, then move component to mouse position
        }
    }

    /* Function to end dragging element */
    public void OnEndDrag(PointerEventData pEventData)
    {
        /* If component is out of area... */
        if (outOfSection)
        {
            this.GetComponent<RectTransform>().position = moduleInitialPos;                 // Return it to its initial position
            outOfSection = false;                                                           // Reinitialize boolean
        }
        else
        {
            button.GetComponent<RectTransform>().position = Input.mousePosition;           // Let the component on the last mouse position before end dragging
        }
    }

    /* Function to detect where module is out of panel */
    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision !");

        /* If the collision object is a Module, then do not consired it as a collision */
        if (!collision.gameObject.name.Contains("Module"))
        {
            outOfSection = true;        // Boolean pass to true
        }
    }
}
