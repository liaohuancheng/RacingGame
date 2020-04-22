using CarCommon;
using CarCommon.Model;
using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Photon.Controller
{
    class BattleController : ControllerBase
    {
        private static BattleController _instance = null;
        private BattleController()
        {

        }
        public static BattleController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new BattleController();
                return _instance;
            }
        }
        public override OperationCode OpCode { get { return OperationCode.Battle; } }

        public void SendPosition(Vector3 position, Vector3 rotation)
        {
            Dictionary<byte, object> paramaters = new Dictionary<byte, object>();
            PlayerGameData playerData = new PlayerGameData
            {
                Username = UserInfo.Instance.UserName,
                ID = UserInfo.Instance.Id
            };
            playerData.Postion = new Vector3Data
            {
                x = position.x,
                y = position.y,
                z = position.z
            };
            playerData.Rotation = new Vector3Data
            {
                x = rotation.x,
                y = rotation.y,
                z = rotation.z
            };
            ParameterTool.AddParameter(paramaters, ParamaterCode.BattleOperationCode, BattleOperationCode.SyncPosition, false);
            ParameterTool.AddParameter(paramaters, ParamaterCode.PlayerGameData, playerData);
            PhotonEngine.Instance.SendRequest(OpCode, paramaters);
        }

        public override void OnOperationResponse(OperationResponse operationResponse)
        {

        }

        public void OnPlayerFinshGame(float finishTime)
        {
            Debug.Log("到达终点");
            Dictionary<byte, object> paramaters = new Dictionary<byte, object>();
            ParameterTool.AddParameter(paramaters, ParamaterCode.First, finishTime, false);
            ParameterTool.AddParameter(paramaters, ParamaterCode.BattleOperationCode, BattleOperationCode.PlayerFinishGame, false);
            PhotonEngine.Instance.SendRequest(OpCode, paramaters);
        }
    }
}
