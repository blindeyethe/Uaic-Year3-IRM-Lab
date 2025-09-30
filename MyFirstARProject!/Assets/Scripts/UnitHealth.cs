using UnityEngine;

namespace IRM
{
    public class UnitHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private int maxHealth;
        private int _currentHealth;

        private void Awake() => _currentHealth = maxHealth;

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;

            if (_currentHealth <= 0)
                Destroy(gameObject);
        }
    }
}