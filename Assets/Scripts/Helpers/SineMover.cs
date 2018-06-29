using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineMover : MonoBehaviour
{
    public Vector3 startPos;
    public Vector2 circle;
    public Vector2 speed;
	
	void Start ()
    {
        startPos = transform.position;
	}
	

	void Update ()
    {
        transform.position = new Vector3(startPos.x + Mathf.Sin(Time.time*speed.x) * circle.x, startPos.y + Mathf.Cos(Time.time*speed.y) * circle.y, 0);
	}
}
