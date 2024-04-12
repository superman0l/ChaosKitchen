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
            //ʱ�䵽��֮��������һ��������Ĳ�
            //managerList.Add(recipeListSO.RecipeSOList[UnityEngine.Random.Range(0, recipeListSO.RecipeSOList.Count)]);
            orderIndexPointer++;

            // Update UI
            
            // OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
        }
    }

    public void DeliverRecipe(NetPlateKitchenObject plateKitchenObject)
    {
        Dictionary<KitchenObjectSO, int> plateContentMap = new Dictionary<KitchenObjectSO, int>();//����Map���бȽ�
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
                // �����еĲ�Ʒ������ȥ�Ĳ�Ʒ��ͬ����������Ʒ���
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in thisRecipeSO.kitchenObjectSOList)
                {
                    if (recipeContentMap.ContainsKey(recipeKitchenObjectSO)) recipeContentMap[recipeKitchenObjectSO]++;
                    else recipeContentMap.Add(recipeKitchenObjectSO, 1);
                }
                bool isRecipeEqualsPlate = false;
                //�ж�Map�Ƿ����
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
                if (isRecipeEqualsPlate)//��ϣ����һ��
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
        // ���������ж�����û���ҵ�ƥ��Ķ���
        // Update Sound OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> getManagerList()
    {
        return managerList;
    }

    public string getRecipeDeliveredNum()//UI���ýӿ�
    {
        return recipeDeliveredNum.ToString();
    }
}
