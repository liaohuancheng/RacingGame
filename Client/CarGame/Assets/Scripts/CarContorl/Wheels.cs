using UnityEngine;

public class Wheels : MonoBehaviour
{
    public WheelCollider wc;
    public float degree = 0;//转动速度
    

    // Update is called once per frame
    void Update()
    {
        this.degree = (this.wc.rpm * 360 / 60) * Time.deltaTime;
        this.transform.Rotate(degree * Vector3.right);
    }
}
