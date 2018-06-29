using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
	public Rail rail;

	public bool VRMode;
	private int currentSeg;
	private float transition;
	private bool isCompleted;
	public float moveSpeed = 2.5f;
	public PlayMode mode;
	public bool loop;
    public GameObject player;
    public Vector3 offset;


	private void LateUpdate ()
	{
        if (!rail)
        {
            return;
        }

        if (!isCompleted)
        {
            Follow();
        }
        /*
		if (!rail) {
			return;
		}

		if (!isCompleted) {
			Play ();
		}*/
    }

	private void Play ()
	{
		float m = (rail.nodes [currentSeg + 1].position - rail.nodes [currentSeg].position).magnitude;
		float s = (Time.deltaTime * 1 / m) * moveSpeed;


		transition += s;
		if (transition > 1) {
			transition = 0;
			currentSeg++;
			if (currentSeg == rail.nodes.Length - 1) {
				if (loop) {
					currentSeg = 0;
				} else {
					isCompleted = true;
					return;
				}
			}
		} else if (transition < 0) {
			transition = 1;
			currentSeg--;
		}

		transform.position = rail.PositionOnRail (currentSeg, transition, mode, rail.path);
		if (!VRMode) {
			transform.rotation = rail.Orientation (currentSeg, transition);
		}
	}


    private void Follow()
    {
        if (currentSeg > -1)
        {
            float m = (rail.nodes[currentSeg + 1].position - rail.nodes[currentSeg].position).magnitude;
            //float s = (Vector3.Distance(rail.nodes[currentSeg].position, player.transform.position)) / (Vector3.Distance(rail.nodes[currentSeg + 1].position, rail.nodes[currentSeg].position));
            float s = (player.transform.position.x - rail.nodes[currentSeg].position.x) / (rail.nodes[currentSeg + 1].position.x - rail.nodes[currentSeg].position.x);
            transition = s;
            if (transition > 1)
            {
                transition = 0;
                currentSeg++;
                if (currentSeg == rail.nodes.Length - 1)
                {
                    if (loop)
                    {
                        currentSeg = 0;
                    }
                    else
                    {
                        isCompleted = true;
                        return;
                    }
                }
            }
            else if (transition < 0)
            {
                transition = 1;
                currentSeg--;
            }

            transform.position = rail.PositionOnRail(currentSeg, transition, mode, rail.path) + offset;
            if (!VRMode)
            {
                transform.rotation = rail.Orientation(currentSeg, transition);
            }
        }
        else
        {
            currentSeg = 0;
        }
    }
}
