using System.Collections.Generic;
using CarCommon;
using CarCommon.Model;
using ExitGames.Client.Photon;

namespace Assets.Scripts.Photon.Controller
{
    class GetRoomController : ControllerBase
    {
        private static GetRoomController _instance = null;
        private GetRoomController()
        {

        }

        public static GetRoomController Instance
        {
            get { return _instance ?? (_instance = new GetRoomController()); }
        }
        public override OperationCode OpCode
        {
            get { return OperationCode.GetRoomLst; }
        }

        public void GetRoomLst()
        {
            Dictionary<byte, object> parameters = null;
            PhotonEngine.Instance.SendRequest(OperationCode.GetRoomLst, parameters);
        }

        public override void OnOperationResponse(OperationResponse operationResponse)
        {
            List<RoomProperty> roomList =  ParameterTool.GetParameter<List<RoomProperty>>(operationResponse.Parameters, ParamaterCode.RoomList);
            RoomListDgt.Instance.CreateRoomList(roomList);
        }
    }
}
