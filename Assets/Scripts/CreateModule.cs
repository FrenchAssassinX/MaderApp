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
    private string URLEstimationModule = "v1/createmodulewithestimation"; // Specific url for estimation module
    private string URLRange = "v1/getallrange"; // Specific url for range

    //Type (CanvasLeft)
    public Dropdown ddrange; //gamme
    public Dropdown ddmodel; //modele
    public Dropdown ddcut; //coupe
    public Input name; //name of module
    public Button ButtonModificationModule;
    public Button ButtonCreateModule;

    //Modification Module (CanvasRight)
    public Canvas canvasModificationModule;
    public Dropdown woodenUpright; //montantBois;
    public Dropdown insulationPanels; //panneauxIsolation;
    public Dropdown rainBarrier; //parePluie;
    public Dropdown intermediatePanels; //panneauxIntermediaires;
    public Dropdown hatchPanels; //panneauxCouverture;
    public Dropdown floor; //plancher;

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
        //Button btnCM = ButtonCreateModule.GetComponent<Button>();
        //btnMM.onClick.AddListener();

        //return in create project page
        Button btnHP = ButtonReturn.GetComponent<Button>();
        btnHP.onClick.AddListener(ReturnCreateProjectPage);

        StartCoroutine(GetAllRange());

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

    public IEnumerator GetAllRange()
    {

        UnityWebRequest request = UnityWebRequest.Get(CONST.GetComponent<CONST>().url + URLRange);
        
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
                Debug.Log("it's ok");
                    // The database return a JSON file of all user infos
                    string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                    // Create a root object thanks to the JSON file
                    RequestGetAllRange entities = JsonUtility.FromJson<RequestGetAllRange>(jsonResult);
                    
                    Debug.Log("entities : " + entities.range);

                    Debug.Log("result jsdon : " + jsonResult);

                    foreach (var item in entities.range)
                    {
                    
                        Debug.Log("dans la boucle");
                        Debug.Log("item : " + item);
                        string getId = item._id;
                        string getLibelle = item.libelle;

                        Debug.Log("libelle : " + getLibelle);
                        Debug.Log("id : " + getId);

                        //Poster all ranges
                        List<string> dropdownranges = new List<string>() { getLibelle };
                        ddrange.AddOptions(dropdownranges);
                    }
                }
                else
                {
                    Debug.Log("la requete n'est pas bonne");
                }
            }
        
    }
    //active CreateNewCient
    void DisplayModificationModule()
    {
        canvasModificationModule.transform.gameObject.SetActive(true);

    }

}


