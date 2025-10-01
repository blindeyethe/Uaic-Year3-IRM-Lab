using System;
using System.Collections;
using UnityEngine;
using TheBlindEye.ObjectPoolSystem;

namespace IRM
{
    internal sealed class Bullet : PoolObject<Bullet>
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float lifeTime = 2f;
        
        private Transform _transform;
        
        private float _elapsedTime;
        private Vector3 _direction;

        private void Awake() =>
            _transform = transform;

        public void Initialize(Vector3 direction)
        {
            _elapsedTime = 0f;
            _direction = direction;
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= lifeTime)
            {
                Pool.Release(this);
                return;
            }
            
            _transform.position += _direction * (speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IDamageable>(out var damageable))
                damageable.TakeDamage();
            
            Pool.Release(this);
        }
    }
}