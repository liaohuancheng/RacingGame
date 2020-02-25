using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Car))]
public class CarUsrControl : MonoBehaviour
{
    private Car car;

    void Awake()
    {
        car = GetComponent<Car>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float handbrake = Input.GetAxis("Jump");
        car.Move(h, v, -v, handbrake);
    }
}
