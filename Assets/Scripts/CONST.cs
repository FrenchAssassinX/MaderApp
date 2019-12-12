using System.Collections.Generic;
using UnityEngine;

public class CONST : MonoBehaviour
{
    public string url = "https://madera-figueiredo.space/";
    public string token;
    public string userID;
    public string userName;
    public string selectedProjectID;
    public string selectedEstimationID;
    public string customerName;
    public string projectName;
    public string estimationPrice;
    public string estimationDiscount;
    public string state;
    public List<string> listModulesCreated = new List<string>();
    public Dictionary<string, string> dictComponentsForModule = new Dictionary<string, string>();
}
