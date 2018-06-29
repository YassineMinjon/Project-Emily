using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum PlayMode
{
	Linear,
	Catmull,
}


[ExecuteInEditMode]
public class Rail : MonoBehaviour
{
	public Transform[] nodes;
    public List<Transform> path;
    public PathContainer container;
	[HideInInspector]
	public bool DrawPathLinear = true;
	[HideInInspector]
	public bool DrawPathCatmull = true;
    [HideInInspector]
    public int DrawnPath = 0;
	[HideInInspector]
	public bool DrawCones = true;
	public Mover[] mymover;
	private float speed;
	public GameObject WaypointPrefab;

	[HideInInspector]
	public int Errors;

	public int playerPosition;


	void Start ()
	{
		if (mymover == null) {
			speed = 5;
		} else {
//            speed = mymover[0].moveSpeed;
		}
	}

	void OnEnable ()
	{
		
	}

	void OnDisable ()
	{
		for (int i = 1; i < nodes.Length; i++) {
			nodes [i].GetComponent<SegControl> ().OnSegTrigger -= OnSegEntered;
		}
	}

	private void OnSegEntered (int seg)
	{
		Debug.Log ("trigger entered of seg " + seg);
		playerPosition = seg;
	}

	public Vector3 PositionOnRail (int seg, float ratio, PlayMode mode, List<Transform> path)
	{
		switch (mode) {
		default:
		case PlayMode.Linear:
			return LinearPosition (seg, ratio);
		case PlayMode.Catmull:
			return CatmullPosition (seg, ratio, path);
		}
	}

	public Vector3 LinearPosition (int seg, float ratio)
	{
		Vector3 p1 = nodes [seg].position;
		Vector3 p2 = nodes [seg + 1].position;

		return Vector3.Lerp (p1, p2, ratio);
	}

	public Vector3 CatmullPosition (int seg, float ratio, List<Transform> path)
	{
		Vector3 p1, p2, p3, p4;

		if (seg == 0) {
			p1 = path [seg].position;
			p2 = p1;
			p3 = path [seg + 1].position;
			p4 = path [seg + 2].position;
		} else if (seg == path.Count - 2) {
			p1 = path [seg - 1].position;
			p2 = path [seg].position;
			p3 = path [seg + 1].position;
			p4 = p3;
		} else {
			p1 = path [seg - 1].position;
			p2 = path [seg].position;
			p3 = path [seg + 1].position;
			p4 = path [seg + 2].position;
		}

		float t2 = ratio * ratio;
		float t3 = t2 * ratio;

		float x = 
			0.5f * (
			    (2.0f * p2.x)
			    + (-p1.x + p3.x)
			    * ratio + (2.0f * p1.x - 5.0f * p2.x + 4 * p3.x - p4.x)
			    * t2 + (-p1.x + 3.0f * p2.x - 3.0f * p3.x + p4.x)
			    * t3);

		float y = 
			0.5f * (
			    (2.0f * p2.y)
			    + (-p1.y + p3.y)
			    * ratio + (2.0f * p1.y - 5.0f * p2.y + 4 * p3.y - p4.y)
			    * t2 + (-p1.y + 3.0f * p2.y - 3.0f * p3.y + p4.y)
			    * t3);

		float z = 
			0.5f * (
			    (2.0f * p2.z)
			    + (-p1.z + p3.z)
			    * ratio + (2.0f * p1.z - 5.0f * p2.z + 4 * p3.z - p4.z)
			    * t2 + (-p1.z + 3.0f * p2.z - 3.0f * p3.z + p4.z)
			    * t3);

		return new Vector3 (x, y, z);
	}

	public Quaternion Orientation (int seg, float ratio)
	{
		Quaternion q1 = nodes [seg].rotation;
		Quaternion q2 = nodes [seg + 1].rotation;

		return Quaternion.Lerp (q1, q2, ratio);
	}
	#if UNITY_EDITOR
	private void OnDrawGizmos ()
	{
        container.nodes = GetComponentsInChildren<Transform>();
        nodes = container.nodes;
        path.Clear();
        foreach(Transform node in nodes)
        {
            path.Add(node);
        }

        Errors = 0;
		for (int i = 0; i < nodes.Length - 1; i++) {
			if (Vector3.Distance (nodes [i].position, nodes [i + 1].position) <= speed) {
				Handles.color = new Color (1F, 0F, 0F);
				Errors++;
			} else if (Vector3.Distance (nodes [i].position, nodes [i + 1].position) <= (speed * 1.5) && Vector3.Distance (nodes [i].position, nodes [i + 1].position) >= speed) {
				Handles.color = new Color (1F, 0.4F, 0F);
				Errors++;
			} else if (Vector3.Distance (nodes [i].position, nodes [i + 1].position) >= (speed * 1.5)) {
				Handles.color = new Color (0F, 1F, 0F);
			}

			if (DrawPathLinear) {
				Handles.DrawDottedLine (nodes [i].position, nodes [i + 1].position, 3.0f);
			}

			if (DrawCones) {
				Handles.ConeHandleCap (0, nodes [i].position, nodes [i].rotation, 1f, EventType.Repaint);
				if (i == nodes.Length - 2) {
					Handles.ConeHandleCap (0, nodes [i + 1].position, nodes [i + 1].rotation, 1f, EventType.Repaint);
				}
			}     
		}

		if (DrawPathCatmull) { 
            //Transform[] tempPath = new Transform[]
			var sequence = Interpolate.NewCatmullRom (nodes, 20, false);
			var firstPoint = nodes [0].position;
			var segmentStart = firstPoint;

			foreach (var segmentEnd in sequence) {
				Gizmos.DrawLine (segmentStart, segmentEnd);
				segmentStart = segmentEnd;
			}
		}
	}
	#endif
}

