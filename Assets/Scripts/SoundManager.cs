using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    public static SoundManager Instance { get; private set; }

    private float volume = 0.8f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeCompleted += RecipeCompleteSound;
        DeliveryManager.Instance.OnRecipeFailed += RecipeFailSound;
        CuttingCounter.OnAnyCut += CuttingSound;
        Player.Instance.OnGrabObject += GrabSound;
        BaseCounter.OnDropObject += DropSound;
        TrashCounter.OnTrash += TrashSound;
    }

    public void FootStepSound(Vector3 position)
    {
        PlaySound(audioClipRefsSO.footstep, position);
    }

    private void TrashSound(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.trash, (sender as TrashCounter).transform.position);
    }

    private void DropSound(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectDrop, (sender as BaseCounter).transform.position);
    }

    private void GrabSound(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position);
    }

    private void CuttingSound(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.chop, (sender as CuttingCounter).transform.position);
    }

    private void RecipeCompleteSound(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliverySuccess, DeliveryCounter.Instance.transform.position);
    }

    private void RecipeFailSound(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliveryFail, DeliveryCounter.Instance.transform.position);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], position, volume);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume_coefficient = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume * volume_coefficient);
    }

    public void ChangeVolume()
    {
        volume += 0.1f;
        if (volume > 1.05f)
        {
            volume = 0;
        }
    }

    public float GetVolume()
    {
        return volume;
    }
}
