using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TechnicalFolder : MonoBehaviour
{
    public GameObject CONST;                                    // CONST object contains server route, token and user infos

    void Start()
    {
        CONST = GameObject.Find("CONST");
        Debug.Log("Const:" + CONST);
    }

    void Update()
    {
        
    }

    // Function to Go to EstimationView scene
    public void GoToEstimationModality()
    {
        DontDestroyOnLoad(CONST);                                                   // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);       // Load Estimation Modality scene
    }

    // Function to return to EstimationView_2 scene
    public void ReturnToEstimationView_2()
    {
        DontDestroyOnLoad(CONST);                                                   // Keep the CONST object between scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);       // Load EstimationView_2 scene
    }

}
