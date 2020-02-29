using Assets.Scripts.Photon.Controller;
using CarCommon;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using LitJson;
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
        Dictionary<byte, object> parameters = operationResponse.Parameters;
        object jsonObject = null;
        parameters.TryGetValue((byte)ParamaterCode.ServerList, out jsonObject);
        List<CarCommon.Model.ServerProperty> serverLst = 
            JsonMapper.ToObject<List<CarCommon.Model.ServerProperty>>(jsonObject.ToString());
        LoginDelegate.Instance.InitServerList(serverLst);



    }

}
