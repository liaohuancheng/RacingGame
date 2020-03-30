using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RoomListView : MonoBehaviour
{
    public GameObject roomCellPre;
    public List<GameObject> RoomCellList;
    public Transform contentTrans;

    public Dictionary<int, GameObject> RoomCellDic { get; set; }

    //public RoomListView()
    //{
    //    roomCell = (GameObject)Resources.Load("Prefabs/UI/RoomCell");
    //}
    public void Start()
    {
        contentTrans = GameObject.Find("RoomCellContent").transform;
        roomCellPre = (GameObject)Resources.Load("Prefabs/UI/RoomCell");
        RoomCellDic = new Dictionary<int, GameObject>();
    }

    public void AddRoomCell(string roomName, string houseOwerName, string peopleCount,int RoomID)
    {
        
        GameObject roomCellGo = GameObject.Instantiate(roomCellPre, contentTrans);
        GameObject roomNameObj = roomCellGo.transform.Find("RoomName").gameObject;
        GameObject houseOwerNameObj = roomCellGo.transform.Find("HouseOwerName").gameObject;
        GameObject peopleCountObj = roomCellGo.transform.Find("PeopleCount").gameObject;
        roomCellGo.GetComponent<RoomCellDgt>().Id = RoomID;
        roomNameObj.GetComponent<Text>().text = roomName;
        houseOwerNameObj.GetComponent<Text>().text = houseOwerName;
        peopleCountObj.GetComponent<Text>().text = peopleCount;
        RoomCellDic.Add(RoomID, roomCellGo);
    }

    public void RemoveRoomCell()
    {
        foreach (var item in RoomCellDic.ToList())
        {
            GameObject roomCellGo = item.Value;
            RoomCellDic.Remove(item.Key);
            Destroy(roomCellGo);
        }
        
    }

    //public void Test()
    //{
    //    AddRoomCell("111", "123", "2/12", 1);
    //    AddRoomCell("112", "123", "2/12", 2);
    //    AddRoomCell("113", "123", "2/12", 3);
    //    AddRoomCell("114", "123", "2/12", 4);
    //    RemoveRoomCell();
    //}
}
