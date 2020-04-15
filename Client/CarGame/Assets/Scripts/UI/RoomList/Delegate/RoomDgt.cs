using Assets.Scripts;
using Assets.Scripts.Photon.Controller;
using CarCommon.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomDgt : MonoBehaviour
{
    private static RoomDgt _instance = null;
    public RoomView roomView;
    public RoomModel roomModel;
    public int SelectingRoomId { get; set; }
    public static RoomDgt Instance
    {
        get
        {
            if (_instance == null)
            {    //查找场景中是否已经存在单例
                _instance = GameObject.FindObjectOfType<RoomDgt>();
                if (_instance == null)
                {    //创建游戏对象然后绑定单例脚本
                    GameObject go = new GameObject("Singleton");
                    _instance = go.AddComponent<RoomDgt>();
                }
            }
            return _instance;
        }

    }


    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
        roomView = GetComponent<RoomView>();
        roomModel = new RoomModel();
    }

    public void CreatePlayerLSt(List<User> UserLst)
    {
        if (UserLst == null)
        {
            return;
        }
        roomModel.UserLst = UserLst;
        roomView.RemovePlayerCell();
        foreach (var player in UserLst)
        {
            roomView.AddPlayerCell(player.UserName, player.Level);
        }
    }

    public void OnClickStartGameBtn()
    {
        RoomController.Instance.StartFight();
    }

    public void OnClickExitRoomBtn()
    {
        RoomController.Instance.ExitRoom(UserInfo.Instance.Id);
        UIController.Instance.HideUI("RoomRoot");
        UIController.Instance.ShowUI("RoomLstRoot");
    }
}
