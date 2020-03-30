using CarCommon;
using ExitGames.Client.Photon;
using UnityEngine;

namespace Assets.Scripts.Photon.Controller
{
    public abstract class ControllerBase
    {
        public abstract OperationCode OpCode { get; }
        public ControllerBase()
        {
            PhotonEngine.Instance.RegisterController(OpCode, this);
        }
        public abstract void OnOperationResponse(OperationResponse operationResponse );

        public virtual void OnDestroy()
        {
            PhotonEngine.Instance.UnRegisterController(OpCode);
        }

    }
}
