using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewProject : MonoBehaviour
{
    public GameObject CONST;
    private string url;
    private string URLCreateCustomer = "v1/createcustomer"; // Specific url for create customer
    private string URLCreateProject = "v1/createproject"; // Specific url for create project
    private string URLGetCustomers = "v1/getallcustomer"; // Specific url for get all customers

    //project (CanvasLeft)
    public InputField nameProject;
    public InputField referenceProject;
    public Dropdown idCustomer;
    public Button ButtonCreateCustomer;
    public Button ButtonCreateProject;

    //new client (CanvasRight)
    public InputField name;
    public InputField surname;
    public InputField roadNum;
    public InputField road;
    public InputField zipcode;
    public InputField city;
    public InputField roadExtra;
    public InputField email;
    public InputField phone;
    public Button ButtonCreateNewCustomer;
    public Canvas canvasNewClient;

    //text information
    public GameObject createValideCustomer;
    public GameObject createValideProject;
    public GameObject errorCreateCustomer;

    //Top
    public Button ButtonReturn;

    public string idClientForForm;
    string IdCustomerGenerated;
    List<string> dropdowncustomer = new List<string>();

    string change;
    string getSurname;
    string getName;
    string newGetSurname;
    string newGetName;

    // Start is called before the first frame update
    void Start()
    {

        CONST = GameObject.Find("CONST"); //Get the CONST gameObject

        url = CONST.GetComponent<CONST>().url;

        //it's not visible for now
        canvasNewClient.transform.gameObject.SetActive(false);
        createValideCustomer.transform.gameObject.SetActive(false);
        errorCreateCustomer.transform.gameObject.SetActive(false);
        createValideProject.transform.gameObject.SetActive(false);
        referenceProject.enabled = false;
        //Active canvasRight for add new customer
        Button btnCC = ButtonCreateCustomer.GetComponent<Button>();
        btnCC.onClick.AddListener(DisplayCreateNewCustomer);

        //send new project
        Button btnCP = ButtonCreateProject.GetComponent<Button>();
        btnCP.onClick.AddListener(SendCreateProject);

        //send new customer
        Button btnNC = ButtonCreateNewCustomer.GetComponent<Button>();
        btnNC.onClick.AddListener(SendCreateCustomer);

        //return home page
        Button btnHP = ButtonReturn.GetComponent<Button>();
        btnHP.onClick.AddListener(ReturnHomePage);

        StartCoroutine(GetAllCustomers());

        idCustomer.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(idCustomer);
        });

        }

    //void Update()
    //{
    //    if (!idCustomer.GetComponent<Dropdown>().options.Equals("Client"))
    //    {
    //        referenceProject.GetComponent<InputField>().text = IdCustomerGenerated;
    //    }
    //}         



        //active CreateNewCient
        void DisplayCreateNewCustomer()
        {
            canvasNewClient.transform.gameObject.SetActive(true);

        }

        //add new project
        void SendCreateProject()
        {
            StartCoroutine(PostFormNewProject());
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);

        }

        //add new customer
        void SendCreateCustomer()
        {

            StartCoroutine(PostFormNewCustomer());

        }

        //return to home page
        void ReturnHomePage()
        {
            //Send the previous scene (home page)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

        IEnumerator PostFormNewCustomer()
        {
            WWWForm form = new WWWForm();                   // New form for web request
            form.AddField("name", name.text);
            form.AddField("surename", surname.text);
            form.AddField("road", road.text);
            form.AddField("roadNum", roadNum.text);
            form.AddField("zipcode", zipcode.text);
            form.AddField("city", city.text);
            form.AddField("roadExtra", roadExtra.text);
            form.AddField("phone", phone.text);
            form.AddField("email", email.text);
            form.AddField("_id", "12345");
            form.AddField("__v", "0");


            /* New webrequest with: CONST url, local URLCreateCustomer and the form */
            using (UnityWebRequest request = UnityWebRequest.Post(url + URLCreateCustomer, form))
            {
                request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);


                yield return request.SendWebRequest();
                if (request.isNetworkError || request.isHttpError)
                {
                    // error
                    errorCreateCustomer.transform.gameObject.SetActive(true);
                }
                else
                {
                    if (request.isDone)
                    {
                        // The database return a JSON file of all user infos
                        string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                        // Create a root object thanks to the JSON file
                        CreateCustomer entity = JsonUtility.FromJson<CreateCustomer>(jsonResult);
                        //message validate new customer
                        createValideCustomer.transform.gameObject.SetActive(true);

                        DontDestroyOnLoad(CONST);                                               // Keep the CONST object between scenes
                    }

                }
            }



        }

        IEnumerator PostFormNewProject()
        {

            WWWForm form = new WWWForm();
            form.AddField("userID", "5dbee1c6e9b5241f704fdca9");
            form.AddField("date", "2019-11-22");
            form.AddField("road", road.text);
            form.AddField("roadNum", roadNum.text);
            form.AddField("roadExtra", roadExtra.text);
            form.AddField("zipcode", zipcode.text);
            form.AddField("city", city.text);
            form.AddField("customerID", idClientForForm); //not .text in the end because is a string
            form.AddField("reference", referenceProject.text);
            form.AddField("projectName", nameProject.text);


            using (UnityWebRequest request = UnityWebRequest.Post(url + URLCreateProject, form))
            {
                request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

                yield return request.SendWebRequest();
                if (request.isNetworkError || request.isHttpError)
                {
                    //error
                }
                else
                {
                    if (request.isDone)
                    {
                        // The database return a JSON file of all user infos
                        string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                        // Create a root object thanks to the JSON file
                        CreateProject entity = JsonUtility.FromJson<CreateProject>(jsonResult);
                        //message validate new customer
                        createValideProject.transform.gameObject.SetActive(true);

                    }

                }
            }


        }

        IEnumerator GetAllCustomers()
        {
            UnityWebRequest request = UnityWebRequest.Get(CONST.GetComponent<CONST>().url + URLGetCustomers);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                //error
            }
            else
            {
                if (request.isDone)
                {

                    string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);          // Get JSON file

                    RequestGetAllCustomer entities = JsonUtility.FromJson<RequestGetAllCustomer>(jsonResult);         // Convert JSON file

                    Debug.Log("jsons result : " + jsonResult);


                    foreach (var item in entities.customers)
                    {
                    //recuperation values in customers
                    string getId = item._id;
                    getSurname = item.surename;
                    getName = item.name;
                    
                    //Poster all customers
                    dropdowncustomer.Add(getName + getSurname);

                    //Debug.Log("Dropdown customer :" + idCustomer);
                    //Select the 5 firsts letters for surname
                    
                    

                    //Select the first letters for name
                    
                    
                                    

                    idClientForForm = getId;
                }
                idCustomer.options.Clear();
                idCustomer.AddOptions(dropdowncustomer);

                }
            }
        }
    void DropdownValueChanged(Dropdown pChange)
    {
        Debug.Log(pChange.options[pChange.value].text);
        change = pChange.options[pChange.value].text;
        Debug.Log("change : " + change);

        //Select the timestanp
        var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

        //---------------------------Generate the ID Project-----------------------------------
        if (getSurname != "" && getSurname != null)
        {
            newGetSurname = getSurname.Substring(0, 3);
        }

        if (getName != "" && getName != null)
        {
            newGetName = getName.Substring(0, 1);
        }

        IdCustomerGenerated = change + Timestamp;

        //post ref project
        referenceProject.GetComponent<InputField>().text = IdCustomerGenerated;

        //-------------------------------------------------------------------------------------
    }
}
