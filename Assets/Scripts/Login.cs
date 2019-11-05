using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    private string loginUrl = "http://madera-figueiredo.space/v1/login";
    public InputField username;
    public InputField password;
    public Button connexion;
    public Text infoCo = null;
    
    
    

    void Start()
    {
        Button btn = connexion.GetComponent<Button>();
        btn.onClick.AddListener(SendConnexion);
        
    }

    void SendConnexion()
    {
        //When you click on the login button you start this function
        StartCoroutine(PostLogin());
    }
    
    IEnumerator PostLogin()
    {
        WWWForm form = new WWWForm();
        form.AddField("matricule", username.text);
        form.AddField("password", password.text);

        using (UnityWebRequest request = UnityWebRequest.Post(loginUrl, form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                //request error, error message.
                Debug.Log(request.error);
                Debug.Log("erreur dans l'identification");
                string inf = "erreur dans l'identification";
                infoCo.text = inf;
            }
            else
            {
                if (request.isDone)
                {
                    string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                    //delete soon
                    Debug.Log(jsonResult);
                    RootObject entity = JsonUtility.FromJson<RootObject>(jsonResult);
                    //delete soon
                    Debug.Log("bienvenu");
                    //going into home.

                }
                
            }
            }
        }

    }