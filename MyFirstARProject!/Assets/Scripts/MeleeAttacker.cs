using System;
using UnityEngine;

namespace IRM
{
    public class MeleeAttacker : MonoBehaviour, IAttacker
    {
        [SerializeField] private int attackDamage;

        public void DoDamage(Transform target)
        {
            var damageable = target.GetComponent<IDamageable>();
            damageable.TakeDamage(attackDamage);
        }
    }
}