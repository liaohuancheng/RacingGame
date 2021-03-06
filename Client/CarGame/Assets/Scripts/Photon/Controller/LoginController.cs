﻿using System;
using ExitGames.Client.Photon;
using LitJson;
using System.Collections.Generic;
using CarCommon.Model;
using CarCommon;
using UnityEngine;

namespace Assets.Scripts.Photon.Controller
{
    class LoginController : ControllerBase
    {
        public override OperationCode OpCode
        {
            get { return OperationCode.Login; }
        }

        public void Login(string userName, string password)
        {
            User user = new User() { UserName = userName, Password = password };
            string json = JsonMapper.ToJson(user);
            Dictionary<byte, object> parameters = new Dictionary<byte, object>();
            parameters.Add((byte)ParamaterCode.User, json);
            PhotonEngine.Instance.SendRequest(OperationCode.Login, parameters);
        }
        public override void OnOperationResponse(OperationResponse operationResponse)
        {
            switch (operationResponse.ReturnCode)
            {
                case (short)ReturnCode.Success:
                    Debug.Log("登录成功");
                    User user = ParameterTool.GetParameter<User>(operationResponse.Parameters, ParamaterCode.User);
                    Debug.Log(user.ID);
                    LoginDelegate.Instance.LoginSuccess(user);
                    break;
                case (short)ReturnCode.Fail:
                    Debug.Log("登录失败");
                    break;
            }
        }
    }
}
