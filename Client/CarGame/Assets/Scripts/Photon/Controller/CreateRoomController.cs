using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarCommon;
using CarCommon.Model;
using ExitGames.Client.Photon;
using LitJson;

namespace Assets.Scripts.Photon.Controller
{
    class CreateRoomController : ControllerBase
    {
        public void CreateRoom()
        {
            RoomProperty roomProperty = new RoomProperty() { RoomOwnerName = "Test"};
            string json = JsonMapper.ToJson(roomProperty);
            Dictionary<byte, object> parameters = new Dictionary<byte, object>();
            parameters.Add((byte)ParamaterCode.RoomProperty, json);
            PhotonEngine.Instance.SendRequest(OperationCode.CreateRoom, parameters);
        }
        public override void OnOperationResponse(OperationResponse operationResponse)
        {
            throw new NotImplementedException();
        }
    }
}
