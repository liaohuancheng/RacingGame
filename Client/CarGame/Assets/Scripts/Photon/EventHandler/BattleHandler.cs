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
                case BattleOperationCode.GameOver:
                    HandleGameOver();
                    break;
            }
        }

        private void HandleGameOver()
        {
            PhotonEngine.Instance.StartCoroutine(ChangeToStartMenu());
        }

        public IEnumerator ChangeToStartMenu()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(0);
            operation.allowSceneActivation = false;
            //ProgressBar.Instance.Show(operation);
            while (!operation.isDone)
            {
                yield return new WaitForSeconds(0.1f);
                if (operation.progress >= 0.9f)//加载进度大于等于0.9时说明加载完毕
                {
                    operation.allowSceneActivation = true;//手动赋值为true（此值为true时，isDone自动会跟着变
                }
                if (operation.isDone)
                {
                    break;
                }
            }
            UIController.Instance.HideUI("LoginPanelRoot");
            UIController.Instance.ShowUI("RoomRoot");
            RoomDgt.Instance.Refresh();
            yield return null;
            
        }

        private void HandleSyncPostion(EventData eventData)
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
