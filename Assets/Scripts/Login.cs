using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    // Const object and properties to transfer datas on every scenes of the project
    public GameObject CONST;
    private string url;
    private string loginUrl = "v1/login";       // Specific url for this scene

    public InputField username;                 /*** Text infos UI Elements ***/
    public InputField password;                 /*                            */
    public Button connection;                   /******************************/
    public GameObject textConnectionError;      // Error message display if connection failed

    void Start()
    {
        // Get the url of CONST
        url = CONST.GetComponent<CONST>().url;

        // Disable error message by default
        textConnectionError.transform.gameObject.SetActive(false);

        // Declare button and the behaviour of the button
        Button btn = connection.GetComponent<Button>();
        btn.onClick.AddListener(SendConnection);
    }

    /* Function starting when the connection button will be pressed */
    void SendConnection()
    {
        //When you click on the login button you start this function
        StartCoroutine(PostLogin());
    }

    /* Function to connect user with the database */
    IEnumerator PostLogin()
    {
        WWWForm form = new WWWForm();                   // New form for web request
        form.AddField("matricule", username.text);      // Add to the form the value of the UI Element 'Matricule'
        form.AddField("password", password.text);       // Add to the form the value of the UI Element 'Password'

        /* New webrequest with: CONST url, local url and the form */
        using (UnityWebRequest request = UnityWebRequest.Post(url + loginUrl, form))
        {
            yield return request.SendWebRequest();

            // If connection failed
            if (request.isNetworkError || request.isHttpError)  
            {
                // Display UI Element error message 
                textConnectionError.transform.gameObject.SetActive(true);
            }
            // If connection succeeded
            else
            {
                if (request.isDone)
                {
                    // The database return a JSON file of all user infos
                    string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                    // Create a root object thanks to the JSON file
                    RootObject entity = JsonUtility.FromJson<RootObject>(jsonResult);
                    // Get token and users info before changing scene
                    CONST.GetComponent<CONST>().token = entity.token.ToString();
                    CONST.GetComponent<CONST>().userID = entity.user._id.ToString();
                    CONST.GetComponent<CONST>().userName = entity.user.prenom.ToString();
                    
                    // Keep the CONST gameObject between scenes
                    DontDestroyOnLoad(CONST.transform);
                    // Go to Home Scene
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }

            }
        }
    }

}