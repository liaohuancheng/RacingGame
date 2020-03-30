using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomCellDgt : MonoBehaviour
{
    public int Id { get; set; }
    public void OnClickRoomCell()
    {
        RoomListDgt.Instance.SelectingRoomId = Id;
    }
}
