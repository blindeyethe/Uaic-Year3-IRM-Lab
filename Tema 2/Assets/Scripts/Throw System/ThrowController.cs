using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace IRM
{
    public class ThrowController : MonoBehaviour
    {
        public static event Action<float, float> OnHoldThrow;
        public static event Action OnStopHoldThrow;
        
        [SerializeField] private InputActionReference throwAction;
        
        [SerializeField] private float maxHoldDuration = 5f;
        [SerializeField] private float throwForce = 10f;

        private float _heldTime;
        private bool _isHolding, _hasItem;
        
        private XRDirectInteractor _interactor;
        private IXRSelectInteractable _interactable;

        private void Awake() =>
            _interactor = GetComponentInChildren<XRDirectInteractor>();

        private void OnEnable()
        {
            _interactor.selectEntered.AddListener(OnSelectEnter);
            _interactor.selectExited.AddListener(OnSelectExit);
            
            throwAction.action.performed += OnThrowPerform;
        }

        private void OnDisable()
        {
            _interactor.selectEntered.RemoveListener(OnSelectEnter);
            _interactor.selectExited.RemoveListener(OnSelectExit);
            
            throwAction.action.performed -= OnThrowPerform;
        }

        private void Update()
        {
            if (!_isHolding || !_hasItem)
                return;

            _heldTime += Time.deltaTime;
            OnHoldThrow?.Invoke(maxHoldDuration, _heldTime);
        }

        private void Throw()
        {
            _interactor.interactionManager.SelectExit(_interactor, _interactable);
            
            var throwPercent = _heldTime / maxHoldDuration;
            var interactableTransform = _interactable.transform;
            
            var interactableRigidBody = interactableTransform.GetComponent<Rigidbody>();
            
            var velocity = _interactor.transform.forward * (throwForce * throwPercent);
            interactableRigidBody.AddForce(velocity, ForceMode.Impulse);
            
            if(interactableTransform.TryGetComponent(out Dart dart))
                dart.OnThrow();
        }

        private void OnSelectEnter(SelectEnterEventArgs eventArgs)
        {
            _interactable = eventArgs.interactableObject;
            _hasItem = true;
        }

        private void OnSelectExit(SelectExitEventArgs _)
        {
            _hasItem = false;
            OnStopHoldThrow?.Invoke();
        }

        private void OnThrowPerform(InputAction.CallbackContext context)
        {
            if (!_hasItem)
                return;

            _isHolding = context.ReadValueAsButton();
            if (_isHolding) _heldTime = 0f;
            else Throw();
        }
    }
}