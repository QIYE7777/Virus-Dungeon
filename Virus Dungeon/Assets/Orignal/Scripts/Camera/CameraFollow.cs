using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;
    Transform _target;
    public float smoothing = 5f;

    public Vector3 offset;

    private void Awake()
    {
        instance = this;
    }

    public void Init(Transform t)
    {
        _target = t;
        Vector3 targetCamPos = _target.position + offset;
        transform.position = targetCamPos;
    }

    public void SyncPos(Vector3 targetPos)
    {
        Vector3 targetCamPos = targetPos + offset;
        transform.position = targetCamPos;
    }

    private void FixedUpdate()
    {
        if (_target == null)
            return;
        Vector3 targetCamPos = _target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
