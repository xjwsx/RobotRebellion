using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [Serializable]
    public struct Pool
    {
        public GameObject prefab;
        public string name;
        public int size;
    }
    public static ObjectPoolManager instance;
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDict;

    private void Awake()
    {
        instance = this;
        InitializePool();
    }

    public void InitializePool()
    {
        poolDict = new Dictionary<string, Queue<GameObject>>();
        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDict.Add(pool.name, objectPool);
        }
    }
    public GameObject SpawnFromPool(string name)
    {
        if (!poolDict.ContainsKey(name))
            return null;

        GameObject obj = poolDict[name].Dequeue();
        poolDict[name].Enqueue(obj);
        return obj;
    }

}
