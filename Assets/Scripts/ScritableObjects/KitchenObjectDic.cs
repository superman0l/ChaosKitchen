using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class KitchenObjectDic : ScriptableObject
{
    [Serializable]
    public struct pair
    {
        public int id;
        public KitchenObjectSO kitchenObjectSO;
    }

    public pair[] contents;
}
