using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Cinemachine;

public class CameraSet : NetworkBehaviour
{
    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
    NetPlayerController netPlayerController;

    void Start()
    {
        if (Object.HasStateAuthority)
        {
            cinemachineVirtualCamera.Follow = netPlayerController.transform;
            cinemachineVirtualCamera.LookAt = netPlayerController.transform;
        }
    }

        // Update is called once per frame
        void Update()
    {
        
    }
}
