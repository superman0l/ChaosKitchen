using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveGameObject;
    [SerializeField] private GameObject particlesGameObject;

    private void Awake()
    {
        stoveCounter.onStateChange += FryingVisual;
        stoveCounter.onStateChange += FriedVisual;
    }

    private void FryingVisual(object sender, StoveCounter.onStateChangeEventArgs e)
    {
        if (e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried)
            stoveGameObject.SetActive(true);
        else stoveGameObject.SetActive(false);
    }

    private void FriedVisual(object sender, StoveCounter.onStateChangeEventArgs e)
    {
        if (e.state == StoveCounter.State.Fried)
            particlesGameObject.SetActive(true);
        else particlesGameObject.SetActive(false);
    }


}
