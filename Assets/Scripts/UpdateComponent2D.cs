using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpdateComponent2D : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public bool isSelected;

    private Button button;

    private bool isPointerDown;
    private float pointerDownTimer;
    private float requiredHoldTime;


    void Start()
    {
        button = this.GetComponent<Button>();
        button.onClick.AddListener(SelectComponent);

        isSelected = false;
    }

    void Update()
    {
        if (isSelected)
        {
            //Debug.Log("Component selected");

        }
    }

    private void SelectComponent()
    {
        isSelected = true;
    }

    /*public void OnPointerDown(PointerEventData pEventData)
    {
        isPointerDown = true;
        Debug.Log("PointerDown");
    }

    public void OnPointerUp(PointerEventData pEventData)
    {
        Reset();
        Debug.Log("PointerUp");
    }

    private void Reset()
    {
        isPointerDown = false;
        pointerDownTimer = 0;
    }*/

    public void OnDrag(PointerEventData pEventData)
    {
        //if (isSelected)
        //{
            GetComponent<RectTransform>().position = Input.mousePosition;
        //}
    }

    public void OnEndDrag(PointerEventData pEventData)
    {
        GetComponent<RectTransform>().position = Input.mousePosition;
    }
}
