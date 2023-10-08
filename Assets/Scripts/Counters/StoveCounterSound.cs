using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private void Start()
    {
        stoveCounter.onStateChange += StateSound;
    }

    private void StateSound(object sender, StoveCounter.onStateChangeEventArgs e)
    {
        if(e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried)
        {
            GetComponent<AudioSource>().Play();
            if(e.state == StoveCounter.State.Frying) GetComponent<AudioSource>().volume = 0.6f;
            else GetComponent<AudioSource>().volume = 1.0f;
        }
        else
        {
            GetComponent<AudioSource>().Pause();
        }
    }
}
