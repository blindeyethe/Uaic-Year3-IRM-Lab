using System.Collections;
using UnityEngine;

namespace IRM
{
    public class UnitHealth : MonoBehaviour, IDamageable
    {
        private static readonly WaitForSeconds WAIT = new(0.2f);
        
        [SerializeField] private ParticleSystem hitParticles;
        private SkinnedMeshRenderer _skinnedMeshRenderer;

        private void Awake() =>
            _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        public void TakeDamage()
        {
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