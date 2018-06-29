using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegControl : MonoBehaviour
{
	public delegate void SegEvent (int seg);

	public SegEvent OnSegTrigger;

	private int seg;

	public void Setup (int seg)
	{
		this.seg = seg;
	}

	private void OnTriggerEnter (Collider coll)
	{
		if (coll.tag == "Player" && seg != 0) {
			if (OnSegTrigger != null) {
				OnSegTrigger (seg);
			}
		}
	}
}
