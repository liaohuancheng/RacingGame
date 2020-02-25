using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabilizerBar : MonoBehaviour
{
    public WheelCollider wheelColliderLeft;
    public WheelCollider WheelColliderRight;
    public float AntiRoll = 5000.0f;
    public Rigidbody rigidbody;

    private void Start()
    {
        rigidbody.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        WheelHit hit;
        float travelL = 1.0f;
        float travelR = 1.0f;

        bool groundedL = wheelColliderLeft.GetGroundHit(out hit);
        if (groundedL)
        {
            travelL = (-wheelColliderLeft.transform.InverseTransformPoint(hit.point).y - wheelColliderLeft.radius) / wheelColliderLeft.suspensionDistance;
        }

        bool groundedR = WheelColliderRight.GetGroundHit(out hit);
        if (groundedR)
        {
            travelR = (-WheelColliderRight.transform.InverseTransformPoint(hit.point).y - WheelColliderRight.radius) / WheelColliderRight.suspensionDistance;
        }

        float antiRollForce = (travelL - travelR) * AntiRoll;

        if (groundedL)
        {
            rigidbody.AddForceAtPosition(wheelColliderLeft.transform.up * -antiRollForce, wheelColliderLeft.transform.position);
        }
        if (groundedR)
        {
            rigidbody.AddForceAtPosition(WheelColliderRight.transform.up * -antiRollForce, WheelColliderRight.transform.position);
        }
            
    }
}
