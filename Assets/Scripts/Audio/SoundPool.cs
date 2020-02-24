using UnityEngine;

public class SoundPool : MonoBehaviour
{
    public static SoundPool instance;

    public SoundPoolable soundPoolable;
    public int poolSize;

    private SoundPoolable[] soundPoolables;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        soundPoolables = new SoundPoolable[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            soundPoolables[i] = Instantiate(soundPoolable);
        }
    }

    public SoundPoolable PlaySound(AudioClip clip, Vector3 location, bool threeDSound = true, bool loop = false, float volume = 1.0f)
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (soundPoolables[i].IsAvailable())
            {
                soundPoolables[i].Play(clip, location, threeDSound, loop, volume);
                return soundPoolables[i];
            }
        }
        Debug.LogWarning("Pool Warning: No audio sources available");
        return null;
    }
}
