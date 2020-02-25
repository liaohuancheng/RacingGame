using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{
    public GameObject car;
    public Text speedUI;
    void Update()
    {
        speedUI.text = car.GetComponent<Rigidbody>().velocity.magnitude.ToString();
    }
}
