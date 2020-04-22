using Assets.Scripts.Photon.Controller;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public enum CtrlType
    {
        none,
        player,
        net,
    }

    public CtrlType ctrlType = CtrlType.player;

    public Vector3 lastPos;
    public Vector3 lastRot;
    public Vector3 frontPos;
    public Vector3 frontRot;

    public Quaternion frontRotTest;

    public Transform wheelFR;
    public Transform wheelFL;
    public Transform wheelRR;
    public Transform wheelRL;

    public WheelCollider wheelColliderFR;
    public WheelCollider wheelColliderFL;
    public WheelCollider wheelColliderRR;
    public WheelCollider wheelColliderRL;

    public new Rigidbody rigidbody;

    public Transform centerOfMass;

    public float motorTorque;
    public float steeringAngle;

    public float maxSpeed = 200;
    public float minSpeed = 30;
    private float currentSpeed;
    private CarUsrControl usrControl;

    public float brakeTorque ;
    private Rigidbody rigibody;
    public int reverseTorque;

    private float moveOffset = 0.1f;

    private float lastSendTime;
    private float lastRecvTime;
    private float timeInterval;

    private bool canControll = true;

    // Start is called before the first frame update
    void Awake()
    {
        initProperty();
        rigidbody = GetComponent<Rigidbody>();
        if (ctrlType == CtrlType.player)
        {
            usrControl = GetComponent<CarUsrControl>();
        }
        lastSendTime = Time.time;
        lastRecvTime = Time.deltaTime;
        lastPos = transform.position;
        lastRot = transform.eulerAngles;

        frontRotTest = transform.rotation;
        canControll = true;
    }

    private void Update()
    {
        if(ctrlType == CtrlType.player)
        {
            if (!canControll)
            {
                return;
            }
            usrControl.control();
            if(Time.time - lastSendTime > 0.1f)
            {
                SyncPosition();
                lastSendTime = Time.time;
            }
            
        }
        else if(ctrlType == CtrlType.net)
        {
            NetUpdate();
        }
    }

    public void NetForecastInfo(Vector3 nowPostion, Vector3 nowRotation) 
    {
        frontPos = lastPos +(nowPostion - lastPos) * 2;
        frontRot = lastRot + (nowRotation - lastRot) * 2;
        if (Time.time - lastRecvTime > 0.1f)
        {
            frontPos = nowPostion;
            frontRot = nowRotation;
        }

        timeInterval = Time.time - lastRecvTime;

        lastPos = nowPostion;
        lastRot = nowRotation;
        lastRecvTime = Time.time;


    }

    private void NetUpdate()
    {
        Vector3 pos = transform.position;
        Vector3 rot = transform.eulerAngles;
        
        if(timeInterval > 0)
        {
            transform.position = Vector3.Lerp(pos, frontPos, timeInterval);
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(rot), Quaternion.Euler(frontRot), timeInterval);
            
        }
    }

    void SyncPosition()
    {
        if(Vector3.Distance(transform.position, lastPos) > moveOffset)
        {
            lastPos = transform.position;
            BattleController.Instance.SendPosition(transform.position, transform.eulerAngles);
        }
    }

    private void initProperty()//初始化各个物件
    {
        wheelColliderFL = transform.Find("WheelsHubs/WheelHubFrontLeft").GetComponent<WheelCollider>();
        wheelColliderFR = transform.Find("WheelsHubs/WheelHubFrontRight").GetComponent<WheelCollider>();
        wheelColliderRR  = transform.Find("WheelsHubs/WheelHubRearRight").GetComponent<WheelCollider>();
        wheelColliderRL  = transform.Find("WheelsHubs/WheelHubRearLeft").GetComponent<WheelCollider>();

        wheelFL = transform.Find("model/Car02WheelFrontLeft");
        wheelFR = transform.Find("model/Car02WheelFrontRight");
        wheelRR = transform.Find("model/Car02WheelRearRight");
        wheelRL = transform.Find("model/Car02WheelRearLeft");

        rigibody = GetComponent<Rigidbody>();

    }

    public void InitNetCtrl()
    {
        lastPos = transform.position;
        lastRot = transform.eulerAngles;
        frontPos = transform.position;
        frontPos = transform.eulerAngles;
        rigibody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void Move(float steering, float accel, float footbrake, float handbrake)
    {
        bool isBrake = false;

        currentSpeed = wheelColliderFL.rpm * (wheelColliderFL.radius * 2 * Mathf.PI) * 60 / 1000;
        currentSpeed = rigibody.velocity.magnitude;
        //Debug.Log(wheelColliderFL.motorTorque);
        //Debug.Log(currentSpeed);
        ApplyDrive(accel, footbrake);

        if (handbrake>0)
        {
            isBrake = true;
        }
        else
        {
            isBrake = false;
        }

        //Debug.Log(isBrake);

        if (isBrake)
        {
            wheelColliderRR.brakeTorque = brakeTorque;
            wheelColliderRL.brakeTorque = brakeTorque;
        }
        else
        {
            wheelColliderRR.brakeTorque = 0;
            wheelColliderRL.brakeTorque = 0;
        }
        wheelColliderFL.steerAngle = steering * steeringAngle;
        wheelColliderFR.steerAngle = steering * steeringAngle;


        RotateWheels();//轮胎旋转
        SteerWheels();//轮胎偏转
        AddDownForce();//增加抓地力，方便控制
    }

    private void ApplyDrive(float accel, float footbrake)
    {
        if ((currentSpeed > maxSpeed && accel > 0) || (currentSpeed < -minSpeed && footbrake > 0))
        {
            //Debug.Log("置0");
            wheelColliderFL.motorTorque = 0;
            wheelColliderFR.motorTorque = 0;
            wheelColliderRL.motorTorque = 0;
            wheelColliderRR.motorTorque = 0;
        }
        else
        {

            wheelColliderFL.motorTorque = accel * motorTorque;
            wheelColliderFR.motorTorque = accel * motorTorque;
            wheelColliderRR.motorTorque = accel * motorTorque;
            wheelColliderRL.motorTorque = accel * motorTorque;
        }

        if(currentSpeed > 5 && Vector3.Angle(transform.forward, rigidbody.velocity) < 50f)
        {
            wheelColliderFL.brakeTorque = brakeTorque * footbrake;
            wheelColliderFR.brakeTorque = brakeTorque * footbrake;
            wheelColliderRR.brakeTorque = brakeTorque * footbrake;
            wheelColliderRL.brakeTorque = brakeTorque * footbrake;
        }else if (footbrake > 0)
        {
            wheelColliderFL.brakeTorque = 0f;
            wheelColliderFR.brakeTorque = 0f;
            wheelColliderRR.brakeTorque = 0f;
            wheelColliderRL.brakeTorque = 0f;

            wheelColliderFL.motorTorque = -reverseTorque * footbrake; ;
            wheelColliderFR.motorTorque = -reverseTorque * footbrake; ;
            wheelColliderRR.motorTorque = -reverseTorque * footbrake; ;
            wheelColliderRL.motorTorque = -reverseTorque * footbrake; ;
        }
        
    }

    private void AddDownForce()
    {
        //m_WheelColliders[0].attachedRigidbody.AddForce(-transform.up*m_Downforce*
        //                                                 m_WheelColliders[0].attachedRigidbody.velocity.magnitude);
        rigibody.AddForce(-transform.up * 100 * wheelColliderFL.attachedRigidbody.velocity.magnitude);
    }

    private void SteerWheels()
    {
        Vector3 localEulerAngles = wheelFL.localEulerAngles;
        localEulerAngles.y = wheelColliderFL.steerAngle;

        wheelFL.localEulerAngles = localEulerAngles;
        wheelFR.localEulerAngles = localEulerAngles;
    }

    private void RotateWheels()
    {
        wheelFL.Rotate(wheelColliderFL.rpm * 360 / 60 * Time.deltaTime * Vector3.right);
        wheelFR.Rotate(wheelColliderFR.rpm * 360 / 60 * Time.deltaTime * Vector3.right);
        wheelRR.Rotate(wheelColliderRR.rpm * 360 / 60 * Time.deltaTime * Vector3.right);
        wheelRL.Rotate(wheelColliderRL.rpm * 360 / 60 * Time.deltaTime * Vector3.right);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "CheckPoint")
        {
            GameMgr.Instance.PassCheckPoint(other.gameObject);
        }
        else if(other.tag == "OverPoint")
        {
            if (canControll)
            {
                GameMgr.Instance.ArrivalTerminal();
                canControll = false;
            }
        }
    }
}
