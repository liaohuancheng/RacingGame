using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarByTransform : MonoBehaviour
{
    public Transform centOfMass;
    public float topSpeed = 160.0f;
    public float throttle = 0.0f;
    private float steer = 0.0f;
    private new Rigidbody rigidbody;
    const float minTurn = 10;
    const float maxTurn = 15;
    public Transform[] wheels;
    public WheelCollider[] wheelColliders;
    const int rpm = 1500;
    public Material brakeLights;
    public bool handbrake = false;//是否拉下手刹
    private float handbrakeXDragFactor = 0.5f;//拉下刹车后对x轴上阻力因子的改变系数
    private float initDragMulitiplierX = 0.5f;//x轴上的阻力因子，用于在刹车结束时
    private float handbrakeTime = 0.0f;//控制刹车时间
    private float handbrakeTimer = 1.0f;
    private Vector3 dragMultiplier = new Vector3(2, 5, 1);//阻力因子

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.centerOfMass = centOfMass.localPosition;
        topSpeed = utilities.MilePerHour2MeterPerSecond(topSpeed);
        initDragMulitiplierX = dragMultiplier.x;
    }

    void Update()
    {
        throttle = Input.GetAxis("Vertical");
        steer = Input.GetAxis("Horizontal");
        RotateWhells();
        //if (throttle < 0)
        //{
        //    brakeLights.SetFloat("_Intensity", Mathf.Abs(throttle));
        //}
        //else
        //{
        //    brakeLights.SetFloat("_Intensity", 0.0f);
        //}
        CheckHandbrake();
    }
    private void FixedUpdate()
    {
        Vector3 rv = transform.InverseTransformDirection(rigidbody.velocity);
        UpdateDrag(rv);
        ApplySteering(rv);
        ApplyThrottle(rv);
    }

    float SpeedToTurn(float speed)
    {
        float threshold = topSpeed / 2;
        if (speed > threshold) return minTurn;
        float speedFactor = 1 - (speed / threshold);
        return minTurn + speedFactor * (maxTurn - minTurn);
    }

    void ApplySteering(Vector3 v)
    {
        float turnRadius = (float)(3.0 / Mathf.Sin((90 - (steer * 30)) * Mathf.Deg2Rad));//是个估值
        float minMaxTurn = SpeedToTurn(rigidbody.velocity.magnitude);
        float turnSpeed = Mathf.Clamp(v.z / turnRadius, -minMaxTurn / 10, minMaxTurn / 10);
        transform.RotateAround(transform.position + transform.right * turnRadius * steer, transform.up, turnSpeed * Mathf.Rad2Deg * Time.deltaTime * steer);

        if (initDragMulitiplierX > dragMultiplier.x)
        {
            float rotDir = Mathf.Sign(steer);
            if (steer == 0)
            {
                if (rigidbody.angularVelocity.y < 1)
                {
                    rotDir = Random.Range(-1.0f, 1.0f);
                }
                else
                {
                    rotDir = rigidbody.angularVelocity.y;
                }
            }
            Vector3 p = (wheels[0].localPosition + wheels[1].localPosition) * 0.5f;
            transform.RotateAround(transform.TransformPoint(p), transform.up, rigidbody.velocity.magnitude * Mathf.Clamp01(1 - rigidbody.velocity.magnitude / topSpeed) * rotDir * Time.deltaTime * 2);
        }
    }

    void ApplyThrottle(Vector3 rv)
    {
        float throttleForce = 0;
        float brakeForece = 0;
        if (utilities.SameSign(rv.z, throttle))
        {
            if (!handbrake) throttleForce = throttle * 1000 * rigidbody.mass;
        }
        else
        {
            brakeForece = throttle * 50 * rigidbody.mass;
        }
        rigidbody.AddForce(transform.forward * Time.deltaTime * (throttleForce + brakeForece));
    }

    void RotateWhells()
    {
        Vector3 e = wheels[0].localEulerAngles;
        e.y = steer * maxTurn;
        wheels[0].localEulerAngles = e;
        wheels[1].localEulerAngles = e;
        Vector3 v = Vector3.right * -rigidbody.velocity.z * rpm / 60 * Time.deltaTime * Mathf.Rad2Deg;
        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].Rotate(v);
        }
    }

    IEnumerator StopHandbraking(float sec)
    {
        float diff = initDragMulitiplierX - dragMultiplier.x;
        handbrakeXDragFactor = sec;
        while (dragMultiplier.x < initDragMulitiplierX && !handbrake)
        {
            dragMultiplier.x += diff * (Time.deltaTime / sec);
            handbrakeTimer -= Time.deltaTime;
            yield return null;
        }
        dragMultiplier.x = initDragMulitiplierX;
        handbrakeTimer = 0;

    }

    void CheckHandbrake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //Debug.Log("刹车成功");
            if (!handbrake)
            {
                handbrake = true;
                handbrakeTime = Time.time;
                dragMultiplier.x = initDragMulitiplierX * handbrakeXDragFactor;
            }
        }
        else if (handbrake)
        {
            handbrake = false;
            StartCoroutine(StopHandbraking(Mathf.Min(5, Time.time - handbrakeTime)));
        }
    }

    void UpdateDrag(Vector3 rv)
    {
        Vector3 rd = new Vector3(-rv.x * Mathf.Abs(rv.x), -rv.y * Mathf.Abs(rv.y), -rv.z * Mathf.Abs(rv.z));
        Vector3 drag = Vector3.Scale(dragMultiplier, rd);
        if (initDragMulitiplierX > dragMultiplier.x)//刹车状态
        {
            drag.x /= (rv.magnitude) / (topSpeed / (1 + 2 * handbrakeXDragFactor));
            drag.z *= (1 + Mathf.Abs(Vector3.Dot(rigidbody.velocity.normalized, transform.forward)));//z轴的摩擦力与速度方向和行进方向有关
            drag += rigidbody.velocity * Mathf.Clamp01(rigidbody.velocity.magnitude / topSpeed);
        }
        else
        {
            drag.x *= topSpeed / rv.magnitude;
        }
        if (Mathf.Abs(rv.x) > 5 && !handbrake)
        {
            drag.x = -rv.x * dragMultiplier.x;
        }
        rigidbody.AddForce(transform.TransformDirection(drag) * rigidbody.mass * Time.deltaTime);
    }
}
