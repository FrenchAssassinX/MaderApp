using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateHomeTitle : MonoBehaviour
{
    public GameObject homeText; 

    void Start()
    {
        homeText.GetComponent<UnityEngine.UI.Text>().text = "Bienvenue Nicolas";
    }

    void Update()
    {
        
    }
}
