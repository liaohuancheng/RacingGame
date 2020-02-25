using CarCommon;
using ExitGames.Client.Photon;
using UnityEngine;

namespace Assets.Scripts.Photon.Controller
{
    public abstract class ControllerBase : MonoBehaviour
    {
        public OperationCode opCode;

        public virtual void Start()
        {
            PhotonEngine.Instance.RegisterController(opCode, this);
        }

        public abstract void OnOperationResponse(OperationResponse operationResponse );

        public virtual void OnDestroy()
        {
            PhotonEngine.Instance.UnRegisterController(opCode);
        }

    }
}
