﻿using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    private Dictionary<GameObject, ObjectPool> pools = new Dictionary<GameObject, ObjectPool>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterPool(GameObject poolType, ObjectPool pool)
    {
        if (pools.ContainsKey(poolType)) return;
        pools.Add(poolType, pool);
    }

    public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!instance.pools.ContainsKey(prefab))
        {
            Debug.Log("no pool exists to spawn this item from");
            return null;
        }

        GameObject _poolObject = instance.pools[prefab].Spawn(position, rotation);
        return _poolObject;
    }

    public static void Despawn(GameObject poolObject)
    {
        bool _poolFound = instance.pools.TryGetValue(poolObject, out ObjectPool _pool);
        if (!_poolFound)
        {
            Debug.Log("attempting to despawn an item with no pool");
            return;
        }

        _pool.Despawn(poolObject);
    }
}