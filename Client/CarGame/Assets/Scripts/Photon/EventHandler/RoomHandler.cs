using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarCommon;
using CarCommon.Model;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Photon.EventHandler
{
    class RoomHandler : HandlerBase
    {
        public override void OnEventHandle(EventData eventData)
        {
            var operationCode = ParameterTool.GetParameter<RoomOperationCode>(eventData.Parameters, ParamaterCode.RoomOperationCode, false);
            switch (operationCode)
            {
                case RoomOperationCode.EnterRoom:
                    HandleEnterRoom(eventData);
                    break;
                case RoomOperationCode.ExitRoom:
                    HandleExitRoom(eventData);
                    break;
                case RoomOperationCode.StartFight:
                    HandleStartFight(eventData);
                    break;
            }
        }

        private void HandleEnterRoom(EventData eventData)
        {
            List<User> users = ParameterTool.GetParameter<List<User>>(eventData.Parameters, ParamaterCode.UserLst);
            RoomDgt.Instance.CreatePlayerLSt(users);
        }

        private void HandleExitRoom(EventData eventData)
        {
            List<User> users = ParameterTool.GetParameter<List<User>>(eventData.Parameters, ParamaterCode.UserLst);
            RoomDgt.Instance.CreatePlayerLSt(users);
        }

        private void HandleStartFight(EventData eventData)
        {
            int PlayerCount = ParameterTool.GetParameter<int>(eventData.Parameters, ParamaterCode.First, false);
            List<User> userLst = ParameterTool.GetParameter<List<User>>(eventData.Parameters, ParamaterCode.UserLst);

            PhotonEngine.Instance.StartCoroutine(StartFight(PlayerCount, userLst));
        }

        public IEnumerator StartFight(int PlayerCount, List<User> userLst)
        {
            UIController.Instance.HideUI("RoomRoot");
            AsyncOperation operation = SceneManager.LoadSceneAsync(1);
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
            GameMgr.Instance.StartBattle(PlayerCount, userLst);
            yield return null;
        }
    }
}
