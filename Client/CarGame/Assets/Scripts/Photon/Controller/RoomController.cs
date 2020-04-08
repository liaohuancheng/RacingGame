using System.Collections.Generic;
using CarCommon;
using CarCommon.Model;
using ExitGames.Client.Photon;

namespace Assets.Scripts.Photon.Controller
{
    class RoomController : ControllerBase
    {
        private static RoomController _instance = null;
        private RoomController()
        {

        }
        public static RoomController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new RoomController();
                return _instance;
            }
        }
        public override OperationCode OpCode { get => OperationCode.Room; }

        public void EnterRoom(int userID, int roomID)
        {
            OperationRequest request = new OperationRequest();
            Dictionary<byte, object> paramaters = new Dictionary<byte, object>();
            ParameterTool.AddParameter(paramaters, ParamaterCode.First, userID, false);
            ParameterTool.AddParameter(paramaters, ParamaterCode.Second, roomID, false);
            ParameterTool.AddParameter(paramaters, ParamaterCode.RoomOperationCode, RoomOperationCode.EnterRoom,false);
            PhotonEngine.Instance.SendRequest(OperationCode.Room, paramaters);
        }

        public void ExitRoom(int userID)
        {
            Dictionary<byte, object> paramaters = new Dictionary<byte, object>();
            ParameterTool.AddParameter(paramaters, ParamaterCode.First, userID, false);
            ParameterTool.AddParameter(paramaters, ParamaterCode.RoomOperationCode, RoomOperationCode.ExitRoom, false);
            PhotonEngine.Instance.SendRequest(OperationCode.Room, paramaters);
        }

        public override void OnOperationResponse(OperationResponse operationResponse)
        {
            var roomOperationCode = ParameterTool.GetParameter<RoomOperationCode>(operationResponse.Parameters, ParamaterCode.RoomOperationCode, false);
            switch (roomOperationCode)
            {
                case RoomOperationCode.EnterRoom:
                    var users = ParameterTool.GetParameter<List<User>>(operationResponse.Parameters, ParamaterCode.UserLst);
                    RoomListDgt.Instance.EnterSuccess(users);
                    break;
                case RoomOperationCode.ExitRoom:
                    break;
            }
        }
    }
}
