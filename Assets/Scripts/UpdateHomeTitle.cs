using UnityEngine;

public class UpdateHomeTitle : MonoBehaviour
{
    /* Game object to changing home message with the name of the connected user */
    public GameObject homeText; 

    void Start()
    {
        homeText.GetComponent<UnityEngine.UI.Text>().text = "Bienvenue Nicolas";
    }

    void Update()
    {
        
    }
}
