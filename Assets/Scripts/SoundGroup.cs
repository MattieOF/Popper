using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sound Group", menuName = "Game/Sound Group")]
public class SoundGroup : ScriptableObject
{
    public List<AudioClip> sounds;

    public AudioClip GetRandomSound()
    {
        return sounds[Mathf.RoundToInt(Random.Range(0, sounds.Count - 1))];
    }

    public void PlayRandomSound(AudioSource audioSource)
    {
        audioSource.clip = GetRandomSound();
        audioSource.Play();
    }
}

public static class AudioSourceExtensions
{
    public static void PlayRandomSound(this AudioSource source, SoundGroup soundGroup)
    {
        soundGroup.PlayRandomSound(source);
    }
}

public static class AudioUtil
{
    public static GameObject PlaySoundAtPos(Vector3 pos, AudioClip sound)
    {
        GameObject go = new GameObject("Sound");
        go.transform.position = pos;
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = sound;
        source.loop = false;
        source.Play();
        return go;
    }

    public static GameObject PlaySoundAtPos(Vector3 pos, SoundGroup group)
    {
        return PlaySoundAtPos(pos, group.GetRandomSound());
    }
}
