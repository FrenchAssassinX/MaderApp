using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateModule : MonoBehaviour
{
    //Type (CanvasLeft)
    public Dropdown gamme;
    public Dropdown modele;
    public Dropdown coupe;
    public Button ButtonModificationModule;
    public Button ButtonCreateModule;

    //Modification Module (CanvasRight)
    public Canvas canvasModificationModule;


    // Start is called before the first frame update
    void Start()
    {
        canvasModificationModule.transform.gameObject.SetActive(false);

        Button btnMM = ButtonModificationModule.GetComponent<Button>();
        btnMM.onClick.AddListener(DisplayModificationModule);

        Button btnCM = ButtonCreateModule.GetComponent<Button>();
        btnMM.onClick.AddListener(SendCreateModule);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //active CreateNewCient
    void DisplayModificationModule()
    {
        canvasModificationModule.transform.gameObject.SetActive(true);

    }

    //add new project
    void SendCreateModule()
    {
        
    }
}
