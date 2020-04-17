using System;
using CarCommon;
using ExitGames.Client.Photon;
using CarCommon.Model;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts.CarContorl;

namespace Assets.Scripts.Photon.EventHandler
{
    class BattleHandler : HandlerBase
    {
        public override void OnEventHandle(EventData eventData)
        {
            var operationCode = ParameterTool.GetParameter<BattleOperationCode>(eventData.Parameters, ParamaterCode.BattleOperationCode, false);
            switch (operationCode)
            {
                case BattleOperationCode.SyncPosition:
                    HandleSyncPostion(eventData);
                    break;
            }
        }

        

        public void HandleSyncPostion(EventData eventData)
        {
            PlayerGameData playerGameData = ParameterTool.GetParameter<PlayerGameData>(eventData.Parameters, ParamaterCode.PlayerGameData);
            int Id = playerGameData.ID;
            CarContainer netCar;

            GameMgr.Instance.CarDic.TryGetValue(Id, out netCar);

            if(netCar == null)
            {
                return;
            }
            Vector3 nowPostion = new Vector3()
            {
                x = playerGameData.Postion.x,
                y = playerGameData.Postion.y,
                z = playerGameData.Postion.z,
            };
            Vector3 nowRotation = new Vector3()
            {
                x = playerGameData.Rotation.x,
                y = playerGameData.Rotation.y,
                z = playerGameData.Rotation.z,
            };
            netCar.car.NetForecastInfo(nowPostion, nowRotation);
        }

        
    }
}
