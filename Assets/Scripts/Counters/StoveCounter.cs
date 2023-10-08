using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StoveCounter : BaseCounter, IHasProgress
{
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    private State state;
    private float fryingTime = 0;
    private FryingObjectSO fryingObjectSO;
    [SerializeField] private FryingObjectSO[] fryingObjectSOs;

    public event EventHandler<onStateChangeEventArgs> onStateChange;
    public class onStateChangeEventArgs : EventArgs
    {
        public State state;
    }

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public override void Intersect(Player player)
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
            else if(player.getKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
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
            {}
            else//柜子上有物体 给角色
            {
                ChangeState(State.Idle);
                fryingTime = 0;
                getKitchenObject().setKitchenObjectParent(player);
            }
        }
    }

    private void Start()
    {
        ChangeState(State.Idle);
    }

    private void ChangeState(State targetState)
    {
        state = targetState;
        onStateChange?.Invoke(this, new onStateChangeEventArgs
        {
            state = state
        });

    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                if (hasKitchenObject())
                {
                    fryingTime = 0;
                    ChangeState(State.Frying);
                }
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0
                });
                break;
            case State.Frying:
                fryingTime += Time.deltaTime;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = (float)fryingTime / fryingObjectSO.FryingTime
                });
                if (fryingTime > (float)fryingObjectSO.FryingTime)
                {
                    getKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(fryingObjectSO.output, this);
                    fryingTime = 0;
                    fryingObjectSO = getFryingObjectSO(getKitchenObject().getKitchenObjectSO());
                    ChangeState(State.Fried);
                }
                break;
            case State.Fried:
                fryingTime += Time.deltaTime;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = (float)fryingTime / fryingObjectSO.FryingTime
                });
                if (fryingTime > (float)fryingObjectSO.FryingTime)
                {
                    getKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(fryingObjectSO.output, this);
                    fryingTime = 0;
                    ChangeState(State.Burned);
                }
                break;
            case State.Burned:
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0
                });
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
}
