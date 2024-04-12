using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StoveCounter;

public class NetStoveCounter : NetBaseCounter
{
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    private State state;
    private ChangeDetector _changes;
    private FryingObjectSO fryingObjectSO;
    [SerializeField] private FryingObjectSO[] fryingObjectSOs;
    [SerializeField] private NetProgressBar progress;

    [Networked] public float fryingTime { get; private set; }
    [Networked] public float maxFryingTime { get; private set; }

    public override void Intersect(NetPlayerController player)
    {
        if (player.hasKitchenObject())//角色持有物品
        {
            //角色手上的东西不能烤
            if ((getFryingObjectSO(player.getKitchenObject().getKitchenObjectSO()) != null) && !hasKitchenObject())
            {
                fryingTime = 0;
                player.getKitchenObject().setKitchenObjectParent(this);
                fryingObjectSO = getFryingObjectSO(getKitchenObject().getKitchenObjectSO());
            }
            else if (player.getKitchenObject().TryGetPlate(out NetPlateKitchenObject plateKitchenObject))
            {
                if (plateKitchenObject.AddList(getKitchenObject().getKitchenObjectSO()))
                    getKitchenObject().DestroySelf();
                ChangeState(State.Idle);
                fryingTime = 0;
            }
        }
        else
        {
            if (!hasKitchenObject())
            { }
            else//柜子上有物体 给角色
            {
                ChangeState(State.Idle);
                fryingTime = 0;
                getKitchenObject().setKitchenObjectParent(player);
            }
        }
    }

    public override void Spawned()
    {
        ChangeState(State.Idle);
        _changes = GetChangeDetector(ChangeDetector.Source.SimulationState);
    }

    public override void Render()
    {
        foreach (var change in _changes.DetectChanges(this, out var previousBuffer, out var currentBuffer))
        {
            switch (change)
            {
                case nameof(fryingTime):
                    var reader = GetPropertyReader<float>(nameof(fryingTime));
                    var (previous, current) = reader.Read(previousBuffer, currentBuffer);
                    float percent = current / maxFryingTime;
                    progress.SetBar(percent);
                    break;
            }
        }
    }

    private void ChangeState(State targetState)
    {
        state = targetState;
        /*onStateChange?.Invoke(this, new onStateChangeEventArgs
        {
            state = state
        });*/

    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                if (hasKitchenObject())
                {
                    maxFryingTime = fryingObjectSO.FryingTime;
                    fryingTime = 0;
                    ChangeState(State.Frying);
                }
                break;
            case State.Frying:
                maxFryingTime = fryingObjectSO.FryingTime;
                fryingTime += Time.deltaTime;
                if (fryingTime > (float)fryingObjectSO.FryingTime)
                {
                    getKitchenObject().DestroySelf();
                    SpawnNetKitchenObject(fryingObjectSO.output);
                    fryingTime = 0;
                    maxFryingTime = fryingObjectSO.FryingTime;
                    fryingObjectSO = getFryingObjectSO(getKitchenObject().getKitchenObjectSO());
                    ChangeState(State.Fried);
                }
                break;
            case State.Fried:
                maxFryingTime = fryingObjectSO.FryingTime;
                fryingTime += Time.deltaTime;
                if (fryingTime > (float)fryingObjectSO.FryingTime)
                {
                    getKitchenObject().DestroySelf();
                    SpawnNetKitchenObject(fryingObjectSO.output);
                    maxFryingTime = fryingObjectSO.FryingTime;
                    fryingTime = 0;
                    ChangeState(State.Burned);
                }
                break;
            case State.Burned:
                break;
        }
    }

    private FryingObjectSO getFryingObjectSO(KitchenObjectSO kitchenObjectSOInput)
    {
        foreach (var fryingObjectSO in fryingObjectSOs)
        {
            if (fryingObjectSO.input == kitchenObjectSOInput)
                return fryingObjectSO;
        }
        return null;
    }

    public void SpawnNetKitchenObject(KitchenObjectSO spawnObject)
    {
        NetKitchenObject netKitchenObject = spawnObject.prefab.GetComponent<NetKitchenObject>();
        Transform target = getKitchenObjectPos();
        var kitchenObjectTransform = Runner.Spawn(netKitchenObject,
                target.position,
                target.rotation,
                Object.InputAuthority);
        kitchenObjectTransform.setKitchenObjectParent(this);
    }
}
