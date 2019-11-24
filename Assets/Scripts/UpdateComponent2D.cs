using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpdateComponent2D : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public bool isSelected;                 // Boolean to detect when component is selected

    public InputField sectionInput;         // Input to change height of the component
    public InputField widthInput;           // Input to change width of the component
    public InputField angleInput;           // Input to change angle of the component

    private Button button;                  // Button variable to get component on start

    void Start()
    {
        sectionInput = GameObject.Find("SectionInputField").GetComponent<InputField>();     // Retrieve input on scene
        widthInput = GameObject.Find("LengthInputField").GetComponent<InputField>();        // Retrieve input on scene
        angleInput = GameObject.Find("AngleInputField").GetComponent<InputField>();         // Retrieve input on scene

        button = this.GetComponent<Button>();                                               // Retrieve button on scene
        button.onClick.AddListener(SelectComponent);                                        // Add event to select component on scene

        isSelected = false;                                                                 // On start, component is not selected
    }

    void Update()
    {
        /* If component is selected */
        if (isSelected)
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
            }

        }
    }

    /* Function to select component on scene */
    private void SelectComponent()
    {
        isSelected = true;
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
