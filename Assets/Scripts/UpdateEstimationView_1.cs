using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UpdateEstimationView_1 : MonoBehaviour
{
    public GameObject CONST;

    //Text conteners that are showed in the window
    public GameObject customer;
    public GameObject projectName;
    public GameObject totalAfterDiscount;
    public GameObject totalBeforeDiscount;
    public GameObject discount;


    public GameObject listItemPrefab;                  // Prefab item that represents a component
    public GameObject componentList;                   //panel wich will contain all the components of the estimation
    double estimationPrice = 0;

    // Start is called before the first frame update
    void Start()
    {
        CONST = GameObject.Find("CONST"); //CONST initialize
        
        string totalBeforeDiscountSt = CONST.GetComponent<CONST>().estimationPrice; //string that receives the price of the estimation  before any discount
        string discountSt = CONST.GetComponent<CONST>().estimationDiscount; //string that receives the discount value
        double discountInt = Convert.ToDouble(discountSt); //parsing the string discount to an int for the calculation of the estimation price after the discount
        double totalBeforeDiscountInt = estimationPrice; //parsing the totalBeforeDiscountSt to an int for the calculation of the estimation price after the discount

        double totalAfterDiscountInt = totalBeforeDiscountInt; //Initantiate the price after the discount as the start original price

        if (discountInt != 0) //if the price isn't null we calculate the discounted price
        {
            double mult = totalBeforeDiscountInt * discountInt; //firstly we multiply the original price with the discount number
            double sub = mult / 100; //secondly we divide the result per 100. It wil give the amount of the discount
            totalAfterDiscountInt = totalBeforeDiscountInt - sub; //we substract the amout of the discount from the original price, and we have the price after the discounting
        }

        totalBeforeDiscount.GetComponent<UnityEngine.UI.Text>().text = totalBeforeDiscountSt; //Shows the value of the original price
        discount.GetComponent<UnityEngine.UI.Text>().text = discountSt; //show the value of the discount 
        totalAfterDiscount.GetComponent<UnityEngine.UI.Text>().text = totalAfterDiscountInt.ToString(); //shows the final price
        customer.GetComponent<UnityEngine.UI.Text>().text = CONST.GetComponent<CONST>().customerName; //get the customers name and shows it
        projectName.GetComponent<UnityEngine.UI.Text>().text = CONST.GetComponent<CONST>().projectName; //get the project names and shows it

        StartCoroutine(GetEstimation()); //Start the search of the estimation datas
    }

    //Get the Estimation datas
    public IEnumerator GetEstimation()
    {
        var urlToGetEstimation = CONST.GetComponent<CONST>().url + "v1/getestimationbyid"; //http road to get the estimation by giving its Id

        WWWForm estimationForm = new WWWForm();                       // New form for web request
        estimationForm.AddField("estimationID", CONST.GetComponent<CONST>().selectedEstimationID);    // Add to the form the value of the ID of the estimation to get

        UnityWebRequest requestForEstimation = UnityWebRequest.Post(urlToGetEstimation, estimationForm);     // Create new WebRequest
        requestForEstimation.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");           // Complete form with authentication datas
        requestForEstimation.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        requestForEstimation.certificateHandler = new CONST.BypassCertificate();    // Bypass certificate for https

        yield return requestForEstimation.SendWebRequest(); //execute the web request

        if (requestForEstimation.isNetworkError || requestForEstimation.isHttpError)
        {
            Debug.Log(requestForEstimation.error);
        }
        else
        {
            string jsonResultFromEstimation = System.Text.Encoding.UTF8.GetString(requestForEstimation.downloadHandler.data);          // Get JSON file

            RequestAnEstimation estimationEntity = JsonUtility.FromJson<RequestAnEstimation>(jsonResultFromEstimation);         // Convert JSON file

            Estimation estimation = estimationEntity.estimation; //Create a serialized estimation object 

            
            foreach (var moduleItem in estimation.module) //bubkle into all the modules of the current estimation
            {
                var urlToGetModule = CONST.GetComponent<CONST>().url + "v1/getmodulebyid"; //http road to get the module datas by giving its ID

                WWWForm moduleForm = new WWWForm();                       // New form for web request
                moduleForm.AddField("moduleID", moduleItem.id);    // Add to the form the value of the ID of the module to get

                UnityWebRequest requestForModule = UnityWebRequest.Post(urlToGetModule, moduleForm);     // Create new WebRequest
                requestForModule.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                      // Complete form with authentication datas
                requestForModule.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

                requestForModule.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

                yield return requestForModule.SendWebRequest(); //execute the web request

                if (requestForModule.isNetworkError || requestForModule.isHttpError)
                {
                    Debug.Log(requestForModule.error);
                }
                else
                {
                    string jsonResultFromModule = System.Text.Encoding.UTF8.GetString(requestForModule.downloadHandler.data);          // Get JSON file

                    RequestAModule moduleEntity = JsonUtility.FromJson<RequestAModule>(jsonResultFromModule);         // Convert JSON file
                    Module module = moduleEntity.module; //create a serealized module object 

                    double moduleCost = Int32.Parse(module.cost);
                    estimationPrice += moduleCost;
                    Debug.Log("module price : " + moduleCost);
                    foreach (var componentItem in module.components)//bubkle into all the components of the current module
                    {
                        StartCoroutine(GetComponent(componentItem.id, componentItem.qte));//start the search of the current component datas, sending its id, and its quantity
                    }
                }
            }

            estimationPrice = estimationPrice * 2.60;
            Debug.Log("estimationPrice : " + estimationPrice);

            string totalBeforeDiscountSt = estimationPrice.ToString(); //string that receives the price of the estimation  before any discount
            string discountSt = CONST.GetComponent<CONST>().estimationDiscount; //string that receives the discount value
            double discountInt = Convert.ToDouble(discountSt); //parsing the string discount to an int for the calculation of the estimation price after the discount
            double totalBeforeDiscountInt = estimationPrice; //parsing the totalBeforeDiscountSt to an int for the calculation of the estimation price after the discount

            double totalAfterDiscountInt = totalBeforeDiscountInt; //Initantiate the price after the discount as the start original price

            if (discountInt != 0) //if the price isn't null we calculate the discounted price
            {
                double mult = totalBeforeDiscountInt * discountInt; //firstly we multiply the original price with the discount number
                double sub = mult / 100; //secondly we divide the result per 100. It wil give the amount of the discount
                totalAfterDiscountInt = totalBeforeDiscountInt - sub; //we substract the amout of the discount from the original price, and we have the price after the discounting
            }

            totalBeforeDiscount.GetComponent<UnityEngine.UI.Text>().text = totalBeforeDiscountSt; //Shows the value of the original price
            discount.GetComponent<UnityEngine.UI.Text>().text = discountSt; //show the value of the discount 
            totalAfterDiscount.GetComponent<UnityEngine.UI.Text>().text = totalAfterDiscountInt.ToString(); //shows the final price

            StartCoroutine(UpdateEstimationPrice(estimationPrice, estimation));
        }
    }

    //get the module datas and will buckle into for searching the components datas
    public IEnumerator UpdateEstimationPrice(double estimationPrice, Estimation estimation)
    {

        var urlToUpdate = CONST.GetComponent<CONST>().url + "v1/updateestimationprice"; //http road to get the module datas by giving its ID

        WWWForm estimationForm = new WWWForm();                       // New form for web request
        estimationForm.AddField("estimationID", estimation._id);    // Add to the form the value of the ID of the module to get
        estimationForm.AddField("price", estimationPrice.ToString());

        UnityWebRequest requestForModule = UnityWebRequest.Post(urlToUpdate, estimationForm);     // Create new WebRequest
        requestForModule.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                      // Complete form with authentication datas
        requestForModule.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        requestForModule.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

        yield return requestForModule.SendWebRequest(); //execute the web request

        if (requestForModule.isNetworkError || requestForModule.isHttpError)
        {
            Debug.Log(requestForModule.error);
        }
    }


    //get the component datas and show them into the grid list
    public IEnumerator GetComponent(string componentId, string componentQte)
    {
        var urlToGetComponent = CONST.GetComponent<CONST>().url + "v1/getcomponentbyid";

        WWWForm componentForm = new WWWForm();                       // New form for web request
        componentForm.AddField("componentID", componentId);    // Add to the form the value of the ID of the project to get

        UnityWebRequest requestForComponent = UnityWebRequest.Post(urlToGetComponent, componentForm);     // Create new WebRequest
        requestForComponent.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                      // Complete form with authentication datas
        requestForComponent.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        requestForComponent.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

        yield return requestForComponent.SendWebRequest();

        if (requestForComponent.isNetworkError || requestForComponent.isHttpError)
        {
            Debug.Log(requestForComponent.error);
        }
        else
        {
            string jsonResultFromComponent = System.Text.Encoding.UTF8.GetString(requestForComponent.downloadHandler.data);          // Get JSON file

            RequestAComponent componentEntity = JsonUtility.FromJson<RequestAComponent>(jsonResultFromComponent);         // Convert JSON file

            ComponentToShow component = componentEntity.component;

            // Create prefab
            GameObject listItem = Instantiate(listItemPrefab, Vector3.zero, Quaternion.identity);

            // Set estimationListPanel as parent of prefab in project hierarchy
            listItem.transform.SetParent(componentList.transform);

            listItem.GetComponent<RectTransform>().localScale = componentList.GetComponent<RectTransform>().localScale;
            listItem.GetComponent<RectTransform>().sizeDelta = new Vector2(componentList.GetComponent<RectTransform>().sizeDelta.x, listItem.GetComponent<RectTransform>().sizeDelta.y);

            // Find children in listItem to use them
            GameObject nameValue = GameObject.Find("nameText");
            GameObject quantityValue = GameObject.Find("quantityText");
            GameObject unitValue = GameObject.Find("unitText");
            GameObject priceValue = GameObject.Find("priceText");

            // Customize props name of the prefab to find it when it will be create
            nameValue.name = nameValue.name + listItem.GetComponent<ItemListComponent>().name;
            quantityValue.name = quantityValue.name + listItem.GetComponent<ItemListComponent>().name;
            unitValue.name = unitValue.name + listItem.GetComponent<ItemListComponent>().name;
            priceValue.name = priceValue.name + listItem.GetComponent<ItemListComponent>().name;

            nameValue.GetComponent<RectTransform>().localScale = listItem.GetComponent<RectTransform>().localScale;
            quantityValue.GetComponent<RectTransform>().localScale = listItem.GetComponent<RectTransform>().localScale;
            unitValue.GetComponent<RectTransform>().localScale = listItem.GetComponent<RectTransform>().localScale;
            priceValue.GetComponent<RectTransform>().localScale = listItem.GetComponent<RectTransform>().localScale;

            nameValue.GetComponent<RectTransform>().sizeDelta = listItem.GetComponent<RectTransform>().sizeDelta;
            quantityValue.GetComponent<RectTransform>().sizeDelta = listItem.GetComponent<RectTransform>().sizeDelta;
            unitValue.GetComponent<RectTransform>().sizeDelta = listItem.GetComponent<RectTransform>().sizeDelta;
            priceValue.GetComponent<RectTransform>().sizeDelta = listItem.GetComponent<RectTransform>().sizeDelta;

            // Change text value of the list item
            nameValue.GetComponent<UnityEngine.UI.Text>().text = component.name;
            quantityValue.GetComponent<UnityEngine.UI.Text>().text = componentQte;
            unitValue.GetComponent<UnityEngine.UI.Text>().text = component.unit;
            priceValue.GetComponent<UnityEngine.UI.Text>().text = component.price + "€";

        }
    }

    public void GoToHomePage()
    {
        DontDestroyOnLoad(CONST);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 6);
    }

    //Get back  button function
    public void BackPage()
    {
        DontDestroyOnLoad(CONST);                                                   // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);       //Send the previous scene (ProjectSheet)
    }

    //Get back  button function
    public void NextPage()
    {
        DontDestroyOnLoad(CONST);                                                   // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);       //Send the next scene (estimationView_2)
    }

    //function called by clicking on the actualize button
    //actualize the prices with the discount value
    public void actualize()
    {
        string totalBeforeDiscountSt = CONST.GetComponent<CONST>().estimationPrice; //string that receives the price of the estimation  before any discount
        string discountSt = discount.GetComponent<UnityEngine.UI.Text>().text; //string that receives the discount value
        double discountInt = Convert.ToDouble(discountSt); //parsing the string discount to an int for the calculation of the estimation price after the discount
        double totalBeforeDiscountInt = estimationPrice; //parsing the totalBeforeDiscountSt to an int for the calculation of the estimation price after the discount

        double totalAfterDiscountInt = totalBeforeDiscountInt; //Initantiate the price after the discount as the start original price

        if (discountInt != 0) //if the price isn't null we calculate the discounted price
        {
            double mult = totalBeforeDiscountInt * discountInt; //firstly we multiply the original price with the discount number
            double sub = mult / 100; //secondly we divide the result per 100. It wil give the amount of the discount
            totalAfterDiscountInt = totalBeforeDiscountInt - sub; //we substract the amout of the discount from the original price, and we have the price after the discounting
        }

        totalBeforeDiscount.GetComponent<UnityEngine.UI.Text>().text = totalBeforeDiscountSt; //Shows the value of the original price
        discount.GetComponent<UnityEngine.UI.Text>().text = discountSt; //show the value of the discount 
        totalAfterDiscount.GetComponent<UnityEngine.UI.Text>().text = totalAfterDiscountInt.ToString(); //shows the final price

        StartCoroutine(UpdateEstimation(discountSt));
    }

    //Update the discount value of the selected estimation
    public IEnumerator UpdateEstimation(string discountSt)
    {
        var urlToUpdateEstimation = CONST.GetComponent<CONST>().url + "v1/updateestimationdiscount"; //http road to update the estimation by giving its Id

        WWWForm estimationForm = new WWWForm();                       // New form for web request
        estimationForm.AddField("estimationID", CONST.GetComponent<CONST>().selectedEstimationID);    // Add to the form the value of the ID of the project to get
        estimationForm.AddField("discount", discountSt);

        UnityWebRequest requestForEstimation = UnityWebRequest.Post(urlToUpdateEstimation, estimationForm);     // Create new WebRequest
        requestForEstimation.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");                      // Complete form with authentication datas
        requestForEstimation.SetRequestHeader("Authorization", CONST.GetComponent<CONST>().token);

        requestForEstimation.certificateHandler = new CONST.BypassCertificate();     // Bypass certificate for https

        yield return requestForEstimation.SendWebRequest(); //execute the web request 

        if (requestForEstimation.isNetworkError || requestForEstimation.isHttpError)
        {
            Debug.Log(requestForEstimation.error);
        }
    }
}
