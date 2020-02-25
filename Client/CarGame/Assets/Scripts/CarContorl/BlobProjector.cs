using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobProjector : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.parent.position + Vector3.up * 8.25f;
        transform.rotation = Quaternion.LookRotation(-Vector3.up, transform.parent.forward);
    }
}
