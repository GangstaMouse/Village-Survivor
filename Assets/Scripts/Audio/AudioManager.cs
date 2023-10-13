using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioManager
{
    public static GameObject CreateNewInst(AudioClip clip, Vector3 loc)
    {
        GameObject ob = new GameObject("Audio Instance", typeof(AudioSource));
        ob.transform.position = loc;
        var aud = ob.GetComponent<AudioSource>();
        aud.clip = clip;
        aud.Play();
        return ob;
    }
}
