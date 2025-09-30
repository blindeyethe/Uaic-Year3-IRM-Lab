using UnityEngine;

namespace IRM
{
    public class MeleeAttacker : MonoBehaviour, IAttacker
    {
        public void DoDamage(Vector3 attackerPosition, Transform target)
        {
            var damageable = target.GetComponent<IDamageable>();
            damageable.TakeDamage();
        }
    }
}