using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerMode
{
    Point,
    Path
}

public class CameraTrigger : MonoBehaviour
{
    public TriggerMode mode;
    public Transform target;
    public Vector3 offset;
    public AnimationCurve smoothInCurve;
    public AnimationCurve smoothOutCurve;
    public float movementSmoothing;
    public float rotationSmoothing;
    public LayerMask triggerLayers;

    private Transform origTarget;
    private Vector3 origOffset;
    private float origMovementSmoothing;
    private float origRotationSmoothing;

    public delegate void CameraEvent(Transform target, Vector3 offset, float moveSmoothing, float rotationSmoothing, AnimationCurve curve);
    public static event CameraEvent SetTarget;

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((triggerLayers & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            if (SetTarget != null)
            {
                if (mode == TriggerMode.Point)
                {
                    CameraController camController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
                    origTarget = camController.target;
                    origOffset = camController.offset;
                    origMovementSmoothing = camController.movementSmoothing;
                    origRotationSmoothing = camController.rotationSmoothing;
                    SetTarget(target, offset, movementSmoothing, rotationSmoothing, smoothInCurve);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if ((triggerLayers & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            if (SetTarget != null)
            {
                if (mode == TriggerMode.Point)
                {
                    SetTarget(origTarget, origOffset, origMovementSmoothing, origRotationSmoothing, smoothOutCurve);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(target.transform.position + offset, 1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(target.transform.position, 1f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(target.transform.position, target.transform.position + offset);
    }
}
