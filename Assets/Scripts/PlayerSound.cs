using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private float footstepTimer;
    private float footstepTimerMax = 0.1f;

    private void Update()
    {
        footstepTimer += Time.deltaTime;
        if (footstepTimer > footstepTimerMax)
        {
            footstepTimer = 0;
            if(Player.Instance.isWalking())SoundManager.Instance.FootStepSound(Player.Instance.transform.position);
        }
    }
}
