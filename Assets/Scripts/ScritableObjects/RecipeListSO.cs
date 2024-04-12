using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeListSO : ScriptableObject
{
    [Serializable]
    public struct DicItem
    {
        public int id;
        public RecipeSO recipeSO;
    }
    public List<RecipeSO> RecipeSOList;
    public List<DicItem> dicItems;
}
