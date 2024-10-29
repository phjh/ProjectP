using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public PoolingListSO PoolingSO;

    [HideInInspector]
    public Player player;

    [HideInInspector]
    public InGameFlow game;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        MakePool();
    }
  
    



    private void MakePool()
    {
        PoolManager.Instance = new PoolManager();

        GameObject obj = new GameObject();
        obj.transform.parent = this.transform;
        obj.name = "PoolItems";

        GameObject poolObj = new GameObject();
        poolObj.transform.parent = obj.transform;
        poolObj.name = "PoolingObjects";
        foreach (var item in PoolingSO.PoolObjectLists)
        {
            PoolManager.Instance.CreatePool(item, poolObj.transform);
        }

        GameObject poolEffect = new GameObject();
        poolEffect.transform.parent = obj.transform;
        poolEffect.name = "PoolingEffects";
        foreach (var item in PoolingSO.PoolEffectLists)
        {
            PoolManager.Instance.CreatePool(item, poolEffect.transform);
        }

        GameObject poolUI = new GameObject();
        poolUI.transform.parent = obj.transform;
        poolUI.name = "PoolingUI";
        foreach (var item in PoolingSO.PoolUILists)
        {
            PoolManager.Instance.CreatePool(item, poolUI.transform);
        }
    }

    public static void TestingDebug(string doing)
    {
        Debug.Log("Test Method is Invoking : " + doing);
    }

}
