using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreeb : MonoBehaviour
{
    public AudioSource source;

    public AudioClip clip;

    private void OnEnable()
    {
        source.clip = clip;
        source.Play();
    }
}
