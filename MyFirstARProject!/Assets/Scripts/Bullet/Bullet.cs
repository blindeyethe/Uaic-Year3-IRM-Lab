using UnityEngine;
using TheBlindEye.ObjectPoolSystem;

namespace IRM
{
    internal sealed class Bullet : PoolObject<Bullet>
    {
        [SerializeField] private float speed = 10f;

        private Transform _transform;
        private Vector3 _direction;

        private void Awake() =>
            _transform = transform;

        public void Initialize(Vector3 direction) =>
            _direction = direction;

        private void Update() =>
            _transform.position += _direction * (speed * Time.deltaTime);

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<IDamageable>(out var damageable))
                damageable.TakeDamage();
            
            Return(this);
        }
    }
}