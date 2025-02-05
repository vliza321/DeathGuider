using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : autorizedObject
{
	private int MaxSize;  // 풀의 초기 크기

	private Queue<T> poolQueue;

    private T CachingObject;

    public Queue<T> PoolQueue
    {
        get { return poolQueue; }
    }

    public ObjectPool(int MaxSize)
    {
        this.MaxSize = MaxSize;
        poolQueue = new Queue<T>(this.MaxSize);
    }

    public T GetObject()
    {
        if (poolQueue.Count <= 0) return null;
       
        CachingObject = poolQueue.Dequeue();
        CachingObject.gameObject.SetActive(true);
        return CachingObject;
    }

    public bool ReleaseObject(T obj)
	{
        if (poolQueue.Count >= MaxSize) return true;
		obj.gameObject.SetActive(false);
		poolQueue.Enqueue(obj);
        return false;
	}
}

public class ObjectPool
{
    private int MaxSize;

    //2단계 public -> private
    private Queue<GameObject> PoolQueue { get; }

    public ObjectPool(int maxSize)
    {
        this.MaxSize = maxSize;
        PoolQueue = new Queue<GameObject>(maxSize);
    }

    public GameObject OnGetObject()
    {
        if(PoolQueue.Count == 0)
        {
            return default;
        }
        else
        {
            GameObject obj = PoolQueue.Dequeue();
            obj.SetActive(true);
            return obj;
        }
    }

    public bool OnRelease(GameObject obj)
    {
        //1단계 별도의 삭제 없이 거부
        if (PoolQueue.Count >= MaxSize) return true;
        obj.SetActive(false);
        PoolQueue.Enqueue(obj);
        return false;
    }
}