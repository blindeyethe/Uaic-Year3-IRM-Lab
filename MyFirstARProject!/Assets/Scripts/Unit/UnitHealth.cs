using System;
using System.Collections;
using UnityEngine;

namespace IRM
{
    public class UnitHealth : MonoBehaviour, IDamageable
    {
        private const int DAMAGE = 2;
        
        private static readonly WaitForSeconds Wait = new(0.2f);
        private static readonly int IsDead = Animator.StringToHash("IsDead");

        public event Action<int> OnDamage; 
        
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private ParticleSystem hitParticles;
        
        private int _currentHealth;
        private int _defaultLayer;

        private Animator _animator;
        private SkinnedMeshRenderer _skinnedMeshRenderer;

        private UnitController _controller;
        
        public int MaxHealth => maxHealth;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _controller = GetComponent<UnitController>();
            
            _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            
            _currentHealth  = maxHealth;
            _defaultLayer = gameObject.layer;
        }

        public void TakeDamage()
        {
            _currentHealth -= DAMAGE;
            OnDamage?.Invoke(_currentHealth);
            
            hitParticles.Play();

            if (_currentHealth <= 0)
            {
                Die();
                return;
            }
            
            ChangeColor(Color.red);
            StartCoroutine(ChangeColorCoroutine());
        }

        private void Die()
        {
            _animator.SetBool(IsDead, true);
            gameObject.layer = 0;
            _controller.enabled = false;
        }

        private IEnumerator ChangeColorCoroutine()
        {
            yield return Wait;
            ChangeColor(Color.white);
        }

        private void ChangeColor(Color color)
        {
            var materials = _skinnedMeshRenderer.materials;
                
            foreach (var material in materials)
                material.color = color;
        }

        public void Reset()
        {
            if (_currentHealth > 0)
                return;
            
            _controller.enabled = true;
            
            gameObject.layer = _defaultLayer;
            _currentHealth = maxHealth;
         
            _animator.SetBool(IsDead, false);
            OnDamage?.Invoke(_currentHealth);
        }
    }
}