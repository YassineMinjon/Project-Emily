using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed;
    public float runSpeed;
    public float jumpHeight;

    [Header("GamePlay")]
    public int maxHealth;
    private int currentHealth;

    [Header("Math&Physics")]
    public Vector2 gravity;
    public Vector2 currGrav;
    public bool grounded;
    public Vector2 velocity;
    public Vector2 currVelocity;
    public Vector2 prevVelocity;
    public Vector2 currPos, prevPos;
    public List<Vector2> ForceList = new List<Vector2>();

    [Header("Externals")]
    public LayerMask nonPlayerMask;
    public Collider col;

    private Vector3 pointEnd;

    void Start()
    {
        col = GetComponent<Collider>();
    }

    void FixedUpdate()
    {
        #region velocity
        prevPos = currPos;
        currPos = transform.position;
        velocity = (currPos - prevPos) * (1 / Time.fixedDeltaTime);
        #endregion
        Gravity();
        //Jump();
        //SimpleMove();
    }

    void LateUpdate()
    {
        FinalMove();
        ForceList.Clear();
    }

    private void Gravity()
    {
        Ray ray = new Ray(transform.position, ((transform.position + (Vector3)gravity) - transform.position));
        RaycastHit hit;
        Debug.DrawRay(transform.position, ((transform.position + (Vector3)gravity) - transform.position));
        //float length = -(currGrav + (gravity * Time.fixedDeltaTime)).y;
        float length = Vector3.Distance(transform.position + (Vector3)((gravity * Time.fixedDeltaTime) * Time.fixedDeltaTime), transform.position) + 0.001f;
        pointEnd = transform.position + (Vector3.down * length);
        if (Physics.Raycast(ray, out hit, length, nonPlayerMask))
        {

                currGrav = Vector2.zero;
                velocity.y =0f;
                //transform.position = hit.point + (Vector3)(Vector2.up * col.bounds.size.y);
                grounded = true;
 
        }
        else
        {
            currGrav += (gravity * Time.fixedDeltaTime) * Time.fixedDeltaTime;
            grounded = false;
        }
    }

    private void Jump()
    {

    }

    private void SimpleMove()
    {

    }

    private void FinalMove()
    {
        transform.position += (Vector3)currGrav;
    }

    public void AddForce(Vector2 force)
    {
        ForceList.Add(force);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position + (Vector3)gravity, Vector3.one);
        Gizmos.DrawSphere(pointEnd, 1f);
    }
}
