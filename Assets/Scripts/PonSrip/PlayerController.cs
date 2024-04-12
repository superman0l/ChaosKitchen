using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] NetworkCharacterController networkCharacterController = null;
    [SerializeField] float moveSpeed = 15f;
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] int maxHp = 100;
    [SerializeField] Image hpBar;
    [SerializeField] MeshRenderer meshRenderer;

    private ChangeDetector _changes;
    [Networked] public int Hp {  get; private set; }  
    [Networked] public NetworkButtons ButtonsPrevious {  get; set; } // 记录上一个tick按钮状态

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            _changes = GetChangeDetector(ChangeDetector.Source.SimulationState);
            Hp = maxHp;
        }
    }

    public void TakeDamage(int damage)
    {
        if(Object.HasStateAuthority)
            Hp -= damage;
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            NetworkButtons buttons = data.buttons;
            var pressed = buttons.GetPressed(ButtonsPrevious);
            ButtonsPrevious = buttons; // tick更新button状态

            Vector3 moveVector = data.movementInput.normalized;
            networkCharacterController.Move(moveSpeed * moveVector * Runner.DeltaTime);

            if (pressed.IsSet(InputButtons.JUMP)){
                networkCharacterController.Jump();
            }

            if (pressed.IsSet(InputButtons.FIRE))
            {
                Runner.Spawn(bulletPrefab,
                    transform.position + transform.TransformDirection(Vector3.forward),
                    Quaternion.LookRotation(transform.TransformDirection(Vector3.forward)),
                    Object.InputAuthority);
            }
        }
        
        if(Hp <= 0 || networkCharacterController.transform.position.y <= -5f)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        Hp = maxHp;
        networkCharacterController.transform.position = Vector3.up * 2;
    }

    public override void Render()
    {
        foreach (var change in _changes.DetectChanges(this, out var previousBuffer, out var currentBuffer))
        {
            switch (change)
            {
                case nameof(Hp):
                    var reader = GetPropertyReader<int>(nameof(Hp));
                    var (previous, current) = reader.Read(previousBuffer, currentBuffer);
                    hpBar.fillAmount = (float)current / maxHp;
                    break;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeColor_RPC(Color.red);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            ChangeColor_RPC(Color.green);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            ChangeColor_RPC(Color.blue);
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsHostPlayer)]
    private void ChangeColor_RPC(Color newColor)
    {
        meshRenderer.material.color = newColor;
    }
}
