using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace IRM
{
    internal sealed class Dart : MonoBehaviour
    {
        [SerializeField] private ParticleSystem hitParticles;
        private Vector3 _throwPosition;

        public void OnThrow() =>
            _throwPosition = transform.position;
        
        public float GetThrowDistance() =>
            Vector3.Distance(transform.position, _throwPosition);

        public void Hit()
        {
            var dartRigidBody = GetComponent<Rigidbody>();
            dartRigidBody.linearVelocity = Vector3.zero;
            dartRigidBody.isKinematic = true;

            var dartInteractable = GetComponent<XRGrabInteractable>();
            dartInteractable.enabled = false;
            
            hitParticles.Play();
        }
    }
}