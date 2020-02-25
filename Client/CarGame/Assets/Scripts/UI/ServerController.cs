using Assets.Scripts.Photon.Controller;
using CarCommon;
using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerController : ControllerBase
{
    public override void Start()
    {
        base.Start();
        PhotonEngine.Instance.onConnectedToServer += GetServerList;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        PhotonEngine.Instance.onConnectedToServer -= GetServerList;
    }

    public void GetServerList()
    {
        PhotonEngine.Instance.SendRequest(OperationCode.GetServer, new Dictionary<byte, object>());
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        throw new System.NotImplementedException();
    }

}
