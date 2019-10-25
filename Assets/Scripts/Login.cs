using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Login : MonoBehaviour
{
    private string loginUrl = "http://madera-figueiredo.space/v1/login";

    void Start()
    {
        StartCoroutine(PostLogin());
    }

    IEnumerator PostLogin()
    {
        WWWForm form = new WWWForm();
        form.AddField("matricule", "THAZEA0898");
        form.AddField("password", "dede");

        using (UnityWebRequest request = UnityWebRequest.Post(loginUrl, form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                if (request.isDone)
                {
                    string jsonResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
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

