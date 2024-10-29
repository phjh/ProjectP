using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager 
{
    public static PoolManager Instance;

    private Dictionary<PoolObjectListEnum, Pool<PoolableMono>> ObjectPoolingList = new();
    private Dictionary<PoolEffectListEnum, Pool<PoolableMono>> EffectPoolingList = new();
    private Dictionary<PoolUIListEnum, Pool<PoolableMono>> UIPoolingList = new();

    #region Improved PoolManager

    public void CreatePool(PoolingPair pair, Transform parent)
    {
        Pool<PoolableMono> pool = new Pool<PoolableMono>(pair.prefab, parent, pair.count);
        if (pool == null)
            Debug.LogError("pool is null");
        ObjectPoolingList.Add(pair.enumtype, pool);
    }

    public void CreatePool(EffectPoolingPair pair, Transform parent)
    {
        Pool<PoolableMono> pool = new Pool<PoolableMono>(pair.prefab, parent, pair.count);
        EffectPoolingList.Add(pair.enumtype, pool);
    }

    public void CreatePool(UIPoolingPair pair, Transform parent)
    {
        Pool<PoolableMono> pool = new Pool<PoolableMono>(pair.prefab, parent, pair.count);
        UIPoolingList.Add(pair.enumtype, pool);
    }

    public PoolableMono Pop(PoolObjectListEnum enumlist)
    {
        if (enumlist == PoolObjectListEnum.None)
        {
            Debug.LogError("enumlist is null");
            return null;
        }

        if (!ObjectPoolingList.ContainsKey(enumlist))
        {
            Debug.LogError($"Prefab {enumlist.ToString()} doesnt exist on pool");
            return null;
        }
        PoolableMono item = ObjectPoolingList[enumlist].Pop();
        item.PoolInit();
        return item;
    }

    public PoolableMono Pop(PoolEffectListEnum enumlist)
    {
        if (!EffectPoolingList.ContainsKey(enumlist))
        {
            Debug.LogError($"Prefab - {enumlist.ToString()} doesnt exist on pool");
            return null;
        }
        PoolableMono item = EffectPoolingList[enumlist].Pop();
        item.PoolInit();
        return item;
    }

    public PoolableMono Pop(PoolUIListEnum enumlist)
    {
        if (!UIPoolingList.ContainsKey(enumlist))
        {
            Debug.LogError("Prefab doesnt exist on pool");
            return null;
        }
        PoolableMono item = UIPoolingList[enumlist].Pop();
        //item.ResetPoolingItem();
        return item;
    }

    public void Push(PoolableMono obj, PoolObjectListEnum enumlist)
    {
        ObjectPoolingList[enumlist].Push(obj);
    }

    public void Push(PoolableMono obj, PoolEffectListEnum enumlist)
    {
        EffectPoolingList[enumlist].Push(obj);
    }

    public void Push(PoolableMono obj, PoolUIListEnum enumlist)
    {
        UIPoolingList[enumlist].Push(obj);
    }

    #endregion

}