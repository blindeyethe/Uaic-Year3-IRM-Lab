using UnityEngine;

namespace IRM
{
    internal sealed class UnitController : MonoBehaviour
    {
        private const float ROTATION_SPEED = 5f;
        private static readonly int Attack = Animator.StringToHash("Attack");
        
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
            
            _attackTimer = attackCooldown;
        }
            
        private void Update()
        {
            var closestEnemy = GetClosestEnemy();
            if (closestEnemy == null)
            {
                _attackTimer = attackCooldown;
                return;
            }
            
            var targetTransform = closestEnemy.transform;
            RotateTowards(targetTransform.position);
            
            _attackTimer += Time.deltaTime;
            if (_attackTimer <= attackCooldown)
                return;
            
            _attackTimer = 0f;
            
            _animator.SetTrigger(Attack);
            _attacker.DoDamage(_transform.position, targetTransform);
        }
        
        private void RotateTowards(Vector3 targetPosition)
        {
            var direction = (targetPosition - _transform.position).normalized;
            direction.y = 0f;

            if (direction.sqrMagnitude <= 0f)
                return;

            var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, ROTATION_SPEED * Time.deltaTime);
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
        
