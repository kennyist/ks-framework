using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class KS_PoolManager : MonoBehaviour {

    public KS_Scriptable_GameConfig gameConfig;
    public GameObject pooledObjectsContainer;

    private static KS_PoolManager instance;
    public static KS_PoolManager Instance
    {
        get
        {
            if(instance != null)
            {
                return instance;
            }
            else
            {
                return null;
            }
        }
    }

    Dictionary<int, KS_IPoolObject> pool = new Dictionary<int, KS_IPoolObject>();

    private int currentId = 0;

    private void Awake()
    {
        instance = this;

        _inactive = new GameObject("Inactive");
        _active = new GameObject("Active");

        _inactive.transform.parent = pooledObjectsContainer.transform;
        _active.transform.parent = pooledObjectsContainer.transform;

        KS_Manager.OnLoadLevel += LevelLoadStarted;
    }

    private void OnDestroy()
    {
        KS_Manager.OnLoadLevel -= LevelLoadStarted;
    }

    public void LevelLoadStarted(int i = 0)
    {
        if (gameConfig.pool_ClearOnLoadLevel)
        {
            Clear();
        }
    }

    GameObject _inactive;
    GameObject _active;

    public int GetUniqueId()
    {
        currentId++;
        return currentId;
    }

    public int GetAmmountOfType(string tag)
    {
        return pool.Count(f => f.Value.PoolSettings().tag == tag);
    }

    public void AddPoolObject(KS_IPoolObject poolObject)
    {
        KS_IPoolObject data;

        if (!pool.TryGetValue(poolObject._Id, out data))
        {

            // check pooling limits for 

            Debug.Log("A: " + GetAmmountOfType(poolObject.PoolSettings().tag) + " T: " + poolObject.PoolSettings().poolLimit);

            if (GetAmmountOfType(poolObject.PoolSettings().tag) > (poolObject.PoolSettings().poolLimit - 1))
            {
                RemovePoolItem(poolObject.GameObject);
                return;
            }
            else
            {
                poolObject.GameObject.transform.parent = _inactive.transform;

                pool.Add(poolObject._Id, poolObject);
                poolObject.GameObject.SetActive(false);
            }

        }
        else
        {
            
        }
    }

    public KS_IPoolObject Get(string tag)
    {
        var poolableObj = pool.Values.FirstOrDefault(f => f.PoolSettings().tag == tag);
        if (poolableObj != null)
        {
            pool.Remove(poolableObj._Id);
            poolableObj.GameObject.transform.SetParent(_active.transform);
            poolableObj.GameObject.SetActive(true);
            return poolableObj;
        }
        return null;
    }

    public void RemovePoolItem(GameObject poolObject)
    {
        KS_IPoolObject i = poolObject.GetComponent<KS_IPoolObject>();

        if (i.HasId)
        {
            pool.Remove(i._Id);
        }

        Destroy(poolObject);
    }

    //

    /// <summary>
    /// 
    /// </summary>
    public void Clear()
    {
        pool.Clear();
    }
}

public class PoolObjectSettings
{
    public string tag;
    public int poolLimit;

    public PoolObjectSettings(string tag, int maxPooledOfType)
    {
        this.tag = tag;
        this.poolLimit = maxPooledOfType;
    }
}

public interface KS_IPoolObject
{
    int _Id { get; }
    GameObject GameObject { get; }
    PoolObjectSettings PoolSettings();
    bool HasId { get; }
    T GetConcreteType<T>();
}

public abstract class KS_poolObject : MonoBehaviour, KS_IPoolObject
{
    private int? _id;

    public int _Id
    {
        get
        {
            if(_id != null)
            {
                return (int)_id;
            }
            else
            {
                _id = KS_PoolManager.Instance.GetUniqueId();
                return (int)_id;
            }
        }
    }

    public bool HasId
    {
        get
        {
            if(_id != null)
            {
                return true;
            }

            return false;
        }
    }

    public GameObject GameObject { get { return gameObject; } }

    /// <summary>
    /// Returns the concrete type of this PoolableType.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enemy"></param>
    /// <returns></returns>
    public T GetConcreteType<T>()
    {
        try
        {
            return (T)Convert.ChangeType(this, GetType());
        }
        catch (InvalidCastException)
        {
            return default(T);
        }
    }

    public virtual PoolObjectSettings PoolSettings()
    {
        return null;
    }

    public virtual void AddToPool()
    {
        // Add to pool.
        KS_PoolManager.Instance.AddPoolObject(this);
    }
}

