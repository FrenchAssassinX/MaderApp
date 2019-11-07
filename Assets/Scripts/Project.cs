using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Project : MonoBehaviour
{
    public GameObject dateValue;
    public GameObject refValue;
    public GameObject clientValue;
    public GameObject statusValue;
    public GameObject sellerValue;

    void Start()
    {
        dateValue = GameObject.Find("DateValue");
        refValue = GameObject.Find("RefValue");
        clientValue = GameObject.Find("ClientValue");
        statusValue = GameObject.Find("StatusValue");
        sellerValue = GameObject.Find("SellerValue");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
