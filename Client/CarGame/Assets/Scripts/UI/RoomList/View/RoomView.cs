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
        if(PlayerLst == null)
        {
            return;
        }
        
        for (int i = 0; i < PlayerLst.Count; i++)
        {
            GameObject PlayerPropGo = PlayerLst[i];
            PlayerLst.Remove(PlayerLst[i]);
            Destroy(PlayerPropGo);
        }

    }
}
