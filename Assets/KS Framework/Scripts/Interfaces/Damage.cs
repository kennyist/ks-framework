using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS_Core
{
    public interface IDamageable
    {
        void TakeDamage(float damage);
        void TakeDamage(int damage);
    }

    public interface IHealable
    {
        void TakeHealth(float ammount);
    }
}