using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [Networked] TickTimer life { get; set; }

    [SerializeField] float bulletSpeed = 4f;
    [SerializeField] float lifeSecond = 5f;
    [SerializeField] int damage = 5;

    public override void Spawned()
    {
        life = TickTimer.CreateFromSeconds(Runner, lifeSecond); // 任何NetworkBehavior都可以调用Runner
    }
    public override void FixedUpdateNetwork()
    {
        if (life.Expired(Runner))
        {
            Runner.Despawn(Object);
        }
        else
        {
            transform.position += bulletSpeed * transform.forward * Runner.DeltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>();
            player.TakeDamage(damage);
            Runner.Despawn(Object);
        }
    }
}
