using System;
using System.Collections.Generic;
using CarCommon;
using CarCommon.Model;
using ExitGames.Client.Photon;
using UnityEngine;

namespace Assets.Scripts.Photon.Controller
{
    class SyncPositionController : ControllerBase
    {
        private static SyncPositionController _instance = null;
        private SyncPositionController()
        {

        }
        public static SyncPositionController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SyncPositionController();
                return _instance;
            }
        }
        public override OperationCode OpCode { get { return OperationCode.SyncPosition; } }

        public void SendPosition(Vector3 position)
        {
            Dictionary<byte, object> paramaters = new Dictionary<byte, object>();
            PlayerGameData playerData = new PlayerGameData
            {
                Username = UserInfo.Instance.UserName
            };
            playerData.pos = new Vector3Data
            {
                x = position.x,
                y = position.y,
                z = position.z
            };
            ParameterTool.AddParameter(paramaters, ParamaterCode.PlayerGameData, playerData);
            PhotonEngine.Instance.SendRequest(OpCode, paramaters);
        }

        public override void OnOperationResponse(OperationResponse operationResponse)
        {
            
        }
    }
}
