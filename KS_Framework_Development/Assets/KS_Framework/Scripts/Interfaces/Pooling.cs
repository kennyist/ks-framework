using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS_Core.Pooling
{
    /// <summary>
    /// Allow object to be pooled <see cref="KS_poolObject"/>
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