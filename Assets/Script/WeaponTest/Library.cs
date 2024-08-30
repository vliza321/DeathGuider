using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class _INode<T>
{
	public T data;
	public _INode<T> next;

	public _INode(T Data, _INode<T> Next)
	{
		data = Data;
		next = Next;
	}

	~_INode()
	{
		data = default;
		next = null;
	}
}
public class _IQueue<T>
{
	private _INode<T> _dummy;
	private _INode<T> _cashingDummy;
	private int _size;
	public _INode<T> Rear()
	{
		return _dummy.next;
	}

	public int _Size
	{
		get { return _size; }
	}


	public _IQueue(T dummyObject)
	{
		_dummy = new _INode<T>(dummyObject, _dummy);
		_cashingDummy = new _INode<T>(dummyObject, null);
	}

	public void Enqueue(T addNode)
	{

	}

	public void Dequeue(T removeNode)
	{

	}
}
