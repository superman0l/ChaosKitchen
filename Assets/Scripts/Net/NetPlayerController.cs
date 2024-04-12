using Cinemachine;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Fusion.Sockets.NetBitBuffer;

public class NetPlayerController : NetworkBehaviour, INetKitchenObjectParent
{
    [Header("BasicInfo")]
    [SerializeField] private float moveSpeed = 7.0f;
    [SerializeField] private float rotateSpeed = 10.0f;
    [SerializeField] private Transform KitchenObjectPos;

    [Header("Net")]
    [SerializeField]
    NetworkCharacterController networkCharacterController;

    private bool is_Walking;
    private GameInput gameInput;
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private ChangeDetector _changes;
    private NetBaseCounter selectedCounter;
    private NetKitchenObject kitchenObject;

    [Networked] public NetworkButtons ButtonsPrevious { get; set; }

    public override void Spawned()
    {
        _changes = GetChangeDetector(ChangeDetector.Source.SimulationState);
        if (Object.HasInputAuthority)
        {
            cinemachineVirtualCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
            cinemachineVirtualCamera.Follow = transform;
            cinemachineVirtualCamera.LookAt = transform;
        }
        
    }

    public override void FixedUpdateNetwork()
    {
        HandleIntersections();

        if (GetInput(out PlayerInput data))
        {
            NetworkButtons buttons = data.buttons;
            var pressed = buttons.GetPressed(ButtonsPrevious);
            ButtonsPrevious = buttons; // tick¸üÐÂbutton×´Ì¬

            if (pressed.IsSet(PlayerInputButtons.Intersect))
            {
                selectedCounter?.Intersect(this);
            }

            if (pressed.IsSet(PlayerInputButtons.Cut))
            {
                selectedCounter?.IntersectAlternate(this);
            }

            // move
            Vector2 inputVector = data.moveDir.normalized;
            Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
            networkCharacterController.Move(moveSpeed * moveDir * Runner.DeltaTime);
        }
    }

    private void HandleIntersections()
    {
        float rayLength = 1.3f;
        RaycastHit hitinfo;
        LayerMask countersMask = LayerMask.GetMask("Counters");
        if (Physics.Raycast(transform.position, transform.forward, out hitinfo, rayLength, countersMask) &&
            hitinfo.transform.TryGetComponent(out NetBaseCounter netBaseCounter))//layermask 6 Counters 
        {
            if(selectedCounter != netBaseCounter)
            {
                IntersectEffect(selectedCounter, netBaseCounter);
                selectedCounter = netBaseCounter;
            }
        }
        else
        {
            if(selectedCounter) IntersectEffect(selectedCounter, null);
            selectedCounter = null;
        }
    }

    public override void Render()
    {
        foreach (var change in _changes.DetectChanges(this, out var previousBuffer, out var currentBuffer))
        {
            switch (change)
            {
            }
        }
    }

    private void IntersectEffect(NetBaseCounter previous, NetBaseCounter current)
    {
        if (Object.HasInputAuthority)
        {
            previous?.OnUnselected();
            current?.OnSelected();
        }
    }

    public Transform getKitchenObjectPos()
    {
        return KitchenObjectPos;
    }

    public void setKitchenObject(NetKitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public NetKitchenObject getKitchenObject()
    {
        return kitchenObject;
    }

    public void clearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool hasKitchenObject()
    {
        return kitchenObject != null;
    }

    Transform INetKitchenObjectParent.getSelf()
    {
        return transform;
    }
}
