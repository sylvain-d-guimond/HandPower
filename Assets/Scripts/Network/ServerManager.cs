using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ServerManager : Singleton<ServerManager> {

    public StringArrEvent OnServerListChanged;
    public StringEvent OnServerChanged;
    public IntEvent OnNumberOfServersChanged;
    public string currentServer;

    private Dictionary<string, ServerInfo> _servers;
    private bool _monitoring = true;

    private void Awake()
    {
        _servers = new Dictionary<string, ServerInfo>();
    }

    private void Start()
    {
        UpdateServerList();
        StartCoroutine(ServerListMonitor(MyAutoDiscovery.instance.broadcastInterval / 1000));
    }

    public void HandlePing(string id, string address)
    {
        if (_servers.ContainsKey(id))
            _servers[id].lastPing = Time.time;
        else
        {
            _servers.Add(id, new ServerInfo(id, address));
            UpdateServerList();
        }

        if (_servers.Count.Equals(1) && currentServer.Equals(string.Empty))
            ConnectToServer(id);
    }

    private void UpdateServerList()
    {
        List<string> list = new List<string>();

        foreach (ServerInfo info in _servers.Values)
            list.Add(info.serverId);

        OnServerListChanged.Invoke(list.ToArray());
        OnNumberOfServersChanged.Invoke(list.Count);
    }

    public void ConnectToServer(string id)
    {
        if (!currentServer.Equals(string.Empty) && !id.Equals(string.Empty))
            MyNetworkManager.singleton.client.Disconnect();
        if (!id.Equals(string.Empty))
        {
            MyNetworkManager.singleton.networkAddress = _servers[id].address;
            MyNetworkManager.singleton.StartClient();
        }
        currentServer = id;
        OnServerChanged.Invoke(id);
    }

    private IEnumerator ServerListMonitor(float frequency)
    {
        List<string> removeList = new List<string>();
        bool changed = false;
        while(_monitoring)
        {
            yield return new WaitForSeconds(frequency);

            foreach (ServerInfo server in _servers.Values)
                if (server.lastPing + (frequency * 2) < Time.time)
                {
                    removeList.Add(server.serverId);
                    changed = true;
                }

            foreach (string id in removeList)
                _servers.Remove(id);
            removeList.Clear();

            if (changed)
            {
                UpdateServerList();
                changed = false;
            }
        }
    }

    private void OnApplicationQuit()
    {
        _monitoring = false;
    }
}

public class ServerInfo
{
    public string serverId;
    public float lastPing;
    public string address;

    public ServerInfo(string id, string address)
    {
        this.serverId = id;
        this.address = address;
        this.lastPing = Time.time;
    }
}