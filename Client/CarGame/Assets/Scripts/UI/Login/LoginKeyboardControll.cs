using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginKeyboardControll : MonoBehaviour
{
    public InputField userNameInPut;
    public InputField passwordInPut;
    public Button LoginBtn;
    public Button RegisterBtn;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
        }
    }
}
