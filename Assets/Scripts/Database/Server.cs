using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
    private const int MAX_USERS = 100;
    private const int PORT = 26000;                                 // Port using for the connection
    private const int WEB_PORT = 26001;                             // Port using for the connection with browser

    private byte reliableChannel;
    private int hostID;
    private int webHostID;

    private bool isServerStarted;                                   // To knows is the server has start to run

    private Mongo database;

    #region Monobehaviour
    public void Start()
    {
        DontDestroyOnLoad(gameObject);                              // Avoid to destroy Server object at every start
        Init();
    }
    #endregion

    public void Init()
    {
        database = new Mongo();
        database.Init();

        NetworkTransport.Init();

        ConnectionConfig cc = new ConnectionConfig();               // Defines the roads
        reliableChannel = cc.AddChannel(QosType.Reliable);          // QosType.Reliable is a type of message guaranted to be delivered

        HostTopology topology = new HostTopology(cc, MAX_USERS);    // HostTopology defines how many users at the same time can use the road passed in parameter


        // Code only for the server
        hostID = NetworkTransport.AddHost(topology, PORT, null);             // Host need the topology, the connection port and need to know if we want to keep the server for a specific IP address
        webHostID = NetworkTransport.AddWebsocketHost(topology, WEB_PORT, null);    // Allows to connect to tyhe server using a browser

        Debug.Log(string.Format("Opening connection on port {0} and webport {1}", PORT, WEB_PORT));
        isServerStarted = true;
    }

    public void Shutdown()
    {
        isServerStarted = false;
        NetworkTransport.Shutdown();
    }
}
