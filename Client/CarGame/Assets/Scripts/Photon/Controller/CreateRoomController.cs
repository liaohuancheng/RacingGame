using System;
using System.Collections.Generic;
using CarCommon;
using CarCommon.Model;
using ExitGames.Client.Photon;
using LitJson;

namespace Assets.Scripts.Photon.Controller
{
    public class CreateRoomController : ControllerBase
    {
        public override OperationCode OpCode
        {
            get { return OperationCode.CreateRoom; }
        }

        public void CreateRoom(string roomName)
        {
            Dictionary<byte, object> parameters = new Dictionary<byte, object>();
            ParameterTool.AddParameter(parameters, ParamaterCode.First, roomName, false);
            PhotonEngine.Instance.SendRequest(OperationCode.CreateRoom, parameters);
        }
        public override void OnOperationResponse(OperationResponse operationResponse)
        {
            UIController.Instance.HideUI("CreateRoomPanel");
            UIController.Instance.ShowUI("RoomRoot");
            var usrLst = ParameterTool.GetParameter<List<User>>(operationResponse.Parameters, ParamaterCode.UserLst);
            RoomDgt.Instance.CreatePlayerLSt(usrLst);
        }
    }
}
