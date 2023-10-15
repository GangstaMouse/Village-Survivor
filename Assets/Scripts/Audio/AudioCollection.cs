using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Collection", menuName = "Audio Collection")]
public class AudioCollection : ScriptableObject
{
    //public float GlobalVolume = 1.0f;
    //public float GlobalPitch = 1.0f;
    public float PitchRange = 0;
    public float VolumeRange = 0;
    public List<AudioContainer> Audios = new();
}

[Serializable]
public struct AudioContainer
{
    public AudioClip Audio;
    public float Volume;
    public float Pitch;
}