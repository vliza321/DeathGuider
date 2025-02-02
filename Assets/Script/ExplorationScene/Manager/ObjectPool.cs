using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectPool<T> where T : autorizedObject
{
	private int poolSize;  // 풀의 초기 크기

	private Queue<T> poolQueue;

    public Queue<T> PoolQueue
    {
        get { return poolQueue; }
    }
    public ObjectPool(int MaxSize)
    {
        poolSize = MaxSize;
        poolQueue = new Queue<T>(poolSize);
    }

    public T GetObject()
    {
        return poolQueue.Count > 0 ? ActivateAndReturn(poolQueue.Dequeue()) : null;
    }

    public T ActivateAndReturn(T obj)
    {
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReleaseObject(T obj)
	{
        if (poolQueue.Count >= poolSize) return;
		obj.gameObject.SetActive(false);
		poolQueue.Enqueue(obj);
	}
}