using UnityEngine;
using UnityEngine.UI;

namespace IRM.UI
{
    internal sealed class UIThrowPanel : MonoBehaviour
    {
        [SerializeField] private Image image;

        private void OnEnable()
        {
            ThrowController.OnHoldThrow += OnHoldThrow;
            ThrowController.OnStopHoldThrow += OnStopHoldThrow;
        }

        private void OnDisable()
        {
            ThrowController.OnHoldThrow -= OnHoldThrow;
            ThrowController.OnStopHoldThrow += OnStopHoldThrow;
        }

        private void OnHoldThrow(float maxDuration, float heldTime)
        {
            float value = heldTime / maxDuration;

            image.fillAmount = value;
            image.color = Color.Lerp(Color.white, Color.red, value);
        }

        private void OnStopHoldThrow() =>
            image.fillAmount = 0f;
    }
}