using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject poolObject = default;
    [SerializeField] private int preloadCount = 0;
    [SerializeField] private bool limit = false;
    [SerializeField] private int limitCount = 0;
    [Header("Pools Items (For Debugging Only)")]
    public List<GameObject> active = new List<GameObject>();
    public List<GameObject> inactive = new List<GameObject>();

    private Transform thisTransform;

    protected virtual void Awake()
    {
        thisTransform = transform;
    }

    // register pool with manager and pre-spawn all objects
    private void Start()
    {
        if (poolObject == null) return;
        PoolManager.instance.RegisterPool(poolObject, this);
        PreloadPool();
    }

    // will spawn and deactivate numerous objects to start scene with
    private void PreloadPool()
    {
        for (int i = PoolCount; i < preloadCount; i++)
        {
            GameObject obj = Instantiate(poolObject);
            obj.transform.SetParent(thisTransform);
            obj.SetActive(false);
            inactive.Add(obj);
        }
    }

    // returns number of items in pool, active and inactive combined
    private int PoolCount
    {
        get
        {
            int _total = 0;
            _total += active.Count;
            _total += inactive.Count;
            return _total;
        }
    }

    // spawn an object
    public GameObject Spawn(Vector3 position,Quaternion rotation)
    {
        GameObject _spawnedObject;
        Transform _spawnedObjectTransform;

        if(inactive.Count > 0)
        {
            _spawnedObject = inactive[0];
            _spawnedObjectTransform = _spawnedObject.transform;
            inactive.RemoveAt(0);
        }
        else
        {
            if(limit && active.Count >= limitCount)
            {
                Debug.Log("pool limit reached", this);
                return null;
            }
            _spawnedObject = Instantiate(poolObject);
            _spawnedObjectTransform = _spawnedObject.transform;
        }

        _spawnedObjectTransform.position = position;
        _spawnedObjectTransform.rotation = rotation;
        _spawnedObjectTransform.parent = thisTransform;

        active.Add(_spawnedObject);
        _spawnedObject.SetActive(true);

        return _spawnedObject;
    }

    // despawn an object
    public virtual void Despawn(GameObject instance)
    {
        active.Remove(instance);
        inactive.Add(instance);
        instance.SetActive(false);
    }
}
