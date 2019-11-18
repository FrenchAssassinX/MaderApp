using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CreateModule : MonoBehaviour
{
    public GameObject CONST;
    private string url;
    private string URLCreateModule = "v1/createmodule";  // Specific url for create module

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
        CONST = GameObject.Find("CONST"); //Get the CONST gameObject

        url = CONST.GetComponent<CONST>().url;

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

    IEnumerable PostCreateModule()
    {

        WWWForm form = new WWWForm();
        form.AddField("name", "name");
        form.AddField("cost", "cost");
        form.AddField("angle", "angle");
        form.AddField("cctp", "cctp");
        form.AddField("cut", "cut");
        form.AddField("range", "range");

        /* New webrequest with: CONST url, local URLCreateCustomer and the form */
        using (UnityWebRequest request = UnityWebRequest.Post(url + URLCreateModule, form))
        {
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);


            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                // error
                Debug.Log("erreur de la requete : " + request);
            }
            else
            {
                if (request.isDone)
                {

                }
                else
                {
                    Debug.Log("la requete n'est pas bonne");
                }
            }
        }
  
    }

    //active CreateNewCient
    void DisplayModificationModule()
    {
        canvasModificationModule.transform.gameObject.SetActive(true);

    }

    //add new project
    void SendCreateModule()
    {
        PostCreateModule();
    }
}
