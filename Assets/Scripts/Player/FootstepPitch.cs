using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepPitch : MonoBehaviour
{
    void OnEnable()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.pitch = Random.Range(0.9f, 1.1f);
        Destroy(gameObject, 1f);
    }
}
