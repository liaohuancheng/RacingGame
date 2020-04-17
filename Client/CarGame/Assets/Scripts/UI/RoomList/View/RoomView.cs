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

    public void Awake()
    {
        contentTrans = GameObject.Find("RoomContent").transform;
        roomCellPre = (GameObject)Resources.Load("Prefabs/UI/PlayerProp");
        PlayerLst = new List<GameObject>();
    }

    public void AddPlayerCell(string playerName, int level)
    {

        GameObject PlayerPropGo = GameObject.Instantiate(roomCellPre, contentTrans);
        GameObject roomNameObj = PlayerPropGo.transform.Find("PlayerName").gameObject;
        GameObject LevelText = PlayerPropGo.transform.Find("Level").gameObject;
        roomNameObj.GetComponent<Text>().text = playerName;
        LevelText.GetComponent<Text>().text = level.ToString();
        PlayerLst.Add(PlayerPropGo);
    }

    public void RemovePlayerCell()
    {
        List<GameObject> tmpLst = new List<GameObject>(PlayerLst);
        
        foreach(var playerCell in tmpLst)
        {
            PlayerLst.Remove(playerCell);
            Destroy(playerCell);
        }

    }
}
