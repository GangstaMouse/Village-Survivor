using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Collection", menuName = "Audio Collection")]
public class AudioCollection : ScriptableObject
{
    [field: SerializeField] public float PitchRange { get; private set; }
    [field: SerializeField] public float VolumeRange { get; private set; }
    [field: SerializeField] public List<AudioContainer> Audios { get; private set; } = new();
}
