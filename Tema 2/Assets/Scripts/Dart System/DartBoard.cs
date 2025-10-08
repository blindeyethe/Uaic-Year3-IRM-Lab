using System;
using System.Collections;
using UnityEngine;

namespace IRM
{
    internal sealed class DartBoard : MonoBehaviour
    {
        private const int MAX_POINTS = 100;
        public static event Action<int> OnScored;
        
        [SerializeField] private float perfectScoreThrowDistance = 5f;
        [SerializeField] private float perfectScoreCenterDistance = 0.5f;

        [SerializeField] private float circleDuration = 0.5f;
        [SerializeField] private float circleLifeTime = 1f;

        [SerializeField] private Transform circleTransform;

        private Transform _transform;
        private int _currentScore;

        private void Awake() =>
            _transform = transform;

        private void OnCollisionEnter(Collision other)
        {
            var otherTransform = other.transform;
            if (!otherTransform.TryGetComponent(out Dart dart))
                return;

            dart.Hit();
            
            var otherPosition = otherTransform.position;
            var hitPosition = new Vector3(circleTransform.position.x, otherPosition.y, otherPosition.z);

            _currentScore += GetScore(dart, otherPosition, hitPosition);
            OnScored?.Invoke(_currentScore);
            
            circleTransform.position = hitPosition;
            StartCoroutine(CircleCoroutine());
        }

        private int GetScore(Dart dart, Vector3 dartPosition, Vector3 hitPosition)
        {
            float throwDistance = dart.GetThrowDistance();
            float throwMultiplier = throwDistance / perfectScoreThrowDistance;
            
            float distanceFromCenter = Vector3.Distance(hitPosition, dartPosition);
            float centerMultiplier = 1f - distanceFromCenter / perfectScoreCenterDistance;
            
            return Mathf.FloorToInt(MAX_POINTS * throwMultiplier * centerMultiplier);
        }
        
        [ContextMenu("TestCircle")]
        private void TestCircle() =>
            StartCoroutine(CircleCoroutine());
        
        private IEnumerator CircleCoroutine()
        {
            float time = 0f;
            var startPosition = circleTransform.position;
            
            var boardPosition = _transform.position;
            var endPosition = new Vector3(circleTransform.position.x, boardPosition.y, boardPosition.z);
            
            circleTransform.localScale = Vector3.one * 0.1f;
            yield return new WaitForSeconds(circleDuration);
            
            while (time < circleDuration)
            {
                float delta = time / circleDuration;
                float scale = Mathf.Lerp(0f, 1f, delta);
                
                circleTransform.localScale = Vector3.one * scale;
                circleTransform.position = Vector3.Lerp(startPosition, endPosition, delta);
                
                time += Time.deltaTime;
                yield return null;
            }

            circleTransform.localScale = Vector3.one;
            circleTransform.position = endPosition;
            
            yield return new WaitForSeconds(circleLifeTime);
            circleTransform.localScale = Vector3.zero;
        }

        private void OnDrawGizmos()
        {
            var position = transform.position;
            var center = new Vector3(circleTransform.position.x, position.y, position.z);
            
            Gizmos.DrawSphere(center, perfectScoreCenterDistance);
        }
    }
}