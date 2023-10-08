using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    private float volume = 0.3f;

    private void Awake()
    {
        Instance = this;

        audioSource.volume = volume;
    }

    public void ChangeVolume()
    {
        volume += 0.1f;
        if (volume > 1.05f)
        {
            volume = 0;
        }
        audioSource.volume = volume;
    }

    public float GetVolume()
    {
        return volume;
    }
}
