using UnityEngine;

namespace IRM
{
    internal sealed class Unit : MonoBehaviour
    {
        [SerializeField] private float detectionRadius = 5f;
        [SerializeField] private float attackCooldown = 2f;
        [SerializeField] private LayerMask enemyLayerMask;
        
        private readonly Collider[] _hits = new Collider[5];
        private float _attackTimer;
        
        private Transform _transform;
        private IAttacker _attacker;
        private Animator _animator;

        private void Awake()
        {
            _transform = transform;
            _animator = GetComponent<Animator>();
            _attacker = GetComponent<IAttacker>();
            _attackTimer = 0f;
        }
            
        private void Update()
        {
            _attackTimer += Time.deltaTime;
            if (_attackTimer <= attackCooldown)
                return;
            
            _attackTimer = 0f;
            
            var closestEnemy = GetClosestEnemy();
            if (closestEnemy == null)
                return;
            
            //_animator.SetTrigger();
            _attacker.DoDamage(closestEnemy.transform);
        }

        private Collider GetClosestEnemy()
        {
            var hitCount = Physics.OverlapSphereNonAlloc(_transform.position, detectionRadius, _hits, enemyLayerMask);
            if (hitCount == 0)
                return null;
            
            Collider closestEnemy = null;
            float closestDistance = float.MaxValue;

            for (var i = 0; i < hitCount; i++)
            {
                var hitPosition = _hits[i].bounds.center;
                var directionToHit = hitPosition - _transform.position;

                var distance = directionToHit.sqrMagnitude;
                if(distance >= closestDistance) 
                    continue;
                
                closestEnemy = _hits[i];
                closestDistance = distance;
            }

            return closestEnemy;
        }
    }
}
        
