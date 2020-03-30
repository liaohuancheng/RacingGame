using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomView : MonoBehaviour
{
    public GameObject roomCellPre;
    public List<GameObject> RoomCellList;
    public Transform contentTrans;

    public List<GameObject> PlayerLst { get; set; }

    public void Start()
    {
        contentTrans = GameObject.Find("RoomContent").transform;
        roomCellPre = (GameObject)Resources.Load("Prefabs/UI/PlayerProp");
        PlayerLst = new List<GameObject>();
    }

    public void AddPlayerCell(string playerName, string level)
    {

        GameObject PlayerPropGo = GameObject.Instantiate(roomCellPre, contentTrans);
        GameObject roomNameObj = PlayerPropGo.transform.Find("PlayerName").gameObject;
        GameObject LevelText = PlayerPropGo.transform.Find("Level").gameObject;
        roomNameObj.GetComponent<Text>().text = playerName;
        LevelText.GetComponent<Text>().text = level;
        PlayerLst.Add(PlayerPropGo);
    }

    public void RemovePlayerCell()
    {
        foreach (var item in PlayerLst)
        {
            GameObject PlayerPropGo = item;
            PlayerLst.Remove(PlayerPropGo);
            Destroy(PlayerPropGo);
        }

    }
}
