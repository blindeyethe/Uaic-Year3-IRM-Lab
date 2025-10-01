using UnityEngine;
using UnityEngine.UI;

namespace IRM.UI
{
    internal sealed class UIHealthBar : MonoBehaviour
    {
        [SerializeField] private UnitHealth unitHealth;
        [SerializeField] private Slider slider;

        private void OnEnable() =>
            unitHealth.OnDamage += OnDamage;
        
        private void OnDisable() =>
            unitHealth.OnDamage -= OnDamage;

        private void OnDamage(int currentHealth) =>
            slider.value = (float)currentHealth / unitHealth.MaxHealth;
    }
}