using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    private Dictionary<int, Queue<GameObject>> pools = new Dictionary<int, Queue<GameObject>>();

    public static ObjectPoolManager Instnace {get; private set; }

    public int ArrowCnt = 0;

    void Update()
    {
        ArrowCnt = pools[0].Count;
    }
    private void Awake()
    {
        Instnace = this;

        for (int i = 0; i < prefabs.Length; i++)
        {
            pools[i] = new Queue<GameObject>();
        }
    }

    public GameObject GetObject(int prefabIndex, Vector3 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(prefabIndex))
        {
            Debug.LogError($"Pool with index {prefabIndex} does not exist.");
            return null;
        }

        GameObject obj;
        if (pools[prefabIndex].Count > 0)
            obj = pools[prefabIndex].Dequeue();
        else
        {
            obj = Instantiate(prefabs[prefabIndex]);
            obj.GetComponent<IPoolable>().Initialize(obj => ReturnObject(prefabIndex, obj));
            
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.GetComponent<IPoolable>().OnSpawn();
        
        return obj;
    }
 
    private void ReturnObject(int prefabIndex, GameObject @object)
    {
       if (!pools.ContainsKey(prefabIndex))
       {
            Debug.LogError($"Pool with index {prefabIndex} does not exist.");
            Destroy(@object);
            return;
       }

       @object.SetActive(false);
       pools[prefabIndex].Enqueue(@object);
    }
}
