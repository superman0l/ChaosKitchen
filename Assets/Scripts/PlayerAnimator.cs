using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private Animator playerAnimator;
    private GameObject Player;

    private void Awake(){
        playerAnimator = GetComponent<Animator>();
        Player = GameObject.Find("Player");
    }

    private void Update(){
        playerAnimator.SetBool(IS_WALKING,Player.GetComponent<Player>().isWalking());
    }
}
