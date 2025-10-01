using UnityEngine;

namespace IRM
{
    public class RangeAttacker : MonoBehaviour, IAttacker
    {
        [SerializeField] private BulletObjectPoolData objectPool;
        [SerializeField] private Transform bulletPivot;
        
        public void DoDamage(Vector3 attackerPosition, Transform target)
        {
            var bullet = objectPool.Get(bulletPivot.position, Quaternion.identity);
            var direction = (target.position - attackerPosition).normalized;
            
            bullet.Initialize(direction);
            bullet.Pool = objectPool;
        }
    }
}