using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public class MessageManager : NetworkBehaviour
{

    public UnityEvent OnReset;
    public UnityEvent OnSpatialMeshToggle;
    public UnityEvent OnCalibrationModeToggle;
    public UnityEvent OnDebugModeToggle;
    public StringEvent OnLogMessage;
    public UnityEvent OnPreviousStage;
    public UnityEvent OnNextStage;
    public StringArrEvent OnLogDetail;
    public FloatEvent OnFPS;
    public UnityEvent OnSave;
    public UnityEvent OnStartAutorun;
    public UnityEvent OnStopAutorun;

    private MyNetworkManager _networkManager;

    public static short resetMessage = 5555;
    public static short logMessage = 5556;
    public static short spatialMeshToggleMessage = 5557;
    public static short calibrationModeToggleMessage = 5558;
    public static short debugModeToggleMessage = 5559;
    public static short previousStageMessage = 5560;
    public static short nextStageMessage = 5561;
    public static short logDetailMessage = 5562;
    public static short fpsMessage = 5563;
    public static short saveMessage = 5564;
    public static short startAutoRunMessage = 5565;
    public static short stopAutoRunMessage = 5566;

    private void OnEnable()
    {
        _networkManager = FindObjectOfType<MyNetworkManager>();
    }

    private void Start()
    {
        NetworkServer.RegisterHandler(resetMessage, ResetMessage);
        NetworkServer.RegisterHandler(spatialMeshToggleMessage, ToggleSpatialMesh);
        NetworkServer.RegisterHandler(calibrationModeToggleMessage, ToggleCalibrationMode);
        NetworkServer.RegisterHandler(debugModeToggleMessage, ToggleDebugMode);
        NetworkServer.RegisterHandler(previousStageMessage, PreviousStage);
        NetworkServer.RegisterHandler(nextStageMessage, NextStage);
        NetworkServer.RegisterHandler(saveMessage, Save);
        NetworkServer.RegisterHandler(startAutoRunMessage, StartAutorun);
        NetworkServer.RegisterHandler(stopAutoRunMessage, StopAutorun);

        NetworkServer.RegisterHandler(35, ignore);
    }

    public void RegisterClientHandlers()
    {
        _networkManager.client.RegisterHandler(logMessage, LogMessage);
        _networkManager.client.RegisterHandler(logDetailMessage, LogDetail);
        _networkManager.client.RegisterHandler(fpsMessage, FPS);
    }

    public void SendResetMessage()
    {
        _networkManager.client.Send(resetMessage, new EmptyMessage());
        Logger.instance.LogMessage("Send Reset");
    }

    public void SendToggleSpatialMeshMessage()
    {
        _networkManager.client.Send(spatialMeshToggleMessage, new EmptyMessage());
        Logger.instance.LogMessage("Send Toggle Spatial Mesh");
    }

    public void SendToggleCalibrationModeMessage()
    {
        _networkManager.client.Send(calibrationModeToggleMessage, new EmptyMessage());
        Logger.instance.LogMessage("Send Toggle Calibration Mode");
    }

    public void SendToggleDebugModeMessage()
    {
        _networkManager.client.Send(debugModeToggleMessage, new EmptyMessage());
        Logger.instance.LogMessage("Send Toggle Debug Mode");
    }

    public void SendPreviousStageMessage()
    {
        _networkManager.client.Send(previousStageMessage, new EmptyMessage());
        Logger.instance.LogMessage("Send Previous Stage");
    }

    public void SendNextStageMessage()
    {
        _networkManager.client.Send(nextStageMessage, new EmptyMessage());
        Logger.instance.LogMessage("Send Next Stage");
    }

    public void SendLogMessage(string str)
    {
        if (_networkManager.client != null)
            _networkManager.SendToClients(logMessage, new StringMessage(str));
    }

    public void SendLogDetailMessage(string[] strArr)
    {
        if (_networkManager.client != null)
            _networkManager.SendToClients(logDetailMessage, new StringArrMessage(strArr));
    }

    public void SendFPSMessage(float fps)
    {
        if (_networkManager.client != null)
            _networkManager.SendToClients(fpsMessage, new FloatMessage(fps));
    }

    public void SendRequestDetailsMessage()
    {
        _networkManager.client.Send(saveMessage, new EmptyMessage());
    }

    public void SendStartAutorunMessage()
    {
        _networkManager.client.Send(startAutoRunMessage, new EmptyMessage());
    }

    public void SendStopAutorunMessage()
    {
        _networkManager.client.Send(stopAutoRunMessage, new EmptyMessage());
    }

    private void ResetMessage(NetworkMessage msg)
    {
        OnReset.Invoke();
    }

    private void ToggleSpatialMesh(NetworkMessage msg)
    {
        OnSpatialMeshToggle.Invoke();
    }

    private void ToggleCalibrationMode(NetworkMessage msg)
    {
        OnCalibrationModeToggle.Invoke();
    }

    private void ToggleDebugMode(NetworkMessage msg)
    {
        OnDebugModeToggle.Invoke();
    }

    private void LogMessage(NetworkMessage msg)
    {
        OnLogMessage.Invoke(msg.ReadMessage<StringMessage>().value);
    }

    private void PreviousStage(NetworkMessage msg)
    {
        OnPreviousStage.Invoke();
    }

    private void NextStage(NetworkMessage msg)
    {
        OnNextStage.Invoke();
    }

    private void LogDetail(NetworkMessage msg)
    {
        OnLogDetail.Invoke(msg.ReadMessage<StringArrMessage>().value);
    }

    private void FPS(NetworkMessage msg)
    {
        OnFPS.Invoke(msg.ReadMessage<FloatMessage>().value);
    }

    private void Save(NetworkMessage msg)
    {
        OnSave.Invoke();
    }

    private void StartAutorun(NetworkMessage msg)
    {
        OnStartAutorun.Invoke();
    }

    private void StopAutorun(NetworkMessage msg)
    {
        OnStopAutorun.Invoke();
    }

    private void ignore(NetworkMessage msg)
    {
    }
}

public class EmptyMessage : MessageBase { }

public class StringMessage : MessageBase
{
    public string value;

    public StringMessage(string str)
    {
        value = str;
    }

    public StringMessage() { }
}

public class StringArrMessage : MessageBase
{
    public string[] value;

    public StringArrMessage(string[] strArr)
    {
        value = strArr;
    }

    public StringArrMessage()
    { }
}

public class FloatMessage : MessageBase
{
    public float value;

    public FloatMessage(float f)
    {
        value = f;
    }

    public FloatMessage()
    { }
}