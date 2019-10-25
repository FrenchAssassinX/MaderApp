using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour
{
    private const int MAX_USERS = 100;
    private const int PORT = 26000;                                         // Port using for the connection
    private const int WEB_PORT = 26001;                                     // Port using for the connection with browser
    private const string SERVER_IP = "127.0.0.1";

    private byte reliableChannel;
    private int hostID;
    private byte error;                                                     // Identify network error if something goes wrong

    private bool isServerStarted;                                           // To knows is the server has start to run

    #region Monobehaviour
    public void Start()
    {
        DontDestroyOnLoad(gameObject);                                      // Avoid to destroy Client object every time we changing scene on Unity
        Init();
    }
    #endregion

    public void Init()
    {
        NetworkTransport.Init();

        ConnectionConfig cc = new ConnectionConfig();                       // Defines the roads
        reliableChannel = cc.AddChannel(QosType.Reliable);                  // QosType.Reliable is a type of message guaranted to be delivered

        HostTopology topology = new HostTopology(cc, MAX_USERS);            // HostTopology defines how many users at the same time can use the road passed in parameter
    

        // Client only for the server
        hostID = NetworkTransport.AddHost(topology, 0);                     // Host only need the topology

#if UNITY_WEBGL && !UNITY_EDITOR
        // Web client
        NetworkTransport.Connect(hostID, SERVER_IP, WEB_PORT, 0, out error);    // Connecting to the server
        Debug.Log("Connecting from Web")
#else
        // Standalone client
        NetworkTransport.Connect(hostID, SERVER_IP, PORT, 0, out error);    // Connecting to the server
        Debug.Log("Connecting from Standalone");
#endif

        Debug.Log(string.Format("Attempting to connect on {0}....", SERVER_IP));
        isServerStarted = true;
    }

    public void Shutdown()
    {
        isServerStarted = false;
        NetworkTransport.Shutdown();
    }
}
