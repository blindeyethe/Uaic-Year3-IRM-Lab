using System;
using System.Collections;
using UnityEngine;

namespace IRM
{
    public class UnitHealth : MonoBehaviour, IDamageable
    {
        private const int DAMAGE = 2;
        private static readonly WaitForSeconds WAIT = new(0.2f);

        public static event Action<int> OnDamage; 
        
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private ParticleSystem hitParticles;
        
        private int _currentHealth;
        private SkinnedMeshRenderer _skinnedMeshRenderer;

        private void Awake()
        {
            _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            _currentHealth  = maxHealth;
        }

        public void TakeDamage()
        {
            _currentHealth -= DAMAGE;
            OnDamage?.Invoke(_currentHealth);
            
            hitParticles.Play();
            
            ChangeColor(Color.red);
            StartCoroutine(ChangeColorCoroutine());
        }

        private IEnumerator ChangeColorCoroutine()
        {
            yield return WAIT;
            ChangeColor(Color.white);
        }

        private void ChangeColor(Color color)
        {
            var materials = _skinnedMeshRenderer.materials;
                
            foreach (var material in materials)
                material.color = color;
        }
    }
}