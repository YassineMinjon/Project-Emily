using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public AnimationCurve smoothCurve;
    public float movementSmoothing;
    public float rotationSmoothing;
    private Vector3 velocity;
    public float prevMovementSmoothing;
    private float timer;

    void OnEnable()
    {
        CameraTrigger.SetTarget += SetTarget;
        prevMovementSmoothing = movementSmoothing;
    }

    void OnDisable()
    {
        CameraTrigger.SetTarget -= SetTarget;
    }

    private void Update()
    {
        Position();
        Rotation();
    }

    private void Rotation()
    {  
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation((target.transform.position - transform.position).normalized), rotationSmoothing * Time.deltaTime);
    }

    private void Position()
    {
        timer += Time.deltaTime;
        //transform.position = Vector3.Lerp(transform.position, target.transform.position + offset, movementSmoothing * Time.deltaTime);
        //prevMovementSmoothing = Mathf.Lerp(prevMovementSmoothing, movementSmoothing, smoothCurve.Evaluate(Time.time) * Time.deltaTime);
        prevMovementSmoothing = smoothCurve.Evaluate(timer);
        transform.position = Vector3.SmoothDamp(transform.position, target.transform.position + offset, ref velocity, prevMovementSmoothing);
    }

    private void SetTarget(Transform _target, Vector3 _offset, float _movementSmoothing, float _rotationSmoothing, AnimationCurve curve)
    {
        timer = 0f;
        target = _target;
        offset = _offset;
        smoothCurve = curve;
        movementSmoothing = _movementSmoothing;
        rotationSmoothing = _rotationSmoothing;
    }
}
