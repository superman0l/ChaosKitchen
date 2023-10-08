using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> managerList;
    private float countTime = 0;
    private float spawnTime = 4;
    private int maxListNum = 4;

    private int recipeDeliveredNum = 0;

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeFailed;

    private void Awake()
    {
        Instance = this;
        managerList = new List<RecipeSO>();
    }

    public List<RecipeSO> getManagerList()
    {
        return managerList;
    }

    private void Update()
    {
        if (managerList.Count < maxListNum) countTime += Time.deltaTime;
        if (countTime >= spawnTime && managerList.Count < maxListNum)
        {
            countTime = 0;
            //ʱ�䵽��֮��������һ��������Ĳ�
            managerList.Add(recipeListSO.RecipeSOList[UnityEngine.Random.Range(0, recipeListSO.RecipeSOList.Count)]);
            OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
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
                    managerList.RemoveAt(i);
                    recipeDeliveredNum++;
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    return;
                }
                recipeContentMap.Clear();
            }
        }
        // ���������ж�����û���ҵ�ƥ��Ķ���
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public string getRecipeDeliveredNum()//UI���ýӿ�
    {
        return recipeDeliveredNum.ToString();
    }
}
