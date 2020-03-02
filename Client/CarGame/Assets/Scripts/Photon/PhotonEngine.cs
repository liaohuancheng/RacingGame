using Assets.Scripts.Photon.Controller;
using CarCommon;
using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonEngine : MonoBehaviour ,IPhotonPeerListener
{
    public ConnectionProtocol protocol = ConnectionProtocol.Tcp;
    public string serverAddress = "127.0.0.1:4530";
    public string applicationName = "CarServer";

    public delegate void OnConnectedToServerEvent();
    public event OnConnectedToServerEvent onConnectedToServer;

    private Dictionary<byte, ControllerBase> controllers = new Dictionary<byte, ControllerBase>();

    private PhotonPeer peer;
    private bool isConnected = false;

    public static PhotonEngine _instance;

    public static PhotonEngine Instance
    {
        get
        {
            return _instance;
        }
    }

    public void RegisterController(OperationCode code, ControllerBase controller)
    {
        controllers.Add((byte)code, controller);
    }

    public void UnRegisterController(OperationCode code)
    {
        controllers.Remove((byte)code);
    }

    public void SendRequest(OperationCode operationCode, Dictionary<byte,object> parameters)
    {
        Debug.Log("发起请求 operationCode:" + operationCode);
        peer.OpCustom((byte)operationCode, parameters, true);
    }

    void Awake()
    {
        _instance = this;
        peer = new PhotonPeer(this, protocol);
        peer.Connect(serverAddress, applicationName);
    }

    // Update is called once per frame
    void Update()
    {
        if (peer != null)
            peer.Service();
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log(level + ":" + message);
    }

    public void OnEvent(EventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        ControllerBase controller;
        controllers.TryGetValue(operationResponse.OperationCode, out controller);
        Debug.Log("opCode:" + operationResponse.OperationCode);
        if (controller != null)
        {
            controller.OnOperationResponse(operationResponse);
        }
        else
        {
            Debug.Log("Recevied Unknow Response OperationCode:" + operationResponse.OperationCode);
        }
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        Debug.Log("status changed:" + statusCode);
        switch (statusCode)
        {
            case StatusCode.Connect:
                isConnected = true;
                onConnectedToServer();
                break;
            default:
                isConnected = false;
                break;
        }
    }

    // Start is called before the first frame update
    
}
