using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListView : MonoBehaviour
{
    public GameObject roomCellPre;
    public List<GameObject> RoomCellList;
    public Transform contentTrans;

    //public RoomListView()
    //{
    //    roomCell = (GameObject)Resources.Load("Prefabs/UI/RoomCell");
    //}
    public void Start()
    {
        contentTrans = GameObject.Find("RoomCellContent").transform;
        roomCellPre = (GameObject)Resources.Load("Prefabs/UI/RoomCell");
        Test();
    }

    public void AddRoomCell(string roomName, string houseOwerName, string peopleCount)
    {
        
        GameObject roomCellGo = GameObject.Instantiate(roomCellPre, contentTrans);
        GameObject roomNameObj = roomCellGo.transform.Find("RoomName").gameObject;
        GameObject houseOwerNameObj = roomCellGo.transform.Find("HouseOwerName").gameObject;
        GameObject peopleCountObj = roomCellGo.transform.Find("PeopleCount").gameObject;
        roomNameObj.GetComponent<Text>().text = roomName;
        houseOwerNameObj.GetComponent<Text>().text = houseOwerName;
        peopleCountObj.GetComponent<Text>().text = peopleCount;
        RoomCellList.Add(roomCellGo);
    }

    public void Test()
    {
        AddRoomCell("111", "123", "2/12");
        AddRoomCell("111", "123", "2/12");
        AddRoomCell("111", "123", "2/12");
        AddRoomCell("111", "123", "2/12");
    }
}
