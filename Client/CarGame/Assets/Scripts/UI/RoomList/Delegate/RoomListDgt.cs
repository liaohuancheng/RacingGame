using Assets.Scripts;
using Assets.Scripts.Photon.Controller;
using CarCommon.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomListDgt : MonoBehaviour
{
    private static RoomListDgt _instance = null;
    public RoomListView roomListView;
    public RoomListModel roomListModel;
    public int SelectingRoomId { get; set; }
    public static RoomListDgt Instance
    {
        get
        {
            if (_instance == null)
            {    //查找场景中是否已经存在单例
                _instance = GameObject.FindObjectOfType<RoomListDgt>();
                if (_instance == null)
                {    //创建游戏对象然后绑定单例脚本
                    GameObject go = new GameObject("Singleton");
                    _instance = go.AddComponent<RoomListDgt>();
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
        GetRoomController.Instance.GetRoomLst();
        roomListView = GetComponent<RoomListView>();
        roomListModel = new RoomListModel();
    }

    public void CreateRoomList(List<RoomProperty> roomLst)
    {
        if(roomLst == null)
        {
            return;
        }
        roomListModel.RoomPropertyList = roomLst;
        roomListView.RemoveRoomCell();
        foreach (var roomCell in roomLst)
        {
            roomListView.AddRoomCell(roomCell.RoomName, roomCell.RoomOwnerName, roomCell.Count.ToString(), roomCell.ID);
        }
    }

    public void EnterSuccess(IList<User> users)
    {
        UIController.Instance.HideUI("RoomLstRoot");
        UIController.Instance.ShowUI("RoomRoot");
        RoomDgt.Instance.CreatePlayerLSt(users);
    }

    public void OnClickEnterBtn()
    {
        RoomController.Instance.EnterRoom(UserInfo.Instance.Id, SelectingRoomId);
    }

    public void OnClickCreateRoomBtn()
    {

        UIController.Instance.HideUI("RoomLstRoot");
        UIController.Instance.ShowUI("CreateRoomPanel");
    }
}
