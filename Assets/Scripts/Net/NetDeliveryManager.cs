using Fusion;
using FusionHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

public class NetDeliveryManager : NetworkBehaviour
{
    [SerializeField] private RecipeListSO recipeListSO;
    [SerializeField] private DeliveryManagerUI deliveryManagerUI;
    [Header("LevelInfo")]
    [SerializeField] private float countTime = 0;
    [SerializeField] private float spawnTime = 4;
    [SerializeField] private int maxListNum = 4;

    private int recipeDeliveredNum = 0;

    private ChangeDetector _changes;
    private List<RecipeSO> managerList = new List<RecipeSO>();
    [Networked, Capacity(4)]
    public NetworkArray<int> orderIDList { get; }
    [Networked]
    public int orderIndexPointer { get; set;} = 0;

    public override void Spawned()
    {
        orderIndexPointer = 0;
        _changes = GetChangeDetector(ChangeDetector.Source.SimulationState);
        Runner.RegisterSingleton<NetDeliveryManager>(this);
    }

    public override void Render()
    {
        foreach (var change in _changes.DetectChanges(this, out var previousBuffer, out var currentBuffer))
        {
            switch (change)
            {
                case nameof(orderIndexPointer):
                    var reader = GetPropertyReader<int>(nameof(orderIndexPointer));
                    var (previous, current) = reader.Read(previousBuffer, currentBuffer);
                    Debug.Log("pre" + previous + "curr" + current);
                    
                    if (current > previous)
                    {
                        int newRecipeID = recipeListSO.RecipeSOList[UnityEngine.Random.Range(0, recipeListSO.RecipeSOList.Count)].id;
                        orderIDList.Set(previous, newRecipeID);
                    }
                    else
                    {
                        orderIDList.Set(current, 0);
                    }
                    
                    break;

                case nameof(orderIDList):
                    ChangeListFromIndex();
                    deliveryManagerUI.UpdateRecipeUI(managerList);
                    break;
            }
        }
    }

    private void ChangeListFromIndex()
    {
        managerList.Clear();
        for(int i = 0; i < orderIndexPointer; i++)
        {
            foreach (var item in recipeListSO.RecipeSOList)
            {
                if (item.id == orderIDList[i])
                    managerList.Add(item);
            }
            
        }
    }

    private void Update()
    {
        if (managerList.Count < maxListNum) countTime += Time.deltaTime;
        if (countTime >= spawnTime && managerList.Count < maxListNum)
        {
            countTime = 0;
            //时间到了之后随机添加一个菜谱里的菜
            //managerList.Add(recipeListSO.RecipeSOList[UnityEngine.Random.Range(0, recipeListSO.RecipeSOList.Count)]);
            orderIndexPointer++;

            // Update UI
            
            // OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
        }
    }

    public void DeliverRecipe(NetPlateKitchenObject plateKitchenObject)
    {
        Dictionary<KitchenObjectSO, int> plateContentMap = new Dictionary<KitchenObjectSO, int>();//利用Map进行比较
        foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.getKitchenObjectSOList())
        {
            if (plateContentMap.ContainsKey(plateKitchenObjectSO)) plateContentMap[plateKitchenObjectSO]++;
            else plateContentMap.Add(plateKitchenObjectSO, 1);
        }
        Dictionary<KitchenObjectSO, int> recipeContentMap = new Dictionary<KitchenObjectSO, int>();
        for (int i = 0; i < managerList.Count; i++)
        {
            RecipeSO thisRecipeSO = managerList[i];
            if (thisRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.getKitchenObjectSOList().Count)
            {
                // 订单中的菜品与送上去的菜品由同样数量的物品组成
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in thisRecipeSO.kitchenObjectSOList)
                {
                    if (recipeContentMap.ContainsKey(recipeKitchenObjectSO)) recipeContentMap[recipeKitchenObjectSO]++;
                    else recipeContentMap.Add(recipeKitchenObjectSO, 1);
                }
                bool isRecipeEqualsPlate = false;
                //判断Map是否相等
                foreach (var kitchenObjectSO_int in plateContentMap)
                {
                    if (recipeContentMap.ContainsKey(kitchenObjectSO_int.Key) && recipeContentMap[kitchenObjectSO_int.Key] == kitchenObjectSO_int.Value)
                        isRecipeEqualsPlate = true;
                    else
                    {
                        isRecipeEqualsPlate = false;
                        break;
                    }
                }
                if (isRecipeEqualsPlate)//哈希表结果一致
                {
                    //managerList.RemoveAt(i);
                    orderIndexPointer--;
                    recipeDeliveredNum++;
                    // OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    // Update UI Sound
                    Debug.Log("Complete");
                    return;
                }
                recipeContentMap.Clear();
            }
        }
        // 遍历了所有订单，没有找到匹配的订单
        // Update Sound OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> getManagerList()
    {
        return managerList;
    }

    public string getRecipeDeliveredNum()//UI调用接口
    {
        return recipeDeliveredNum.ToString();
    }
}
