using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetProgressBar : NetworkBehaviour
{
    [SerializeField] private Image barImage;

    public override void Spawned()
    {
        gameObject.SetActive(false);
        barImage.fillAmount = 0;
    }

    public void SetBar(float percent)
    {
        if (percent > 0 && percent < 1)
            gameObject.SetActive(true);
        else gameObject.SetActive(false);
        barImage.fillAmount = percent;
    }
}
