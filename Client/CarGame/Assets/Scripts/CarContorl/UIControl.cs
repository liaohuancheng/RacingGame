using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public Text TxtBrakeTorque;
    public Text TxtMotorTorque;
    public Text TxtAngle;
    public Text TxtFootbrake;
    public Text TxtVel;
    public Car car;
    public CarUsrControl carUsrControl;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TxtBrakeTorque.text = "BrakeTorque:" +"FL:" +car.wheelColliderFL.brakeTorque + "FR:" + car.wheelColliderFR.brakeTorque + "RL:" + car.wheelColliderRL.brakeTorque + "RR:" + car.wheelColliderRR.brakeTorque;
        TxtMotorTorque.text = "MotorTorque:" + "FL:" + car.wheelColliderFL.motorTorque + "FR:" + car.wheelColliderFR.motorTorque + "RL:" + car.wheelColliderRL.motorTorque + "RR:" + car.wheelColliderRR.motorTorque;
        TxtAngle.text = "Angle:" + Vector3.Angle(car.transform.forward, car.rigidbody.velocity);
        TxtFootbrake.text = "Footbrake:" + Input.GetAxis("Vertical");
        TxtVel.text = "Vel:" + car.rigidbody.velocity.magnitude;

    }
}
