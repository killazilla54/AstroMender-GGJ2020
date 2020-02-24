using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPoolable : MonoBehaviour
{
    public AudioSource source;

    private void Awake()
    {
        if (source == null)
        {
            source = GetComponent<AudioSource>();
        }
    }

    public bool IsAvailable()
    {
        return source.clip == null || source.isPlaying == false;
    }

    public void Play(AudioClip clip, Vector3 location, bool threeDSound = true, bool loop = false, float volume = 1.0f)
    {
        transform.position = location;
        if (threeDSound)
        {
            source.spatialBlend = 1.0f;
        }
        else
        {
            source.spatialBlend = 0.0f;
        }
        source.clip = clip;
        source.loop = loop;
        source.volume = volume;
        source.Play();
    }

    public void Stop()
    {
        source.clip = null;
        source.Stop();
    }
}
