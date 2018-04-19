using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS_Core.Pooling
{
    /// <summary>
    /// 
    /// </summary>
    public interface KS_IPoolObject
    {
        int _Id { get; }
        GameObject GameObject { get; }
        PoolObjectSettings PoolSettings();
        bool HasId { get; }
        T GetConcreteType<T>();
    }
}