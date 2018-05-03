using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS_Core { 

    /// <summary>
    /// Allow object to take damage from sources
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        /// On take damage from source
        /// </summary>
        /// <param name="damage">Damage ammount</param>
        /// <param name="source">Source object</param>
        void TakeDamage(float damage, GameObject source = null);
    }

    /// <summary>
    /// Allow object to take healing from sources
    /// </summary>
    public interface IHealable
    {
        /// <summary>
        /// On heal from source
        /// </summary>
        /// <param name="ammount">Healing ammount</param>
        /// <param name="source">Source of the healing</param>
        void TakeHealth(float ammount, GameObject source = null);
    }
}