using Assets.Scripts.Photon.Controller;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomDgt : MonoBehaviour
{
    public InputField RoomNameInput;
    private CreateRoomController createRoomController;

    private void Start()
    {
        RoomNameInput = GameObject.Find("RoomNameInput").GetComponent<InputField>();
        createRoomController = new CreateRoomController();
    }
    public void CreateRoom()
    {
        if (RoomNameInput.text != null)
        {
            createRoomController.CreateRoom(RoomNameInput.text);
        }
    }
}
