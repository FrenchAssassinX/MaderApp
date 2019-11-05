using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
<<<<<<< HEAD
=======
using UnityEngine.UI;
>>>>>>> 26f194e22cb6451e4153689c131b7e56011bd382

public class Login : MonoBehaviour
{
    private string loginUrl = "http://madera-figueiredo.space/v1/login";
<<<<<<< HEAD

    void Start()
    {
        StartCoroutine(PostLogin());
    }

    IEnumerator PostLogin()
    {
        WWWForm form = new WWWForm();
        form.AddField("matricule", "THAZEA0898");
        form.AddField("password", "dede");
=======
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
>>>>>>> 26f194e22cb6451e4153689c131b7e56011bd382

        using (UnityWebRequest request = UnityWebRequest.Post(loginUrl, form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
<<<<<<< HEAD
                Debug.Log(request.error);
=======
                //request error, error message.
                Debug.Log(request.error);
                Debug.Log("erreur dans l'identification");
                string inf = "erreur dans l'identification";
                infoCo.text = inf;
>>>>>>> 26f194e22cb6451e4153689c131b7e56011bd382
            }
            else
            {
                if (request.isDone)
                {
                    string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
<<<<<<< HEAD
                    Debug.Log(jsonResult);
                    RootObject entity = JsonUtility.FromJson<RootObject>(jsonResult);

                    //User user = entity.user;

                    Debug.Log(entity.user.email);

                    /*foreach (RootObject rootObject in entities)
                    {
                        Debug.Log(rootObject.name);
                    }*/
                }
            }
        }
    }
}

=======
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
>>>>>>> 26f194e22cb6451e4153689c131b7e56011bd382
