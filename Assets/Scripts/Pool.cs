using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : SingletonMonoBehaviour<Pool>
{
	public GameObject PooledGameObjects;

	private Dictionary<string, GameObject> originals = new Dictionary<string, GameObject>();

	private Dictionary<string, Queue<GameObject>> pool = new Dictionary<string, Queue<GameObject>>();

	private Dictionary<string, int> counter = new Dictionary<string, int>();

	public void WarmUp(string key, GameObject original, int poolSize)
	{
		IEnumerator enumerator = WarmUpCoroutine(key, original, poolSize);
		while (enumerator.MoveNext())
		{
		}
	}

	public IEnumerator WarmUpCoroutine(string key, GameObject original, int poolSize)
	{
		if (!pool.ContainsKey(key))
		{
			pool[key] = new Queue<GameObject>(poolSize * 2);
		}
		int count = poolSize - pool[key].Count;
		if (count > 0)
		{
			for (int i = 0; i < count; i++)
			{
				GameObject copy = Object.Instantiate(original);
				copy.name = original.name;
				pool[key].Enqueue(copy);
				copy.transform.parent = PooledGameObjects.transform;
				yield return null;
			}
		}
		originals[key] = original;
	}

	public bool ContainsKey(string key)
	{
		return originals.ContainsKey(key) && originals[key] != null;
	}

	public bool ContainsObjects(string key)
	{
		return pool.ContainsKey(key) && pool[key].Count > 0;
	}

	public GameObject Alloc(string key)
	{
		if (!pool.ContainsKey(key))
		{
			UnityEngine.Debug.LogError("Need to call Warmup before Alloc for key: " + key);
		}
		if (pool[key].Count == 0)
		{
			WarmUp(key, originals[key], 1);
			if (Application.isEditor)
			{
				UnityEngine.Debug.LogWarning("Pool run out of " + key);
			}
		}
		if (!counter.ContainsKey(key))
		{
			counter[key] = 0;
		}
		Dictionary<string, int> dictionary;
		string key2;
		(dictionary = counter)[key2 = key] = dictionary[key2] + 1;
		return pool[key].Dequeue();
	}

	public void Free(GameObject copy)
	{
		if (copy == null || copy.transform.parent == PooledGameObjects.transform)
		{
			return;
		}
		string name = copy.name;
		if (pool.ContainsKey(name))
		{
			if (counter.ContainsKey(name))
			{
				Dictionary<string, int> dictionary;
				string key;
				(dictionary = counter)[key = name] = dictionary[key] - 1;
			}
			pool[name].Enqueue(copy);
			copy.transform.parent = PooledGameObjects.transform;
		}
	}
}
