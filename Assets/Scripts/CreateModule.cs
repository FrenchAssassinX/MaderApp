using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
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

    // Banner (CanvasTop)
    public Button ButtonReturn;


    // Start is called before the first frame update
    void Start()
    {
        CONST = GameObject.Find("CONST"); //Get the CONST gameObject

        url = CONST.GetComponent<CONST>().url;

        //it's not visible for now
        canvasModificationModule.transform.gameObject.SetActive(false);

        //Modification module
        Button btnMM = ButtonModificationModule.GetComponent<Button>();
        btnMM.onClick.AddListener(DisplayModificationModule);

        //Create module
        Button btnCM = ButtonCreateModule.GetComponent<Button>();
        btnMM.onClick.AddListener(SendCreateModule);

        //return in create project page
        Button btnHP = ButtonReturn.GetComponent<Button>();
        btnHP.onClick.AddListener(ReturnCreateProjectPage);

    }

    // Update is called once per frame
    void Update()
    {

    }

    //return to home page
    void ReturnCreateProjectPage()
    {
        //Send the previous scene (create project)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
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
                    // The database return a JSON file of all user infos
                    string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                    // Create a root object thanks to the JSON file
                    CreateModules entity = JsonUtility.FromJson<CreateModules>(jsonResult);

                    //foreach (var item in entities.customers)
                    //{

                    //}
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


