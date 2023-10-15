using UnityEngine;

public static class AudioManager
{
    public static AudioSource CreateAudioInstance(in AudioCollection AudioCollection)
    {
        AudioContainer audioContainer = AudioCollection.Audios[UnityEngine.Random.Range(0, AudioCollection.Audios.Count)];
        AudioSource audioSource = CreateAudioInstance(audioContainer.Audio);

        audioSource.volume = audioContainer.Volume + UnityEngine.Random.Range(-AudioCollection.VolumeRange, AudioCollection.VolumeRange);
        audioSource.pitch = audioContainer.Pitch + UnityEngine.Random.Range(-AudioCollection.PitchRange, AudioCollection.PitchRange);
        return audioSource;
    }

    public static AudioSource CreateAudioInstance(in AudioCollection AudioCollection, Vector3 position)
    {
        AudioSource audioSource = CreateAudioInstance(AudioCollection);
        return SetAudioLocation(audioSource, position);
    }

    public static AudioSource CreateAudioInstance(AudioClip clip)
    {
        GameObject newObject = new GameObject("Audio Instance", typeof(AudioSource));
        var audioSource = newObject.GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
        MonoBehaviour.Destroy(newObject, clip.length);
        return audioSource;
    }

    public static AudioSource CreateAudioInstance(AudioClip clip, Vector3 position)
    {
        var audioSource = CreateAudioInstance(clip);
        return SetAudioLocation(audioSource, position);
    }

    private static AudioSource SetAudioLocation(AudioSource audioSource, Vector3 position)
    {
        audioSource.transform.position = position;
        audioSource.spatialBlend = 0.8f;
        return audioSource;
    }
}
