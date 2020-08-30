using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyAutoDiscovery : NetworkDiscovery
{
    private static MyAutoDiscovery _instance;

    public static MyAutoDiscovery instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MyAutoDiscovery>();
            }
            return _instance;
        }
    }

    private MyNetworkManager _networkManager;

    private void OnEnable()
    {
        _networkManager = GetComponent<MyNetworkManager>();
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        ServerManager.instance.HandlePing(data, fromAddress);
//        string ServerIp = fromAddress.Substring(fromAddress.LastIndexOf(':') + 1);
//#if !UNITY_EDITOR
//            // Tell the network transmitter the IP to request anchor data from if needed.
//            GenericNetworkTransmitter.instance.SetServerIP(ServerIp);
//#endif
    }
}
