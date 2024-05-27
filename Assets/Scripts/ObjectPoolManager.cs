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
    private static ObjectPoolManager instance;
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDict;
    public static ObjectPoolManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ObjectPoolManager>();
                if (instance != null)
                {
                    instance.InitializePool();
                }
                else
                {
                    Debug.LogError("There needs to be one active ObjectPoolManager script on a GameObject in your scene.");
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitializePool();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
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
