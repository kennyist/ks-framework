using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using KS_Core;

namespace KS_Core.Pooling
{
    /// <summary>
    /// KS pool manager, this script handles the pooling, fetching and storage of game objects.
    /// </summary>
    public class KS_PoolManager : MonoBehaviour
    {
        /// <summary>
        /// Game config <see cref="KS_Scriptable_GameConfig"/>
        /// </summary>
        public KS_Scriptable_GameConfig gameConfig;
        /// <summary>
        /// Object to use as parent to pool objects while dissabled
        /// </summary>
        public GameObject pooledObjectsContainer;

        private static KS_PoolManager instance;
        /// <summary>
        /// Current active instance of KS_PoolManager
        /// </summary>
        public static KS_PoolManager Instance
        {
            get
            {
                if (instance != null)
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

        /// <summary>
        /// Get new unique id for pool object
        /// </summary>
        /// <returns></returns>
        public int GetUniqueId()
        {
            currentId++;
            return currentId;
        }

        /// <summary>
        /// How many objects of type are stored
        /// </summary>
        /// <param name="tag">object tag</param>
        /// <returns>Int count of objects</returns>
        public int GetAmountOfType(string tag)
        {
            return pool.Count(f => f.Value.PoolSettings().tag == tag);
        }

        /// <summary>
        /// Add new object to pool
        /// </summary>
        /// <param name="poolObject">Pool objects interface</param>
        public void AddPoolObject(KS_IPoolObject poolObject)
        {
            KS_IPoolObject data;

            if (!pool.TryGetValue(poolObject._Id, out data))
            {

                // check pooling limits for 

                Debug.Log("A: " + GetAmountOfType(poolObject.PoolSettings().tag) + " T: " + poolObject.PoolSettings().poolLimit);

                if (GetAmountOfType(poolObject.PoolSettings().tag) > (poolObject.PoolSettings().poolLimit - 1))
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

        /// <summary>
        /// Get pool object by tag
        /// </summary>
        /// <param name="tag">Game objects pool tag</param>
        /// <returns>Pool object interface</returns>
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

        /// <summary>
        /// Remove object from pool
        /// </summary>
        /// <param name="poolObject">Game object to reomve (Must use KS_IPoolObject interface) <see cref="KS_IPoolObject"/></param>
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
        /// Clear al objects in the pool
        /// </summary>
        public void Clear()
        {
            pool.Clear();
        }
    }

    /// <summary>
    /// Filled pool object interface for inheriting (MonoBehaviour)
    /// </summary>
    public abstract class KS_poolObject : MonoBehaviour, KS_IPoolObject
    {
        private int? _id;

        /// <summary>
        /// Pool object ID 
        /// </summary>
        public int _Id
        {
            get
            {
                if (_id != null)
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

        /// <summary>
        /// Does object have ID
        /// </summary>
        public bool HasId
        {
            get
            {
                if (_id != null)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Pool object GameObject
        /// </summary>
        public GameObject GameObject { get { return gameObject; } }

        /// <summary>
        /// Returns the concrete type of this Poolable Type.
        /// </summary>
        /// <typeparam name="T">Poolable Type</typeparam>
        /// <returns>concrete type of this Poolable Type</returns>
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

        /// <summary>
        /// Get Pool object settings (Must be filled on pool object) <see cref="PoolObjectSettings"/>
        /// </summary>
        /// <returns>Filled poolObjectSettings</returns>
        public virtual PoolObjectSettings PoolSettings()
        {
            return null;
        }

        /// <summary>
        /// Add game object to pool
        /// </summary>
        public virtual void AddToPool()
        {
            // Add to pool.
            KS_PoolManager.Instance.AddPoolObject(this);
        }
    }

}