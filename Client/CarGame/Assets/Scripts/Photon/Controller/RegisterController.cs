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
    class RegisterController : ControllerBase
    {
        public void Register(string userName, string password)
        {
            User user = new User() { UserName = userName, Password = password };
            string json = JsonMapper.ToJson(user);
            Dictionary<byte, object> parameters = new Dictionary<byte, object>();
            parameters.Add((byte)ParamaterCode.User, json);
            PhotonEngine.Instance.SendRequest(OperationCode.Register, parameters);
        }
        public override void OnOperationResponse(OperationResponse operationResponse)
        {
            switch (operationResponse.ReturnCode)
            {
                case (short)ReturnCode.Success:
                    RegisterDgt.Instance.RegisterSuccess();
                    break;
                case (short)ReturnCode.Fail:
                    RegisterDgt.Instance.RegisterFail();
                    break;
            }
        }
    }
}
