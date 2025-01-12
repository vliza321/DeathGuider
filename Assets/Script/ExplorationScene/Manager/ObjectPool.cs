using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
	
	private GameObject[] prefab;  // 풀링할 객체의 프리팹 -> 2개 

	private int poolSize;  // 풀의 초기 크기

	private Queue<GameObject> poolQueue;

	public ObjectPool(GameObject[] prefab, int MaxMonster)
    {
        poolQueue = new Queue<GameObject>();
        /*
        for (int i = 0; i < MaxMonster; i++)
        {
            GameObject obj = CreateNewObject();
            obj.SetActive(false);
            poolQueue.Enqueue(obj);
        }*/
    }

    public Queue<GameObject> PoolQueue
    {
        get { return poolQueue; }
    }
    public ObjectPool(int MaxMonster)
    {
        poolQueue = new Queue<GameObject>();
        /*
        for (int i = 0; i < MaxMonster; i++)
        {
            GameObject obj = CreateNewObject();
            obj.SetActive(false);
            poolQueue.Enqueue(obj);
        }*/
    }

    public GameObject GetObject()
    {
        if (poolQueue.Count > 0)
        {
            GameObject obj = poolQueue.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            return default;
        }
    }

    public void ReturnObject(GameObject obj)
	{
		obj.SetActive(false);
		poolQueue.Enqueue(obj);
	}
}