using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
	[SerializeField]
	private GameObject prefab;  // 풀링할 객체의 프리팹
	[SerializeField]
	private int poolSize;  // 풀의 초기 크기

	private Queue<GameObject> poolQueue;

	/*
	private void Awake() // -> Init()
	{
		poolSize = 10;
		poolQueue = new Queue<GameObject>();
		// 초기 풀 크기만큼 객체를 생성해 큐에 저장
		for (int i = 0; i < poolSize; i++)
		{
			GameObject obj = Instantiate(prefab);
			obj.SetActive(false);
			poolQueue.Enqueue(obj);
		}
	}

	public ObjectPool(int MaxMonster)
    {
		poolSize = MaxMonster;
    }
	/*
	private void Init()
    {
		poolSize = 10;
		poolQueue = new Queue<GameObject>();
		// 초기 풀 크기만큼 객체를 생성해 큐에 저장
		for (int i = 0; i < poolSize; i++)
		{
			GameObject obj = Instantiate(prefab);
			obj.SetActive(false);
			poolQueue.Enqueue(obj);
		}
	}
	*/

	// 풀에서 객체를 가져옴
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
			// 만약 풀이 비어있으면 새로운 객체를 생성하여 반환
			GameObject obj = Instantiate(prefab);
			obj.SetActive(true);
			return obj;
		}
	}

	// 객체를 다시 풀에 반환
	public void ReturnObject(GameObject obj)
	{
		obj.SetActive(false);
		poolQueue.Enqueue(obj);
	}
}