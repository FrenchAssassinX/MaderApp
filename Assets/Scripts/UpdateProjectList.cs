using UnityEngine;
using UnityEngine.SceneManagement;

public class UpdateProjectList : MonoBehaviour
{
    public GameObject CONST;
    public GameObject listItemPrefab;
    public GameObject gridList;

    void Start()
    {
        CONST = GameObject.Find("CONST");
        gridList = GameObject.Find("GridList");

        // TODO: Connection to bdd
        for (int i = 0; i < 10; i++)
        {
            GameObject listItem = Instantiate(listItemPrefab, 
                                                new Vector3(
                                                    gridList.transform.position.x,
                                                    gridList.transform.position.y - 500,
                                                    gridList.transform.position.z
                                                ), 
                                                Quaternion.identity
            );
            listItem.transform.SetParent(gridList.transform);
        }
    }

    void Update()
    {
        
    }

    public void GoBackToMenu()
    {
        DontDestroyOnLoad(CONST);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2); //Go back to Home Scene
    }
}
