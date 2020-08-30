using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager
{
    public NetworkRole role;
    public GameObject[] networkObjects;
    public UnityEvent OnConnectedToServer;
    public int deviceIdLength = 6;

    private NetworkDiscovery _discovery;
    private List<int> _connectedIDs;

    private void OnEnable()
    {
        _discovery = GetComponent<NetworkDiscovery>();
        _connectedIDs = new List<int>();
    }

    private void Start()
    {
        _discovery.broadcastData = ServerID();
        _discovery.Initialize();

        if (role == NetworkRole.Server)
            StartCoroutine(InitServer());
        else if (role == NetworkRole.Client)
        {
            _discovery.StartAsClient();
        }

    }

    private IEnumerator InitServer()
    {
        StartHost();

        //Wait a frame to initialize broadcasting
        yield return null;

        _discovery.StartAsServer();

        //Initialize UWP networking
        GenericNetworkTransmitter.instance.ConfigureAsServer();
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        OnConnectedToServer.Invoke();
        Logger.instance.LogMessage("Client connected to: " + conn.address);
        foreach (GameObject obj in networkObjects)
            obj.SetActive(true);
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        _connectedIDs.Add(conn.connectionId);
        Logger.instance.LogMessage("Server id " + ServerID() + " connected to: " + conn.address);
        NetworkServer.SetClientReady(conn);
        NetworkServer.SpawnObjects();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        Logger.instance.LogMessage("Disconnected");
        ServerManager.instance.ConnectToServer(string.Empty);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        _connectedIDs.Remove(conn.connectionId);
        Logger.instance.LogMessage("Server disconnected: " + conn.address);
    }

    public void SendToClients(short messageId, MessageBase msg)
    {
        foreach (int id in _connectedIDs)
            NetworkServer.SendToClient(id, messageId, msg);
    }

    public string ServerID()
    {
        string id = SystemInfo.deviceUniqueIdentifier;
        return id.Substring(id.Length - deviceIdLength - 1, deviceIdLength);
    }

}

public enum NetworkRole
{
    Server,
    Client
}
