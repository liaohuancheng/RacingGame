
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThridCamera : MonoBehaviour
{
    public Transform target;
    public float height = 2f;//在目标上方的距离
    public float velocityDamping = 3f;//插值系数
    public LayerMask ignoreLayers = -1;//忽略的层
    private LayerMask raycastLayers = -1;//使用的层
    private RaycastHit hit = new RaycastHit();
    private Vector3 preVelocity = Vector3.zero;
    private Vector3 currentVelocity = Vector3.zero;
    public new Camera camera;
    Vector3 velocity = Vector3.zero;
    void Start()
    {
        raycastLayers = ~ignoreLayers;    
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    void FixedUpdate()
    {
        if (target == null)
            return;
        currentVelocity = Vector3.Lerp(preVelocity, target.root.GetComponent<Rigidbody>().velocity, velocityDamping * Time.deltaTime);
        currentVelocity.y = 0;
        preVelocity = currentVelocity;
    }

    void LateUpdate()
    {
        if (target == null)
            return;
        float speedFactor = Mathf.Clamp01(target.root.GetComponent<Rigidbody>().velocity.magnitude / 70.0f);
        camera.fieldOfView = Mathf.Lerp(52, 77, speedFactor);
        float currentDistance = Mathf.Lerp(7.5f, 6.5f, speedFactor);
        currentVelocity = currentVelocity.normalized;
        Vector3 newTargetPostion = target.position + Vector3.up * height;
        Vector3 newPostion = newTargetPostion - (currentVelocity * currentDistance);
        newPostion.y = newTargetPostion.y;
        Vector3 TargetPostion = newPostion - newTargetPostion;
        Vector3 targetDirection = newPostion - newTargetPostion;
        if (Physics.Raycast(newTargetPostion, targetDirection, out hit, currentDistance, raycastLayers))
            newPostion = hit.point;
        transform.position = newPostion;
        //transform.position = Vector3.SmoothDamp(transform.position, newPostion, ref velocity, 0.2f);
        transform.LookAt(newTargetPostion);
    }
}
