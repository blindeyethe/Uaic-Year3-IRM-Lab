using UnityEngine;

namespace IRM
{
    public class RangeAttacker : MonoBehaviour, IAttacker
    {
        [SerializeField] private BulletObjectPoolData objectPool;
        
        public void DoDamage(Vector3 attackerPosition, Transform target)
        {
            var bullet = objectPool.Get(transform.position, Quaternion.identity);
            var direction = (target.position - attackerPosition).normalized;
            
            bullet.Initialize(direction);
            bullet.Pool = objectPool;
        }

    }
}