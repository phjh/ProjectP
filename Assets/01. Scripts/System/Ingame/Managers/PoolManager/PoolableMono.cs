using UnityEngine;
public abstract class PoolableMono : MonoBehaviour
{
    public PoolingPair pair;

    private void Start()
    {
        pair.prefab = this.gameObject.GetComponent<PoolableMono>();
        PoolInit();
    }

    public abstract void PoolInit();//init Pooling Items

    public void CustomInstantiate(Vector2 pos, PoolObjectListEnum objenum)
    {
        Debug.Log("CustomInstantiate : " + objenum);
        PoolableMono poolItem = PoolManager.Instance.Pop(objenum);
        poolItem.transform.localPosition = pos;
    }

    public void CustomInstantiate(Vector2 pos, PoolObjectListEnum objenum, out PoolableMono poolItem)
	{
        Debug.Log("CustomInstantiate : " + objenum);
		poolItem = PoolManager.Instance.Pop(objenum);
		poolItem.transform.localPosition = pos;
	}



}
