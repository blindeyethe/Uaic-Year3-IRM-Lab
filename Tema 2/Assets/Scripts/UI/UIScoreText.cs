using TMPro;
using UnityEngine;

namespace IRM.UI
{
    internal sealed class UIScoreText : MonoBehaviour
    {
        private TMP_Text _text;

        private void Awake() =>
            _text = GetComponent<TMP_Text>();

        private void OnEnable() =>
            DartBoard.OnScored += OnScored;
        
        private void OnDisable() =>
            DartBoard.OnScored -= OnScored;
        
        private void OnScored(int score) =>
            _text.SetText("{0}", score);
    }
}